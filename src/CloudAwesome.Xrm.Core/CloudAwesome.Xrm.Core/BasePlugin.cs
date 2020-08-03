using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Xrm.Sdk;

using CloudAwesome.Xrm.Core.PluginModel;

namespace CloudAwesome.Xrm.Core
{
    public abstract class BasePlugin : IPlugin
    {
        public BasePlugin() { }

        public BasePlugin(string secureConfig, string unsecureConfig)
        {
            this.secureConfig = secureConfig;
            this.unsecureConfig = unsecureConfig;
        }

        public List<PluginStepDefinition> RegisteredSteps
        {
            get; set;
        }

        /// <summary>
        /// Wrapper method which creates a PluginContext instance, then calls <see cref="ExecutePlugin">ExecutePlugin</see>.
        /// </summary>
        /// <param name="serviceProvider">
        /// Instance of IServiceProvider provided by Dynamics 365 when the plugin is fired.
        /// </param>
        public void Execute(IServiceProvider serviceProvider)
        {
            PluginContext ctx = new PluginContext(serviceProvider);

            // TODO - review what is traced
            ctx.Trace($"Plugin Instantiated: {GetType()}");
            ctx.Trace($"Date/Time: {DateTime.Now.ToUniversalTime()}");
            ctx.Trace($"Primary Record: {ctx.PluginExecutionContext.PrimaryEntityName} {ctx.PluginExecutionContext.PrimaryEntityId}");
            ctx.Trace($"Message: {ctx.PluginExecutionContext.MessageName}");
            ctx.Trace($"Stage: {ctx.PluginExecutionContext.Stage}");
            ctx.Trace($"Depth: {ctx.PluginExecutionContext.Depth}");
            ctx.Trace($"User ID: {ctx.PluginExecutionContext.UserId}");
            ctx.Trace($"Input Parameters: {string.Join(",", ctx.PluginExecutionContext.InputParameters.Select(q => $"{q.Key} ({q.Value})").ToArray())}");

#if DEBUG
            ExecutePlugin(ctx);
#else
            try
            {
                ExecutePlugin(ctx);
            }
            catch (InvalidPluginExecutionException pluginEx)
            {
                ctx.Trace($"**** {pluginEx.ToString()}");
                throw;
            }
            catch (Exception ex)
            {
                ctx.Trace($"**** {ex.ToString()}");
                new InvalidPluginExecutionException(ex.ToString());
            }
#endif
        }

        protected string secureConfig;
        protected string unsecureConfig;

        /// <summary>
        /// Associates an Action with a combination of Stage, Execution Mode and Message
        /// </summary>
        /// <para>
        /// TODO - document usage of all the input parameters, because they may not be too obvious ;)
        /// </para>
        protected PluginStepDefinition RegisterStep(PluginStage stage, PluginExecutionMode mode, string message, Action<PluginContext> action, params EntityImage[] images)
        {
            PluginStepDefinition step = new PluginStepDefinition { Stage = stage, Mode = mode, Message = message, Action = action, Images = images };
            RegisterStep(step);
            return step;
        }

        /// <summary>
        /// Associates an Action with a combination of Stage, Execution Mode, Message and Entity Type
        /// </summary>
        /// <para>
        /// TODO - document usage of all the input parameters, because they may not be too obvious ;)
        /// </para>
        protected PluginStepDefinition RegisterStep(PluginStage stage, PluginExecutionMode mode, string message, string entityType, Action<PluginContext> action, params EntityImage[] images)
        {
            PluginStepDefinition step = new PluginStepDefinition { Stage = stage, Mode = mode, Message = message, EntityType = entityType, Action = action, Images = images };
            RegisterStep(step);
            return step;
        }

        /// <summary>
        /// Associates an Action with a combination of Stage, Execution Mode, Message and Entity Type
        /// </summary>
        /// <para>
        /// TODO - document usage of all the input parameters, because they may not be too obvious ;)
        /// </para>
        protected void RegisterStep(PluginStepDefinition pluginStepDefinition)
        {
            if (RegisteredSteps == null)
            {
                RegisteredSteps = new List<PluginStepDefinition>();
            }

            RegisteredSteps.Add(pluginStepDefinition);
        }

        private static readonly Random random = new Random();

        /// <summary>
        /// Identifies appropriate entry in the RegisteredEvents collection and calls its Action
        /// <param name="ctx">The plugin context.</param>
        /// </summary>
        private void ExecutePlugin(PluginContext ctx)
        {
            PluginExecutionMode actualMode = ctx.ExecutionContext.IsolationMode == 2 ? PluginExecutionMode.Sandbox :
                (ctx.ExecutionContext.OperationId == Guid.Empty ? PluginExecutionMode.Synchronous : PluginExecutionMode.Asynchronous);
            PluginStage actualStage = (PluginStage)ctx.PluginExecutionContext.Stage;

            PluginStepDefinition evDef = RegisteredSteps.FirstOrDefault(q => 
                (string.IsNullOrEmpty(q.EntityType) || q.EntityType == ctx.PluginExecutionContext.PrimaryEntityName) &&
                q.Message == ctx.PluginExecutionContext.MessageName && 
                q.Stage == actualStage && (actualMode == PluginExecutionMode.Sandbox || q.Mode == actualMode));

            if (evDef == null)
            {
                // Ensure the events are in source control and not registered manually/accidentally 
                throw new InvalidPluginExecutionException(
                    $"Plugin '{GetType().Name}' has been triggered on a non-registered event: " +
                    $"{ctx.PluginExecutionContext.PrimaryEntityName} {ctx.PluginExecutionContext.MessageName} {actualStage} {actualMode}");
            }

            evDef.Action.Invoke(ctx);
        }
    }

    public enum PluginExecutionMode
    {
        Sandbox,
        Synchronous,
        Asynchronous
    }

    public enum PluginStage
    {
        PreValidation = 10,
        PreOperation = 20,
        PostOperation = 40,
    }
}
