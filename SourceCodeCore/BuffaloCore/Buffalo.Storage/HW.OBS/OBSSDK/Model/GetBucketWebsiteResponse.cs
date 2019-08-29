namespace OBS.Model
{
    using OBS;
    using System;
    using System.Runtime.CompilerServices;

    public class GetBucketWebsiteResponse : ObsWebServiceResponse
    {
        public WebsiteConfiguration Configuration { get; internal set; }
    }
}

