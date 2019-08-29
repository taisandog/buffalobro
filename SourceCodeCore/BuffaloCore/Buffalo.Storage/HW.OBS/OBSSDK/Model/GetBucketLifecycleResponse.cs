namespace OBS.Model
{
    using OBS;
    using System;
    using System.Runtime.CompilerServices;

    public class GetBucketLifecycleResponse : ObsWebServiceResponse
    {
        public LifecycleConfiguration Configuration { get; internal set; }
    }
}

