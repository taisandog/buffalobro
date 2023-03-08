using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace Buffalo.MQ.RabbitMQ
{
    public class RabbitCallbackMessage : MQCallBackMessage
    {
        protected IModel _channel;

        public IModel Channel 
        {
            get { return _channel; }
        }
        protected BasicDeliverEventArgs _basicDeliverEventArgs;
        /// <summary>
        /// 消息
        /// </summary>
        public BasicDeliverEventArgs BasicDeliverEventArgs
        {
            get { return _basicDeliverEventArgs; }
        }

        public RabbitCallbackMessage(string topic, string exchange, byte[] body, 
            IModel channel, BasicDeliverEventArgs basicDeliverEventArgs) :
            base(topic, body)
        {
            _channel = channel;
            _basicDeliverEventArgs = basicDeliverEventArgs;
            _exchange = exchange;
        }

        public override void Commit()
        {
            if (_channel == null || _basicDeliverEventArgs == null)
            {
                return;
            }

            _channel.BasicAck(_basicDeliverEventArgs.DeliveryTag,false);
        }

        public override void Dispose()
        {
            _channel=null;
            _basicDeliverEventArgs=null;
            _exchange=null;
            base.Dispose();
        }
        protected string _exchange = null;
        /// <summary>
        /// 交换机(Rabbit有效)
        /// </summary>
        public string Exchange
        {
            get { return _exchange; }
        }
        ~RabbitCallbackMessage() 
        {
            Dispose();
        }
    }
}
