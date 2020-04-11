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
    //连接字符串:server=192.168.1.25;vhost="/";uid=admin;pwd=111111;exchange=fanout
    /// <summary>
    /// RabbitMQ适配
    /// </summary>
    public partial class RabbitMQConnection : MQConnection
    {
        private ConnectionFactory _fac;
        private IConnection _connection;
        private IModel _channel;
        /// <summary>
        /// 交换方式(direct,fanout,headers,topic;)
        /// </summary>
        private string _exchangeMode;
        /// <summary>
        /// 类型 1不持久化，2持久化
        /// </summary>
        private byte _deliveryMode;

        /// <summary>
        /// 队列名
        /// </summary>
        private string[] _queueName;
        /// <summary>
        /// 自动删除
        /// </summary>
        private bool _autoDelete;
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
        /// <summary>
        /// 监听的路由键
        /// </summary>
        private IEnumerable<string> _listeneKeys;
        /// <summary>
        /// RabbitMQ适配
        /// </summary>
        /// <param name="connString">连接字符串</param>
        public RabbitMQConnection(string connString)
        {

            Dictionary<string, string> hs = ConnStringFilter.GetConnectInfo(connString);
            _fac = new ConnectionFactory();
            _fac.UserName = hs.GetDicValue<string, string>("uid");
            _fac.VirtualHost = hs.GetDicValue<string, string>("vhost");
            if (string.IsNullOrWhiteSpace(_fac.VirtualHost))
            {
                _fac.VirtualHost = "/";
            }
            _fac.Protocol = Protocols.DefaultProtocol;
            _fac.HostName = hs.GetDicValue<string, string>("server");
            _fac.Password = hs.GetDicValue<string, string>("pwd");
            _exchangeMode = hs.GetDicValue<string, string>("exchangeMode");
            _topic = hs.GetDicValue<string, string>("exchangeName");
            _autoDelete = hs.GetDicValue<string, string>("autoDelete") == "1";
            string queueName = hs.GetDicValue<string, string>("queueName");//队列名，用|隔开,只有Fanout模式可用
            if (!string.IsNullOrWhiteSpace(queueName))
            {
                _queueName = queueName.Split('|');
            }
            string deliveryModeStr = hs.GetDicValue<string, string>("deliveryMode");//1不持久化，2持久化

            if (deliveryModeStr == null)
            {
                _deliveryMode = 1;
            }
            else
            {
                _deliveryMode = deliveryModeStr.ConvertTo<byte>();
            }
            if (string.IsNullOrWhiteSpace(_exchangeMode))
            {
                _exchangeMode = ExchangeType.Direct;
            }

            string listeneKeys = hs.GetDicValue<string, string>("lisKeys");//监听的键,用|隔开
            if (!string.IsNullOrWhiteSpace(listeneKeys))
            {
                _listeneKeys = listeneKeys.Split('|');
            }
        }
        /// <summary>
        /// 打来连接
        /// </summary>
        private void Open()
        {
            Close();
            _connection = _fac.CreateConnection();
            _channel = _connection.CreateModel();
            IBasicProperties properties = _channel.CreateBasicProperties();
            properties.DeliveryMode = _deliveryMode;

            //UInt32 prefetchSize,  每次取的长度
            //UInt16 prefetchCount,     每次取几条
            //Boolean global    是否对connection通用
            _channel.BasicQos(0, 1, true);
            _channel.ExchangeDeclare(_topic, _exchangeMode, _deliveryMode == 2, _autoDelete, null);
            
        }
        /// <summary>
        /// 初始化发布者模式
        /// </summary>
        public override void InitPublic()
        {
            Open();
            if (_queueName != null)
            {
                foreach (string name in _queueName)
                {
                    _channel.QueueDeclare(name, _deliveryMode == 2, false, _autoDelete, null);
                    _channel.QueueBind(name, _topic, "", null);
                }
            }
        }
        

       
        
        /// <summary>
        /// 发布内容
        /// </summary>
        /// <param name="routingKey"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        public override APIResault SendMessage(string routingKey,byte[] body)
        {
            
            _channel.BasicPublish(_topic, routingKey, false,null, body);
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
                curDelete = _queueName;
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
            _channel.ExchangeDelete(_topic, ifUnused);
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

        public override void Dispose()
        {
            Close();
        }
        ~RabbitMQConnection()
        {
            Close();
            GC.SuppressFinalize(this);
        }
    }
}
