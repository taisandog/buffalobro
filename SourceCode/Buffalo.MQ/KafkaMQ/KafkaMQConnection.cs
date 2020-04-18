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

        private IProducer<byte[], byte[]> _producer;

        private static Encoding DefaultEncoding = Encoding.UTF8;
        /// <summary>
        /// 队列
        /// </summary>
        private Queue<Task<DeliveryResult<byte[], byte[]>>> _queResault = null;
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
            _queResault = new Queue<Task<DeliveryResult<byte[], byte[]>>>();
        }

        
        protected override APIResault SendMessage(string key, byte[] body)
        {
            Message<byte[], byte[]> message = new Message<byte[], byte[]>();
            message.Key = DefaultEncoding.GetBytes(key);
            message.Value = body;
           
            Task<DeliveryResult<byte[], byte[]>> delRes = _producer.ProduceAsync(key, message);
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
            Queue<Task<DeliveryResult<byte[], byte[]>>> curQueue = _queResault;
            if (curQueue == null)
            {
                return;
            }
            while (curQueue.Count > 0)
            {
                Task<DeliveryResult<byte[], byte[]>> delRes = curQueue.Dequeue();
                DeliveryResult<byte[], byte[]> re = delRes.Result;
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

        

        protected override void Open()
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
