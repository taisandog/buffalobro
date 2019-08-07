namespace OBS.Model
{
    using OBS;
    using System;
    using System.Runtime.CompilerServices;

    public class CopyPartRequest : ObsBucketWebServiceRequest
    {
        internal override string GetAction()
        {
            return "CopyPart";
        }

        public OBS.Model.ByteRange ByteRange { get; set; }

        public SseCHeader DestinationSseCHeader { get; set; }

        public string ObjectKey { get; set; }

        public int PartNumber { get; set; }

        public string SourceBucketName { get; set; }

        public string SourceObjectKey { get; set; }

        public SseCHeader SourceSseCHeader { get; set; }

        public string SourceVersionId { get; set; }

        public string UploadId { get; set; }
    }
}

