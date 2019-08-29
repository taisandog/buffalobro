namespace OBS.Model
{
    using OBS;
    using System;
    using System.Runtime.CompilerServices;

    public class ListObjectsRequest : ObsBucketWebServiceRequest
    {
        internal override string GetAction()
        {
            return "ListObjectsRequest";
        }

        public string Delimiter { get; set; }

        public string Marker { get; set; }

        public int? MaxKeys { get; set; }

        public string Prefix { get; set; }
    }
}

