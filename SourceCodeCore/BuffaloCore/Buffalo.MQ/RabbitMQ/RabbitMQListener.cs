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
        private IChannel _channel;
        
        /// <summary>
        /// 信道
        /// </summary>
        public IChannel Channel
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
        private async Task OpenAsync()
        {
            

            _connection = await _config.Factory.CreateConnectionAsync();
            _channel = await _connection.CreateChannelAsync();
            //IBasicProperties properties = _channel.CreateBasicProperties();
            //properties.DeliveryMode = (DeliveryModes)_config.DeliveryMode;

            //UInt32 prefetchSize,  每次取的长度
            //UInt16 prefetchCount,     每次取几条
            //Boolean global    是否对connection通用
            await _channel.BasicQosAsync(0, 1, true);
            await _channel.ExchangeDeclareAsync(_config.ExchangeName, _config.ExchangeMode, _config.DeliveryMode == 2, _config.AutoDelete, null);
            if (_config.QueueName != null)
            {
                foreach (string name in _config.QueueName)
                {
                    await _channel.QueueDeclareAsync(name, _config.DeliveryMode == 2, false, _config.AutoDelete, null);
                    await _channel.QueueBindAsync(name, _config.ExchangeName, "", null);
                }
            }

            //_connection = _config.Factory.CreateConnection();
            //_channel = _connection.CreateModel();
            //IBasicProperties properties = _channel.CreateBasicProperties();
            //properties.DeliveryMode = _config.DeliveryMode;
            
            ////UInt32 prefetchSize,  每次取的长度
            ////UInt16 prefetchCount,     每次取几条
            ////Boolean global    是否对connection通用
            //_channel.BasicQos(0, 1, true);
            //_channel.ExchangeDeclare(_config.ExchangeName, _config.ExchangeMode, _config.DeliveryMode == 2, _config.AutoDelete, null);
            //if (_config.QueueName != null)
            //{
            //    foreach (string name in _config.QueueName)
            //    {
            //        _channel.QueueDeclare(name, _config.DeliveryMode == 2, false, _config.AutoDelete, null);
            //        _channel.QueueBind(name, _config.ExchangeName, "", null);
            //    }
            //}
        }
        /// <summary>
        /// 打开事件监听
        /// </summary>
        public override void StartListend(IEnumerable<string> listenKeys)
        {
            
            OpenAsync().Wait();
            ResetWait();
            AsyncEventingBasicConsumer consumer = new AsyncEventingBasicConsumer(_channel);
            if (_config.QueueName != null)
            {
                foreach (string name in _config.QueueName)
                {
                    _channel.QueueDeclareAsync(name, _config.DeliveryMode == 2, false, _config.AutoDelete, null).Wait();
                    
                    foreach (string key in listenKeys)
                    {

                        _channel.QueueBindAsync(name, _config.ExchangeName, key, null).Wait();

                    }

                    _channel.BasicConsumeAsync(name, false, consumer).Wait();
                }
            }
            consumer.ReceivedAsync += Consumer_Received;
            SetWait();
        }
        public override void StartListend(IEnumerable<MQOffestInfo> listenKeys)
        {
            StartListend(MQUnit.GetLintenKeys(listenKeys));
        }
        private async Task Consumer_Received(object sender, BasicDeliverEventArgs e)
        {
            RabbitCallbackMessage mess = new RabbitCallbackMessage(e.RoutingKey, e.Exchange, e.Body.ToArray(), _channel, e);
           
            CallBack(mess);
            //_channel.BasicAck(e.DeliveryTag, false);//手动应答
        }

        

        public override void Dispose()
        {
            Close();
        }

        public override void Close()
        {
            if (_channel != null)
            {
                try
                {
                    _channel.CloseAsync().Wait();
                }
                catch (Exception ex)
                {
                    OnException(ex);
                }
                _channel = null;
            }
            if (_connection != null)
            {
                try
                {
                    _connection.CloseAsync().Wait();
                }
                catch (Exception ex)
                {
                    OnException(ex);
                }
                _connection = null;
            }
            DisponseWait();
        }
    }
}
