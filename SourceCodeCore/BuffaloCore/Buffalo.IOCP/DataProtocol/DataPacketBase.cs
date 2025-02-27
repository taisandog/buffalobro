
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Buffalo.IOCP.DataProtocol
{
    public class DataPacketBase : IDisposable
    {
        
        /// <summary>
        /// 合并标签(用Object.Equals比较相等则合并)
        /// </summary>
        protected object _mergeTag;
        /// <summary>
        /// 合并标签(用Object.Equals比较相等则合并)
        /// </summary>
        public object MergeTag
        {
            set
            {
                _mergeTag = value;
            }
            get
            {
                return _mergeTag;
            }
        }
        private bool _isHeart=false;
        /// <summary>
        /// 是否心跳包
        /// </summary>
        public bool IsHeart
        {
            set 
            {
                _isHeart = value;
            }
            get 
            {
                return _isHeart;
            }
        }
        /// <summary>
        /// 是否验证丢失
        /// </summary>
        public bool IsLost
        {
            protected set;
            get;
        }
        private int _maxResend = 0;
        /// <summary>
        /// 最大重发次数(0为无限重发)
        /// </summary>
        public int MaxResend
        {
            set
            {
                _maxResend = value;
            }
            get
            {
                return _maxResend;
            }
        }

        internal int _resendCount = 0;
        /// <summary>
        /// 当前重发计数
        /// </summary>
        public int ResendCount
        {
            
            get
            {
                return _resendCount;
            }
        }
        /// <summary>
        /// 发送时间,用来判断是否重发
        /// </summary>
        public DateTime SendTime
        {
            set;
            get;
        }
        protected INetProtocol _netProtocol;
        /// <summary>
        /// 协议
        /// </summary>
        public INetProtocol NetProtocol
        {
            get
            {
                return _netProtocol;
            }
        }
        

        private bool _isRaw=false;
        /// <summary>
        /// 是否原始发送的数据
        /// </summary>
        public bool IsRaw
        {
            get { return _isRaw; }
            set { _isRaw = value; }
        }

        /// <summary>
        /// 数据包长度
        /// </summary>
        public int Length
        {
            protected set;
            get;
        }

        
        /// <summary>
        /// 数据体
        /// </summary>
        public byte[] Data
        {
            set;
            get;
        }


        /// <summary>
        /// 数据包编号
        /// </summary>
        public string PacketID
        {
            set;
            get;
        }

        
        public virtual void Dispose()
        {
            lock (this)
            {
                _mergeTag = null;
                Data = null;
            }
        }
        
        ~DataPacketBase()
        {
            Dispose();
        }
    }
}
