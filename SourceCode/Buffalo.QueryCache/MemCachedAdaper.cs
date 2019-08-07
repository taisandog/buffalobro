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
using System.Net.Sockets;

namespace Buffalo.QueryCache
{
    /// <summary>
    /// 利用MemCached做缓存
    /// </summary>
    public class MemCachedAdaper : NetCacheBase<MemcachedConnection>
    {
        //MemcachedClientConfiguration _config = null;

        private MemcachedClient _client = null;
        private MemcachedClientConfiguration _config;
        

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
            int poolSize = 10;
            string[] conStrs = connStr.Split(';');
            string serverString = "server=";
            string sizeString = "poolsize=";
            string expirString = "expir=";
            string throwString = "throw=";
            string uid = "uid=";
            string pwd = "pwd=";
            string part = null;

            //List<string> lstServers = new List<string>();
            _config = new MemcachedClientConfiguration();
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
                    //serverStr = System.Web.HttpUtility.UrlDecode(serverStr);
                    string serverStr = CacheUnit.CutString(part, serverString.Length);
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
                            IPAddress ipa=null;
                            if (!IPAddress.TryParse(ip, out ipa)) 
                            {
                                IPHostEntry hostEntry = Dns.GetHostEntry(ip);
                                ipa = hostEntry.AddressList[0];
                            }

