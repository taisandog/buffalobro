using System;
using System.Collections.Generic;
using System.Text;

namespace Buffalo.MQ
{
    /// <summary>
    /// 发消息
    /// </summary>
    public class MQSendMessage
    {
        /// <summary>
        /// 发送消息类
        /// </summary>
        /// <param name="topic">话题</param>
        /// <param name="value">内容</param>
        public MQSendMessage(string topic,byte[] value) 
        {
            _topic = topic;
            _value = value;
        }
        /// <summary>
        /// 话题
        /// </summary>
        private string _topic;
        /// <summary>
        /// 话题
        /// </summary>
        public string Topic
        {
            get { return _topic; }
        }
        /// <summary>
        /// 值
        /// </summary>
        private byte[] _value;
        /// <summary>
        /// 内容
        /// </summary>
        public byte[] Value
        {
            get { return _value; }
        }

        public virtual void Dispose()
        {
            _value = null;
            _topic = null;

            GC.SuppressFinalize(this);
        }

        ~MQSendMessage()
        {
            Dispose();
        }
    }
}
