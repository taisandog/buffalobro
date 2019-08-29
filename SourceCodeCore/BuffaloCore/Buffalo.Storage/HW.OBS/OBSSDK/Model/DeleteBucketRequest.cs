namespace OBS.Model
{
    using OBS;
    using System;

    public class DeleteBucketRequest : ObsBucketWebServiceRequest
    {
        internal override string GetAction()
        {
            return "DeleteBucket";
        }
    }
}

