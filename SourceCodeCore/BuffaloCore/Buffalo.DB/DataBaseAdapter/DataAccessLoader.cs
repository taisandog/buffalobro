using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Reflection;
using System.Diagnostics;
using Buffalo.DB.CommBase;
using Buffalo.DB.CommBase.DataAccessBases;
using Buffalo.DB.DbCommon;
using Buffalo.Kernel.Defaults;
using Buffalo.DB.EntityInfos;
using Buffalo.Kernel.FastReflection;
using System.IO;
using Buffalo.Kernel;
using Buffalo.DB.CacheManager;
using System.Collections.Concurrent;

namespace Buffalo.DB.DataBaseAdapter
{
    /// <summary>
    /// ���ݷ��ʲ�ļ��ض�ȡ��
    /// </summary>
    public class DataAccessLoader
    {
        

        private static Dictionary<string, DBInfo> _dicDBInfo = new Dictionary<string, DBInfo>();//����ʵ���¼����
        private static Dictionary<string, Type> _dicBoLoader = null;//����ʵ��ҵ��㼯��

        private static Dictionary<Type, DBInfo> _dicEntityDBInfo = null;//����ʵ���DBInfo��ӳ��
        private static Dictionary<string, DBInfo> _dicInterfaceDBInfo = null;//����ʵ���DBInfo��ӳ��
        /// <summary>
        /// ��������
        /// </summary>
        private static List<ConfigInfo> _configdocs=new List<ConfigInfo>();
        /// <summary>
        /// �Ƿ��Ѿ���ʼ��
        /// </summary>
        internal static bool HasInit 
        {
            get 
            {
                return _dicBoLoader != null;
            }
        }

        /// <summary>
        /// ��ȡ���ݿ���Ϣ
        /// </summary>
        /// <param name="dbName">���ݿ���</param>
        /// <returns></returns>
        public static DBInfo GetDBInfo(string dbName) 
        {
            if (_dicDBInfo == null)
            {
                return null;
            }
            DBInfo ret=null;
            _dicDBInfo.TryGetValue(dbName, out ret);
            return ret;
        }

        /// <summary>
        /// ��ȡ���һ�����ݿ���Ϣ
        /// </summary>
        /// <returns></returns>
        internal static DBInfo GetFristDBInfo() 
        {
            if (_dicDBInfo == null)
            {
                return null;
            }
            foreach (KeyValuePair<string, DBInfo> kvp in _dicDBInfo) 
            {
                return kvp.Value;
            }
            return null;
        }

        #region ��ʼ������
        /// <summary>
        /// ������ݿ���Ϣ
        /// </summary>
        /// <param name="dbinfo"></param>
        public static void AppendDBInfo(DBInfo dbinfo) 
        {
            _dicDBInfo[dbinfo.Name] = dbinfo;
        }

        /// <summary>
        /// ������ݿ������ļ�
        /// </summary>
        /// <param name="configPath">�ļ�·������</param>
        public static void AddDBConfig(IEnumerable<string> configPath)
        {
            _configdocs.AddRange(ConfigXmlLoader.LoadXml(configPath));
        }
        /// <summary>
        /// ������ݿ���������
        /// </summary>
        /// <param name="configContent">��������</param>
        private void AddDBConfigContent(IEnumerable<string> configContent)
        {
            _configdocs.AddRange(ConfigXmlLoader.LoadXmlContent(configContent));
        }



