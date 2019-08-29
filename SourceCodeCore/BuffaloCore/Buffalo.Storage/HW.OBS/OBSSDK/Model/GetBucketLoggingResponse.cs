namespace OBS.Model
{
    using OBS;
    using System;
    using System.Runtime.CompilerServices;

    public class GetBucketLoggingResponse : ObsWebServiceResponse
    {
        public LoggingConfiguration Configuration { get; internal set; }
    }
}

