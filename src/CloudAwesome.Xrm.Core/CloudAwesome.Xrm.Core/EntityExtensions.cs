using System;
using System.Collections.Generic;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace CloudAwesome.Xrm.Core
{
    public static class EntityExtensionsTests
    {
        public static Guid Create(this Entity entity, IOrganizationService organizationService)
        {
            // TODO - tracing
            entity.Id = organizationService.Create(entity);
            return entity.Id;
        }

        public static void Delete(this Entity entity, IOrganizationService organizationService)
        {
            // TODO - tracing
            organizationService.Delete(entity.LogicalName, entity.Id);
        }

        public static bool Update(IOrganizationService organizationService)
        {
            // 1. Create new entity with only changed attributes, so doesn't flood the audit history
            
            // 2. Validate before update
            
            // 3. Update with OrgService
            
            // 4. Cleanup and report back

            throw new NotImplementedException("TODO");
        }

        public static Guid ExecuteWorkflow(this Entity entity, 
            IOrganizationService organizationService, Guid workflowId)
        {
            // TODO - Unit test
            return ((ExecuteWorkflowResponse)organizationService.Execute(new ExecuteWorkflowRequest
            {
                EntityId = entity.Id, 
                WorkflowId = workflowId
            })).Id;
        }

        public static bool Retrieve(this Entity entity,
            IOrganizationService organizationService, ColumnSet columnSet = null)
        {
            throw new NotImplementedException("TODO");
        }

        public static void CloneFrom(this Entity entity, Entity otherEntity)
        {
            if (entity.LogicalName != otherEntity.LogicalName)
            {
                throw new Exception($"Other entity must also be '{entity.LogicalName}'.");
            }
            foreach (var attr in otherEntity.Attributes)
            {
                if (attr.Value is Guid id)
                {
                    // Ignore Id Attribute
                    if (id == otherEntity.Id) continue;
                }

                // TODO
                //SetValue(attr.Key, attr.Value);
            }
        }

        public static bool RetrieveFromQuery<T>(this Entity entity,
            IOrganizationService organizationService, T query) where T : QueryBase
        {
            throw new NotImplementedException("TODO");
        }

        public static void SetState(this Entity entity, 
            IOrganizationService organization, int stateCode, int statusCode)
        {
            throw new NotImplementedException("TODO");
        }

    }
}
