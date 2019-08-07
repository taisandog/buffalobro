namespace OBS.Model
{
    using OBS;
    using System;
    using System.Runtime.CompilerServices;

    public class GetBucketMetadataResponse : ObsWebServiceResponse
    {
        public string Location { get; internal set; }

        public string ObsVersion { get; internal set; }

        public StorageClassEnum? StorageClass { get; internal set; }
    }
}

