namespace OBS.Model
{
    using OBS;
    using System;

    public class GetBucketStorageInfoRequest : ObsBucketWebServiceRequest
    {
        internal override string GetAction()
        {
            return "GetBucketStorageInfo";
        }
    }
}

