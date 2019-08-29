namespace OBS.Model
{
    using OBS;
    using System;
    using System.Runtime.CompilerServices;

    public class SetBucketAclRequest : ObsBucketWebServiceRequest
    {
        internal override string GetAction()
        {
            return "SetBucketAcl";
        }

        public OBS.Model.AccessControlList AccessControlList { get; set; }

        public CannedAclEnum? CannedAcl { get; set; }
    }
}

