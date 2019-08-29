namespace OBS.Model
{
    using OBS;
    using System;

    public class GetBucketAclRequest : ObsBucketWebServiceRequest
    {
        internal override string GetAction()
        {
            return "GetBucketAcl";
        }
    }
}

