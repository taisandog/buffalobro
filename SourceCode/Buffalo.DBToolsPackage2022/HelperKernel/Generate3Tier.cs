using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Microsoft.VisualStudio.EnterpriseTools.ArtifactModel.Clr;
using Buffalo.DBTools.ROMHelper;
using EnvDTE;
using Buffalo.Win32Kernel;
using Buffalo.DBTools.UIHelper;
using Buffalo.DBToolsPackage;
using DBTools;

namespace Buffalo.DBTools.HelperKernel
{
    /// <summary>
    /// �������ݲ�
    /// </summary>
    public class Generate3Tier : GrneraterBase
    {
        DataAccessMappingConfig dmt = null;
        
        public Generate3Tier(EntityConfig entity) 
            :base(entity)
        {
            dmt = new DataAccessMappingConfig(entity);
        }

        public Generate3Tier(DBEntityInfo entity,ClassDesignerInfo info)
            : base(entity, info)
        {
            dmt = new DataAccessMappingConfig(entity, info);
        }

        /// <summary>
        /// ����ҵ���
        /// </summary>
        /// <param name="entity"></param>
        public void GenerateBusiness() 
        {
            Microsoft.VisualStudio.Shell.ThreadHelper.ThrowIfNotOnUIThread();
            FileInfo info = new FileInfo(ClassDesignerFileName);
            

            string dicPath = info.DirectoryName + "\\Business";
            if (!Directory.Exists(dicPath)) 
            {
                Directory.CreateDirectory(dicPath);
            }
            string fileName = dicPath + "\\" + ClassName + "Business.cs";
            if (File.Exists(fileName)) 
            {
                return;
            }


            string model = Models.business;
            
            string baseClass = null;
            
            string businessClassName=ClassName+"Business";

            if (EntityConfig.IsSystemTypeName(EntityBaseTypeName))
            {
                baseClass = "BusinessModelBase";
            }
            else
            {
                baseClass = BaseNamespace + ".Business." + EntityBaseTypeShortName + "BusinessBase";
            }
            
            

            List<string> codes = new List<string>();
            TagManager tag = new TagManager();

            using (StringReader reader = new StringReader(model))
            {
                string tmp = null;
                while ((tmp = reader.ReadLine()) != null)
                {
                    if (tmp.StartsWith("<%#IF TableName%>"))
                    {
                        tag.AddTag("TableName");
                    }
                    else if (tmp.StartsWith("<%#ENDIF%>"))
                    {
                        tag.PopTag();
                    }
                    else
                    {
                        if (tag.CurrentTag == "TableName" && string.IsNullOrEmpty(Table.TableName))
                        {
                            continue;
                        }
                        tmp = tmp.Replace("<%=EntityNamespace%>", EntityNamespace);
                        tmp = tmp.Replace("<%=Summary%>", Table.Description);
                        tmp = tmp.Replace("<%=BusinessClassName%>", businessClassName);
                        tmp = tmp.Replace("<%=ClassName%>", ClassName);
                        tmp = tmp.Replace("<%=BusinessNamespace%>", BusinessNamespace);
                        tmp = tmp.Replace("<%=BaseBusinessClass%>", baseClass);
                        codes.Add(tmp);
                    }
                }
            }
            CodeFileHelper.SaveFile(fileName, codes);
            
            EnvDTE.ProjectItem newit = DesignerInfo.CurrentProject.ProjectItems.AddFromFile(fileName);
            newit.Properties.Item("BuildAction").Value = (int)BuildAction.Code;
        }

        /// <summary>
        /// ���ݲ������
        /// </summary>
        public static readonly ComboBoxItemCollection DataAccessTypes = InitItems();

        /// <summary>
        /// ���ݲ������
        /// </summary>
        public static readonly ComboBoxItemCollection CacheTypes = InitCacheItems();

