using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace Buffalo.WebKernel.WebCommons
{
    public class PagerUrlCreater
    {
        Dictionary<string, string> values = new Dictionary<string, string>();
        public PagerUrlCreater() 
        {
            string[] keys = HttpContext.Current.Request.QueryString.AllKeys;
            foreach (string key in keys) 
            {
                values[key] = HttpContext.Current.Request.QueryString[key];
            }
        }

        /// <summary>
        /// 根据键值的变量集合
        /// </summary>
        public string this[string key] 
        {
            get 
            {
                string ret = null;
                values.TryGetValue(key, out ret);
                return ret;
            }
            set 
            {
                values[key] = value;
            }
        }

        /// <summary>
        /// 根据键值生成url
        /// </summary>
        /// <returns></returns>
        public string GetUrl() 
        {
            StringBuilder url = new StringBuilder(HttpContext.Current.Request.Url.AbsolutePath);
            if (values.Count > 0) 
            {
                url.Append("?");
                Dictionary<string, string>.Enumerator enums = values.GetEnumerator();
                while (enums.MoveNext()) 
                {
                    KeyValuePair<string, string> item = enums.Current;
                    url.Append(item.Key);
                    url.Append("=");
                    url.Append(item.Value);
                    url.Append("&");
                }
                url.Remove(url.Length - 1, 1);
            }
            return url.ToString();
        }
    }
}
