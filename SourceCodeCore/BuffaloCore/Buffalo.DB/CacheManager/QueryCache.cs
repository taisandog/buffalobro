using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Threading;

using System.Web;
using Buffalo.DB.DataBaseAdapter;
using Buffalo.DB.DbCommon;
using System.Data;
using Buffalo.DB.DataFillers;
using System.Data.Common;
using System.Reflection;
using Buffalo.DB.CommBase;
using Buffalo.DB.CacheManager.CacheCollection;
using System.Threading.Tasks;

namespace Buffalo.DB.CacheManager
{

    /// <summary>
    /// 内存缓存类
    /// </summary>
    public class QueryCache
    {

        
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
            if (_cacheAssembly == null)
            {
                try
                {
                    _cacheAssembly = Assembly.Load("Buffalo.QueryCache");
                }
                catch (Exception ex)
                {
                    throw new MissingMemberException("找不到类Buffalo.QueryCache,请保证项目已经引用了Buffalo.QueryCache.dll");
                }
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
            string sql, ParamList lstParam, TimeSpan expir, DataBaseOperate oper)
        {
            if (_cache == null)
            {
                return false;
            }
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append(sql);
            sbSql.Append(";");
            sbSql.Append(lstParam.GetParamString(_db,oper));
            return _cache.SetData(tables, sbSql.ToString(), ds, expir, oper);
            
        }
        // <summary>
        /// 获取缓存中的DataSet
        /// </summary>
        /// <param name="tables">表</param>
        /// <param name="sql">SQL语句</param>
        /// <param name="lstParam">变量集合</param>
        /// <returns></returns>
        public async Task<bool> SetDataSetAsync(DataSet ds, IDictionary<string, bool> tables,
            string sql, ParamList lstParam, TimeSpan expir, DataBaseOperate oper)
        {
            if (_cache == null)
            {
                return false;
            }
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append(sql);
            sbSql.Append(";");
            sbSql.Append(lstParam.GetParamString(_db, oper));
            return await _cache.SetDataAsync(tables, sbSql.ToString(), ds, expir, oper);

        }
        /// 获取缓存中的DataSet
        /// </summary>
        /// <param name="tables">表</param>
        /// <param name="sql">SQL语句</param>
        /// <param name="lstParam">变量集合</param>
        /// <returns></returns>
        public bool SetDataSet(DataSet ds, IDictionary<string, bool> tables,
            string sql, ParamList lstParam, DataBaseOperate oper)
        {
            return SetDataSet(ds,tables,sql,lstParam,TimeSpan.MinValue,oper);
        }
       
