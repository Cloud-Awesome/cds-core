using System;
using CloudAwesome.Xrm.Core.Loggers;
using CloudAwesome.Xrm.Core.Models;
using CloudAwesome.Xrm.Core.Tests.Stubs;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NUnit.Framework;

namespace CloudAwesome.Xrm.Core.Tests
{
    [TestFixture]
    public class TracingHelperTests
    {
        private static readonly Guid InstrumentationKey = Guid.NewGuid();

        private readonly string _logMessage = "this is a test message";
        private readonly StubTelemetryChannel _stubTelemetryChannel;

        [Test]
        public void CorrectLoggerImplementationIsConstructed()
        {
            var logger = new ConsoleLogger(LogLevel.None);
            var t = new TracingHelper(logger);

            t.LoggerImplementationType.Should().Be(nameof(ConsoleLogger));
        }

        [Test]
        public void CorrectLoggingImplementationStrategyIsConstructed()
        {
            var loggingConfiguration = new LoggingConfiguration()
            {
                LoggerConfigurationType = LoggerConfigurationType.Console,
                LogLevelToTrace = LogLevel.Warning
            };
            var t = new TracingHelper(loggingConfiguration);

            t.LoggerImplementationType.Should().Be(nameof(ConsoleLogger));
        }

        [Test]
        public void EmptyConstructorSilentlyDisregardsTraces()
        {
            var t = new TracingHelper();

            Action action = () => t.Log(LogLevel.Debug, _logMessage);
            action.Should().NotThrow();
        }

        [Test]
        [TestCase(LogLevel.Debug)]
        [TestCase(LogLevel.Critical)]
        [TestCase(LogLevel.Error)]
        [TestCase(LogLevel.Information)]
        [TestCase(LogLevel.None)]
        [TestCase(LogLevel.Trace)]
        [TestCase(LogLevel.Warning)]
        public void TestLog(LogLevel logLevel)
        {
            var loggerStub = new StubLogger();
            var t = new TracingHelper(loggerStub);

            t.Log(logLevel, _logMessage);

            loggerStub.ResponseMessage.Should().Be(_logMessage);
            loggerStub.ResponseLogLevel.Should().Be(logLevel);
        }

        [Test]
        public void TestTrace()
        {
            var loggerStub = new StubLogger();
            var t = new TracingHelper(loggerStub);

            t.Trace(_logMessage);

            loggerStub.ResponseMessage.Should().Be(_logMessage);
            loggerStub.ResponseLogLevel.Should().Be(LogLevel.Trace);
        }

        [Test]
        public void TestDebug()
        {
            var loggerStub = new StubLogger();
            var t = new TracingHelper(loggerStub);

            t.Debug(_logMessage);

            loggerStub.ResponseMessage.Should().Be(_logMessage);
            loggerStub.ResponseLogLevel.Should().Be(LogLevel.Debug);
        }

        [Test]
        public void TestCritical()
        {
            var loggerStub = new StubLogger();
            var t = new TracingHelper(loggerStub);

            t.Critical(_logMessage);

            loggerStub.ResponseMessage.Should().Be(_logMessage);
            loggerStub.ResponseLogLevel.Should().Be(LogLevel.Critical);
        }

        [Test]
        public void TestError()
        {
            var loggerStub = new StubLogger();
            var t = new TracingHelper(loggerStub);

            t.Error(_logMessage);

            loggerStub.ResponseMessage.Should().Be(_logMessage);
            loggerStub.ResponseLogLevel.Should().Be(LogLevel.Error);
        }

        [Test]
        public void TestWarning()
        {
            var loggerStub = new StubLogger();
            var t = new TracingHelper(loggerStub);

            t.Warning(_logMessage);

            loggerStub.ResponseMessage.Should().Be(_logMessage);
            loggerStub.ResponseLogLevel.Should().Be(LogLevel.Warning);
        }

        [Test]
        public void TestInfo()
        {
            var loggerStub = new StubLogger();
            var t = new TracingHelper(loggerStub);

            t.Info(_logMessage);

            loggerStub.ResponseMessage.Should().Be(_logMessage);
            loggerStub.ResponseLogLevel.Should().Be(LogLevel.Information);
        }

    }
}
