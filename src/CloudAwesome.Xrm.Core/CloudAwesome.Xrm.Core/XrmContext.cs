using System;

using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace CloudAwesome.Xrm.Core
{
    public class XrmContext
    {
        private EntityReference _defaultCurrency;

        private IOrganizationService _orgSvc;

        private IOrganizationService _orgSvcImpersonated;

        public XrmContext() { }

        public EntityReference DefaultCurrency
        {
            get
            {
                if (_defaultCurrency == null)
                {
                    _defaultCurrency = OrganizationService.Retrieve("organization", ExecutionContext.OrganizationId,
                        new ColumnSet("basecurrencyid")).GetAttributeValue<EntityReference>("basecurrencyid");
                }
                return _defaultCurrency;
            }
        }

        public IServiceEndpointNotificationService EndpointService
        {
            get; set;
        }

        public IExecutionContext ExecutionContext
        {
            get; set;
        }

        public Guid ImpersonationUserId
        {
            get; set;
        }

        public IOrganizationService OrganizationService
        {
            get
            {
                if (_orgSvc == null)
                {
                    _orgSvc = ServiceFactory.CreateOrganizationService(ExecutionContext.UserId);
                }

                return _orgSvc;
            }
            set => _orgSvc = value;
        }

        public IOrganizationService OrganizationServiceImpersonated
        {
            get
            {
                if (_orgSvcImpersonated == null)
                {
                    if (ImpersonationUserId == Guid.Empty)
                    {
                        throw new InvalidPluginExecutionException("Impersonation User Guid not set.");
                    }

                    _orgSvcImpersonated = ServiceFactory.CreateOrganizationService(ImpersonationUserId);
                }
                return _orgSvcImpersonated;
            }
        }

        public IOrganizationServiceFactory ServiceFactory
        {
            get; set;
        }

        public ITracingService TracingService
        {
            get; set;
        }

        public EntityReference GetCurrentUserReference()
        {
            return new EntityReference("systemuser", ExecutionContext.UserId);
        }

        public EntityReference GetInitiatingUserReference()
        {
            return new EntityReference("systemuser", ExecutionContext.InitiatingUserId);
        }

        public EntityReference GetPrimaryEntityReference()
        {
            return new EntityReference(ExecutionContext.PrimaryEntityName, ExecutionContext.PrimaryEntityId);
        }

        public void SetSharedVariable<T>(string variableName, T value)
        {
            Trace($"Setting Shared Variable '{variableName}' to '{value}'.");
            ExecutionContext.SharedVariables[variableName] = value;
        }

        public void Trace(string format, params object[] args)
        {
            TracingService.Trace(format, args);
        }
    }
}