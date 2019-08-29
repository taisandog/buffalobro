namespace OBS.Model
{
    using OBS;
    using System;

    public class GetBucketLifecycleRequest : ObsBucketWebServiceRequest
    {
        internal override string GetAction()
        {
            return "GetBucketLifecycle";
        }
    }
}

