using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buffalo.MQ
{
    /// <summary>
    /// 队列位置信息
    /// </summary>
    public class MQOffestInfo
    {
        /// <summary>
        /// 监听
        /// </summary>
        /// <param name="key">监听键</param>
        /// <param name="partition">分区(Kafka有用)</param>
        /// <param name="offset">位置(Kafka有用)</param>
        /// <param name="queueKey">队列键(Redis有用)</param>
        public MQOffestInfo(string key, int partition, long offset,string queueKey)
        {
            _key = key;
            _partition = partition;
            _offest = offset;
            _queueKey = queueKey;
        }

        private string _queueKey;
        /// <summary>
        /// 队列键(Redis有用)
        /// </summary>
        public string QueueKey
        {
            get { return _queueKey; }
        }
        /// <summary>
        /// 监听键
        /// </summary>
        private string _key;
        /// <summary>
        /// 监听键
        /// </summary>
        public string Key
        {
            get
            {
                return _key;
            }
        }
        /// <summary>
        /// 分区
        /// </summary>
        private int _partition;
        /// <summary>
        /// 分区
        /// </summary>
        public int Partition
        {
            get
            {
                return _partition;
            }
        }

        /// <summary>
        /// 位置
        /// </summary>
        private long _offest;
        /// <summary>
        /// 分区
        /// </summary>
        public long Offest
        {
            get
            {
                return _offest;
            }
        }
        
    }
}
