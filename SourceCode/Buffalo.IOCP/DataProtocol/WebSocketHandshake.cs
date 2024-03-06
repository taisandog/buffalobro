using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Buffalo.IOCP.DataProtocol
{
    public class WebSocketHandshake:IDisposable
    {
        /// <summary>
        /// 地址
        /// </summary>
        private string _url;
        /// <summary>
        /// 地址
        /// </summary>
        public string Url 
        {
            get { return _url; }
        }
        /// <summary>
        /// 请求参数
        /// </summary>
        private Dictionary<string, string> _dicParam = null;
        /// <summary>
        /// 请求参数
        /// </summary>
        public Dictionary<string, string> Param
        {
            get { return _dicParam; }
        }

        private bool _isSuccess;
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool IsSuccess
        {
            get { return _isSuccess; }
            internal set { _isSuccess = value; }
        }

        private Dictionary<string, string> _dicHandshakeContent = null;
        /// <summary>
        /// 其他握手内容
        /// </summary>
        public Dictionary<string, string> HandshakeContent
        {
            get { return _dicHandshakeContent; }
        }

        private List<string> _otherMessage;
        /// <summary>
        /// 其他信息
        /// </summary>
        public List<string> OtherMessages 
        {
            get { return _otherMessage; }
        }

        /// <summary>
        /// 握手信息
        /// </summary>
        /// <param name="content"></param>
        public WebSocketHandshake(byte[] content,int start,int count) 
        {
            _dicParam=new Dictionary<string, string>(StringComparer.CurrentCultureIgnoreCase);
            _dicHandshakeContent=new Dictionary<string, string>(StringComparer.CurrentCultureIgnoreCase);
            _otherMessage=new List<string>();
            using (MemoryStream ms = new MemoryStream(content, start, count))
            {
                using (StreamReader reader = new StreamReader(ms, Encoding.UTF8))
                {
                    string line = null;
                    while ((line = reader.ReadLine()) != null)
                    {
                        if (string.IsNullOrWhiteSpace(line)) 
                        {
                            continue;
                        }
                        line = line.Trim();
                        if(line.StartsWith("GET ", StringComparison.CurrentCultureIgnoreCase)) 
                        {
                            string prm = line.Substring(4);
                            
                            _url = GetUrlInfo(prm, _dicParam);

                        }
                        else 
                        {
                            FillContentInfo(line, _dicHandshakeContent, _otherMessage);
                        }
                    }
                }
            }
        }

        private static void FillContentInfo(string line, Dictionary<string, string> dicHandshakeContent,List<string> otherMessage) 
        {
            if (string.IsNullOrWhiteSpace(line)) 
            {
                return;
            }
            if (line.StartsWith("==>")) 
            {
                otherMessage.Add(line);
                return;
            }

            int index = line.IndexOf(':');
            if (index < 0)
            {
                otherMessage.Add(line);
                return;
            }


            string key = line.Substring(0, index);
            if (string.IsNullOrEmpty(key)) 
            {
                return;
            }
            key= key.Trim();
            string value ="";
            if (index <= line.Length - 2)
            {
                value = line.Substring(index + 1);
            }
            value = value.Trim();
            dicHandshakeContent[key] = value;
        }

        /// <summary>
        /// 获取url信息
        /// </summary>
        /// <param name="line"></param>
        /// <param name="dicParam"></param>
        /// <returns></returns>
        private static string GetUrlInfo(string line, Dictionary<string, string>  dicParam) 
        {
            line = line.Trim();
            int index = line.IndexOf(' ');
            if (index > 0)
            {
                line = line.Substring(0, index);
            }

            index = line.IndexOf('?');
            if (index < 0) 
            {
                return line;
            }
            
            string url= line.Substring(0, index);
            if (index > line.Length - 2) 
            {
                return url;
            }


            string strParam= line.Substring(index+1);
            string[] sparams = strParam.Split('&');
            string key = "";
            string value = "";
            string[] keyvaluepair = null;
            foreach (string sparam in sparams)
            {
                

                keyvaluepair = sparam.Split('=');

                key = keyvaluepair[0];
                if (string.IsNullOrWhiteSpace(key)) 
                {
                    
                    continue;
                }
                key = key.Trim();
                key = System.Web.HttpUtility.UrlDecode(key);
                
                if (keyvaluepair.Length > 1)
                {
                    value = keyvaluepair[1];
                    if (!string.IsNullOrWhiteSpace(value))
                    {
                        value= value.Trim();
                        value = System.Web.HttpUtility.UrlDecode(value);
                    }
                }
                else 
                {
                    value = "";
                }
                dicParam[key] = value;

            }
            return url;
        }

        public void Dispose()
        {
            _dicHandshakeContent = null;
            _dicParam = null;
            _url = null;
            _otherMessage = null;
        }
    }
}
