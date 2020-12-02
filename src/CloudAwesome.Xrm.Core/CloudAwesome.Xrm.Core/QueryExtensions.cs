using System;
using System.Linq;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Query;

namespace CloudAwesome.Xrm.Core
{
    public static class QueryExtensions
    {
        public static Entity RetrieveSingleRecord(this QueryBase query, IOrganizationService organizationService)
        {
            return RetrieveRecordFromQuery(organizationService, query);
        }

        public static EntityCollection RetrieveMultiple(this QueryBase query, IOrganizationService organizationService)
        {
            return RetrieveMultipleFromQuery(organizationService, query);
        }

        public static Entity RetrieveRecordFromQuery<T>(IOrganizationService organizationService, T query, 
            bool throwExceptionOnMultipleResults = true) where T : QueryBase
        {
            var queryResults = RetrieveMultipleFromQuery(organizationService, query);
            var entitiesReturned = queryResults.Entities.Count;
            
            if (throwExceptionOnMultipleResults && entitiesReturned > 1)
            {
                throw new Exception($"Query retrieved {entitiesReturned} records. " +
                                    $"Either tighten filter criteria or pass throwExceptionOnMultipleResults = false to return the FirstOrDefault record");
            }

            return queryResults.Entities.FirstOrDefault();
        }

        public static EntityCollection RetrieveMultipleFromQuery<T>(IOrganizationService organizationService, T query) where T : QueryBase
        {
            return organizationService.RetrieveMultiple(query);
        }

        public static void DeleteSingleRecord(this QueryBase query, IOrganizationService organizationService)
        {
            var result = query.RetrieveSingleRecord(organizationService);
            result.Delete(organizationService);
        }

        public static bool DeleteAllResults(this QueryBase query, IOrganizationService organizationService,
            int maxRecordsToDelete = 50, int? expectedResultsToDelete = null)
        {
            var results = query.RetrieveMultiple(organizationService);
            var entitiesReturned = results.Entities.Count;

            if (entitiesReturned > maxRecordsToDelete)
            {
                throw new Exception($"DeleteAllResults query returned too many results to proceed. " +
                                    $"Threshold was set to {maxRecordsToDelete}");
            }

            if (expectedResultsToDelete != null && expectedResultsToDelete != entitiesReturned)
            {
                throw new Exception($"Could not safely delete results of query. " +
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

            var response = (ExecuteMultipleResponse) organizationService.Execute(request);
            return response.IsFaulted;
        }
    }
}