        /// <summary>
        /// 获取所有键
        /// </summary>
        /// <param name="pattern">通配符 允许使用的通配符：?，*，其中? 代表任意一个字符，* 代表零或多个任意字符</param>
        /// <param name="oper"></param>
        /// <returns></returns>
        public IEnumerable<string> GetAllKeys(string pattern)
        {
            return _cache.GetAllKeys(pattern);
        }
        /// <summary>
        /// 获取所有键
        /// </summary>
        /// <param name="pattern">通配符 允许使用的通配符：?，*，其中? 代表任意一个字符，* 代表零或多个任意字符</param>
        /// <param name="oper"></param>
        /// <returns></returns>
        public Task<IEnumerable<string>> GetAllKeysAsync(string pattern)
        {
            return _cache.GetAllKeysAsync(pattern);
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
        /// 写入缓存中的Reader
        /// </summary>
        /// <param name="tables">表</param>
        /// <param name="sql">SQL语句</param>
        /// <param name="lstParam">变量集合</param>
        /// <returns></returns>
        public DbDataReader SetReader(IDataReader reader, IDictionary<string, bool> tables,
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
        /// 写入缓存中的Reader
        /// </summary>
        /// <param name="tables">表</param>
        /// <param name="sql">SQL语句</param>
        /// <param name="lstParam">变量集合</param>
        /// <returns></returns>
        public async Task<DbDataReader> SetReaderAsync(DbDataReader reader, IDictionary<string, bool> tables,
            string sql, ParamList lstParam, DataBaseOperate oper)
        {
            if (_cache == null)
            {
                return null;
            }

            DataSet ds = await CacheReader.GenerateDataSetAsync(reader, false);
            MemCacheReader mreader = new MemCacheReader(ds);
            await SetDataSetAsync(ds, tables, sql, lstParam,TimeSpan.MinValue, oper);
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
        /// 删除表的缓存
        /// </summary>
        /// <param name="tables"></param>
        /// <returns></returns>
        public  async Task<bool> ClearTableCacheAsync(IDictionary<string, bool> tables, DataBaseOperate oper)
        {
            if (_cache == null)
            {
                return false;
            }

            foreach (KeyValuePair<string, bool> kvp in tables)
            {
                if (IsCacheTable(kvp.Key) || _isAllTableCache)
                {
                    await _cache.RemoveByTableNameAsync(kvp.Key, oper);
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
        /// 获取值
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="valueType">值类型</param>
        /// <returns></returns>
        public async Task<IDictionary<string, object>> GetValuesAsync(string[] keys)
        {
            return await _cache.GetValuesAsync(keys, _db.DefaultOperate);
        }
        /// <summary>
        /// 获取值
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="defaultValue">没有键时候的默认值</param>
        /// <returns></returns>
        public E GetValue<E>(string key,E defaultValue)
        {
            return _cache.GetValue<E>(key,defaultValue, _db.DefaultOperate);
        }
        /// <summary>
        /// 获取值
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="defaultValue">没有键时候的默认值</param>
        /// <returns></returns>
        public async Task<E> GetValueAsync<E>(string key, E defaultValue)
        {
            return await _cache.GetValueAsync<E>(key, defaultValue, _db.DefaultOperate);
        }
        /// <summary>
        /// 获取值
        /// </summary>
        /// <param name="key">键</param>
        /// <returns></returns>
        public E GetValue<E>(string key)
        {
            return _cache.GetValue<E>(key, default(E), _db.DefaultOperate);
        }
        /// <summary>
        /// 获取值
        /// </summary>
        /// <param name="key">键</param>
        /// <returns></returns>
        public async Task<E> GetValueAsync<E>(string key)
        {
            return await _cache.GetValueAsync<E>(key, default(E), _db.DefaultOperate);
        }
        /// <summary>
        /// 获取值
        /// </summary>
        /// <param name="key">键</param>
        /// <returns></returns>
        public object GetValue(string key)
        {
            return _cache.GetValue(key, _db.DefaultOperate);
        }
        /// <summary>
        /// 获取值
        /// </summary>
        /// <param name="key">键</param>
        /// <returns></returns>
        public async Task<object> GetValueAsync(string key)
        {
            return await _cache.GetValueAsync(key, _db.DefaultOperate);
        }
        /// <summary>
        /// Key是否存在
        /// </summary>
        /// <param name="key">键</param>
        /// <returns></returns>
        public bool ExistsKey(string key)
        {
            return _cache.ExistsKey(key, _db.DefaultOperate);
        }
        /// <summary>
        /// Key是否存在
        /// </summary>
        /// <param name="key">键</param>
        /// <returns></returns>
        public async Task<bool> ExistsKeyAsync(string key)
        {
            return await _cache.ExistsKeyAsync(key, _db.DefaultOperate);
        }
        /// <summary>
        /// 设置key过期
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="expirSeconds">过期时间(秒)</param>
        /// <returns></returns>
        public bool SetKeyExpire(string key, TimeSpan expir)
        {
            return _cache.SetKeyExpire(key, expir, _db.DefaultOperate);
        }
        /// <summary>
        /// 设置key过期
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="expirSeconds">过期时间(秒)</param>
        /// <returns></returns>
        public async Task<bool> SetKeyExpireAsync(string key, TimeSpan expir)
        {
            return await _cache.SetKeyExpireAsync(key, expir, _db.DefaultOperate);
        }
        
        /// <summary>
        /// 清空所有缓存值
        /// </summary>
        /// <returns></returns>
        public void Clear()
        {
            _cache.ClearAll();
        }
        /// <summary>
        /// 设置值
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <returns>是否设置成功</returns>
        public bool SetValue<E>(string key, E value) 
        {
            return _cache.SetValue<E>(key,value,SetValueType.Set,TimeSpan.MinValue, _db.DefaultOperate);
        }
        /// <summary>
        /// 设置值
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <returns>是否设置成功</returns>
        public async Task<bool> SetValueAsync<E>(string key, E value)
        {
            return await _cache.SetValueAsync<E>(key, value, SetValueType.Set, TimeSpan.MinValue, _db.DefaultOperate);
        }
        /// <summary>
        /// 设置值
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="expirSeconds">超时秒数</param>
        /// <param name="value">值</param>
        /// <param name="type">设置值方式</param>
        /// <returns>是否设置成功</returns>
        public bool SetValue<E>(string key, E value, SetValueType type, TimeSpan expir)
        {
            return _cache.SetValue<E>(key, value,type, expir, _db.DefaultOperate);
        }
        /// <summary>
        /// 设置值
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="expirSeconds">超时秒数</param>
        /// <param name="value">值</param>
        /// <param name="type">设置值方式</param>
        /// <returns>是否设置成功</returns>
        public async Task<bool> SetValueAsync<E>(string key, E value, SetValueType type, TimeSpan expir)
        {
            return await _cache.SetValueAsync<E>(key, value, type, expir, _db.DefaultOperate);
        }
        /// <summary>
        /// 设置值
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="expirSeconds">超时秒数</param>
        /// <param name="value">值</param>
        /// <param name="type">设置值方式</param>
        /// <returns>是否设置成功</returns>
        public bool SetValue(string key, object value,SetValueType type, TimeSpan expir)
        {
            return _cache.SetValue(key, value,type, expir, _db.DefaultOperate);
        }
        /// <summary>
        /// 设置值
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="expirSeconds">超时秒数</param>
        /// <param name="value">值</param>
        /// <param name="type">设置值方式</param>
        /// <returns>是否设置成功</returns>
        public async Task<bool> SetValueAsync(string key, object value, SetValueType type, TimeSpan expir)
        {
            return await _cache.SetValueAsync(key, value, type, expir, _db.DefaultOperate);
        }
        /// <summary>
        /// 设置值
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <returns>是否设置成功</returns>
        public bool SetValue(string key, object value)
        {
            return _cache.SetValue(key, value,SetValueType.Set, TimeSpan.MinValue, _db.DefaultOperate);
        }
        /// <summary>
        /// 设置值
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <returns>是否设置成功</returns>
        public async Task<bool> SetValueAsync(string key, object value)
        {
            return await _cache.SetValueAsync(key, value, SetValueType.Set, TimeSpan.MinValue, _db.DefaultOperate);
        }

        /// <summary>
        /// 删除值
        /// </summary>
        /// <param name="key">键</param>
        /// <returns></returns>
        public void DeleteValue(string key) 
        {
            _cache.DeleteValue(key, _db.DefaultOperate);
        }
        /// <summary>
        /// 删除值
        /// </summary>
        /// <param name="key">键</param>
        /// <returns></returns>
        public async Task DeleteValueAsync(string key)
        {
            await _cache.DeleteValueAsync(key, _db.DefaultOperate);
        }
        /// <summary>
        /// 自增1
        /// </summary>
        /// <param name="key"></param>
        public long DoIncrement(string key)
        {
            return _cache.DoIncrement(key, 1, _db.DefaultOperate);
        }
        /// <summary>
        /// 自增1
        /// </summary>
        /// <param name="key"></param>
        public async Task<long> DoIncrementAsync(string key)
        {
            return await _cache.DoIncrementAsync(key, 1, _db.DefaultOperate);
        }
        /// <summary>
        /// 自增1
        /// </summary>
        /// <param name="key"></param>
        public long DoIncrement(string key,ulong inc)
        {
           return  _cache.DoIncrement(key, inc, _db.DefaultOperate);
        }
        /// <summary>
        /// 自增1
        /// </summary>
        /// <param name="key"></param>
        public async Task<long> DoIncrementAsync(string key, ulong inc)
        {
            return await _cache.DoIncrementAsync(key, inc, _db.DefaultOperate);
        }
        /// <summary>
        /// 自减1
        /// </summary>
        /// <param name="key"></param>
        public long DoDecrement(string key)
        {
            return _cache.DoDecrement(key, 1, _db.DefaultOperate);
        }
        /// <summary>
        /// 自减1
        /// </summary>
        /// <param name="key"></param>
        public async Task<long> DoDecrementAsync(string key)
        {
            return await _cache.DoDecrementAsync(key, 1, _db.DefaultOperate);
        }
        /// <summary>
        /// 自减
        /// </summary>
        /// <param name="key"></param>
        public long DoDecrement(string key, ulong dec)
        {
            return _cache.DoDecrement(key, dec, _db.DefaultOperate);
        }
        /// <summary>
        /// 自减
        /// </summary>
        /// <param name="key"></param>
        public async Task<long> DoDecrementAsync(string key, ulong dec)
        {
            return await _cache.DoDecrementAsync(key, dec, _db.DefaultOperate);
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="entityType">实体类型</param>
        /// <returns></returns>
        public EntityBase GetEntity(string key, Type entityType) 
        {
            IList lst = GetList(key, entityType);
            if (lst != null && lst.Count > 0) 
            {
                return lst[0] as EntityBase;
            }
            return null;
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="key">键</param>
        /// <returns></returns>
        public T GetEntity<T>(string key) where T : EntityBase
        {
            IList lst = GetList(key, typeof(T));
            if (lst!=null && lst.Count > 0)
            {
                return lst[0] as T;
            }
            return null;
        }
        /// <summary>
        /// 保存实体
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="enity">实体</param>
        /// <returns></returns>
        public bool SetEntity(string key, EntityBase enity)
        {
            return SetEntity(key, TimeSpan.MinValue, enity);
        }
        /// <summary>
        /// 保存实体
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="enity">实体</param>
        /// <returns></returns>
        public bool SetEntity(string key, TimeSpan expir, EntityBase enity)
        {
            ArrayList lstEntity = new ArrayList();
            lstEntity.Add(enity);
            return SetList(key, expir, lstEntity);
        }
        /// <summary>
        /// 获取集合
        /// </summary>
        /// <param name="key">键</param>
        /// <returns></returns>
        public IList GetList(string key,Type entityType) 
        {
            return _cache.GetEntityList(key,entityType, _db.DefaultOperate);
        }
        /// <summary>
        /// 获取集合
        /// </summary>
        /// <param name="key">键</param>
        /// <returns></returns>
        public List<T> GetList<T>(string key) where T:EntityBase
        {
            return _cache.GetEntityList(key,typeof(T), _db.DefaultOperate) as List<T>;
        }

        /// <summary>
        /// 设置集合
        /// </summary>
        /// <param name="key">键</param>
        ///  <param name="expirSeconds">过期时间</param>
        /// <param name="lstEntiity">实体集合</param>
        /// <returns></returns>
        public bool SetList(string key, TimeSpan expir, IList lstEntiity)
        {
            return _cache.SetEntityList(key,lstEntiity, expir, _db.DefaultOperate);
        }
        /// <summary>
        /// 设置集合
        /// </summary>
        /// <param name="key">键</param>
        ///  <param name="expirSeconds">过期时间</param>
        /// <param name="lstEntiity">实体集合</param>
        /// <returns></returns>
        public bool SetList(string key, IList lstEntiity)
        {
            return _cache.SetEntityList(key, lstEntiity, TimeSpan.MinValue, _db.DefaultOperate);
        }

        /// <summary>
        /// 获取哈希表的操作方式
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public ICacheHash GetCacheHashMap(string key) 
        {
            return _cache.GetHashMap(key, _db.DefaultOperate);
        }

        /// <summary>
        /// 获取线性表的操作方式
        /// </summary>
        /// <param name="key"></param>
        /// <param name="oper"></param>
        /// <returns></returns>
        public ICacheList GetCacheList(string key) 
        {
            return _cache.GetList(key, _db.DefaultOperate);
        }

        /// <summary>
        /// 获取锁的操作方式
        /// </summary>
        /// <param name="key"></param>
        /// <param name="oper"></param>
        /// <returns></returns>
        public ICacheLock GetCacheLock(string key)
        {
            return _cache.GetCacheLock(key, _db.DefaultOperate);
        }

        /// <summary>
        /// 获取排序表的操作方式
        /// </summary>
        /// <param name="key"></param>
        /// <param name="oper"></param>
        /// <returns></returns>
        public ICacheSortedSet GetCacheSortedSet(string key) 
        {
            return _cache.GetSortedSet(key, _db.DefaultOperate);
        }

        /// <summary>
        /// 当前缓存的连接端
        /// </summary>
        public ICacheAdaper Client 
        {
            get 
            {
                return _cache;
            }
        }
    }

    /// <summary>
    /// 缓存指令
    /// </summary>
    public class QueryCacheCommand
    {
        public const string CommandDeleteSQL = "DeleteSQL";
        public const string CommandDeleteTable = "DeleteTable";
        public const string CommandSetDataSet = "SetDataSet";
        public const string CommandGetDataSet = "GetDataSet";
        public const string CommandGetList = "GetList";
        public const string CommandGetHash = "Hash";
        public const string CommandGetValues = "GetValues";
        public const string CommandSetValues = "SetValues";
        public const string CommandDeleteValues = "DeleteValues";
        public const string CommandIncrement = "Increment";
        public const string CommandSortedSet = "SortSet";
        public const string CommandLock = "Lock";
        public const string CommandSetList = "SetList";
    }
}
