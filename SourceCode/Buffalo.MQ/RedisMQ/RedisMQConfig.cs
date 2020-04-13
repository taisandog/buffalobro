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
        /// 命令标记
        /// </summary>
        public readonly CommandFlags CommanfFlags;
        /// <summary>
        /// 配置
        /// </summary>
        public readonly ConfigurationOptions Options;

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
            Options.Ssl = hs.GetDicValue<string, string>("ssl") == "1";

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
