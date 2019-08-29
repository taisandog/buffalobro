namespace OBS.Model
{
    using OBS;
    using System;
    using System.Runtime.CompilerServices;

    public class SetBucketLifecycleRequest : ObsBucketWebServiceRequest
    {
        internal override string GetAction()
        {
            return "SetBucketLifecycle";
        }

        public LifecycleConfiguration Configuration { get; set; }
    }
}

