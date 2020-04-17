using Buffalo.Kernel;
using Confluent.Kafka;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buffalo.MQ.KafkaMQ
{
    /// <summary>
    /// Kafka根据字符串读取的配置
    /// </summary>
    public class KafkaMQConfig: MQConfigBase
    {
        /// <summary>
        /// 生产者配置
        /// </summary>
        public readonly ProducerConfig ProducerConfig;

        private ProducerBuilder<string, byte[]> _producerBuilder;
        /// <summary>
        /// 生产者构造器
        /// </summary>
        public ProducerBuilder<string, byte[]> ProducerBuilder
        {
            get
            {
                if (_producerBuilder == null)
                {
                    _producerBuilder = new ProducerBuilder<string, byte[]>(ProducerConfig);
                }
                return _producerBuilder;
            }
        }
        /// <summary>
        /// 管理员配置
        /// </summary>
        public readonly AdminClientConfig AdminConfig;

        private AdminClientBuilder _adminBuilder;
        /// <summary>
        /// 管理员配置
        /// </summary>
        public AdminClientBuilder AdminBuilder
        {
            get
            {
                if (_adminBuilder== null)
                {
                    _adminBuilder = new AdminClientBuilder(AdminConfig);
                }
                return _adminBuilder;
            }
        }
        /// <summary>
        /// 主题
        /// </summary>
        public readonly string Topic;

        /// <summary>
        /// 消费者配置
        /// </summary>
        public readonly ConsumerConfig KConsumerConfig;

        public ConsumerBuilder<string, byte[]> _consumerBuilder;
        /// <summary>
        /// 消费者构造器
        /// </summary>
        public ConsumerBuilder<string, byte[]> KConsumerBuilder
        {
            get
            {
                if (_consumerBuilder == null)
                {
                    _consumerBuilder = new ConsumerBuilder<string, byte[]>(KConsumerConfig);
                }
                return _consumerBuilder;
            }
        }

        public KafkaMQConfig(string connectString):base(connectString)
        {
            
            Topic = _configs.GetDicValue<string, string>("topic");

            ProducerConfig = new ProducerConfig();
            InitProduceConfig(_configs);

            AdminConfig = new AdminClientConfig();
            InitAdminConfig(_configs);
            

            KConsumerConfig = new ConsumerConfig();
            InitConsumerConfig(_configs);
            
        }

        /// <summary>
        /// 初始化消费者配置
        /// </summary>
        private void InitConsumerConfig(Dictionary<string, string> hs)
        {
            KafkaMQConnection.SetBaseConfig(this.KConsumerConfig, hs);

            this.KConsumerConfig.GroupId = hs.GetDicValue<string, string>("groupId");
            if (string.IsNullOrWhiteSpace(this.KConsumerConfig.GroupId))
            {
                this.KConsumerConfig.GroupId = CommonMethods.GuidToString(Guid.NewGuid());
            }
            this.KConsumerConfig.EnableAutoCommit = hs.GetDicValue<string, string>("autoCommit") == "1";


            string value = hs.GetDicValue<string, string>("interval");
            int statisticsIntervalMs = 5000;
            if (!string.IsNullOrWhiteSpace(value))
            {
                statisticsIntervalMs = value.ConvertTo<int>(5000);
            }
            this.KConsumerConfig.StatisticsIntervalMs = statisticsIntervalMs;

            value = hs.GetDicValue<string, string>("sessionTimeout");
            int sessionTimeoutMs = 6000;
            if (!string.IsNullOrWhiteSpace(value))
            {
                sessionTimeoutMs = value.ConvertTo<int>(6000);
            }
            this.KConsumerConfig.SessionTimeoutMs = sessionTimeoutMs;

            

            int offsetType = hs.GetDicValue<string, string>("offsetType").ConvertTo<int>((int)AutoOffsetReset.Earliest);

            this.KConsumerConfig.AutoOffsetReset = (AutoOffsetReset)offsetType;

            string server = hs.GetDicValue<string, string>("server");
            if (!server.Contains(":"))
                server = server + ":9092";
            this.KConsumerConfig.BootstrapServers = server;

        }

        /// <summary>
        /// 初始化消费者配置
        /// </summary>
        private void InitProduceConfig(Dictionary<string, string> hs)
        {

            SetBaseConfig(ProducerConfig, hs);
            

            string value = hs.GetDicValue<string, string>("interval");
            int statisticsIntervalMs = 5000;
            if (!string.IsNullOrWhiteSpace(value))
            {
                statisticsIntervalMs = value.ConvertTo<int>(5000);
            }
            ProducerConfig.StatisticsIntervalMs = statisticsIntervalMs;

            
        }

        /// <summary>
        /// 初始化消费者配置
        /// </summary>
        private void InitAdminConfig(Dictionary<string, string> hs)
        {
           
            SetBaseConfig(AdminConfig, hs);


            string value = hs.GetDicValue<string, string>("interval");
            int statisticsIntervalMs = 5000;
            if (!string.IsNullOrWhiteSpace(value))
            {
                statisticsIntervalMs = value.ConvertTo<int>(5000);
            }
            AdminConfig.StatisticsIntervalMs = statisticsIntervalMs;


        }

        /// <summary>
        /// 读入基础配置
        /// </summary>
        /// <param name="config"></param>
        /// <param name="hs"></param>
        internal static void SetBaseConfig(ClientConfig config, Dictionary<string, string> hs)
        {

            string server = hs.GetDicValue<string, string>("server");
            if (!server.Contains(':'))
            {
                server += ":9092";
            }
            config.BootstrapServers = server;


            string value = hs.GetDicValue<string, string>("saslUsername");
            if (!string.IsNullOrWhiteSpace(value))
            {
                config.SaslUsername = value;
            }
            value = hs.GetDicValue<string, string>("saslPassword");
            if (!string.IsNullOrWhiteSpace(value))
            {
                config.SaslPassword = value;
            }
            value = hs.GetDicValue<string, string>("saslMechanism");
            if (!string.IsNullOrWhiteSpace(value))
            {
                config.SaslMechanism = (SaslMechanism)value.ConvertTo<int>(1);
            }
            value = hs.GetDicValue<string, string>("securityProtocol");
            if (!string.IsNullOrWhiteSpace(value))
            {
                config.SecurityProtocol = (SecurityProtocol)value.ConvertTo<int>(1);
            }
            else
            {
                config.SecurityProtocol = SecurityProtocol.Plaintext;
            }

            value = hs.GetDicValue<string, string>("sslCaLocation");
            if (!string.IsNullOrWhiteSpace(value))
            {
                config.SslCaLocation = value;
            }

        }

        public override MQConnection CreateConnection()
        {
            return new KafkaMQConnection(this);
        }

        public override MQListener CreateListener()
        {
            return new KafkaMQListener(this);
        }
    }
    
}
