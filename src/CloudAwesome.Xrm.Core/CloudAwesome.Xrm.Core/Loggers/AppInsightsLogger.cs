using System;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Extensions.Logging;

namespace CloudAwesome.Xrm.Core.Loggers
{
    /// <summary>
    /// Implements an Azure Application Insights ILogger to be consumed in the TracingHelper class.
    /// Requires LogLevel and AppInsights connection string to be included in manifest or configuration
    /// </summary>
    public class AppInsightsLogger : ILogger
    {
        private readonly LogLevel _logLevel;
        private readonly string _connectionString;
        private TelemetryClient _telemetryClient;
        private readonly TelemetryConfiguration _telemetryConfiguration;

        /// <summary>
        /// Constructor for AppInsights ILogger implementation
        /// </summary>
        /// <param name="logLevel">Microsoft.Extensions.Logging.LogLevel. Any traces below this level will be ignored</param>
        /// <param name="connectionString">AppInsights connection string, including instrumentation key and target client</param>
        public AppInsightsLogger(LogLevel logLevel, string connectionString)
        {
            _logLevel = logLevel;
            _connectionString = connectionString;

            _telemetryConfiguration = TelemetryConfiguration.CreateDefault();
        }

        public AppInsightsLogger(LogLevel logLevel, string connectionString, ITelemetryChannel telemetryChannel)
        {
            _logLevel = logLevel;
            _connectionString = connectionString;

            _telemetryConfiguration = new TelemetryConfiguration
            {
                TelemetryChannel = telemetryChannel
            };
        }

        /// <summary>
        /// Register a log entry in AppInsights
        /// </summary>
        /// <typeparam name="TState"></typeparam>
        /// <param name="logLevel"></param>
        /// <param name="eventId"></param>
        /// <param name="state"></param>
        /// <param name="exception"></param>
        /// <param name="formatter"></param>
        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (!IsEnabled(logLevel))
            {
                return;
            }

            if (_telemetryClient == null)
            {
                _telemetryConfiguration.ConnectionString = _connectionString;
                _telemetryClient = new TelemetryClient(_telemetryConfiguration);
            }

            switch (logLevel)
            {
                case LogLevel.None:
                case LogLevel.Trace:
                case LogLevel.Debug:
                case LogLevel.Information:
                    _telemetryClient.TrackTrace($"{formatter(state, exception)}", SeverityLevel.Information);
                    break;
                case LogLevel.Warning:
                    _telemetryClient.TrackTrace($"{formatter(state, exception)}", SeverityLevel.Warning);
                    break;
                case LogLevel.Error:
                    // TODO - Extend to track exception?
                    _telemetryClient.TrackTrace($"{formatter(state, exception)}", SeverityLevel.Error);
                    break;
                case LogLevel.Critical:
                    _telemetryClient.TrackTrace($"{formatter(state, exception)}", SeverityLevel.Critical);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(logLevel), logLevel, null);
            }
            _telemetryClient.Flush();
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return logLevel >= _logLevel;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            throw new NotImplementedException();
        }
    }
}
