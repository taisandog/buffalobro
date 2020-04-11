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

namespace Buffalo.QueryCache
{


    public class RedisAdaperByStackExchange : NetCacheBase<RedisConnection>
    {
        private ConnectionMultiplexer _redis =null;
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
            
            string[] conStrs = connectionString.Split(';');
            string serverString = "server=";
            string passwordString = "pwd=";
            string expirString = "expir=";
            string sslString = "ssl=";
            string throwString = "throw=";
            string part = null;
            List<string> servers = new List<string>();
            foreach (string lpart in conStrs)
            {
                if (string.IsNullOrEmpty(lpart))
                {
                    continue;
                }
                part = lpart.Trim();
                if (part.IndexOf(serverString, StringComparison.CurrentCultureIgnoreCase) == 0)
                {
                    //string serverStr = part.Substring(serverString.Length);
                    string serverStr = CacheUnit.CutString(part, serverString.Length);
                    string[] parts = serverStr.Split(',');
                    foreach (string sser in parts)
                    {
                        if (!string.IsNullOrEmpty(sser))
                        {
                            servers.Add(sser);
                        }
                    }
                }
                //else if (part.IndexOf(readonlyserverString, StringComparison.CurrentCultureIgnoreCase) == 0)
                //{
                //    //string serverStr = part.Substring(serverString.Length);
                //    roserver = CacheUnit.CutString(part, serverString.Length);
                   
                //}
                else if (part.IndexOf(passwordString, StringComparison.CurrentCultureIgnoreCase) == 0)
                {
                    //string serverStr = part.Substring(serverString.Length);
                    options.Password= CacheUnit.CutString(part, passwordString.Length);

                }
                else if (part.IndexOf(sslString, StringComparison.CurrentCultureIgnoreCase) == 0)
                {
                    //string serverStr = part.Substring(serverString.Length);
                    options.Ssl = CacheUnit.CutString(part, sslString.Length)=="1";

                }
                else if (part.IndexOf(throwString, StringComparison.CurrentCultureIgnoreCase) == 0)
                {
                    //string throwStr = part.Substring(throwString.Length);
                    string throwStr = CacheUnit.CutString(part, throwString.Length);
                    _throwExcertion = (throwStr == "1");
                }
                else if (part.IndexOf(expirString, StringComparison.CurrentCultureIgnoreCase) == 0)
                {
                    //string expirStr = part.Substring(expirString.Length);
                    string expirStr = CacheUnit.CutString(part, expirString.Length);
                    double mins = 30;
                    if (!double.TryParse(expirStr, out mins))
                    {
                        throw new ArgumentException("数据保存分钟数必须是0-999999999的值");
                    }
                    if (mins < 0 || mins > 999999999)
                    {
                        throw new ArgumentException("数据保存分钟数必须是0-999999999的值");
                    }
                    if (mins > 0)
                    {
                        _expiration = TimeSpan.FromMinutes((double)mins);
                    }
                }
            }
            if(servers.Count>0)
            {
                mainserver=servers[0];
            }
            //options.EndPoints.Add(mainserver);
            _mainServer = mainserver;
            
            _serverCount = 1;
            if (servers.Count>0)
            {
                foreach (string strServer in servers)
                {
                    options.EndPoints.Add(strServer);
                    _roserver = strServer;
                }
                
            }
            _serverCount = servers.Count;
            return ConnectionMultiplexer.Connect(options);
        }


        #region ICacheAdaper 成员


        protected override RedisConnection CreateClient(bool realOnly, string cmd)
        {
            IDatabase client = null;
            if (realOnly && _hasROServer && _serverCount>1)
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
            if (value.IsNull) 
            {
                return defaultValue;
            }
            return RedisConverter.RedisValueToValue<E>(value);
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
            foreach(RedisKey val in coll)
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

            return client.StringSet(key, val,when: when);
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
            RedisKey[] rkeys=new RedisKey[keys.Length];
            for(int i=0;i<keys.Length;i++)
            {
                rkeys[i]=keys[i];
            }
            RedisValue[] values=client.StringGet(rkeys);
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