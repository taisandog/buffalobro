namespace OBS.Model
{
    using OBS;
    using System;
    using System.Runtime.CompilerServices;

    public class ListMultipartUploadsRequest : ObsBucketWebServiceRequest
    {
        internal override string GetAction()
        {
            return "ListMultipartUploads";
        }

        public string Delimiter { get; set; }

        public string KeyMarker { get; set; }

        public int? MaxUploads { get; set; }

        public string Prefix { get; set; }

        public string UploadIdMarker { get; set; }
    }
}