        /// <summary>
        /// ��ʼ������
        /// </summary>
        internal static bool InitConfig(IEnumerable<string> configPath)
        {
            if (HasInit)
            {
                return true;
            }
            //_dicEntityLoaderConfig = new Dictionary<string, Type>();
            _dicBoLoader = new Dictionary<string, Type>();
            _dicEntityDBInfo = new Dictionary<Type, DBInfo>();
            _dicInterfaceDBInfo = new Dictionary<string, DBInfo>();
            Dictionary<string, XmlDocument> dicEntityConfig = new Dictionary<string, XmlDocument>();


            if (configPath != null)
            {
                _configdocs.AddRange(ConfigXmlLoader.LoadXml(configPath));
            }
            else
            {
                string config = System.Configuration.ConfigurationManager.AppSettings["Buffalo.Config"];
                if (CommonMethods.IsNullOrWhiteSpace(config))
                {
                    config = System.Configuration.ConfigurationManager.AppSettings["DataAccessConfig"];
                }
                if (!CommonMethods.IsNullOrWhiteSpace(config))
                {
                    string[] loadPath = config.Split('|');
                    _configdocs.AddRange(ConfigXmlLoader.LoadXml(loadPath));
                }
            }

            if (_configdocs.Count > 0)
            {

                DBInfo existsInfo = null;
                foreach (ConfigInfo doc in _configdocs)
                {
                    XmlDocument docInfo = doc.Document;
                    DBInfo dbinfo = GetDBInfo(docInfo);
                    if (!_dicDBInfo.TryGetValue(dbinfo.Name, out existsInfo))
                    {
                        _dicDBInfo[dbinfo.Name] = dbinfo;
                    }
                    else
                    {
                        if (!existsInfo.ConnectionString.Equals(dbinfo.ConnectionString))
                        {
                            throw new Exception("ͬ�����ݿ�:" + dbinfo.Name + "���������ַ�����ͬ");
                        }
                        if (!existsInfo.DbType.Equals(existsInfo.DbType))
                        {
                            throw new Exception("ͬ�����ݿ�:" + dbinfo.Name + "�������ݿ����Ͳ�ͬ");
                        }
                    }

                }

            }
            if (_dicDBInfo.Count == 0)
            {
                StringBuilder exMess = new StringBuilder();
                exMess.Append("û���������ݿ���Ϣ������");
                if (CommonMethods.IsWebContext)
                {
                    exMess.Append("web.config");
                }
                else
                {
                    exMess.Append("app.config");
                }
                exMess.Append("��appSettings���Ƿ��� Buffalo.Config �� DataAccessConfig �ڵ�");
                throw new Exception(exMess.ToString());
            }

            LoadModel();
            return true;
        }

