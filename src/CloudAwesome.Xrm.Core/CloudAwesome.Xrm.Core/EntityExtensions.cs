using CloudAwesome.Xrm.Core.Exceptions;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CloudAwesome.Xrm.Core
{
    /// <summary>
    /// Extensions to the Xrm SDK Entity class
    /// </summary>
    public static class EntityExtensions
    {
        /// <summary>
        /// Create new record in CDS
        /// </summary>
        /// <param name="entity">Record to create. Can be a base entity or any early bound entity inheriting from Entity</param>
        /// <param name="organizationService">IOrganization reference</param>
        /// <exception cref="OperationPreventedException">Throws when Logical Name of the record is null or empty</exception>
        /// <returns>EntityReference of the created record</returns>
        public static EntityReference Create(this Entity entity, IOrganizationService organizationService)
        {
            if (string.IsNullOrEmpty(entity.LogicalName))
            {
                throw new OperationPreventedException("Cannot create a new record if the entity's logical name is null or empty. IT is recommended to use early bound entity types if possible");
            }
            entity.Id = organizationService.Create(entity);
            return new EntityReference(entity.LogicalName, entity.Id);
        }

        /// <summary>
        /// Delete the referenced record in CRM
        /// </summary>
        /// <param name="entity">Record to delete. Can be a base entity or any early bound entity inheriting from Entity</param>
        /// <param name="organizationService">IOrganization reference</param>
        /// <exception cref="OperationPreventedException">Throws when primary GUID of the record is null</exception>
        public static void Delete(this Entity entity, IOrganizationService organizationService)
        {
            if (entity.Id == Guid.Empty || entity.Id == null)
            {
                throw new OperationPreventedException("Cannot delete a record if GUID is null");
            }
            organizationService.Delete(entity.LogicalName, entity.Id);
        }

        /// <summary>
        /// Update the reference record in CRM
        /// </summary>
        /// <param name="entity">Record to update. Can be a base entity or any early bound entity inheriting from Entity</param>
        /// <param name="organizationService">IOrganization reference</param>
        /// <exception cref="OperationPreventedException">Throws when primary GUID of the record is null</exception>
        /// <returns>EntityReference of the updated record</returns>
        public static EntityReference Update(this Entity entity, IOrganizationService organizationService)
        {
            if (entity.Id == Guid.Empty || entity.Id == null)
            {
                throw new OperationPreventedException("Cannot update a record if GUID is null");
            }
            organizationService.Update(entity);
            return entity.ToEntityReference();
        }

        /// <summary>
        /// Update existing record or create new if doesn't already exist, based on QueryBase given
        /// </summary>
        /// <param name="entity">Record to create or update</param>
        /// <param name="organizationService">IOrganization reference</param>
        /// <param name="query">QueryBase implementation to determine if the record already exists. To update, method expects a single result to be returned</param>
        /// <returns>EntityReference of updated or created record</returns>
        public static EntityReference CreateOrUpdate(this Entity entity, IOrganizationService organizationService,
            QueryBase query)
        {
            var result = query.RetrieveSingleRecord(organizationService);
            if (result == null)
            {
                entity.Id = entity.Create(organizationService).Id;
            }
            else
            {
                entity.Id = result.Id;
                entity.Update(organizationService);
            }

            return entity.ToEntityReference();
        }

        public static Guid ExecuteWorkflow(this Entity entity,
            IOrganizationService organizationService, Guid workflowId)
        {
            throw FeatureRequestException.PartiallyImplementedFeatureException(typeof(ExecuteWorkflowRequest).ToString());
        }

        /// <summary>
        /// Retrieves an entity based on the current entity's primary ID.
        /// If other query criteria are required used one of the QueryExtensions methods
        /// </summary>
        /// <param name="entity">Current record to retrieve, must have primary ID populated</param>
        /// <param name="organizationService">IOrganization reference</param>
        /// <param name="columnSet">ColumnSet to retrieve. Pass null to retrieve all columns (not recommended in most cases)</param>
        /// <exception cref="OperationPreventedException">Throws when primary GUID of the record is null</exception>
        /// <returns>Entity retrieved. Use entity.ToEntity&lt;&gt;() to parse into an early bound entity type</returns>
        public static Entity Retrieve(this Entity entity,
            IOrganizationService organizationService, ColumnSet columnSet = null)
        {
            if (Guid.Empty == entity.Id)
            {
                throw new OperationPreventedException("Cannot retrieve Entity if ID is null or empty. Try a query instead.");
            }
            return organizationService.Retrieve(entity.LogicalName, entity.Id, columnSet ?? new ColumnSet(true));
        }

        /// <summary>
        /// Creates a copy of the source entity into the target entity. Always excludes the record's GUID and can exclude other fields listed
        /// </summary>
        /// <param name="targetEntity">New entity receive cloned attribute values</param>
        /// <param name="sourceEntity">Existing record to source attribute values</param>
        /// <param name="excludeAttributesList">Optional list of attribute schema names to ignore from clone</param>
        /// <exception cref="Exception">Throws a generic exception if the source and target entities are not of the same type (based on logical name)</exception>
        /// <returns>Cloned Entity. Use entity.ToEntity&lt;&gt;() to parse into an early bound entity type as required</returns>
        public static Entity CloneFrom(this Entity targetEntity, Entity sourceEntity, List<string> excludeAttributesList = null)
        {
            if (targetEntity.LogicalName != sourceEntity.LogicalName)
            {
                throw new OperationPreventedException($"Source entity must also be '{targetEntity.LogicalName}'.");
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

            return targetEntity;
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
        }

        public static void Disassociate(this Entity entity, IOrganizationService organizationService,
            string relationshipName, EntityReferenceCollection relatedEntities)
        {
            throw FeatureRequestException.PartiallyImplementedFeatureException(typeof(DisassociateRequest).ToString());
        }

    }
}
