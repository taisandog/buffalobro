using Buffalo.IOCP.DataProtocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameBoxIOCP.Sources.TimeLineUnit
{
    /// <summary>
    /// 时间轴索引集合
    /// </summary>
    public class TimeIndexColl
    {
        /// <summary>
        /// 重发时间索引
        /// </summary>
        public int ResendTimeIndex;
        /// <summary>
        /// 心跳时间集合
        /// </summary>
        public int HeartTimeIndex;
        /// <summary>
        /// 过期时间集合
        /// </summary>
        public int ExpiredTime;
        /// <summary>
        /// 连接
        /// </summary>
        public ClientSocketBase Socket;
        /// <summary>
        /// 清空连接
        /// </summary>
        public void Clear() 
        {
            Socket = null;
        }
        ~TimeIndexColl() 
        {
            Clear();
        }
    }
}
