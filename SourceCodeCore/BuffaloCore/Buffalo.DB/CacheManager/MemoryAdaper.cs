using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Collections;
using Buffalo.Kernel;
using Buffalo.DB.DataBaseAdapter;
using Buffalo.DB.MessageOutPuters;
using Buffalo.DB.DbCommon;
using System.Collections.Concurrent;
using Buffalo.DB.CacheManager.CacheCollection;
using Buffalo.Kernel.Collections;
using System.Threading.Tasks;
using Buffalo.Kernel.Defaults;

namespace Buffalo.DB.CacheManager
{
    /// <summary>
    /// 系统内存的缓存适配器
    /// </summary>
    public class MemoryAdaper : LocalCacheBase, ICacheAdaper 
    {
        public MemoryAdaper(DBInfo info) 
        {
            _info = info;
        }
        /// <summary>
        /// 锁对象
        /// </summary>
        protected LockObjects<string> _lockObjects = new LockObjects<string>();

        private Hashtable _cache = Hashtable.Synchronized(new Hashtable());
        private Hashtable _hsToKey = Hashtable.Synchronized(new Hashtable());
        private DBInfo _info;
        /// <summary>
        /// 数据库信息
        /// </summary>
        public DBInfo Info
        {
            get { return _info; }
        }

        public object ConnectConfiguration 
        {
            get 
            {
                return null;
            }
            set { }
        }

        public object ConnectClient 
        {
            get 
            {
                return _cache;
            }
        }
        /// <summary>
        /// 重连连接redis
        /// </summary>
        public void ReconnectClient()
        {
            

        }
        /// <summary>
        /// 设置数据
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="sql">SQL名</param>
        /// <param name="dt">数据</param>
        /// <returns></returns>
        public bool SetData(IDictionary<string, bool> tableNames, string sql, DataSet ds, TimeSpan expir, DataBaseOperate oper) 
        {
            string key = GetKey(sql);
            ArrayList sqlItems = null;
            //添加表对应的SQL语句值
            foreach (KeyValuePair<string, bool> kvptableName in tableNames)
            {
                string tableName = kvptableName.Key;
                sqlItems = _hsToKey[tableName] as ArrayList;
                if (sqlItems == null)
                {

                    sqlItems = ArrayList.Synchronized(new ArrayList());
                    _hsToKey[tableName] = sqlItems;

                }

                sqlItems.Add(key);

            }
            if (_info.SqlOutputer.HasOutput)
            {
                OutPutMessage(QueryCacheCommand.CommandSetDataSet, sql,oper);
            }
            _cache[key] = ds;
            
            return true;
        }
        /// <summary>
        /// 获取表名
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        private string GetTableName(string tableName)
        {
            StringBuilder sbInfo = new StringBuilder(tableName.Length + 10);
            sbInfo.Append(_info.FullName);
            sbInfo.Append(".");
            sbInfo.Append(tableName);
            return sbInfo.ToString();
        }

        /// <summary>
        /// 生成表名对应的Key
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <returns></returns>
        private  string GetTableKeyName(string tableName)
        {
            StringBuilder sbRet = new StringBuilder(tableName.Length + 20);
            sbRet.Append("___Table:");
            sbRet.Append(GetTableName(tableName));
            return sbRet.ToString();
        }
        /// <summary>
        /// 通过SQL获取键
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        private string GetKey(string sql) 
        {
            StringBuilder sbKey = new StringBuilder(100);
            sbKey.Append("BuffaloCache:");
            sbKey.Append(sql.Length );
            sbKey.Append(":");
            sbKey.Append(PasswordHash.ToMD5String(_info.FullName+":"+sql));
            return sbKey.ToString();
        }

        /// <summary>
        /// 根据SQL语句从缓存中找出数据
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="tableNames">表名集合</param>
        /// <returns></returns>
        public  DataSet GetData(IDictionary<string, bool> tableNames, string sql, DataBaseOperate oper)
        {
            string key = GetKey(sql);
            if (_info.SqlOutputer.HasOutput)
            {
                OutPutMessage(QueryCacheCommand.CommandGetDataSet, sql,oper);
            }
            return _cache[key] as DataSet;
        }

        /// <summary>
        /// 通过SQL删除某项
        /// </summary>
        /// <param name="sql"></param>
        public void RemoveBySQL(IDictionary<string, bool> tableNames, string sql, DataBaseOperate oper) 
        {
            string key = GetKey(sql);
            
            _cache.Remove(key);
            if (_info.SqlOutputer.HasOutput)
            {
                OutPutMessage(QueryCacheCommand.CommandDeleteSQL, sql,oper);
            }

        }
        /// <summary>
        /// 通过表名删除关联项
        /// </summary>
        /// <param name="sql"></param>
        public void RemoveByTableName(string tableName, DataBaseOperate oper)
        {
            ArrayList sqlItems = _hsToKey[tableName] as ArrayList;

            if (sqlItems != null)
            {


                foreach (object okey in sqlItems)
                {
                    string key = okey as string;
                    if (!string.IsNullOrEmpty(key))
                    {
                        _cache.Remove(key);
                    }
                }
                sqlItems.Clear();

            }
            if (_info.SqlOutputer.HasOutput)
            {
                OutPutMessage(QueryCacheCommand.CommandDeleteTable, tableName,oper);
            }
        }

