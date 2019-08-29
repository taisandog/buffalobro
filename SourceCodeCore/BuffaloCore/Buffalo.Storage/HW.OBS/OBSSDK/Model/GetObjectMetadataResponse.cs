namespace OBS.Model
{
    using OBS;
    using System;
    using System.Runtime.CompilerServices;

    public class GetObjectMetadataResponse : ObsWebServiceResponse
    {
        private long _nextPosition = -1L;
        private MetadataCollection metadataCollection;

        public bool Appendable { get; internal set; }

        public string BucketName { get; internal set; }

        public override long ContentLength { get; internal set; }

        public string ContentType { get; internal set; }

        public bool DeleteMarker { get; internal set; }

        public string ETag { get; internal set; }

        public OBS.Model.ExpirationDetail ExpirationDetail { get; internal set; }

        public DateTime? LastModified { get; internal set; }

        public MetadataCollection Metadata
        {
            get
            {
                return (this.metadataCollection ?? (this.metadataCollection = new MetadataCollection()));
            }
            set
            {
                this.metadataCollection = value;
            }
        }

        public long NextPosition
        {
            get
            {
                return this._nextPosition;
            }
            internal set
            {
                this._nextPosition = value;
            }
        }

        public string ObjectKey { get; internal set; }

        public OBS.Model.RestoreStatus RestoreStatus { get; set; }

        public StorageClassEnum? StorageClass { get; internal set; }

        public string VersionId { get; internal set; }

        public string WebsiteRedirectLocation { get; internal set; }
    }
}

