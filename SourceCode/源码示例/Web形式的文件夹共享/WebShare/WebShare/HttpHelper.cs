using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;

namespace WebShare
{
    public class HttpHelper
    {
        private const string IP138URL = "http://www.ip138.com/ips1388.asp";

        private static readonly Encoding DefaultResponseEncoding = Encoding.Default;
        /// <summary>
        /// 获取互联网IP
        /// </summary>
        /// <returns></returns>
        public static string GetInternetIP() 
        {
            try
            {
                string content = GetData(IP138URL);

                if (content != null)
                {
                    string strIPTag = "您的IP地址是：[";
                    using (StringReader reader = new StringReader(content))
                    {
                        string tmp = null;
                        while ((tmp = reader.ReadLine()) != null)
                        {
                            int uidIndex = tmp.IndexOf(strIPTag);
                            if (uidIndex >= 0)
                            {
                                int star = uidIndex + strIPTag.Length;
                                int end = tmp.IndexOf("]", star + 1);
                                if (star < end)
                                {
                                    string ip = tmp.Substring(star, end - star );
                                    return ip;
                                }
                            }
                        }
                    }
                }
            }
            catch 
            {
                return null;
            }
            return null;
        }

        /// <summary>
        /// GET方式获取信息
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string GetData(string url)
        {
            HttpWebRequest webRequest2 = (HttpWebRequest)WebRequest.Create(new Uri(url));
            webRequest2.Headers.Add(HttpRequestHeader.AcceptCharset, "GB2312,utf-8;q=0.7,*;q=0.7");
            webRequest2.Headers.Add(HttpRequestHeader.AcceptEncoding, "gzip,deflate,sdch");
            webRequest2.Headers.Add(HttpRequestHeader.AcceptLanguage, "zh-cn,zh;q=0.5");
            webRequest2.UserAgent = "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 6.1; Trident/4.0)";
            webRequest2.Accept = "*/*";
            webRequest2.KeepAlive = true;

            webRequest2.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";

            HttpWebResponse response2 = (HttpWebResponse)webRequest2.GetResponse();
            using (StreamReader sr2 = new StreamReader(response2.GetResponseStream(), DefaultResponseEncoding))
            {
                string ret = sr2.ReadToEnd();
                return ret;
            }
        }

        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="url">url</param>
        /// <param name="postData">要发送的数据</param>
        /// <returns></returns>
        public static string PostData(string url, byte[] postData)
        {
            HttpWebRequest webRequest2 = (HttpWebRequest)WebRequest.Create(new Uri(url));
            webRequest2.Headers.Add(HttpRequestHeader.AcceptCharset, "GB2312,utf-8;q=0.7,*;q=0.7");
            webRequest2.Headers.Add(HttpRequestHeader.AcceptEncoding, "gzip,deflate,sdch");
            webRequest2.Headers.Add(HttpRequestHeader.AcceptLanguage, "zh-cn,zh;q=0.5");
            webRequest2.UserAgent = "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 6.1; Trident/4.0)";
            webRequest2.Method = "POST";
            webRequest2.Accept = "*/*";
            webRequest2.KeepAlive = true;

            webRequest2.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
            webRequest2.ContentLength = postData.Length;
            using (Stream postStm = webRequest2.GetRequestStream())
            {
                // Send the data.
                postStm.Write(postData, 0, postData.Length);    //写入参数
            }
            HttpWebResponse response2 = (HttpWebResponse)webRequest2.GetResponse();
            using (StreamReader sr2 = new StreamReader(response2.GetResponseStream(), DefaultResponseEncoding))
            {
                string ret = sr2.ReadToEnd();
                return ret;
            }
        }
    }
}
