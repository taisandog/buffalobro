namespace OBS.Model
{
    using OBS;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class ListObjectsResponse : ObsWebServiceResponse
    {
        private IList<string> commonPrefixes;
        private IList<ObsObject> contents;

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

        public string Location { get; internal set; }

        public string Marker { get; internal set; }

        public int? MaxKeys { get; internal set; }

        public string NextMarker { get; internal set; }

        public IList<ObsObject> ObsObjects
        {
            get
            {
                return (this.contents ?? (this.contents = new List<ObsObject>()));
            }
            internal set
            {
                this.contents = value;
            }
        }

        public string Prefix { get; internal set; }
    }
}

