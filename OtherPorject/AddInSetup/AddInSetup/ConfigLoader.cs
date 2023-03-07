using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml;
using System.Windows.Forms;
using System.Diagnostics;

namespace AddInSetup
{
    /// <summary>
    /// Config加载器
    /// </summary>
    public class ConfigLoader
    {
        /// <summary>
        /// 插件名称
        /// </summary>
        public const string AddInName = "Buffalo.DBTools.AddIn";

        /// <summary>
        /// 基路径
        /// </summary>
        public static readonly string BasePath = AppDomain.CurrentDomain.BaseDirectory;
        

        private List<AddInInfo> _lstAddInInfos;
        /// <summary>
        /// 插件信息
        /// </summary>
        public List<AddInInfo> AddInInfos
        {
            get { return _lstAddInInfos; }
        }
        private List<HelpDocItem> _lstDocItems;
        /// <summary>
        /// 帮助文档项
        /// </summary>
        public List<HelpDocItem> LstDocItems
        {
            get { return _lstDocItems; }
        }

        private List<DllVerInfo> _lstDllVerInfo;
        /// <summary>
        /// 插件Dll版本信息
        /// </summary>
        public List<DllVerInfo> DllVerInfos
        {
            get { return _lstDllVerInfo; }
        }
        private List<DllItem> _lstDllItem;
        /// <summary>
        /// 插件Dll信息
        /// </summary>
        public List<DllItem> DllItems
        {
            get { return _lstDllItem; }
        }

        private DateTime _version=DateTime.MinValue;
        /// <summary>
        /// 版本日期
        /// </summary>
        public DateTime Version
        {
            get { return _version; }
        }

        /// <summary>
        /// 获取版本
        /// </summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        public static DateTime GetVersion(XmlDocument doc)
        {
            XmlNodeList rootNode = doc.GetElementsByTagName("root");
            DateTime dtRet = DateTime.MinValue;
            if (rootNode.Count > 0)
            {
                XmlNode root = rootNode[0];
                XmlAttribute sVerAttr = root.Attributes["date"];
                if (sVerAttr != null)
                {
                    string sver = sVerAttr.InnerText;
                    if (!DateTime.TryParse(sver, out dtRet))
                    {
                        dtRet= DateTime.MinValue;
                    }
                }
            }
            return dtRet;
        }

        /// <summary>
        /// 加载配置
        /// </summary>
        public void LoadConfig()
        {
            string infoFile = Path.Combine(ConfigLoader.BasePath ,"AddInConfig.xml");
            if (!File.Exists(infoFile))
            {
                return;
            }
            XmlDocument doc = new XmlDocument();
            doc.Load(infoFile);
            
            _version = GetVersion(doc);
            //插件
            XmlNodeList addinNodes = doc.GetElementsByTagName("AddIns");
            _lstAddInInfos = new List<AddInInfo>();
            if (addinNodes.Count > 0)
            {
                XmlNode addInNode = addinNodes[0];
                foreach (XmlNode node in addInNode.ChildNodes)
                {
                    AddInInfo info = AddInInfo.LoadForNode(node);
                    _lstAddInInfos.Add(info);
                }
            }

            string standardPath = null;
            //.net版本
            XmlNodeList dllVerNodes = doc.GetElementsByTagName("DllVer");
            _lstDllVerInfo = new List<DllVerInfo>();
            if (dllVerNodes.Count > 0)
            {
                XmlNode dllVerNode = dllVerNodes[0];
                foreach (XmlNode node in dllVerNode.ChildNodes)
                {
                    DllVerInfo info = DllVerInfo.LoadForNode(node);
                    if (info.IsStandard) 
                    {
                        standardPath = info.CurPath;
                    }
                    _lstDllVerInfo.Add(info);
                }
            }


            //文件
            XmlNodeList dllItemNodes = doc.GetElementsByTagName("Dlls");
            _lstDllItem = new List<DllItem>();
            if (dllItemNodes.Count > 0)
            {
                XmlNode dllItemNode = dllItemNodes[0];
                foreach (XmlNode node in dllItemNode.ChildNodes)
                {
                    DllItem info = DllItem.LoadForNode(node);
                    _lstDllItem.Add(info);
                }
            }
            RebuildStandardItem(_lstDllItem, standardPath);
            //帮助文档
            XmlNodeList docNodes = doc.GetElementsByTagName("Docs");
            _lstDocItems = new List<HelpDocItem>();
            if (docNodes.Count > 0)
            {
                XmlNode docNode = docNodes[0];
                foreach (XmlNode node in docNode.ChildNodes)
                {

                    HelpDocItem info = HelpDocItem.LoadForNode(node);
                    _lstDocItems.Add(info);
                }
            }
        }

        private void RebuildStandardItem(List<DllItem> lstItems,string standardPath) 
        {
            List<DllItemFile> lstStandards = new List<DllItemFile>();

            List<DllItem> lst = new List<DllItem>();
            foreach(DllItem dllfile in lstItems) 
            {
                dllfile.FillStandardFileInfo(lstStandards);
            }

            string[] files=Directory.GetFiles(standardPath);
            foreach(string fileName in files) 
            {
                FileInfo finfo=new FileInfo(fileName);
                foreach(DllItemFile dfile in lstStandards) 
                {
                    if (finfo.Name.IndexOf(dfile.Path) == 0) 
                    {
                        dfile.Path = finfo.Name ;
                        
                        break;
                    }
                }
            }
        }
    }
}
