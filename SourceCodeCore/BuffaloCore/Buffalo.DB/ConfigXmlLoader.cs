using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;
using Buffalo.DB.DataBaseAdapter;
using System.Diagnostics;
namespace Buffalo.DB
{
    /// <summary>
    /// ���ü�����
    /// </summary>
    public class ConfigXmlLoader
    {
        /// <summary>
        /// ��ȡ����XML
        /// </summary>
        /// <param name="configContent">����·�����ļ�</param>
        /// <returns></returns>
        public static List<ConfigInfo> LoadXmlContent(IEnumerable<string> configContent)
        {
            //string configRoot = System.Configuration.ConfigurationManager.AppSettings[appRoot];
            List<ConfigInfo> docs = new List<ConfigInfo>();

            if (configContent == null)
            {
                return docs;
            }
            //string[] configs = configRoot.Split('|');
            foreach (string content in configContent)
            {
               
                XmlDocument doc = new XmlDocument();
                try
                {
                    doc.LoadXml(content);

                    ConfigInfo info = new ConfigInfo("", doc);
                    docs.Add(info);
                }
                catch (Exception ex)
                {
#if DEBUG
                    Debug.Write(ex.ToString());
#endif
                }

            }

            return docs;
        }

        /// <summary>
        /// ��ȡ����XML
        /// </summary>
        /// <param name="configPaths">����·������</param>
        /// <returns></returns>
        public static List<ConfigInfo> LoadXml(IEnumerable<string> configPaths)
        {
            //string configRoot = System.Configuration.ConfigurationManager.AppSettings[appRoot];
            List<ConfigInfo> docs = new List<ConfigInfo>();

            if (configPaths == null)
            {
                return docs;
            }
            //string[] configs = configRoot.Split('|');
            foreach (string config in configPaths)
            {
                string path = Buffalo.Kernel.CommonMethods.GetBaseRoot(config);
                if (!File.Exists(path))
                {
                    continue;
                }
                XmlDocument doc = new XmlDocument();
                try
                {
                    doc.Load(path);

                    ConfigInfo info = new ConfigInfo(path, doc);
                    docs.Add(info);
                }
                catch (Exception ex)
                {
#if DEBUG
                    Debug.Write(ex.ToString());
#endif
                }

            }

            return docs;
        }
    }
}
