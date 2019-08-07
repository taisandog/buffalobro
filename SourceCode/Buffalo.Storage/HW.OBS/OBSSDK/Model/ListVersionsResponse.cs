namespace OBS.Model
{
    using OBS;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class ListVersionsResponse : ObsWebServiceResponse
    {
        private IList<string> commonPrefixes;
        private IList<ObsObjectVersion> versions;

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

        public string Location { get; internal set; }

        public int? MaxKeys { get; internal set; }

        public string NextKeyMarker { get; internal set; }

        public string NextVersionIdMarker { get; internal set; }

        public string Prefix { get; internal set; }

        public string VersionIdMarker { get; internal set; }

        public IList<ObsObjectVersion> Versions
        {
            get
            {
                return (this.versions ?? (this.versions = new List<ObsObjectVersion>()));
            }
            internal set
            {
                this.versions = value;
            }
        }
    }
}

