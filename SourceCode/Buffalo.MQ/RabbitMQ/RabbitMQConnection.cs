using Buffalo.ArgCommon;
using Buffalo.Kernel;
using Buffalo.MQ.RedisMQ;
using MQTTnet.Internal;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buffalo.MQ.RabbitMQ
{
    
    /// <summary>
    /// RabbitMQ适配
    /// </summary>
    public partial class RabbitMQConnection : MQConnection
    {
       
        private IConnection _connection;
        private IChannel _channel;
        private RabbitMQConfig _config;
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

        public override bool IsOpen
        {
            get
            {
                return _channel != null;
            }

           
        }
        

        /// <summary>
        /// RabbitMQ适配
        /// </summary>
        /// <param name="connString">连接字符串</param>
        public RabbitMQConnection(RabbitMQConfig config)
        {
            _config = config;
        }
        /// <summary>
        /// 打来连接
        /// </summary>
        protected override void Open()
        {
            OpenAsync().Wait();
        }
        /// <summary>
        /// 打来连接
        /// </summary>
        protected override async Task OpenAsync()
        {
            if (IsOpen)
            {
                return;
            }

            
            _connection = await _config.Factory.CreateConnectionAsync();
            _channel =await _connection.CreateChannelAsync();
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


        }
        /// <summary>
        /// 发布内容
        /// </summary>
        /// <param name="routingKey"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        protected override APIResault SendMessage(string routingKey, byte[] body)
        {
            return SendMessageAsync(routingKey, body).Result;


        }
        /// <summary>
        /// 发布内容
        /// </summary>
        /// <param name="routingKey"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        protected override APIResault SendMessage(MQSendMessage message)
        {
            return SendMessageAsync(message).Result;
        }
        /// <summary>
        /// 删除队列(Rabbit可用)
        /// </summary>
        /// <param name="queueName">队列名，如果为null则全删除</param>
        public void DeleteQueue(IEnumerable<string> queueName, bool ifUnused, bool ifEmpty)
        {
            IEnumerable<string> curDelete = queueName;
            if (curDelete == null)
            {
                curDelete = _config.QueueName;
            }
            foreach (string delName in curDelete)
            {
                _channel.QueueDeleteAsync(delName, ifUnused, ifEmpty).Wait();
            }
        }
        /// <summary>
        /// 删除交换器
        /// </summary>

        public void DeleteExchange(bool ifUnused)
        {
            _channel.ExchangeDeleteAsync(_config.ExchangeName, ifUnused).Wait();
        }

        /// <summary>
        /// 关闭连接
        /// </summary>
        public override void Close()
        {
            CloseAsync().Wait();
        }



        protected override APIResault StartTran()
        {
            
            return StartTranAsync().Result;
        }

        protected override APIResault CommitTran()
        {
            return CommitTranAsync().Result;
        }
        protected override async Task<APIResault> CommitTranAsync()
        {
             await _channel.TxCommitAsync();
            return ApiCommon.GetSuccess();
        }
        protected override APIResault RoolbackTran()
        {
            return RoolbackTranAsync().Result;
        }

        protected override async Task<APIResault> SendMessageAsync(MQSendMessage message)
        {
            MQRabbitMessage mess = message as MQRabbitMessage;
            if (mess == null)
            {
                return ApiCommon.GetFault("message must been a MQRabbitMessage");
            }
            string exchange = mess.Exchange;
            if (string.IsNullOrWhiteSpace(exchange))
            {
                exchange = _config.ExchangeName;
            }
            await _channel.BasicPublishAsync(exchange, mess.Topic, mess.Mandatory, mess.Value);
            return ApiCommon.GetSuccess();
        }

        protected override async Task<APIResault> SendMessageAsync(string key, byte[] body)
        {
            await _channel.BasicPublishAsync(_config.ExchangeName, key, false,  body);
            return ApiCommon.GetSuccess();
        }

        protected override async Task<APIResault> StartTranAsync()
        {
            await OpenAsync();
            await _channel.TxSelectAsync();
            return ApiCommon.GetSuccess();
        }

        protected override async Task<APIResault> RoolbackTranAsync()
        {
            await _channel.TxRollbackAsync();
            return ApiCommon.GetSuccess();
        }

        public override async Task CloseAsync()
        {
            if (_channel != null)
            {
                await _channel.CloseAsync();
                _channel = null;
            }
            if (_connection != null)
            {
                await _connection.CloseAsync();
                _connection = null;
            }
        }
    }
}