        /// <summary>
        /// ��ȡ��ǰ�����ļ������ݿ���Ϣ
        /// </summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        public static DBInfo GetDBInfo(XmlDocument doc) 
        {
            string dbType = null;
            string connectionString = null;
            string readconnectionString = null;
            string name = null;
            //string output = null;
            string[] attNames = null;
            ICacheAdaper ica=null;
            string cacheType = null;
            string cacheConn = null;
            LazyType lazy=LazyType.Enable;
            //Dictionary<string, string> extendDatabaseConnection = new Dictionary<string,string>();
            if (doc == null)
            {
                throw new Exception("�Ҳ��������ļ�");
            }
            bool isAlltable = false ;
            XmlNodeList lstConfig = doc.GetElementsByTagName("config");
            if (lstConfig.Count > 0)
            {
                XmlNode node = lstConfig[0];
                foreach (XmlAttribute att in node.Attributes)
                {
                    if (att.Name.Equals("dbType",StringComparison.CurrentCultureIgnoreCase))
                    {
                        dbType = att.InnerText;
                    }
                    //else if (att.Name.Equals("output", StringComparison.CurrentCultureIgnoreCase))
                    //{
                    //    output = att.InnerText;
                    //}
                    else if (att.Name.Equals("name",StringComparison.CurrentCultureIgnoreCase))
                    {
                        name=att.InnerText;
                    }
                    else if (att.Name.Equals("connectionString",StringComparison.CurrentCultureIgnoreCase))
                    {
                        connectionString = att.InnerText;
                    }
                    else if (att.Name.Equals("roconnectionString", StringComparison.CurrentCultureIgnoreCase))
                    {
                        readconnectionString = att.InnerText;
                    }
                    else if (att.Name.Equals("appnamespace",StringComparison.CurrentCultureIgnoreCase)) 
                    {
                        string names=att.InnerText;

                        if (!string.IsNullOrEmpty(names))
                        {
                            attNames = names.Split(new char[] { '|' });
                        }
                        for (int i = 0; i < attNames.Length; i++) 
                        {
                            string attName = attNames[i];
                            if (!attName.EndsWith(".")) 
                            {
                                attNames[i] = attName + ".";
                            }
                        }
                    }
                    else if (att.Name.Equals("cache", StringComparison.CurrentCultureIgnoreCase)) 
                    {
                        cacheType = att.InnerText;
                    }
                    else if (att.Name.Equals("cacheConnString", StringComparison.CurrentCultureIgnoreCase))
                    {
                        cacheConn = att.InnerText;
                    }
                    else if (att.Name.Equals("allCache", StringComparison.CurrentCultureIgnoreCase))
                    {
                        isAlltable = att.InnerText=="1";
                    }
                    else if (att.Name.Equals("lazy", StringComparison.CurrentCultureIgnoreCase))
                    {
                        int ilazy=0;
                        if(int.TryParse(att.InnerText, out ilazy))
                        {
                            lazy=(LazyType)ilazy;
                        }
                    }
                }
            }
            else
            {
                throw new Exception("�����ļ�û��config�ڵ�");
            }
            
            DBInfo info = new DBInfo(name, connectionString, readconnectionString, dbType,lazy);
            
            ica = QueryCache.GetCache(info,cacheType, cacheConn);

            info.SetQueryCache(ica,isAlltable);
            
            info.DataaccessNamespace=attNames;
            return info;
        }


