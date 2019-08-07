namespace OBS.Model
{
    using OBS;
    using System;
    using System.Runtime.CompilerServices;

    public class CopyObjectResponse : ObsWebServiceResponse
    {
        public string ETag { get; internal set; }

        public DateTime? LastModified { get; internal set; }

        public string SourceVersionId { get; internal set; }

        public StorageClassEnum? StorageClass { get; internal set; }

        public string VersionId { get; internal set; }
    }
}

