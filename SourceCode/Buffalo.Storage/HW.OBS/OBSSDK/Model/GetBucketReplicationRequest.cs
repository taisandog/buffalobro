namespace OBS.Model
{
    using OBS;
    using System;

    public class GetBucketReplicationRequest : ObsBucketWebServiceRequest
    {
        internal override string GetAction()
        {
            return "GetBucketReplication";
        }
    }
}

