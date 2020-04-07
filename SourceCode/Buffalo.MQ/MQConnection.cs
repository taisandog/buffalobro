using Buffalo.ArgCommon;
using Buffalo.MQ.RabbitMQ;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buffalo.MQ
{
    public delegate void DelOnMQReceived(MQConnection sender,string exchange, string routingKey, byte[] body);

    public abstract class MQConnection : IDisposable
    {
        /// <summary>
        /// 获取连接
        /// </summary>
        /// <param name="mqType"></param>
        /// <param name="connectString"></param>
        /// <returns></returns>
        public static MQConnection GetConnection(string mqType,string connectString)
        {
            if (string.Equals(mqType, "rabbitMQ", StringComparison.CurrentCultureIgnoreCase))
            {
                return new RabbitMQConnection(connectString);
            }
            return null;
        }

        public event DelOnMQReceived OnMQReceived;



        /// <summary>
        /// 初始化发布者模式
        /// </summary>
        public abstract void InitPublic();
        /// <summary>
        /// 打开事件监听
        /// </summary>
        public abstract void StartListend(IEnumerable<string> keys);

        public abstract void Dispose();

        /// <summary>
        /// 发布内容
        /// </summary>
        /// <param name="routingKey"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        public abstract APIResault BasicPublish(string routingKey, byte[] body);

        /// <summary>
        /// 删除队列(Rabbit可用)
        /// </summary>
        /// <param name="queueName">队列名，如果为null则全删除</param>
        public abstract void DeleteQueue(IEnumerable<string> queueName, bool ifUnused, bool ifEmpty);
        /// <summary>
        /// 删除交换器
        /// </summary>

        public abstract void DeleteExchange(bool ifUnused);
        /// <summary>
        /// 关闭连接
        /// </summary>
        public abstract void Close();
        /// <summary>
        /// 监听信息后回调
        /// </summary>
        protected void CallBack(string routingKey,string exchange, byte[] body)
        {
            if (OnMQReceived == null)
            {
                return;
            }
            OnMQReceived(this, exchange,routingKey, body);
        }
    }
}
