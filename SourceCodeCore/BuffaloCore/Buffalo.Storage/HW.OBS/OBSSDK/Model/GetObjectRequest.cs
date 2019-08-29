namespace OBS.Model
{
    using System;
    using System.Runtime.CompilerServices;

    public class GetObjectRequest : GetObjectMetadataRequest
    {
        internal override string GetAction()
        {
            return "GetObject";
        }

        public OBS.Model.ByteRange ByteRange { get; set; }

        public string IfMatch { get; set; }

        public DateTime? IfModifiedSince { get; set; }

        public string IfNoneMatch { get; set; }

        public DateTime? IfUnmodifiedSince { get; set; }

        public string ImageProcess { get; set; }

        public OBS.Model.ResponseHeaderOverrides ResponseHeaderOverrides { get; set; }
    }
}

