namespace OBS.Model
{
    using OBS;
    using System;
    using System.Runtime.CompilerServices;

    public class SetBucketCorsRequest : ObsBucketWebServiceRequest
    {
        internal override string GetAction()
        {
            return "SetBucketCors";
        }

        public CorsConfiguration Configuration { get; set; }
    }
}

