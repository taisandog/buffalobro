namespace OBS.Model
{
    using OBS;
    using System;

    public class DeleteBucketTaggingRequest : ObsBucketWebServiceRequest
    {
        internal override string GetAction()
        {
            return "DeleteBucketTagging";
        }
    }
}

