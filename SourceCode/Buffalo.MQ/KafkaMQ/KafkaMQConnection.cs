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
        /// <summary>
        /// 生产者
        /// </summary>
        private IProducer<byte[], byte[]> _producer;
        /// <summary>
        /// 事务生产者
        /// </summary>
        private IProducer<byte[], byte[]> _tranProducer;

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
            if (_queResault == null)
            {
                _queResault = new Queue<Task<DeliveryResult<byte[], byte[]>>>();
            }
        }
        /// <summary>
        /// 初始化消费者配置
        /// </summary>
        private void InitTranProducerConfig()
        {

            _tranProducer = _config.TranProducerBuilder.Build();
            _tranProducer.InitTransactions(TimeSpan.FromMilliseconds(_config.TranProducerConfig.TransactionTimeoutMs.GetValueOrDefault(3000)));
            if (_queResault == null)
            {
                _queResault = new Queue<Task<DeliveryResult<byte[], byte[]>>>();
            }
        }
        /// <summary>
        /// 获取生产者
        /// </summary>
        /// <returns></returns>
        private IProducer<byte[], byte[]> GetProducer()
        {
            if (_isTran)
            {
                if (_tranProducer == null)
                {
                    InitTranProducerConfig();
                }
                return _tranProducer;
            }
            if (_producer == null)
            {
                InitProducerConfig();
            }
            return _producer;
        }

        protected override APIResault SendMessage(string key, byte[] body)
        {
            Message<byte[], byte[]> message = new Message<byte[], byte[]>();
            message.Key = DefaultEncoding.GetBytes(key);
            message.Value = body;
            IProducer<byte[], byte[]> producer = GetProducer();
            Task<DeliveryResult<byte[], byte[]>> delRes = producer.ProduceAsync(key, message);
            
            _queResault.Enqueue(delRes);
            return ApiCommon.GetSuccess();
        }
        protected override APIResault SendMessage(string key, string body)
        {
            byte[] content = DefaultEncoding.GetBytes(body);
            return SendMessage(key, body);
        }

        /// <summary>
        /// 清空缓冲区
        /// </summary>
        private void DoFlush()
        {
            if (_producer != null)
            {
                _producer.Flush(new TimeSpan(2000));
            }
            if (_tranProducer != null)
            {
                _tranProducer.Flush(new TimeSpan(2000));
            }
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
            if (_producer != null)
            {
                DoFlush();
                _producer.Dispose();
                _producer = null;
            }
            if (_tranProducer != null)
            {
                DoFlush();
                _tranProducer.Dispose();
                _tranProducer = null;
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
            //if (IsOpen)
            //{
            //    return;
            //}
            //InitProducerConfig();
        }

        private void CheckInitTransactions()
        {
            
            if (string.IsNullOrWhiteSpace(_config.ProducerConfig.TransactionalId))
            {
                throw new MissingMemberException("开启事务请在连接字符串配置TransactionalId");
            }
            
        }
        protected override APIResault StartTran()
        {
            Open();
            if (_tranProducer == null)
            {
                InitTranProducerConfig();
            }
            _tranProducer.BeginTransaction();
            return ApiCommon.GetSuccess();
        }

        protected override APIResault CommitTran()
        {
            if (_tranProducer == null)
            {
                throw new NullReferenceException("还没开启事务");
            }
            _tranProducer.CommitTransaction(_timeout);
            return ApiCommon.GetSuccess();
        }

        protected override APIResault RoolbackTran()
        {
            if (_tranProducer == null)
            {
                throw new NullReferenceException("还没开启事务");
            }
            _tranProducer.AbortTransaction(_timeout);
            return ApiCommon.GetSuccess();
        }
    }
}
