namespace OBS.Model
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class CorsRule
    {
        private IList<string> allowedHeaders;
        private IList<HttpVerb> allowedMethods;
        private IList<string> allowedOrigins;
        private IList<string> exposeHeaders;

        public IList<string> AllowedHeaders
        {
            get
            {
                return (this.allowedHeaders ?? (this.allowedHeaders = new List<string>()));
            }
            set
            {
                this.allowedHeaders = value;
            }
        }

        public IList<HttpVerb> AllowedMethods
        {
            get
            {
                return (this.allowedMethods ?? (this.allowedMethods = new List<HttpVerb>()));
            }
            set
            {
                this.allowedMethods = value;
            }
        }

        public IList<string> AllowedOrigins
        {
            get
            {
                return (this.allowedOrigins ?? (this.allowedOrigins = new List<string>()));
            }
            set
            {
                this.allowedOrigins = value;
            }
        }

        public IList<string> ExposeHeaders
        {
            get
            {
                return (this.exposeHeaders ?? (this.exposeHeaders = new List<string>()));
            }
            set
            {
                this.exposeHeaders = value;
            }
        }

        public string Id { get; set; }

        public int? MaxAgeSeconds { get; set; }
    }
}

