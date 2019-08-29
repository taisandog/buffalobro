namespace OBS.Model
{
    using OBS;
    using System;

    public class GetBucketWebsiteRequest : ObsBucketWebServiceRequest
    {
        internal override string GetAction()
        {
            return "GetBucketWebsite";
        }
    }
}

