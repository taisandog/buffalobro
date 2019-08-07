namespace OBS.Model
{
    using OBS;
    using System;
    using System.Runtime.CompilerServices;

    public class GetBucketNotificationReponse : ObsWebServiceResponse
    {
        public NotificationConfiguration Configuration { get; internal set; }
    }
}

