using CloudAwesome.Xrm.Core.Loggers;
using CloudAwesome.Xrm.Core.Models;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace CloudAwesome.Xrm.Core
{
    /// <summary>
    /// Consumes an ILogger implementation for logging output. If no ILogger is provided, all trace logs are ignored
    /// </summary>
    public class TracingHelper
    {
        private readonly ILogger _logger;
        private readonly Dictionary<LoggerConfigurationType, ILogger> _loggers = new Dictionary<LoggerConfigurationType, ILogger>();

        public string LoggerImplementationType => _logger.GetType().Name;

        /// <summary>
        /// Construct trace logging with one of the pre-rolled loggers, included in the manifest's configuration
        /// </summary>
        /// <param name="loggingConfiguration">Logging configuration with logger type and any required parameters for the type</param>
        public TracingHelper(LoggingConfiguration loggingConfiguration)
        {
            _loggers.Add(LoggerConfigurationType.Console, new ConsoleLogger(loggingConfiguration.LogLevelToTrace));
            _loggers.Add(LoggerConfigurationType.ApplicationInsights, new AppInsightsLogger(loggingConfiguration.LogLevelToTrace, loggingConfiguration.ApplicationInsightsConnectionString));
            _loggers.Add(LoggerConfigurationType.TextFile, new TextFileLogger(loggingConfiguration.LogLevelToTrace, loggingConfiguration.TextFileOutputPath));

            _logger = _loggers[loggingConfiguration.LoggerConfigurationType];
        }

        /// <summary>
        /// Construct trace logging with a custom ILogger implementation, not one of those provided with the .Core library
        /// </summary>
        /// <param name="logger">An ILogger implementation. If null, all logs are ignored</param>
        public TracingHelper(ILogger logger)
        {
            if (logger != null)
            {
                _logger = logger;
            }
        }
        
        /// <summary>
        /// Used to silently throw away any logs
        /// If no traces/logs are required for whatever reason in the consuming application,
        /// construct an empty TracingHelper where one is required 
        /// </summary>
        public TracingHelper() { }

        /// <summary>
        /// Register a log entry
        /// </summary>
        /// <param name="logLevel"></param>
        /// <param name="message"></param>
        public void Log(LogLevel logLevel, string message)
        {
            _logger?.Log(logLevel, message);
        }

        /// <summary>
        /// Register a log entry with LogLevel of Debug
        /// </summary>
        /// <param name="message"></param>
        public void Debug(string message)
        {
            Log(LogLevel.Debug, message);
        }

        /// <summary>
        /// Register a log entry with LogLevel of Information
        /// </summary>
        /// <param name="message"></param>
        public void Info(string message)
        {
            Log(LogLevel.Information, message);
        }

        /// <summary>
        /// Register a log entry with LogLevel of Critical
        /// </summary>
        /// <param name="message"></param>
        public void Critical(string message)
        {
            Log(LogLevel.Critical, message);
        }

        /// <summary>
        /// Register a log entry with LogLevel of Error
        /// </summary>
        /// <param name="message"></param>
        public void Error(string message)
        {
            Log(LogLevel.Error, message);
        }

        /// <summary>
        /// Register a log entry with LogLevel of Trace
        /// </summary>
        /// <param name="message"></param>
        public void Trace(string message)
        {
            Log(LogLevel.Trace, message);
        }

        /// <summary>
        /// Register a log entry with LogLevel of Warning
        /// </summary>
        /// <param name="message"></param>
        public void Warning(string message)
        {
            Log(LogLevel.Warning, message);
        }

    }
}
