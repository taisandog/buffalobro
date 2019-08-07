namespace OBS.Model
{
    using System;
    using System.Runtime.CompilerServices;

    public class ResponseHeaderOverrides
    {
        public string CacheControl { get; set; }

        public string ContentDisposition { get; set; }

        public string ContentEncoding { get; set; }

        public string ContentLanguage { get; set; }

        public string ContentType { get; set; }

        public string Expires { get; set; }
    }
}

