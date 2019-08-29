namespace OBS.Model
{
    using OBS;
    using System;
    using System.Runtime.CompilerServices;

    public class GetObjectMetadataRequest : ObsBucketWebServiceRequest
    {
        internal override string GetAction()
        {
            return "GetObjectMetadata";
        }

        public string ObjectKey { get; set; }

        public OBS.Model.SseCHeader SseCHeader { get; set; }

        public string VersionId { get; set; }
    }
}

