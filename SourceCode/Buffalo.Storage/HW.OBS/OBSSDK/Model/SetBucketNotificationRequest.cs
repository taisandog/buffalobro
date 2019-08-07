namespace OBS.Model
{
    using OBS;
    using System;
    using System.Runtime.CompilerServices;

    public class SetBucketNotificationRequest : ObsBucketWebServiceRequest
    {
        internal override string GetAction()
        {
            return "SetBucketNotification";
        }

        public NotificationConfiguration Configuration { get; set; }
    }
}

