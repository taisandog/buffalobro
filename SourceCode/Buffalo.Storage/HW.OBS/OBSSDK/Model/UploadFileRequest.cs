namespace OBS.Model
{
    using OBS;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class UploadFileRequest : ObsBucketWebServiceRequest
    {
        private string checkpointFile;
        private bool enableCheckpoint;
        private bool enableCheckSum;
        private IDictionary<string, string> headers;
        private MetadataCollection metadataCollection;
        private long partSize;
        private string uploadFile;

        public UploadFileRequest()
        {
            this.partSize = 0x500000L;
        }

        public UploadFileRequest(string bucketName, string objectKey)
        {
            this.partSize = 0x500000L;
            this.BucketName = bucketName;
            this.Key = objectKey;
        }

        public UploadFileRequest(string uploadFile, string bucketName, string objectKey) : this(bucketName, objectKey)
        {
            this.uploadFile = uploadFile;
        }

        public UploadFileRequest(string bucketName, string objectKey, string uploadFile, long partSize) : this(uploadFile, bucketName, objectKey)
        {
            this.partSize = partSize;
        }

        public UploadFileRequest(string bucketName, string objectKey, string uploadFile, long partSize, int taskNum, bool enableCheckpoint) : this(bucketName, objectKey, uploadFile, partSize, taskNum, enableCheckpoint, null)
        {
        }

        public UploadFileRequest(string bucketName, string objectKey, string uploadFile, long partSize, int taskNum, bool enableCheckpoint, string checkpointFile) : this(bucketName, objectKey)
        {
            this.partSize = partSize;
            this.uploadFile = uploadFile;
            this.enableCheckpoint = enableCheckpoint;
            this.checkpointFile = checkpointFile;
        }

        public UploadFileRequest(string bucketName, string objectKey, string uploadFile, long partSize, int taskNum, bool enableCheckpoint, string checkpointFile, bool enableCheckSum) : this(bucketName, objectKey, uploadFile, partSize, taskNum, enableCheckpoint, checkpointFile)
        {
            this.enableCheckSum = enableCheckSum;
        }

        internal override string GetAction()
        {
            return "UploadFile";
        }

        public CannedAclEnum CannedACL { get; set; }

        public string CheckpointFile
        {
            get
            {
                return this.checkpointFile;
            }
            set
            {
                this.checkpointFile = value;
            }
        }

        public bool EnableCheckpoint
        {
            get
            {
                return this.enableCheckpoint;
            }
            set
            {
                this.enableCheckpoint = value;
            }
        }

        public bool EnableCheckSum
        {
            get
            {
                return this.enableCheckSum;
            }
            set
            {
                this.enableCheckSum = value;
            }
        }

        public IDictionary<string, string> Headers
        {
            get
            {
                return (this.headers ?? (this.headers = new Dictionary<string, string>()));
            }
            internal set
            {
                this.headers = value;
            }
        }

        public string Key { get; set; }

        public MetadataCollection Metadata
        {
            get
            {
                return (this.metadataCollection ?? (this.metadataCollection = new MetadataCollection()));
            }
            internal set
            {
                this.metadataCollection = value;
            }
        }

        public StorageClassEnum StorageClass { get; set; }

        public string UploadFile
        {
            get
            {
                return this.uploadFile;
            }
            set
            {
                this.uploadFile = value;
            }
        }

        public long UploadPartSize
        {
            get
            {
                return this.partSize;
            }
            set
            {
                if (this.partSize < 0x500000L)
                {
                    this.partSize = 0x500000L;
                }
                else if (this.partSize > 0x140000000L)
                {
                    this.partSize = 0x140000000L;
                }
                else
                {
                    this.partSize = value;
                }
            }
        }

        public string WebsiteRedirectLocation { get; set; }
    }
}