        //private static Dictionary<string, string> _dicConnString = InitConnStrings();
        ///// <summary>
        ///// ��ʼ���ο��ַ���
        ///// </summary>
        ///// <returns></returns>
        //private static Dictionary<string, string> InitConnStrings() 
        //{
        //    Dictionary<string, string> dic = new Dictionary<string, string>();
        //    dic["Sql2K"] = "server={server};database={database};uid={username};pwd={pwd}";
        //    dic["Sql2K5"] = "server={server};database={database};uid={username};pwd={pwd}";
        //    dic["Sql2K8"] = "server={server};database={database};uid={username};pwd={pwd}";
        //    dic["Oracle9"] = "server={server};user id={username};password={pwd}";
        //    dic["MySQL5"] = "User ID={username};Password={pwd};Host={server};Port=3306;Database={database};charset=utf8";
        //    dic["SQLite"] = "Data Source={databasePath}";
        //    dic["DB2v9"] = "server={server}:50000;DATABASE ={database};UID={username};PWD={pwd}";
        //    dic["Psql9"] = "Server={server};Port=5432;User Id={username};Password={pwd};Database={database}";
        //    return dic;
        //}

        ///// <summary>
        ///// ��ȡ�ο��ַ���
        ///// </summary>
        ///// <param name="dbType">���ݿ�����</param>
        ///// <returns></returns>
        //public static string GetConnString(string dbType) 
        //{
        //    string conn = null;
        //    if(_dicConnString.TryGetValue(dbType,out conn))
        //    {
        //        return conn;
        //    }
        //    return null;
        //}

