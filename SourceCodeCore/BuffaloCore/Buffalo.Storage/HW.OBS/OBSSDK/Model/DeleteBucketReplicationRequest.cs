namespace OBS.Model
{
    using OBS;
    using System;

    public class DeleteBucketReplicationRequest : ObsBucketWebServiceRequest
    {
        internal override string GetAction()
        {
            return "DeleteBucketReplication";
        }
    }
}

