namespace OBS.Internal
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net;
    using System.Runtime.CompilerServices;

    internal class HttpResponse : IDisposable
    {
        private bool _disposed;
        private IDictionary<string, string> _headers;
        private HttpWebRequest _request;
        private System.Net.HttpWebResponse _response;

        public HttpResponse(System.Net.HttpWebResponse httpWebResponse)
        {
            this._response = httpWebResponse;
        }

        public HttpResponse(WebException failure, HttpWebRequest httpWebRequest)
        {
            System.Net.HttpWebResponse response = failure.Response as System.Net.HttpWebResponse;
            this.Failure = failure;
            this._response = response;
            this._request = httpWebRequest;
        }

        public void Abort()
        {
            if (this._request != null)
            {
                this._request.Abort();
            }
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Dispose(bool disposing)
        {
            if (!this._disposed && disposing)
            {
                if (this._response != null)
                {
                    this._response.Close();
                    this._response = null;
                }
                if (this._request != null)
                {
                    this._request = null;
                }
                this._disposed = true;
            }
        }

        public Stream Content
        {
            get
            {
                if (this._disposed)
                {
                    throw new ObjectDisposedException(base.GetType().Name);
                }
                if (this._response == null)
                {
                    return null;
                }
                return this._response.GetResponseStream();
            }
        }

        public Exception Failure { get; set; }

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

        public System.Net.HttpWebResponse HttpWebResponse
        {
            get
            {
                return this._response;
            }
        }

        public HttpStatusCode StatusCode
        {
            get
            {
                return this._response.StatusCode;
            }
        }
    }
}

