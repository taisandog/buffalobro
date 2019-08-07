namespace OBS.Model
{
    using OBS;
    using System;
    using System.Runtime.CompilerServices;

    public class SetBucketWebsiteRequest : ObsBucketWebServiceRequest
    {
        internal override string GetAction()
        {
            return "SetBucketWebsite";
        }

        public WebsiteConfiguration Configuration { get; set; }
    }
}

