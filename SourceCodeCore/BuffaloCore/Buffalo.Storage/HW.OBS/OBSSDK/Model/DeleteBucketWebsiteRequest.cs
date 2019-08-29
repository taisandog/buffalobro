namespace OBS.Model
{
    using OBS;
    using System;

    public class DeleteBucketWebsiteRequest : ObsBucketWebServiceRequest
    {
        internal override string GetAction()
        {
            return "DeleteBucketWebsite";
        }
    }
}

