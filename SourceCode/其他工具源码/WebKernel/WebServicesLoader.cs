using System;
using System.Collections.Generic;
using System.Text;

namespace Commons
{
    public class WebServicesLoader
    {


        /// <summary>
        /// 加载webservices
        /// </summary>
        /// <typeparam name="T">webservices的类型</typeparam>
        /// <param name="urlName">配置的名称</param>
        /// <param name="args">参数类型</param>
        /// <returns></returns>
        public static T LoadWebServices<T>(string urlName,object[] args) where T : System.Web.Services.Protocols.SoapHttpClientProtocol
        {
            string url=System.Configuration.ConfigurationManager.AppSettings[urlName];
            T objSer = null;
            if (args == null)
            {
                objSer = Activator.CreateInstance(typeof(T)) as T;
            }
            else 
            {
                objSer = Activator.CreateInstance(typeof(T),args) as T;
            }
            objSer.Url = url;
            return objSer;
        }
        /// <summary>
        /// 加载webservices
        /// </summary>
        /// <typeparam name="T">webservices的类型</typeparam>
        /// <param name="urlName">配置的名称</param>
        /// <returns></returns>
        public static T LoadWebServices<T>(string urlName) where T : System.Web.Services.Protocols.SoapHttpClientProtocol
        {
            return LoadWebServices<T>(urlName, null);
        }
    }
}
