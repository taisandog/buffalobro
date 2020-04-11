using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Buffalo.ArgCommon;
using Buffalo.Kernel;
using Confluent.Kafka;

namespace Buffalo.MQ.KafkaMQ
{
    public partial class KafkaMQConnection : MQConnection
    {
        

        private ProducerConfig _pconfig;

        private AdminClientConfig _adminconfig;

        private string _connString;
        private IProducer<string, byte[]> _producer;
        public KafkaMQConnection(string connString)
        {
            _connString = connString;

        }

        
        /// <summary>
        /// 初始化消费者配置
        /// </summary>
        private void InitProducerConfig()
        {
            Dictionary<string, string> hs = ConnStringFilter.GetConnectInfo(_connString);
            _pconfig = new ProducerConfig();
            SetBaseConfig(_pconfig, hs);


            string value = hs.GetDicValue<string, string>("interval");
            int statisticsIntervalMs = 5000;
            if (!string.IsNullOrWhiteSpace(value))
            {
                statisticsIntervalMs = value.ConvertTo<int>(5000);
            }
            _pconfig.StatisticsIntervalMs = statisticsIntervalMs;
            

            ProducerBuilder<string, byte[]> builder = new ProducerBuilder<string, byte[]>(_pconfig);
            _producer = builder.Build();
        }
        /// <summary>
        /// 读入基础配置
        /// </summary>
        /// <param name="config"></param>
        /// <param name="hs"></param>
        private void SetBaseConfig(ClientConfig config, Dictionary<string, string> hs)
        {
            _topic = hs.GetDicValue<string, string>("topic");
            string server = hs.GetDicValue<string, string>("server");
            if (!server.Contains(':'))
            {
                server += ":9093";
            }
            config.BootstrapServers = server;

            string saslUsername = hs.GetDicValue<string, string>("sasluid");


            if (!string.IsNullOrWhiteSpace(config.SaslUsername))
            {
                config.SaslUsername = saslUsername;
                config.SaslPassword = hs.GetDicValue<string, string>("saslpwd");
                config.SaslMechanism = (SaslMechanism)hs.GetDicValue<string, string>("saslMechanism").ConvertTo<int>(1);
                config.SecurityProtocol = (SecurityProtocol)hs.GetDicValue<string, string>("securityProtocol").ConvertTo<int>(1);
                config.SslCaLocation = hs.GetDicValue<string, string>("sslCaLocation");
            }
            else
            {
                config.SecurityProtocol = SecurityProtocol.Plaintext;
            }
        }
        /// <summary>
        /// 初始化消费者配置
        /// </summary>
        private void InitAdminConfig()
        {
            Dictionary<string, string> hs = ConnStringFilter.GetConnectInfo(_connString);
            _adminconfig = new AdminClientConfig();
            SetBaseConfig(_adminconfig, hs);


            string value = hs.GetDicValue<string, string>("interval");
            int statisticsIntervalMs = 5000;
            if (!string.IsNullOrWhiteSpace(value))
            {
                statisticsIntervalMs = value.ConvertTo<int>(5000);
            }
            _adminconfig.StatisticsIntervalMs = statisticsIntervalMs;

           
        }
        public override APIResault SendMessage(string key, byte[] body)
        {
            Message<string, byte[]> message = new Message<string, byte[]>();
            message.Key = key;
            message.Value = body;
            
            DeliveryResult<string,byte[]> res=_producer.ProduceAsync(_topic, message).Result;
            _producer.Flush(new TimeSpan(10));
            return ApiCommon.GetSuccess();
        }

        public override void Close()
        {
            if (_producer!=null)
            {
                _producer.Dispose();
            }
            CloseListener();
        }

        public override void DeleteTopic(bool ifUnused)
        {
            if (_adminconfig == null)
            {
                InitAdminConfig();

            }
            AdminClientBuilder builder = new AdminClientBuilder(_adminconfig);
            using (IAdminClient admin = builder.Build())
            {
                admin.DeleteTopicsAsync(new string[] { _topic }, null);
                
            }
        }

        public override void DeleteQueue(IEnumerable<string> queueName, bool ifUnused, bool ifEmpty)
        {
            
        }

        public override void Dispose()
        {
            Close();
        }

        public override void InitPublic()
        {
            InitProducerConfig();
        }

        
    }
}
