using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Threading;

using System.Data.SqlClient;
using System.Web;
using System.Web.Caching;
using Buffalo.DB.DataBaseAdapter;
using Buffalo.DB.DbCommon;
using System.Data;
using Buffalo.DB.DataFillers;
using System.Data.Common;
using System.Reflection;

namespace Buffalo.DB.CacheManager
{
    /// <summary>
    /// 内存缓存类
    /// </summary>
    public class QueryCache
    {
        internal const string CommandDeleteSQL = "DeleteSQL";
        internal const string CommandDeleteTable = "DeleteTable";
        internal const string CommandSetDataSet = "SetDataSet";
        internal const string CommandGetDataSet = "GetDataSet";

        private DBInfo _db;

        private Dictionary<string, bool> _dicAllowCache = new Dictionary<string, bool>();

        private bool _isAllTableCache;
        /// <summary>
        /// 是否所有表都使用缓存
        /// </summary>
        public bool IsAllTableCache
        {
            get { return _isAllTableCache; }
        }
        /// <summary>
        /// 数据库信息
        /// </summary>
        public DBInfo DBInfo
        {
            get { return _db; }
            internal set { _db = value; }
        }
        private ICacheAdaper _cache;

        /// <summary>
        /// 缓存操作类
        /// </summary>
        /// <param name="db">数据库</param>
        public QueryCache(DBInfo db) 
        {
            _db = db;
           
        }
        /// <summary>
        /// 初始化缓存
        /// </summary>
        /// <param name="cache">缓存类</param>
        /// <param name="isAllTableCache">是否所有表都进行缓存</param>
        public void InitCache(ICacheAdaper cache, bool isAllTableCache) 
        {
            _cache = cache;
            _isAllTableCache = isAllTableCache;
        }

        /// <summary>
        /// 是否使用了缓存
        /// </summary>
        public bool HasCache 
        {
            get 
            {
                return _cache != null;
            }
        }

        /// <summary>
        /// 根据类型创建缓存适配器
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="connectionString">连接字符串</param>
        /// <returns></returns>
        public static ICacheAdaper GetCache(DBInfo info, string type, string connectionString)
        {
            if (string.IsNullOrEmpty(type))
            {
                return null;
            }
            string dtype = type.Trim();
            if (dtype.Equals("system", StringComparison.CurrentCultureIgnoreCase))//内存
            {
                return new MemoryAdaper(info);
            }
            ICacheAdaper cache = GetAssemblyCache(info, dtype, connectionString);
            if (cache != null) 
            {
                return cache;
            }

            throw new NotSupportedException("不支持:" + type + " 的缓存类型，当前只支持system、memcached、redis类型的缓存");
        }

        private static Assembly _cacheAssembly = null;
        /// <summary>
        /// 获取外部程序集的缓存
        /// </summary>
        /// <param name="info"></param>
        /// <param name="type"></param>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        private static ICacheAdaper GetAssemblyCache(DBInfo info, string type, string connectionString) 
        {
            Assembly _cacheAssembly = null;
            try
            {
                _cacheAssembly = Assembly.Load("Buffalo.QueryCache");
            }
            catch (Exception ex)
            {
                throw new MissingMemberException("找不到类Buffalo.QueryCache,请保证项目已经引用了Buffalo.QueryCache.dll");
            }
            if (_cacheAssembly == null) 
            {
                throw new MissingMemberException("找不到类Buffalo.QueryCache,请保证项目已经引用了Buffalo.QueryCache.dll");
            }
            Type loaderType = _cacheAssembly.GetType("Buffalo.QueryCache.CacheLoader", false, false);
            if (loaderType == null) 
            {
                throw new MissingMemberException("找不到类Buffalo.QueryCache.CacheLoader,请保证Buffalo.QueryCache.dll的完整性");
            }
            MethodInfo mInfo = loaderType.GetMethod("GetCache");
            if (loaderType == null)
            {
                throw new MissingMethodException("找不到方法GetCache,请保证Buffalo.QueryCache.dll的完整性");
            }
            ICacheAdaper cache = mInfo.Invoke(null, new object[] {info,type,connectionString }) as ICacheAdaper;
            return cache;
        }

        /// <summary>
        /// 获取缓存中的DataSet
        /// </summary>
        /// <param name="tables">表</param>
        /// <param name="sql">SQL语句</param>
        /// <param name="lstParam">变量集合</param>
        /// <returns></returns>
        public DataSet GetDataSet(IDictionary<string, bool> tables,
            string sql, ParamList lstParam, DataBaseOperate oper)
        {
            if (_cache == null)
            {
                return null;
            }
            CheckTable(tables);
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append(sql);
            sbSql.Append(";");
            sbSql.Append(lstParam.GetParamString(_db, oper));

            DataSet ds = _cache.GetData(tables, sbSql.ToString(), oper);

            return ds;
        }
        /// <summary>
        /// 获取缓存中的DataSet
        /// </summary>
        /// <param name="tables">表</param>
        /// <param name="sql">SQL语句</param>
        /// <param name="lstParam">变量集合</param>
        /// <returns></returns>
        public bool SetDataSet(DataSet ds, IDictionary<string, bool> tables,
            string sql, ParamList lstParam,DataBaseOperate oper)
        {
            if (_cache == null)
            {
                return false;
            }
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append(sql);
            sbSql.Append(";");
            sbSql.Append(lstParam.GetParamString(_db,oper));
            return _cache.SetData(tables, sbSql.ToString(), ds,oper);
            
        }

