namespace OBS
{
    using OBS.Internal;
    using System;
    using System.Runtime.CompilerServices;

    public class ObsConfig
    {
        private string _endpoint;
        private AuthTypeEnum authType;
        private int bufferSize = 0x2000;
        private int connectionLimit = 0x3e8;
        private int connectTimeout = 0xea60;
        private int maxErrorRetry = 3;
        private int maxIdleTime = 0x7530;
        private int readWriteTimeout = 0xea60;
        private int receiveBufferSize = 0x2000;

        public ObsConfig()
        {
            this.Initialize();
        }

        protected virtual void Initialize()
        {
        }

        public AuthTypeEnum AuthType
        {
            get
            {
                return this.authType;
            }
            set
            {
                this.authType = value;
            }
        }

        public int BufferSize
        {
            get
            {
                return this.bufferSize;
            }
            set
            {
                this.bufferSize = (value <= 0) ? 0x2000 : value;
            }
        }

        public int ConnectionLimit
        {
            get
            {
                return this.connectionLimit;
            }
            set
            {
                this.connectionLimit = (value <= 0) ? 0x3e8 : value;
            }
        }

        public string Endpoint
        {
            get
            {
                return this._endpoint;
            }
            set
            {
                int num;
                this._endpoint = value;
                if (string.IsNullOrEmpty(this._endpoint))
                {
                    throw new ObsException("Endpoint is null");
                }
                this._endpoint = this._endpoint.Trim();
                if (!this._endpoint.StartsWith("http://") && !this._endpoint.StartsWith("https://"))
                {
                    this._endpoint = "https://" + this._endpoint;
                }
                while ((num = this._endpoint.LastIndexOf("/")) == (this._endpoint.Length - 1))
                {
                    this._endpoint = this._endpoint.Substring(0, num);
                }
                if (CommonUtil.IsIP(this._endpoint))
                {
                    this.PathStyle = true;
                }
                else
                {
                    this.PathStyle = false;
                }
            }
        }

        public int MaxErrorRetry
        {
            get
            {
                return this.maxErrorRetry;
            }
            set
            {
                this.maxErrorRetry = (value < 0) ? 0 : value;
            }
        }

        public int MaxIdleTime
        {
            get
            {
                return this.maxIdleTime;
            }
            set
            {
                if (value <= 0)
                {
                    this.maxIdleTime = 0x7530;
                }
                else
                {
                    this.maxIdleTime = value;
                }
            }
        }

        public bool PathStyle { get; set; }

        public string ProxyDomain { get; set; }

        public string ProxyHost { get; set; }

        public string ProxyPassword { get; set; }

        public int ProxyPort { get; set; }

        public string ProxyUserName { get; set; }

        public int ReadWriteTimeout
        {
            get
            {
                return this.readWriteTimeout;
            }
            set
            {
                this.readWriteTimeout = (value <= 0) ? 0xea60 : value;
            }
        }

        public int ReceiveBufferSize
        {
            get
            {
                return this.receiveBufferSize;
            }
            set
            {
                this.receiveBufferSize = (value <= 0) ? 0x2000 : value;
            }
        }

        public int Timeout
        {
            get
            {
                return this.connectTimeout;
            }
            set
            {
                this.connectTimeout = (value <= 0) ? 0xea60 : value;
            }
        }

        public bool ValidateCertificate { get; set; }
    }
}

