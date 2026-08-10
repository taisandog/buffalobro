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
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;

namespace Buffalo.WebKernel.WebCommons.PostForms
{
    /// <summary>
    /// 模拟向表单发送数据
    /// </summary>
    public class FormPostAsync
    {
        static String MULTIPART_FORM_DATA = "multipart/form-data";

        private PostHead _requestHead;

        /// <summary>
        /// 请求头
        /// </summary>
        public PostHead RequestHead
        {
            get
            {

                return _requestHead;
            }
            set { _requestHead = value; }
        }

        public FormPostAsync()
        {
            //_userAgent = DefaultUserAgent;
            _requestHead = PostHead.CreateHeader();
        }



        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="actionUrl">发送的链接</param>
        /// <param name="prms">发送数据的字段和值</param>
        /// <param name="files">要发送的文件</param>
        /// <returns></returns>
        public async Task<String> PostDataAsync(String actionUrl, IEnumerable<KeyValuePair<string, string>> prms, IEnumerable<FormFile> files)
        {
            HttpWebRequest request = FormPost.CreateHttpWebRequest(actionUrl);

            await PostDataAsync(request, RequestHead, prms, files);

            using (HttpWebResponse rep = await request.GetResponseAsync() as HttpWebResponse)
            {
                
                using (StreamReader reader =await GetStreamReaderAsync(rep, RequestHead.PageEncoding))
                {
                    return await reader.ReadToEndAsync();
                }
            }
        }
        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="actionUrl">发送的链接</param>
        /// <param name="prms">发送数据的字段和值</param>
        /// <param name="files">要发送的文件</param>
        /// <returns></returns>
        public Task<String> PostAsync(String actionUrl, string sdata)
        {
            return PostAsync(actionUrl, PostHead.DefaultEncoding.GetBytes(sdata));
        }
        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="actionUrl">发送的链接</param>
        /// <param name="prms">发送数据的字段和值</param>
        /// <param name="files">要发送的文件</param>
        /// <returns></returns>
        public async Task<String> PostAsync(String actionUrl, byte[] data)
        {
            HttpWebRequest request = FormPost.CreateHttpWebRequest(actionUrl);

            await PostAsync(request, RequestHead, data);

            using (HttpWebResponse rep = request.GetResponse() as HttpWebResponse)
            {

                using (StreamReader reader =await GetStreamReaderAsync(rep, RequestHead.PageEncoding))
                {
                    return await reader.ReadToEndAsync();
                }
            }
        }
        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="actionUrl">发送的链接</param>
        /// <param name="prms">发送数据的字段和值</param>
        /// <param name="files">要发送的文件</param>
        /// <returns></returns>
        public async Task<String> PostAsync(String actionUrl, IDictionary<string, string> prms)
        {


            using (StreamReader reader =await PostDataReaderAsync(actionUrl, prms, null))

            {
                return await reader.ReadToEndAsync();
            }

            

        }
        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="actionUrl">发送的链接</param>
        /// <param name="prms">发送数据的字段和值</param>
        /// <param name="files">要发送的文件</param>
        /// <returns></returns>
        public async Task<Stream> PostDataStreamAsync(String actionUrl, IDictionary<string, string> prms, IEnumerable<FormFile> files)
        {
            HttpWebRequest request = FormPost.CreateHttpWebRequest(actionUrl);
            await PostDataAsync(request, RequestHead, prms, files);

            HttpWebResponse rep =await request.GetResponseAsync() as HttpWebResponse;

            return rep.GetResponseStream();
        }
        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="actionUrl">发送的链接</param>
        /// <param name="prms">发送数据的字段和值</param>
        /// <param name="files">要发送的文件</param>
        /// <returns></returns>
        public async Task<Stream> PostStreamAsync(String actionUrl, byte[] data)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(actionUrl);

            await PostAsync(request, RequestHead, data);

            HttpWebResponse rep =await request.GetResponseAsync() as HttpWebResponse;

