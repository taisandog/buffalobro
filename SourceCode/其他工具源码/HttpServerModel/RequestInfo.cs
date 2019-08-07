using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Web;

namespace Buffalo.Kernel.HttpServerModel
{
    /// <summary>
    /// ������Ϣ
    /// </summary>
    public class RequestInfo
    {
        /// <summary>
        /// ������Ϣ
        /// </summary>
        /// <param name="requestContent"></param>
        internal RequestInfo(string requestContent) 
        {
            string strBuffer = null;
            using(StringReader reader=new StringReader(requestContent))
            {
                while ((strBuffer = reader.ReadLine())!=null) 
                {
                    strBuffer = strBuffer.Trim();
                    if (string.IsNullOrEmpty(strBuffer)) 
                    {
                        continue;
                    }

                    if (strBuffer.IndexOf("GET", StringComparison.CurrentCultureIgnoreCase) >= 0 || strBuffer.IndexOf("POST", StringComparison.CurrentCultureIgnoreCase) >= 0 ) 
                    {
                        string[] strItems = strBuffer.Split(' ');
                        if (strItems.Length > 0) 
                        {
                            _requestType = strItems[0];
                        }
                        if (strItems.Length > 1)
                        {
                            FillUrlInfo(strItems[1]);
                        }
                        if (strItems.Length > 2)
                        {
                            _httpVersion=strItems[2];
                        }
                        continue;
                    }

                    int index = strBuffer.IndexOf(':');
                    string key = "";
                    string value = "";
                    if (index >= 0) 
                    {
                        key = strBuffer.Substring(0, index).Trim();
                        value = strBuffer.Substring(index + 1).Trim(' ', ';');
                        _dicHeader[key.ToLower()] = value;
                    }

                    if (key.Equals("Host", StringComparison.CurrentCultureIgnoreCase)) 
                    {
                        _host = value;
                        continue;
                    }

                    if (key.Equals("Connection", StringComparison.CurrentCultureIgnoreCase))
                    {
                        _connection = value;
                        continue;
                    }

                    if (key.Equals("Cache-Control", StringComparison.CurrentCultureIgnoreCase))
                    {
                        _cacheControl = value;
                        continue;
                    }


                    if (key.Equals("User-Agent", StringComparison.CurrentCultureIgnoreCase))
                    {
                        _userAgent = value;
                        continue;
                    }

                    if (key.Equals("Accept", StringComparison.CurrentCultureIgnoreCase))
                    {
                        _accept = value;
                        continue;
                    }

                    if (key.Equals("Accept-Encoding", StringComparison.CurrentCultureIgnoreCase))
                    {
                        _acceptEncoding = value;
                        continue;
                    }

                    if (key.Equals("Accept-Language", StringComparison.CurrentCultureIgnoreCase))
                    {
                        _acceptLanguage = value;
                        continue;
                    }

                    if (key.Equals("Accept-Charset", StringComparison.CurrentCultureIgnoreCase))
                    {
                        _acceptCharset = value;
                        continue;
                    }
                    
                }
            }
        }

        private Dictionary<string, string> _dicHeader = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// ����ͷ��Ϣ
        /// </summary>
        public Dictionary<string, string> Header
        {
            get { return _dicHeader; }
        }

        /// <summary>
        /// �����ļ���
        /// </summary>
        /// <param name="request"></param>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static string GetFileName(RequestInfo request, string filename) 
        {
            string ua = request.UserAgent;
            if (ua.IndexOf("firefox", StringComparison.CurrentCultureIgnoreCase)>=0) 
            {
                return "\""+ filename+"\"";
            }
            return HttpUtility.UrlEncode(filename);
        }

        /// <summary>
        /// ���Url��Ϣ
        /// </summary>
        /// <param name="url"></param>
        private void FillUrlInfo(string url) 
        {
            int index = url.IndexOf('?');
            if (index > 0)
            {
                _path = url.Substring(0, index);
                string paramList = url.Substring(index + 1, url.Length - index - 1);

                string[] paramItems = url.Substring(index + 1, url.Length - index - 1).Split('&');
                string pName = null;
                string pValue = null;
                foreach (string pitem in paramItems)
                {
                    int pindex = pitem.IndexOf('=');

                    if (pindex > 0)
                    {
                        pName = pitem.Substring(0, pindex);
                        pValue = pitem.Substring(pindex + 1, pitem.Length - pindex - 1);
                        _requestParam[pName] = pValue;
                    }
                }
            }
            else 
            {
                _path = url;
            }
        }


        /// <summary>
        /// ��������
        /// </summary>
        private string _requestType;
        /// <summary>
        /// ��������
        /// </summary>
        public string RequestType
        {
            get { return _requestType; }
        }

        /// <summary>
        /// ����Ŀ¼
        /// </summary>
        private string _path;
        /// <summary>
        /// ����Ŀ¼
        /// </summary>
        public string Path
        {
            get { return _path; }
        }

        /// <summary>
        /// �������
        /// </summary>
        private ReuqestParamCollection _requestParam=new ReuqestParamCollection();
        /// <summary>
        /// �������
        /// </summary>
        public ReuqestParamCollection RequestParam
        {
            get { return _requestParam; }
        }

        /// <summary>
        /// HTTP�汾��
        /// </summary>
        private string _httpVersion;
        /// <summary>
        /// HTTP�汾��
        /// </summary>
        public string HttpVersion
        {
            get { return _httpVersion; }
        }

        /// <summary>
        /// ����
        /// </summary>
        private string _host;
        /// <summary>
        /// ����
        /// </summary>
        public string Host
        {
            get { return _host; }
        }

        /// <summary>
        /// ����״̬
        /// </summary>
        private string _connection;
        /// <summary>
        /// ����״̬
        /// </summary>
        public string Connection
        {
            get { return _connection; }
        }

        /// <summary>
        /// �������
        /// </summary>
        private string _cacheControl;
        /// <summary>
        /// �������
        /// </summary>
        public string CacheControl
        {
            get { return _cacheControl; }
        }

        /// <summary>
        /// UserAgent
        /// </summary>
        private string _userAgent;
        /// <summary>
        /// UserAgent
        /// </summary>
        public string UserAgent
        {
            get { return _userAgent; }
        }

        /// <summary>
        /// ��Ӧ����
        /// </summary>
        private string _accept;
        /// <summary>
        /// ��Ӧ����
        /// </summary>
        public string Accept
        {
            get { return _accept; }
        }

        /// <summary>
        /// ��Ӧ����
        /// </summary>
        private string _acceptEncoding;
        /// <summary>
        /// ��Ӧ����
        /// </summary>
        public string AcceptEncoding
        {
            get { return _acceptEncoding; }
        }

        /// <summary>
        /// ����
        /// </summary>
        private string _acceptLanguage;
        /// <summary>
        /// ����
        /// </summary>
        public string AcceptLanguage
        {
            get { return _acceptLanguage; }
        }

        /// <summary>
        /// �ַ���
        /// </summary>
        private string _acceptCharset;
        /// <summary>
        /// �ַ���
        /// </summary>
        public string AcceptCharset
        {
            get { return _acceptCharset; }
        }



    }
}
