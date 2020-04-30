using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using Buffalo.DB.DataBaseAdapter;
using System.Data;
using Buffalo.DB.DbCommon;
using System.Web;
using System.Web.Caching;
using Buffalo.Kernel;
using Buffalo.DB.MessageOutPuters;

namespace Buffalo.DB.CacheManager
{
    /// <summary>
    /// ASPNET的Cache
    /// </summary>
    public class WebCacheAdaper : ICacheAdaper 
    {
        /// <summary>
        /// 过期时间(分钟)
        /// </summary>
        protected TimeSpan _expiration = TimeSpan.MinValue;
        /// <summary>
        /// 锁对象
        /// </summary>
        protected LockObjects<string> _lockObjects = new LockObjects<string>();
        /// <summary>
        /// 过期时间(分钟)
        /// </summary>
        public TimeSpan Expiration
        {
            get { return _expiration; }
        }
        
        public WebCacheAdaper(DBInfo info,string connStr) 
        {
            _info = info;
            if (CommonMethods.IsWebContext)
            {
                _curCache = HttpContext.Current.Cache;
            }
            else 
            {
                _curCache = HttpRuntime.Cache;
            }
            
            CreatePool(connStr);
        }
        
        /// <summary>
        /// 所有键
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> GetAllKeys(string pattern) 
        {
            List<string> keys = new List<string>();
            IDictionaryEnumerator cacheEnum = _curCache.GetEnumerator();
            while (cacheEnum.MoveNext())
            {
                string val = ValueConvertExtend.ConvertValue<string>(cacheEnum.Key);
                if (!CommonMethods.IsPatternMatch(val, pattern)) 
                {
                    continue;
                }
                keys.Add(val);
            }
            return keys;
        }
        public void ClearAll()
        {
            List<string> cacheKes = new List<string>(_curCache.Count);
            IDictionaryEnumerator cacheEnum = _curCache.GetEnumerator();
            while (cacheEnum.MoveNext())
            {
                cacheKes.Add(cacheEnum.Key.ToString());
            }
            foreach (string key in cacheKes)
            {
                _curCache.Remove(key);
            }
        }
        /// <summary>
        /// 创建连接池
        /// </summary>
        /// <param name="connStr"></param>
        /// <returns></returns>
        private void CreatePool(string connStr)
        {
            string[] conStrs = connStr.Split(';');
            
            string expirString = "expir=";

            //List<string> lstServers = new List<string>();
            string part = null;
            foreach (string lpart in conStrs)
            {
                if (string.IsNullOrEmpty(lpart))
                {
                    continue;
                }
                part = lpart.Trim();
                
                if (part.IndexOf(expirString, StringComparison.CurrentCultureIgnoreCase) == 0)
                {
                    string expirStr =CacheUnit.CutString(part,expirString.Length);
                    int mins = 30;
                    if (!int.TryParse(expirStr, out mins))
                    {
                        throw new ArgumentException("数据保存分钟数必须是1-999999999的值");
                    }
                    if (mins <= 0 || mins >= 999999999)
                    {
                        throw new ArgumentException("数据保存分钟数必须是1-999999999的值");
                    }
                    _expiration = TimeSpan.FromMinutes((double)mins);
                }
            }
        }

        private Cache _curCache = null;
        /// <summary>
        /// 当前缓存
        /// </summary>
        private Cache CurCache 
        {
            get 
            {
                return _curCache;
            }
        }

        private Hashtable _hsToKey = Hashtable.Synchronized(new Hashtable());
        private DBInfo _info;
        /// <summary>
        /// 数据库信息
        /// </summary>
        public DBInfo Info
        {
            get { return _info; }
        }
        /// <summary>
        /// 设置数据
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="sql">SQL名</param>
        /// <param name="dt">数据</param>
        /// <returns></returns>
        public bool SetData(IDictionary<string, bool> tableNames, string sql, DataSet ds, int expirSeconds, DataBaseOperate oper) 
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
            TimeSpan ts = _expiration;
            if (expirSeconds > 0)
            {
                ts = TimeSpan.FromSeconds(expirSeconds);
            }
            if (ts > TimeSpan.MinValue)
            {
                CurCache.Insert(key, ds, null, DateTime.MaxValue, ts);
            }
            else
            {
                CurCache.Insert(key, ds);
            }

            
            
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
            sbInfo.Append(_info.Name);
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
            sbKey.Append(PasswordHash.ToMD5String(_info.Name+":"+sql));
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
            return CurCache.Get(key) as DataSet;
        }

