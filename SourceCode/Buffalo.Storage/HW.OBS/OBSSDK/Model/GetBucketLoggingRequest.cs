namespace OBS.Model
{
    using OBS;
    using System;

    public class GetBucketLoggingRequest : ObsBucketWebServiceRequest
    {
        internal override string GetAction()
        {
            return "GetBucketLogging";
        }
    }
}

