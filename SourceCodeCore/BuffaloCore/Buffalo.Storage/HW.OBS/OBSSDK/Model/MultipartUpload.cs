namespace OBS.Model
{
    using System;
    using System.Runtime.CompilerServices;

    public class MultipartUpload
    {
        public DateTime? Initiated { get; internal set; }

        public OBS.Model.Initiator Initiator { get; internal set; }

        public string ObjectKey { get; internal set; }

        public OBS.Model.Owner Owner { get; internal set; }

        public StorageClassEnum? StorageClass { get; internal set; }

        public string UploadId { get; internal set; }
    }
}

