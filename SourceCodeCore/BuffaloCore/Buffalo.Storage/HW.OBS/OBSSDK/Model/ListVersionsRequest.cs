namespace OBS.Model
{
    using OBS;
    using System;
    using System.Runtime.CompilerServices;

    public class ListVersionsRequest : ObsBucketWebServiceRequest
    {
        internal override string GetAction()
        {
            return "ListVersions";
        }

        public string Delimiter { get; set; }

        public string KeyMarker { get; set; }

        public int? MaxKeys { get; set; }

        public string Prefix { get; set; }

        public string VersionIdMarker { get; set; }
    }
}

