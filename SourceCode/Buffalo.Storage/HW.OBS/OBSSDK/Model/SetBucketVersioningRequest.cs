namespace OBS.Model
{
    using OBS;
    using System;
    using System.Runtime.CompilerServices;

    public class SetBucketVersioningRequest : ObsBucketWebServiceRequest
    {
        internal override string GetAction()
        {
            return "SetBucketVersioning";
        }

        public VersioningConfiguration Configuration { get; set; }
    }
}

