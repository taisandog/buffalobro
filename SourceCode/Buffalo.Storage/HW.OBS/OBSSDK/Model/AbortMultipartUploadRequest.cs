namespace OBS.Model
{
    using OBS;
    using System;
    using System.Runtime.CompilerServices;

    public class AbortMultipartUploadRequest : ObsBucketWebServiceRequest
    {
        internal override string GetAction()
        {
            return "AbortMultipartUpload";
        }

        public string ObjectKey { get; set; }

        public string UploadId { get; set; }
    }
}

