using Buffalo.ArgCommon;
using Buffalo.Kernel;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buffalo.MQ.RedisMQ
{
    public partial class RedisMQConnection : MQConnection
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
        /// 是否抛出异常
        /// </summary>
        bool _throwExcertion = false;
        /// <summary>
        /// 连接字符串
        /// </summary>
        Dictionary<string, string> _hs = null;
        /// <summary>
        /// 发布者
        /// </summary>
        ISubscriber _subscriber;
        /// <summary>
        /// 
        /// </summary>
        CommandFlags _commanfFlags;
        /// <summary>
        /// 监听的key
        /// </summary>
        private string _listen;
        /// <summary>
        /// RabbitMQ适配
        /// </summary>
        /// <param name="connString">连接字符串</param>
        public RedisMQConnection(string connString)
        {
            _hs = ConnStringFilter.GetConnectInfo(connString);
            
        }
        /// <summary>
        /// 创建连接池
        /// </summary>
        /// <param name="connectionString">连接字符串</param>
        /// <returns></returns>
        private ConnectionMultiplexer CreateManager(Dictionary<string, string> hs)
        {
            ConfigurationOptions options = new ConfigurationOptions();
            //ConnectionMultiplexer config = new ConnectionMultiplexer();
            string mainserver = "127.0.0.1:6379";


            string server = hs.GetDicValue<string, string>("server");
            List<string> servers = new List<string>();
            if (!string.IsNullOrEmpty(server))
            {
                string[] parts = server.Split(',');
                foreach (string sser in parts)
                {
                    string cur = sser;
                    if (!string.IsNullOrEmpty(cur))
                    {
                        if (!cur.Contains(':'))
                        {
                            cur += ":6379";
                        }
                        servers.Add(cur);
                    }
                }
            }

            options.Password = hs.GetDicValue<string, string>("pwd");
            options.Ssl = hs.GetDicValue<string, string>("ssl")=="1";
            string throwStr = hs.GetDicValue<string, string>("throw") ;
            _throwExcertion = (throwStr == "1");
            if (servers.Count > 0)
            {
                mainserver = servers[0];
            }
            //options.EndPoints.Add(mainserver);
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
            _listen = _hs.GetDicValue<string, string>("listen");
            _commanfFlags = (CommandFlags)hs.GetDicValue<string, string>("commanfFlags").ConvertTo<int>((int)CommandFlags.None);
            return ConnectionMultiplexer.Connect(options);
        }

        /// <summary>
        /// 打来连接
        /// </summary>
        private void Open()
        {
            Close();
            _redis = CreateManager(_hs);

        }
        /// <summary>
        /// 初始化发布者模式
        /// </summary>
        public override void InitPublic()
        {
            Open();
            _subscriber = _redis.GetSubscriber();
        }




        /// <summary>
        /// 发布内容
        /// </summary>
        /// <param name="routingKey"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        public override APIResault SendMessage(string routingKey, byte[] body)
        {
            RedisValue value = body;

            _subscriber.Publish(routingKey, value, CommandFlags.None);


            return ApiCommon.GetSuccess();
        }
        /// <summary>
        /// 删除队列(Rabbit可用)
        /// </summary>
        /// <param name="queueName">队列名，如果为null则全删除</param>
        public override void DeleteQueue(IEnumerable<string> queueName, bool ifUnused, bool ifEmpty)
        {
            
        }
        /// <summary>
        /// 删除交换器
        /// </summary>

        public override void DeleteTopic(bool ifUnused)
        {
            
        }

        /// <summary>
        /// 关闭连接
        /// </summary>
        public override void Close()
        {
            if (_redis != null)
            {
                _redis.Close();
                _redis.Dispose();
                _redis = null;
            }
        }

        public override void Dispose()
        {
            Close();
        }
        ~RedisMQConnection()
        {
            Close();
            GC.SuppressFinalize(this);
        }
    }
}
