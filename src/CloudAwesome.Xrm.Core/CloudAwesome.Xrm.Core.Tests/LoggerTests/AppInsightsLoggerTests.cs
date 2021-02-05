using System;
using CloudAwesome.Xrm.Core.Loggers;
using CloudAwesome.Xrm.Core.Tests.Stubs;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.Extensions.Logging;
using NUnit.Framework;

namespace CloudAwesome.Xrm.Core.Tests.LoggerTests
{
    [TestFixture]
    public class AppInsightsLoggerTests
    {
        private static readonly Guid InstrumentationKey = Guid.NewGuid();

        private readonly string _logMessage = "this is a test message";
        private readonly string _connString = $"InstrumentationKey={InstrumentationKey}";
        private StubTelemetryChannel _stubTelemetryChannel;

        [SetUp]
        public void Init()
        {
            _stubTelemetryChannel = new StubTelemetryChannel();
        }

        [TearDown]
        public void TearDown()
        {
            _stubTelemetryChannel = null;
        }

        [Test]
        [TestCase(LogLevel.None, SeverityLevel.Information)]
        //[TestCase(LogLevel.Trace, SeverityLevel.Information)]
        [TestCase(LogLevel.Debug, SeverityLevel.Information)]
        //[TestCase(LogLevel.Information, SeverityLevel.Information)]
        [TestCase(LogLevel.Warning, SeverityLevel.Warning)]
        [TestCase(LogLevel.Error, SeverityLevel.Error)]
        [TestCase(LogLevel.Critical, SeverityLevel.Critical)]
        public void TraceLogSendsToAppInsights(LogLevel logLevel, SeverityLevel expectedSeverity)
        {
            var logger = new AppInsightsLogger(LogLevel.Debug, _connString, _stubTelemetryChannel);
            logger.Log(logLevel, _logMessage);

            _stubTelemetryChannel.Flush();

            Assert.AreEqual(_logMessage, _stubTelemetryChannel.ResponseText);
            Assert.IsNotNull(_stubTelemetryChannel.ResponseSeverityLevel);
            Assert.AreEqual(expectedSeverity, _stubTelemetryChannel.ResponseSeverityLevel);
        }

        [Test]
        public void OnlyTracesAboveEnableLevelAreLogged()
        {
            var logger = new AppInsightsLogger(LogLevel.Warning, _connString, _stubTelemetryChannel);
            logger.Log(LogLevel.Information, _logMessage);
            logger.Log(LogLevel.Critical, _logMessage);

            _stubTelemetryChannel.Flush();

            Assert.AreEqual(_logMessage, _stubTelemetryChannel.ResponseText);
            Assert.AreEqual(SeverityLevel.Critical, _stubTelemetryChannel.ResponseSeverityLevel);

        }

        [Test]
        public void BeginScopeIsNotImplemented()
        {
            var logger = new AppInsightsLogger(LogLevel.Information, _connString, _stubTelemetryChannel);
            Assert.Throws<NotImplementedException>(() => logger.BeginScope("tester"));
        }

        [Test]
        public void StandardConstructorHappyPath()
        {
            var sut = new AppInsightsLogger(LogLevel.None, _connString);
            Assert.IsNotNull(sut);
        }

    }
}