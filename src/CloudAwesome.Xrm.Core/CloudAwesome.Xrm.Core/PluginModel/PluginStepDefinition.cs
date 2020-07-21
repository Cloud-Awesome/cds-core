using System;
using System.Xml.Serialization;

namespace CloudAwesome.Xrm.Core.PluginModel
{
    public class PluginStepDefinition
    {
        public PluginStepDefinition()
        {
        }

        [XmlIgnore]
        public Action<PluginContext> Action
        {
            get; set;
        }

        public string EntityType
        {
            get; set;
        }

        public int ExecutionOrder
        {
            get; set;
        }

        public string[] FilteringAttributes
        {
            get; set;
        }

        public EntityImage[] Images
        {
            get; set;
        }

        public string Message
        {
            get; set;
        }

        public PluginExecutionMode Mode
        {
            get; set;
        }

        public PluginStage Stage
        {
            get; set;
        }

        public string UnsecureConfiguration
        {
            get; set;
        }
    }
}