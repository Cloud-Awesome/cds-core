using System.Activities;
using System.Collections.Generic;

using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Workflow;

namespace CloudAwesome.Xrm.Core
{
    public class WorkflowActivityContext : XrmContext
    {
        public WorkflowActivityContext(CodeActivityContext activityContext)
        {
            ServiceFactory = activityContext.GetExtension<IOrganizationServiceFactory>();
            ActivityContext = activityContext;
            WorkflowContext = activityContext.GetExtension<IWorkflowContext>();
            TracingService = new TracingHelper(activityContext.GetExtension<ITracingService>());
            EndpointService = activityContext.GetExtension<IServiceEndpointNotificationService>();
            ExecutionContext = WorkflowContext;
        }

        public CodeActivityContext ActivityContext
        {
            get; set;
        }

        public IWorkflowContext WorkflowContext
        {
            get; set;
        }

        public List<IWorkflowContext> GetAncestorContexts(IWorkflowContext context = null)
        {
            List<IWorkflowContext> result = new List<IWorkflowContext>();
            if (context == null)
            {
                context = WorkflowContext;
            }

            IWorkflowContext ancestor = context.ParentContext;
            while (ancestor != null)
            {
                result.Add(ancestor);
                ancestor = ancestor.ParentContext;
            }
            return result;
        }
    }
}