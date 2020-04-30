using Buffalo.DB.CacheManager;
using Buffalo.DB.DataBaseAdapter;
using Buffalo.Kernel.Defaults;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Newtonsoft.Json;
using System.IO;
using MemcacheClient;
using Buffalo.Kernel;

namespace Buffalo.QueryCache
{


    public class RedisAdaperByStackExchange : NetCacheBase<RedisConnection>
    {
        private ConnectionMultiplexer _redis = null;
        /// <summary>
        /// 主服务器
        /// </summary>
        private string _mainServer;
        /// <summary>
        /// 只读服务器
        /// </summary>
        private string _roserver;
        /// <summary>
        /// 服务器数量
        /// </summary>
        int _serverCount = 0;
        /// <summary>
        /// 命令标记
        /// </summary>
         CommandFlags _commanfFlags;
        /// <summary>
        /// memcached的适配器
        /// </summary>
        /// <param name="connStr">连接字符串</param>
        public RedisAdaperByStackExchange(string connStr, DBInfo info)
        {
            _info = info;
            _redis = CreateManager(connStr);

        }
        /// <summary>
        /// 是否有只读服务器
        /// </summary>
        private bool _hasROServer = false;

        /// <summary>
        /// 创建连接池
        /// </summary>
        /// <param name="connectionString">连接字符串</param>
        /// <returns></returns>
        private ConnectionMultiplexer CreateManager(string connectionString)
        {
            ConfigurationOptions options = new ConfigurationOptions();
            //ConnectionMultiplexer config = new ConnectionMultiplexer();
            string mainserver = "127.0.0.1:6379";
            Dictionary<string, string> configs = ConnStringFilter.GetConnectInfo(connectionString);

            List<string> servers = new List<string>();



            string serverStr = configs.GetDicValue<string, string>("server");
            string[] parts = serverStr.Split(',');
            foreach (string sser in parts)
            {
                if (!string.IsNullOrEmpty(sser))
                {
                    servers.Add(sser);
                }
            }

            string pwdStr = configs.GetDicValue<string, string>("server");
            if (!string.IsNullOrEmpty(pwdStr))
            {
                options.Password = pwdStr;
            }

            options.Ssl = configs.GetDicValue<string, string>("ssl") == "1";

            string throwStr = configs.GetDicValue<string, string>("throw");
            _throwExcertion = (throwStr == "1");

            string expirStr = configs.GetDicValue<string, string>("expir");
            double mins = 30;
            if (!string.IsNullOrWhiteSpace(expirStr))
            {
                if (!double.TryParse(expirStr, out mins))
                {
                    throw new ArgumentException("数据保存分钟数必须是0-999999999的值");
                }
                if (mins < 0 || mins > 999999999)
                {
                    throw new ArgumentException("数据保存分钟数必须是0-999999999的值");
                }

            }
            if (mins > 0)
            {
                _expiration = TimeSpan.FromMinutes((double)mins);
            }
            if (servers.Count > 0)
            {
                mainserver = servers[0];
            }
            _mainServer = mainserver;

            _serverCount = 1;
            if (servers.Count > 0)
            {
                foreach (string strServer in servers)
                {
                    options.EndPoints.Add(strServer);
                    _roserver = strServer;
                }

            }
            _serverCount = servers.Count;
            _commanfFlags = (CommandFlags)configs.GetDicValue<string, string>("commanfFlags").ConvertTo<int>((int)CommandFlags.None);

            return ConnectionMultiplexer.Connect(options);
        }


        #region ICacheAdaper 成员



        protected override RedisConnection CreateClient(bool realOnly, string cmd)
        {
            IDatabase client = null;
            if (realOnly && _hasROServer && _serverCount > 1)
            {
                client = _redis.GetDatabase(1);
            }
            else
            {
                client = _redis.GetDatabase(0);
            }
            return new RedisConnection(client);
        }


        protected override bool DoExistsKey(string key, RedisConnection connection)
        {
            IDatabase client = connection.DB;
            return client.KeyExists(key);
        }

