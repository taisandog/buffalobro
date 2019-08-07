namespace OBS
{
    using System;
    using System.Runtime.CompilerServices;

    public abstract class ObsBucketWebServiceRequest : ObsWebServiceRequest
    {
        protected ObsBucketWebServiceRequest()
        {
        }

        public virtual string BucketName { get; set; }
    }
}