        public const string EnitityNameEnd = ".ETM.xml";
        public const string EnitityOldNameEnd = ".BEM.xml";
        /// <summary>
        /// ����ģ����Ϣ
        /// </summary>
        private static void LoadModel()
        {
            List<Assembly> lstAss = GetAllAssembly();
            Dictionary<string, EntityConfigInfo> dicAllEntityInfo = new Dictionary<string, EntityConfigInfo>();//ʵ����Ϣ
            
            foreach (Assembly ass in lstAss) 
            {
                
                string[] resourceNames = ass.GetManifestResourceNames();
                foreach (string name in resourceNames)
                {
                    bool endOldName=name.EndsWith(EnitityOldNameEnd, StringComparison.CurrentCultureIgnoreCase);

                    bool endName=name.EndsWith(EnitityNameEnd, StringComparison.CurrentCultureIgnoreCase);
                    if (endOldName || endName)
                    {
                        try
                        {
                            Stream stm = ass.GetManifestResourceStream(name);
                            XmlDocument doc = new XmlDocument();
                            doc.Load(stm);

                            //��ȡ����
                            XmlNodeList lstNode = doc.GetElementsByTagName("class");
                            if (lstNode.Count > 0)
                            {
                                XmlNode classNode = lstNode[0];
                                XmlAttribute att = classNode.Attributes["ClassName"];
                                if (att != null)
                                {
                                    string className = att.InnerText;
                                    if (!string.IsNullOrEmpty(className))
                                    {
                                        Type cType = ass.GetType(className);

                                        EntityConfigInfo info = new EntityConfigInfo();
                                        info.Type = cType;
                                        info.ConfigXML = doc;
                                        if (endOldName) 
                                        {
                                            if (dicAllEntityInfo.ContainsKey(className)) 
                                            {
                                                continue;
                                            }
                                        }
                                        dicAllEntityInfo[className] = info;

                                    }
                                }
                                
                            }
                        }
                        catch
                        {

                        }
                    }
                    else if (name.EndsWith(".BDM.xml", StringComparison.CurrentCultureIgnoreCase))
                    {
                        Stream stm = ass.GetManifestResourceStream(name);
                        XmlDocument doc = new XmlDocument();
                        doc.Load(stm);
                        AppendDalLoader(ass, doc);
                        AppendBoLoader(ass, doc);
                    }
                }
            }
            EntityInfoManager.InitAllEntity(dicAllEntityInfo);

        }
        /// <summary>
        /// ��ӵ����ݲ�
        /// </summary>
        /// <param name="doc"></param>
        private static void AppendDalLoader(Assembly ass, XmlDocument doc) 
        {
            XmlNodeList nodes = doc.GetElementsByTagName("dataaccess");
            if (nodes.Count <= 0) 
            {
                return;
            }
            XmlAttribute att = nodes[0].Attributes["name"];
            if (att == null) 
            {
                return;
            }
            string name = att.InnerText;
            DBInfo db = null;
            if (!_dicDBInfo.TryGetValue(name, out db)) 
            {
                return;
            }
            Dictionary<string, List<DBInfo>> namespaces = new Dictionary<string, List<DBInfo>>();
            foreach(string cnamespace in db.DataaccessNamespace) 
            {
                AppendToColl(db,cnamespace, namespaces);
            }
            
            ConcurrentDictionary<int, DBInfo> dic = db.GetAllChildDBInfo();
            if (dic != null) 
            {
                foreach (KeyValuePair<int, DBInfo> kvp in dic) 
                {
                    foreach (string cnamespace in kvp.Value.DataaccessNamespace)
                    {
                        AppendToColl(kvp.Value, cnamespace, namespaces); 
                    }
                }
            }
            
            XmlNodeList dalNodes = nodes[0].ChildNodes;
            foreach (XmlNode dalNode in dalNodes) 
            {
                att = dalNode.Attributes["type"];
                if (att == null) 
                {
                    continue;
                }
                string typeName = att.InnerText;
                List<DBInfo> lstDB = null;
                foreach (KeyValuePair<string, List<DBInfo>> kvp in namespaces) 
                {
                    string allNameSpace = kvp.Key;
                    if (typeName.StartsWith(allNameSpace)) 
                    {
                        lstDB = kvp.Value;
                        Type dalType = ass.GetType(typeName);
                        if (dalType != null) 
                        {
                            att = dalNode.Attributes["interface"];
                            if (att == null)
                            {
                                break;
                            }

                            Type[] gTypes = DefaultType.GetGenericType(dalType, true);

                            string interfaceName = att.InnerText;

                            foreach (DBInfo curDB in lstDB)
                            {
                                
                                curDB.DataaccessInterfaceMapping[interfaceName] = dalType;
                                if (curDB.ChildKey < 0) //������Դ
                                {
                                    _dicInterfaceDBInfo[interfaceName] = curDB;
                                }

                                if (gTypes != null && gTypes.Length > 0)
                                {
                                    Type gType = gTypes[0];
                                    curDB.DataaccessEntityMapping[gType] = dalType;
                                    if (curDB.ChildKey < 0) //������Դ
                                    {
                                        _dicEntityDBInfo[gType] = curDB;
                                    }
                                }
                                
                            }
                        }

                        break;
                    }
                }
            }
        }


        private static void AppendToColl(DBInfo info,string key, Dictionary<string, List<DBInfo>> dic) 
        {
            List<DBInfo> lst = null;
            if(!dic.TryGetValue(key,out lst)) 
            {
                lst = new List<DBInfo>();
                dic[key] = lst;
            }
            lst.Add(info);
        }

