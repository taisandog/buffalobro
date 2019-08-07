namespace OBS.Model
{
    using OBS;
    using System;

    public class DeleteBucketCorsRequest : ObsBucketWebServiceRequest
    {
        internal override string GetAction()
        {
            return "DeleteBucketCors";
        }
    }
}

