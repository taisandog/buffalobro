
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Buffalo.Kernel;
using System.Data;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using Buffalo.DB.DataBaseAdapter;
using Buffalo.DB.MessageOutPuters;
using ServiceStack.Redis;
using Buffalo.DB.DbCommon;
using Buffalo.DB.CacheManager;
using ServiceStack.Text;
using MemcacheClient;

namespace Buffalo.QueryCache
{
    public class RedisAdaperByServiceStack : NetCacheBase<IRedisClient>
    {
        PooledRedisClientManager _pool =null;
        
        /// <summary>
        /// memcached的适配器
        /// </summary>
        /// <param name="connStr">连接字符串</param>
        public RedisAdaperByServiceStack(string connStr, DBInfo info) 
        {
            _info = info;
            _pool = CreateManager(connStr);
            
        }
        /// <summary>
        /// 是否有只读服务器
        /// </summary>
        private bool _hasROServer=false;

        /// <summary>
        /// 创建连接池
        /// </summary>
        /// <param name="connectionString">连接字符串</param>
        /// <returns></returns>
        private PooledRedisClientManager CreateManager(string connectionString)
        {
            string localserver = "127.0.0.1:6379";
            //uint port = 6379;
            int poolSize = 10;
            Dictionary<string, string> configs = ConnStringFilter.GetConnectInfo(connStr);


            string part = null;
            List<string> lstServers = new List<string>();
            List<string> lstRoServers = new List<string>();

            part = lpart.Trim();

            string serverStr = configs.GetDicValue<string, string>("server");

            string[] parts = serverStr.Split(',');
            foreach (string sser in parts)
            {
                if (!string.IsNullOrEmpty(sser))
                {
                    lstServers.Add(sser);
                }
            }


            string serverStr = configs.GetDicValue<string, string>("roserver");
            string[] parts = serverStr.Split(',');
            foreach (string sser in parts)
            {
                if (!string.IsNullOrEmpty(sser))
                {
                    lstRoServers.Add(sser);
                }
            }


            string maxsizeStr = configs.GetDicValue<string, string>("poolsize");
            if (!int.TryParse(maxsizeStr, out poolSize))
            {
                throw new ArgumentException("最大连接数必须是1-" + MaxVersion + "的值");
            }
            if (poolSize <= 0 || poolSize >= int.MaxValue)
            {
                throw new ArgumentException("最大连接数必须是1-" + MaxVersion + "的值");
            }


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


            if (lstServers.Count == 0)
            {
                lstServers.Add(localserver);
            }
            string[] serviers = lstServers.ToArray();

            string[] roserviers = null;
            if (lstRoServers.Count > 0)
            {
                roserviers = lstRoServers.ToArray();
                _hasROServer = true;
            }


            RedisClientManagerConfig config = new RedisClientManagerConfig();
            config.MaxWritePoolSize = poolSize;//“写”链接池链接数
            config.MaxReadPoolSize = poolSize;//“读”链接池链接数
            config.AutoStart = true;

            //支持读写分离，均衡负载
            return new PooledRedisClientManager(serviers, roserviers, config);
        }


#region ICacheAdaper 成员


        protected override IRedisClient CreateClient(bool realOnly,string cmd)
        {
            IRedisClient client =null;
            if (realOnly && _hasROServer)
            {
                client = _pool.GetReadOnlyClient();
            }
            else
            {
                client = _pool.GetClient();
            }
            return client;
        }


        /// <summary>
        /// 获取值
        /// </summary>
        /// <typeparam name="E"></typeparam>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <param name="client"></param>
        /// <returns></returns>
        protected override E GetValue<E>(string key, E defaultValue, IRedisClient client)
        {
            if (typeof(E) == typeof(byte[])) 
            {
                byte[] brr = client.Get<byte[]>(key);
                if (brr == null)
                {
                    return defaultValue;
                }
                return (E)(object)brr;
            }
            string value = client.GetValue(key);
            if (value == null) 
            {
                return defaultValue;
            }
            return JsonSerializer.DeserializeFromString<E>(value);
        }
        /// <summary>
        /// 所有键
        /// </summary>
        /// <returns></returns>
        public override IEnumerable<string> GetAllKeys(string pattern, IRedisClient client)
        {
#if NET_3_5||NET_4_0
            return new List<string>();
#else
            if (string.IsNullOrEmpty(pattern)) 
            {
                pattern = "*";
            }
            return client.GetKeysByPattern(pattern);
#endif
        }


        protected override bool SetValue<E>(string key, E value,SetValueType type, int expirSeconds, IRedisClient client)
        {
            TimeSpan ts = _expiration;
            if (expirSeconds > 0)
            {
                ts = TimeSpan.FromSeconds(expirSeconds);
            }

            if (ts > TimeSpan.MinValue)
            {
                switch (type) 
                {
                    case SetValueType.AddNew:
                        return client.Add<E>(key, value, ts);
                    case SetValueType.Replace:
                        return client.Replace<E>(key, value, ts);
                    default:
                        return client.Set<E>(key, value, ts);
                }
                
            }
            switch (type)
            {
                case SetValueType.AddNew:
                    return client.Add<E>(key, value);
                case SetValueType.Replace:
                    return client.Replace<E>(key, value);
                default:
                    return client.Set<E>(key, value);
            }
        }

