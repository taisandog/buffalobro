namespace OBS.Model
{
    using OBS;
    using System;
    using System.Runtime.CompilerServices;

    public class GetBucketQuotaResponse : ObsWebServiceResponse
    {
        public long StorageQuota { get; internal set; }
    }
}

