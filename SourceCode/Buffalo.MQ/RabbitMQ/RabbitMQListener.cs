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
    public partial class RabbitMQListener:MQListener
    {
        private IModel _channel;
        
        /// <summary>
        /// 信道
        /// </summary>
        public IModel Channel
        {
            get
            {
                return _channel;
            }
        }
        private IConnection _connection;
        private RabbitMQConfig _config;
        /// <summary>
        /// RabbitMQ适配
        /// </summary>
        /// <param name="connString">连接字符串</param>
        public RabbitMQListener(RabbitMQConfig config)
        {
            _config = config;
        }
        /// <summary>
        /// 打来连接
        /// </summary>
        private void Open()
        {
            Close();
            _connection = _config.Factory.CreateConnection();
            _channel = _connection.CreateModel();
            IBasicProperties properties = _channel.CreateBasicProperties();
            properties.DeliveryMode = _config.DeliveryMode;

            //UInt32 prefetchSize,  每次取的长度
            //UInt16 prefetchCount,     每次取几条
            //Boolean global    是否对connection通用
            _channel.BasicQos(0, 1, true);
            _channel.ExchangeDeclare(_config.ExchangeName, _config.ExchangeMode, _config.DeliveryMode == 2, _config.AutoDelete, null);
            if (_config.QueueName != null)
            {
                foreach (string name in _config.QueueName)
                {
                    _channel.QueueDeclare(name, _config.DeliveryMode == 2, false, _config.AutoDelete, null);
                    _channel.QueueBind(name, _config.ExchangeName, "", null);
                }
            }
        }
        /// <summary>
        /// 打开事件监听
        /// </summary>
        public override void StartListend(IEnumerable<string> listenKeys)
        {
            Open();
            EventingBasicConsumer consumer = new EventingBasicConsumer(_channel);
            if (_config.QueueName != null)
            {
                foreach (string name in _config.QueueName)
                {
                    _channel.QueueDeclare(name, _config.DeliveryMode == 2, false, _config.AutoDelete, null);
                    
                    foreach (string key in listenKeys)
                    {

                        _channel.QueueBind(name, _config.ExchangeName, key, null);

                    }

                    _channel.BasicConsume(name, false, consumer);
                }
            }
            consumer.Received += Consumer_Received;
        }
        public override void StartListend(IEnumerable<MQOffestInfo> listenKeys)
        {
            StartListend(MQUnit.GetLintenKeys(listenKeys));
        }
        private void Consumer_Received(object sender, BasicDeliverEventArgs e)
        {
            byte[] bytes = e.Body;
            string key = e.RoutingKey;
            string exchange = e.Exchange;
            CallBack(key, exchange, bytes,0,0);
            _channel.BasicAck(e.DeliveryTag, false);//手动应答
        }

        

        public override void Dispose()
        {
            Close();
        }

        public override void Close()
        {
            if (_channel != null)
            {
                _channel.Close();
                _channel = null;
            }
            if (_connection != null)
            {
                _connection.Close();
                _connection = null;
            }
        }
    }
}
