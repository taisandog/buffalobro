namespace OBS.Internal
{
    using OBS;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    internal class HttpContext
    {
        private static readonly List<HttpResponseHandler> _handlers = new List<HttpResponseHandler>();

        public HttpContext(OBS.Internal.SecurityProvider sp, OBS.ObsConfig obsConfig)
        {
            this.SecurityProvider = sp;
            this.ObsConfig = obsConfig;
        }

        public IList<HttpResponseHandler> Handlers
        {
            get
            {
                return _handlers;
            }
        }

        public OBS.ObsConfig ObsConfig { get; set; }

        public string RedirectLocation { get; set; }

        public OBS.Internal.SecurityProvider SecurityProvider { get; set; }
    }
}

