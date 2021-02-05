using System;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace CloudAwesome.Xrm.Core.Tests.Stubs
{
    public class StubLogger: ILogger
    {
        private readonly LogLevel _logLevel;

        public string ResponseMessage;
        public LogLevel ResponseLogLevel;

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            ResponseLogLevel = logLevel;
            ResponseMessage = state.ToString();
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