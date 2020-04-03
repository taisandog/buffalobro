using Buffalo.Kernel;
using RabbitMQ.Client;
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
    public class RabbitMQConnection: IMQConnection
    {
        private ConnectionFactory _fac;
        private IConnection _connection;
        private IModel _channel;
        private string _exchangeMode;
        /// <summary>
        /// RabbitMQ适配
        /// </summary>
        /// <param name="connString">连接字符串</param>
        public RabbitMQConnection(string connString)
        {
            
            Dictionary<string, string> hs = ConnStringFilter.GetConnectInfo(connString);
            _fac = new ConnectionFactory();
            _fac.UserName = hs.GetDicValue<string, string>("uid");
            _fac.VirtualHost= hs.GetDicValue<string, string>("vhost");
            _fac.HostName= hs.GetDicValue<string, string>("server");
            _fac.Password = hs.GetDicValue<string, string>("pwd");
            _exchangeMode= hs.GetDicValue<string, string>("exchange");
            if (string.IsNullOrWhiteSpace(_exchangeMode))
            {
                _exchangeMode = ExchangeType.Fanout;
            }
        }
        /// <summary>
        /// 打来连接
        /// </summary>
        public void Open()
        {
            Close();
            _connection = _fac.CreateConnection();
            _channel = _connection.CreateModel();
        }
        /// <summary>
        /// 关闭连接
        /// </summary>
        public void Close()
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

        public void Dispose()
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
