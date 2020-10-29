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

            throw new NotSupportedException("不支持:" + type + " 的缓存类型，当前只支持system、Web、memcached、redis类型的缓存");
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
            string sql, ParamList lstParam, int expirSeconds, DataBaseOperate oper)
        {
            if (_cache == null)
            {
                return false;
            }
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append(sql);
            sbSql.Append(";");
            sbSql.Append(lstParam.GetParamString(_db,oper));
            return _cache.SetData(tables, sbSql.ToString(), ds, expirSeconds, oper);
            
        }
        /// <summary>
        /// 获取缓存中的DataSet
        /// </summary>
        /// <param name="tables">表</param>
        /// <param name="sql">SQL语句</param>
        /// <param name="lstParam">变量集合</param>
        /// <returns></returns>
        public bool SetDataSet(DataSet ds, IDictionary<string, bool> tables,
            string sql, ParamList lstParam, DataBaseOperate oper)
        {
            return SetDataSet(ds,tables,sql,lstParam,-1,oper);
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
        public object GetValue(string key)
        {
            return _cache.GetValue(key, _db.DefaultOperate);
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
        /// 设置key过期
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="expirSeconds">过期时间(秒)</param>
        /// <returns></returns>
        public bool SetKeyExpire(string key, int expirSeconds)
        {
            return _cache.SetKeyExpire(key, expirSeconds, _db.DefaultOperate);
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
            return _cache.SetValue<E>(key,value,SetValueType.Set,-1, _db.DefaultOperate);
        }
        /// <summary>
        /// 设置值
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="expirSeconds">超时秒数</param>
        /// <param name="value">值</param>
        /// <param name="type">设置值方式</param>
        /// <returns>是否设置成功</returns>
        public bool SetValue<E>(string key, E value, SetValueType type, int expirSeconds)
        {
            return _cache.SetValue<E>(key, value,type, expirSeconds, _db.DefaultOperate);
        }
        /// <summary>
        /// 设置值
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="expirSeconds">超时秒数</param>
        /// <param name="value">值</param>
        /// <param name="type">设置值方式</param>
        /// <returns>是否设置成功</returns>
        public bool SetValue(string key, object value,SetValueType type, int expirSeconds)
        {
            return _cache.SetValue(key, value,type, expirSeconds, _db.DefaultOperate);
        }
        /// <summary>
        /// 设置值
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <returns>是否设置成功</returns>
        public bool SetValue(string key, object value)
        {
            return _cache.SetValue(key, value,SetValueType.Set, -1, _db.DefaultOperate);
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
        public long DoIncrement(string key,ulong inc)
        {
           return  _cache.DoIncrement(key, inc, _db.DefaultOperate);
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
        /// 自减
        /// </summary>
        /// <param name="key"></param>
        public long DoDecrement(string key, ulong dec)
        {
            return _cache.DoDecrement(key, dec, _db.DefaultOperate);
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
            return SetEntity(key, -1, enity);
        }
        /// <summary>
        /// 保存实体
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="enity">实体</param>
        /// <returns></returns>
        public bool SetEntity(string key, int expirSeconds, EntityBase enity)
        {
            ArrayList lstEntity = new ArrayList();
            lstEntity.Add(enity);
            return SetList(key, expirSeconds, lstEntity);
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
        public bool SetList(string key, int expirSeconds, IList lstEntiity)
        {
            return _cache.SetEntityList(key,lstEntiity,expirSeconds, _db.DefaultOperate);
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
            return _cache.SetEntityList(key, lstEntiity, -1, _db.DefaultOperate);
        }

        #region List方法
        /// <summary>
        /// 增加到列表
        /// </summary>
        /// <typeparam name="E"></typeparam>
        /// <param name="key">键</param>
        /// <param name="index">索引(0为增加到头部，-1为增加到尾部)</param>
        /// <param name="value">值</param>
        /// <param name="setType">设置值方式</param>
        /// <returns></returns>
        public long ListAddValue<E>(string key,  E value, long index=-1, SetValueType setType=SetValueType.Set)
        {
            return _cache.ListAddValue<E>(key, index, value, setType, _db.DefaultOperate);
        }
        /// <summary>
        /// 获取值
        /// </summary>
        /// <typeparam name="E"></typeparam>
        /// <param name="key">键</param>
        /// <param name="index">值位置</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns></returns>
        public E ListGetValue<E>(string key, long index, E defaultValue=default(E))
        {
            return _cache.ListGetValue<E>(key, index, defaultValue, _db.DefaultOperate);
        }

        /// <summary>
        /// 获取集合长度
        /// </summary>
        /// <typeparam name="E"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public long ListGetLength(string key)
        {
            return _cache.ListGetLength(key, _db.DefaultOperate);
        }
        /// <summary>
        /// 移除并返回值
        /// </summary>
        /// <typeparam name="E"></typeparam>
        /// <param name="key">键</param>
        /// <param name="isPopEnd">是否从尾部移除(true则从尾部移除，否则从头部移除)</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns></returns>
        public E ListPopValue<E>(string key, bool isPopEnd=true, E defaultValue=default(E))
        {
            return _cache.ListPopValue<E>(key, isPopEnd,defaultValue, _db.DefaultOperate);
        }

        /// <summary>
        /// 移除值
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="count">要移除几个，0则为全部移除</param>
        /// <returns></returns>
        public long ListRemoveValue(string key, object value, long count=0)
        {
            return _cache.ListRemoveValue(key, value,count, _db.DefaultOperate);
        }


        /// <summary>
        /// 获取集合所有值
        /// </summary>
        /// <typeparam name="E"></typeparam>
        /// <param name="key">键</param>
        /// <param name="start">起始位置(默认0)</param>
        /// <param name="end">结束位置(-1则为读到末尾)</param>
        /// <returns></returns>
        public List<E> ListAllValues<E>(string key, long start=0, long end=-1)
        {
            return _cache.ListAllValues<E>(key, start, end, _db.DefaultOperate);
        }
        #endregion

        #region hash方法

        /// <summary>
        /// 批量给HashSet设置值
        /// </summary>
        /// <typeparam name="E"></typeparam>
        /// <param name="key">键</param>
        /// <param name="dicSet">值</param>
        /// <returns></returns>
        public void HashSetRangeValue(string key, IDictionary dicSet)
        {
            _cache.HashSetRangeValue(key, dicSet, _db.DefaultOperate);
        }
        /// <summary>
        /// HashSet设置值
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="hashkey">哈希表的键</param>
        /// <param name="value">哈希表的值</param>
        /// <param name="type">设置方式</param>
        public bool HashSetValue(string key, object hashkey, object value, SetValueType type)
        {
            return _cache.HashSetValue(key, hashkey,value,type, _db.DefaultOperate);
        }
        /// <summary>
        /// HashSet设置值
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="hashkey">哈希表的键</param>
        /// <param name="value">哈希表的值</param>
        /// <param name="type">设置方式</param>
        public bool HashSetValue(string key, object hashkey, object value)
        {
            return _cache.HashSetValue(key, hashkey, value, SetValueType.Set, _db.DefaultOperate);
        }
        /// <summary>
        /// 获取哈希表的值
        /// </summary>
        /// <typeparam name="E"></typeparam>
        /// <param name="key">键</param>
        /// <param name="hashkey">哈希表的键</param>
        /// <param name="defaultValue">默认值</param>
        /// <param name="connection"></param>
        /// <returns></returns>
        public E HashGetValue<E>(string key, object hashkey, E defaultValue)
        {
            return _cache.HashGetValue<E>(key, hashkey, defaultValue,_db.DefaultOperate);
        }
        /// <summary>
        /// 获取哈希表的值
        /// </summary>
        /// <typeparam name="E"></typeparam>
        /// <param name="key">键</param>
        /// <param name="hashkey">哈希表的键</param>
        /// <param name="defaultValue">默认值</param>
        /// <param name="connection"></param>
        /// <returns></returns>
        public E HashGetValue<E>(string key, object hashkey)
        {
            return _cache.HashGetValue<E>(key, hashkey, default(E), _db.DefaultOperate);
        }
        /// <summary>
        /// 获取所有哈希表的值
        /// </summary>
        /// <typeparam name="E"></typeparam>
        /// <param name="key">键</param>
        /// <param name="defaultValue">默认值</param>
        /// <param name="connection"></param>
        /// <returns></returns>
        public List<KeyValuePair<K, V>> HashGetAllValues<K, V>(string key, V defaultValue)
        {
            return _cache.HashGetAllValues<K, V>(key, defaultValue, _db.DefaultOperate);
        }
        /// <summary>
        /// 获取哈希表的所有值
        /// </summary>
        /// <typeparam name="K"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <param name="key">键</param>
        /// <returns></returns>
        public List<KeyValuePair<K, V>> HashGetAllValues<K, V>(string key)
        {
            return _cache.HashGetAllValues<K, V>(key, default(V), _db.DefaultOperate);
        }
        /// <summary>
        /// 删除哈希表的值
        /// </summary>
        /// <typeparam name="E"></typeparam>
        /// <param name="key">键</param>
        /// <param name="hashkey">哈希表的键</param>
        /// <returns></returns>
        public bool HashDeleteValue(string key, object hashkey)
        {
            return _cache.HashDeleteValue(key, hashkey, _db.DefaultOperate);
        }
        /// <summary>
        /// 批量删除哈希表的值
        /// </summary>
        /// <typeparam name="E"></typeparam>
        /// <param name="key">键</param>
        /// <param name="hashkeys">要删除哈希表的键</param>
        /// <returns></returns>
        public long HashDeleteValues(string key, IEnumerable hashkeys)
        {
            return _cache.HashDeleteValues(key, hashkeys, _db.DefaultOperate);
        }
        /// <summary>
        /// 哈希表的键是否存在
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="hashkey">哈希表的键</param>
        /// <returns></returns>
        public bool HashExists(string key, object hashkey)
        {
            return _cache.HashExists(key, hashkey, _db.DefaultOperate);
        }
        /// <summary>
        /// 哈希表自增
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="hashkey">哈希键</param>
        /// <returns></returns>
        public long HashIncrement(string key, object hashkey)
        {
            return _cache.HashIncrement(key, hashkey,1,_db.DefaultOperate);
        }
        /// <summary>
        /// 哈希表自增
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="hashkey">哈希键</param>
        /// <param name="value">自增量</param>
        /// <returns></returns>
        public long HashIncrement(string key, object hashkey, long value)
        {
            return _cache.HashIncrement(key, hashkey, value, _db.DefaultOperate);
        }

        /// <summary>
        /// 哈希表自减
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="hashkey">哈希键</param>
        /// <param name="value">自减量</param>
        /// <returns></returns>
        public long HashDecrement(string key, object hashkey, long value)
        {
            return _cache.HashDecrement(key, hashkey, value, _db.DefaultOperate);
        }
        /// <summary>
        /// 哈希表自减
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="hashkey">哈希键</param>
        /// <returns></returns>
        public long HashDecrement(string key, object hashkey)
        {
            return _cache.HashDecrement(key, hashkey, 1, _db.DefaultOperate);
        }
        #endregion
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
        public const string CommandSetList = "SetList";
        public const string CommandGetValues = "GetValues";
        public const string CommandSetValues = "SetValues";
        public const string CommandDeleteValues = "DeleteValues";
        public const string CommandIncrement = "Increment";
        public const string CommandListAdd = "ListAdd";
        public const string CommandListGet = "ListGet";
        public const string CommandListDelete = "ListDelete";
        public const string CommandHashAdd = "HashAdd";
        public const string CommandHashGet = "HashGet";
        public const string CommandHashDelete = "HashDelete";
    }
}