        /// <summary>
        /// 创建缓存关联表信息
        /// </summary>
        /// <param name="tables"></param>
        /// <returns></returns>
        internal Dictionary<string, bool> CreateMap(params string[] tables) 
        {
            if (_cache == null)
            {
                return null;
            }
            Dictionary<string, bool> dic = new Dictionary<string, bool>(tables.Length+1);
            foreach (string tableName in tables) 
            {
                dic[tableName] = true;
            }
            return dic;
        }
        /// <summary>
        /// 获取缓存中的Reader
        /// </summary>
        /// <param name="tables">表</param>
        /// <param name="sql">SQL语句</param>
        /// <param name="lstParam">变量集合</param>
        /// <returns></returns>
        public MemCacheReader GetReader( IDictionary<string, bool> tables,
            string sql, ParamList lstParam, DataBaseOperate oper)
        {
            if (_cache == null)
            {
                return null;
            }
            CheckTable(tables);
            DataSet ds = GetDataSet(tables,sql,lstParam,oper);
            if (ds == null) 
            {
                return null;
            }
            MemCacheReader reader = new MemCacheReader(ds);
            return reader;
        }
        /// <summary>
        /// 获取缓存中的Reader
        /// </summary>
        /// <param name="tables">表</param>
        /// <param name="sql">SQL语句</param>
        /// <param name="lstParam">变量集合</param>
        /// <returns></returns>
        public IDataReader SetReader(IDataReader reader, IDictionary<string, bool> tables,
            string sql, ParamList lstParam,DataBaseOperate oper)
        {
            if (_cache == null)
            {
                return null;
            }
            
            DataSet ds = CacheReader.GenerateDataSet(reader, false);
            MemCacheReader mreader = new MemCacheReader(ds);
            SetDataSet(ds, tables, sql, lstParam,oper);
            return mreader;
        }
        /// <summary>
        /// 删除表的缓存
        /// </summary>
        /// <param name="tables"></param>
        /// <returns></returns>
        public bool ClearTableCache(IDictionary<string, bool> tables, DataBaseOperate oper) 
        {
            if (_cache == null)
            {
                return false; 
            }
            
            foreach (KeyValuePair<string, bool> kvp in tables)
            {
                if (IsCacheTable(kvp.Key) || _isAllTableCache)
                {
                    _cache.RemoveByTableName(kvp.Key,oper);
                }
            }
            return true;
        }

        /// <summary>
        /// 检查表是否可用缓存
        /// </summary>
        /// <param name="tables"></param>
        private void CheckTable(IDictionary<string, bool> tables) 
        {
            if (_isAllTableCache) 
            {
                return;
            }
            StringBuilder sbBuffer=new StringBuilder();
            foreach (KeyValuePair<string, bool> kvp in tables) 
            {
                if (!IsCacheTable(kvp.Key)) 
                {
                    sbBuffer.Append(kvp.Key);
                    sbBuffer.Append(",");
                }
            }
            if (sbBuffer.Length > 0) 
            {
                sbBuffer.Remove(sbBuffer.Length - 1, 1);
                throw new Exception("表:" + sbBuffer.ToString() + "没设置为使用缓存，请在配置文件中指定");
            }
        }

        /// <summary>
        /// 判断表名是否允许使用缓存
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        private bool IsCacheTable(string table) 
        {
            bool hascache = false;

            if (_dicAllowCache.TryGetValue(table, out hascache)) 
            {
                return hascache;
            }
            
            return false;
        }

        /// <summary>
        /// 设置需要缓存的表
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public bool SetCacheTable(string tableName) 
        {
            _dicAllowCache[tableName] = true;
            return true;
        }

        /// <summary>
        /// 获取值
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="valueType">值类型</param>
        /// <returns></returns>
        public IDictionary<string, object> GetValues(string[] keys) 
        {
            return _cache.GetValues(keys, _db.DefaultOperate);
        }
        /// <summary>
        /// 设置值
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        public void SetValue<E>(string key, E value) 
        {
            _cache.SetValue<E>(key,value, _db.DefaultOperate);
        }
        /// <summary>
        /// 删除值
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="client">客户端</param>
        /// <returns></returns>
        public void DeleteValue(string key) 
        {
            _cache.DeleteValue(key, _db.DefaultOperate);
        }
        /// <summary>
        /// 自增1
        /// </summary>
        /// <param name="key"></param>
        public void DoIncrement(string key) 
        {
            _cache.DoIncrement(key, _db.DefaultOperate);
        }
    }
}