        /// <summary>
        /// 获取值
        /// </summary>
        /// <typeparam name="E"></typeparam>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <param name="client"></param>
        /// <returns></returns>
        protected override E GetValue<E>(string key, E defaultValue, RedisConnection connection)
        {

            IDatabase client = connection.DB;
            RedisValue value = client.StringGet(key);
           

            return RedisConverter.RedisValueToValue<E>(value,defaultValue);
        }
        /// <summary>
        /// 所有键
        /// </summary>
        /// <returns></returns>
        public override IEnumerable<string> GetAllKeys(string pattern, RedisConnection connection)
        {
#if NET_3_5||NET_4_0
            return new List<string>();
#else
            if (string.IsNullOrEmpty(pattern))
            {
                pattern = "*";
            }
            IEnumerable<RedisKey> coll = _redis.GetServer(_mainServer).Keys();
            List<string> lst = new List<string>();
            foreach (RedisKey val in coll)
            {
                lst.Add(val.ToString());
            }
            return lst;
#endif
        }



        protected override bool SetValue<E>(string key, E value, SetValueType type, int expirSeconds, RedisConnection connection)
        {
            IDatabase client = connection.DB;
            TimeSpan ts = _expiration;
            if (expirSeconds > 0)
            {
                ts = TimeSpan.FromSeconds(expirSeconds);
            }
            RedisValue val = RedisConverter.ValueToRedisValue(value);
            When when = GetSetValueMode(type);
            if (ts > TimeSpan.MinValue)
            {
                return client.StringSet(key, val, ts, when);
            }

            return client.StringSet(key, val, when: when);
        }



        protected override DataSet DoGetDataSet(string key, RedisConnection connection)
        {
            IDatabase client = connection.DB;
            byte[] content = client.StringGet(key);
            using (MemoryStream stm = new MemoryStream(content))
            {
                return MemDataSerialize.LoadDataSet(stm);
            }
        }

        protected override bool DoSetDataSet(string key, DataSet value, int expirSeconds, RedisConnection connection)
        {
            IDatabase client = connection.DB;
            byte[] bval = MemDataSerialize.DataSetToBytes(value);
            TimeSpan ts = _expiration;
            if (expirSeconds > 0)
            {
                ts = TimeSpan.FromSeconds(expirSeconds);
            }
            RedisValue val = RedisConverter.ValueToRedisValue(bval);
            if (ts > TimeSpan.MinValue)
            {
                client.StringSet(key, val, ts);
            }
            else
            {
                client.StringSet(key, val);
            }

            return true;
        }

        protected override void DeleteValue(string key, RedisConnection connection)
        {
            IDatabase client = connection.DB;
            client.KeyDelete(key);
        }
        /// <summary>
        /// 设置版本号
        /// </summary>
        /// <param name="key"></param>
        /// <param name="client"></param>
        protected override void DoNewVer(string key, RedisConnection connection)
        {
            IDatabase client = connection.DB;
            if (_expiration > TimeSpan.MinValue)
            {
                client.StringSet(key, 1, _expiration);
            }
            else
            {
                client.StringSet(key, 1);
            }

        }
        protected override long DoIncrement(string key, ulong inc, RedisConnection connection)
        {
            IDatabase client = connection.DB;
            if (inc == 1)
            {
                return client.StringIncrement(key);
            }
            else
            {
                return client.StringIncrement(key, (long)inc);
            }
        }
        protected override long DoDecrement(string key, ulong dec, RedisConnection connection)
        {
            IDatabase client = connection.DB;
            if (dec == 1)
            {
                return (long)client.StringDecrement(key);
            }
            else
            {
                return (long)client.StringDecrement(key, dec);
            }
        }
        /// <summary>
        /// 获取值
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="valueType">值类型</param>
        /// <param name="client">客户端</param>
        /// <returns></returns>
        protected override IDictionary<string, object> GetValues(string[] keys, RedisConnection connection)
        {
            IDatabase client = connection.DB;
            return GetValuesMap(keys, connection);
        }

