namespace OBS.Internal
{
    using OBS.Model;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Runtime.CompilerServices;

    internal class HttpRequest : IDisposable
    {
        private bool _disposed;
        private IDictionary<string, string> _headers;
        private IDictionary<string, string> _parameters;
        private string _url;
        /// <summary>
        /// 是否需要关闭内容流
        /// </summary>
        private bool _needCloseContent=false;
        /// <summary>
        /// 是否需要关闭内容流
        /// </summary>
        public bool NeedCloseContent
        {
            get
            {
                return _needCloseContent;
            }
            set
            {
                _needCloseContent = value;
            }
        }
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this._disposed && disposing)
            {
                if (this.Content != null && _needCloseContent)
                {
                    this.Content.Close();
                }
                this.Content = null;
                this._disposed = true;
            }
        }

        public string GetHost(string endpoint)
        {
            UriBuilder builder = new UriBuilder(endpoint);
            string host = builder.Host;
            if ((builder.Port != 0x1bb) && (builder.Port != 80))
            {
                host = host + ":" + builder.Port;
            }
            if (!string.IsNullOrEmpty(this.BucketName) && !this.PathStyle)
            {
                host = this.BucketName + "." + host;
            }
            return host;
        }

        public string GetUrl()
        {
            if (string.IsNullOrEmpty(this._url))
            {
                string endpoint = this.Endpoint;
                bool flag1 = !string.IsNullOrEmpty(this.BucketName);
                if (flag1)
                {
                    if (this.PathStyle)
                    {
                        endpoint = endpoint + "/" + this.BucketName;
                    }
                    else
                    {
                        int index = endpoint.IndexOf("//");
                        string str3 = endpoint.Substring(index + 2);
                        endpoint = endpoint.Substring(0, index + 2) + this.BucketName + "." + str3;
                    }
                }
                if (flag1 && !string.IsNullOrEmpty(this.ObjectKey))
                {
                    endpoint = endpoint + "/" + CommonUtil.UrlEncode(this.ObjectKey, "utf-8", "/");
                }
                string str2 = CommonUtil.ConvertParamsToString(this.Params);
                if (!string.IsNullOrEmpty(str2))
                {
                    endpoint = endpoint + "?" + str2;
                }
                this._url = endpoint;
            }
            return this._url;
        }

        public string BucketName { get; set; }

        public virtual Stream Content { get; set; }

        public string Endpoint { get; set; }

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

        public bool IsRepeatable
        {
            get
            {
                if (this.Content != null)
                {
                    return this.Content.CanSeek;
                }
                return true;
            }
        }

        public HttpVerb Method { get; set; }

        public string ObjectKey { get; set; }

        public IDictionary<string, string> Params
        {
            get
            {
                return (this._parameters ?? (this._parameters = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)));
            }
            internal set
            {
                this._parameters = value;
            }
        }

        public bool PathStyle { get; set; }
    }
}

