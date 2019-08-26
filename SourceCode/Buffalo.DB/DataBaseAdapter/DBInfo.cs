using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DB.DataBaseAdapter.IDbAdapters;
using Buffalo.DB.DbCommon;
using Buffalo.DB.MessageOutPuters;
using Buffalo.DB.BQLCommon.BQLConditionCommon;
using Buffalo.DB.CommBase;
using Buffalo.DB.DBCheckers;
using Buffalo.DB.CacheManager;
using Buffalo.DB.Exceptions;
using System.Reflection;
using System.ComponentModel;
using System.Collections.Concurrent;

namespace Buffalo.DB.DataBaseAdapter
{
    public class DBInfo
    {
        private string _dbName = null;
        private IDBAdapter _curDbAdapter = null;
        
        private IAggregateFunctions _curAggregateFunctions = null;

        private IMathFunctions _curMathFunctions = null;
        private IConvertFunction _curConvertFunctions = null;
        private ICommonFunction _curCommonFunctions = null;
        private IDBStructure _curDBStructure = null;
        MessageOutput _sqlOutputer = new MessageOutput();


        private string _connectionString = null;
        private string _readOnlyConnectionString = null;
        private string _dbType = null;

        private bool _operatorPrecedence=true;
        /// <summary>
        /// ���ɵ�SQL��������������ȼ��Ż��ɶ���
        /// </summary>
        public bool OperatorPrecedence
        {
            get { return _operatorPrecedence; }
            set { _operatorPrecedence = value; }
        }
        private QueryCache _cache;
        /// <summary>
        /// ��ѯ����
        /// </summary>
        public QueryCache QueryCache
        {
            get { return _cache; }
        }

        private LazyType _allowLazy;
        /// <summary>
        /// �����ӳټ���
        /// </summary>
        public LazyType AllowLazy
        {
            get { return _allowLazy; }
        }

        /// <summary>
        /// ���ò�ѯ����
        /// </summary>
        /// <param name="ica"></param>
        internal void SetQueryCache(ICacheAdaper ica, bool isAlltable) 
        {
            _cache.InitCache(ica,isAlltable);
        }

        //private static Dictionary<string, IAdapterLoader> _dicAdapterLoaderName = InitAdapterLoaderName();

        ///// <summary>
        ///// ���ݿ�������������
        ///// </summary>
        //public static Dictionary<string, IAdapterLoader> AdapterLoaders
        //{
        //    get { return DBInfo._dicAdapterLoaderName; }
        //}

        ///// <summary>
        ///// ��ʼ�������������ռ��б�
        ///// </summary>
        ///// <returns></returns>
        //private static Dictionary<string, IAdapterLoader> InitAdapterLoaderName() 
        //{
        //    Dictionary<string, IAdapterLoader> dic = new Dictionary<string, IAdapterLoader>();
        //    dic["Sql2K"] = new Buffalo.DB.DataBaseAdapter.SqlServer2KAdapter.AdapterLoader();
        //    dic["Sql2K5"] = new Buffalo.DB.DataBaseAdapter.SqlServer2K5Adapter.AdapterLoader();
        //    dic["Sql2K8"] = new Buffalo.DB.DataBaseAdapter.SqlServer2K8Adapter.AdapterLoader();
        //    dic["Oracle9"] =new Buffalo.DB.DataBaseAdapter.Oracle9Adapter.AdapterLoader();
        //    ////dic["MySQL5"] = new Buffalo.DB.DataBaseAdapter.MySQL5Adapter.AdapterLoader();
        //    ////dic["SQLite"] = new Buffalo.DB.DataBaseAdapter.SQLiteAdapter.AdapterLoader();
        //    ////dic["DB2v9"] = new Buffalo.DB.DataBaseAdapter.IBMDB2V9Adapter.AdapterLoader();
        //    ////dic["Psql9"] = new Buffalo.DB.DataBaseAdapter.PostgreSQL9Adapter.AdapterLoader();
        //    dic["Access"] = new Buffalo.DB.DataBaseAdapter.AccessAdapter.AdapterLoader();
        //    return dic;
        //}

