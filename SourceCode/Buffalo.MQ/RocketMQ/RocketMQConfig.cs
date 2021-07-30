using Aliyun.MQ;
using Buffalo.Kernel;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buffalo.MQ.RocketMQ
{
    public class RocketMQConfig : MQConfigBase
    {
        /// <summary>
        /// 设置HTTP接入域名（此处以公共云生产环境为例）
        /// </summary>
        public readonly string Endpoint;
        /// <summary>
        /// AccessKey 阿里云身份验证，在阿里云服务器管理控制台创建
        /// </summary>
        public readonly string AccessKeyId ;
        /// <summary>
        /// SecretKey 阿里云身份验证，在阿里云服务器管理控制台创建
        /// </summary>
        public readonly string SecretAccessKey ;
        /// <summary>
        /// 所属的 Topic
        /// </summary>
        public readonly string TopicName ;
        /// <summary>
        /// Topic所属实例ID，默认实例为空
        /// </summary>
        public readonly string InstanceId ;
        /// <summary>
        /// 您在控制台创建的 Consumer ID(Group ID)
        /// </summary>
        public readonly string GroupId;

        /// <summary>
        /// MQ客户端
        /// </summary>
        public readonly MQClient Client;

        public RocketMQConfig(string connString) : base(connString)
        {


            Endpoint = _configs.GetDicValue<string, string>("endpoint");
            AccessKeyId = _configs.GetDicValue<string, string>("accessKeyId");
            SecretAccessKey = _configs.GetDicValue<string, string>("secretAccessKey");
            TopicName = _configs.GetDicValue<string, string>("topic");
            InstanceId = _configs.GetDicValue<string, string>("instanceId");
            GroupId = _configs.GetDicValue<string, string>("groupId");

            Client = new MQClient(AccessKeyId, SecretAccessKey, Endpoint);
        }


        public override MQConnection CreateConnection()
        {
           
        }

        public override MQListener CreateListener()
        {
           
        }
    }
}
