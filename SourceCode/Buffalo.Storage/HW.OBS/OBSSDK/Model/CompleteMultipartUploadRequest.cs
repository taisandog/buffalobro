namespace OBS.Model
{
    using OBS;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class CompleteMultipartUploadRequest : ObsBucketWebServiceRequest
    {
        private IList<PartETag> partETags;

        public void AddPartETags(params CopyPartResponse[] responses)
        {
            foreach (CopyPartResponse response in responses)
            {
                this.PartETags.Add(new PartETag(response.PartNumber, response.ETag));
            }
        }

        public void AddPartETags(params PartETag[] partETags)
        {
            foreach (PartETag tag in partETags)
            {
                this.PartETags.Add(tag);
            }
        }

        public void AddPartETags(params UploadPartResponse[] responses)
        {
            foreach (UploadPartResponse response in responses)
            {
                this.PartETags.Add(new PartETag(response.PartNumber, response.ETag));
            }
        }

        public void AddPartETags(IEnumerable<CopyPartResponse> responses)
        {
            foreach (CopyPartResponse response in responses)
            {
                this.PartETags.Add(new PartETag(response.PartNumber, response.ETag));
            }
        }

        public void AddPartETags(IEnumerable<PartETag> partETags)
        {
            foreach (PartETag tag in partETags)
            {
                this.PartETags.Add(tag);
            }
        }

        public void AddPartETags(IEnumerable<UploadPartResponse> responses)
        {
            foreach (UploadPartResponse response in responses)
            {
                this.PartETags.Add(new PartETag(response.PartNumber, response.ETag));
            }
        }

        internal override string GetAction()
        {
            return "CompleteMultipartUpload";
        }

        public string ObjectKey { get; set; }

        public IList<PartETag> PartETags
        {
            get
            {
                return (this.partETags ?? (this.partETags = new List<PartETag>()));
            }
            set
            {
                this.partETags = value;
            }
        }

        public string UploadId { get; set; }
    }
}

