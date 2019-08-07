namespace OBS.Model
{
    using OBS;
    using System;

    public class GetBucketTaggingRequest : ObsBucketWebServiceRequest
    {
        internal override string GetAction()
        {
            return "GetBucketTagging";
        }
    }
}

