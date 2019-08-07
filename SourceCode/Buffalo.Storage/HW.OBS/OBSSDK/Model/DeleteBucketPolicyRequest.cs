namespace OBS.Model
{
    using OBS;
    using System;

    public class DeleteBucketPolicyRequest : ObsBucketWebServiceRequest
    {
        internal override string GetAction()
        {
            return "DeleteBucketPolicy";
        }
    }
}