        private void OutPutMessage(string type, string message, DataBaseOperate oper)
        {
            oper.OutMessage(MessageType.QueryCache, "SystemMemory", type, message);
        }

        #region ICacheAdaper 成员

        public E GetValue<E>(string key,E defaultValue, DataBaseOperate oper)
        {
            //object val = _cache[key];
            //if (val is E)
            //{
            //    return (E)val;
            //}
            //return (E)Convert.ChangeType(val, typeof(E));
            return ValueConvertExtend.GetMapDataValue<E>(_cache, key, defaultValue);
           
        }

        public IDictionary<string, object> GetValues(string[] keys, DataBaseOperate oper)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>(keys.Length);
            foreach (string str in keys) 
            {
                object val = _cache[str];
                dic[str] = val;
            }
            return dic;
        }

        public bool SetValue<E>(string key, E value,SetValueType type, TimeSpan expir, DataBaseOperate oper)
        {
            switch (type)
            {
                case SetValueType.AddNew:
                    if (_cache[key] != null)
                    {
                        return false;
                    }

                    break;
                case SetValueType.Replace:
                    if (_cache[key] == null)
                    {
                        return false;
                    }

                    break;
                default:
                    break;
            }
            _cache[key] = value;
            return true;
        }

        public bool DeleteValue(string key, DataBaseOperate oper)
        {
            object lok = _lockObjects.GetObject(key);
            lock (lok)
            {
                bool ret = _cache.ContainsKey(key);
                if (ret)
                {
                    _cache.Remove(key);
                }
                return ret;
            }
            
        }

        public long DoIncrement(string key, ulong inc, DataBaseOperate oper)
        {
            object lok = _lockObjects.GetObject(key);
            lock (lok)
            {
                object oval = _cache[key];

                long value = ValueConvertExtend.ConvertValue<long>(oval);
                value += (long)inc;
                _cache[key] = value;
                return value;
            }
        }
        public long DoDecrement(string key, ulong dec, DataBaseOperate oper)
        {
            object lok = _lockObjects.GetObject(key);
            lock (lok)
            {
                object oval = _cache[key];
                long value = ValueConvertExtend.ConvertValue<long>(oval);
                value -= (long)dec;
                _cache[key] = value;
                return value;
            }
        }
        #endregion


        public IList GetEntityList(string key, Type entityType, DataBaseOperate oper)
        {
            if (_info.SqlOutputer.HasOutput)
            {
                OutPutMessage(QueryCacheCommand.CommandGetList, key, oper);
            }
            return _cache[key] as IList;
        }

        public bool SetEntityList(string key, IList lstEntity, TimeSpan expir, DataBaseOperate oper)
        {
            _cache[key] = lstEntity;
            return true;
        }
        /// <summary>
        /// 所有键
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> GetAllKeys(string pattern)
        {
            List<string> keys = new List<string>();
            IDictionaryEnumerator cacheEnum = _cache.GetEnumerator();
            while (cacheEnum.MoveNext())
            {
                string val = ValueConvertExtend.ConvertValue<string>(cacheEnum.Key);
                if (!IsPatternMatch(val, pattern))
                {
                    continue;
                }
                keys.Add(val);
            }
            return keys;
        }
        /// <summary>
        /// 获取哈希表的操作方式
        /// </summary>
        /// <param name="key"></param>
        /// <param name="oper"></param>
        /// <returns></returns>
        public ICacheHash GetHashMap(string key, DataBaseOperate oper)
        {
            IDictionary<string, object> dic = _cache[key] as IDictionary<string, object>;
            if (dic == null)
            {
                dic = new Dictionary<string, object>();
                _cache[key]=dic;
            }
            return new MemoryCacheHash(dic);
        }
        /// <summary>
        /// 获取哈希表的操作方式
        /// </summary>
        /// <param name="key"></param>
        /// <param name="oper"></param>
        /// <returns></returns>
        public ICacheList GetList(string key, DataBaseOperate oper)
        {
            IList dic = _cache[key] as IList;
            if (dic == null)
            {
                dic = new List<object>();
                _cache[key] = dic;
            }
            return new MemoryCacheList(dic);
        }
        /// <summary>
        /// 获取锁的操作方式
        /// </summary>
        /// <param name="key"></param>
        /// <param name="oper"></param>
        /// <returns></returns>
        public ICacheLock GetCacheLock(string key, DataBaseOperate oper)
        {

            return new MemoryCacheLock(key);
        }

