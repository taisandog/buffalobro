using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using Buffalo.DB.DataBaseAdapter;
using EnvDTE;
using Microsoft.VisualStudio.Modeling.Diagrams;
using System.IO;
using Microsoft.VisualStudio.EnterpriseTools.ClassDesigner;
using Buffalo.DBTools.DocSummary;

namespace Buffalo.DBTools.HelperKernel
{
    public class DBConfigInfo
    {
        private string _dbName;

        /// <summary>
        /// 数据库名
        /// </summary>
        public string DbName
        {
            get { return _dbName; }
            set { _dbName = value; }
        }

        private string _appNamespace;
        /// <summary>
        /// 数据层所在的命名空间
        /// </summary>
        public string AppNamespace
        {
            get { return _appNamespace; }
            set { _appNamespace = value; }
        }

        private string _assembly;
        /// <summary>
        /// 程序集(一般留空)
        /// </summary>
        public string Assembly
        {
            get { return _assembly; }
            set { _assembly = value; }
        }

        private string _dbType;
        /// <summary>
        /// 数据库类型
        /// </summary>
        public string DbType
        {
            get { return _dbType; }
            set { _dbType = value; }
        }

        private string _connectionString;
        /// <summary>
        /// 数据连接字符串
        /// </summary>
        public string ConnectionString
        {
            get { return _connectionString; }
            set { _connectionString = value; }
        }
        private string _roconnectionString;
        /// <summary>
        /// 只读数据连接字符串
        /// </summary>
        public string ROConnectionString
        {
            get { return _roconnectionString; }
            set { _roconnectionString = value; }
        }
        private string _fileName;

        /// <summary>
        /// 所在文件路径
        /// </summary>
        public string FileName
        {
            get { return _fileName; }
            set { _fileName = value; }
        }

        private int _tier=1;

        /// <summary>
        /// 层数
        /// </summary>
        public int Tier
        {
            get { return _tier; }
            set { _tier = value; }
        }

        private SummaryShowItem _summaryShow=SummaryShowItem.All;

        /// <summary>
        /// 显示方式
        /// </summary>
        public SummaryShowItem SummaryShow
        {
            get { return _summaryShow; }
            set { _summaryShow = value; }
        }

        private bool _isAllDal=false;

        /// <summary>
        /// 是否生成所有数据层
        /// </summary>
        public bool IsAllDal
        {
            get { return _isAllDal; }
            set { _isAllDal = value; }
        }

        private bool _entityToDirectory=false;
        /// <summary>
        /// 实体是否放到Entity文件夹中
        /// </summary>
        public bool EntityToDirectory
        {
            get { return _entityToDirectory; }
            set { _entityToDirectory = value; }
        }

        private string _cacheType;
        /// <summary>
        /// 缓存类型
        /// </summary>
        public string CacheType
        {
            get { return _cacheType; }
            set { _cacheType = value; }
        }

        private string _cacheConnString;
        /// <summary>
        /// 缓存的连接字符串
        /// </summary>
        public string CacheConnString
        {
            get { return _cacheConnString; }
            set { _cacheConnString = value; }
        }

        private bool _isAllTable;
        /// <summary>
        /// 是否所有表都可用缓存
        /// </summary>
        public bool IsAllTable
        {
            get { return _isAllTable; }
            set { _isAllTable = value; }
        }

        private bool _addDescription;
        /// <summary>
        /// 是否添加Description标签
        /// </summary>
        public bool AddDescription
        {
            get { return _addDescription; }
            set { _addDescription = value; }
        }

        private LazyType _allowLazy=LazyType.Enable;
        /// <summary>
        /// 允许延迟加载
        /// </summary>
        public LazyType AllowLazy
        {
            get { return _allowLazy; }
            set { _allowLazy=value; }
        }
        /// <summary>
        /// 创建数据库信息
        /// </summary>
        /// <returns></returns>
        public DBInfo CreateDBInfo() 
        {
            
            DBInfo info = new DBInfo(DbName, ConnectionString,ROConnectionString, DbType,LazyType.Disable);
            return info;
        }



