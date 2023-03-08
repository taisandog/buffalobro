using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace Buffalo.MQ.RabbitMQ
{
    public class MQRabbitMessage : MQSendMessage
    {
        protected string _exchange;
        /// <summary>
        /// 交换
        /// </summary>
        public string Exchange
        {
            get { return _exchange; }
        }
        protected bool _mandatory;
        /// <summary>
        /// 强制
        /// </summary>
        public bool Mandatory
        {
            get { return _mandatory; }
        }
        IBasicProperties _basicProperties;
        /// <summary>
        /// 基本属性
        /// </summary>
        public IBasicProperties BasicProperties
        {
            get { return _basicProperties; }
        }
        /// <summary>
        /// Rabbit消息
        /// </summary>
        /// <param name="topic">主题(routingKey)</param>
        /// <param name="value">内容</param>
        /// <param name="exchange">交换</param>
        /// <param name="mandatory">强制</param>
        /// <param name="basicProperties">基本属性</param>
        public MQRabbitMessage(string topic, byte[] value, string exchange, bool mandatory, IBasicProperties basicProperties) 
            : base(topic, value)
        {
            _exchange = exchange;
            _mandatory = mandatory;
            _basicProperties = basicProperties;
        }

        public override void Dispose()
        {
            _exchange = null;
            _basicProperties = null;
            base.Dispose();
        }

        ~MQRabbitMessage()
        {
            Dispose();
        }
    }
}