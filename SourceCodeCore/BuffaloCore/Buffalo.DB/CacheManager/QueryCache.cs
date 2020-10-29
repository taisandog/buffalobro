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
    /// �ڴ滺����
    /// </summary>
    public class QueryCache
    {

        
        private DBInfo _db;

        private Dictionary<string, bool> _dicAllowCache = new Dictionary<string, bool>();

        private bool _isAllTableCache;
        /// <summary>
        /// �Ƿ����б�ʹ�û���
        /// </summary>
        public bool IsAllTableCache
        {
            get { return _isAllTableCache; }
        }
        
        /// <summary>
        /// ���ݿ���Ϣ
        /// </summary>
        public DBInfo DBInfo
        {
            get { return _db; }
            internal set { _db = value; }
        }
        private ICacheAdaper _cache;

        /// <summary>
        /// ���������
        /// </summary>
        /// <param name="db">���ݿ�</param>
        public QueryCache(DBInfo db) 
        {
            _db = db;
           
        }
        /// <summary>
        /// ��ʼ������
        /// </summary>
        /// <param name="cache">������</param>
        /// <param name="isAllTableCache">�Ƿ����б����л���</param>
        public void InitCache(ICacheAdaper cache, bool isAllTableCache) 
        {
            _cache = cache;
            _isAllTableCache = isAllTableCache;
        }

        /// <summary>
        /// �Ƿ�ʹ���˻���
        /// </summary>
        public bool HasCache 
        {
            get 
            {
                return _cache != null;
            }
        }

        /// <summary>
        /// �������ʹ�������������
        /// </summary>
        /// <param name="type">����</param>
        /// <param name="connectionString">�����ַ���</param>
        /// <returns></returns>
        public static ICacheAdaper GetCache(DBInfo info, string type, string connectionString)
        {
            if (string.IsNullOrEmpty(type))
            {
                return null;
            }
            string dtype = type.Trim();
            if (dtype.Equals("system", StringComparison.CurrentCultureIgnoreCase))//�ڴ�
            {
                return new MemoryAdaper(info);
            }
            
            ICacheAdaper cache = GetAssemblyCache(info, dtype, connectionString);
            if (cache != null) 
            {
                return cache;
            }

            throw new NotSupportedException("��֧��:" + type + " �Ļ������ͣ���ǰֻ֧��system��Web��memcached��redis���͵Ļ���");
        }

        private static Assembly _cacheAssembly = null;

        

        /// <summary>
        /// ��ȡ�ⲿ���򼯵Ļ���
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
                    throw new MissingMemberException("�Ҳ�����Buffalo.QueryCache,�뱣֤��Ŀ�Ѿ�������Buffalo.QueryCache.dll");
                }
            }
            if (_cacheAssembly == null) 
            {
                throw new MissingMemberException("�Ҳ�����Buffalo.QueryCache,�뱣֤��Ŀ�Ѿ�������Buffalo.QueryCache.dll");
            }
            Type loaderType = _cacheAssembly.GetType("Buffalo.QueryCache.CacheLoader", false, false);
            if (loaderType == null) 
            {
                throw new MissingMemberException("�Ҳ�����Buffalo.QueryCache.CacheLoader,�뱣֤Buffalo.QueryCache.dll��������");
            }
            MethodInfo mInfo = loaderType.GetMethod("GetCache");
            if (loaderType == null)
            {
                throw new MissingMethodException("�Ҳ�������GetCache,�뱣֤Buffalo.QueryCache.dll��������");
            }
            ICacheAdaper cache = mInfo.Invoke(null, new object[] {info,type,connectionString }) as ICacheAdaper;
            return cache;
        }

       
        /// <summary>
        /// ��ȡ�����е�DataSet
        /// </summary>
        /// <param name="tables">��</param>
        /// <param name="sql">SQL���</param>
        /// <param name="lstParam">��������</param>
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
        /// ��ȡ�����е�DataSet
        /// </summary>
        /// <param name="tables">��</param>
        /// <param name="sql">SQL���</param>
        /// <param name="lstParam">��������</param>
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
        /// ��ȡ�����е�DataSet
        /// </summary>
        /// <param name="tables">��</param>
        /// <param name="sql">SQL���</param>
        /// <param name="lstParam">��������</param>
        /// <returns></returns>
        public bool SetDataSet(DataSet ds, IDictionary<string, bool> tables,
            string sql, ParamList lstParam, DataBaseOperate oper)
        {
            return SetDataSet(ds,tables,sql,lstParam,-1,oper);
        }
        /// <summary>
        /// ��ȡ���м�
        /// </summary>
        /// <param name="pattern">ͨ��� ����ʹ�õ�ͨ�����?��*������? ��������һ���ַ���* ��������������ַ�</param>
        /// <param name="oper"></param>
        /// <returns></returns>
        public IEnumerable<string> GetAllKeys(string pattern)
        {
            return _cache.GetAllKeys(pattern);
        }
        /// <summary>
        /// ���������������Ϣ
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
        /// ��ȡ�����е�Reader
        /// </summary>
        /// <param name="tables">��</param>
        /// <param name="sql">SQL���</param>
        /// <param name="lstParam">��������</param>
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
        /// д�뻺���е�Reader
        /// </summary>
        /// <param name="tables">��</param>
        /// <param name="sql">SQL���</param>
        /// <param name="lstParam">��������</param>
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
        /// ɾ����Ļ���
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
        /// �����Ƿ���û���
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
                throw new Exception("��:" + sbBuffer.ToString() + "û����Ϊʹ�û��棬���������ļ���ָ��");
            }
        }

        /// <summary>
        /// �жϱ����Ƿ�����ʹ�û���
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
        /// ������Ҫ����ı�
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public bool SetCacheTable(string tableName) 
        {
            _dicAllowCache[tableName] = true;
            return true;
        }

        /// <summary>
        /// ��ȡֵ
        /// </summary>
        /// <param name="key">��</param>
        /// <param name="valueType">ֵ����</param>
        /// <returns></returns>
        public IDictionary<string, object> GetValues(string[] keys) 
        {
            return _cache.GetValues(keys, _db.DefaultOperate);
        }

        /// <summary>
        /// ��ȡֵ
        /// </summary>
        /// <param name="key">��</param>
        /// <param name="defaultValue">û�м�ʱ���Ĭ��ֵ</param>
        /// <returns></returns>
        public E GetValue<E>(string key,E defaultValue)
        {
            return _cache.GetValue<E>(key,defaultValue, _db.DefaultOperate);
        }
        /// <summary>
        /// ��ȡֵ
        /// </summary>
        /// <param name="key">��</param>
        /// <returns></returns>
        public E GetValue<E>(string key)
        {
            return _cache.GetValue<E>(key, default(E), _db.DefaultOperate);
        }
        /// <summary>
        /// ��ȡֵ
        /// </summary>
        /// <param name="key">��</param>
        /// <returns></returns>
        public object GetValue(string key)
        {
            return _cache.GetValue(key, _db.DefaultOperate);
        }
        /// <summary>
        /// Key�Ƿ����
        /// </summary>
        /// <param name="key">��</param>
        /// <returns></returns>
        public bool ExistsKey(string key)
        {
            return _cache.ExistsKey(key, _db.DefaultOperate);
        }
        /// <summary>
        /// ����key����
        /// </summary>
        /// <param name="key">��</param>
        /// <param name="expirSeconds">����ʱ��(��)</param>
        /// <returns></returns>
        public bool SetKeyExpire(string key, int expirSeconds)
        {
            return _cache.SetKeyExpire(key, expirSeconds, _db.DefaultOperate);
        }
        /// <summary>
        /// ������л���ֵ
        /// </summary>
        /// <returns></returns>
        public void Clear()
        {
            _cache.ClearAll();
        }
        /// <summary>
        /// ����ֵ
        /// </summary>
        /// <param name="key">��</param>
        /// <param name="value">ֵ</param>
        /// <returns>�Ƿ����óɹ�</returns>
        public bool SetValue<E>(string key, E value) 
        {
            return _cache.SetValue<E>(key,value,SetValueType.Set,-1, _db.DefaultOperate);
        }
        /// <summary>
        /// ����ֵ
        /// </summary>
        /// <param name="key">��</param>
        /// <param name="expirSeconds">��ʱ����</param>
        /// <param name="value">ֵ</param>
        /// <param name="type">����ֵ��ʽ</param>
        /// <returns>�Ƿ����óɹ�</returns>
        public bool SetValue<E>(string key, E value, SetValueType type, int expirSeconds)
        {
            return _cache.SetValue<E>(key, value,type, expirSeconds, _db.DefaultOperate);
        }
        /// <summary>
        /// ����ֵ
        /// </summary>
        /// <param name="key">��</param>
        /// <param name="expirSeconds">��ʱ����</param>
        /// <param name="value">ֵ</param>
        /// <param name="type">����ֵ��ʽ</param>
        /// <returns>�Ƿ����óɹ�</returns>
        public bool SetValue(string key, object value,SetValueType type, int expirSeconds)
        {
            return _cache.SetValue(key, value,type, expirSeconds, _db.DefaultOperate);
        }
        /// <summary>
        /// ����ֵ
        /// </summary>
        /// <param name="key">��</param>
        /// <param name="value">ֵ</param>
        /// <returns>�Ƿ����óɹ�</returns>
        public bool SetValue(string key, object value)
        {
            return _cache.SetValue(key, value,SetValueType.Set, -1, _db.DefaultOperate);
        }

        
        /// <summary>
        /// ɾ��ֵ
        /// </summary>
        /// <param name="key">��</param>
        /// <returns></returns>
        public void DeleteValue(string key) 
        {
            _cache.DeleteValue(key, _db.DefaultOperate);
        }
       
        /// <summary>
        /// ����1
        /// </summary>
        /// <param name="key"></param>
        public long DoIncrement(string key)
        {
            return _cache.DoIncrement(key, 1, _db.DefaultOperate);
        }
        /// <summary>
        /// ����1
        /// </summary>
        /// <param name="key"></param>
        public long DoIncrement(string key,ulong inc)
        {
           return  _cache.DoIncrement(key, inc, _db.DefaultOperate);
        }
        /// <summary>
        /// �Լ�1
        /// </summary>
        /// <param name="key"></param>
        public long DoDecrement(string key)
        {
            return _cache.DoDecrement(key, 1, _db.DefaultOperate);
        }
        /// <summary>
        /// �Լ�
        /// </summary>
        /// <param name="key"></param>
        public long DoDecrement(string key, ulong dec)
        {
            return _cache.DoDecrement(key, dec, _db.DefaultOperate);
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="key">��</param>
        /// <param name="entityType">ʵ������</param>
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
        /// ��ȡʵ��
        /// </summary>
        /// <typeparam name="T">ʵ������</typeparam>
        /// <param name="key">��</param>
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
        /// ����ʵ��
        /// </summary>
        /// <param name="key">��</param>
        /// <param name="enity">ʵ��</param>
        /// <returns></returns>
        public bool SetEntity(string key, EntityBase enity)
        {
            return SetEntity(key, -1, enity);
        }
        /// <summary>
        /// ����ʵ��
        /// </summary>
        /// <param name="key">��</param>
        /// <param name="enity">ʵ��</param>
        /// <returns></returns>
        public bool SetEntity(string key, int expirSeconds, EntityBase enity)
        {
            ArrayList lstEntity = new ArrayList();
            lstEntity.Add(enity);
            return SetList(key, expirSeconds, lstEntity);
        }
        /// <summary>
        /// ��ȡ����
        /// </summary>
        /// <param name="key">��</param>
        /// <returns></returns>
        public IList GetList(string key,Type entityType) 
        {
            return _cache.GetEntityList(key,entityType, _db.DefaultOperate);
        }
        /// <summary>
        /// ��ȡ����
        /// </summary>
        /// <param name="key">��</param>
        /// <returns></returns>
        public List<T> GetList<T>(string key) where T:EntityBase
        {
            return _cache.GetEntityList(key,typeof(T), _db.DefaultOperate) as List<T>;
        }

        /// <summary>
        /// ���ü���
        /// </summary>
        /// <param name="key">��</param>
        ///  <param name="expirSeconds">����ʱ��</param>
        /// <param name="lstEntiity">ʵ�弯��</param>
        /// <returns></returns>
        public bool SetList(string key, int expirSeconds, IList lstEntiity)
        {
            return _cache.SetEntityList(key,lstEntiity,expirSeconds, _db.DefaultOperate);
        }
        /// <summary>
        /// ���ü���
        /// </summary>
        /// <param name="key">��</param>
        ///  <param name="expirSeconds">����ʱ��</param>
        /// <param name="lstEntiity">ʵ�弯��</param>
        /// <returns></returns>
        public bool SetList(string key, IList lstEntiity)
        {
            return _cache.SetEntityList(key, lstEntiity, -1, _db.DefaultOperate);
        }

        #region List����
        /// <summary>
        /// ���ӵ��б�
        /// </summary>
        /// <typeparam name="E"></typeparam>
        /// <param name="key">��</param>
        /// <param name="index">����(0Ϊ���ӵ�ͷ����-1Ϊ���ӵ�β��)</param>
        /// <param name="value">ֵ</param>
        /// <param name="setType">����ֵ��ʽ</param>
        /// <returns></returns>
        public long ListAddValue<E>(string key,  E value, long index=-1, SetValueType setType=SetValueType.Set)
        {
            return _cache.ListAddValue<E>(key, index, value, setType, _db.DefaultOperate);
        }
        /// <summary>
        /// ��ȡֵ
        /// </summary>
        /// <typeparam name="E"></typeparam>
        /// <param name="key">��</param>
        /// <param name="index">ֵλ��</param>
        /// <param name="defaultValue">Ĭ��ֵ</param>
        /// <returns></returns>
        public E ListGetValue<E>(string key, long index, E defaultValue=default(E))
        {
            return _cache.ListGetValue<E>(key, index, defaultValue, _db.DefaultOperate);
        }

        /// <summary>
        /// ��ȡ���ϳ���
        /// </summary>
        /// <typeparam name="E"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public long ListGetLength(string key)
        {
            return _cache.ListGetLength(key, _db.DefaultOperate);
        }
        /// <summary>
        /// �Ƴ�������ֵ
        /// </summary>
        /// <typeparam name="E"></typeparam>
        /// <param name="key">��</param>
        /// <param name="isPopEnd">�Ƿ��β���Ƴ�(true���β���Ƴ��������ͷ���Ƴ�)</param>
        /// <param name="defaultValue">Ĭ��ֵ</param>
        /// <returns></returns>
        public E ListPopValue<E>(string key, bool isPopEnd=true, E defaultValue=default(E))
        {
            return _cache.ListPopValue<E>(key, isPopEnd,defaultValue, _db.DefaultOperate);
        }

        /// <summary>
        /// �Ƴ�ֵ
        /// </summary>
        /// <param name="key">��</param>
        /// <param name="value">ֵ</param>
        /// <param name="count">Ҫ�Ƴ�������0��Ϊȫ���Ƴ�</param>
        /// <returns></returns>
        public long ListRemoveValue(string key, object value, long count=0)
        {
            return _cache.ListRemoveValue(key, value,count, _db.DefaultOperate);
        }


        /// <summary>
        /// ��ȡ��������ֵ
        /// </summary>
        /// <typeparam name="E"></typeparam>
        /// <param name="key">��</param>
        /// <param name="start">��ʼλ��(Ĭ��0)</param>
        /// <param name="end">����λ��(-1��Ϊ����ĩβ)</param>
        /// <returns></returns>
        public List<E> ListAllValues<E>(string key, long start=0, long end=-1)
        {
            return _cache.ListAllValues<E>(key, start, end, _db.DefaultOperate);
        }
        #endregion

        #region hash����

        /// <summary>
        /// ������HashSet����ֵ
        /// </summary>
        /// <typeparam name="E"></typeparam>
        /// <param name="key">��</param>
        /// <param name="dicSet">ֵ</param>
        /// <returns></returns>
        public void HashSetRangeValue(string key, IDictionary dicSet)
        {
            _cache.HashSetRangeValue(key, dicSet, _db.DefaultOperate);
        }
        /// <summary>
        /// HashSet����ֵ
        /// </summary>
        /// <param name="key">��</param>
        /// <param name="hashkey">��ϣ��ļ�</param>
        /// <param name="value">��ϣ���ֵ</param>
        /// <param name="type">���÷�ʽ</param>
        public bool HashSetValue(string key, object hashkey, object value, SetValueType type)
        {
            return _cache.HashSetValue(key, hashkey,value,type, _db.DefaultOperate);
        }
        /// <summary>
        /// HashSet����ֵ
        /// </summary>
        /// <param name="key">��</param>
        /// <param name="hashkey">��ϣ��ļ�</param>
        /// <param name="value">��ϣ���ֵ</param>
        /// <param name="type">���÷�ʽ</param>
        public bool HashSetValue(string key, object hashkey, object value)
        {
            return _cache.HashSetValue(key, hashkey, value, SetValueType.Set, _db.DefaultOperate);
        }
        /// <summary>
        /// ��ȡ��ϣ���ֵ
        /// </summary>
        /// <typeparam name="E"></typeparam>
        /// <param name="key">��</param>
        /// <param name="hashkey">��ϣ��ļ�</param>
        /// <param name="defaultValue">Ĭ��ֵ</param>
        /// <param name="connection"></param>
        /// <returns></returns>
        public E HashGetValue<E>(string key, object hashkey, E defaultValue)
        {
            return _cache.HashGetValue<E>(key, hashkey, defaultValue,_db.DefaultOperate);
        }
        /// <summary>
        /// ��ȡ��ϣ���ֵ
        /// </summary>
        /// <typeparam name="E"></typeparam>
        /// <param name="key">��</param>
        /// <param name="hashkey">��ϣ��ļ�</param>
        /// <param name="defaultValue">Ĭ��ֵ</param>
        /// <param name="connection"></param>
        /// <returns></returns>
        public E HashGetValue<E>(string key, object hashkey)
        {
            return _cache.HashGetValue<E>(key, hashkey, default(E), _db.DefaultOperate);
        }
        /// <summary>
        /// ��ȡ���й�ϣ���ֵ
        /// </summary>
        /// <typeparam name="E"></typeparam>
        /// <param name="key">��</param>
        /// <param name="defaultValue">Ĭ��ֵ</param>
        /// <param name="connection"></param>
        /// <returns></returns>
        public List<KeyValuePair<K, V>> HashGetAllValues<K, V>(string key, V defaultValue)
        {
            return _cache.HashGetAllValues<K, V>(key, defaultValue, _db.DefaultOperate);
        }
        /// <summary>
        /// ��ȡ��ϣ�������ֵ
        /// </summary>
        /// <typeparam name="K"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <param name="key">��</param>
        /// <returns></returns>
        public List<KeyValuePair<K, V>> HashGetAllValues<K, V>(string key)
        {
            return _cache.HashGetAllValues<K, V>(key, default(V), _db.DefaultOperate);
        }
        /// <summary>
        /// ɾ����ϣ���ֵ
        /// </summary>
        /// <typeparam name="E"></typeparam>
        /// <param name="key">��</param>
        /// <param name="hashkey">��ϣ��ļ�</param>
        /// <returns></returns>
        public bool HashDeleteValue(string key, object hashkey)
        {
            return _cache.HashDeleteValue(key, hashkey, _db.DefaultOperate);
        }
        /// <summary>
        /// ����ɾ����ϣ���ֵ
        /// </summary>
        /// <typeparam name="E"></typeparam>
        /// <param name="key">��</param>
        /// <param name="hashkeys">Ҫɾ����ϣ��ļ�</param>
        /// <returns></returns>
        public long HashDeleteValues(string key, IEnumerable hashkeys)
        {
            return _cache.HashDeleteValues(key, hashkeys, _db.DefaultOperate);
        }
        /// <summary>
        /// ��ϣ��ļ��Ƿ����
        /// </summary>
        /// <param name="key">��</param>
        /// <param name="hashkey">��ϣ��ļ�</param>
        /// <returns></returns>
        public bool HashExists(string key, object hashkey)
        {
            return _cache.HashExists(key, hashkey, _db.DefaultOperate);
        }
        /// <summary>
        /// ��ϣ������
        /// </summary>
        /// <param name="key">��</param>
        /// <param name="hashkey">��ϣ��</param>
        /// <returns></returns>
        public long HashIncrement(string key, object hashkey)
        {
            return _cache.HashIncrement(key, hashkey,1,_db.DefaultOperate);
        }
        /// <summary>
        /// ��ϣ������
        /// </summary>
        /// <param name="key">��</param>
        /// <param name="hashkey">��ϣ��</param>
        /// <param name="value">������</param>
        /// <returns></returns>
        public long HashIncrement(string key, object hashkey, long value)
        {
            return _cache.HashIncrement(key, hashkey, value, _db.DefaultOperate);
        }

        /// <summary>
        /// ��ϣ���Լ�
        /// </summary>
        /// <param name="key">��</param>
        /// <param name="hashkey">��ϣ��</param>
        /// <param name="value">�Լ���</param>
        /// <returns></returns>
        public long HashDecrement(string key, object hashkey, long value)
        {
            return _cache.HashDecrement(key, hashkey, value, _db.DefaultOperate);
        }
        /// <summary>
        /// ��ϣ���Լ�
        /// </summary>
        /// <param name="key">��</param>
        /// <param name="hashkey">��ϣ��</param>
        /// <returns></returns>
        public long HashDecrement(string key, object hashkey)
        {
            return _cache.HashDecrement(key, hashkey, 1, _db.DefaultOperate);
        }
        #endregion
    }

    /// <summary>
    /// ����ָ��
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
