using System;
using System.Data;
using System.Configuration;

using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Cache;
using System.IO;
using Buffalo.Kernel;

namespace Buffalo.WebKernel.WebCommons.PostForms
{
    /// <summary>
    /// 模拟向表单发送数据
    /// </summary>
    public class FormPost
    {
        public FormPost()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
        }
        static String MULTIPART_FORM_DATA = "multipart/form-data";
        static Encoding encoding = Encoding.UTF8;
        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="actionUrl">发送的链接</param>
        /// <param name="prms">发送数据的字段和值</param>
        /// <param name="files">要发送的文件</param>
        /// <returns></returns>
        public static String PostData(String actionUrl, Dictionary<string, string> prms, FormFile[] files)
        {
            MemoryStream stm = new MemoryStream();
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(actionUrl);

            PostData(request, prms, files);

            WebResponse rep = request.GetResponse();

            using (StreamReader reader = new StreamReader(rep.GetResponseStream(), encoding))
            {
                return reader.ReadToEnd();
            }
        }

        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="request">发送请求</param>
        /// <param name="prms">发送数据的字段和值</param>
        /// <param name="files">要发送的文件</param>
        /// <returns></returns>
        public static void PostData(HttpWebRequest request, Dictionary<string, string> prms, FormFile[] files)
        {
            String BOUNDARY = "---------" + CommonMethods.GuidToString(Guid.NewGuid()); //数据分隔线  
            request.CachePolicy = new RequestCachePolicy(RequestCacheLevel.NoCacheNoStore);
            //conn.setUseCaches(false);//不使用Cache  
            request.Method = "POST";

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

            using (Stream outStream = request.GetRequestStream())
            {
                byte[] send = System.Text.Encoding.UTF8.GetBytes(sb.ToString());
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
                    WriteString(outStream, split.ToString());
                    WriteData(outStream, file.Data);
                    WriteString(outStream, "\r\n");

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
        private static void WriteString(Stream stm, string str)
        {
            byte[] data = encoding.GetBytes(str);
            stm.Write(data, 0, data.Length);
        }
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