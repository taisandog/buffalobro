namespace OBS.Model
{
    using OBS;
    using System;
    using System.Runtime.CompilerServices;

    public class GetBucketStoragePolicyResponse : ObsWebServiceResponse
    {
        public StorageClassEnum? StorageClass { get; internal set; }
    }
}

