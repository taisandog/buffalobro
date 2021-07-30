using Aliyun.MQ;
using Aliyun.MQ.Model;
using Buffalo.ArgCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buffalo.MQ.RocketMQ
{
    public class RocketMQConnection : MQConnection
    {
        /// <summary>
        /// 配置
        /// </summary>
        private RocketMQConfig _config;
        /// <summary>
        /// 生产者
        /// </summary>
        private MQProducer _producer;
        /// <summary>
        /// 带事件的生产者
        /// </summary>
        private MQTransProducer _tranProducer;

        private Queue<TopicMessage> _que = null;
        /// <summary>
        /// RabbitMQ适配
        /// </summary>
        /// <param name="connString">连接字符串</param>
        public RocketMQConnection(RocketMQConfig config)
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

            _producer = _config.Client.GetProducer(_config.InstanceId, _config.TopicName);

        }

        /// <summary>
        /// 发布内容
        /// </summary>
        /// <param name="routingKey"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        protected override APIResault SendMessage(string routingKey, string body)
        {
            TopicMessage topicMessage = new TopicMessage(body,routingKey);

            TopicMessage mess=_producer.PublishMessage(topicMessage);
            return ApiCommon.GetSuccess(null, mess);
        }

        public APIResault SendToPublic(TopicMessage topicMessage) 
        {
            _tranProducer
        }

        /// <summary>
        /// 删除队列(Rabbit可用)
        /// </summary>
        /// <param name="queueName">队列名，如果为null则全删除</param>
        public override void DeleteQueue(IEnumerable<string> queueName, bool ifUnused, bool ifEmpty)
        {
            
        }
        /// <summary>
        /// 删除交换器
        /// </summary>

        public override void DeleteTopic(bool ifUnused)
        {
            
        }

        

        /// <summary>
        /// 关闭连接
        /// </summary>
        protected override void Close()
        {
            if (_que != null)
            {
                _que.Clear();
            }
            _que = null;
            _producer = null;
            _tranProducer = null;
        }



        protected override APIResault StartTran()
        {
            _que = new Queue<TopicMessage>();
            return ApiCommon.GetSuccess();
        }

        protected override APIResault CommitTran()
        {
            if (_que != null)
            {
                TopicMessage mess = null;
                while (_que.Count > 0)
                {
                    mess = _que.Dequeue();
                    SendToPublic(mess.RoutingKey, mess.Value);

                }
            }
            return ApiCommon.GetSuccess();
        }

        protected override APIResault RoolbackTran()
        {
            if (_que != null)
            {
                _que.Clear();
            }
            return ApiCommon.GetSuccess();
        }
    }
}
