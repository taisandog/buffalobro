using Buffalo.Kernel;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buffalo.MQ.RedisMQ
{
    /// <summary>
    /// 消息模式
    /// </summary>
    public enum RedisMQMessageMode
    {
        /// <summary>
        /// 轮询
        /// </summary>
        Polling=0,
        /// <summary>
        /// 订阅
        /// </summary>
        Subscriber =1,
    }
    /// <summary>
    /// 格式化key的委托
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public delegate string RedisMQFormatKeyHandle(string key);

    /// <summary>
    /// Redis配置
    /// </summary>
    public class RedisMQConfig:MQConfigBase
    {
        /// <summary>
        /// 订阅标记
        /// </summary>
        public static readonly byte[] PublicTag = new byte[] {0x54,0x86 };
        /// <summary>
        /// 队列头
        /// </summary>
        public const string BuffaloMQHead = "$bufmq.";
        /// <summary>
        /// 命令标记
        /// </summary>
        public readonly CommandFlags CommanfFlags;
        /// <summary>
        /// 配置
        /// </summary>
        public readonly ConfigurationOptions Options;
        /// <summary>
        /// 保存到队列
        /// </summary>
        public bool SaveToQueue=false;
        /// <summary>
        /// 消息模式
        /// </summary>
        public RedisMQMessageMode Mode;
        /// <summary>
        /// 轮询间隔
        /// </summary>
        public int PollingInterval = 500;

        private int _useDatabase;
        /// <summary>
        /// 使用数据库
        /// </summary>
        public int UseDatabase
        {
            get
            {
                return _useDatabase;
            }
        }
        public RedisMQConfig(string connString) : base(connString)
        {
            Dictionary<string, string> hs = _configs;

            CommanfFlags = (CommandFlags)hs.GetDicValue<string, string>("commanfFlags").ConvertTo<int>((int)CommandFlags.None);
            Options = new ConfigurationOptions();


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
            Options.Password = hs.GetDicValue<string, string>("pwd");
            _useDatabase = hs.GetDicValue<string, string>("database").ConvertTo<int>(0);
            Options.DefaultDatabase= _useDatabase;
            Options.Ssl = hs.GetDicValue<string, string>("ssl") == "1";
            Options.SyncTimeout = hs.GetDicValue<string, string>("syncTimeout").ConvertTo<int>(1000);
            Mode =hs.GetDicValue<string, string>("messageMode") == "1"? RedisMQMessageMode.Subscriber: RedisMQMessageMode.Polling;//消息模式
            if (Mode == RedisMQMessageMode.Subscriber)
            {
                SaveToQueue = hs.GetDicValue<string, string>("useQueue") == "1";//保存到队列,只对订阅模式有效
            }
            else 
            {
                SaveToQueue = true;
            }

            PollingInterval= hs.GetDicValue<string, string>("pInterval").ConvertTo<int>(500);//轮询间隔时间(毫秒)
            if (PollingInterval < 10) 
            {
                PollingInterval = 10;
            }
            if (servers.Count > 0)
            {
                foreach (string strServer in servers)
                {
                    Options.EndPoints.Add(strServer);
                }

            }
        }

        /// <summary>
        /// 当选择了订阅模式+保存数据到队列时候，自定义格式化队列的key
        /// </summary>
        public RedisMQFormatKeyHandle FormatQueueKeyHandle;

        /// <summary>
        /// 获取队列的默认键
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string GetDefaultQueueKey(string key)
        {
            if (FormatQueueKeyHandle != null) 
            {
                return FormatQueueKeyHandle(key);
            }
            return BuffaloMQHead + key;
        }
        public override MQConnection CreateConnection()
        {
            return new RedisMQConnection(this);
        }

        public override MQListener CreateListener()
        {
            return new RedisMQListener(this);
        }
    }
}
