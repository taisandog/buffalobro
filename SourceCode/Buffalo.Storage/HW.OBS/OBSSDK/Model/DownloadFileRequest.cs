namespace OBS.Model
{
    using System;

    public class DownloadFileRequest : GetObjectRequest
    {
        private string checkpointFile;
        private string downloadFile;
        private bool enableCheckpoint;
        private long partSize;

        public DownloadFileRequest()
        {
            this.partSize = 0x19000L;
        }

        public DownloadFileRequest(string bucketName, string objectKey)
        {
            this.partSize = 0x19000L;
            this.BucketName = bucketName;
            base.ObjectKey = objectKey;
        }

        public DownloadFileRequest(string bucketName, string objectKey, string downloadFile) : this(bucketName, objectKey)
        {
            this.downloadFile = downloadFile;
        }

        public DownloadFileRequest(string bucketName, string objectKey, string downloadFile, long partSize) : this(bucketName, objectKey)
        {
            this.downloadFile = downloadFile;
            this.partSize = partSize;
        }

        public DownloadFileRequest(string bucketName, string objectKey, string downloadFile, long partSize, bool enableCheckpoint) : this(bucketName, objectKey, downloadFile, partSize, enableCheckpoint, null)
        {
        }

        public DownloadFileRequest(string bucketName, string objectKey, string downloadFile, long partSize, bool enableCheckpoint, string checkpointFile) : this(bucketName, objectKey)
        {
            this.partSize = partSize;
            this.downloadFile = downloadFile;
            this.enableCheckpoint = enableCheckpoint;
            this.checkpointFile = checkpointFile;
        }

        public DownloadFileRequest(string bucketName, string objectKey, string downloadFile, long partSize, bool enableCheckpoint, string checkpointFile, string versionId) : this(bucketName, objectKey)
        {
            this.partSize = partSize;
            this.downloadFile = downloadFile;
            this.enableCheckpoint = enableCheckpoint;
            this.checkpointFile = checkpointFile;
            base.VersionId = versionId;
        }

        internal override string GetAction()
        {
            return "DownloadFile";
        }

        public string GetTempDownloadFile()
        {
            return (this.DownloadFile + ".tmp");
        }

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

        public string DownloadFile
        {
            get
            {
                return (this.downloadFile ?? base.ObjectKey);
            }
            set
            {
                this.downloadFile = value;
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

        public long PartSize
        {
            get
            {
                return this.partSize;
            }
            set
            {
                this.partSize = value;
            }
        }
    }
}