        /// <summary>
        /// ��ʼ�����ݿ�����
        /// </summary>
        /// <returns></returns>
        private static ComboBoxItemCollection InitCacheItems()
        {
            ComboBoxItemCollection types = new ComboBoxItemCollection();
            ComboBoxItem item = null;
            item = new ComboBoxItem("��", "");
            item.Tag = "";
            types.Add(item);

            item = new ComboBoxItem("�ڴ�", "system");
            item.Tag = "";
            types.Add(item);

            item = new ComboBoxItem("ASP.NET Cache", "web");
            item.Tag = "expir=30";
            types.Add(item);

#if (NET_2_0)
#else
            item = new ComboBoxItem("Memcached", "memcached");
            item.Tag = "server=127.0.0.1:11211,127.0.0.1:11212;expir=30;poolsize=30;throw=0";
            types.Add(item);
            item = new ComboBoxItem("Redis", "redis");
            item.Tag = "server=127.0.0.1:6379,127.0.0.1:6380;readserver=127.0.0.1:6381,127.0.0.1:6382;expir=30;poolsize=30;throw=0";
            types.Add(item);
#endif



            return types;
        }
#if (NET_2_0)
        private static string version = "2.0";
        private static string orcVer = "2.0";
#elif (NET_3_0)
        private static string version = "3.0";
        private static string orcVer = "2.0";
#elif (NET_3_5)
        private static string version = "3.5";
        private static string orcVer = "2.0";
#elif (NET_4_0)
        private static string version = "4.0";
        private static string orcVer = "4.0";
#elif (NET_4_5)
        private static string version = "4.5";
        private static string orcVer = "4.0";
#elif (NET_4_5_1)
        private static string version = "4.5.1";
        private static string orcVer = "4.0";
#elif (NET_4_6)
        private static string version = "4.6";
        private static string orcVer = "4.0";
#elif (NET_4_6_2)
        private static string version = "4.6.2";
        private static string orcVer = "4.0";
#elif (NET_4_7_2)
        private static string version = "4.7.2";
        private static string orcVer = "4.0";
#elif (NET_4_8)
        private static string version = "4.8";
        private static string orcVer = "4.0";
#endif
        /// <summary>
        /// ��ȡOracle���ݿ�ı�ע
        /// </summary>
        /// <param name="oracleVersion">oracle�汾</param>
        /// <returns></returns>
        private static string GetOracleSummary(string oracleVersion,string oraOps) 
        {
            StringBuilder sbRet = new StringBuilder();
            sbRet.AppendLine("�������ʹ��ODAC����Oracle");
            sbRet.AppendLine("����Ҫ�� BuffaloCode\\dll\\Buffalo\\Net" + version + "\\Oracle" + oracleVersion + "\\ Ŀ¼�µ� Buffalo.Data.Oracle" + oracleVersion + ".dll��Oracle.DataAccess.dll�� OraOps" + oraOps + ".dll");
            sbRet.AppendLine("���� BuffaloResource\\OracleODAC\\Oracle" + oracleVersion + "\\X86\\ Ŀ¼�µ� oci.dll ��oraociei" + oracleVersion + ".dll");
            sbRet.AppendLine("������ BuffaloCode\\Tools\\AddInSetup\\AddIns\\Net" + version + "\\ Ŀ¼��");
            return sbRet.ToString();
        }
        /// <summary>
        /// ��ʼ�����ݿ�����
        /// </summary>
        /// <returns></returns>
        private static ComboBoxItemCollection InitItems() 
        {
            ComboBoxItemCollection types = new ComboBoxItemCollection();
            ComboBoxItem item = new ComboBoxItem("SQL Server 2000", "Sql2K");
            item.Tag = new ComboBoxItem("server=127.0.0.1;database=mydb;uid=sa;pwd=sa", null);
            types.Add(item);

            item = new ComboBoxItem("SQL Server 2005", "Sql2K5");
            item.Tag = new ComboBoxItem("server=127.0.0.1;database=mydb;uid=sa;pwd=sa", null);
            types.Add(item);

            item = new ComboBoxItem("SQL Server 2008", "Sql2K8");
            item.Tag = new ComboBoxItem("server=127.0.0.1;database=mydb;uid=sa;pwd=sa", null);
            types.Add(item);

            item = new ComboBoxItem("SQL Server 2012 ������", "Sql2K12");
            item.Tag = new ComboBoxItem("server=127.0.0.1;database=mydb;uid=sa;pwd=sa", null);
            types.Add(item);

            item = new ComboBoxItem("Oracle 9 ������", "Oracle9");
            item.Tag = new ComboBoxItem("Data Source=ORCL;user id=username;password=pwd", GetOracle9Summary());
            types.Add(item);

            item = new ComboBoxItem("Oracle 9 ������ ODP.Net", "Buffalo.Data.Oracle:9");
            item.Tag = new ComboBoxItem("user id=system;password=123456;data source=//127.0.0.1:1521/orcl", GetOracleSummary("11", "11w"));
            types.Add(item);

            item = new ComboBoxItem("Oracle 11 ������ ODP.Net", "Buffalo.Data.Oracle:11");
            item.Tag = new ComboBoxItem("user id=system;password=123456;data source=//127.0.0.1:1521/orcl", GetOracleSummary("12", "12"));
            types.Add(item);

            item = new ComboBoxItem("MySQL 5.0 ������", "Buffalo.Data.MySQL");
            item.Tag = new ComboBoxItem("User ID=root;Password=pwd;Host=127.0.0.1;Port=3306;Database=mydb;", null);
            types.Add(item);

            item = new ComboBoxItem("SQLite", "Buffalo.Data.SQLite");
            item.Tag = new ComboBoxItem("Data Source=D:\\db.s3db", null);
            types.Add(item);

            item = new ComboBoxItem("IBM DB2 v9������", "Buffalo.Data.DB2");
            item.Tag = new ComboBoxItem("server=127.0.0.1:50000;DATABASE =mydb;UID=DB2Admin;PWD=pwd", "��Ҫ��clidriver.zip�ŵ�IBM.Data.DB2.dll��ͬĿ¼");
            types.Add(item);

            item = new ComboBoxItem("Postgresql9������", "Buffalo.Data.PostgreSQL");
            item.Tag = new ComboBoxItem("Server=127.0.0.1;Port=5432;User Id=postgres;Password=pwd;Database=mydb", null);
            types.Add(item);

            item = new ComboBoxItem("Access", "Access");
            item.Tag = new ComboBoxItem("Provider=Microsoft.Jet.OLEDB.4.0; Data Source=c:\\db.mdb; Jet OLEDB:Database Password=pwd", null);
            types.Add(item);


            return types;
        }