        /// <summary>
        /// ��ӵ�ҵ���
        /// </summary>
        /// <param name="doc"></param>
        private static void AppendBoLoader(Assembly ass, XmlDocument doc)
        {
            XmlNodeList nodes = doc.GetElementsByTagName("business");
            if (nodes.Count <= 0)
            {
                return;
            }
            XmlNodeList dalNodes = nodes[0].ChildNodes;
            foreach (XmlNode dalNode in dalNodes)
            {
                XmlAttribute att = dalNode.Attributes["type"];
                if (att == null)
                {
                    continue;
                }
                string typeName = att.InnerText;
                Type boType = ass.GetType(typeName);
                if (boType != null)
                {
                    att = dalNode.Attributes["class"];
                    if (att == null)
                    {
                        break;
                    }
                    _dicBoLoader[att.InnerText] = boType;
                }
            }
        }

        private static List<Assembly> _modelAssembly = new List<Assembly>();

        /// <summary>
        /// ���Ҫ����ĳ���
        /// </summary>
        /// <param name="ass">����</param>
        public static void AppendModelAssembly(Assembly ass) 
        {
            
            string key=ass.FullName;
            foreach (Assembly curAss in _modelAssembly) 
            {
                string curKey = curAss.FullName;
                if (key == curKey) 
                {
                    return;
                }
            }
            _modelAssembly.Add(ass);
        }
        /// <summary>
        /// ��ȡ��ģ�������г���
        /// </summary>
        /// <returns></returns>
        private static List<Assembly> GetAllAssembly() 
        {
            return _modelAssembly;
        }

        #endregion

        /// <summary>
        /// ����ʵ�����ͻ�ȡ��Ӧ�����ݷ��ʲ��ʵ��
        /// </summary>
        /// <param name="entityType">ʵ������</param>
        /// <returns></returns>
        public static object GetInstanceByEntity(Type entityType, DataBaseOperate oper)
        {
            //InitConfig();
            Type objType = null;
            DBInfo info = null;
            if(!_dicEntityDBInfo.TryGetValue(entityType,out info)) 
            {
                throw new Exception("�Ҳ�����Ӧ������" + entityType.FullName + "�����ã����������ļ�");
            }

            if (!info.DataaccessEntityMapping.TryGetValue(entityType, out objType))
            {
                throw new Exception("�Ҳ�����Ӧ������" + entityType.FullName+"�����ã����������ļ�");
            }
            return Activator.CreateInstance(objType, oper);
        }

        /// <summary>
        /// ����ʵ�����ͻ�ȡ��Ӧ�����ݷ��ʲ��ʵ��
        /// </summary>
        /// <param name="entityType">ʵ������</param>
        /// <returns></returns>
        public static IDataAccessModel<T> GetInstanceByEntity<T>(DataBaseOperate oper)
                    where T : EntityBase, new()
        {

            return GetInstanceByEntity(typeof(T),oper) as IDataAccessModel<T>;
        }
        /// <summary>
        /// ����ʵ�����ͻ�ȡ��Ӧ�����ݷ��ʲ��ʵ��
        /// </summary>
        /// <param name="entityType">ʵ������</param>
        /// <returns></returns>
        public static IViewDataAccess<T> GetViewInstanceByEntity<T>(DataBaseOperate oper)
                    where T : EntityBase, new()
        {
            return GetInstanceByEntity(typeof(T), oper) as IViewDataAccess<T>;
        }
        /// <summary>
        /// ����ʵ�����ͻ�ȡ��Ӧ�����ݷ��ʲ��ʵ��
        /// </summary>
        /// <param name="interfaceType">ʵ������</param>
        /// <returns></returns>
        public static object GetInstance(Type interfaceType, DataBaseOperate oper) 
        {
            //InitConfig();
            Type objType = null;
            if (!oper.DBInfo.DataaccessInterfaceMapping.TryGetValue(interfaceType.FullName, out objType)) 
            {
                throw new Exception("�Ҳ����ӿ�" + interfaceType.FullName + "�Ķ�Ӧ���ã����������ļ�");
            }
            return Activator.CreateInstance(objType, oper);
        }
       
