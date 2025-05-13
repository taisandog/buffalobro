using Confluent.Kafka;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Buffalo.MQ.KafkaMQ
{
    /// <summary>
    /// Kafka发送消息类
    /// </summary>
    public class MQKafkaMessage : MQSendMessage
    {
        protected TopicPartition _topicPartition;
        /// <summary>
        /// 话题分区
        /// </summary>
        public TopicPartition TopicPartition 
        {
            get { return _topicPartition; }
        }
        protected Message<byte[], byte[]> _message;
        /// <summary>
        /// 消息
        /// </summary>
        public Message<byte[], byte[]> Message
        {
            get { return _message; }
        }

        protected CancellationToken _cancellationToken;
        /// <summary>
        /// 取消器
        /// </summary>
        public CancellationToken CancellationToken
        {
            get { return _cancellationToken; }
        }

        public MQKafkaMessage(TopicPartition topicPartition, Message<byte[], byte[]> message, CancellationToken cancellationToken = default(CancellationToken)) 
            : base(topicPartition.Topic, message.Value)
        {
            _topicPartition = topicPartition;
            _message = message;
            _cancellationToken = cancellationToken;
        }

        public override void Dispose()
        {
            _topicPartition = null;
            _message = null;
            base.Dispose();
        }

        ~MQKafkaMessage()
        {
            Dispose();
        }
    }
}
