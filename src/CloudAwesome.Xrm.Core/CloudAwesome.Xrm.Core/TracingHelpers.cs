using System;

using Microsoft.Xrm.Sdk;

namespace CloudAwesome.Xrm.Core
{
    public class TracingHelpers: ITracingService
    {
        // TODO - Set info, warning, error, verbose, etc...
        // TODO - Feed in either ITracing service, AppInsights, or both...


        public TracingHelpers(ITracingService tracingService)
        {
            throw new NotImplementedException();
        }

        public void Trace(string format, params object[] args)
        {
            throw new NotImplementedException();
        }
    }
}