                            _config.Servers.Add(new IPEndPoint(ipa, Convert.ToInt32(port)));
                        }
                    }
                }
                else if (part.IndexOf(sizeString, StringComparison.CurrentCultureIgnoreCase) == 0)
                {
                    //string maxsizeStr = part.Substring(sizeString.Length);
                    //maxsizeStr = System.Web.HttpUtility.UrlDecode(maxsizeStr);
                    string maxsizeStr = CacheUnit.CutString(part, sizeString.Length);

                    if (!int.TryParse(maxsizeStr, out poolSize))
                    {
                        throw new ArgumentException("最大连接数必须是1-" + MaxVersion + "的值");
                    }
                    if (poolSize <= 0 || poolSize >= int.MaxValue)
                    {
                        throw new ArgumentException("最大连接数必须是1-" + MaxVersion + "的值");
                    }
                }
                else if (part.IndexOf(throwString, StringComparison.CurrentCultureIgnoreCase) == 0)
                {
                    string throwStr = part.Substring(throwString.Length);
                    throwStr = System.Web.HttpUtility.UrlDecode(throwStr);
                    _throwExcertion = (throwStr == "1");
                }
                
                else if (part.IndexOf(uid, StringComparison.CurrentCultureIgnoreCase) == 0)
                {
                    
                    //config.Authentication.Parameters["userName"] = part.Substring(uid.Length);

                    //string valname = part.Substring(uid.Length);
                    string valname = CacheUnit.CutString(part, uid.Length);
                    if (string.IsNullOrEmpty(valname))
                    {
                        

                        _config.Authentication.Type = typeof(PlainTextAuthenticator);
                        _config.Authentication.Parameters["userName"] = valname;
                    }
                }
                else if (part.IndexOf(pwd, StringComparison.CurrentCultureIgnoreCase) == 0)
                {
                    //string valpwd= part.Substring(pwd.Length);
                    string valpwd = CacheUnit.CutString(part, pwd.Length);
                    if (string.IsNullOrEmpty(valpwd))
                    {
                        _config.Authentication.Parameters["password"] = valpwd;
                    }
                }
                else if (part.IndexOf(expirString, StringComparison.CurrentCultureIgnoreCase) == 0)
                {
                    //string expirStr = part.Substring(expirString.Length);
                    //expirStr = System.Web.HttpUtility.UrlDecode(expirStr);
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
                        _expiration = TimeSpan.FromMinutes(mins);
                    }
                }
            }
            _config.SocketPool.ReceiveTimeout = new TimeSpan(0, 0, 2);
            _config.SocketPool.DeadTimeout = new TimeSpan(0, 0, 10);
            _config.Protocol = MemcachedProtocol.Binary;
            _config.SocketPool.MaxPoolSize = poolSize;
             //使用默认的数据桶
            _client = new MemcachedClient(_config);
            
        }
        /// <summary>
        /// 获取连接
        /// </summary>
        /// <param name="readOnly"></param>
        /// <param name="cmd"></param>
        /// <returns></returns>
        protected override MemcachedConnection CreateClient(bool readOnly, string cmd)
        {
            return new MemcachedConnection(_client);
        }

        protected override E GetValue<E>(string key, E defaultValue, MemcachedConnection client)
        {
            //return _client.Get<E>(key);
            object val = client.Client.Get(key);
            return ValueConvertExtend.ConvertValue<E>(val, defaultValue);
        }

        protected override bool SetValue<E>(string key, E value, SetValueType type, int expirSeconds, MemcachedConnection client)
        {
            TimeSpan ts = _expiration;
            if (expirSeconds > 0)
            {
                ts = TimeSpan.FromSeconds(expirSeconds);
            }

            if (ts > TimeSpan.MinValue)
            {
                return client.Client.Store(GetSetValueMode(type), key, value, ts);
            }

            return client.Client.Store(GetSetValueMode(type), key, value);

        }

        protected override DataSet DoGetDataSet(string key, MemcachedConnection client)
        {
            byte[] content = client.Client.Get<byte[]>(key);
            if (content == null) 
            {
                return new DataSet();
            }
            using (MemoryStream stm = new MemoryStream(content))
            {
                return MemDataSerialize.LoadDataSet(stm);
            }
            
        }
        #region getAllKeys
        /// <summary>
        /// 查找所有键
        /// </summary>
        /// <returns></returns>
        public override IEnumerable<string> GetAllKeys(string pattern, MemcachedConnection client)
        {
            IList<IPEndPoint> serverList = _config.Servers;
            
            Dictionary<string, bool> dic = new Dictionary<string,bool>();
            foreach (IPEndPoint ip in serverList)
            {
                FillAllKeys(dic,ip);
            }
            List<string> lst = new List<string>(dic.Count);
            foreach (KeyValuePair<string, bool> kvp in dic) 
            {
                string val = kvp.Key;
                if (!CommonMethods.IsPatternMatch(val, pattern)) 
                {
                    continue;
                }
                lst.Add(val);
            }
            return lst;
        }
        /// <summary>
        /// 查找键
        /// </summary>
        /// <param name="serverIP"></param>
        /// <param name="pattern"></param>
        /// <returns></returns>
        private void FillAllKeys(Dictionary<string, bool> allKeydic,IPEndPoint serverIP)
        {
           
            //var ipString = "127.0.0.1";
            //var port = 11211;
            IEnumerable<string> keyIter = null;
            IEnumerable<string> slabIdIter = null;
            

            using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
            {
                socket.Connect(serverIP);
                slabIdIter = QuerySlabId(socket);
                keyIter = QueryKeys(socket, slabIdIter);
                socket.Close();
            }

            foreach (String key in keyIter)
            {
                allKeydic[key] = true;

            }

            
        }

        /// <summary>
        /// 执行返回字符串标量
        /// </summary>
        /// <param name="socket">套接字</param>
        /// <param name="command">命令</param>
        /// <returns>执行结果</returns>
        private String ExecuteScalarAsString(Socket socket, String command)
        {
            var sendNumOfBytes = socket.Send(Encoding.UTF8.GetBytes(command));
            var bufferSize = 0x1000;
            var buffer = new Byte[bufferSize];
            var readNumOfBytes = 0;
            var sb = new StringBuilder();

            while (true)
            {
                readNumOfBytes = socket.Receive(buffer);
                sb.Append(Encoding.UTF8.GetString(buffer));

                if (readNumOfBytes < bufferSize)
                    break;
            }

            return sb.ToString();
        }

        /// <summary>
        /// 查询slabId
        /// </summary>
        /// <param name="socket">套接字</param>
        /// <returns>slabId遍历器</returns>
        private IEnumerable<String> QuerySlabId(Socket socket)
        {
            var command = "stats items STAT items:0:number 0 \r\n";
            var contentAsString = ExecuteScalarAsString(socket, command);

            return ParseStatsItems(contentAsString);
        }

        /// <summary>
        /// 解析STAT items返回slabId
        /// </summary>
        /// <param name="contentAsString">解析内容</param>
        /// <returns>slabId遍历器</returns>
        private IEnumerable<String> ParseStatsItems(String contentAsString)
        {
            List<String> slabIds = new List<String>();
            string separator = "\r\n";
            char separator2 = ':';
            string[] items = contentAsString.Split(new string[]{separator}, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < items.Length; i += 4)
            {
                string[] itemParts = items[i].Split(new char[]{separator2}, StringSplitOptions.RemoveEmptyEntries);

                if (itemParts.Length < 3)
                    continue;

                slabIds.Add(itemParts[1]);
            }

            return slabIds;
        }

        /// <summary>
        /// 查询键
        /// </summary>
        /// <param name="socket">套接字</param>
        /// <param name="slabIdIter">被查询slabId</param>
        /// <returns>键遍历器</returns>
        private IEnumerable<String> QueryKeys(Socket socket, IEnumerable<String> slabIdIter)
        {
            var keys = new List<String>();
            var cmdFmt = "stats cachedump {0} 200000 ITEM views.decorators.cache.cache_header..cc7d9 [6 b; 1256056128 s] \r\n";
            var contentAsString = String.Empty;

            foreach (String slabId in slabIdIter)
            {
                contentAsString = ExecuteScalarAsString(socket, String.Format(cmdFmt, slabId));
                keys.AddRange(ParseKeys(contentAsString));
            }

            return keys;
        }

        /// <summary>
        /// 解析stats cachedump返回键
        /// </summary>
        /// <param name="contentAsString">解析内容</param>
        /// <returns>键遍历器</returns>
        private IEnumerable<String> ParseKeys(String contentAsString)
        {
            var keys = new List<String>();
            var separator = "\r\n";
            var separator2 = ' ';
            var prefix = "ITEM";
            var items = contentAsString.Split(new string[]{separator}, StringSplitOptions.RemoveEmptyEntries);

            foreach (var item in items)
            {
                var itemParts = item.Split(new char[]{separator2}, StringSplitOptions.RemoveEmptyEntries);
                string part = null;
                if (itemParts.Length > 0) 
                {
                    part = itemParts[0];
                }
                if ((itemParts.Length < 3) || !String.Equals(part, prefix, StringComparison.OrdinalIgnoreCase))
                    continue;

                keys.Add(itemParts[1]);
            }

            return keys;
        }
    

    

        #endregion
        protected override bool DoSetDataSet(string key, DataSet value, int expirSeconds, MemcachedConnection client)
        {
            TimeSpan ts = _expiration;
            if (expirSeconds > 0)
            {
                ts = TimeSpan.FromSeconds(expirSeconds);
            }

            byte[] bval = MemDataSerialize.DataSetToBytes(value);
            if (ts > TimeSpan.MinValue)
            {
                client.Client.Store(StoreMode.Set, key, bval, ts);
            }
            else 
            {
                client.Client.Store(StoreMode.Set, key, bval);
            }
            return true;
        }
        
        protected override bool DoExistsKey(string key, MemcachedConnection client)
        {
            return client.Client.Get(key) != null;
        }
        protected override void DeleteValue(string key, MemcachedConnection client)
        {
            client.Client.Remove(key);
        }
        /// <summary>
        /// 设置版本号
        /// </summary>
        /// <param name="key"></param>
        /// <param name="client"></param>
        protected override void DoNewVer(string key, MemcachedConnection client) 
        {
            DoIncrement(key, 1, client);
            //_client.Increment(key, 1, 1,_expiration);
        }
        protected override long DoIncrement(string key, ulong inc, MemcachedConnection client)
        {
            TimeSpan ts = _expiration;
            
            if (ts > TimeSpan.MinValue)
            {
                return (long)client.Client.Increment(key,0, inc, ts);
            }
            else
            {
                return (long)client.Client.Increment(key,0, inc);
            }

            
        }

        

        protected override long DoDecrement(string key, ulong dec, MemcachedConnection client)
        {
            TimeSpan ts = _expiration;

            if (ts > TimeSpan.MinValue)
            {
                return (long)client.Client.Decrement(key,0, dec, ts);
            }
            else
            {
                return (long)client.Client.Decrement(key,0, dec);
            }
        }

        protected override IDictionary<string, object> GetValues(string[] keys, MemcachedConnection client)
        {
            return client.Client.Get(keys);
        }
       

        protected override string GetCacheName()
        {
            return "memcached";
        }

        public override System.Collections.IList DoGetEntityList(string key, Type entityType, MemcachedConnection client)
        {
            byte[] content = client.Client.Get<byte[]>(key);
            if (content == null)
            {
                return MemDataSerialize.CreateList(entityType);
            }
            using (MemoryStream stm = new MemoryStream(content))
            {
                return MemDataSerialize.LoadList(stm,entityType);
            }
        }

        public override bool DoSetEntityList(string key, System.Collections.IList lstEntity, int expirSeconds, MemcachedConnection client)
        {
            TimeSpan ts = _expiration;
            if (expirSeconds > 0)
            {
                ts = TimeSpan.FromSeconds(expirSeconds);
            }
            byte[] bval = MemDataSerialize.ListToBytes(lstEntity);

            if (ts > TimeSpan.MinValue)
            {
                client.Client.Store(StoreMode.Set, key, bval, ts);
            }
            else
            {
                client.Client.Store(StoreMode.Set, key, bval);
            }

            
            return true;
        }



       

        public override object GetClient()
        {
            return new MemcachedConnection(_client);
        }

        protected override object GetValue(string key, MemcachedConnection client)
        {
            return client.Client.Get(key);
        }

        protected override bool SetValue(string key, object value, SetValueType type, int expirSeconds, MemcachedConnection client)
        {
            TimeSpan ts = _expiration;
            if (expirSeconds > 0)
            {
                ts = TimeSpan.FromSeconds(expirSeconds);
            }

            if (ts > TimeSpan.MinValue)
            {
                return client.Client.Store(GetSetValueMode(type), key, value, ts);
            }

            return client.Client.Store(GetSetValueMode(type), key, value);

        }

        public override void ClearAll(MemcachedConnection client)
        {
            client.Client.FlushAll();
        }

        /// <summary>
        /// 获取设置值模式
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private StoreMode GetSetValueMode(SetValueType type)
        {
            switch (type)
            {
                case SetValueType.Set:
                    return StoreMode.Set;
                case SetValueType.Replace:
                    return StoreMode.Replace;
                case SetValueType.AddNew:
                    return StoreMode.Add;
                default:
                    return StoreMode.Set;
            }
        }
    }

    /// <summary>
    /// Memcached
    /// </summary>
    public class MemcachedConnection : IDisposable
    {
        /// <summary>
        /// 关联连接
        /// </summary>
        internal MemcachedClient Client;
        

        public MemcachedConnection(MemcachedClient client)
        {
            Client = client;
        }


        public void Dispose()
        {

        }


    }
}