        private static ConcurrentDictionary<string, Assembly> _dicAdapterLoader = new ConcurrentDictionary<string, Assembly>();

        /// <summary>
        /// ��ȡ�ⲿ������
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private static Type GetAdapterLoader(string key) 
        {
            Type ret = null;
            Assembly assembly = null;
            string typeName = key + ".AdapterLoader";

            if (_dicAdapterLoader.TryGetValue(key, out assembly)) 
            {
                ret = assembly.GetType(typeName, false, false);
                return ret;
            }
           
            try
            {
                assembly = Assembly.Load(key);
            }
            catch (Exception ex)
            {
                throw new MissingMemberException("�Ҳ�����:" + key + ",�뱣֤��Ŀ�Ѿ�������" + key + ".dll");
            }

            ret = assembly.GetType(typeName, false, false);
            if (ret == null)
            {
                throw new MissingMemberException("�Ҳ�����" + typeName + ",�뱣֤" + key + ".dll��������");
            }
            _dicAdapterLoader[key] = assembly;
            return ret;
        }

        /// <summary>
        /// ��ȡ������
        /// </summary>
        /// <returns></returns>
        public static IAdapterLoader GetLoader(string key) 
        {
            IAdapterLoader ret = null;
            if (key.Equals("Sql2K", StringComparison.CurrentCultureIgnoreCase)) 
            {
                ret = new Buffalo.DB.DataBaseAdapter.SqlServer2KAdapter.AdapterLoader();
            }
            else if (key.Equals("Sql2K5", StringComparison.CurrentCultureIgnoreCase)) 
            {
                ret = new Buffalo.DB.DataBaseAdapter.SqlServer2K5Adapter.AdapterLoader();
            }
            else if (key.Equals("Sql2K8", StringComparison.CurrentCultureIgnoreCase))
            {
                ret = new Buffalo.DB.DataBaseAdapter.SqlServer2K8Adapter.AdapterLoader();
            }
            else if (key.Equals("Sql2K12", StringComparison.CurrentCultureIgnoreCase))
            {
                ret = new Buffalo.DB.DataBaseAdapter.SqlServer2K12Adapter.AdapterLoader();
            }
            else if (key.Equals("Oracle9", StringComparison.CurrentCultureIgnoreCase))
            {
                ret = new Buffalo.DB.DataBaseAdapter.Oracle9Adapter.AdapterLoader();
            }
            else if (key.Equals("Access", StringComparison.CurrentCultureIgnoreCase))
            {
                ret = new Buffalo.DB.DataBaseAdapter.AccessAdapter.AdapterLoader();
            }
            else 
            {
                string[] items = key.Split(':');
                string lib = items[0];
                string version = null;
                if (items.Length > 1)
                {
                    version = items[1];
                }
                Type loaderType = GetAdapterLoader(lib);
                ret = Activator.CreateInstance(loaderType) as IAdapterLoader;
                ret.SetDBVersion(version);
            }
            return ret;
        }


        //private Dictionary<string, string> _extendDatabaseConnection = null;
        /// <summary>
        /// ���ݿ���Ϣ
        /// </summary>
        /// <param name="dbName">����</param>
        /// <param name="connectionString">�����ַ���</param>
        /// <param name="readonlyConnectionString">ֻ�������ַ���</param>
        /// <param name="dbType">���ݿ�����</param>
        /// <param name="allowLazy">�Ƿ��ӳټ���</param>
        public DBInfo(string dbName,string connectionString, string readonlyConnectionString,
            string dbType,LazyType allowLazy
            ) 
        {
            _dbType = dbType;
            _connectionString = connectionString;
            _readOnlyConnectionString = readonlyConnectionString;
            _dbName = dbName;
            _cache = new QueryCache(this);
            _allowLazy = allowLazy;
            InitAdapters();
        }