        public static readonly ComboBoxItemCollection Tiers = InitTiers();
        /// <summary>
        /// ��ʼ���ܹ�����
        /// </summary>
        /// <returns></returns>
        private static ComboBoxItemCollection InitTiers()
        {
            ComboBoxItemCollection tiers = new ComboBoxItemCollection();
            ComboBoxItem item = new ComboBoxItem("����ܹ�", 3);
            tiers.Add(item);
            item = new ComboBoxItem("����ܹ�", 1);
            tiers.Add(item);

            return tiers;
        }
        /// <summary>
        /// ��ȡOracle���ݿ�ı�ע
        /// </summary>
        /// <param name="oracleVersion">oracle�汾</param>
        /// <returns></returns>
        private static string GetOracle9Summary()
        {
            StringBuilder sbRet = new StringBuilder();
            sbRet.AppendLine("�˲���ʹ��OracleClient����Oracle9�����ϵ����ݿ�(����:Oracle10��Oracle11��Oracle12)");
            sbRet.AppendLine("���Ȱ�װOracle���ӿͻ��ˣ�Ȼ����Net Configuration Assistant���ú� �������������");
            sbRet.AppendLine("Ȼ��ѷ���������ο������ַ����е�Data Source��");
            sbRet.AppendLine("��������û������뼴��");
            sbRet.AppendLine("Oracle���ӿͻ�������:");
            sbRet.AppendLine("http://www.oracle.com/technetwork/database/features/instant-client/index-097480.html");
            return sbRet.ToString();
        }
        
        public static string GetDicName(string type)
        {
            return type.Replace(":", ".v");
        }

