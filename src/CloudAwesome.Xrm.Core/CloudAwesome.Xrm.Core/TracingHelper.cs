using System;

using Microsoft.Xrm.Sdk;

namespace CloudAwesome.Xrm.Core
{
    public class TracingHelper: ITracingService
    {
        // Set info, warning, error, verbose, etc...
        // Feed in either ITracing service, AppInsights, or both...


        public TracingHelper(ITracingService tracingService)
        {
            throw new NotImplementedException();
        }

        public void Trace(string format, params object[] args)
        {
            throw new NotImplementedException();
        }
    }
}
