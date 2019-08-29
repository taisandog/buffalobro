namespace OBS.Model
{
    using OBS;
    using System;
    using System.Runtime.CompilerServices;

    public class GetBucketCorsResponse : ObsWebServiceResponse
    {
        public CorsConfiguration Configuration { get; internal set; }
    }
}

