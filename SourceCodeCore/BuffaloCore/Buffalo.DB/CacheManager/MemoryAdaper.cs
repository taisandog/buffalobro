using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Collections;
using Buffalo.Kernel;
using Buffalo.DB.DataBaseAdapter;
using Buffalo.DB.MessageOutPuters;
using Buffalo.DB.DbCommon;

namespace Buffalo.DB.CacheManager
{
    /// <summary>
    /// ϵͳ�ڴ�Ļ���������
    /// </summary>
    public class MemoryAdaper : ICacheAdaper 
    {
        public MemoryAdaper(DBInfo info) 
        {
            _info = info;
        }

        private Hashtable _cache = Hashtable.Synchronized(new Hashtable());
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
            //���ӱ���Ӧ��SQL���ֵ
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
            _cache[key] = ds;
            
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
            return _cache[key] as DataSet;
        }

        /// <summary>
        /// ͨ��SQLɾ��ĳ��
        /// </summary>
        /// <param name="sql"></param>
        public void RemoveBySQL(IDictionary<string, bool> tableNames, string sql, DataBaseOperate oper) 
        {
            string key = GetKey(sql);
            
            _cache.Remove(key);
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
                        _cache.Remove(key);
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

        public bool SetValue<E>(string key, E value,SetValueType type, int expirSeconds, DataBaseOperate oper)
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

        public void DeleteValue(string key, DataBaseOperate oper)
        {
            _cache.Remove(key);
        }

        public long DoIncrement(string key, ulong inc, DataBaseOperate oper)
        {
            lock (_cache)
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
            lock (_cache)
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
                OutPutMessage(QueryCache.CommandGetList, key, oper);
            }
            return _cache[key] as IList;
        }

        public bool SetEntityList(string key, IList lstEntity, int expirSeconds, DataBaseOperate oper)
        {
            _cache[key] = lstEntity;
            return true;
        }
        /// <summary>
        /// ���м�
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> GetAllKeys(string pattern)
        {
            List<string> keys = new List<string>();
            IDictionaryEnumerator cacheEnum = _cache.GetEnumerator();
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

        public object GetClient()
        {
            return _cache;
        }

        #region ICacheAdaper ��Ա


        public object GetValue(string key, DataBaseOperate oper)
        {
            return _cache[key];
        }
        /// <summary>
        /// Key�Ƿ����
        /// </summary>
        /// <param name="key"></param>
        /// <param name="oper"></param>
        /// <returns></returns>
        public bool ExistsKey(string key, DataBaseOperate oper)
        {
            return _cache.ContainsKey(key);
        }
        public bool SetValue(string key, object value,SetValueType type, int expirSeconds, DataBaseOperate oper)
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

        #endregion
    }

    
}