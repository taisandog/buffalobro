namespace OBS.Model
{
    using OBS;
    using System;

    public class HeadBucketRequest : ObsBucketWebServiceRequest
    {
        internal override string GetAction()
        {
            return "HeadBucket";
        }
    }
}

