namespace OBS.Model
{
    using OBS;
    using System;
    using System.Runtime.CompilerServices;

    public class RestoreObjectRequest : ObsBucketWebServiceRequest
    {
        internal override string GetAction()
        {
            return "RestoreObject";
        }

        public int Days { get; set; }

        public string ObjectKey { get; set; }

        public RestoreTierEnum? Tier { get; set; }

        public string VersionId { get; set; }
    }
}

