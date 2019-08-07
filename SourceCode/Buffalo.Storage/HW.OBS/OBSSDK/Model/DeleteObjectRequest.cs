namespace OBS.Model
{
    using OBS;
    using System;
    using System.Runtime.CompilerServices;

    public class DeleteObjectRequest : ObsBucketWebServiceRequest
    {
        internal override string GetAction()
        {
            return "DeleteObject";
        }

        public string ObjectKey { get; set; }

        public string VersionId { get; set; }
    }
}

