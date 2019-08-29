namespace OBS.Model
{
    using OBS;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class ListMultipartUploadsResponse : ObsWebServiceResponse
    {
        private IList<string> commonPrefixes;
        private IList<MultipartUpload> multipartUploads;

        public string BucketName { get; internal set; }

        public IList<string> CommonPrefixes
        {
            get
            {
                return (this.commonPrefixes ?? (this.commonPrefixes = new List<string>()));
            }
            internal set
            {
                this.commonPrefixes = value;
            }
        }

        public string Delimiter { get; internal set; }

        public bool IsTruncated { get; internal set; }

        public string KeyMarker { get; internal set; }

        public int? MaxUploads { get; internal set; }

        public IList<MultipartUpload> MultipartUploads
        {
            get
            {
                return (this.multipartUploads ?? (this.multipartUploads = new List<MultipartUpload>()));
            }
            internal set
            {
                this.multipartUploads = value;
            }
        }

        public string NextKeyMarker { get; internal set; }

        public string NextUploadIdMarker { get; internal set; }

        public string Prefix { get; internal set; }

        public string UploadIdMarker { get; internal set; }
    }
}

