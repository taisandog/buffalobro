using Buffalo.ArgCommon;
using Buffalo.Kernel;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buffalo.MQ.RabbitMQ
{
    public partial class RabbitMQConnection
    {
        
        /// <summary>
        /// 打开事件监听
        /// </summary>
        public override void StartListend()
        {
            Open();


            EventingBasicConsumer consumer = new EventingBasicConsumer(_channel);


            if (_queueName != null)
            {
                foreach (string name in _queueName)
                {
                    _channel.QueueDeclare(name, _deliveryMode == 2, false, _autoDelete, null);
                    if (_listeneKeys != null)
                    {
                        foreach (string key in _listeneKeys)
                        {

                            _channel.QueueBind(name, _topic, key, null);

                        }
                    }
                    _channel.BasicConsume(name, false, consumer);
                }

            }
            consumer.Received += Consumer_Received;
            
        }

        private void Consumer_Received(object sender, BasicDeliverEventArgs e)
        {
            byte[] bytes = e.Body;
            string key = e.RoutingKey;
            string exchange = e.Exchange;
            CallBack(key, exchange, bytes);
            _channel.BasicAck(e.DeliveryTag, false);//手动应答
        }
    }
}
