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
using Buffalo.Kernel;
using System.Threading;
using Buffalo.Kernel.FastReflection;

namespace Buffalo.DB.DataBaseAdapter
{
    /// <summary>
    /// 数据库信息
    /// </summary>
    public class DBInfo
    {
       
        /// <summary>
        /// 获取浮点型长度(整数部分为10000的倍数，小数部分为小于10000的部分)
        /// </summary>
        /// <param name="typeName">类型</param>
        /// <param name="length">长度</param>
        /// <returns></returns>
        public static string GetNumberLengthType(string typeName, long length)
        {
            const int typeLen = 10000;
            StringBuilder sbRet = new StringBuilder();
            sbRet.Append(typeName);
            sbRet.Append("(");
            sbRet.Append(length / typeLen);
            sbRet.Append(",");
            sbRet.Append(length % typeLen);
            sbRet.Append(")");
            return sbRet.ToString();
        }

        private string _dbName = null;
        private int _childKey = -1;

        private IDBAdapter _curDbAdapter = null;
        
        private IAggregateFunctions _curAggregateFunctions = null;

        private IMathFunctions _curMathFunctions = null;
        private IConvertFunction _curConvertFunctions = null;
        private ICommonFunction _curCommonFunctions = null;
        private IDBStructure _curDBStructure = null;
        private MessageOutput _sqlOutputer = new MessageOutput();


        private string _connectionString = null;
        private string _readOnlyConnectionString = null;
        private string _dbType = null;

        private bool _operatorPrecedence=true;

        private CallContext<DataBaseOperate> _operate = new CallContext<DataBaseOperate>();
        /// <summary>
        /// 选中库的数据库连接
        /// </summary>
        public DataBaseOperate SelectedOperate 
        {
            get 
            {
                return SelectedDBInfo._operate.Value;
            }
            set 
            {
                SelectedDBInfo._operate.Value = value;
            }
        }
        /// <summary>
        /// 生成的SQL语句进行运算符优先级优化可读性
        /// </summary>
        public bool OperatorPrecedence
        {
            get { return _operatorPrecedence; }
            set { _operatorPrecedence = value; }
        }
        private QueryCache _cache;
        /// <summary>
        /// 查询缓存
        /// </summary>
        public QueryCache QueryCache
        {
            get { return _cache; }
        }

        private LazyType _allowLazy;
        /// <summary>
        /// 允许延迟加载
        /// </summary>
        public LazyType AllowLazy
        {
            get { return _allowLazy; }
        }

        /// <summary>
        /// 设置查询缓存
        /// </summary>
        /// <param name="ica"></param>
        /// <param name="isAlltable"></param>
        internal void SetQueryCache(ICacheAdaper ica, bool isAlltable) 
        {
            _cache.InitCache(ica,isAlltable);
        }

        private static ConcurrentDictionary<string, Assembly> _dicAdapterLoader = new ConcurrentDictionary<string, Assembly>();

        /// <summary>
        /// 获取外部加载器
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
                throw new MissingMemberException("找不到类:" + key + ",请保证项目已经引用了" + key + ".dll");
            }

