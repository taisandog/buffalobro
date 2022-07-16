using System;
using System.Collections.Generic;
using System.Text;
using System.Net;

namespace Buffalo.WebKernel.WebCommons.PostForms
{
    /// <summary>
    /// 请求头
    /// </summary>
    public class PostHead:Dictionary<string,string>
    {
        private PostHead(IEqualityComparer<string> comparer)
            : base(comparer)

        { 

        }

        /// <summary>
        /// 创建头
        /// </summary>
        /// <returns></returns>
        public static PostHead CreateHeader() 
        {
            PostHead head = new PostHead(StringComparer.CurrentCultureIgnoreCase);
            return head;
        }

        private static readonly string DefaultUserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:50.0) Gecko/20100101 Firefox/50.0";

        public static Encoding DefaultEncoding = Encoding.UTF8;


        private string _userAgent = DefaultUserAgent;
        /// <summary>
        /// 浏览器UA
        /// </summary>
        public string UserAgent
        {
            get { return _userAgent; }
            set { _userAgent = value; }
        }

        private Encoding _pageEncoding=DefaultEncoding;

        /// <summary>
        /// 页面编码
        /// </summary>
        public Encoding PageEncoding
        {
            get { return _pageEncoding; }
            set { _pageEncoding = value; }
        }

        private int _timeout = 5000;

        /// <summary>
        /// 超时时间(毫秒)
        /// </summary>
        public int Timeout
        {
            get { return _timeout; }
            set { _timeout = value; }
        }

        private bool _keepAlive=true;

        /// <summary>
        /// 保持活动
        /// </summary>
        public bool KeepAlive
        {
            get { return _keepAlive; }
            set { _keepAlive = value; }
        }

        private CookieContainer _cookieContainer;

        /// <summary>
        /// Cookies容器
        /// </summary>
        public CookieContainer CookieContainer
        {
            get { return _cookieContainer; }
            set { _cookieContainer = value; }
        }


        private string _contentType;

        /// <summary>
        /// 内容类型
        /// </summary>
        public string ContentType
        {
            get { return _contentType; }
            set { _contentType = value; }
        }

        private string _accept="";

        /// <summary>
        /// Accept头
        /// </summary>
        public string Accept
        {
          get { return _accept; }
          set { _accept = value; }
        }

        private string _connection;

        /// <summary>
        /// HTTP 标头的值。默认值为 null。
        /// </summary>
        public string Connection
        {
            get { return _connection; }
            set { _connection = value; }
        }

        private IWebProxy _proxy;

        /// <summary>
        /// 代理
        /// </summary>
        public IWebProxy Proxy
        {
            get { return _proxy; }
            set { _proxy = value; }
        }

        /// <summary>
        /// 填充信息
        /// </summary>
        /// <param name="request"></param>
        public void FillInfo(HttpWebRequest request)
        {
            if (_accept != null)
            {
                request.Accept = _accept;
            }
            request.UserAgent = _userAgent;
            request.Timeout = _timeout;
            request.KeepAlive = _keepAlive;
            if (_cookieContainer != null)
            {
                request.CookieContainer = _cookieContainer;
            }
            if (_proxy != null)
            {
                request.Proxy = _proxy;
            }
            request.ContentType = _contentType;

            if(_connection!=null)
            {
                request.Connection=_connection;
            }

            foreach (KeyValuePair<string, string> kvp in this)
            {
                request.Headers[kvp.Key] = kvp.Value;
            }
        }
    }
}
