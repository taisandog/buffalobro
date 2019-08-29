namespace OBS.Model
{
    using OBS;
    using System;
    using System.Runtime.CompilerServices;

    public class SetObjectAclRequest : ObsBucketWebServiceRequest
    {
        internal override string GetAction()
        {
            return "SetObjectAcl";
        }

        public OBS.Model.AccessControlList AccessControlList { get; set; }

        public CannedAclEnum? CannedAcl { get; set; }

        public string ObjectKey { get; set; }

        public string VersionId { get; set; }
    }
}

