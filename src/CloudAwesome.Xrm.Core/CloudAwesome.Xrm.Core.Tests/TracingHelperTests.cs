using System;
using CloudAwesome.Xrm.Core.Loggers;
using CloudAwesome.Xrm.Core.Models;
using CloudAwesome.Xrm.Core.Tests.Stubs;
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

            Assert.AreEqual(nameof(ConsoleLogger), t.LoggerImplementationType);
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

            Assert.AreEqual(nameof(ConsoleLogger), t.LoggerImplementationType);
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

            Assert.AreEqual(_logMessage, loggerStub.ResponseMessage);
            Assert.AreEqual(logLevel, loggerStub.ResponseLogLevel);
        }

        [Test]
        public void TestTrace()
        {
            var loggerStub = new StubLogger();
            var t = new TracingHelper(loggerStub);

            t.Trace(_logMessage);

            Assert.AreEqual(_logMessage, loggerStub.ResponseMessage);
            Assert.AreEqual(LogLevel.Trace, loggerStub.ResponseLogLevel);
        }

        [Test]
        public void TestDebug()
        {
            var loggerStub = new StubLogger();
            var t = new TracingHelper(loggerStub);

            t.Debug(_logMessage);

            Assert.AreEqual(_logMessage, loggerStub.ResponseMessage);
            Assert.AreEqual(LogLevel.Debug, loggerStub.ResponseLogLevel);
        }

        [Test]
        public void TestCritical()
        {
            var loggerStub = new StubLogger();
            var t = new TracingHelper(loggerStub);

            t.Critical(_logMessage);

            Assert.AreEqual(_logMessage, loggerStub.ResponseMessage);
            Assert.AreEqual(LogLevel.Critical, loggerStub.ResponseLogLevel);
        }

        [Test]
        public void TestError()
        {
            var loggerStub = new StubLogger();
            var t = new TracingHelper(loggerStub);

            t.Error(_logMessage);

            Assert.AreEqual(_logMessage, loggerStub.ResponseMessage);
            Assert.AreEqual(LogLevel.Error, loggerStub.ResponseLogLevel);
        }

        [Test]
        public void TestWarning()
        {
            var loggerStub = new StubLogger();
            var t = new TracingHelper(loggerStub);

            t.Warning(_logMessage);

            Assert.AreEqual(_logMessage, loggerStub.ResponseMessage);
            Assert.AreEqual(LogLevel.Warning, loggerStub.ResponseLogLevel);
        }

        [Test]
        public void TestInfo()
        {
            var loggerStub = new StubLogger();
            var t = new TracingHelper(loggerStub);

            t.Info(_logMessage);

            Assert.AreEqual(_logMessage, loggerStub.ResponseMessage);
            Assert.AreEqual(LogLevel.Information, loggerStub.ResponseLogLevel);
        }

    }
}
