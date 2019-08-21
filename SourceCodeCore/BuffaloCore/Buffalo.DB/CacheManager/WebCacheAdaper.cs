using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using Buffalo.DB.DataBaseAdapter;
using System.Data;
using Buffalo.DB.DbCommon;
using System.Web;
using Buffalo.Kernel;
using Buffalo.DB.MessageOutPuters;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using System.Reflection;
using Buffalo.Kernel.FastReflection;

namespace Buffalo.DB.CacheManager
{
    /// <summary>
    /// ASPNET��Cache
    /// </summary>
    public class WebCacheAdaper : ICacheAdaper 
    {
        /// <summary>
        /// ����ʱ��(����)
        /// </summary>
        protected TimeSpan _expiration = TimeSpan.MinValue;
        /// <summary>
        /// ����ʱ��(����)
        /// </summary>
        public TimeSpan Expiration
        {
            get { return _expiration; }
        }
        private MemoryCache _curCache = null;
        /// <summary>
        /// ��ǰ����
        /// </summary>
        private MemoryCache CurCache
        {
            get
            {
                return _curCache;
            }
        }
        public WebCacheAdaper(DBInfo info,string connStr) 
        {
            _info = info;

            NewCache();

            CreatePool(connStr);
        }
        private static GetFieldValueHandle _allKeyHandle =FastFieldGetSet.GetGetValueHandle(typeof(MemoryCache).GetField("_entries", BindingFlags.Instance | BindingFlags.NonPublic));
        private void NewCache()
        {
            MemoryCacheOptions option = new MemoryCacheOptions();
            _curCache = new MemoryCache(Options.Create(option));
        }
        /// <summary>
        /// ���м�
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> GetAllKeys(string pattern) 
        {
            object entries = _allKeyHandle.Invoke(_curCache);
            IDictionary cacheItems = entries as IDictionary;
            List<string> keys = new List<string>();
            if (cacheItems == null) return keys;
            foreach (DictionaryEntry cacheItem in cacheItems)
            {
                keys.Add(cacheItem.Key.ToString());
            }
            return keys;
        }
        public void ClearAll()
        {
            _curCache.Dispose();
            NewCache();
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
        public bool SetData(IDictionary<string, bool> tableNames, string sql, DataSet ds, int expirSeconds, DataBaseOperate oper) 
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
                OutPutMessage(QueryCache.CommandSetDataSet, sql,oper);
            }
            TimeSpan ts = _expiration;
            if (expirSeconds > 0)
            {
                ts = TimeSpan.FromSeconds(expirSeconds);
            }

            MemoryCacheEntryOptions option = new MemoryCacheEntryOptions();

            _curCache.Set(key, ds, option);


            return ExistsKey(key,oper);
        }
        /// <summary>
        /// ��ȡ����
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
            sbKey.Append(PasswordHash.ToMD5String(_info.Name+":"+sql));
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
                OutPutMessage(QueryCache.CommandGetDataSet, sql,oper);
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
                OutPutMessage(QueryCache.CommandDeleteSQL, sql,oper);
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
                OutPutMessage(QueryCache.CommandDeleteTable, tableName,oper);
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
        public bool SetValue<E>(string key, E value, SetValueType type, int expirSeconds, DataBaseOperate oper)
        {
            //_cache[key] = value;
            TimeSpan ts = _expiration;
            if (expirSeconds > 0)
            {
                ts = TimeSpan.FromSeconds(expirSeconds);
            }

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

            ICacheEntry entry = _curCache.CreateEntry(key);
            entry.Value = value;
            if (ts > TimeSpan.MinValue)
            {
                entry.SlidingExpiration = ts;
            }

            return ExistsKey(key,oper);
        }

        public void DeleteValue(string key, DataBaseOperate oper)
        {
            CurCache.Remove(key);
        }

        public long DoIncrement(string key, ulong inc, DataBaseOperate oper)
        {
            
            object oval = CurCache.Get(key);
            if (oval == null)
            {
                oval = 1;
            }
            long value = ValueConvertExtend.ConvertValue<long>(oval);
            value += (long)inc;
            //_cache[key] = value;
            ICacheEntry entry = _curCache.CreateEntry(key);
            entry.Value = value;
            if (ts > TimeSpan.MinValue)
            {
                entry.SlidingExpiration = ts;
            }

            return ExistsKey(key, oper);
        }
        public long DoDecrement(string key, ulong dec, DataBaseOperate oper)
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
        #endregion


        public IList GetEntityList(string key, Type entityType, DataBaseOperate oper)
        {
            if (_info.SqlOutputer.HasOutput)
            {
                OutPutMessage(QueryCache.CommandGetList, key, oper);
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
            return _curCache.TryGetValue(key, out _);
        }

        public bool SetValue(string key, object value,SetValueType type, int expirSeconds, DataBaseOperate oper)
        {
            return SetValue<object>(key, value,type, expirSeconds, oper);
        }

        #endregion
    }
}