        /// <summary>
        /// 通过SQL删除某项
        /// </summary>
        /// <param name="sql"></param>
        public void RemoveBySQL(IDictionary<string, bool> tableNames, string sql, DataBaseOperate oper) 
        {
            string key = GetKey(sql);
            
            CurCache.Remove(key);
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
                        CurCache.Remove(key);
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


        public IDictionary<string, object> GetValues(string[] keys, DataBaseOperate oper)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>(keys.Length);
            foreach (string str in keys) 
            {
                object val = CurCache.Get(str);
                dic[str] = val;
            }
            return dic;
        }

        public E GetValue<E>(string key,E defaultValue, DataBaseOperate oper)
        {
            object val = CurCache.Get(key);
            if (val == null) 
            {
                return defaultValue;
            }
            if (val is E)
            {
                return (E)val;
            }

            return ValueConvertExtend.ConvertValue<E>(val);
        }
        public bool SetValue<E>(string key, E value, SetValueType type, int expirSeconds, DataBaseOperate oper)
        {
            TimeSpan ts = _expiration;
            if (expirSeconds > 0)
            {
                ts = TimeSpan.FromSeconds(expirSeconds);
            }
            object lok = _lockObjects.GetObject(key);
            lock (lok)
            {
                switch (type)
                {
                    case SetValueType.AddNew:
                        if (CurCache.Get(key) != null)
                        {
                            return false;
                        }

                        break;
                    case SetValueType.Replace:
                        if (CurCache.Get(key) == null)
                        {
                            return false;
                        }

                        break;
                    default:
                        break;
                }


                if (ts > TimeSpan.MinValue)
                {
                    CurCache.Insert(key, value, null, DateTime.MaxValue, ts);
                }
                else
                {
                    CurCache.Insert(key, value);
                }
            }
            return true;
        }
            public bool SetValue(string key, object value, SetValueType type, int expirSeconds, DataBaseOperate oper)
        {
            //_cache[key] = value;
            TimeSpan ts = _expiration;
            if (expirSeconds > 0)
            {
                ts = TimeSpan.FromSeconds(expirSeconds);
            }
            object lok = _lockObjects.GetObject(key);
            lock (lok)
            {
                switch (type)
                {
                    case SetValueType.AddNew:
                        if (CurCache.Get(key) != null)
                        {
                            return false;
                        }

                        break;
                    case SetValueType.Replace:
                        if (CurCache.Get(key) == null)
                        {
                            return false;
                        }

                        break;
                    default:
                        break;
                }


                if (ts > TimeSpan.MinValue)
                {
                    CurCache.Insert(key, value, null, DateTime.MaxValue, ts);
                }
                else
                {
                    CurCache.Insert(key, value);
                }
            }
            return true;
        }

        public void DeleteValue(string key, DataBaseOperate oper)
        {
            CurCache.Remove(key);
        }

