namespace OBS.Model
{
    using OBS;
    using System;
    using System.Runtime.CompilerServices;

    public class CompleteMultipartUploadResponse : ObsWebServiceResponse
    {
        public string BucketName { get; internal set; }

        public string ETag { get; internal set; }

        public string Location { get; internal set; }

        public string ObjectKey { get; internal set; }

        public string VersionId { get; internal set; }
    }
}