        /// <summary>
        /// ������ݿ�ṹ
        /// </summary>
        /// <returns></returns>
        public List<string> CheckDataBase() 
        {
            return DBChecker.CheckDataBase(this);
        }
        /// <summary>
        /// ��ȡ�������ݵ�SQL
        /// </summary>
        /// <returns></returns>
        public string GetUpdateDBText()
        {
            List<string> lstSQL=DBChecker.CheckDataBase(this);
            StringBuilder sbSQL = new StringBuilder();
            foreach (string sql in lstSQL) 
            {
                sbSQL.AppendLine(sql);
            }
            return sbSQL.ToString();
        }
        /// <summary>
        /// ��鲢�������ݿ�ṹ
        /// </summary>
        /// <returns></returns>
        public string UpdateDataBase() 
        {
            List<string> sql=DBChecker.CheckDataBase(this);
            List<string> res=DBChecker.ExecuteSQL(DefaultOperate, sql);
            StringBuilder sbRet = new StringBuilder();
            foreach (string str in res)
            {
                sbRet.AppendLine(str);
            }
            return sbRet.ToString();
        }

        /// <summary>
        /// �׳��Ҳ�������쳣
        /// </summary>
        /// <param name="eType"></param>
        internal void ThrowNotFondTable(Type eType)
        {
            throw new Exception("�Ҳ���ʵ�壺" + eType.Name + " ��Ӧ�ı������������ʱ�����ݿ⣺" + _dbName + " �Ƿ��Ѿ�����InitDB()�������г�ʼ��");
        }

        /// <summary>
        /// ��ʼ�����ݿ�������
        /// </summary>
        private void InitAdapters() 
        {

            IAdapterLoader loader = GetLoader(DbType);
            if (loader==null) 
            {
                throw new Exception("��֧�����ݿ�����:" + DbType);
            }

            _curAggregateFunctions = loader.AggregateFunctions;
            _curCommonFunctions = loader.CommonFunctions;
            _curConvertFunctions = loader.ConvertFunctions;
            _curDbAdapter = loader.DbAdapter;
            _curDBStructure = loader.DBStructure;
            _curMathFunctions = loader.MathFunctions;

        }


        /// <summary>
        /// Ĭ������
        /// </summary>
        /// <returns></returns>
        public DataBaseOperate DefaultOperate 
        {
            get
            {
                return StaticConnection.GetStaticOperate(this);
            }
        }

        /// <summary>
        /// �������ݿ�����
        /// </summary>
        /// <returns></returns>
        public DataBaseOperate CreateOperate()
        {
            DataBaseOperate oper = new DataBaseOperate(this);
            return oper;
        }

        /// <summary>
        /// ���SQL������
        /// </summary>
        public MessageOutput SqlOutputer 
        {
            get 
            {
                return _sqlOutputer;
            }
        }

        /// <summary>
        /// ��ȡ��ǰ���ݿ������
        /// </summary>
        /// <returns></returns>
        public string Name
        {
            get
            {

                return _dbName;
            }
        }
        private string[] _dataaccessNamespace;

        /// <summary>
        /// ���ݲ�����
        /// </summary>
        public string[] DataaccessNamespace
        {
            get
            {
                return _dataaccessNamespace;
            }
            set
            {
                _dataaccessNamespace = value;
            }
        }
        /// <summary>
        /// ��ȡ��ǰ���ݿ��������
        /// </summary>
        /// <returns></returns>
        public IDBAdapter CurrentDbAdapter
        {
            get
            {

                return _curDbAdapter;
            }
        }
        /// <summary>
        /// ��ȡ�ۺϺ����Ĵ���
        /// </summary>
        /// <returns></returns>
        public IAggregateFunctions Aggregate
        {
            get
            {

                return _curAggregateFunctions;
            }
        }

        /// <summary>
        /// ��ȡ��ѧ�����Ĵ���
        /// </summary>
        /// <returns></returns>
        public IMathFunctions Math
        {
            get
            {

                return _curMathFunctions;
            }
        }