        public IDictionary<string, object> GetValuesMap(string[] keys, RedisConnection connection)
        {
            IDatabase client = connection.DB;

            Dictionary<string, object> results = new Dictionary<string, object>();
            RedisKey[] rkeys = new RedisKey[keys.Length];
            for (int i = 0; i < keys.Length; i++)
            {
                rkeys[i] = keys[i];
            }
            RedisValue[] values = client.StringGet(rkeys);
            RedisValue cur = RedisValue.Null;
            for (int i = 0; i < keys.Length; i++)
            {
                cur = values[i];
                if (cur.IsNull)
                {
                    results[keys[i]] = null;
                }
                else
                {
                    results[keys[i]] = values[i];
                }
            }

            return results;
        }
        public static string FromUtf8Bytes(byte[] bytes)
        {
            return bytes == null ? null
                : Encoding.UTF8.GetString(bytes, 0, bytes.Length);
        }
        protected override string GetCacheName()
        {
            return "redis";
        }


        /// <summary>
        /// 增加到列表
        /// </summary>
        /// <typeparam name="E"></typeparam>
        /// <param name="key">键</param>
        /// <param name="index">索引(0为增加到头部，-1为增加到尾部)</param>
        /// <param name="value">值</param>
        /// <param name="setType">设置值方式</param>
        /// <param name="connection"></param>
        /// <returns></returns>
        protected override long ListAddValue<E>(string key,long index, E value, SetValueType setType, RedisConnection connection)
        {
            IDatabase client = connection.DB;
            TimeSpan ts = _expiration;

            RedisValue val = RedisConverter.ValueToRedisValue(value);
            When when = GetSetValueMode(setType);
            if (index == 0)
            {
                return client.ListLeftPush(key, val, when, _commanfFlags);
            }
            if (index == -1)
            {
                return client.ListRightPush(key, val, when, _commanfFlags);
            }
            client.ListSetByIndex(key, index, val, _commanfFlags);
            return 1;
        }

       
       
       /// <summary>
       /// 获取值
       /// </summary>
       /// <typeparam name="E"></typeparam>
       /// <param name="key">键</param>
       /// <param name="index">值位置</param>
       /// <param name="defaultValue">默认值</param>
       /// <param name="connection"></param>
       /// <returns></returns>
        protected override E ListGetValue<E>(string key,long index,E defaultValue,  RedisConnection connection)
        {
            IDatabase client = connection.DB;
            TimeSpan ts = _expiration;

            RedisValue value = client.ListGetByIndex(key, index, _commanfFlags);
            
            return RedisConverter.RedisValueToValue<E>(value, defaultValue);
        }
       
        /// <summary>
        /// 获取集合长度
        /// </summary>
        /// <typeparam name="E"></typeparam>
        /// <param name="key"></param>
        /// <param name="connection"></param>
        /// <returns></returns>
        protected override long ListGetLength(string key, RedisConnection connection)
        {
            IDatabase client = connection.DB;

            return client.ListLength(key,  _commanfFlags);
        }
        /// <summary>
        /// 移除并返回值
        /// </summary>
        /// <typeparam name="E"></typeparam>
        /// <param name="key">键</param>
        /// <param name="isPopEnd">是否从尾部移除(true则从尾部移除，否则从头部移除)</param>
        /// <param name="defaultValue">默认值</param>
        /// <param name="connection"></param>
        /// <returns></returns>
        protected override E ListPopValue<E>(string key,bool isPopEnd, E defaultValue, RedisConnection connection)
        {
            IDatabase client = connection.DB;
            TimeSpan ts = _expiration;
            RedisValue value = RedisValue.Null;
            if (isPopEnd)
            {
                value = client.ListRightPop(key, _commanfFlags);
            }
            else
            {
                value = client.ListLeftPop(key, _commanfFlags);
            }
            return RedisConverter.RedisValueToValue<E>(value, defaultValue); 
        }
        /// <summary>
        /// 移除值
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="count">要移除几个，0则为全部移除</param>
        /// <param name="connection"></param>
        /// <returns></returns>
        protected override long ListRemoveValue(string key,  object value,long count, RedisConnection connection)
        {
            IDatabase client = connection.DB;
            TimeSpan ts = _expiration;
            RedisValue rvalue =  RedisConverter.ValueToRedisValue(value);
            return client.ListRemove(key, rvalue, count, _commanfFlags);
        }
        
