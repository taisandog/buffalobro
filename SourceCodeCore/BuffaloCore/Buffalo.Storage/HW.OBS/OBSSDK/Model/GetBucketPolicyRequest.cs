namespace OBS.Model
{
    using OBS;
    using System;

    public class GetBucketPolicyRequest : ObsBucketWebServiceRequest
    {
        internal override string GetAction()
        {
            return "GetBucketPolicy";
        }
    }
}

