namespace OBS.Model
{
    using OBS;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class ListPartsResponse : ObsWebServiceResponse
    {
        private IList<PartDetail> parts;

        public string BucketName { get; internal set; }

        public OBS.Model.Initiator Initiator { get; internal set; }

        public bool IsTruncated { get; internal set; }

        public int? MaxParts { get; internal set; }

        public int? NextPartNumberMarker { get; internal set; }

        public string ObjectKey { get; internal set; }

        public OBS.Model.Owner Owner { get; internal set; }

        public int? PartNumberMarker { get; internal set; }

        public IList<PartDetail> Parts
        {
            get
            {
                return (this.parts ?? (this.parts = new List<PartDetail>()));
            }
            internal set
            {
                this.parts = value;
            }
        }

        public StorageClassEnum? StorageClass { get; internal set; }

        public string UploadId { get; internal set; }
    }
}

