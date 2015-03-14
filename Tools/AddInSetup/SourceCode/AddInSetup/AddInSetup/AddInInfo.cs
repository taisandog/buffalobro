using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml;

namespace AddInSetup
{
    /// <summary>
    /// 插件信息
    /// </summary>
    public class AddInInfo
    {
        private string _fileName;
        /// <summary>
        /// 文件路径
        /// </summary>
        public string FileName
        {
            get { return _fileName; }
        }

        private string _version;

        /// <summary>
        /// 版本号
        /// </summary>
        public string Version
        {
            get { return _version; }
        }

        private string _name;

        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            get { return _name; }
        }

        private bool _isSetup;
        /// <summary>
        /// 是否安装
        /// </summary>
        public bool IsSetup
        {
            get { return _isSetup; }
        }

        /// <summary>
        /// 按钮文本
        /// </summary>
        public string ButtonText 
        {
            get 
            {
                if (_isSetup) 
                {
                    return "卸载";
                }
                return "安装";
            }
        }
        /// <summary>
        /// 插件名称
        /// </summary>
        public const string AddInName = "Buffalo.DBTools.AddIn";

        /// <summary>
        /// 基路径
        /// </summary>
        private static readonly string _basePath = AppDomain.CurrentDomain.BaseDirectory.TrimEnd('\\');
        /// <summary>
        /// 我的文档路径
        /// </summary>
        private string _myDocumentPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal).TrimEnd('\\');
        /// <summary>
        /// 安装
        /// </summary>
        /// <returns></returns>
        public string Install() 
        {
            if (IsInstall()) 
            {
                return null;
            }
            
            string fileName = _basePath + "\\" + _fileName;
            if (!File.Exists(fileName)) 
            {
                return "不存在文件:" + fileName;
            }
            string fileContent = AddInSetup.Properties.Resources.DBTools;

            fileContent=fileContent.Replace("<%=Version%>", Version);
            fileContent=fileContent.Replace("<%=FileName%>", fileName);

            string dic=GetAddInDirectory();
            if (!Directory.Exists(dic)) 
            {
                Directory.CreateDirectory(dic);
            }
            File.WriteAllText(GetAddInFileName(), fileContent, Encoding.BigEndianUnicode);
            _isSetup = true;
            return null;
        }



        /// <summary>
        /// 反安装
        /// </summary>
        /// <returns></returns>
        public string UnInstall() 
        {
            if (IsInstall()) 
            {
                File.Delete(GetAddInFileName());
            }
            _isSetup = false;
            return null;
        }
        /// <summary>
        /// 获取插件的所在路径
        /// </summary>
        /// <returns></returns>
        private string GetAddInFileName()
        {
            return GetAddInDirectory() + AddInName;
        }
        /// <summary>
        /// 获取插件的文件夹路径
        /// </summary>
        /// <returns></returns>
        private string GetAddInDirectory() 
        {
            return _myDocumentPath + "\\" + _name + "\\AddIns\\";
        }

        /// <summary>
        /// 判断是否已经安装
        /// </summary>
        /// <returns></returns>
        public bool IsInstall() 
        {
            string addInFile = GetAddInFileName();
            return File.Exists(addInFile);
        }

        /// <summary>
        /// 获取插件信息
        /// </summary>
        /// <returns></returns>
        public static List<AddInInfo> GetAddInInfo() 
        {
            string infoFile = _basePath + "\\AddInConfig.xml";
            if (!File.Exists(infoFile)) 
            {
                return null;
            }
            XmlDocument doc = new XmlDocument();
            doc.Load(infoFile);
            XmlNodeList addinNodes = doc.GetElementsByTagName("AddIn");
            List<AddInInfo> lstAddIn = new List<AddInInfo>();
            foreach (XmlNode node in addinNodes)
            {
                AddInInfo info = new AddInInfo();
                XmlAttribute att = node.Attributes["file"];
                if (att != null) 
                {
                    info._fileName = att.InnerText;
                }
                att = node.Attributes["version"];
                if (att != null)
                {
                    info._version = att.InnerText;
                }
                att = node.Attributes["addin"];
                if (att != null)
                {
                    info._name = att.InnerText;
                }
                info._isSetup = info.IsInstall();
                lstAddIn.Add(info);
            }
            return lstAddIn;
        }
    }
}
