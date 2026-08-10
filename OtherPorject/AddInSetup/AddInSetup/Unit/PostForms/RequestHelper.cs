
using Buffalo.WebKernel.WebCommons.PostForms;

using Newtonsoft.Json;
using Buffalo.ArgCommon;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;

namespace GameBoxAPIHandle
{
    /// <summary>
    /// 提交远程信息
    /// </summary>
    public class RequestHelper:SortedDictionary<string,object>
    {
        
        private string _url;

        /// <summary>
        /// 当前的地址
        /// </summary>
        public string Url
        {
            get { return _url; }
            set { _url = value; }
        }

        PostHead _head = null;
        /// <summary>
        /// 请求头
        /// </summary>
        public PostHead Header
        {
            get { return _head; }
        }
        
        /// <summary>
        /// 带参数Http请求地址
        /// </summary>
        public RequestHelper():base()
        {
            _head = PostHead.CreateHeader();
            _head.Timeout = 10 * 1000;
        }
        /// <summary>
        /// 带参数Http请求地址
        /// </summary>
        /// <param name="url">地址</param>
        public RequestHelper(string url)
            : this()
        {
            _url = url;

        }

        
        /// <summary>
        /// 创建Http访问类
        /// </summary>
        /// <returns></returns>
        public FormPost GetFormPost() 
        {
            FormPost post = new FormPost();
            post.RequestHead.Timeout = 10000;
            
            return post;
        }


        /// <summary>
        /// 使用get来调用Web
        /// </summary>
        /// <returns></returns>
        public string GetData()
        {
            FormPost post = GetFormPost();
            APIResault ret = null;


            string rurl = GetFullGetUrl();
            string res = post.GetData(rurl);



            return res;
        }


        /// <summary>
        /// 使用get来调用Web
        /// </summary>
        /// <returns></returns>
        public APIResault DoGet() 
        {
            FormPost post = GetFormPost();
            APIResault ret = null;
            try
            {
               
                string rurl = GetFullGetUrl();
                string res = post.GetData(rurl);
                ret = new APIResault();
                ret.SetJson(res);
            }
            catch (WebException ex)
            {
                ret = GetExAPI(ex);
            }
            catch (Exception ex) 
            {
                ret = new APIResault();
                ret.SetException(ex);
            }
            return ret;
        }

        private APIResault GetExAPI(WebException ex) 
        {
            APIResault ret=new APIResault();
            try
            {
                HttpWebResponse wenReq = (HttpWebResponse)ex.Response;
                if (wenReq == null)
                {
                    ret.SetException(ex);
                    return ret;
                }
                using (StreamReader sr = new StreamReader(wenReq.GetResponseStream(), Encoding.UTF8))
                {
                    ret.SetException(new WebException(sr.ReadToEnd(), ex));
                    return ret;
                }
            }
            catch (Exception iex) 
            {
                ret.SetException(iex);
            }
            return ret;
        }
        /// <summary>
        /// 使用post来调用Web
        /// </summary>
        /// <returns></returns>
        public APIResault DoPostData(byte[] data)
        {
            FormPost post = GetFormPost();
            APIResault ret = new APIResault();
            try
            {

                string rurl = GetFullGetUrl();
                post.RequestHead = _head;
                FormFile[] arrFile = new FormFile[] {new FormFile("args", data, "args") };
                string res = post.PostData(rurl, null, arrFile);
                ret.SetJson(res);
            }
            catch (WebException ex)
            {
                ret = GetExAPI(ex);
            }
            catch (Exception ex)
            {
                ret.SetException(ex);
            }
            return ret;
        }
        /// <summary>
        /// 使用post来调用Web
        /// </summary>
        /// <returns></returns>
        public APIResault DoPostWebAPI()
        {
            FormPost post = GetFormPost();
            APIResault ret = new APIResault();
            try
            {
                Dictionary<string, string> prms = new Dictionary<string, string>();
                string data = JsonConvert.SerializeObject(this);
                post.RequestHead = _head;
                post.RequestHead.ContentType = "application/json";
                string res = post.Post(_url, data);
                ret.SetJson(res);
            }
            catch (WebException ex)
            {
                ret = GetExAPI(ex);
            }
            catch (Exception ex)
            {
                ret.SetException(ex);
            }
            return ret;
        }
        /// <summary>
        /// 使用post来调用Web
        /// </summary>
        /// <returns></returns>
        public APIResault DoPostWAPI()
        {
            //FormPost post = GetFormPost();
            //APIResault ret = new APIResault();
            //try
            //{
            //    Dictionary<string, string> prms = new Dictionary<string, string>();
            //    prms["args"] = JsonConvert.SerializeObject(this);
            //    //string arg = "args=" + System.Web.HttpUtility.UrlEncode(JsonConvert.SerializeObject(this));
            //    //post.RequestHead = _head;
            //    post.RequestHead.Accept = "application/json";
            //    post.RequestHead.ContentType = "application/json";
            //    string res = post.Post(_url, JsonConvert.SerializeObject(prms));
            //    ret.SetJson(res);
            //}
            //catch (WebException ex)
            //{
            //    ret = GetExAPI(ex);
            //}
            //catch (Exception ex)
            //{
            //    ret.SetException(ex);
            //}
            //return ret;

            Dictionary<string, string> prms = new Dictionary<string, string>();
            prms["args"] = JsonConvert.SerializeObject(this);
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(_url);
                //client.DefaultRequestHeaders.Accept.Add(
                //    new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = client.PostAsync(_url, new StringContent(JsonConvert.SerializeObject(prms))).Result;
                if (!response.IsSuccessStatusCode)
                {
                    return ApiCommon.GetFault(response.StatusCode.ToString());
                }

                string res = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                APIResault ret = new APIResault();
                ret.SetJson(res);
                return ret;
            }
        }

        /// <summary>
        /// 使用post来调用Web
        /// </summary>
        /// <returns></returns>
        public APIResault DoPost()
        {
            FormPost post = GetFormPost();
            APIResault ret = new APIResault();
            try
            {
                Dictionary<string, string> prms = new Dictionary<string, string>();
                prms[""] = JsonConvert.SerializeObject(this);
                post.RequestHead = _head;
                string res = post.PostData(_url, prms, null);
                ret.SetJson(res);
            }
            catch (WebException ex)
            {
                ret = GetExAPI(ex);
            }
            catch (Exception ex)
            {
                ret.SetException(ex);
            }
            return ret;
        }
       
       
        /// <summary>
        /// 获取参数信息
        /// </summary>
        /// <param name="sbUrl"></param>
        public StringBuilder GetParams() 
        {
            string json = JsonConvert.SerializeObject(this);
            StringBuilder sbParam = new StringBuilder();
            sbParam.Append("args=");
            sbParam.Append(System.Web.HttpUtility.UrlEncode(json));
            return sbParam;
        }

       
        /// <summary>
        /// Get的地址+参数,
        /// </summary>
        /// <param name="hasToken">包含Token和seqid</param>
        /// <returns></returns>
        public string GetFullGetUrl()
        {
            StringBuilder prms = GetParams();

            string p = prms.ToString();

            if (string.IsNullOrWhiteSpace(p))
            {
                return _url;
            }
            string conn = "?";
            if (_url.IndexOf("?") > 0)
            {
                conn = "&";
            }
            return _url + conn + p;

        }
    }
}
