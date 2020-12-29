using System;
using System.IO;
using Microsoft.Extensions.Logging;

namespace CloudAwesome.Xrm.Core.Loggers
{
    /// <summary>
    /// Implements an basic Text File ILogger to be consumed in the TracingHelper class.
    /// Requires LogLevel and a file path to be included in manifest or configuration
    /// </summary>
    public class TextFileLogger: ILogger
    {
        private readonly LogLevel _logLevel;
        private readonly string _filePath;

        /// <summary>
        /// Constructor for AppInsights ILogger implementation
        /// </summary>
        /// <param name="logLevel">Microsoft.Extensions.Logging.LogLevel. Any traces below this level will be ignored</param>
        /// <param name="filePath">.txt filepath to output the tracelogs</param>
        public TextFileLogger(LogLevel logLevel, string filePath)
        {
            _logLevel = logLevel;
            _filePath = filePath;
        }

        /// <summary>
        /// Regsiter a log entry to text file
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

            using (var file = new StreamWriter(_filePath, true))
            {
                switch (logLevel)
                {
                    case LogLevel.Trace:
                        file.WriteLine($"{DateTime.Now}: Trace: {formatter(state, exception)}");
                        break;
                    case LogLevel.Debug:
                        file.WriteLine($"{DateTime.Now}: Debug: {formatter(state, exception)}");
                        break;
                    case LogLevel.None:
                        file.WriteLine($"{DateTime.Now}: {formatter(state, exception)}");
                        break;
                    case LogLevel.Information:
                        file.WriteLine($"{DateTime.Now}: Information: {formatter(state, exception)}");
                        break;
                    case LogLevel.Warning:
                        file.WriteLine($"{DateTime.Now}: WARNING: {formatter(state, exception)}");
                        break;
                    case LogLevel.Error:
                        file.WriteLine($"{DateTime.Now}: ** ERROR: {formatter(state, exception)}");
                        break;
                    case LogLevel.Critical:
                        file.WriteLine($"{DateTime.Now}: **** CRITICAL: {formatter(state, exception)}");
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(logLevel), logLevel, null);
                }
            }
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
