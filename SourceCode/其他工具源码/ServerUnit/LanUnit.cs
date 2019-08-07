using System;
using System.Collections.Generic;

using System.Text;

namespace ServerUnit
{
    /// <summary>
    /// 局域网判断类
    /// </summary>
    public class LanUnit
    {
        private static string[] Localhost = { "localhost", "::1", "127.0.0.1" };

        private static string[] AllowIP = LoadAllowIP();

        /// <summary>
        /// 加载允许的IP
        /// </summary>
        /// <returns></returns>
        private static string[] LoadAllowIP() 
        {
            string config = System.Configuration.ConfigurationManager.AppSettings["AllowIP"];
            if (string.IsNullOrEmpty(config)) 
            {
                return new string[] { };
            }
            string[] ret = config.Split(';');
            List<string> lstRet = new List<string>();
            foreach (string item in ret) 
            {
                lstRet.Add(item.Trim());
            }
            return lstRet.ToArray();
        }

        /// <summary>
        /// 判断是否允许的IP
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static bool IsAllowIP(string ip) 
        {
            foreach (string curip in Localhost) 
            {
                if (curip.Equals(ip, StringComparison.CurrentCultureIgnoreCase)) 
                {
                    return true;
                }
            }
            foreach (string curallip in AllowIP)
            {
                if (curallip.Equals("0.0.0.0")) 
                {
                    return true;
                }
                if (curallip.Equals(ip, StringComparison.CurrentCultureIgnoreCase))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
