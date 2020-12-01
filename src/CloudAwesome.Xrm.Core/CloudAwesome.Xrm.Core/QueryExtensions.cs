using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;
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
    }
}
