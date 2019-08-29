namespace OBS.Model
{
    using OBS;
    using System;
    using System.Runtime.CompilerServices;

    public class GetObjectAclRequest : ObsBucketWebServiceRequest
    {
        internal override string GetAction()
        {
            return "GetObjectAcl";
        }

        public string ObjectKey { get; set; }

        public string VersionId { get; set; }
    }
}

