using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Collections;
using Buffalo.Kernel;
using Buffalo.DB.DataBaseAdapter;
using Buffalo.DB.MessageOutPuters;
using Buffalo.DB.DbCommon;
using Buffalo.DB.CacheManager.MemoryCacheUnit;

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

        public WebCacheAdaper(DBInfo info, string connStr)
        {
            _info = info;
            

            CreatePool(connStr);
        }

        /// <summary>
        /// 所有键
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> GetAllKeys(string pattern)
        {
           
            return _curCache.AllKeys();
            
        }
        public void ClearAll()
        {
            _curCache.ClearAll();
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
                    string expirStr = CacheUnit.CutString(part, expirString.Length);
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
            _curCache = new MemoryStorageManager((double)_expiration.TotalMilliseconds);
        }

        private MemoryStorageManager _curCache = null;
        /// <summary>
        /// 当前缓存
        /// </summary>
        private MemoryStorageManager CurCache
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
                OutPutMessage(QueryCacheCommand.CommandSetDataSet, sql, oper);
            }
            TimeSpan ts = _expiration;
            if (expirSeconds > 0)
            {
                ts = TimeSpan.FromSeconds(expirSeconds);
            }
            if (ts > TimeSpan.MinValue)
            {
                CurCache.SetValue(key, ds, ts.TotalMilliseconds);
            }
            else
            {
                CurCache.SetValue(key, ds,0);
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
        private string GetTableKeyName(string tableName)
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
            sbKey.Append(sql.Length);
            sbKey.Append(":");
            sbKey.Append(PasswordHash.ToMD5String(_info.Name + ":" + sql));
            return sbKey.ToString();
        }

        /// <summary>
        /// 根据SQL语句从缓存中找出数据
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="tableNames">表名集合</param>
        /// <returns></returns>
        public DataSet GetData(IDictionary<string, bool> tableNames, string sql, DataBaseOperate oper)
        {
            string key = GetKey(sql);
            if (_info.SqlOutputer.HasOutput)
            {
                OutPutMessage(QueryCacheCommand.CommandGetDataSet, sql, oper);
            }
            return CurCache.GetValue(key) as DataSet;
        }

        /// <summary>
        /// 通过SQL删除某项
        /// </summary>
        /// <param name="sql"></param>
        public void RemoveBySQL(IDictionary<string, bool> tableNames, string sql, DataBaseOperate oper)
        {
            string key = GetKey(sql);

            CurCache.DeleteValue(key);
            if (_info.SqlOutputer.HasOutput)
            {
                OutPutMessage(QueryCacheCommand.CommandDeleteSQL, sql, oper);
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
                        CurCache.DeleteValue(key);
                    }
                }
                sqlItems.Clear();

            }
            if (_info.SqlOutputer.HasOutput)
            {
                OutPutMessage(QueryCacheCommand.CommandDeleteTable, tableName, oper);
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
                object val = CurCache.GetValue(str);
                dic[str] = val;
            }
            return dic;
        }

        public E GetValue<E>(string key, E defaultValue, DataBaseOperate oper)
        {
            object val = CurCache.GetValue(key);
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
                        if (CurCache.GetValue(key) != null)
                        {
                            return false;
                        }

                        break;
                    case SetValueType.Replace:
                        if (CurCache.GetValue(key) == null)
                        {
                            return false;
                        }

                        break;
                    default:
                        break;
                }


                if (ts > TimeSpan.MinValue)
                {
                    CurCache.SetValue(key, value,  ts.TotalMilliseconds);
                }
                else
                {
                    CurCache.SetValue(key, value,0);
                }
            }
            return true;
        }

        public void DeleteValue(string key, DataBaseOperate oper)
        {
            CurCache.DeleteValue(key);
        }

        public long DoIncrement(string key, ulong inc, DataBaseOperate oper)
        {
            object lok = _lockObjects.GetObject(key);
            lock (lok)
            {
                object oval = CurCache.GetValue(key);
                if (oval == null)
                {
                    oval = 1;
                }
                long value = ValueConvertExtend.ConvertValue<long>(oval);
                value += (long)inc;
                //_cache[key] = value;
                if (_expiration > TimeSpan.MinValue)
                {
                    CurCache.SetValue(key, value,  _expiration.TotalMilliseconds);
                }
                else
                {
                    CurCache.SetValue(key, value,0);
                }
                return value;
            }
        }
        public long DoDecrement(string key, ulong dec, DataBaseOperate oper)
        {
            object lok = _lockObjects.GetObject(key);
            lock (lok)
            {
                object oval = CurCache.GetValue(key);
                if (oval == null)
                {
                    oval = 1;
                }
                long value = ValueConvertExtend.ConvertValue<long>(oval);
                value -= (long)dec;
                //_cache[key] = value;
                if (_expiration > TimeSpan.MinValue)
                {
                    CurCache.SetValue(key, value, _expiration.TotalMilliseconds);
                }
                else
                {
                    CurCache.SetValue(key, value, 0);
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
            return CurCache.GetValue(key) as IList;
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
                CurCache.SetValue(key, lstEntity,  ts.TotalMilliseconds);
            }
            else
            {
                CurCache.SetValue(key, lstEntity,0);
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
            object val = CurCache.GetValue(key);
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
            object val = CurCache.GetValue(key);
            return val != null;
        }

        public bool SetValue(string key, object value, SetValueType type, int expirSeconds, DataBaseOperate oper)
        {
            return SetValue<object>(key, value, type, expirSeconds, oper);
        }

        #endregion
    }


}
