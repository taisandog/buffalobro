namespace OBS.Model
{
    using OBS;
    using System;
    using System.Runtime.CompilerServices;

    public class GetBucketStorageInfoResponse : ObsWebServiceResponse
    {
        public long ObjectNumber { get; internal set; }

        public long Size { get; internal set; }
    }
}

