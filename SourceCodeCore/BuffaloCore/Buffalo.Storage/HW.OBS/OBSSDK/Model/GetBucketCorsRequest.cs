namespace OBS.Model
{
    using OBS;
    using System;

    public class GetBucketCorsRequest : ObsBucketWebServiceRequest
    {
        internal override string GetAction()
        {
            return "GetBucketCors";
        }
    }
}

