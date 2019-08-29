namespace OBS.Model
{
    using OBS;
    using System;

    public class DeleteBucketLifecycleRequest : ObsBucketWebServiceRequest
    {
        internal override string GetAction()
        {
            return "DeleteLifecycle";
        }
    }
}

