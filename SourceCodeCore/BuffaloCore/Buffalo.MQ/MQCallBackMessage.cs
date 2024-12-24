using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Buffalo.MQ
{
    /// <summary>
    /// 回调信息
    /// </summary>
    public abstract class MQCallBackMessage:IDisposable
    {
        /// <summary>
        /// 回调信息
        /// </summary>
        /// <param name="topic">主题(Rabbit是RoutingKey)</param>
        /// <param name="exchange">交换</param>
        /// <param name="body">信息体</param>
        /// <param name="partition">分区(kafka有效)</param>
        /// <param name="offset">位置(kafka有效)</param>
        public MQCallBackMessage(string topic, byte[] body) 
        {
            _topic = topic;
            _body = body;
            
        }
       
        /// <summary>
        /// kafka和Rabbit的autocommit为false情况下此调用为Ark应答
        /// </summary>
        public abstract void Commit();

        /// <summary>
        /// kafka和Rabbit的autocommit为false情况下此调用为Ark应答
        /// </summary>
        public abstract Task CommitAsync();

        public virtual void Dispose()
        {
            _body = null;
            _topic = null;

            GC.SuppressFinalize(this);
        }

        protected string _topic = null;


        
        /// <summary>
        /// Key
        /// </summary>
        public string RoutingKey 
        {
            get { return _topic; }
        }
        /// <summary>
        /// Topic
        /// </summary>
        public string Topic
        {
            get { return _topic; }
        }

       
        protected byte[] _body = null;
        /// <summary>
        /// 内容
        /// </summary>
        public byte[] Body
        {
            get { return _body; }
        }

       
    }
}
