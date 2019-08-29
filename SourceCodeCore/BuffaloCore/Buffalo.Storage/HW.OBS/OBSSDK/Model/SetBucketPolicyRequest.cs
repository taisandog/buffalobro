namespace OBS.Model
{
    using OBS;
    using System;
    using System.Runtime.CompilerServices;

    public class SetBucketPolicyRequest : ObsBucketWebServiceRequest
    {
        internal override string GetAction()
        {
            return "SetBucketPolicy";
        }

        public string ContentMD5 { get; set; }

        public string Policy { get; set; }
    }
}

