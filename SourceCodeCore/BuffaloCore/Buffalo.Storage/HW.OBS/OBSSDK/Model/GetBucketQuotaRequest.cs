namespace OBS.Model
{
    using OBS;
    using System;

    public class GetBucketQuotaRequest : ObsBucketWebServiceRequest
    {
        internal override string GetAction()
        {
            return "GetBucketQuota";
        }
    }
}

