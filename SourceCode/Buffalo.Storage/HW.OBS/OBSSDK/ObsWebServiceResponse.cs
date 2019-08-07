namespace OBS
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Runtime.CompilerServices;

    public class ObsWebServiceResponse
    {
        private IDictionary<string, string> _headers;

        public virtual long ContentLength { get; internal set; }

        public IDictionary<string, string> Headers
        {
            get
            {
                return (this._headers ?? (this._headers = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)));
            }
            internal set
            {
                this._headers = value;
            }
        }

        public string RequestId { get; internal set; }

        public HttpStatusCode StatusCode { get; internal set; }
    }
}

