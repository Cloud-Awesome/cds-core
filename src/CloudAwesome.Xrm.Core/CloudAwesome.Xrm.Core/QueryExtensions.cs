using CloudAwesome.Xrm.Core.Exceptions;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Query;
using System.Linq;

namespace CloudAwesome.Xrm.Core
{
    /// <summary>
    /// Extensions and core helpers to the Xrm SDK QueryBase classes 
    /// </summary>
    public static class QueryExtensions
    {
        /// <summary>
        /// Executes a Query expecting a single record to be returned
        /// </summary>
        /// <param name="query">QueryBase implementation (QueryExpression, QueryByAttribute, FetchQuery</param>
        /// <param name="organizationService">IOrganization implementation</param>
        /// <exception cref="QueryBaseException">Throws if the query returns more than one record</exception>
        /// <returns>The single Entity record returned by the query</returns>
        public static Entity RetrieveSingleRecord(this QueryBase query, IOrganizationService organizationService)
        {
            return RetrieveRecordFromQuery(organizationService, query);
        }

        /// <summary>
        /// Executes a Query expecting 0-Many results
        /// </summary>
        /// <param name="query">QueryBase implementation (QueryExpression, QueryByAttribute, FetchQuery</param>
        /// <param name="organizationService">IOrganization implementation</param>
        /// <returns>EntityCollection of all results returned</returns>
        public static EntityCollection RetrieveMultiple(this QueryBase query, IOrganizationService organizationService)
        {
            return RetrieveMultipleFromQuery(organizationService, query);
        }

        /// <summary>
        /// Executes a Query expecting a single record to be returned
        /// </summary>
        /// <param name="query">QueryBase implementation (QueryExpression, QueryByAttribute, FetchQuery</param>
        /// <param name="organizationService">IOrganization implementation</param>
        /// <param name="throwExceptionOnMultipleResults">Defaults to true and throws an exception if more than one result is thrown. If false, the FirstOrDefault result will be returned</param>
        /// <exception cref="QueryBaseException">Throws if throwExceptionOnMultipleResults == true and the query returns more than one record</exception>
        /// <returns>The single Entity record returned by the query</returns>
        public static Entity RetrieveRecordFromQuery<T>(IOrganizationService organizationService, T query,
            bool throwExceptionOnMultipleResults = true) where T : QueryBase
        {
            var queryResults = RetrieveMultipleFromQuery(organizationService, query);
            var entitiesReturned = queryResults.Entities.Count;

            if (throwExceptionOnMultipleResults && entitiesReturned > 1)
            {
                throw new QueryBaseException($"Query retrieved {entitiesReturned} records. " +
                                    $"Either tighten filter criteria or pass throwExceptionOnMultipleResults = false to return the FirstOrDefault record");
            }

            return queryResults.Entities.FirstOrDefault();
        }

        /// <summary>
        /// Executes a Query expecting 0-Many results
        /// </summary>
        /// <param name="query">QueryBase implementation (QueryExpression, QueryByAttribute, FetchQuery</param>
        /// <param name="organizationService">IOrganization implementation</param>
        /// <returns>EntityCollection of all results returned</returns>
        public static EntityCollection RetrieveMultipleFromQuery<T>(IOrganizationService organizationService, T query) where T : QueryBase
        {
            return organizationService.RetrieveMultiple(query);
        }

        /// <summary>
        /// Deletes a single record returned from this Query
        /// </summary>
        /// <param name="query">QueryBase implementation (QueryExpression, QueryByAttribute, FetchQuery</param>
        /// <param name="organizationService">IOrganization implementation</param>
        /// /// <exception cref="QueryBaseException">Throws if the query returns more than one record</exception>
        public static void DeleteSingleRecord(this QueryBase query, IOrganizationService organizationService)
        {
            var result = query.RetrieveSingleRecord(organizationService);
            result?.Delete(organizationService);
        }

        /// <summary>
        /// Deletes all records returned by this Query. Executed in a single Bulk Delete request
        /// </summary>
        /// <param name="query">QueryBase implementation (QueryExpression, QueryByAttribute, FetchQuery</param>
        /// <param name="organizationService">IOrganization implementation</param>
        /// <param name="maxRecordsToDelete">Set max number of records to allow deletion to prevent accidental deletion of too man records. Defaults to 50</param>
        /// <param name="expectedResultsToDelete">Set the expected number of records to delete. If this exact number is not returned, no records are deleted</param>
        /// <exception cref="QueryBaseException">Throws if the number of records returned byt eh query exceeds the maxRecordsToDelete parameter (default is 50)</exception>
        /// <exception cref="OperationPreventedException">Throws if the number of records returned is not exactly the same as the expectedResultsToDelete parameter (if not null)</exception>
        /// <returns>True if all records were deleted with no errors being thrown</returns>
        public static bool DeleteAllResults(this QueryBase query, IOrganizationService organizationService,
            int maxRecordsToDelete = 50, int? expectedResultsToDelete = null)
        {
            var results = query.RetrieveMultiple(organizationService);
            var entitiesReturned = results.Entities.Count;

            if (entitiesReturned > maxRecordsToDelete)
            {
                throw new QueryBaseException($"DeleteAllResults query returned too many results to proceed. " +
                                    $"Threshold was set to {maxRecordsToDelete}");
            }

            if (expectedResultsToDelete != null && expectedResultsToDelete != entitiesReturned)
            {
                throw new OperationPreventedException($"Could not safely delete results of query. " +
                                    $"Expected {expectedResultsToDelete} but actual was {entitiesReturned}");
            }

            var request = new ExecuteMultipleRequest()
            {
                Settings = new ExecuteMultipleSettings()
                {
                    ContinueOnError = false,
                    ReturnResponses = true
                },
                Requests = new OrganizationRequestCollection()
            };
            foreach (var result in results.Entities)
            {
                var deleteRequest = new DeleteRequest()
                {
                    Target = result.ToEntityReference()
                };
                request.Requests.Add(deleteRequest);
            }

            var response = (ExecuteMultipleResponse)organizationService.Execute(request);
            return !response.IsFaulted;
        }
    }
}
