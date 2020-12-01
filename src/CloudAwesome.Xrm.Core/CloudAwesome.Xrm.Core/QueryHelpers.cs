using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace CloudAwesome.Xrm.Core
{
    public static class QueryHelpers
    {
        public static Entity RetrieveRecordFromQuery<T>(IOrganizationService organizationService, T query, bool throwExceptionOnMultipleResults = true) where T : QueryBase
        {
            var queryResults = organizationService.RetrieveMultiple(query);
            var entitiesReturned = queryResults.Entities.Count;
            
            if (throwExceptionOnMultipleResults && entitiesReturned > 1)
            {
                throw new Exception($"Query retrieved {entitiesReturned} records. " +
                                    $"Either tighten filter criteria or pass throwExceptionOnMultipleResults = false to return the FirstOrDefault record");
            }

            return queryResults.Entities.FirstOrDefault();
        }
    }
}
