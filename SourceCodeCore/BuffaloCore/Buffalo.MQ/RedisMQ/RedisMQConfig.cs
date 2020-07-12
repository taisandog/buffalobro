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
            _useDatabase = hs.GetDicValue<string, string>("database").ConvertTo<int>(0);
            Options.DefaultDatabase = _useDatabase;
            Options.Password = hs.GetDicValue<string, string>("pwd");
            Options.Ssl = hs.GetDicValue<string, string>("ssl") == "1";
            SaveToQueue= hs.GetDicValue<string, string>("useQueue") == "1";//保存到队列
            if (servers.Count > 0)
            {
                foreach (string strServer in servers)
                {
                    Options.EndPoints.Add(strServer);
                }

            }
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
