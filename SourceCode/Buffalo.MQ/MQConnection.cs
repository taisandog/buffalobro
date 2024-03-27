using Buffalo.ArgCommon;
using Buffalo.MQ.KafkaMQ;
#if !NET_4_5
using Buffalo.MQ.RabbitMQ;
#endif
using Buffalo.MQ.RedisMQ;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buffalo.MQ
{


    public abstract class MQConnection :IDisposable
    {
        /// <summary>
        /// 默认编码
        /// </summary>
        protected static Encoding DefaultEncoding = Encoding.UTF8;
        /// <summary>
        /// 是否开启
        /// </summary>
        public abstract bool IsOpen
        {
            get;
        }

        internal bool _isTran;
        /// <summary>
        /// 是否开启了事务
        /// </summary>
        public bool IsTransaction
        {
            get
            {
                return _isTran;
            }
            
        }

        //internal bool _isAutoClose=true;
        ///// <summary>
        ///// 是否自动关闭
        ///// </summary>
        //public bool IsAutoClose
        //{
        //    get
        //    {
        //        return _isAutoClose;
        //    }
        //    set 
        //    {
        //        _isAutoClose = value;
        //    }
        //}

        internal bool _isBatch;
        /// <summary>
        /// 是否在批量处理中
        /// </summary>
        public bool IsBatch
        {
            get
            {
                return _isBatch;
            }
        }
        /// <summary>
        /// 发送信息
        /// </summary>
        /// <param name="key"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        public APIResault Send(string key, byte[] body)
        {
            Open();
            APIResault res=SendMessage(key, body);
            //AutoClose();
            return res;
        }
        /// <summary>
        /// 发送信息
        /// </summary>
        /// <param name="key"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        public APIResault Send(string key, string body)
        {
            Open();
            APIResault res = SendMessage(key, body);
            //AutoClose();
            return res;
        }
        /// <summary>
        /// 发送信息
        /// </summary>
        /// <param name="message">内容类</param>
        /// <returns></returns>
        public APIResault Send(MQSendMessage message)
        {
            Open();
            APIResault res = SendMessage(message);

            //AutoClose();
            return res;
        }
        /// <summary>
        /// 开启事务
        /// </summary>
        /// <returns></returns>
        protected abstract APIResault StartTran();
        /// <summary>
        /// 提交事务
        /// </summary>
        /// <returns></returns>
        protected abstract APIResault CommitTran();
        /// <summary>
        /// 回滚事务
        /// </summary>
        /// <returns></returns>
        protected abstract APIResault RoolbackTran();

        /// <summary>
        /// 开启事务
        /// </summary>
        /// <returns></returns>
        public MQTransaction StartTransaction()
        {
            
            if (!_isTran)
            {
                
                StartTran();
                _isTran = true;
                return new MQTransaction(this);
            }
            return new MQTransaction(null);
        }
        /// <summary>
        /// 回滚事务
        /// </summary>
        /// <returns></returns>
        internal APIResault RoolbackTransaction()
        {
            _isTran = false;
            return RoolbackTran();
        }
        /// <summary>
        /// 提交事务
        /// </summary>
        /// <returns></returns>
        internal APIResault CommitTransaction()
        {
            _isTran = false;
            return CommitTran();
        }
        ///// <summary>
        ///// 开启批量处理
        ///// </summary>
        ///// <returns></returns>
        //public MQBatchAction StartBatchAction()
        //{
        //    if (!_isBatch)
        //    {
        //        _isBatch = true;
        //        return new MQBatchAction(this);
        //    }
        //    return new MQBatchAction(null);
        //}
        
        /// <summary>
        /// 初始化发布者模式
        /// </summary>
        protected abstract void Open();



        /// <summary>
        /// 发布内容
        /// </summary>
        /// <param name="mess">内容类</param>
        /// <returns></returns>
        protected abstract APIResault SendMessage(MQSendMessage mess);

        /// <summary>
        /// 发布内容
        /// </summary>
        /// <param name="key">筛选的键</param>
        /// <param name="body">内容</param>
        /// <returns></returns>
        protected abstract APIResault SendMessage(string key, byte[] body);
        /// <summary>
        /// 发布内容
        /// </summary>
        /// <param name="key">筛选的键</param>
        /// <param name="body">内容</param>
        /// <returns></returns>
        protected virtual APIResault SendMessage(string key, string body)
        {
            byte[] content = DefaultEncoding.GetBytes(body);

            return SendMessage(key, content);

        }
        ///// <summary>
        ///// 删除队列(Rabbit可用)
        ///// </summary>
        ///// <param name="queueName">队列名，如果为null则全删除</param>
        //public abstract void DeleteQueue(IEnumerable<string> queueName, bool ifUnused, bool ifEmpty);
        ///// <summary>
        ///// 删除交换器
        ///// </summary>
        //public abstract void DeleteTopic(bool ifUnused);
        /// <summary>
        /// 关闭连接
        /// </summary>
        public abstract void Close();
        ///// <summary>
        ///// 自动关闭
        ///// </summary>
        //public void AutoClose()
        //{
        //    if(!_isBatch && !_isTran && _isAutoClose)
        //    {
        //        Close();
        //    }
        //}

        public void Dispose()
        {
            Close();

            GC.SuppressFinalize(this);
        }
    }
}
