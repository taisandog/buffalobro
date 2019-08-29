namespace OBS.Model
{
    using OBS;
    using System;
    using System.Runtime.CompilerServices;

    public class SetBucketReplicationRequest : ObsBucketWebServiceRequest
    {
        internal override string GetAction()
        {
            return "SetBucketReplication";
        }

        public ReplicationConfiguration Configuration { get; set; }
    }
}

