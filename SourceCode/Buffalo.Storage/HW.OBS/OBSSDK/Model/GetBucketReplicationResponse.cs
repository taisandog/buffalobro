namespace OBS.Model
{
    using OBS;
    using System;
    using System.Runtime.CompilerServices;

    public class GetBucketReplicationResponse : ObsWebServiceResponse
    {
        public ReplicationConfiguration Configuration { get; internal set; }
    }
}

