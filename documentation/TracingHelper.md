# Tracing Helper

Helper class for constructing trace logs, including optional integration with Azure Application Insights.

## TracingHelper
Pass through a custom ILogger implementation. If nothing passed through no logging is produced.

Three sample implementations are provided to save having to implement your own.

## Passing throw configuration
- Models/LoggingConfiguration
- Pass through ILogger implementation type
- Other configuration details as required

## ILogger Implementations

### ConsoleLogger
- Loggers/ConsoleLogger

Basic console app logger 

### TextFileLogger
- Loggers/TextFileLogger

Basic logging to a text file

### AppInsightsLogger
- Loggers/AppInsightsLogger

Sends telemtry to Application Insights

