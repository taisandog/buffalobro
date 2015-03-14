using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.Kernel;
using System.Xml;
using System.IO;

namespace WebShare
{
    /// <summary>
    /// 配置管理器
    /// </summary>
    public class ConfigManager
    {
        private string _bindIP;
        /// <summary>
        /// 绑定IP
        /// </summary>
        public string BindIP
        {
            get { return _bindIP; }
            set { _bindIP = value; }
        }
        private int _bindPort=80;
        /// <summary>
        /// 绑定端口
        /// </summary>
        public int BindPort
        {
            get { return _bindPort; }
            set { _bindPort = value; }
        }
        private ShareInfoCollection _shareInfos=new ShareInfoCollection();
        /// <summary>
        /// 共享信息
        /// </summary>
        public ShareInfoCollection ShareInfos
        {
            get { return _shareInfos; }
        }


        private string _password;
        /// <summary>
        /// 密码
        /// </summary>
        public string Password
        {
            get { return _password; }
            set { _password = value; }
        }
        /// <summary>
        /// 记录配置
        /// </summary>
        public void LoadConfig()
        {
            string path = CommonMethods.GetBaseRoot("config.xml");
            if (!File.Exists(path)) 
            {
                return;
            }
            try
            {
               

                XmlDocument xml = new XmlDocument();
                xml.Load(path);
                XmlNode root = xml.GetElementsByTagName("root")[0];
                XmlAttribute attr = root.Attributes["ip"];
                if (attr != null) 
                {
                    _bindIP = attr.InnerText;
                }
                attr = root.Attributes["port"];
                if (attr != null)
                {
                    _bindPort = Convert.ToInt32(attr.InnerText);
                }
                attr = root.Attributes["pwd"];
                if (attr != null)
                {
                    _password = attr.InnerText;
                }
                XmlNodeList items = root.ChildNodes;
                foreach (XmlNode item in items) 
                {
                    ShareInfo info = new ShareInfo();
                    attr = item.Attributes["name"];
                    if (attr != null)
                    {
                        info.Name = attr.InnerText;
                    }
                    attr = item.Attributes["path"];
                    if (attr != null)
                    {
                        info.Path = attr.InnerText;
                    }
                    
                    _shareInfos.Add(info);
                }
            }
            catch { }
        }

        /// <summary>
        /// 记录配置
        /// </summary>
        public void SaveConfig() 
        {
            string path = CommonMethods.GetBaseRoot("config.xml");
            XmlDocument xml = new XmlDocument();
            xml.CreateXmlDeclaration("1.0", "utf-8", "yes");
            XmlNode root = xml.CreateElement("root");

            XmlAttribute attr=xml.CreateAttribute("ip");
            attr.InnerText=_bindIP;
            root.Attributes.Append(attr);

            attr = xml.CreateAttribute("port");
            attr.InnerText = _bindPort.ToString();
            root.Attributes.Append(attr);

            xml.AppendChild(root);


            foreach (ShareInfo info in _shareInfos) 
            {
                XmlNode item = xml.CreateElement("item");
                attr = xml.CreateAttribute("name");
                attr.InnerText = info.Name;
                item.Attributes.Append(attr);

                attr = xml.CreateAttribute("path");
                attr.InnerText = info.Path;
                item.Attributes.Append(attr);
                root.AppendChild(item);
            }
            xml.Save(path);
        }
    }
}
