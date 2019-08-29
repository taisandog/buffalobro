namespace OBS.Model
{
    using OBS;
    using System;
    using System.Runtime.CompilerServices;

    public class SetBucketLoggingRequest : ObsBucketWebServiceRequest
    {
        internal override string GetAction()
        {
            return "SetBucketLogging";
        }

        public LoggingConfiguration Configuration { get; set; }
    }
}