        /// <summary>
        /// �������ݲ�
        /// </summary>
        /// <param name="entity"></param>
        public void GenerateDataAccess()
        {
            Microsoft.VisualStudio.Shell.ThreadHelper.ThrowIfNotOnUIThread();
            //FileInfo info = new FileInfo(EntityFileName);


            string dicPath = GenerateBasePath + "\\DataAccess";
            if (!Directory.Exists(dicPath))
            {
                Directory.CreateDirectory(dicPath);
            }
            string dal = Models.dataaccess;
            foreach (ComboBoxItem itype in DataAccessTypes) 
            {
                if (!this.BbConfig.IsAllDal && !this.BbConfig.DbType.Equals(itype.Value)) 
                {
                    continue;
                }
                string type = GetDicName(itype.Value.ToString());
                string dalPath = dicPath + "\\" + type;
                if (!Directory.Exists(dalPath))
                {
                    Directory.CreateDirectory(dalPath);
                }
                string fileName = dalPath + "\\" + ClassName + "DataAccess.cs";
                if (File.Exists(fileName))
                {
                    continue;
                }

                List<string> codes = new List<string>();
                using (StringReader reader = new StringReader(dal))
                {
                    string tmp = null;
                    while ((tmp = reader.ReadLine()) != null)
                    {
                        tmp = tmp.Replace("<%=EntityNamespace%>", EntityNamespace);
                        tmp = tmp.Replace("<%=Summary%>", Table.Description);
                        tmp = tmp.Replace("<%=DataAccessNamespace%>", DataAccessNamespace);
                        tmp = tmp.Replace("<%=DataBaseType%>", type);
                        tmp = tmp.Replace("<%=ClassName%>", ClassName);
                        codes.Add(tmp);
                    }
                }
                dmt.AppendDal(DataAccessNamespace + "." + type + "." + ClassName + "DataAccess", DataAccessNamespace + ".IDataAccess.I" + ClassName + "DataAccess");
                dmt.AppendBo(BusinessNamespace + "." + ClassName + "Business", EntityNamespace + "." + ClassName);
                CodeFileHelper.SaveFile(fileName, codes);
                EnvDTE.ProjectItem newit = DesignerInfo.CurrentProject.ProjectItems.AddFromFile(fileName);
                newit.Properties.Item("BuildAction").Value = (int)BuildAction.Code;
            }
            dmt.SaveXML();
        }
        /// <summary>
        /// ����IDataAccess
        /// </summary>
        /// <param name="entity"></param>
        public void GenerateIDataAccess() 
        {
            Microsoft.VisualStudio.Shell.ThreadHelper.ThrowIfNotOnUIThread();
            FileInfo info = new FileInfo(ClassDesignerFileName);
            string dicPath = info.DirectoryName + "\\DataAccess";
            if (!Directory.Exists(dicPath))
            {
                Directory.CreateDirectory(dicPath);
            }
            string idalPath = dicPath + "\\IDataAccess";
            if (!Directory.Exists(idalPath))
            {
                Directory.CreateDirectory(idalPath);
            }
            string fileName = idalPath + "\\I" + ClassName + "DataAccess.cs";
            if (File.Exists(fileName))
            {
                return;
            }
            string idal = Models.idataaccess;
            List<string> codes = new List<string>();
            using (StringReader reader = new StringReader(idal))
            {
                string tmp = null;
                while ((tmp = reader.ReadLine()) != null)
                {
                    tmp = tmp.Replace("<%=EntityNamespace%>", EntityNamespace);
                    tmp = tmp.Replace("<%=Summary%>", Table.Description);
                    tmp = tmp.Replace("<%=DataAccessNamespace%>", DataAccessNamespace);
                    tmp = tmp.Replace("<%=ClassName%>", ClassName);
                    codes.Add(tmp);
                }
            }
            CodeFileHelper.SaveFile(fileName, codes);
            EnvDTE.ProjectItem newit =DesignerInfo.CurrentProject.ProjectItems.AddFromFile(fileName);
            newit.Properties.Item("BuildAction").Value = (int)BuildAction.Code;
        }

        /// <summary>
        /// ����BQL���ݲ�
        /// </summary>
        public void GenerateBQLDataAccess() 
        {
            Microsoft.VisualStudio.Shell.ThreadHelper.ThrowIfNotOnUIThread();
            FileInfo info = new FileInfo(ClassDesignerFileName);
            string dicPath = info.DirectoryName + "\\DataAccess";
            if (!Directory.Exists(dicPath))
            {
                Directory.CreateDirectory(dicPath);
            }
            string type = "Bql";
            string idalPath = dicPath + "\\" + type;
            if (!Directory.Exists(idalPath))
            {
                Directory.CreateDirectory(idalPath);
            }
            string fileName = idalPath + "\\" + ClassName + "DataAccess.cs";
            if (File.Exists(fileName))
            {
                return;
            }
            string idal = Models.bqldataaccess;
            List<string> codes = new List<string>();
            using (StringReader reader = new StringReader(idal))
            {
                string tmp = null;
                while ((tmp = reader.ReadLine()) != null)
                {
                    tmp = tmp.Replace("<%=EntityNamespace%>", EntityNamespace);
                    tmp = tmp.Replace("<%=Summary%>", Table.Description);
                    tmp = tmp.Replace("<%=DataAccessNamespace%>", DataAccessNamespace);
                    tmp = tmp.Replace("<%=DataBaseType%>", type);
                    tmp = tmp.Replace("<%=ClassName%>", ClassName);
                    tmp = tmp.Replace("<%=BQLEntityNamespace%>", BQLEntityNamespace);
                    codes.Add(tmp);
                }
            }
            CodeFileHelper.SaveFile(fileName, codes);
            EnvDTE.ProjectItem newit = DesignerInfo.CurrentProject.ProjectItems.AddFromFile(fileName);
            newit.Properties.Item("BuildAction").Value = (int)BuildAction.Code;
        }

    }
}
