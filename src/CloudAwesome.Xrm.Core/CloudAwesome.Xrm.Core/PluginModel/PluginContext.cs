using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Xrm.Sdk;

namespace CloudAwesome.Xrm.Core.PluginModel
{
    public class PluginContext: XrmContext
    {
        public PluginContext(IServiceProvider serviceProvider)
        {
            ServiceFactory = ((IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory)));
            PluginExecutionContext = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            TracingService = new TracingHelper((ITracingService)serviceProvider.GetService(typeof(ITracingService)));
            EndpointService = (IServiceEndpointNotificationService)serviceProvider.GetService(typeof(IServiceEndpointNotificationService));
            ExecutionContext = PluginExecutionContext;
        }

        private PluginContext() { }

        public IPluginExecutionContext PluginExecutionContext
        {
            get; set;
        }

        /// <summary>
        ///  The TypeName of the Plugin using this Context
        /// </summary>
        public string PluginTypeName
        {
            get; set;
        }

        #region Parameters

        private Entity targetEntity;

        private EntityReference targetEntityReference;

        /// <summary>
        /// Provides quick access to the "Target" Input Parameter as an Entity
        /// </summary>
        public Entity TargetEntity
        {
            get
            {
                if (targetEntity == null)
                {
                    switch (PluginExecutionContext.MessageName)
                    {
                        case "SetState":
                        case "SetStateDynamicEntity":
                            targetEntityReference = GetInputParameter<EntityReference>("EntityMoniker");
                            targetEntity = new Entity(targetEntityReference.LogicalName, targetEntityReference.Id);
                            targetEntity["statecode"] = GetInputParameter<OptionSetValue>("State");
                            targetEntity["statuscode"] = GetInputParameter<OptionSetValue>("Status");
                            break;

                        default:
                            targetEntity = GetInputParameter<Entity>("Target");
                            break;
                    }
                }

                return targetEntity;
            }
        }

        /// <summary>
        /// Provides quick access to the "Target" Input Parameter as an EntityReference
        /// </summary>
        public EntityReference TargetEntityReference
        {
            get
            {
                if (targetEntityReference == null)
                {
                    switch (PluginExecutionContext.MessageName)
                    {
                        case "SetState":
                        case "SetStateDynamicEntity":
                            targetEntityReference = GetInputParameter<EntityReference>("EntityMoniker");
                            break;

                        default:
                            targetEntityReference = GetInputParameter<EntityReference>("Target");
                            break;
                    }
                }

                return targetEntityReference;
            }
        }

        /// <summary>
        /// Returns a Parameter object from the PluginExecutionContext.InputParameters collection
        /// </summary>
        /// <param name="key">Parameter Key.</param>
        /// <param name="errorIfMissing">Throw exception if the parameter is not present.</param>
        public T GetInputParameter<T>(string key, bool errorIfMissing = true)
        {
            return GetParameter<T>(PluginExecutionContext.InputParameters, key, errorIfMissing);
        }

        /// <summary>
        /// Returns a Parameter object from the PluginExecutionContext.OutputParameters collection
        /// </summary>
        /// <param name="key">Parameter Key.</param>
        /// <param name="errorIfMissing">Throw exception if the parameter is not present.</param>
        public T GetOutputParameter<T>(string key, bool errorIfMissing = true)
        {
            return GetParameter<T>(PluginExecutionContext.OutputParameters, key, errorIfMissing);
        }

        /// <summary>
        /// Shortcut method to check the Plugin Execution Context Message Name
        /// </summary>
        public bool MessageIs(params string[] messageNames)
        {
            return messageNames.Contains(PluginExecutionContext.MessageName);
        }

        /// <summary>
        /// Sets the value of a Parameter object in the PluginExecutionContext.OutputParameters collection
        /// </summary>
        /// <param name="key">Parameter Key.</param>
        /// <param name="val">The new value of the parameter.</param>
        public void SetOutputParameter<T>(string key, T val)
        {
            PluginExecutionContext.OutputParameters[key] = val;
        }

        /// <summary>
        /// Sets the value of an Output Parameter
        /// </summary>
        /// <param name="key">Parameter Key.</param>
        /// <param name="value">New value of the output parameter.</param>
        public void WriteOutputParameter(string key, object value)
        {
            PluginExecutionContext.OutputParameters[key] = value;
        }