        /// <summary>
        /// ��ȡת�������Ĵ���
        /// </summary>
        /// <returns></returns>
        public IConvertFunction Convert
        {
            get
            {

                return _curConvertFunctions;
            }
        }

        /// <summary>
        /// ��ȡ���ú����Ĵ���Ĵ���
        /// </summary>
        /// <returns></returns>
        public ICommonFunction Common
        {
            get
            {

                return _curCommonFunctions;
            }
        }
        /// <summary>
        /// ���ݿ�ṹ����
        /// </summary>
        /// <returns></returns>
        public IDBStructure DBStructure
        {
            get
            {

                return _curDBStructure;
            }
        }
        /// <summary>
        /// ��ǰ���ݿ������
        /// </summary>
        public string DbType
        {
            get
            {
                //InitBaseConfig();
                return _dbType;
            }
        }



        /// <summary>
        /// ���ݿ�������ַ���
        /// </summary>
        public string ConnectionString 
        {
            get 
            {
                return _connectionString;
            }
            set 
            {
                _connectionString = value;
            }
        }
        /// <summary>
        /// ֻ�����ݿ�������ַ���
        /// </summary>
        public string ReadOnlyConnectionString
        {
            get
            {
                return _readOnlyConnectionString;
            }
            set
            {
                _readOnlyConnectionString = value;
            }
        }

        /// <summary>
        /// ֻ�����ݿ�������Ƿ����
        /// </summary>
        public bool HasReadOnlyConnection
        {
            get
            {
                return !string.IsNullOrEmpty(_readOnlyConnectionString);
            }
            
        }
        private ConcurrentDictionary<string, BQLEntityTableHandle> _dicTables = new ConcurrentDictionary<string, BQLEntityTableHandle>();

        /// <summary>
        /// ��ӵ�����Ϣ
        /// </summary>
        /// <param name="table"></param>
        internal void AddToDB(BQLEntityTableHandle table) 
        {
            string key = table.GetEntityInfo().EntityType.FullName;
            if (!_dicTables.ContainsKey(key))
            {
                _dicTables[key]=table;
            }
        }

        /// <summary>
        /// ��ȡ���б�
        /// </summary>
        /// <returns></returns>
        public List<BQLEntityTableHandle> GetAllTables() 
        {
            List<BQLEntityTableHandle> allTable = new List<BQLEntityTableHandle>(_dicTables.Count);
            foreach (KeyValuePair<string, BQLEntityTableHandle> kvp in _dicTables) 
            {
                allTable.Add(kvp.Value);
            }
            return allTable;
        }

        /// <summary>
        /// ͨ��ʵ�����Ͳ��Ҷ�Ӧ��BQL����Ϣ
        /// </summary>
        /// <param name="entityType"></param>
        /// <returns></returns>
        public BQLEntityTableHandle FindTable(Type entityType)
        {

            return FindTable(entityType.FullName);
        }

        /// <summary>
        /// ͨ��ʵ�����Ͳ��Ҷ�Ӧ��BQL����Ϣ
        /// </summary>
        /// <param name="entityType"></param>
        /// <returns></returns>
        public BQLEntityTableHandle FindTable(string fullName)
        {
            BQLEntityTableHandle ret = null;
            _dicTables.TryGetValue(fullName, out ret);
            return ret;
        }
        private SQLRunningExceptionOption _exceptionOption=new SQLRunningExceptionOption();
        /// <summary>
        /// �쳣��Ϣ����
        /// </summary>
        public SQLRunningExceptionOption ExceptionOption
        {
            get { return _exceptionOption; }
        }
       
    }

    /// <summary>
    /// �ӳټ�������
    /// </summary>
    public enum LazyType 
    {
        [Description("ȫ������")]
        Enable=1,
        [Description("ȫ������")]
        Disable=2,
        [Description("ʵ���Զ�")]
        User=3
    }
}
