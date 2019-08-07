namespace OBS.Model
{
    using OBS;
    using System;
    using System.IO;
    using System.Runtime.CompilerServices;

    public class UploadPartRequest : ObsBucketWebServiceRequest
    {
        internal override string GetAction()
        {
            return "UploadPart";
        }

        public string ContentMd5 { get; set; }

        public string FilePath { get; set; }

        public Stream InputStream { get; set; }

        public string ObjectKey { get; set; }

        public long? Offset { get; set; }

        public int PartNumber { get; set; }

        public long? PartSize { get; set; }

        public OBS.Model.SseCHeader SseCHeader { get; set; }

        public string UploadId { get; set; }
        /// <summary>
        /// 上传流的总长度
        /// </summary>
        public long InputStreamLength { get; set; }
    }
}

