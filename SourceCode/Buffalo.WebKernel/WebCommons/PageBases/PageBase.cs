using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using System.Collections.Generic;
namespace Buffalo.WebKernel.WebCommons.PageBases
{
    /// <summary>
    /// 所有页面的基类
    /// </summary>
    public class PageBase<T> : PageContext
    {
        public PageBase()
        {
            
            //
            // TODO: 在此处添加构造函数逻辑
            //
        }

        

        private static string userSessionName = null;

        /// <summary>
        /// 当前用户的Session名
        /// </summary>
        protected static string UserSessionName
        {
            get
            {
                if (userSessionName == null)
                {
                    Type type = typeof(T);
                    userSessionName = "user_" + type.FullName;
                }
                return userSessionName;
            }
        }


        private static Dictionary<string, string> DicQueryString
        {
            get
            {
                Dictionary<string, string> dic = HttpContext.Current.Session["$$request_query_string"] as Dictionary<string, string>;
                if (dic == null)
                {
                    dic = InitQueryString();
                    HttpContext.Current.Session["$$request_query_string"] = dic;
                }
                return dic;
            }
        }

        /// <summary>
        /// 初始化查询字符串
        /// </summary>
        private static Dictionary<string, string> InitQueryString()
        {

            Dictionary<string, string> dicQueryString = new Dictionary<string, string>();
            string[] keys = HttpContext.Current.Request.QueryString.AllKeys;
            foreach (string key in keys)
            {
                dicQueryString[key] = HttpContext.Current.Request.QueryString[key];
            }
            return dicQueryString;
        }

        /// <summary>
        /// 设置get传递的某个键的值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void SetQueryString(string key, string value)
        {
            DicQueryString[key] = value;
        }

        /// <summary>
        /// 设置get传递的某个键的值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static string GetQueryString(string key)
        {
            string ret = null;
            DicQueryString.TryGetValue(key, out ret);
            return ret;
        }

        /// <summary>
        /// 删除某个get传递的键值
        /// </summary>
        /// <param name="key"></param>
        public static void DeleteQueryString(string key)
        {
            DicQueryString.Remove(key);
        }

        /// <summary>
        /// 生成查询字符串
        /// </summary>
        /// <returns></returns>
        public static string CreateQueryString()
        {

            Dictionary<string, string>.Enumerator enu = DicQueryString.GetEnumerator();
            string ret = "";
            while (enu.MoveNext())
            {
                ret += "&" + enu.Current.Key + "=" + enu.Current.Value;
            }
            if (ret.Length > 0)
            {
                ret = ret.Substring(1, ret.Length - 1);
            }
            return ret;
        }
        /// <summary>
        /// 获取或设置当前登陆用户
        /// </summary>
        public static T CurrentUser
        {
            get
            {
                object obj=HttpContext.Current.Session[UserSessionName];
                if (obj != null) 
                {
                    return (T)obj;
                }
                return default(T) ;
            }
            set
            {
                HttpContext.Current.Session[UserSessionName] = value;
            }
        }

        

        
    }
    
}