using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buffalo.MQ
{
    public delegate void DelOnMQReceived(MQListener sender, string exchange, string routingKey, byte[] body, int partition, long offset);

    public delegate void DelOnMQException(MQListener sender, Exception ex);

    public abstract class MQListener
    {
        /// <summary>
        /// 接收数据
        /// </summary>
        public event DelOnMQReceived OnMQReceived;
        /// <summary>
        /// 发生异常
        /// </summary>
        public event DelOnMQException OnMQException;

        /// <summary>
        /// 打开事件监听
        /// </summary>
        /// <param name="listenKeys">监听键</param>
        public abstract void StartListend(IEnumerable<string> listenKeys);
        /// <summary>
        /// 打开事件监听
        /// </summary>
        /// <param name="listenKeys">监听键</param>
        public abstract void StartListend(IEnumerable<MQOffestInfo> listenKeys);

        public abstract void Dispose();

        /// <summary>
        /// 关闭连接
        /// </summary>
        public abstract void Close();

       
        /// <summary>
        /// 监听信息后回调
        /// </summary>
        protected void CallBack(string routingKey, string exchange, byte[] body, int partition, long offset)
        {
            if (OnMQReceived == null)
            {
                return;
            }
            OnMQReceived(this, exchange, routingKey, body, partition, offset);
        }
        
        /// <summary>
        /// 监听信息后回调
        /// </summary>
        protected void OnException(Exception ex)
        {
            if (OnMQException == null)
            {
                return;
            }
            OnMQException(this, ex);
        }
    }
}
