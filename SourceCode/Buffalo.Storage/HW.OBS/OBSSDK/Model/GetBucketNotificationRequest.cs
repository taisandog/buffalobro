namespace OBS.Model
{
    using OBS;
    using System;

    public class GetBucketNotificationRequest : ObsBucketWebServiceRequest
    {
        internal override string GetAction()
        {
            return "GetBucketNotification";
        }
    }
}

