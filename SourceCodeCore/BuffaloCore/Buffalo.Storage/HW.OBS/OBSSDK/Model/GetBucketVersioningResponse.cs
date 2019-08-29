namespace OBS.Model
{
    using OBS;
    using System;
    using System.Runtime.CompilerServices;

    public class GetBucketVersioningResponse : ObsWebServiceResponse
    {
        public VersioningConfiguration Configuration { get; internal set; }
    }
}

