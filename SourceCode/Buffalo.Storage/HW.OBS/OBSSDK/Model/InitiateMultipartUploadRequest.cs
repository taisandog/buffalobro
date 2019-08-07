namespace OBS.Model
{
    using System;
    using System.Runtime.CompilerServices;

    public class InitiateMultipartUploadRequest : PutObjectBasicRequest
    {
        internal override string GetAction()
        {
            return "InitiateMultipartUpload";
        }

        public int? Expires { get; set; }
    }
}

