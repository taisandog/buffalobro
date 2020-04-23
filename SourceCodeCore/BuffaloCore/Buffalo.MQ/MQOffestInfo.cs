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
        /// <param name="partition">分区</param>
        /// <param name="offset">位置</param>
        public MQOffestInfo(string key, int partition, long offset)
        {
            _key = key;
            _partition = partition;
            _offest = offset;
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