            ret = assembly.GetType(typeName, false, false);
            if (ret == null)
            {
                throw new MissingMemberException("找不到类" + typeName + ",请保证" + key + ".dll的完整性");
            }
            _dicAdapterLoader[key] = assembly;
            return ret;
        }

        /// <summary>
        /// 获取加载器
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


        private ConcurrentDictionary<int, DBInfo> _childDataSource = null;

        /// <summary>
        /// 设置子数据源
        /// </summary>
        /// <param name="db">数据库信息</param>
        internal void SetChildDataSource(DBInfo db) 
        {
            if (db._childKey < 0) 
            {
                throw new IndexOutOfRangeException("子数据源键必须是大于0的正数");
            }
            if (_childDataSource == null) 
            {
                _childDataSource = new ConcurrentDictionary<int, DBInfo>();
            }
            if (db._cache == null)
            {
                db._cache = _cache;
            }
            if (db._dataaccessNamespace == null)
            {
                db._dataaccessNamespace = this._dataaccessNamespace ;
            }
            db._dbName = _dbName;
            
            _childDataSource[db._childKey] = db;
        }
        /// <summary>
        /// 添加子数据源信息
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="connectionString">连接字符串</param>
        /// <param name="readonlyConnectionString">只读连接字符串</param>
        /// <param name="dbType">数据库类型</param>
        /// <param name="allowLazy">是否延迟加载</param>
        /// <param name="dataaccessNamespace">数据层名称</param>
        /// <param name="cache">缓存</param>
        public static DBInfo CreateChildDataSource(int key, string connectionString, string readonlyConnectionString,
            string dbType, LazyType allowLazy= LazyType.Disable, string[] dataaccessNamespace=null,QueryCache cache=null) 
        {
            DBInfo db = new DBInfo(null, connectionString, readonlyConnectionString, dbType, allowLazy);
            db._dataaccessNamespace = dataaccessNamespace;
            db._cache = cache;
            db._childKey = key;

            return db;
        }

        
        private CallContext<DBInfo> _curDB = new CallContext<DBInfo>();


        /// <summary>
        /// 本线程使用的子数据源（-1则为恢复主数据源）
        /// </summary>
        public int SelectedDataSource 
        {
            get 
            {
                DBInfo val = _curDB.Value;
                if (val == null) 
                {
                    return -1;
                }
                return val._childKey ;
            }
            set 
            {
                if (value < 0) 
                {
                    _curDB.Value = null;
                    return;
                }
                DBInfo info = GetChildDBInfo(value);
                _curDB.Value = info;
            }
        }

       
        /// <summary>
        /// 选中的数据源
        /// </summary>
        private DBInfo SelectedDBInfo 
        {
            get 
            {
                if (_curDB.Value == null)
                {
                    return this;
                }
                return _curDB.Value;
            }
        }

        /// <summary>
        /// 获取子数据源
        /// </summary>
        /// <param name="key">键</param>
        /// <returns></returns>
        public DBInfo GetChildDBInfo(int key) 
        {
            if (_childDataSource == null) 
            {
                return this;
            }
            DBInfo db = null;
            if (!_childDataSource.TryGetValue(key, out db)) 
            {
                throw new KeyNotFoundException("找不到key为:" + key + "的数据源");
            }
            return db;
        }
        /// <summary>
        /// 获取所有子数据源
        /// </summary>
        /// <param name="key">键</param>
        /// <returns></returns>
        internal ConcurrentDictionary<int, DBInfo> GetAllChildDBInfo()
        {
            return _childDataSource;
        }
        /// <summary>
        /// 数据库信息
        /// </summary>
        /// <param name="dbName">名称</param>
        /// <param name="connectionString">连接字符串</param>
        /// <param name="readonlyConnectionString">只读连接字符串</param>
        /// <param name="dbType">数据库类型</param>
        /// <param name="allowLazy">是否延迟加载</param>
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
        /// 检查数据库结构
        /// </summary>
        /// <returns></returns>
        public List<string> CheckDataBase() 
        {
            return DBChecker.CheckDataBase(this);
        }
        /// <summary>
        /// 获取更新数据的SQL
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
        /// 检查并更新数据库结构
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
        /// 抛出找不到表的异常
        /// </summary>
        /// <param name="eType"></param>
        internal void ThrowNotFondTable(Type eType)
        {
            throw new Exception("找不到实体：" + eType.Name + " 对应的表，请检查程序启动时候数据库：" + _dbName + " 是否已经调用InitDB()方法进行初始化");
        }

        /// <summary>
        /// 初始化数据库适配器
        /// </summary>
        private void InitAdapters() 
        {

            IAdapterLoader loader = GetLoader(DbType);
            if (loader==null) 
            {
                throw new Exception("不支持数据库类型:" + DbType);
            }

            _curAggregateFunctions = loader.AggregateFunctions;
            _curCommonFunctions = loader.CommonFunctions;
            _curConvertFunctions = loader.ConvertFunctions;
            _curDbAdapter = loader.DbAdapter;
            _curDBStructure = loader.DBStructure;
            _curMathFunctions = loader.MathFunctions;

        }


        /// <summary>
        /// 默认连接
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
        /// 创新数据库连接
        /// </summary>
        /// <returns></returns>
        public DataBaseOperate CreateOperate()
        {
            DataBaseOperate oper = new DataBaseOperate(this);
            return oper;
        }

        /// <summary>
        /// 输出SQL语句的类
        /// </summary>
        public MessageOutput SqlOutputer 
        {
            get 
            {
                return SelectedDBInfo._sqlOutputer;
            }
        }

        /// <summary>
        /// 获取当前数据库的名字
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
        /// 数据层名称
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
        /// 获取当前数据库的适配器
        /// </summary>
        /// <returns></returns>
        public IDBAdapter CurrentDbAdapter
        {
            get
            {

                return SelectedDBInfo._curDbAdapter;
            }
        }
        /// <summary>
        /// 获取聚合函数的处理
        /// </summary>
        /// <returns></returns>
        public IAggregateFunctions Aggregate
        {
            get
            {

                return SelectedDBInfo._curAggregateFunctions;
            }
        }

        /// <summary>
        /// 获取数学函数的处理
        /// </summary>
        /// <returns></returns>
        public IMathFunctions Math
        {
            get
            {

                return SelectedDBInfo._curMathFunctions;
            }
        }

        /// <summary>
        /// 获取转换函数的处理
        /// </summary>
        /// <returns></returns>
        public IConvertFunction Convert
        {
            get
            {

                return SelectedDBInfo._curConvertFunctions;
            }
        }

        /// <summary>
        /// 获取常用函数的处理的处理
        /// </summary>
        /// <returns></returns>
        public ICommonFunction Common
        {
            get
            {

                return SelectedDBInfo._curCommonFunctions;
            }
        }
        /// <summary>
        /// 数据库结构特性
        /// </summary>
        /// <returns></returns>
        public IDBStructure DBStructure
        {
            get
            {

                return SelectedDBInfo._curDBStructure;
            }
        }
        /// <summary>
        /// 当前数据库的类型
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
        /// 数据库的连接字符串
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
        /// 选中数据源的连接字符串
        /// </summary>
        public string SelectedConnectionString 
        {
            get 
            {
                return SelectedDBInfo.ConnectionString;
            }
        }
        /// <summary>
        /// 选中数据源只读数据库的连接字符串
        /// </summary>
        public string SelectedReadOnlyConnectionString
        {
            get
            {
                return SelectedDBInfo.ReadOnlyConnectionString;
            }
        }
        /// <summary>
        /// 只读数据库的连接字符串
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
        /// 只读数据库的连接是否存在
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
        /// 添加到库信息
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
        /// 获取所有表
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
        /// 通过实体类型查找对应的BQL表信息
        /// </summary>
        /// <param name="entityType"></param>
        /// <returns></returns>
        public BQLEntityTableHandle FindTable(Type entityType)
        {

            return FindTable(entityType.FullName);
        }

        /// <summary>
        /// 通过实体类型查找对应的BQL表信息
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
        /// 异常信息配置
        /// </summary>
        public SQLRunningExceptionOption ExceptionOption
        {
            get { return _exceptionOption; }
        }
        /// <summary>
        /// 键
        /// </summary>
        public int ChildKey 
        {
            get 
            {
                return _childKey;
            }
        }
        private string _fullName = null;
        /// <summary>
        /// 全名
        /// </summary>
        public string FullName 
        {
            get 
            {
                DBInfo selInfo = SelectedDBInfo;
                if (string.IsNullOrWhiteSpace(selInfo._fullName)) 
                {
                    if (selInfo._childKey < 0)
                    {
                        selInfo._fullName = selInfo._dbName;
                    }
                    else
                    {
                        selInfo._fullName = selInfo._dbName + "." + selInfo._childKey;
                    }
                }
                return selInfo._fullName;
            }
        }
        private static Dictionary<string, Type> _dicEntityLoaderConfig = new Dictionary<string, Type>();
        /// <summary>
        /// 实体跟数据层的映射
        /// </summary>
        public Dictionary<string, Type> EntityLoaderConfig
        {
            get
            {
                return _dicEntityLoaderConfig;
            }
        }


        private Dictionary<Type, Type> _dataaccessEntityMapping = new Dictionary<Type, Type>();//配置实体记录集合
        /// <summary>
        /// 实体跟数据层的映射
        /// </summary>
        public Dictionary<Type, Type> DataaccessEntityMapping
        {
            get
            {
                return _dataaccessEntityMapping;
            }
        }

        /// <summary>
        /// 实体跟数据层的映射
        /// </summary>
        public Dictionary<Type, Type> SelectedDataaccessEntityMapping
        {
            get
            {
                return SelectedDBInfo._dataaccessEntityMapping;
            }
        }


        private Dictionary<string, Type> _dataaccessInterfaceMapping = new Dictionary<string, Type>();//配置实体记录集合
        /// <summary>
        /// 接口跟数据层的映射
        /// </summary>
        public Dictionary<string, Type> DataaccessInterfaceMapping
        {
            get 
            {
                return _dataaccessInterfaceMapping;
            }  
            
                
        }
        /// <summary>
        /// 接口跟数据层的映射
        /// </summary>
        public Dictionary<string, Type> SelectedDataaccessInterfaceMapping
        {
            get
            {
                return SelectedDBInfo._dataaccessInterfaceMapping;
            }
        }
    }

    /// <summary>
    /// 延迟加载类型
    /// </summary>
    public enum LazyType 
    {
        [Description("全部启用")]
        Enable=1,
        [Description("全部禁用")]
        Disable=2,
        [Description("实体自定")]
        User=3
    }
}