        /// <summary>
        /// 获取排序表的操作方式
        /// </summary>
        /// <param name="key"></param>
        /// <param name="oper"></param>
        /// <returns></returns>
        public ICacheSortedSet GetSortedSet(string key, DataBaseOperate oper)
        {
            SortedSet<SortedSetItem> lst = _cache[key] as SortedSet<SortedSetItem>;
            if (lst == null)
            {
                lst = new SortedSet<SortedSetItem>();
                _cache[key] = lst;
            }
            return new MemoryCacheSortedSet(lst);
        }

        public object GetClient()
        {
            return _cache;
        }

        #region ICacheAdaper 成员


        public object GetValue(string key, DataBaseOperate oper)
        {
            return _cache[key];
        }
        /// <summary>
        /// Key是否存在
        /// </summary>
        /// <param name="key"></param>
        /// <param name="oper"></param>
        /// <returns></returns>
        public bool ExistsKey(string key, DataBaseOperate oper)
        {
            return _cache.ContainsKey(key);
        }
        public bool SetValue(string key, object value,SetValueType type, TimeSpan expir, DataBaseOperate oper)
        {
            switch (type)
            {
                case SetValueType.AddNew:
                    if (_cache[key] != null)
                    {
                        return false;
                    }

                    break;
                case SetValueType.Replace:
                    if (_cache[key] == null)
                    {
                        return false;
                    }

                    break;
                default:
                    break;
            }
            _cache[key] = value;
            return true;
        }

        public void ClearAll()
        {
            _cache.Clear();
            _hsToKey.Clear();
        }

        
        ///// <summary>
        ///// 获取集合
        ///// </summary>
        ///// <param name="key"></param>
        ///// <returns></returns>
        //protected override IList GetCacheList(string key)
        //{
        //    IList ret=_cache[key] as IList;
        //    if (ret == null)
        //    {
        //        ret = new ArrayList();
        //        _cache[key] = ret;
        //    }
        //    return ret;
        //}

        ///// <summary>
        ///// 获取哈希表
        ///// </summary>
        ///// <param name="key"></param>
        ///// <returns></returns>
        //protected override IDictionary GetCacheHash(string key)
        //{
        //    IDictionary ret = _cache[key] as IDictionary;
        //    if (ret == null)
        //    {
        //        ret = new Hashtable();
        //        _cache[key]= ret;
        //    }
        //    return ret;
        //}

        public bool SetKeyExpire(string key, TimeSpan expir, DataBaseOperate oper)
        {
            return true;
        }

        public Task<E> GetValueAsync<E>(string key, E defaultValue, DataBaseOperate oper)
        {
            return Task.FromResult(GetValue<E>(key, defaultValue, oper));
        }

        public Task<object> GetValueAsync(string key, DataBaseOperate oper)
        {
            return Task.FromResult(GetValue(key, oper));
        }

        public Task<IDictionary<string, object>> GetValuesAsync(string[] keys, DataBaseOperate oper)
        {
            return Task.FromResult(GetValues(keys, oper));
        }

        public Task<bool> SetValueAsync<E>(string key, E value, SetValueType type, TimeSpan expir, DataBaseOperate oper)
        {
            return Task.FromResult(SetValue(key, value, type, expir, oper));
        }

        public Task<bool> SetValueAsync(string key, object value, SetValueType type, TimeSpan expir, DataBaseOperate oper)
        {
            return Task.FromResult(SetValue(key, value, type, expir, oper));
        }

        public Task<bool> ExistsKeyAsync(string key, DataBaseOperate oper)
        {
            return Task.FromResult(ExistsKey(key, oper));
        }

        public Task<bool> SetKeyExpireAsync(string key, TimeSpan expir, DataBaseOperate oper)
        {
            return Task.FromResult(SetKeyExpire(key, expir, oper));
        }

        public Task ClearAllAsync()
        {
            return Task.FromResult(ClearAllAsync());
        }

        public Task<bool> DeleteValueAsync(string key, DataBaseOperate oper)
        {
            return Task.FromResult(DeleteValue(key, oper));
        }

        public Task<long> DoIncrementAsync(string key, ulong inc, DataBaseOperate oper)
        {
            return Task.FromResult(DoIncrement(key, inc, oper));
        }

        public Task<long> DoDecrementAsync(string key, ulong dec, DataBaseOperate oper)
        {
            return Task.FromResult(DoDecrement(key, dec, oper));
        }

        public Task<IEnumerable<string>> GetAllKeysAsync(string pattern)
        {
            return Task.FromResult(GetAllKeys(pattern));
        }



        #endregion
    }

    
}
