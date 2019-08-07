using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace Buffalo.WebKernel.WebCommons
{
    public class LoginRecord
    {
        /// <summary>
        /// �û���
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
        /// ����
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
        /// ��յ�½��Ϣ
        /// </summary>
        public static void ClearLogin() 
        {
            RemoveCookies("name");
            RemoveCookies("password");
        }

        /// <summary>
        /// �Ƴ�ָ������Cookies
        /// </summary>
        /// <param name="key">��</param>
        private static void RemoveCookies(string key) 
        {
            HttpCookie objCookies = new HttpCookie(key, "");
            objCookies.Expires = DateTime.Now.AddYears(-1);//��һ����cookies����
            HttpContext.Current.Response.Cookies.Add(objCookies);
        }

        /// <summary>
        /// ��ȡcookies��ֵ
        /// </summary>
        /// <param name="key">��Ӧ��</param>
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
        /// ����cookies
        /// </summary>
        /// <param name="key">cookies��</param>
        /// <param name="value">ֵ</param>
        public static void SetCookiesValue(string key, string value) 
        {
            SetCookiesValue(key, value, DateTime.Now.AddYears(1));//����һ��
        }

        /// <summary>
        /// ����cookies
        /// </summary>
        /// <param name="key">cookies��</param>
        /// <param name="value">ֵ</param>
        /// <param name="expiresTime">����ʱ��</param>
        public static void SetCookiesValue(string key, string value, DateTime expiresTime) 
        {
            HttpCookie objCookies = new HttpCookie(key, value);
            objCookies.Expires = expiresTime;
            HttpContext.Current.Response.Cookies.Add(objCookies);
        } 
    }
}
