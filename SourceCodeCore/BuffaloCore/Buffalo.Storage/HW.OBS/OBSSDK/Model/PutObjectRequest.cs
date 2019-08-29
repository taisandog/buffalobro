namespace OBS.Model
{
    using System;
    using System.IO;
    using System.Runtime.CompilerServices;

    public class PutObjectRequest : PutObjectBasicRequest
    {
        internal override string GetAction()
        {
            return "PutObject";
        }

        public long? ContentLength { get; set; }

        public string ContentMd5 { get; set; }

        public int? Expires { get; set; }

        public string FilePath { get; set; }

        public Stream InputStream { get; set; }
    }
}

