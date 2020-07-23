using Buffalo.Kernel;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buffalo.MQ.RabbitMQ
{
    public class RabbitMQConfig:MQConfigBase
    {
        public readonly ConnectionFactory Factory;
        /// <summary>
        /// 交换方式(direct,fanout,headers,topic;)
        /// </summary>
        public string ExchangeMode;
        /// <summary>
        /// 交换名称
        /// </summary>
        public string ExchangeName;
        /// <summary>
        /// 类型 1不持久化，2持久化
        /// </summary>
        public byte DeliveryMode;

        /// <summary>
        /// 队列名
        /// </summary>
        public string[] QueueName;
        /// <summary>
        /// 自动删除
        /// </summary>
        public bool AutoDelete;

       

        public RabbitMQConfig(string connString) : base(connString)
        {
            
            Factory = new ConnectionFactory();
            Factory.UserName = _configs.GetDicValue<string, string>("uid");
#if (NET_4_7_2 || NET_4_6_2)

#else
            Factory.Protocol = Protocols.DefaultProtocol;
#endif

            string server = _configs.GetDicValue<string, string>("server");
            if (!string.IsNullOrWhiteSpace(server))
            {
                string[] serPart = server.Split(':');
                Factory.HostName = serPart[0];
                if (serPart.Length > 1)
                {
                    Factory.Port = serPart[1].ConvertTo<int>();
                }
            }
            
            Factory.Password = _configs.GetDicValue<string, string>("pwd");
            ExchangeMode = _configs.GetDicValue<string, string>("exchangeMode");
            if (string.Equals(ExchangeMode, "topic"))
            {
                Factory.VirtualHost = _configs.GetDicValue<string, string>("vhost");
                if (string.IsNullOrWhiteSpace(Factory.VirtualHost))
                {
                    Factory.VirtualHost = "/";
                }
            }
            ExchangeName = _configs.GetDicValue<string, string>("exchangeName");
            AutoDelete = _configs.GetDicValue<string, string>("autoDelete") == "1";
            string queueName = _configs.GetDicValue<string, string>("queueName");//队列名，用|隔开,只有Fanout模式可用
            if (!string.IsNullOrWhiteSpace(queueName))
            {
                QueueName = queueName.Split('|');
            }
            string deliveryModeStr = _configs.GetDicValue<string, string>("deliveryMode");//1不持久化，2持久化

            if (deliveryModeStr == null)
            {
                DeliveryMode = 1;
            }
            else
            {
                DeliveryMode = deliveryModeStr.ConvertTo<byte>();
            }
            if (string.IsNullOrWhiteSpace(ExchangeMode))
            {
                ExchangeMode = ExchangeType.Direct;
            }

            
        }


        public override MQConnection CreateConnection()
        {
            return new RabbitMQConnection(this);
        }

        public override MQListener CreateListener()
        {
            return new RabbitMQListener(this);
        }
    }
}
