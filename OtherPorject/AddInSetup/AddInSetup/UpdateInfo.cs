
using Buffalo.WebKernel.WebCommons.PostForms;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace AddInSetup
{
    /// <summary>
    /// 更新类
    /// </summary>
    public class UpdateInfo
    {
        /// <summary>
        /// 获取最新版本
        /// </summary>
        /// <returns></returns>
        public static DateTime GetLastestVersion()
        {
            string url = System.Configuration.ConfigurationManager.AppSettings["Update.Config"];
            if (string.IsNullOrEmpty(url))
            {
                return DateTime.MinValue;
            }
            try
            {
                FormPost post = new FormPost();
                string xml = post.GetData(url);
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xml);
                return ConfigLoader.GetVersion(doc);
            }
            catch { }
            return DateTime.MinValue;
        }
    }
}