        /// <summary>
        /// Returns a Parameter object
        /// </summary>
        /// <param name="paramCollection">The collection containing the object.</param>
        /// <param name="key">Parameter Key.</param>
        /// <param name="errorIfMissing">Throw exception if the parameter is not present.</param>
        private T GetParameter<T>(ParameterCollection paramCollection, string key, bool errorIfMissing = true)
        {
            Type t = typeof(T);

            if (paramCollection == null)
            {
                throw new InvalidPluginExecutionException(string.Format("Failed to retrieve parameter of type '{0}' with key '{1}'. Parameter Collection is null.", t.FullName, key));
            }

            object result = paramCollection.ContainsKey(key) ? paramCollection[key] : null;
            if (result == null || result.GetType() != t)
            {
                if (errorIfMissing)
                {
                    string collectionItems = string.Join(", ", paramCollection.Select(q => string.Format("'{0}' ({1})", q.Key, q.Value.GetType().ToString())));
                    throw new InvalidPluginExecutionException(string.Format("Failed to retrieve parameter of type '{0}' with key '{1}'. Available keys and object types are: {2}", t.FullName, key, collectionItems));
                }
                else
                {
                    return default(T);
                }
            }
            return (T)result;
        }

        #endregion Parameters

        #region Entity Images

        private Entity postImage;

        private Entity preImage;

        /// <summary>
        ///  Provides quick access to the first Entity in the PostEntityImages collection
        /// </summary>
        public Entity PostImage
        {
            get
            {
                if (postImage == null)
                {
                    string message = PluginExecutionContext.MessageName;

                    postImage = GetFirstEntityImage(PluginExecutionContext.PostEntityImages);
                }

                return postImage;
            }
        }

        /// <summary>
        /// Provides quick access to the first Entity in the PreEntityImages collection
        /// </summary>
        public Entity PreImage
        {
            get
            {
                // Return null if Pre-Operation Create. Allows simpler code in Create/Update plugins
                if (PluginExecutionContext.MessageName == "Create" &&
                  (PluginExecutionContext.Stage == (int)PluginStage.PreOperation ||
                  PluginExecutionContext.Stage == (int)PluginStage.PreValidation))
                {
                    Trace("Attempt to access PreImage in Create plugin ignored.");
                    return null;
                }

                if (preImage == null)
                {
                    preImage = GetFirstEntityImage(PluginExecutionContext.PreEntityImages);
                }

                return preImage;
            }
        }

        private Entity GetFirstEntityImage(EntityImageCollection imageCollection)
        {
            if (!imageCollection.Any()) { throw new InvalidOperationException("Image collection is empty."); }
            return imageCollection.First().Value;
        }

        #endregion Entity Images

        #region Utility Methods

        /// <summary>
        /// Returns a collection of ancestor PluginContexts.
        /// </summary>
        public List<PluginContext> GetAncestorContexts()
        {
            List<PluginContext> result = new List<PluginContext>();
            IPluginExecutionContext context = PluginExecutionContext;
            IPluginExecutionContext ancestor = context.ParentContext;
            while (ancestor != null)
            {
                result.Add(new PluginContext
                {
                    ImpersonationUserId = ImpersonationUserId,
                    EndpointService = EndpointService,
                    ExecutionContext = ancestor,
                    OrganizationService = OrganizationService,
                    PluginExecutionContext = ancestor,
                    ServiceFactory = ServiceFactory,
                    TracingService = TracingService,
                });
                ancestor = ancestor.ParentContext;
            }
            return result;
        }

        /// <summary>
        /// Retrieve a shared variable set in another plugin
        /// </summary>
        public T GetSharedVariable<T>(string variableName, IEnumerable<PluginContext> ancestorContexts = null)
        {
            Dictionary<string, object> allVars = ancestorContexts == null ?
                ExecutionContext.SharedVariables.ToDictionary(k => k.Key, v => v.Value) :
                ExecutionContext.SharedVariables.Union(ancestorContexts.SelectMany(q => q.ExecutionContext.SharedVariables)).ToDictionary(k => k.Key, v => v.Value);

            if (!allVars.ContainsKey(variableName))
            {
                string allVariables = string.Join(",", allVars.Select(q => $"'{q.Key}' = '{q.Value}'").ToArray());
                Trace($"Shared Variable '{variableName}' not found. Currently set Shared Variables are: {allVariables}");
                return default(T);
            }

            object result = allVars[variableName];
            try
            {
                return (T)result;
            }
            catch
            {
                Trace($"Failed to return value '{result}' for Shared Variable '{variableName}'.");
                return default(T);
            }
        }

        /// <summary>
        /// Retrieve a shared variable set in another plugin
        /// </summary>
        public T GetSharedVariable<T>(string variableName, bool includeAncestorContexts)
        {
            return GetSharedVariable<T>(variableName, includeAncestorContexts ? GetAncestorContexts() : null);
        }

        #endregion Utility Methods
    }
}