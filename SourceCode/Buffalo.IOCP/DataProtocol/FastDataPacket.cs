using Buffalo.IOCP.DataProtocol;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Buffalo.IOCP.DataProtocol
{
    public class FastDataPacket : DataPacketBase
    {
        public object EmptyPacketId
        {
            get
            {
                return 0;
            }
        }

        public int PacketIDValue
        {
            get 
            {
                return GetPacketIdIntValue(PacketID);
            }
        }

        public static int GetPacketIdIntValue(string packId) 
        {
            int ret = 0;
            if (int.TryParse(packId, out ret))
            {
                return ret;
            }
            return 0;
        }

        /// <summary>
        /// 构造，生成一个数据包，用来发送
        /// </summary>
        /// <param name="packetID">包编号</param>
        /// <param name="lost">是否验证丢失</param>
        /// <param name="verify">是否验证数据体</param>
        /// <param name="data">数据体</param>
        public FastDataPacket(int packetID,  byte[] data,bool isLost,   INetProtocol netProtocol)
        {
            _netProtocol = netProtocol;
            if (_netProtocol == null)
            {
                throw new NullReferenceException("netProtocol can't be null");
            }
            if (packetID > 0)
            {
                this.PacketID = packetID.ToString();
            }
            this.IsLost = isLost;
            this.ResendCount = _netProtocol.DefaultMaxResend;
            //this.IsVerify = verify;


            if (data != null)
            {
                this.Data = data;
                this.Length = data.Length + _netProtocol.PACKET_LENGHT;
                
            }
            else //空包
            {
                this.Data = null;
                this.Length = _netProtocol.PACKET_LENGHT;
                
            }
            
            //if (hasResetEvent)
            //{
            //    _event = new ManualResetEvent(false);
            //    ResetEvent();
            //}
        }


       
    }
}
