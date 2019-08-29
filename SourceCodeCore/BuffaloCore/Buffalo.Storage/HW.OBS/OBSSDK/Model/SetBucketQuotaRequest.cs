namespace OBS.Model
{
    using OBS;
    using System;
    using System.Runtime.CompilerServices;

    public class SetBucketQuotaRequest : ObsBucketWebServiceRequest
    {
        internal override string GetAction()
        {
            return "SetBucketQuota";
        }

        public long StorageQuota { get; set; }
    }
}

