using System;
using System.Data;
using System.Configuration;

using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Cache;
using System.IO;
using Buffalo.Kernel;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using System.IO.Compression;
using System.Text.RegularExpressions;

namespace Buffalo.WebKernel.WebCommons.PostForms
{
    /// <summary>
    /// 模拟向表单发送数据
    /// </summary>
    public class FormPost
    {
        private static readonly string DefaultUserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; SV1; .NET CLR 1.1.4322; .NET CLR 2.0.50727)";  
        public FormPost()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
        }
        static String MULTIPART_FORM_DATA = "multipart/form-data";
        static Encoding DefaultEncoding = Encoding.UTF8;
        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="actionUrl">发送的链接</param>
        /// <param name="prms">发送数据的字段和值</param>
        /// <param name="files">要发送的文件</param>
        /// <returns></returns>
        public static String PostData(String actionUrl, Dictionary<string, string> prms, FormFile[] files)
        {
            return PostData(actionUrl, prms, files, null);
        }
        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="actionUrl">发送的链接</param>
        /// <param name="prms">发送数据的字段和值</param>
        /// <param name="files">要发送的文件</param>
        /// <returns></returns>
        public static String PostData(String actionUrl, Dictionary<string, string> prms, FormFile[] files,Encoding encoding)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(actionUrl);

            PostData(request, prms, files, encoding);

            HttpWebResponse rep = request.GetResponse() as HttpWebResponse;

            using (StreamReader reader = GetStreamReader(rep))
            {
                return reader.ReadToEnd();
            }
        }

        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="actionUrl">发送的链接</param>
        /// <param name="prms">发送数据的字段和值</param>
        /// <param name="files">要发送的文件</param>
        /// <returns></returns>
        public static Stream PostDataStream(String actionUrl, Dictionary<string, string> prms, FormFile[] files, Encoding encoding)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(actionUrl);

            PostData(request, prms, files, encoding);

            HttpWebResponse rep = request.GetResponse() as HttpWebResponse;

            return rep.GetResponseStream();
        }
        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="actionUrl">发送的链接</param>
        /// <param name="prms">发送数据的字段和值</param>
        /// <param name="files">要发送的文件</param>
        /// <returns></returns>
        public static StreamReader PostDataReader(String actionUrl, Dictionary<string, string> prms, FormFile[] files, Encoding encoding)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(actionUrl);

            PostData(request, prms, files, encoding);

            HttpWebResponse rep = request.GetResponse() as HttpWebResponse;

            StreamReader reader = GetStreamReader(rep);

            return reader;

        }
        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="actionUrl">发送的链接</param>
        /// <param name="prms">发送数据的字段和值</param>
        /// <param name="files">要发送的文件</param>
        /// <returns></returns>
        public static HttpWebResponse PostDataResponse(String actionUrl, Dictionary<string, string> prms, FormFile[] files)
        {
            return PostDataResponse(actionUrl, prms, files, null);
        }
        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="actionUrl">发送的链接</param>
        /// <param name="prms">发送数据的字段和值</param>
        /// <param name="files">要发送的文件</param>
        /// <returns></returns>
        public static HttpWebResponse PostDataResponse(String actionUrl, Dictionary<string, string> prms, FormFile[] files, Encoding encoding)
        {
            
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(actionUrl);

            PostData(request, prms, files, encoding);

            HttpWebResponse rep = request.GetResponse() as HttpWebResponse;

            return rep;
        }



        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="request">发送请求</param>
        /// <param name="prms">发送数据的字段和值</param>
        /// <param name="files">要发送的文件</param>
        /// <returns></returns>
        public static void PostData(HttpWebRequest request, Dictionary<string, string> prms, FormFile[] files,Encoding encoding)
        {
            String BOUNDARY = "---------" + CommonMethods.GuidToString(Guid.NewGuid()); //数据分隔线  
            request.CachePolicy = new RequestCachePolicy(RequestCacheLevel.NoCacheNoStore);
            //conn.setUseCaches(false);//不使用Cache  
            request.Method = "POST";
            request.UserAgent = DefaultUserAgent;
            request.KeepAlive = true;
            
            //request.("Charset", "UTF-8");  
            request.ContentType = MULTIPART_FORM_DATA + "; boundary=" + BOUNDARY;

            StringBuilder sb = new StringBuilder();
            //上传的表单参数部分，格式请参考文章  
            foreach (KeyValuePair<string, string> entry in prms)
            {//构建表单字段内容  
                sb.Append("--");
                sb.Append(BOUNDARY);
                sb.Append("\r\n");
                sb.Append("Content-Disposition: form-data; name=\"" + entry.Key + "\"\r\n\r\n");
                sb.Append(entry.Value);
                sb.Append("\r\n");
            }
            Encoding useEncoding = encoding;
            if (useEncoding == null)
            {
                useEncoding = DefaultEncoding;
            }

            using (Stream outStream = request.GetRequestStream())
            {

                byte[] send = useEncoding.GetBytes(sb.ToString());
                WriteData(outStream, send);

                //上传的文件部分，格式请参考文章  
                foreach (FormFile file in files)
                {
                    StringBuilder split = new StringBuilder();
                    split.Append("--");
                    split.Append(BOUNDARY);
                    split.Append("\r\n");
                    split.Append("Content-Disposition: form-data;name=\"" + file.FormName + "\";filename=\"" + file.FileName + "\"\r\n");
                    split.Append("Content-Type: " + file.ContentType + "\r\n\r\n");
                    WriteString(outStream, split.ToString(),useEncoding);
                    WriteData(outStream, file.Data);
                    WriteString(outStream, "\r\n", useEncoding);

                }
                byte[] end_data = System.Text.Encoding.UTF8.GetBytes("--" + BOUNDARY + "--\r\n");//数据结束标志           
                outStream.Write(end_data, 0, end_data.Length);
                outStream.Flush();

            }
        }

        /// <summary>
        /// 写入数据
        /// </summary>
        /// <param name="stm"></param>
        /// <param name="data"></param>
        private static void WriteData(Stream stm, byte[] data)
        {
            stm.Write(data, 0, data.Length);
        }

        /// <summary>
        /// 写入字符串
        /// </summary>
        /// <param name="stm"></param>
        /// <param name="data"></param>
        private static void WriteString(Stream stm, string str,Encoding encoding)
        {
            byte[] data = encoding.GetBytes(str);
            stm.Write(data, 0, data.Length);
        }


        #region GET
        /// <summary>
        /// Get获取数据
        /// </summary>
        /// <param name="actionUrl">发送的链接</param>
        /// <returns></returns>
        public static String GetData(String actionUrl)
        {


            HttpWebResponse rep = CreateGetHttpResponse(actionUrl, null, null, null);


            using (StreamReader reader = GetStreamReader(rep))
            {
                return reader.ReadToEnd();
            }
        }

        /// <summary>
        /// 获取流读取器
        /// </summary>
        /// <param name="rep"></param>
        /// <returns></returns>
        private static StreamReader GetStreamReader(HttpWebResponse rep) 
        {
            StreamReader reader = null;

            Stream urlStm=rep.GetResponseStream();

            if ((!CommonMethods.IsNullOrWhiteSpace(rep.ContentEncoding)) && rep.ContentEncoding.Equals("gzip", StringComparison.InvariantCultureIgnoreCase))
            {
                reader = new StreamReader(new GZipStream(urlStm, CompressionMode.Decompress));
            }
            else
            {
                Encoding encoding = DefaultEncoding;
                if (!CommonMethods.IsNullOrWhiteSpace(rep.ContentEncoding))
                {
                    encoding = Encoding.GetEncoding(rep.ContentEncoding);
                }
                else 
                {
                    if (CommonMethods.IsNullOrWhiteSpace(rep.CharacterSet) || rep.CharacterSet.Equals("ISO-8859-1", StringComparison.CurrentCultureIgnoreCase))
                    {
                        MemoryStream cacheStm = new MemoryStream();

                        CommonMethods.CopyStreamData(urlStm, cacheStm);
                        urlStm = cacheStm;
                        cacheStm.Position = 0;
                        StreamReader cacheLine = new StreamReader(cacheStm);

                        string line = null;
                        while ((line = cacheLine.ReadLine()) != null)
                        {
                            string charsetString = GetEncodingFromBody(line);
                            if (!CommonMethods.IsNullOrWhiteSpace(charsetString))
                            {
                                encoding = Encoding.GetEncoding(charsetString);

                                break;
                            }
                        }

                        cacheStm.Position = 0;
                    }
                    else
                    {
                        encoding = Encoding.GetEncoding(rep.CharacterSet);
                    }
                }
                reader = new StreamReader(urlStm,encoding);
            }
            return reader;
        }
        /// <summary>
        /// 从页面中读取编码
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        private static string GetEncodingFromBody(string line)
        {
            Regex regex = new Regex(@"<meta(\s+)http-equiv(\s*)=(\s*""?\s*)content-type(\s*""?\s+)content(\s*)=(\s*)""text/html;(\s+)charset(\s*)=(\s*)(?<charset>[a-zA-Z0-9-]+?)""(\s*)(/?)>", RegexOptions.IgnoreCase | RegexOptions.Compiled);

            var regMatch = regex.Match(line);
            if (regMatch.Success)
            {
                var charSet = regMatch.Groups["charset"].Value;
                return charSet;
            }

            return null;
        }
        /// <summary>
        /// Get获取数据
        /// </summary>
        /// <param name="actionUrl">发送的链接</param>
        /// <returns></returns>
        public static HttpWebResponse GetDataResponse(String actionUrl)
        {
            HttpWebResponse rep = CreateGetHttpResponse(actionUrl, null, null, null);
            return rep;
        }
        /// <summary>
        /// Get获取数据
        /// </summary>
        /// <param name="actionUrl">发送的链接</param>
        /// <returns></returns>
        public static StreamReader GetDataReader(String actionUrl)
        {
            HttpWebResponse rep = CreateGetHttpResponse(actionUrl, null, null, null);
            return GetStreamReader(rep);
        }
        /// <summary>
        /// Get获取数据
        /// </summary>
        /// <param name="actionUrl">发送的链接</param>
        /// <returns></returns>
        public static Stream GetDataStream(String actionUrl)
        {
            HttpWebResponse rep = CreateGetHttpResponse(actionUrl, null, null, null);
            return rep.GetResponseStream();
        }

        /// <summary>  
        /// 创建GET方式的HTTP请求  
        /// </summary>  
        /// <param name="url">请求的URL</param>  
        /// <param name="timeout">请求的超时时间</param>  
        /// <param name="userAgent">请求的客户端浏览器信息，可以为空</param>  
        /// <param name="cookies">随同HTTP请求发送的Cookie信息，如果不需要身份验证可以为空</param>  
        /// <returns></returns>  
        public static HttpWebResponse CreateGetHttpResponse(string url,int? timeout, string userAgent,CookieCollection cookies)  
        {  
            if (string.IsNullOrEmpty(url))  
            {  
                throw new ArgumentNullException("url");  
            }  
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;  
            request.Method = "GET";  
            request.UserAgent = DefaultUserAgent;  
            if (!string.IsNullOrEmpty(userAgent))  
            {  
                request.UserAgent = userAgent;  
            }  
            if (timeout.HasValue)  
            {  
                request.Timeout = timeout.Value;  
            }  
            if (cookies != null)  
            {  
                request.CookieContainer = new CookieContainer();  
                request.CookieContainer.Add(cookies);  
            }  
            return request.GetResponse() as HttpWebResponse;  
        }  
        /// <summary>  
        /// 创建POST方式的HTTP请求  
        /// </summary>  
        /// <param name="url">请求的URL</param>  
        /// <param name="parameters">随同请求POST的参数名称及参数值字典</param>  
        /// <param name="timeout">请求的超时时间</param>  
        /// <param name="userAgent">请求的客户端浏览器信息，可以为空</param>  
        /// <param name="requestEncoding">发送HTTP请求时所用的编码</param>  
        /// <param name="cookies">随同HTTP请求发送的Cookie信息，如果不需要身份验证可以为空</param>  
        /// <returns></returns>  
        public static HttpWebResponse CreatePostHttpResponse(string url,IDictionary<string,string> parameters,int? timeout, string userAgent,Encoding requestEncoding,CookieCollection cookies)  
        {  
            if (string.IsNullOrEmpty(url))  
            {  
                throw new ArgumentNullException("url");  
            }  
            if(requestEncoding==null)  
            {  
                throw new ArgumentNullException("requestEncoding");  
            }  
            HttpWebRequest request=null;  
            //如果是发送HTTPS请求  
            if(url.StartsWith("https",StringComparison.OrdinalIgnoreCase))  
            {  
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);  
                request = WebRequest.Create(url) as HttpWebRequest;  
                request.ProtocolVersion=HttpVersion.Version10;  
            }  
            else 
            {  
                request = WebRequest.Create(url) as HttpWebRequest;  
            }  
            request.Method = "POST";  
            request.ContentType = "application/x-www-form-urlencoded";  
              
            if (!string.IsNullOrEmpty(userAgent))  
            {  
                request.UserAgent = userAgent;  
            }  
            else 
            {  
                request.UserAgent = DefaultUserAgent;  
            }  
 
            if (timeout.HasValue)  
            {  
                request.Timeout = timeout.Value;  
            }  
            if (cookies != null)  
            {  
                request.CookieContainer = new CookieContainer();  
                request.CookieContainer.Add(cookies);  
            }  
            //如果需要POST数据  
            if(!(parameters==null||parameters.Count==0))  
            {  
                StringBuilder buffer = new StringBuilder();  
                int i = 0;  
                foreach (string key in parameters.Keys)  
                {  
                    if (i > 0)  
                    {  
                        buffer.AppendFormat("&{0}={1}", key, parameters[key]);  
                    }  
                    else 
                    {  
                        buffer.AppendFormat("{0}={1}", key, parameters[key]);  
                    }  
                    i++;  
                }  
                byte[] data = requestEncoding.GetBytes(buffer.ToString());  
                using (Stream stream = request.GetRequestStream())  
                {  
                    stream.Write(data, 0, data.Length);  
                }  
            }  
            return request.GetResponse() as HttpWebResponse;  
        }  
 
        private static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)  
        {  
            return true; //总是接受  
        }  
    
        #endregion



    }
}

/*------------例子----------
FormFile file = new FormFile("FileUpload1", GetFile("E:\\JiaEn_51aspx.rar"), "form1", "application/zip");
FormFile file2 = new FormFile("FileUpload2", GetFile("E:\\Shopxp.rar"), "form1", "application/zip");
Dictionary<string, string> dic = new Dictionary<string, string>();
dic["name"] = "taisandog";
dic["age"] = "27";
string str = FormPost.PostData("http://localhost:2108/WebSite2/default.aspx", dic, new FormFile[] { file,file2 });
richTextBox1.Text = str;
-----------------------------*/