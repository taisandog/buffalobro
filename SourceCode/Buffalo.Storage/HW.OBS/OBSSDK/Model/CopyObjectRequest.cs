namespace OBS.Model
{
    using System;
    using System.Runtime.CompilerServices;

    public class CopyObjectRequest : PutObjectBasicRequest
    {
        internal override string GetAction()
        {
            return "CopyObject";
        }

        public string IfMatch { get; set; }

        public DateTime? IfModifiedSince { get; set; }

        public string IfNoneMatch { get; set; }

        public DateTime? IfUnmodifiedSince { get; set; }

        public MetadataDirectiveEnum MetadataDirective { get; set; }

        public string SourceBucketName { get; set; }

        public string SourceObjectKey { get; set; }

        public SseCHeader SourceSseCHeader { get; set; }

        public string SourceVersionId { get; set; }
    }
}

