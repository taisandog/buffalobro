namespace OBS.Model
{
    using OBS;
    using System;
    using System.Runtime.CompilerServices;

    public class InitiateMultipartUploadResponse : ObsWebServiceResponse
    {
        public string BucketName { get; internal set; }

        public string ObjectKey { get; internal set; }

        public string UploadId { get; internal set; }
    }
}

