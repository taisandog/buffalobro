namespace OBS.Model
{
    using OBS;
    using System;
    using System.Runtime.CompilerServices;

    public class SetBucketStoragePolicyRequest : ObsBucketWebServiceRequest
    {
        internal override string GetAction()
        {
            return "SetBucketStoragePolicy";
        }

        public StorageClassEnum? StorageClass { get; set; }
    }
}

