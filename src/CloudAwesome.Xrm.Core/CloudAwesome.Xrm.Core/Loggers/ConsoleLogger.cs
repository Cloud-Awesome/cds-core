using System;
using Microsoft.Extensions.Logging;

namespace CloudAwesome.Xrm.Core.Loggers
{
    /// <summary>
    /// Implements an Console ILogger to be consumed in the TracingHelper class.
    /// Doesn't require any special inputs to construct, just logs to consuming Console application
    /// </summary>
    public class ConsoleLogger : ILogger
    {
        private readonly LogLevel _logLevel;

        /// <summary>
        /// Constructor for Console ILogger implementation
        /// </summary>
        /// <param name="logLevel">Microsoft.Extensions.Logging.LogLevel. Any traces below this level will be ignored</param>
        public ConsoleLogger(LogLevel logLevel)
        {
            _logLevel = logLevel;
        }

        /// <summary>
        /// Register a log entry in the active Console
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

            var originalBackgroundColour = Console.BackgroundColor;
            var originalForegroundColour = Console.ForegroundColor;

            switch (logLevel)
            {
                case LogLevel.Trace:
                case LogLevel.Debug:
                case LogLevel.None:
                    break;
                case LogLevel.Information:
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    break;
                case LogLevel.Warning:
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    break;
                case LogLevel.Error:
                case LogLevel.Critical:
                    Console.BackgroundColor = ConsoleColor.DarkRed;

                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(logLevel), logLevel, null);
            }

            Console.WriteLine($"{DateTime.Now}: {formatter(state, exception)}");
            Console.BackgroundColor = originalBackgroundColour;
            Console.ForegroundColor = originalForegroundColour;
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
