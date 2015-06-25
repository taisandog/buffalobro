using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace Buffalo.Kernel
{
    /// <summary>
    /// 连接字符串处理类
    /// </summary>
    public class ConnStrFilter
    {
        /// <summary>
        /// 获取连接信息
        /// </summary>
        /// <param name="connstr">字符串</param>
        /// <param name="comparer">比较器</param>
        /// <returns></returns>
        public static Hashtable GetConnectInfo(string connstr, IEqualityComparer comparer) 
        {
            Hashtable hs = new Hashtable(comparer);
            string[] conStrs = connstr.Split(';');
            foreach (string part in conStrs)
            {
                string[] items = part.Split('=');
                if (items.Length < 2) 
                {
                    continue;
                }
                hs[items[0].Trim()] = items[1].Trim();
            }
            return hs;
        }
        /// <summary>
        /// 获取连接信息
        /// </summary>
        /// <param name="connstr">字符串</param>
        /// <param name="comparer">比较器</param>
        /// <returns></returns>
        public static Hashtable GetConnectInfo(string connstr)
        {
            return GetConnectInfo(connstr, StringComparer.CurrentCultureIgnoreCase);
        }
    }
}