        /// <summary>
        /// ����ʵ�����ͻ�ȡ��Ӧ�����ݷ��ʲ��ʵ��
        /// </summary>
        /// <param name="DataBaseOperate">����</param>
        /// <returns></returns>
        public static T GetInstance<T>(DataBaseOperate oper)
        {
            return (T)GetInstance(typeof(T),oper);
        }



        /// <summary>
        /// ����ʵ�����ͻ�ȡ��Ӧ�����ݷ��ʲ��ʵ��
        /// </summary>
        /// <param name="entityType">ʵ������</param>
        /// <returns></returns>
        public static object GetInstanceByEntity(Type entityType)
        {
            //InitConfig();
            Type objType = null;
            DBInfo info = null;
            if(!_dicEntityDBInfo.TryGetValue(entityType,out info)) 
            {
                throw new Exception("�Ҳ�����Ӧ������");
            }



            if (!info.SelectedDataaccessEntityMapping.TryGetValue(entityType, out objType))
            {
                throw new Exception("�Ҳ�����Ӧ������");
            }
            return Activator.CreateInstance(objType);
        }

        /// <summary>
        /// ����ʵ�����ͻ�ȡ��Ӧ�����ݷ��ʲ��ʵ��
        /// </summary>
        /// <param name="entityType">ʵ������</param>
        /// <returns></returns>
        public static IDataAccessModel<T> GetInstanceByEntity<T>()
                    where T : EntityBase, new()
        {

            return GetInstanceByEntity(typeof(T)) as IDataAccessModel<T>;
        }
        /// <summary>
        /// ����ʵ�����ͻ�ȡ��Ӧ�����ݷ��ʲ��ʵ��
        /// </summary>
        /// <param name="entityType">ʵ������</param>
        /// <returns></returns>
        public static IViewDataAccess<T> GetViewInstanceByEntity<T>()
                    where T : EntityBase, new()
        {
            return GetInstanceByEntity(typeof(T)) as IViewDataAccess<T>;
        }
        /// <summary>
        /// ����ʵ�����ͻ�ȡ��Ӧ�����ݷ��ʲ��ʵ��
        /// </summary>
        /// <param name="interfaceType">ʵ������</param>
        /// <returns></returns>
        public static object GetInstance(Type interfaceType)
        {
            //InitConfig();
            Type objType = null;
            DBInfo info = null;
            if (!_dicInterfaceDBInfo.TryGetValue(interfaceType.FullName, out info))
            {
                throw new Exception("�Ҳ�����Ӧ������" + interfaceType.FullName + "�����ã����������ļ�");
            }

            if (!info.SelectedDataaccessInterfaceMapping.TryGetValue(interfaceType.FullName, out objType))
            {
                throw new Exception("�Ҳ�����Ӧ������");
            }
            return Activator.CreateInstance(objType);
        }

        /// <summary>
        /// ��ȡҵ��������
        /// </summary>
        /// <param name="classTypeName">ʵ��������</param>
        /// <returns></returns>
        public static Type GetBoType(string classTypeName) 
        {
            Type objType = null;
            if (!_dicBoLoader.TryGetValue(classTypeName, out objType))
            {
                throw new Exception("�Ҳ�����Ӧ������");
            }
            return objType;
        }
        /// <summary>
        ///  ��ȡҵ����ʵ��
        /// </summary>
        /// <param name="classTypeName">ʵ��������</param>
        /// <returns></returns>
        public static object GetBoInstance(string classTypeName) 
        {
            Type type = GetBoType(classTypeName);
            if (type == null) 
            {
                return null;
            }
            return Activator.CreateInstance(type);
        }

        /// <summary>
        /// ����ʵ�����ͻ�ȡ��Ӧ�����ݷ��ʲ��ʵ��
        /// </summary>
        /// <param name="DataBaseOperate">����</param>
        /// <returns></returns>
        public static T GetInstance<T>()
        {
            return (T)GetInstance(typeof(T));
        }
    }
}
