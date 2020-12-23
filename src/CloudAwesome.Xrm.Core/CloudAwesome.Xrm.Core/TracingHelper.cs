using System.Collections.Generic;
using CloudAwesome.Xrm.Core.Loggers;
using CloudAwesome.Xrm.Core.Models;
using Microsoft.Extensions.Logging;

namespace CloudAwesome.Xrm.Core
{
    public class TracingHelper
    {
        private readonly ILogger _logger;
        private readonly Dictionary<LoggerConfigurationType, ILogger> _loggers = new Dictionary<LoggerConfigurationType, ILogger>();

        public TracingHelper(LoggingConfiguration loggingConfiguration)
        {
            _loggers.Add(LoggerConfigurationType.Console, new ConsoleLogger(loggingConfiguration.LogLevelToTrace));
            _loggers.Add(LoggerConfigurationType.ApplicationInsights, new AppInsightsLogger(loggingConfiguration.LogLevelToTrace, loggingConfiguration.ApplicationInsightsConnectionString));
            _loggers.Add(LoggerConfigurationType.TextFile, new TextFileLogger(loggingConfiguration.LogLevelToTrace, loggingConfiguration.TextFileOutputPath));

            _logger = _loggers[loggingConfiguration.LoggerConfigurationType];
        }

        public TracingHelper(ILogger logger)
        {
            if (logger != null)
            {
                _logger = logger;
            }
        }

        public void Log(LogLevel logLevel, string message)
        {
            _logger?.Log(logLevel, message);
        }

        public void Debug(string message)
        {
            Log(LogLevel.Debug, message);
        }

        public void Info(string message)
        {
            Log(LogLevel.Information, message);
        }

        public void Critical(string message)
        {
            Log(LogLevel.Critical, message);
        }

        public void Error(string message)
        {
            Log(LogLevel.Error, message);
        }

        public void Trace(string message)
        {
            Log(LogLevel.Trace, message);
        }

        public void Warning(string message)
        {
            Log(LogLevel.Warning, message);
        }

    }
}