            return rep.GetResponseStream();
        }
        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="actionUrl">发送的链接</param>
        /// <param name="prms">发送数据的字段和值</param>
        /// <param name="files">要发送的文件</param>
        /// <returns></returns>
        public async Task<StreamReader> PostDataReaderAsync(String actionUrl, IDictionary<string, string> prms, FormFile[] files)
        {
            HttpWebRequest request = FormPost.CreateHttpWebRequest(actionUrl);

            await PostDataAsync(request, RequestHead, prms, files);

            HttpWebResponse rep =await request.GetResponseAsync() as HttpWebResponse;

            StreamReader reader =await GetStreamReaderAsync(rep, RequestHead.PageEncoding);

            return reader;

        }

        /// <summary>
        /// 请求参数
        /// </summary>
        /// <param name="prms"></param>
        /// <returns></returns>
        private string GetParamValue(IDictionary<string, string> prms)
        {
            StringBuilder args = new StringBuilder();
            foreach (KeyValuePair<string, string> kvp in prms)
            {
                args.Append(System.Web.HttpUtility.UrlEncode(kvp.Key));
                args.Append("=");
                args.Append(System.Web.HttpUtility.UrlEncode(kvp.Value));
                args.Append("&");
            }
            if (args.Length > 0)
            {
                args.Remove(args.Length - 1, 1);
            }
            return args.ToString();
        }

        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="actionUrl">发送的链接</param>
        /// <param name="prms">发送数据的字段和值</param>
        /// <param name="files">要发送的文件</param>
        /// <returns></returns>
        public async Task<HttpWebResponse> PostDataResponseAsync(String actionUrl, IDictionary<string, string> prms, FormFile[] files)
        {
            HttpWebRequest request =FormPost.CreateHttpWebRequest(actionUrl);
           

            await PostDataAsync(request, RequestHead, prms, files);

            HttpWebResponse rep =await request.GetResponseAsync() as HttpWebResponse;

            return rep;
        }
        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="actionUrl">发送的链接</param>
        /// <param name="prms">发送数据的字段和值</param>
        /// <param name="files">要发送的文件</param>
        /// <returns></returns>
        public async Task<HttpWebResponse> PostResponseAsync(String url, byte[] data)
        {
            HttpWebRequest request = FormPost.CreateHttpWebRequest(url);
            await PostAsync(request, RequestHead, data);

            HttpWebResponse rep =await request.GetResponseAsync() as HttpWebResponse;
            
            return rep;
        }

        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="request">发送请求</param>
        /// <param name="prms">发送数据的字段和值</param>
        /// <param name="files">要发送的文件</param>
        /// <returns></returns>
        public static async Task PostAsync(HttpWebRequest request, PostHead head, byte[] data)
        {
            request.CachePolicy = new RequestCachePolicy(RequestCacheLevel.NoCacheNoStore);
            request.Method = "POST";
            head.FillInfo(request);
            Encoding useEncoding = head.PageEncoding;
            if (data != null)
            {
                using (Stream outStream =await request.GetRequestStreamAsync())
                {
                    await WriteDataAsync(outStream, data);
                    //await outStream.FlushAsync();
                }
            }
        }
        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="request">发送请求</param>
        /// <param name="prms">发送数据的字段和值</param>
        /// <param name="files">要发送的文件</param>
        /// <returns></returns>
        public static async Task PostDataAsync(HttpWebRequest request, PostHead head, IEnumerable<KeyValuePair<string, string>> prms, IEnumerable<FormFile> files)
        {
            String BOUNDARY = "---------" + CommonMethods.GuidToString(Guid.NewGuid()); //数据分隔线  
            request.CachePolicy = new RequestCachePolicy(RequestCacheLevel.NoCacheNoStore);
            request.Method = "POST";

            head.FillInfo(request);
            request.ContentType = MULTIPART_FORM_DATA + "; boundary=" + BOUNDARY;
            StringBuilder sb = new StringBuilder();
            //上传的表单参数部分，格式请参考文章  
            if (prms != null)
            {
                foreach (KeyValuePair<string, string> entry in prms)
                {//构建表单字段内容  
                    sb.Append("--");
                    sb.Append(BOUNDARY);
                    sb.Append("\r\n");
                    sb.Append("Content-Disposition: form-data; name=\"" + entry.Key + "\"\r\n\r\n");
                    sb.Append(entry.Value);
                    sb.Append("\r\n");
                }
            }
            Encoding useEncoding = head.PageEncoding;


            using (Stream outStream = request.GetRequestStream())
            {

                byte[] send = useEncoding.GetBytes(sb.ToString());
                await WriteDataAsync(outStream, send);
                if (files != null)
                {
                    //上传的文件部分，格式请参考文章  
                    foreach (FormFile file in files)
                    {
                        StringBuilder split = new StringBuilder();
                        split.Append("--");
                        split.Append(BOUNDARY);
                        split.Append("\r\n");
                        split.Append("Content-Disposition: form-data;name=\"" + file.FormName + "\";filename=\"" + file.FileName + "\"\r\n");
                        split.Append("Content-Type: " + file.ContentType + "\r\n\r\n");
                        await WriteStringAsync(outStream, split.ToString(), useEncoding);
                        if (file.Data != null)
                        {
                            await WriteDataAsync(outStream, file.Data);
                        }
                        else
                        {
                            await WriteStreamAsync(outStream, file.DataStream);

                        }
                        await WriteStringAsync(outStream, "\r\n", useEncoding);

                    }
                }
                byte[] end_data = System.Text.Encoding.UTF8.GetBytes("--" + BOUNDARY + "--\r\n");//数据结束标志           
                await outStream.WriteAsync(end_data, 0, end_data.Length);
                await outStream.FlushAsync();

            }
        }
        /// <summary>
        /// 写入数据
        /// </summary>
        /// <param name="stm"></param>
        /// <param name="dataStream"></param>
        private static Task WriteStreamAsync(Stream stm, Stream dataStream)
        {
            return CopyStreamDataAsync( dataStream,stm);
        }
        /// <summary>
        /// 写入数据
        /// </summary>
        /// <param name="stm"></param>
        /// <param name="data"></param>
        private static async Task WriteDataAsync(Stream stm, byte[] data)
        {
            await stm.WriteAsync(data, 0, data.Length);
        }

