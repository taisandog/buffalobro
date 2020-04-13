using System;
using System.Collections.Generic;
using System.Diagnostics;
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


        private KafkaMQConfig _config;

        private IProducer<string, byte[]> _producer;
        /// <summary>
        /// 队列
        /// </summary>
        private Queue<Task<DeliveryResult<string, byte[]>>> _queResault = null;
        public override bool IsOpen
        {
            get
            {
                return _producer != null;
            }
        }
        private TimeSpan _timeout;
        public KafkaMQConnection(KafkaMQConfig config)
        {
            _config = config;
            int to = _config.ProducerConfig.SocketTimeoutMs.GetValueOrDefault();
            if (to < 100)
            {
                to = 2000;
            }
            _timeout = TimeSpan.FromMilliseconds(to);
        }
        
        /// <summary>
        /// 初始化消费者配置
        /// </summary>
        private void InitProducerConfig()
        {
            _producer = _config.ProducerBuilder.Build();
            _queResault = new Queue<Task<DeliveryResult<string, byte[]>>>();
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
        
        protected override APIResault SendMessage(string key, byte[] body)
        {
            Message<string, byte[]> message = new Message<string, byte[]>();
            message.Key = key;
            message.Value = body;

            Task<DeliveryResult<string, byte[]>> delRes = _producer.ProduceAsync(key, message);
            //DeliveryResult<string, byte[]> re = delRes.Result;
            //_producer.Produce(key, message);
            _queResault.Enqueue(delRes);
            return ApiCommon.GetSuccess();
        }
        /// <summary>
        /// 清空缓冲区
        /// </summary>
        private void DoFlush()
        {
            _producer.Flush(new TimeSpan(2000));
            Queue<Task<DeliveryResult<string, byte[]>>> curQueue = _queResault;
            if (curQueue == null)
            {
                return;
            }
            while (curQueue.Count > 0)
            {
                Task<DeliveryResult<string, byte[]>> delRes = curQueue.Dequeue();
                DeliveryResult<string, byte[]> re = delRes.Result;
            }
        }

        protected override void Close()
        {
            if (_producer!=null)
            {
                DoFlush();
                _producer.Dispose();
                _producer = null;
            }
            _queResault = null;
        }

        public override void DeleteTopic(bool ifUnused)
        {
            
            
            
        }

        public override void DeleteQueue(IEnumerable<string> queueName, bool ifUnused, bool ifEmpty)
        {
            using (IAdminClient admin = _config.AdminBuilder.Build())
            {
                admin.DeleteTopicsAsync(queueName, null);

            }
        }

        

        public override void Open()
        {
            if (IsOpen)
            {
                return;
            }
            InitProducerConfig();
        }

        protected override APIResault StartTran()
        {
            _producer.BeginTransaction();
            return ApiCommon.GetSuccess();
        }

        protected override APIResault CommitTran()
        {
            
            _producer.CommitTransaction(_timeout);
            return ApiCommon.GetSuccess();
        }

        protected override APIResault RoolbackTran()
        {
            _producer.AbortTransaction(_timeout);
            return ApiCommon.GetSuccess();
        }
    }
}
