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
        /// ���ݿ���
        /// </summary>
        public string DbName
        {
            get { return _dbName; }
            set { _dbName = value; }
        }

        private string _appNamespace;
        /// <summary>
        /// ���ݲ����ڵ������ռ�
        /// </summary>
        public string AppNamespace
        {
            get { return _appNamespace; }
            set { _appNamespace = value; }
        }

        private string _assembly;
        /// <summary>
        /// ����(һ������)
        /// </summary>
        public string Assembly
        {
            get { return _assembly; }
            set { _assembly = value; }
        }

        private string _dbType;
        /// <summary>
        /// ���ݿ�����
        /// </summary>
        public string DbType
        {
            get { return _dbType; }
            set { _dbType = value; }
        }

        private string _connectionString;
        /// <summary>
        /// ���������ַ���
        /// </summary>
        public string ConnectionString
        {
            get { return _connectionString; }
            set { _connectionString = value; }
        }
        private string _roconnectionString;
        /// <summary>
        /// ֻ�����������ַ���
        /// </summary>
        public string ROConnectionString
        {
            get { return _roconnectionString; }
            set { _roconnectionString = value; }
        }
        private string _fileName;

        /// <summary>
        /// �����ļ�·��
        /// </summary>
        public string FileName
        {
            get { return _fileName; }
            set { _fileName = value; }
        }

        private int _tier=1;

        /// <summary>
        /// ����
        /// </summary>
        public int Tier
        {
            get { return _tier; }
            set { _tier = value; }
        }

        private SummaryShowItem _summaryShow=SummaryShowItem.All;

        /// <summary>
        /// ��ʾ��ʽ
        /// </summary>
        public SummaryShowItem SummaryShow
        {
            get { return _summaryShow; }
            set { _summaryShow = value; }
        }

        private bool _isAllDal=false;

        /// <summary>
        /// �Ƿ������������ݲ�
        /// </summary>
        public bool IsAllDal
        {
            get { return _isAllDal; }
            set { _isAllDal = value; }
        }

        private bool _entityToDirectory=false;
        /// <summary>
        /// ʵ���Ƿ�ŵ�Entity�ļ�����
        /// </summary>
        public bool EntityToDirectory
        {
            get { return _entityToDirectory; }
            set { _entityToDirectory = value; }
        }

        private string _cacheType;
        /// <summary>
        /// ��������
        /// </summary>
        public string CacheType
        {
            get { return _cacheType; }
            set { _cacheType = value; }
        }

        private string _cacheConnString;
        /// <summary>
        /// ����������ַ���
        /// </summary>
        public string CacheConnString
        {
            get { return _cacheConnString; }
            set { _cacheConnString = value; }
        }

        private bool _isAllTable;
        /// <summary>
        /// �Ƿ����б����û���
        /// </summary>
        public bool IsAllTable
        {
            get { return _isAllTable; }
            set { _isAllTable = value; }
        }

        private bool _addDescription;
        /// <summary>
        /// �Ƿ����Description��ǩ
        /// </summary>
        public bool AddDescription
        {
            get { return _addDescription; }
            set { _addDescription = value; }
        }

        private LazyType _allowLazy=LazyType.Enable;
        /// <summary>
        /// �����ӳټ���
        /// </summary>
        public LazyType AllowLazy
        {
            get { return _allowLazy; }
            set { _allowLazy=value; }
        }
        /// <summary>
        /// �������ݿ���Ϣ
        /// </summary>
        /// <returns></returns>
        public DBInfo CreateDBInfo() 
        {
            
            DBInfo info = new DBInfo(DbName, ConnectionString,ROConnectionString, DbType,LazyType.Disable);
            return info;
        }



        /// <summary>
        /// �����ļ���
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
        /// �������õ��ļ���
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
        /// ��ȡ��ǰ��ͼ�Ŀ���
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
        /// ����
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
        /// ����������Ϣ
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
        /// �������ݿ���Ϣ
        /// </summary>
        /// <param name="curProject">��ǰ����</param>
        /// <param name="curDiagram">��ǰͼ</param>
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
        /// �������ݿ�������Ϣ
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
        /// ��������������Ϣ
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
