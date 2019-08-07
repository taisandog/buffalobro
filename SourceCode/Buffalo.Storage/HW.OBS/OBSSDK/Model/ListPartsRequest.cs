namespace OBS.Model
{
    using OBS;
    using System;
    using System.Runtime.CompilerServices;

    public class ListPartsRequest : ObsBucketWebServiceRequest
    {
        internal override string GetAction()
        {
            return "ListParts";
        }

        public int? MaxParts { get; set; }

        public string ObjectKey { get; set; }

        public int? PartNumberMarker { get; set; }

        public string UploadId { get; set; }
    }
}

