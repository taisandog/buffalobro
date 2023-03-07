using Confluent.Kafka;
using System;
using System.Collections.Generic;
using System.Text;

namespace Buffalo.MQ.KafkaMQ
{
    /// <summary>
    /// Kafka消息回调
    /// </summary>
    public class KafkaCallbackMessage : MQCallBackMessage
    {
        protected int _partition = 0;
        /// <summary>
        /// 分区，Kafka有效
        /// </summary>
        public int Partition
        {
            get { return _partition; }
        }

        protected long _offset = 0;

        /// <summary>
        /// 位置，Kafka有效
        /// </summary>
        public long Offset
        {
            get { return _offset; }
        }

        public KafkaCallbackMessage(string topic,  byte[] body, int partition, long offset, 
            IConsumer<byte[], byte[]> consumer, ConsumeResult<byte[], byte[]> consumeResult) :
            base(topic, body) 
        {
            _consumer=consumer;
            _consumeResult=consumeResult;
            _partition=partition;
            _offset=offset;
        }
        protected IConsumer<byte[], byte[]> _consumer;
        /// <summary>
        /// 消费者类
        /// </summary>
        public IConsumer<byte[], byte[]> Consumer 
        {
            get { return _consumer; }
        }

        protected ConsumeResult<byte[], byte[]> _consumeResult;
        /// <summary>
        /// 消费信息
        /// </summary>
        public ConsumeResult<byte[], byte[]> ConsumeResult
        {
            get { return _consumeResult; }
        }

        public override void Commit()
        {
            if(_consumer==null || _consumeResult == null) 
            {
                return;
            }

            _consumer.Commit(_consumeResult);
        }

        public override void Dispose()
        {
            _consumer = null;
            _consumeResult = null;
           
            base.Dispose();
        }

        ~KafkaCallbackMessage()
        {
            Dispose();
           
        }
    }
}
