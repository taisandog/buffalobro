﻿using Buffalo.DB.CacheManager;
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
using System.Collections;
using Buffalo.DB.CacheManager.CacheCollection;
using Buffalo.DB.DbCommon;
using Buffalo.QueryCache.RedisCollections;
using System.Security.Authentication;

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
        /// 服务器数量
        /// </summary>
        int _serverCount = 0;
        /// <summary>
        /// 命令标记
        /// </summary>
        private CommandFlags _commanfFlags;
        /// <summary>
        /// 命令标记
        /// </summary>
        public CommandFlags UseCommanfFlags 
        {
            get 
            {
                return _commanfFlags;
            }
            set 
            {
                _commanfFlags = value;
            }
        }
        /// <summary>
        /// 使用第几个库(0-15)
        /// </summary>
        private int _db;

        // <summary>
        /// 命令标记
        /// </summary>
        public int UseDB
        {
            get
            {
                return _db;
            }
            set
            {
                _db = value;
            }
        }

        private ConfigurationOptions _options;

        /// <summary>
        /// 连接配置
        /// </summary>
        public override object ConnectConfiguration
        {
            get
            {
                return _options;
            }
            set
            {
                _options = (ConfigurationOptions)value;
            }
        }
        
        /// <summary>
        /// 连接的客户端
        /// </summary>
        public override object ConnectClient 
        {
            get 
            {
                return _redis;
            }
            
        }
        /// <summary>
        /// 重连连接redis
        /// </summary>
        public override void ReconnectClient() 
        {
            if (_redis != null)
            {
                try
                {
                    _redis.Dispose();
                }
                catch { }
            }
            _redis= ConnectionMultiplexer.Connect(_options);
            
        }
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
            _options = new ConfigurationOptions();
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

            string pwdStr = configs.GetDicValue<string, string>("pwd");
            if (!string.IsNullOrEmpty(pwdStr))
            {
                _options.Password = pwdStr;
            }

            _options.Ssl = configs.GetDicValue<string, string>("ssl") == "1";
            if (_options.Ssl)
            {
                _options.SslProtocols = SslProtocols.Tls | SslProtocols.Tls11 | SslProtocols.Tls12 | SslProtocols.Tls13;
                bool skipCert = configs.GetDicValue<string, string>("skipCert") == "1";//跳过验证
                if (skipCert)
                {
                    _options.CertificateValidation += _options_CertificateValidation;
                }
            }
            string throwStr = configs.GetDicValue<string, string>("throw");
            _throwExcertion = (throwStr == "1");

            string expirStr = configs.GetDicValue<string, string>("expir");
            double seconds = 0;
            if (!string.IsNullOrWhiteSpace(expirStr))
            {
                if (!double.TryParse(expirStr, out seconds))
                {
                    throw new ArgumentException("数据保存秒钟数必须是0-"+ int.MaxValue + "的值");
                }
                if (seconds < 0 || seconds > int.MaxValue)
                {
                    throw new ArgumentException("数据保存秒钟数必须是0-" + int.MaxValue + "的值");
                }

            }
            if (seconds > 0)
            {
                _expiration = TimeSpan.FromSeconds((double)seconds);
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
                    _options.EndPoints.Add(strServer);

                    //_roserver = strServer;
                }

            }
            string ssyncTimeout = configs.GetDicValue<string, string>("syncTimeout");
            if (!string.IsNullOrWhiteSpace(ssyncTimeout))
            {
                _options.SyncTimeout = ssyncTimeout.ConvertTo<int>(5000);
            }
            _serverCount = servers.Count;
            _commanfFlags = (CommandFlags)configs.GetDicValue<string, string>("commanfFlags").ConvertTo<int>((int)CommandFlags.None);
            _db= configs.GetDicValue<string, string>("database").ConvertTo<int>(0);
            return ConnectionMultiplexer.Connect(_options);
        }

        private bool _options_CertificateValidation(object sender, System.Security.Cryptography.X509Certificates.X509Certificate certificate, System.Security.Cryptography.X509Certificates.X509Chain chain, System.Net.Security.SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }
        #region ICacheAdaper 成员



        protected override RedisConnection CreateClient(bool realOnly, string cmd)
        {
            IDatabase client = null;

            client = _redis.GetDatabase(_db);
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



        protected override bool SetValue<E>(string key, E value, SetValueType type, TimeSpan expir, RedisConnection connection)
        {
            IDatabase client = connection.DB;
            TimeSpan ts =LocalCacheBase.GetExpir(_expiration, expir);
            
            RedisValue val = RedisConverter.ValueToRedisValue(value);
            When when = GetSetValueMode(type);
            if (ts > TimeSpan.MinValue)
            {
                return client.StringSet(key, val, ts, when, _commanfFlags);
            }
            
            return client.StringSet(key, val,null, when, _commanfFlags);
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

        protected override bool DoSetDataSet(string key, DataSet value, TimeSpan expir, RedisConnection connection)
        {
            IDatabase client = connection.DB;
            byte[] bval = MemDataSerialize.DataSetToBytes(value);
            TimeSpan ts = LocalCacheBase.GetExpir(_expiration, expir);
            RedisValue val = RedisConverter.ValueToRedisValue(bval);
            if (ts > TimeSpan.MinValue)
            {
                client.StringSet(key, val, ts,When.Always, _commanfFlags);
            }
            else
            {
                client.StringSet(key, val,null,When.Always, _commanfFlags);
            }

            return true;
        }

        protected override bool DeleteValue(string key, RedisConnection connection)
        {
            IDatabase client = connection.DB;
            return client.KeyDelete(key);
        }
        /// <summary>
        /// 设置版本号
        /// </summary>
        /// <param name="key"></param>
        /// <param name="client"></param>
        protected override bool DoNewVer(string key, RedisConnection connection)
        {
            IDatabase client = connection.DB;
            if (_expiration > TimeSpan.MinValue)
            {
                return client.StringSet(key, 1, _expiration,When.Always,_commanfFlags);
            }
            
            return client.StringSet(key, 1,null, When.Always, _commanfFlags);
            

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


       

        public override bool DoSetKeyExpire(string key, TimeSpan expir, RedisConnection connection)
        {
            IDatabase client = connection.DB;
            TimeSpan ts = LocalCacheBase.GetExpir(_expiration, expir);
            return client.KeyExpire(key, ts, _commanfFlags);
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

        public override bool DoSetEntityList(string key, System.Collections.IList lstEntity, TimeSpan expir, RedisConnection connection)
        {
            IDatabase client = connection.DB;
            TimeSpan ts = LocalCacheBase.GetExpir(_expiration, expir);
            byte[] bval = MemDataSerialize.ListToBytes(lstEntity);

            if (ts > TimeSpan.MinValue)
            {
                client.StringSet(key, bval, ts, When.Always, _commanfFlags);
            }
            else
            {
                client.StringSet(key, bval,null,When.Always,_commanfFlags);
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

        protected override bool SetValue(string key, object value, SetValueType type, TimeSpan expir, RedisConnection connection)
        {
            IDatabase client = connection.DB;
            TimeSpan ts = LocalCacheBase.GetExpir(_expiration, expir);
            RedisValue val = RedisConverter.ValueToRedisValue(value);
            if (ts > TimeSpan.MinValue)
            {
                return client.StringSet(key, val, ts, GetSetValueMode(type),_commanfFlags);
            }
            
            return client.StringSet(key, val,null, GetSetValueMode(type), _commanfFlags);
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
        internal static When GetSetValueMode(SetValueType type)
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

        public override ICacheHash GetHashMap(string key, RedisConnection connection)
        {
            return new RedisHash(connection.DB,key, _commanfFlags, _expiration);
        }

        public override ICacheList GetList(string key, RedisConnection connection)
        {
            return new RedisList(connection.DB, key, _commanfFlags, _expiration);
        }

        public override ICacheLock GetCacheLock(string key, RedisConnection connection)
        {
            return new RedisLock(connection.DB, key, _commanfFlags);
        }

        public override ICacheSortedSet GetSortedSet(string key, RedisConnection connection)
        {
            return new RedisSortedSet(connection.DB, key, _commanfFlags, _expiration);
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