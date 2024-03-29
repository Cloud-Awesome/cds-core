﻿using System;
using System.IO.Abstractions.TestingHelpers;
using CloudAwesome.Xrm.Core.Loggers;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NUnit.Framework;

namespace CloudAwesome.Xrm.Core.Tests.LoggerTests
{
    [TestFixture]
    public class TextFileLoggerTests
    {
        private readonly string _logMessage = "this is a test message";
        private readonly string _filePath = "c:\\logoutput.txt";

        [Test]
        [TestCase(LogLevel.None)]
        [TestCase(LogLevel.Trace)]
        [TestCase(LogLevel.Debug)]
        [TestCase(LogLevel.Information)]
        [TestCase(LogLevel.Warning)]
        [TestCase(LogLevel.Error)]
        [TestCase(LogLevel.Critical)]
        [Description("Debug trace log is written to text file")]
        public void TraceLogWritesToTextFile(LogLevel logLevel)
        {
            var mockFileSystem = new MockFileSystem();

            var logger = new TextFileLogger(LogLevel.Trace, _filePath, mockFileSystem);
            logger.Log(logLevel, _logMessage);

            var logFile = mockFileSystem.GetFile(_filePath);
            var fileContents = logFile.TextContents.ToLower();

            fileContents.Should().Contain(logLevel == LogLevel.None
                ? _logMessage
                : $"{logLevel.ToString().ToLower()}: {_logMessage}");
        }

        [Test]
        public void OnlyTracesAboveEnableLevelAreLogged()
        {
            var mockFileSystem = new MockFileSystem();

            var logger = new TextFileLogger(LogLevel.Information, _filePath, mockFileSystem);
            logger.Log(LogLevel.Debug, _logMessage);
            logger.Log(LogLevel.Critical, _logMessage);

            var logFile = mockFileSystem.GetFile(_filePath);
            var fileContents = logFile.TextContents.ToLower();

            fileContents.Should().Contain($"{LogLevel.Critical.ToString().ToLower()}: {_logMessage}");
            fileContents.Should().NotContain($"{LogLevel.Debug.ToString().ToLower()}: {_logMessage}");
        }

        [Test]
        public void BeginScopeIsNotImplemented()
        {
            var mockFileSystem = new MockFileSystem();
            var logger = new TextFileLogger(LogLevel.Information, _filePath, mockFileSystem);

            Action action = () => logger.BeginScope("tester");
            action.Should().Throw<NotImplementedException>();
        }

        [Test]
        public void StandardConstructorHappyPath()
        {
            var sut = new TextFileLogger(LogLevel.None, "../test.txt");
            sut.Should().NotBeNull();
        }
    }
}