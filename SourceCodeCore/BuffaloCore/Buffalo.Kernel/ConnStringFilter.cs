using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace Buffalo.Kernel
{
    /// <summary>
    /// 连接字符串处理类
    /// </summary>
    public class ConnStringFilter
    {
        /// <summary>
        /// 获取连接信息
        /// </summary>
        /// <param name="connstr">字符串</param>
        /// <param name="comparer">比较器</param>
        /// <returns></returns>
        public static Dictionary<string,string> GetConnectInfo(string connstr, IEqualityComparer<string> comparer) 
        {
            Dictionary<string, string> hs = new Dictionary<string, string>(comparer);
            string[] conStrs = connstr.Split(';');
            foreach (string part in conStrs)
            {
                string[] items = part.Split('=');
                if (items.Length < 2) 
                {
                    continue;
                }
                string key = items[0];
                if (!string.IsNullOrWhiteSpace(key)) 
                {
                    key = System.Web.HttpUtility.UrlDecode(key.Trim());
                }
                string value = items[1];
                if (!string.IsNullOrWhiteSpace(value))
                {
                    value = System.Web.HttpUtility.UrlDecode(value.Trim());
                }
                hs[key] = value;
            }
            return hs;
        }
        /// <summary>
        /// 获取连接信息
        /// </summary>
        /// <param name="connstr">字符串</param>
        /// <param name="comparer">比较器</param>
        /// <returns></returns>
        public static Dictionary<string, string> GetConnectInfo(string connstr)
        {
            return GetConnectInfo(connstr, StringComparer.CurrentCultureIgnoreCase);
        }
    }
}