        public long DoIncrement(string key, ulong inc, DataBaseOperate oper)
        {
            object lok = _lockObjects.GetObject(key);
            lock (lok)
            {
                object oval = CurCache.Get(key);
                if (oval == null)
                {
                    oval = 1;
                }
                long value = ValueConvertExtend.ConvertValue<long>(oval);
                value += (long)inc;
                //_cache[key] = value;
                if (_expiration > TimeSpan.MinValue)
                {
                    CurCache.Insert(key, value, null, DateTime.MaxValue, _expiration);
                }
                else
                {
                    CurCache.Insert(key, value);
                }
                return value;
            }
        }
        public long DoDecrement(string key, ulong dec, DataBaseOperate oper)
        {
            object lok = _lockObjects.GetObject(key);
            lock (lok)
            {
                object oval = CurCache.Get(key);
                if (oval == null)
                {
                    oval = 1;
                }
                long value = ValueConvertExtend.ConvertValue<long>(oval);
                value -= (long)dec;
                //_cache[key] = value;
                if (_expiration > TimeSpan.MinValue)
                {
                    CurCache.Insert(key, value, null, DateTime.MaxValue, _expiration);
                }
                else
                {
                    CurCache.Insert(key, value);
                }
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
            return CurCache.Get(key) as IList;
        }

        public bool SetEntityList(string key, IList lstEntity, int expirSeconds, DataBaseOperate oper)
        {
            //_cache[key] = lstEntity;
            TimeSpan ts = _expiration;
            if (expirSeconds > 0)
            {
                ts = TimeSpan.FromSeconds(expirSeconds);
            }

            if (ts > TimeSpan.MinValue)
            {
                CurCache.Insert(key, lstEntity, null, DateTime.MaxValue, ts);
            }
            else
            {
                CurCache.Insert(key, lstEntity);
            }
           
            return true;
        }


        public object GetClient()
        {
            return CurCache;
        }

        #region ICacheAdaper 成员


        public object GetValue(string key, DataBaseOperate oper)
        {
            object val = CurCache.Get(key);
            return val;
        }
        /// <summary>
        /// Key是否存在
        /// </summary>
        /// <param name="key"></param>
        /// <param name="oper"></param>
        /// <returns></returns>
        public bool ExistsKey(string key, DataBaseOperate oper)
        {
            object val = CurCache.Get(key);
            return val!=null;
        }

        /// <summary>
        /// 获取集合
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private IList GetCacheList(string key)
        {
            IList ret = CurCache.Get(key) as IList;
            if (ret == null)
            {
                ret = new List<object>();
                CurCache.Insert(key,ret);
            }
            return ret;
        }
        public long ListAddValue<E>(string key, long index, E value, SetValueType setType, DataBaseOperate oper)
        {
            IList lst = GetCacheList(key);
            lock (lst)
            {
                if (index < 0)
                {
                    lst.Add(value);
                }
                lst.Insert((int)index, value);
                return 1;
            }
        }

        public E ListGetValue<E>(string key, long index, E defaultValue, DataBaseOperate oper)
        {

            IList lst = GetCacheList(key);
            lock (lst)
            {
                if (lst.Count <= 0)
                {
                    return defaultValue;
                }
                if (index < 0)
                {
                    index = lst.Count - 1;
                }
                return lst[(int)index].ConvertTo<E>(defaultValue);
            }
        }

        public long ListGetLength(string key, DataBaseOperate oper)
        {

            IList lst = GetCacheList(key);

            return lst.Count;
        }

        public E ListPopValue<E>(string key, bool isPopEnd, E defaultValue, DataBaseOperate oper)
        {

            IList lst = GetCacheList(key);
            lock (lst)
            {
                if (lst.Count <= 0)
                {
                    return defaultValue;
                }
                E ret = defaultValue;
                int index = 0;
                if (isPopEnd)
                {
                    index = lst.Count - 1;
                }
                ret = lst[index].ConvertTo<E>(defaultValue);
                lst.RemoveAt(index);
                return ret;
            }
        }

        public long ListRemoveValue(string key, object value, long count, DataBaseOperate oper)
        {

            IList lst = GetCacheList(key);
            lock (lst)
            {
                int ret = 0;
                for (int i = lst.Count - 1; i >= 0; i--)
                {
                    if (lst[i] == value)
                    {
                        lst.RemoveAt(i);
                        ret++;
                        if (count > 0 && ret >= count)
                        {
                            break;
                        }
                    }
                }
                return ret;
            }
        }

        public List<E> ListAllValues<E>(string key, long start, long end, DataBaseOperate oper)
        {
            IList lst = GetCacheList(key);
            lock (lst)
            {
                List<E> retlst = new List<E>(lst.Count);
                int s = (int)start;
                int e = (int)end;
                if (e <= 0)
                {
                    e = lst.Count - 1;
                }
                for (int i = s; i < e; i++)
                {
                    object oval = lst;
                    retlst.Add(oval.ConvertTo<E>());
                }
                return retlst;
            }
        }

        #endregion
    }
}
