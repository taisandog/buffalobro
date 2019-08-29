namespace OBS.Model
{
    using OBS;
    using System;

    public class GetBucketLocationRequest : ObsBucketWebServiceRequest
    {
        internal override string GetAction()
        {
            return "GetBucketLocation";
        }
    }
}