        protected override DataSet DoGetDataSet(string key, IRedisClient client)
        {
            byte[] content = client.Get<byte[]>(key);
            using (MemoryStream stm = new MemoryStream(content))
            {
                return MemDataSerialize.LoadDataSet(stm);
            }
        }

        protected override bool DoSetDataSet(string key, DataSet value,int expirSeconds, IRedisClient client)
        {
            byte[] bval = MemDataSerialize.DataSetToBytes(value);
            TimeSpan ts = _expiration;
            if (expirSeconds > 0)
            {
                ts = TimeSpan.FromSeconds(expirSeconds);
            }
            if (ts > TimeSpan.MinValue)
            {
                client.Set<byte[]>(key, bval, ts);
            }
            else
            {
                client.Set<byte[]>(key, bval);
            }
            
            return true;
        }

        protected override void DeleteValue(string key, IRedisClient client)
        {
            client.Remove(key);
        }
        /// <summary>
        /// 设置版本号
        /// </summary>
        /// <param name="key"></param>
        /// <param name="client"></param>
        protected override void DoNewVer(string key, IRedisClient client)
        {
            if (_expiration > TimeSpan.MinValue)
            {
                client.Set<int>(key, 1, _expiration);
            }
            else
            {
                client.Set<int>(key, 1);
            }
            
        }
        protected override long DoIncrement(string key,ulong inc , IRedisClient client)
        {
            if (inc == 1)
            {
                return (long)client.IncrementValue(key);
            }
            else 
            {
                return (long)client.Increment(key, (uint)inc);
            }
        }
        protected override long DoDecrement(string key, ulong dec, IRedisClient client)
        {
            if (dec == 1)
            {
                return (long)client.DecrementValue(key);
            }
            else
            {
                return (long)client.Decrement(key, (uint)dec);
            }
        }
        /// <summary>
        /// 获取值
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="valueType">值类型</param>
        /// <param name="client">客户端</param>
        /// <returns></returns>
        protected override IDictionary<string, object> GetValues(string[] keys, IRedisClient client) 
        {
            return GetValuesMap(keys, client);
        }

        public IDictionary<string, object> GetValuesMap(string[] keys, IRedisClient client)
        {
            RedisNativeClient cli = client as RedisNativeClient;
            if (keys == null) throw new ArgumentNullException("keys");
            if (cli == null) return new Dictionary<string, object>();
            var resultBytesArray = cli.MGet(keys);

            var results = new Dictionary<string, object>();
            for (var i = 0; i < resultBytesArray.Length; i++)
            {
                var key = keys[i];

                var resultBytes = resultBytesArray[i];
                if (resultBytes == null)
                {
                    results.Add(key, null);
                }
                else
                {
                    var resultString = FromUtf8Bytes(resultBytes);
                    var result = JsonSerializer.DeserializeFromString<string>(resultString);
                    results.Add(key, result);
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
            return "Redis";
        }

        #endregion



        public override System.Collections.IList DoGetEntityList(string key, Type entityType, IRedisClient client)
        {
            byte[] content = client.Get<byte[]>(key);
            using (MemoryStream stm = new MemoryStream(content))
            {
                return MemDataSerialize.LoadList(stm, entityType);
            }
        }

        public override bool DoSetEntityList(string key, System.Collections.IList lstEntity, int expirSeconds, IRedisClient client)
        {
            TimeSpan ts = _expiration;
            if (expirSeconds > 0)
            {
                ts = TimeSpan.FromSeconds(expirSeconds);
            }
            byte[] bval = MemDataSerialize.ListToBytes(lstEntity);

            if (ts > TimeSpan.MinValue)
            {
                client.Set<byte[]>(key, bval, ts);
            }
            else
            {
                client.Set<byte[]>(key, bval);
            }
            
            return true;
        }

        
       

        public override object GetClient()
        {
            return CreateClient(false, "any");
        }

        protected override object GetValue(string key, IRedisClient client)
        {
            return client.Get<string>(key);
        }

        protected override bool SetValue(string key, object value,SetValueType type, int expirSeconds, IRedisClient client)
        {
            TimeSpan ts = _expiration;
            if (expirSeconds > 0)
            {
                ts = TimeSpan.FromSeconds(expirSeconds);
            }

            if (ts > TimeSpan.MinValue)
            {
                switch (type)
                {
                    case SetValueType.AddNew:
                        return client.Add(key, value, ts);
                    case SetValueType.Replace:
                        return client.Replace(key, value, ts);
                    default:
                        return client.Set(key, value, ts);
                }

            }
            switch (type)
            {
                case SetValueType.AddNew:
                    return client.Add(key, value);
                case SetValueType.Replace:
                    return client.Replace(key, value);
                default:
                    return client.Set(key, value);
            }
            
        }
        protected override bool DoExistsKey(string key, IRedisClient client)
        {
            
            return client.ContainsKey(key);
        }

        public override void ClearAll(IRedisClient client)
        {
            client.FlushAll();
        }
    }
}