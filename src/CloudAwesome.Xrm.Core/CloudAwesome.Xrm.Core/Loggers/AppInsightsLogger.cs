using System;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Extensions.Logging;

namespace CloudAwesome.Xrm.Core.Loggers
{
    public class AppInsightsLogger : ILogger
    {
        private readonly LogLevel _logLevel;
        private readonly TelemetryClient _telemetryClient;

        public AppInsightsLogger(LogLevel logLevel, string connectionString)
        {
            var telemetryConfiguration = TelemetryConfiguration.CreateDefault();
            telemetryConfiguration.ConnectionString = connectionString;

            _telemetryClient = new TelemetryClient(telemetryConfiguration);
            _logLevel = logLevel;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
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