        /// <summary>
        /// 写入字符串
        /// </summary>
        /// <param name="stm"></param>
        /// <param name="data"></param>
        private static Task WriteStringAsync(Stream stm, string str, Encoding encoding)
        {
            byte[] data = encoding.GetBytes(str);
            return stm.WriteAsync(data, 0, data.Length);
        }


        #region GET
        /// <summary>
        /// Get获取数据
        /// </summary>
        /// <param name="actionUrl">发送的链接</param>
        /// <returns></returns>
        public async Task<String> GetDataAsync(String actionUrl)
        {


            using (HttpWebResponse rep =await CreateGetHttpResponseAsync(actionUrl, RequestHead))
            {


                using (StreamReader reader =await GetStreamReaderAsync(rep, RequestHead.PageEncoding))
                {
                    return await reader.ReadToEndAsync();
                }
            }
        }

        /// <summary>
        /// 获取流读取器
        /// </summary>
        /// <param name="rep"></param>
        /// <returns></returns>
        public static async Task<StreamReader> GetStreamReaderAsync(HttpWebResponse rep, Encoding encoding)
        {
            StreamReader reader = null;

            Stream urlStm = rep.GetResponseStream();

            if ((!CommonMethods.IsNullOrWhiteSpace(rep.ContentEncoding)) && rep.ContentEncoding.Equals("gzip", StringComparison.InvariantCultureIgnoreCase))
            {
                reader = new StreamReader(new GZipStream(urlStm, CompressionMode.Decompress));
            }
            else
            {
                if (!CommonMethods.IsNullOrWhiteSpace(rep.ContentEncoding))
                {
                    encoding = Encoding.GetEncoding(rep.ContentEncoding);
                }
                else
                {
                    if (CommonMethods.IsNullOrWhiteSpace(rep.CharacterSet) || rep.CharacterSet.Equals("ISO-8859-1", StringComparison.CurrentCultureIgnoreCase))
                    {
                        MemoryStream cacheStm = new MemoryStream();

                        await CopyStreamDataAsync(urlStm, cacheStm);
                        urlStm = cacheStm;
                        cacheStm.Position = 0;
                        StreamReader cacheLine = new StreamReader(cacheStm);

                        string line = null;
                        while ((line =await cacheLine.ReadLineAsync()) != null)
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
                reader = new StreamReader(urlStm, encoding);
            }
            return reader;
        }
        public static async Task CopyStreamDataAsync(Stream stmSource, Stream stmTarget)
        {
            byte[] array = new byte[1024];
            int num = 0;
            do
            {
                num =await stmSource.ReadAsync(array, 0, array.Length);
                if (num > 0)
                {
                    await stmTarget.WriteAsync(array, 0, num);
                }
            }
            while (num > 0);
        }
        /// <summary>
        /// 从页面中读取编码
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        private static string GetEncodingFromBody(string line)
        {
            Regex regex = new Regex(@"<meta(\s+)http-equiv(\s*)=(\s*""?\s*)content-type(\s*""?\s+)content(\s*)=(\s*)""text/html;(\s+)charset(\s*)=(\s*)(?<charset>[a-zA-Z0-9-]+?)""(\s*)(/?)>", RegexOptions.IgnoreCase | RegexOptions.Compiled);

            Match regMatch = regex.Match(line);
            if (regMatch.Success)
            {
                string charSet = regMatch.Groups["charset"].Value;
                return charSet;
            }

            return null;
        }
        /// <summary>
        /// Get获取数据
        /// </summary>
        /// <param name="actionUrl">发送的链接</param>
        /// <returns></returns>
        public async Task<HttpWebResponse> GetDataResponse(String actionUrl)
        {
            HttpWebResponse rep =await CreateGetHttpResponseAsync(actionUrl, _requestHead);
            return rep;
        }

        /// <summary>
        /// Get获取数据
        /// </summary>
        /// <param name="actionUrl">发送的链接</param>
        /// <returns></returns>
        public async Task<StreamReader> GetDataReader(String actionUrl)
        {
            HttpWebResponse rep =await CreateGetHttpResponseAsync(actionUrl, _requestHead);
            return await GetStreamReaderAsync(rep, _requestHead.PageEncoding);
        }

        /// <summary>
        /// Get获取数据
        /// </summary>
        /// <param name="actionUrl">发送的链接</param>
        /// <returns></returns>
        public async Task<Stream> GetDataStreamAsync(String actionUrl)
        {
            HttpWebResponse rep =await CreateGetHttpResponseAsync(actionUrl, _requestHead);
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
        public static async Task<HttpWebResponse> CreateGetHttpResponseAsync(string url, PostHead header)
        {
            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentNullException("url");
            }
            HttpWebRequest request = FormPost.CreateHttpWebRequest(url);

            request.Method = "GET";
            header.FillInfo(request);
            return await request.GetResponseAsync() as HttpWebResponse;
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
        public static async Task<HttpWebResponse> CreatePostHttpResponseAsync(string url, IDictionary<string, string> parameters, PostHead header)
        {
            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentNullException("url");
            }

            HttpWebRequest request = FormPost.CreateHttpWebRequest(url);
            request.Method = "POST";
            header.FillInfo(request);
            request.ContentType = "application/x-www-form-urlencoded";

            //如果需要POST数据  
            if (!(parameters == null || parameters.Count == 0))
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
                byte[] data = header.PageEncoding.GetBytes(buffer.ToString());
                using (Stream stream = await request.GetRequestStreamAsync())
                {
                    await stream.WriteAsync(data, 0, data.Length);
                }
            }
            return await request.GetResponseAsync() as HttpWebResponse;
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