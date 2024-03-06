using Buffalo.IOCP.DataProtocol;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Buffalo.IOCP.DataProtocol
{
    public abstract partial class ClientSocketBase
    {
        private static long _releaseCount = 0;
        ~ClientSocketBase()
        {
            Close();
            //SocketCount--;

#if DEBUG
            long curCount = 0;
            lock (autoLock)
            {
                _releaseCount++;
                curCount = _releaseCount;
            }

            if (_netProtocol.ShowLog)
            {
                string str = String.Format("ClientSocket Release, id ={0}, count={1}", _id, curCount);
                _netProtocol.Log(str);
            }
#endif
        }

        
        /// <summary>
        /// 连接的键
        /// </summary>
        public string SocketKey { get; set; }
        ///// <summary>
        ///// 创建包
        ///// </summary>
        ///// <param name="hasResetEvent">是否有阻塞函数</param>
        ///// <returns></returns>
        //public DataPacketBase CreatePacket(bool hasResetEvent) 
        //{
        //    DataPacket packet = new DataPacket(_sendPakcetID++, hasResetEvent,_netProtocol);
        //    return packet;
        //}

        

        /// <summary>
        /// 附加信息
        /// </summary>
        public object Tag { get; set; }

    }
}
