using Microsoft.Extensions.Logging;

namespace CloudAwesome.Xrm.Core.Models
{
    public enum LoggerConfigurationType { Console = 0, ApplicationInsights = 1, TextFile = 2 }

    public class LoggingConfiguration
    {
        public LoggerConfigurationType LoggerConfigurationType { get; set; }

        public string ApplicationInsightsConnectionString { get; set; }

        public string TextFileOutputPath { get; set; }

        public LogLevel LogLevelToTrace { get; set; }

    }
}
