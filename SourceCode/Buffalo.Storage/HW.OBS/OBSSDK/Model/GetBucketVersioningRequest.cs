namespace OBS.Model
{
    using OBS;
    using System;

    public class GetBucketVersioningRequest : ObsBucketWebServiceRequest
    {
        internal override string GetAction()
        {
            return "GetBucketVersioning";
        }
    }
}

