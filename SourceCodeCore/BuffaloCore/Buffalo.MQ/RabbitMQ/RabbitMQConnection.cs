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
    
    /// <summary>
    /// RabbitMQ适配
    /// </summary>
    public partial class RabbitMQConnection : MQConnection
    {
       
        private IConnection _connection;
        private IModel _channel;
        private RabbitMQConfig _config;
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
            if (IsOpen)
            {
                return;
            }
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
        /// 发布内容
        /// </summary>
        /// <param name="routingKey"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        protected override APIResault SendMessage(string routingKey, byte[] body)
        {

            _channel.BasicPublish(_config.ExchangeName, routingKey, false, null, body);
            return ApiCommon.GetSuccess();
        }
      
        /// <summary>
        /// 删除队列(Rabbit可用)
        /// </summary>
        /// <param name="queueName">队列名，如果为null则全删除</param>
        public override void DeleteQueue(IEnumerable<string> queueName, bool ifUnused, bool ifEmpty)
        {
            IEnumerable<string> curDelete = queueName;
            if (curDelete == null)
            {
                curDelete = _config.QueueName;
            }
            foreach (string delName in curDelete)
            {
                _channel.QueueDelete(delName,ifUnused,ifEmpty);
            }
        }
        /// <summary>
        /// 删除交换器
        /// </summary>
        
        public override void DeleteTopic(bool ifUnused)
        {
            _channel.ExchangeDelete(_config.ExchangeName, ifUnused);
        }

        /// <summary>
        /// 关闭连接
        /// </summary>
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



        protected override APIResault StartTran()
        {
            Open();
            _channel.TxSelect();
            return ApiCommon.GetSuccess();
        }

        protected override APIResault CommitTran()
        {
            _channel.TxCommit();
            return ApiCommon.GetSuccess();
        }

        protected override APIResault RoolbackTran()
        {
            _channel.TxRollback();
            return ApiCommon.GetSuccess();
        }

        
    }
}
