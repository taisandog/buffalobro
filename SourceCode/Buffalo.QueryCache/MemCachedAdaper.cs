using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.Kernel;
using System.Data;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using Buffalo.DB.DataBaseAdapter;
using Buffalo.DB.MessageOutPuters;
using Buffalo.DB.DbCommon;
using Buffalo.DB.CacheManager;
using Enyim.Caching;
using Enyim.Caching.Configuration;
using Enyim.Caching.Memcached;
using MemcacheClient;

namespace Buffalo.QueryCache
{
    /// <summary>
    /// 利用MemCached做缓存
    /// </summary>
    public class MemCachedAdaper : NetCacheBase<EmptyClass>
    {
        //MemcachedClientConfiguration _config = null;

        MemcachedClient _client = null;
        
        
        /// <summary>
        /// memcached的适配器
        /// </summary>
        /// <param name="connStr">连接字符串</param>
        public MemCachedAdaper(string connStr, DBInfo info) 
        {
            _info = info;
            CreatePool(connStr);
            
        }


        /// <summary>
        /// 创建连接池
        /// </summary>
        /// <param name="connStr"></param>
        /// <returns></returns>
        private void CreatePool(string connStr) 
        {
            //uint port = 11211;
            int maxSize = 10;
            string[] conStrs = connStr.Split(';');
            string serverString = "server=";
            string sizeString = "maxsize=";
            string expirString = "expir=";
            string throwString = "throw=";
            string part = null;

            //List<string> lstServers = new List<string>();
            MemcachedClientConfiguration config = new MemcachedClientConfiguration();
            foreach (string lpart in conStrs)
            {
                part = lpart.Trim();
                if (part.IndexOf(serverString, StringComparison.CurrentCultureIgnoreCase) == 0)
                {
                    string serverStr = part.Substring(serverString.Length);
                    
                    string[] parts = serverStr.Split(',');
                    foreach (string sser in parts) 
                    {
                        if (!string.IsNullOrEmpty(sser)) 
                        {
                            string[] serPart=sser.Split(':');
                            string ip=null;
                            string port="11211";
                            if(serPart.Length>0)
                            {
                                ip=serPart[0];
                            }
                            if(serPart.Length>1)
                            {
                                port=serPart[1];
                            }
                            config.Servers.Add(new IPEndPoint(IPAddress.Parse(ip), Convert.ToInt32(port)));
                        }
                    }
                }
                else if (part.IndexOf(sizeString, StringComparison.CurrentCultureIgnoreCase) == 0)
                {
                    string maxsizeStr = part.Substring(sizeString.Length);
                    if (!int.TryParse(maxsizeStr, out maxSize))
                    {
                        throw new ArgumentException("最大连接数必须是1-" + MaxVersion + "的值");
                    }
                    if (maxSize <= 0 || maxSize >= int.MaxValue)
                    {
                        throw new ArgumentException("最大连接数必须是1-" + MaxVersion + "的值");
                    }
                }
                else if (part.IndexOf(throwString, StringComparison.CurrentCultureIgnoreCase) == 0)
                {
                    string throwStr = part.Substring(throwString.Length);
                    _throwExcertion = (throwStr == "1");
                }
                else if (part.IndexOf(expirString, StringComparison.CurrentCultureIgnoreCase) == 0)
                {
                    string expirStr = part.Substring(expirString.Length);
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
            config.SocketPool.ReceiveTimeout = new TimeSpan(0, 0, 2);
            config.SocketPool.DeadTimeout = new TimeSpan(0, 0, 10);
            config.Protocol = MemcachedProtocol.Binary;
            
             //使用默认的数据桶
            _client = new MemcachedClient(config);
            
        }
        protected override EmptyClass CreateClient(bool readOnly, string cmd)
        {

            return null;
        }

        protected override E GetValue<E>(string key, EmptyClass client)
        {
            object value = _client.Get(key);
            if (value == null)
            {
                return default(E);
            }
            Type curType = typeof(E);

            return (E)Convert.ChangeType(value, curType);
        }

        protected override void SetValue<E>(string key, E value, EmptyClass client)
        {
            _client.Store(StoreMode.Set, key, value, _expiration);
        }

        protected override DataSet DoGetDataSet(string key, EmptyClass client)
        {
            byte[] content = _client.Get<byte[]>(key);
            using (MemoryStream stm = new MemoryStream(content))
            {
                return MemDataSerialize.LoadDataSet(stm);
            }
            
        }

        protected override bool DoSetDataSet(string key, DataSet value, EmptyClass client)
        {
            byte[] bval = MemDataSerialize.DataSetToBytes(value);
            _client.Store(StoreMode.Set, key, bval, _expiration);
            return true;
        }

        protected override void DeleteValue(string key, EmptyClass client)
        {
            _client.Remove(key);
        }
        /// <summary>
        /// 设置版本号
        /// </summary>
        /// <param name="key"></param>
        /// <param name="client"></param>
        protected override void DoNewVer(string key, EmptyClass client) 
        {
            _client.Increment(key, 1, 1,_expiration);
        }
        protected override void DoIncrement(string key, EmptyClass client)
        {
            _client.Increment(key, 1, 1, _expiration);
        }


        protected override IDictionary<string, object> GetValues(string[] keys, EmptyClass client)
        {
            return _client.Get(keys);
        }
       

        protected override string GetCacheName()
        {
            return "Memcached";
        }
    }

    public class EmptyClass :IDisposable
    {
        public EmptyClass() 
        {
            
        }
        public void Dispose()
        {
            
        }
    }
}
