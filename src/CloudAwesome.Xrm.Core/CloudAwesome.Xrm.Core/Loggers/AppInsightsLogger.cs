using System;
using Microsoft.Extensions.Logging;

namespace CloudAwesome.Xrm.Core.Loggers
{
    public class AppInsightsLogger : ILogger
    {
        private readonly LogLevel _logLevel;
        private readonly string _instrumentationKey;

        public AppInsightsLogger(LogLevel logLevel, string instrumentationKey)
        {
            _logLevel = logLevel;
            _instrumentationKey = instrumentationKey;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            throw new NotImplementedException();
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
