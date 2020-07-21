using System.Activities;

namespace CloudAwesome.Xrm.Core
{
    public abstract class BaseWorkflowActivity: CodeActivity
    {
        /// <summary>
        /// Override this method in custom workflow activities, which is then called by the base Execute override
        /// </summary>
        protected abstract void ExecuteWorkflowActivity(WorkflowActivityContext ctx);

        protected override void Execute(CodeActivityContext activityContext)
        {
            // Create Workflow Activity Context
            WorkflowActivityContext ctx = new WorkflowActivityContext(activityContext);

            // TODO - review what is traced
            ctx.Trace("Custom Workflow Step Instantiated: {0}", GetType().Name);
            ctx.Trace("Primary Record: {0} {1}", ctx.WorkflowContext.PrimaryEntityName, ctx.WorkflowContext.PrimaryEntityId);
            ctx.Trace("Depth: {0}", ctx.WorkflowContext.Depth);

            ExecuteWorkflowActivity(ctx);
        }
        
        /// <summary>
        /// Returns the value of an InArgument
        /// </summary>
        public T GetInputValue<T>(WorkflowActivityContext ctx, InArgument<T> input)
        {
            return input.Get<T>(ctx.ActivityContext);
        }

        /// <summary>
        /// Returns the value of an InOutArgument
        /// </summary>
        public T GetInputValue<T>(WorkflowActivityContext ctx, InOutArgument<T> input)
        {
            return input.Get<T>(ctx.ActivityContext);
        }

        /// <summary>
        /// Sets the value of an OutArgument
        /// </summary>
        public void SetOutputValue<T>(WorkflowActivityContext ctx, OutArgument<T> output, T value)
        {
            output.Set(ctx.ActivityContext, value);
        }

        /// <summary>
        /// Sets the value of an InOutArgument
        /// </summary>
        public void SetOutputValue<T>(WorkflowActivityContext ctx, InOutArgument<T> output, T value)
        {
            output.Set(ctx.ActivityContext, value);
        }

        /// <summary>
        /// Sets the value of an OutArgument to null
        /// </summary>
        public void SetOutputValueNull(WorkflowActivityContext ctx, OutArgument output)
        {
            output.Set(ctx.ActivityContext, null);
        }

        /// <summary>
        /// Sets the value of an InOutArgument to null
        /// </summary>
        public void SetOutputValueNull(WorkflowActivityContext ctx, InOutArgument output)
        {
            output.Set(ctx.ActivityContext, null);
        }

    }
}
