using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace Buffalo.WebKernel.WebCommons
{
    public class LoginRecord
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public static string Name 
        {
            get 
            {
                return GetCookiesValue("name");
            }
            set 
            {
                SetCookiesValue("name", value);
            }
        }
        /// <summary>
        /// 密码
        /// </summary>
        public static string Password
        {
            get
            {
                return GetCookiesValue("password");
            }
            set
            {
                SetCookiesValue("password", value);
            }
        }

        /// <summary>
        /// 清空登陆信息
        /// </summary>
        public static void ClearLogin() 
        {
            RemoveCookies("name");
            RemoveCookies("password");
        }

        /// <summary>
        /// 移除指定键的Cookies
        /// </summary>
        /// <param name="key">键</param>
        private static void RemoveCookies(string key) 
        {
            HttpCookie objCookies = new HttpCookie(key, "");
            objCookies.Expires = DateTime.Now.AddYears(-1);//负一年让cookies过期
            HttpContext.Current.Response.Cookies.Add(objCookies);
        }

        /// <summary>
        /// 获取cookies的值
        /// </summary>
        /// <param name="key">对应键</param>
        /// <returns></returns>
        public static string GetCookiesValue(string key) 
        {
            HttpCookie objCookies = HttpContext.Current.Request.Cookies[key];
            if (objCookies != null)
            {
                return objCookies.Value;
            }
            return null;
        }

        /// <summary>
        /// 设置cookies
        /// </summary>
        /// <param name="key">cookies键</param>
        /// <param name="value">值</param>
        public static void SetCookiesValue(string key, string value) 
        {
            SetCookiesValue(key, value, DateTime.Now.AddYears(1));//保存一年
        }

        /// <summary>
        /// 设置cookies
        /// </summary>
        /// <param name="key">cookies键</param>
        /// <param name="value">值</param>
        /// <param name="expiresTime">过期时间</param>
        public static void SetCookiesValue(string key, string value, DateTime expiresTime) 
        {
            HttpCookie objCookies = new HttpCookie(key, value);
            objCookies.Expires = expiresTime;
            HttpContext.Current.Response.Cookies.Add(objCookies);
        } 
    }
}