        /// <summary>
        /// 获取集合所有值
        /// </summary>
        /// <typeparam name="E"></typeparam>
        /// <param name="key">键</param>
        /// <param name="start">起始位置(默认0)</param>
        /// <param name="end">结束位置(-1则为读到末尾)</param>
        /// <param name="connection"></param>
        /// <returns></returns>
        protected override List<E> ListAllValues<E>(string key,long start,long end, RedisConnection connection)
        {
            IDatabase client = connection.DB;
            RedisValue[] values = client.ListRange(key, start, end, _commanfFlags);
            List<E> result = new List<E>();
            foreach (RedisValue item in values)
            {
                result.Add(RedisConverter.RedisValueToValue<E>(item,default(E)));
            }
           
            return result;
        }
        #endregion



        public override System.Collections.IList DoGetEntityList(string key, Type entityType, RedisConnection connection)
        {

            IDatabase client = connection.DB;
            byte[] content = client.StringGet(key);
            using (MemoryStream stm = new MemoryStream(content))
            {
                return MemDataSerialize.LoadList(stm, entityType);
            }
        }

        public override bool DoSetEntityList(string key, System.Collections.IList lstEntity, int expirSeconds, RedisConnection connection)
        {
            IDatabase client = connection.DB;
            TimeSpan ts = _expiration;
            if (expirSeconds > 0)
            {
                ts = TimeSpan.FromSeconds(expirSeconds);
            }
            byte[] bval = MemDataSerialize.ListToBytes(lstEntity);

            if (ts > TimeSpan.MinValue)
            {
                client.StringSet(key, bval, ts);
            }
            else
            {
                client.StringSet(key, bval);
            }

            return true;
        }




        public override object GetClient()
        {
            return CreateClient(false, "any");
        }

        protected override object GetValue(string key, RedisConnection connection)
        {
            IDatabase client = connection.DB;
            RedisValue ret = client.StringGet(key);
            if (ret.IsNull)
            {
                return null;
            }
            return ret;
        }

        protected override bool SetValue(string key, object value, SetValueType type, int expirSeconds, RedisConnection connection)
        {
            IDatabase client = connection.DB;
            TimeSpan ts = _expiration;
            if (expirSeconds > 0)
            {
                ts = TimeSpan.FromSeconds(expirSeconds);
            }
            RedisValue val = RedisConverter.ValueToRedisValue(value);
            if (ts > TimeSpan.MinValue)
            {
                return client.StringSet(key, val, ts, GetSetValueMode(type));
            }
            
            return client.StringSet(key, val,null, GetSetValueMode(type));
        }

        public override void ClearAll(RedisConnection connection)
        {
            IServer server= _redis.GetServer(_mainServer);
            server.FlushAllDatabases();
        }
        /// <summary>
        /// 获取设置值模式
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private When GetSetValueMode(SetValueType type)
        {
            switch (type)
            {
                case SetValueType.Set:
                    return When.Always;
                case SetValueType.Replace:
                    return When.Exists;
                case SetValueType.AddNew:
                    return When.NotExists;
                default:
                    return When.Always;
            }
        }
    }

    /// <summary>
    /// Redis连接
    /// </summary>
    public class RedisConnection : IDisposable 
    {
        /// <summary>
        /// 关联连接
        /// </summary>
        internal IDatabase DB;
        

        public RedisConnection(IDatabase db) 
        {
            DB = db;
        }
        

        public void Dispose()
        {
            
        }

        
    }
}