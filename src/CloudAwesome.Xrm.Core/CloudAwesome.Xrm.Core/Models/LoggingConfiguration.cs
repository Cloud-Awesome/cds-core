using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Logging;

namespace CloudAwesome.Xrm.Core.Models
{
    public enum LoggerConfigurationType { Console = 0, ApplicationInsights = 1, TextFile = 2 }

    /// <summary>
    /// Configuration for pre-rolled telemetry and logging .Core implementations.
    /// All entry points take an ILogger parameter so these implementations can be used,
    /// a custom implementation, or no implementation at all (in which case, trace logs are ignored)
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class LoggingConfiguration
    {
        /// <summary>
        /// Type of pre-rolled ILogger implementation to user. Console, AppInsights and TextFile are currently supported
        /// </summary>
        public LoggerConfigurationType LoggerConfigurationType { get; set; }

        /// <summary>
        /// Azure Application insights connection string. Required if LoggerConfigurationType == ApplicationInsights
        /// </summary>
        public string ApplicationInsightsConnectionString { get; set; }

        /// <summary>
        /// Filepath to .txt file for output. Required if LoggerConfigurationType == TextFile
        /// </summary>
        public string TextFileOutputPath { get; set; }

        /// <summary>
        /// Microsoft.Extensions.Logging.LogLevel. Any traces below this level will be ignored
        /// </summary>
        public LogLevel LogLevelToTrace { get; set; }

    }
}
