namespace OBS.Model
{
    using OBS;
    using System;

    public class GetBucketStoragePolicyRequest : ObsBucketWebServiceRequest
    {
        internal override string GetAction()
        {
            return "GetBucketStoragePolicy";
        }
    }
}

