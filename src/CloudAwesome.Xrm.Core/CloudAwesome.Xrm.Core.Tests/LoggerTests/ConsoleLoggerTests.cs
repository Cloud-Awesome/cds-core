using System;
using System.IO;
using CloudAwesome.Xrm.Core.Loggers;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NUnit.Framework;

namespace CloudAwesome.Xrm.Core.Tests.LoggerTests
{
    [TestFixture]
    public class ConsoleLoggerTests
    {
        private readonly string _logMessage = "this is a test message";

        private TextWriter _originalConsole = new StringWriter();
        private readonly StringWriter _testConsole = new StringWriter();

        [SetUp]
        public void Init()
        {
            _originalConsole = Console.Out;
            Console.SetOut(_testConsole);
        }

        [TearDown]
        public void TearDown()
        {
            Console.SetOut(_originalConsole);
        }

        [Test]
        [TestCase(LogLevel.None)]
        [TestCase(LogLevel.Trace)]
        [TestCase(LogLevel.Debug)]
        [TestCase(LogLevel.Information)]
        [TestCase(LogLevel.Warning)]
        [TestCase(LogLevel.Error)]
        [TestCase(LogLevel.Critical)]
        public void TraceLogWritesToConsole(LogLevel logLevel)
        { 
            var logger = new ConsoleLogger(LogLevel.Trace);
            logger.Log(logLevel, _logMessage);

            _testConsole.ToString().Should().Contain(_logMessage);
        }

        [Test]
        public void OnlyTracesAboveEnableLevelAreLogged()
        {
            var logger = new ConsoleLogger(LogLevel.Information);
            logger.Log(LogLevel.Debug, $"Debug - {_logMessage}");
            logger.Log(LogLevel.Critical, $"Critical - {_logMessage}");
            
            _testConsole.ToString().Should().Contain("Critical");
            _testConsole.ToString().Should().NotContain("Debug");
        }

        [Test]
        public void BeginScopeIsNotImplemented()
        {
            var logger = new ConsoleLogger(LogLevel.Information);

            Action action = () => logger.BeginScope("tester");
            action.Should().Throw<NotImplementedException>();
        }
    }
}