        /// <summary>
        /// 生成文件名
        /// </summary>
        /// <param name="curProject"></param>
        /// <param name="curDiagram"></param>
        public static string GetFileName(ClassDesignerInfo info) 
        {
            string dbName = GetDbName(info);
            string proFile = info.CurrentProject.FileName;
            FileInfo file = new FileInfo(proFile);
            string directory = file.DirectoryName;
            return directory + "\\" + dbName + ".xml";

        }
        /// <summary>
        /// 生成配置的文件名
        /// </summary>
        /// <param name="curProject"></param>
        /// <param name="curDiagram"></param>
        public static string GetConfigFileName(ClassDesignerInfo info)
        {
            string dbName = GetDbName(info);
            string proFile = info.CurrentProject.FileName;
            FileInfo file = new FileInfo(proFile);
            string directory = file.DirectoryName;
            return directory + "\\" + dbName + ".config.xml";

        }
        /// <summary>
        /// 获取当前类图的库名
        /// </summary>
        /// <param name="docView"></param>
        /// <returns></returns>
        public static string GetDbName(ClassDesignerInfo info) 
        {
            FileInfo docFile = new FileInfo(info.SelectDocView.DocData.FileName);
            string dbName = docFile.Name;
            int dot = dbName.IndexOf('.');
            if (dot > 0)
            {
                dbName = dbName.Substring(0, dot);
            }
            return dbName;
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="path"></param>
        public void SaveConfig(string path) 
        {
            XmlDocument doc = EntityMappingConfig.NewXmlDocument();
            XmlNode configNode = doc.CreateElement("config");
            doc.AppendChild(configNode);

            XmlAttribute att = doc.CreateAttribute("connectionString");
            att.InnerText = this.ConnectionString;
            configNode.Attributes.Append(att);

            att = doc.CreateAttribute("roconnectionString");
            att.InnerText = this.ROConnectionString;
            configNode.Attributes.Append(att);

            att = doc.CreateAttribute("dbType");
            att.InnerText = this.DbType;
            configNode.Attributes.Append(att);

            att = doc.CreateAttribute("appnamespace");
            att.InnerText = this.AppNamespace;
            configNode.Attributes.Append(att);

            att = doc.CreateAttribute("assembly");
            att.InnerText = this.Assembly;
            configNode.Attributes.Append(att);

            att = doc.CreateAttribute("name");
            att.InnerText = this.DbName;
            configNode.Attributes.Append(att);

            att = doc.CreateAttribute("cache");
            att.InnerText = this.CacheType;
            configNode.Attributes.Append(att);

            att = doc.CreateAttribute("cacheConnString");
            att.InnerText = this.CacheConnString;
            configNode.Attributes.Append(att);

            att = doc.CreateAttribute("allCache");
            att.InnerText = this.IsAllTable ? "1" : "0";
            configNode.Attributes.Append(att);

            att = doc.CreateAttribute("lazy");
            att.InnerText = ((int)this.AllowLazy).ToString();
            configNode.Attributes.Append(att);

            EntityMappingConfig.SaveXML(path, doc);
            SaveConfigInfo(path);
        }

        /// <summary>
        /// 保存配置信息
        /// </summary>
        private void SaveConfigInfo(string path) 
        {
            XmlDocument doc = EntityMappingConfig.NewXmlDocument();
            XmlNode configNode = doc.CreateElement("config");
            doc.AppendChild(configNode);

            XmlAttribute att = doc.CreateAttribute("tier");
            att.InnerText = this.Tier.ToString();
            configNode.Attributes.Append(att);

            att = doc.CreateAttribute("isAllDal");
            att.InnerText = this.IsAllDal ? "1" : "0";
            configNode.Attributes.Append(att);

            att = doc.CreateAttribute("summary");
            att.InnerText = ((int)this.SummaryShow).ToString();
            configNode.Attributes.Append(att);

            att = doc.CreateAttribute("entityToDirectory");
            att.InnerText = this.EntityToDirectory ? "1" : "0";
            configNode.Attributes.Append(att);

            int last = path.LastIndexOf(".xml");
            string savePath = path;
            if (last > 0)
            {
                savePath = path.Substring(0, last);
                savePath += ".config.xml";
            }
            EntityMappingConfig.SaveXML(savePath, doc);
        }
        /// <summary>
        /// 加载数据库信息
        /// </summary>
        /// <param name="curProject">当前工程</param>
        /// <param name="curDiagram">当前图</param>
        /// <returns></returns>
        public static DBConfigInfo LoadInfo(ClassDesignerInfo info) 
        {
            if (info.CurrentProject == null || info.SelectDocView == null) 
            {
                return null;
            }
            string xmlFieName = GetFileName(info);
            DBConfigInfo ret = null;
            XmlDocument doc = new XmlDocument();
            try
            {
                doc.Load(xmlFieName);
                ret = LoadInfo(doc);
                LoadConfig(info, ret);
                ret.FileName = xmlFieName;
            }
            catch 
            {
                
            }
            return ret;
        }

        /// <summary>
        /// 加载数据库配置信息
        /// </summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        public static DBConfigInfo LoadInfo(XmlDocument doc)
        {
            XmlNodeList lstConfig = doc.GetElementsByTagName("config");
            if (lstConfig.Count > 0)
            {
                DBConfigInfo info = new DBConfigInfo();
                XmlNode config = lstConfig[0];

                XmlAttribute att = config.Attributes["connectionString"];
                if (att != null)
                {
                    info.ConnectionString = att.InnerText;
                }
                att = config.Attributes["roconnectionString"];
                if (att != null)
                {
                    info.ROConnectionString = att.InnerText;
                }
                att = config.Attributes["dbType"];
                if (att != null)
                {
                    info.DbType = att.InnerText;
                }

                att = config.Attributes["appnamespace"];
                if (att != null)
                {
                    info.AppNamespace = att.InnerText;
                }
                att = config.Attributes["assembly"];
                if (att != null)
                {
                    info.Assembly = att.InnerText;
                }
                att = config.Attributes["name"];
                if (att != null)
                {
                    info.DbName = att.InnerText;
                }

                att = config.Attributes["cache"];
                if (att != null)
                {
                    info.CacheType = att.InnerText;
                }

                att = config.Attributes["cacheConnString"];
                if (att != null)
                {
                    info.CacheConnString = att.InnerText;
                }
                att = config.Attributes["allCache"];
                if (att != null)
                {
                    info.IsAllTable = att.InnerText=="1";
                }
                att = config.Attributes["lazy"];
                if (att != null)
                {
                    LazyType lazy = LazyType.User;
                    int ilazy = 0;
                    if (int.TryParse(att.InnerText, out ilazy))
                    {
                        lazy = (LazyType)ilazy;
                    }
                    info.AllowLazy = lazy;
                }
                return info;
            }
            return null;
        }
        /// <summary>
        /// 加载其他配置信息
        /// </summary>
        /// <param name="info"></param>
        private static void LoadConfig(ClassDesignerInfo cdinfo,DBConfigInfo info) 
        {
            XmlDocument doc = new XmlDocument();
            try
            {
                doc.Load(GetConfigFileName(cdinfo));
                XmlNodeList lstConfig = doc.GetElementsByTagName("config");
                if (lstConfig.Count > 0)
                {
                    XmlNode config = lstConfig[0];
                    XmlAttribute att = att = config.Attributes["tier"];
                    if (att != null)
                    {
                        int tier = 1;
                        if (int.TryParse(att.InnerText, out tier))
                        {
                            info.Tier = Convert.ToInt32(att.InnerText);
                        }
                        else
                        {
                            info.Tier = 3;
                        }
                    }

                    att = config.Attributes["isAllDal"];
                    if (att != null)
                    {
                        info.IsAllDal = (att.InnerText == "1");
                    }
                    att = config.Attributes["entityToDirectory"];
                    if (att != null)
                    {
                        info.EntityToDirectory = (att.InnerText == "1");
                    }
                    att = config.Attributes["summary"];
                    if (att != null)
                    {
                        int summary = (int)SummaryShowItem.All;
                        if (int.TryParse(att.InnerText, out summary))
                        {
                            info.SummaryShow = (SummaryShowItem)summary;
                        }
                        else
                        {
                            info.SummaryShow = SummaryShowItem.All;
                        }
                    }
                }
            }
            catch
            {

            }
        }

    }
}
