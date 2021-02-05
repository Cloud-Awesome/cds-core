using System;
using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;

namespace CloudAwesome.Xrm.Core.Tests.Stubs
{
    public class StubTelemetryChannel: ITelemetryChannel
    {
        public StubTelemetryChannel()
        {
            this.OnSend = telemetry =>
            {
                ResponseText += ((TraceTelemetry) telemetry).Message;

                var severityLevel = ((TraceTelemetry) telemetry).SeverityLevel;
                if (severityLevel != null)
                    ResponseSeverityLevel = severityLevel.Value;
            };
            this.OnFlush = () => { };
            this.OnDispose = () => { };
        }

        public Action<ITelemetry> OnSend { get; set; }
        public Action OnFlush { get; set; }
        public Action OnDispose { get; set; }

        public string ResponseText { get; set; }
        public SeverityLevel ResponseSeverityLevel { get; set; }

        public bool ThrowError { get; set; }
        
        public void Dispose()
        {
            this.OnDispose();
        }

        public void Send(ITelemetry item)
        {
            this.OnSend(item);
        }

        public void Flush()
        {
            this.OnFlush();
        }

        public bool? DeveloperMode { get; set; }
        public string EndpointAddress { get; set; }
    }
}
