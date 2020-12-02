using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Query;

namespace CloudAwesome.Xrm.Core
{
    // TODO - * Enum.TryParse wrapper?

    public static class EntityExtensions
    {
        public static Guid Create(this Entity entity, IOrganizationService organizationService)
        {
            // TODO - tracing (and validation?)
            entity.Id = organizationService.Create(entity);
            return entity.Id;
        }

        public static void Delete(this Entity entity, IOrganizationService organizationService)
        {
            // TODO - tracing
            organizationService.Delete(entity.LogicalName, entity.Id);
        }

        public static void Update(this Entity entity, IOrganizationService organizationService)
        {
            // TODO - tracing and validation
            organizationService.Update(entity);
        }

        public static Guid CreateOrUpdate(this Entity entity, IOrganizationService organizationService,
            QueryBase query)
        {
            var result = query.RetrieveSingleRecord(organizationService);
            if (result == null)
            {
                entity.Id = entity.Create(organizationService);
            }
            else
            {
                
                entity.Id = result.Id;
                entity.Update(organizationService);
            }

            return entity.Id;
        }

        public static Guid ExecuteWorkflow(this Entity entity, 
            IOrganizationService organizationService, Guid workflowId)
        {
            throw FeatureRequestException.PartiallyImplementedFeatureException(typeof(ExecuteWorkflowRequest).ToString());

            //return ((ExecuteWorkflowResponse)organizationService.Execute(new ExecuteWorkflowRequest
            //{
            //    EntityId = entity.Id, 
            //    WorkflowId = workflowId
            //})).Id;
        }

        public static Entity Retrieve(this Entity entity,
            IOrganizationService organizationService, ColumnSet columnSet = null)
        {
            if (Guid.Empty == entity.Id)
            {
                throw new Exception("Cannot retrieve Entity if ID is null or empty. Try a query instead.");
            }
            return organizationService.Retrieve(entity.LogicalName, entity.Id, columnSet?? new ColumnSet(true));
        }

        public static void CloneFrom(this Entity targetEntity, Entity sourceEntity, List<string> excludeAttributesList = null)
        {
            if (targetEntity.LogicalName != sourceEntity.LogicalName)
            {
                throw new Exception($"Source entity must also be '{targetEntity.LogicalName}'.");
            }
            foreach (var attr in sourceEntity.Attributes)
            {
                if (attr.Value is Guid id)
                {
                    // Ignore Id Attribute
                    if (id == sourceEntity.Id) continue;
                }
                if (excludeAttributesList != null && excludeAttributesList.Contains(attr.Key)) continue;
                
                targetEntity[attr.Key] = attr.Value;
            }
        }

        public static void Associate(this Entity entity, IOrganizationService organizationService,
            string relationshipName, IEnumerable<Entity> relatedEntities)
        {
            organizationService.Associate(entity.LogicalName, entity.Id, new Relationship(relationshipName), 
                new EntityReferenceCollection(relatedEntities.Select(e => e.ToEntityReference()).ToArray())); 
        }

        public static void Associate(this Entity entity, IOrganizationService organizationService,
            string relationshipName, EntityReferenceCollection relatedEntities)
        {
            organizationService.Associate(entity.LogicalName, entity.Id, 
                new Relationship(relationshipName), relatedEntities);
        }

        public static void Disassociate(this Entity entity, IOrganizationService organizationService,
            string relationshipName, IEnumerable<Entity> relatedEntities)
        {
            throw FeatureRequestException.PartiallyImplementedFeatureException(typeof(DisassociateRequest).ToString());

            //organizationService.Disassociate(entity.LogicalName, entity.Id, new Relationship(relationshipName),
            //    new EntityReferenceCollection(relatedEntities.Select(e => e.ToEntityReference()).ToArray()));
        }

        public static void Disassociate(this Entity entity, IOrganizationService organizationService,
            string relationshipName, EntityReferenceCollection relatedEntities)
        {
            throw FeatureRequestException.PartiallyImplementedFeatureException(typeof(DisassociateRequest).ToString());

            //organizationService.Disassociate(entity.LogicalName, entity.Id,
            //    new Relationship(relationshipName), relatedEntities);
        }
        
        public static void SetState(this Entity entity, 
            IOrganizationService organization, int stateCode, int statusCode)
        {
            // Has SetState now been deprecated?
            throw FeatureRequestException.NotImplementedFeatureException(typeof(SetStateRequest).ToString());
        }
    }
}
