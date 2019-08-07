using System;
using System.Collections.Generic;
using System.Text;

namespace Commons
{
    public class WebServicesLoader
    {


        /// <summary>
        /// ����webservices
        /// </summary>
        /// <typeparam name="T">webservices������</typeparam>
        /// <param name="urlName">���õ�����</param>
        /// <param name="args">��������</param>
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
        /// ����webservices
        /// </summary>
        /// <typeparam name="T">webservices������</typeparam>
        /// <param name="urlName">���õ�����</param>
        /// <returns></returns>
        public static T LoadWebServices<T>(string urlName) where T : System.Web.Services.Protocols.SoapHttpClientProtocol
        {
            return LoadWebServices<T>(urlName, null);
        }
    }
}
