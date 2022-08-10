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
using System.Collections.Concurrent;
using Buffalo.DB.CacheManager.CacheCollection;
using Buffalo.Kernel.Collections;

namespace Buffalo.DB.CacheManager
{
    /// <summary>
    /// ASPNET��Cache
    /// </summary>
    public class WebCacheAdaper : LocalCacheBase, ICacheAdaper 
    {
        /// <summary>
        /// ����ʱ��(����)
        /// </summary>
        protected TimeSpan _expiration = TimeSpan.MinValue;
        /// <summary>
        /// ������
        /// </summary>
        protected LockObjects<string> _lockObjects = new LockObjects<string>();
        /// <summary>
        /// ����ʱ��(����)
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
                return _curCache;
            }
        }
        /// <summary>
        /// ��������redis
        /// </summary>
        public void ReconnectClient()
        {


        }
        /// <summary>
        /// ���м�
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
        /// �������ӳ�
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
                        throw new ArgumentException("���ݱ��������������1-999999999��ֵ");
                    }
                    if (mins <= 0 || mins >= 999999999)
                    {
                        throw new ArgumentException("���ݱ��������������1-999999999��ֵ");
                    }
                    _expiration = TimeSpan.FromMinutes((double)mins);
                }
            }
        }

        private Cache _curCache = null;
        /// <summary>
        /// ��ǰ����
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
        /// ���ݿ���Ϣ
        /// </summary>
        public DBInfo Info
        {
            get { return _info; }
        }
        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="tableName">����</param>
        /// <param name="sql">SQL��</param>
        /// <param name="dt">����</param>
        /// <returns></returns>
        public bool SetData(IDictionary<string, bool> tableNames, string sql, DataSet ds, TimeSpan expir, DataBaseOperate oper) 
        {
            string key = GetKey(sql);
            ArrayList sqlItems = null;
            //��ӱ��Ӧ��SQL���ֵ
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
            TimeSpan ts = LocalCacheBase.GetExpir( _expiration, expir);
            
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
        /// ��ȡ����
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
        /// ���ɱ�����Ӧ��Key
        /// </summary>
        /// <param name="tableName">����</param>
        /// <returns></returns>
        private  string GetTableKeyName(string tableName)
        {
            StringBuilder sbRet = new StringBuilder(tableName.Length + 20);
            sbRet.Append("___Table:");
            sbRet.Append(GetTableName(tableName));
            return sbRet.ToString();
        }
        /// <summary>
        /// ͨ��SQL��ȡ��
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        private string GetKey(string sql) 
        {
            StringBuilder sbKey = new StringBuilder(100);
            sbKey.Append("BuffaloCache:");
            sbKey.Append(sql.Length );
            sbKey.Append(":");
            sbKey.Append(PasswordHash.ToMD5String(_info.FullName + ":"+sql));
            return sbKey.ToString();
        }

        /// <summary>
        /// ����SQL���ӻ������ҳ�����
        /// </summary>
        /// <param name="sql">SQL���</param>
        /// <param name="tableNames">��������</param>
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
        /// ͨ��SQLɾ��ĳ��
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
        /// ͨ������ɾ��������
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

        #region ICacheAdaper ��Ա


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
        public bool SetValue<E>(string key, E value, SetValueType type, TimeSpan expir, DataBaseOperate oper)
        {
            TimeSpan ts = LocalCacheBase.GetExpir(_expiration, expir);
            
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
            public bool SetValue(string key, object value, SetValueType type, TimeSpan expir, DataBaseOperate oper)
        {
            //_cache[key] = value;
            TimeSpan ts = LocalCacheBase.GetExpir(_expiration, expir);
            
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

        public bool DeleteValue(string key, DataBaseOperate oper)
        {
            object lok = _lockObjects.GetObject(key);
            lock (lok)
            {
                object val = CurCache.Get(key);
                if (val != null)
                {
                    CurCache.Remove(key);
                    return true;
                }
                return false;
            }
                
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

        public bool SetEntityList(string key, IList lstEntity, TimeSpan expir, DataBaseOperate oper)
        {
            TimeSpan ts = LocalCacheBase.GetExpir(_expiration, expir);
            

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

        /// <summary>
        /// ��ȡ��ϣ��Ĳ�����ʽ
        /// </summary>
        /// <param name="key"></param>
        /// <param name="oper"></param>
        /// <returns></returns>
        public ICacheHash GetHashMap(string key, DataBaseOperate oper) 
        {
            IDictionary<string, object> dic = CurCache.Get(key) as IDictionary<string, object>;
            if (dic == null) 
            {
                dic = new Dictionary<string, object>();
                CurCache.Insert(key, dic);
            }
            return new MemoryCacheHash(dic);
        }
        /// <summary>
        /// ��ȡ��ϣ��Ĳ�����ʽ
        /// </summary>
        /// <param name="key"></param>
        /// <param name="oper"></param>
        /// <returns></returns>
        public ICacheList GetList(string key, DataBaseOperate oper)
        {
            IList dic = CurCache.Get(key) as IList;
            if (dic == null)
            {
                dic = new List<object>();
                CurCache.Insert(key, dic);
            }
            return new MemoryCacheList(dic);
        }
        /// <summary>
        /// ��ȡ���Ĳ�����ʽ
        /// </summary>
        /// <param name="key"></param>
        /// <param name="oper"></param>
        /// <returns></returns>
        public ICacheLock GetCacheLock(string key, DataBaseOperate oper) 
        {
            
            return new MemoryCacheLock(key);
        }

        /// <summary>
        /// ��ȡ�����Ĳ�����ʽ
        /// </summary>
        /// <param name="key"></param>
        /// <param name="oper"></param>
        /// <returns></returns>
        public ICacheSortedSet GetSortedSet(string key, DataBaseOperate oper) 
        {
            SortedSet<SortedSetItem> lst = CurCache.Get(key) as SortedSet<SortedSetItem>;
            if (lst == null)
            {
                lst = new SortedSet<SortedSetItem>();
                CurCache.Insert(key, lst);
            }
            return new MemoryCacheSortedSet(lst);
        }




        

        public object GetClient()
        {
            return CurCache;
        }



        #region ICacheAdaper ��Ա


        public object GetValue(string key, DataBaseOperate oper)
        {
            object val = CurCache.Get(key);
            return val;
        }
        /// <summary>
        /// Key�Ƿ����
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
        /// ��ȡ����
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        protected IList GetCacheList(string key)
        {
            IList ret = CurCache.Get(key) as IList;
            if (ret == null)
            {
                ret =new ArrayList();
                CurCache.Insert(key, ret);
            }
            return ret;
        }
        /// <summary>
        /// ��ȡ��ϣ��
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        protected IDictionary GetCacheHash(string key)
        {
            IDictionary ret = CurCache.Get(key) as IDictionary;
            if (ret == null)
            {
                ret = new Hashtable();
                CurCache.Insert(key, ret);
            }
            return ret;
        }
        
        public bool SetKeyExpire(string key, TimeSpan expir, DataBaseOperate oper)
        {
            TimeSpan ts = LocalCacheBase.GetExpir(_expiration, expir);
            object oval = CurCache.Get(key);
            if (oval == null)
            {
                return false;
            }
            if (ts > TimeSpan.MinValue)
            {
                CurCache.Insert(key, oval, null, DateTime.MaxValue, ts);
            }
            else
            {
                CurCache.Insert(key, oval);
            }
            return true;
        }
        #endregion
    }
}
