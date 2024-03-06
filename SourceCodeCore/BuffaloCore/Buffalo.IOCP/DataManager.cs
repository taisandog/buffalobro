using Buffalo.Kernel.Collections;
using Buffalo.IOCP.DataProtocol;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Sockets;

namespace Buffalo.IOCP
{
    /// <summary>
    /// 数据管理器
    /// </summary>
    public class DataManager
    {
        #region 属性
        /// <summary>
        /// 要发送的数据包
        /// </summary>
        private ConcurrentQueue<DataPacketBase> _sendPacket;
        /// <summary>
        /// 要求回应的包
        /// </summary>
        private LinkedDictionary<object, DataPacketBase> _lostPacket;
        /// <summary>
        /// 已接受过的ID(去重)
        /// </summary>
        private LinkedDictionary<object, bool> _receiverID;

        /// <summary>
        /// 最大发消息的缓存池
        /// </summary>
        private int _maxMessagePool = 20;
        /// <summary>
        /// 最大发消息的缓存池
        /// </summary>
        public int MaxMessagePool
        {
            get { return _maxMessagePool; }
            set { _maxMessagePool = value; }
        }
        /// <summary>
        /// 最大发消息的缓存池
        /// </summary>
        private int _maxLostMessagePool = 20;
        /// <summary>
        /// 最大验证丢失消息的缓存池
        /// </summary>
        public int MaxLostMessagePool
        {
            get { return _maxLostMessagePool; }
            set { _maxLostMessagePool = value; }
        }

        private INetProtocol _netProtocol = null;
        #endregion

        #region 方法
        /// <summary>
        /// 数据管理
        /// </summary>
        /// <param name="maxSendPool">最大发送消息缓冲池</param>
        /// <param name="maxLostMessagePool">最大验证丢失消息的缓冲池</param>
        public DataManager(int maxSendPool,int maxLostMessagePool, INetProtocol netProtocol)
        {
            _sendPacket = new ConcurrentQueue<DataPacketBase>();
            _lostPacket = new LinkedDictionary<object, DataPacketBase>();
            _receiverID = new LinkedDictionary<object, bool>();
            _netProtocol = netProtocol;
            if (_netProtocol == null)
            {
                throw new NullReferenceException("netProtocol can't be null");
            }
            _maxMessagePool = maxSendPool;
            if (_maxMessagePool < 5)
            {
                _maxMessagePool = 5;
            }

            _maxLostMessagePool = maxLostMessagePool;
            if (_maxLostMessagePool < 5)
            {
                _maxLostMessagePool = 5;
            }
        }
       
        /// <summary>
        /// 发送列表是否已满
        /// </summary>
        public bool IsSendPacketFull
        {
            get
            {
                ConcurrentQueue<DataPacketBase> queSend = _sendPacket;
                bool ret = true;
                if (queSend != null)
                {
                    ret = queSend.Count >= _maxMessagePool;
                }
                queSend = null;
                return ret;
            }
        }
        /// <summary>
        /// 重发列表是否已满
        /// </summary>
        public bool IsLostPacketFull
        {
            get
            {
                LinkedDictionary<object, DataPacketBase> diclp = _lostPacket;
                bool ret = true;
                if (diclp != null)
                {
                    ret = diclp.Count >= _maxLostMessagePool;
                }
                diclp = null;
                return ret;
            }
        }
        public void Close()
        {
            ConcurrentQueue<DataPacketBase> queSend = _sendPacket;
            _sendPacket = null;
            DataPacketBase dp = null;
            if (queSend != null)
            {
                while (queSend.Count > 0)
                {
                    queSend.TryDequeue(out dp);
                }
            }
            dp = null;
            queSend = null;
            LinkedDictionary<object, DataPacketBase> diclp = _lostPacket;
            _lostPacket = null;
            if (diclp != null)
            {
                lock (diclp)
                {
                    diclp.Clear();
                }
            }
            diclp = null;
            LinkedDictionary<object, bool> dicId = _receiverID;
            _receiverID = null;
            if (dicId != null)
            {
                lock (dicId)
                {
                    dicId.Clear();
                }
            }
            dicId = null;
        }
        /// <summary>
        /// 裁剪发送数据
        /// </summary>
        /// <param name="maxPacket"></param>
        private void TrimSendData(ConcurrentQueue<DataPacketBase> queSend)
        {
            DataPacketBase dp = null;

            
            if (queSend == null)
            {
                return;
            }
            while (queSend.Count >= _maxMessagePool)
            {
                if(!queSend.TryDequeue(out dp))
                {
                    break;
                }
                try
                {
                    dp.Dispose();
                }
                catch { }
            }
            queSend = null;
        }
        /// <summary>
        /// 添加要发送的数据
        /// </summary>
        /// <param name="dataPacket">数据</param>
        public string AddData(DataPacketBase dataPacket)
        {
            ConcurrentQueue<DataPacketBase> queSend = _sendPacket;
            if (queSend == null)
            {
                return "Connect is Close";
            }
            object mergeTag = dataPacket.MergeTag;
            string packetId = dataPacket.PacketID;
            if (mergeTag!=null || !_netProtocol.IsEmptyPacketId(packetId))
            {
                if (CheckExists(queSend, mergeTag, packetId))
                {
                    return "merge same Tag";
                }
            }
            queSend.Enqueue(dataPacket);
            TrimSendData(queSend);
            queSend = null;
            return null;
        }
        /// <summary>
        /// 检测是否重复包
        /// </summary>
        /// <param name="dataPacket"></param>
        /// <returns></returns>
        private bool CheckExists(ConcurrentQueue<DataPacketBase> queSend, object mergeTag, string packetId)
        {
            Type mergeTagType = null;
            if (mergeTag == null)
            {
                return false;
            }
            mergeTagType = mergeTag.GetType();

            object curMrgeTag = null;
            foreach (DataPacketBase dp in queSend)
            {
                if (string.Equals(packetId , dp.PacketID))
                {
                    return true;
                }
                
                curMrgeTag = dp.MergeTag;
                if (curMrgeTag == null)
                {
                    continue;
                }
                
                try
                {
                    if (mergeTag.Equals(curMrgeTag))
                    {
                        return true;
                    }
                }
                catch { }
            }
            return false;
        }

        /// <summary>
        /// 添加收到的回应包
        /// </summary>
        /// <param name="dataPacket"></param>
        public void AddReceive(DataPacketBase dataPacket)
        {

            DataPacketBase dp = null;
            LinkedDictionary<object, DataPacketBase> diclp = _lostPacket;
            if (diclp != null )
            {
                lock (diclp)
                {

                    dp = diclp.RemoveKey(dataPacket.PacketID);

                    if (dp != null) 
                    {
                        if (dataPacket.NetProtocol.ShowError)
                        {
                            string str=String.Format("Receive Packet,ID:{0},", dp.PacketID);
                            dataPacket.NetProtocol.Log(str);
                        }
                    }

                }
               
            }

        }
        /// <summary>
        /// 将超时的数据添加到要以送的列表
        /// </summary>
        /// <param name="timeHeart"></param>
        public void CheckResend(int timeHeart)
        {
            LinkedDictionary<object, DataPacketBase> diclp = _lostPacket;
            ConcurrentQueue<DataPacketBase> queSend = _sendPacket;

            if (diclp == null)
            {
                return;
            }
            if (queSend == null)
            {
                return;
            }
            if (diclp.Count <= 0)
            {
                return;
            }
            DateTime dt = DateTime.Now;
            DataPacketBase dp = null;
            List<object> lstRemoveLost = new List<object>();
            lock (diclp)
            {
                foreach (KeyValuePair<object, DataPacketBase> kvp in diclp)
                {
                    dp = kvp.Value;
                    if (dt.Subtract(dp.SendTime).TotalMilliseconds > timeHeart)
                    {
                        if (!IsSendPacketFull)
                        {
                            queSend.Enqueue(dp);
                        }
                        lstRemoveLost.Add(kvp.Key);
                        //_lostPacket.RemoveAt(i);
                    }
                }
                foreach (object packId in lstRemoveLost)
                {
                    diclp.Remove(packId);
                }
                queSend = null;
                lstRemoveLost = null;
            }
        }

        /// <summary>
        /// 获取要发送的数据，需要验证重发，如果返回null,就是没数据发
        /// </summary>
        /// <returns></returns>
        public DataPacketBase GetData()
        {

            DataPacketBase dataPacket = null;
            ConcurrentQueue<DataPacketBase> queSend = _sendPacket;
            if (queSend!=null)
            {
                
                if(!queSend.TryDequeue(out dataPacket))
                {
                    return null;
                }

            }
            if (dataPacket.IsLost)
            {//如果是要回应的要就放到回应的队列
                LinkedDictionary<object, DataPacketBase> diclp = _lostPacket;
                if (diclp != null && !IsLostPacketFull)
                {
                    dataPacket.SendTime = DateTime.Now;
                    lock (diclp)
                    {
                        diclp[dataPacket.PacketID] = dataPacket;
                        TrimLostPacket(diclp);
                    }
                }
                
            }
            return dataPacket;
        }
        private DateTime _dtLastCheckLostPacket = DateTime.MinValue;

        /// <summary>
        /// 裁剪需要检测丢包的集合
        /// </summary>
        private void TrimLostPacket(LinkedDictionary<object, DataPacketBase> diclp)
        {
            if (!IsLostPacketFull)
            {
                return;
            }
            DateTime dt = DateTime.Now;
            if (dt.Subtract(_dtLastCheckLostPacket).TotalSeconds < 10)
            {
                return;
            }

            diclp.TrimCount(_maxLostMessagePool);

            _dtLastCheckLostPacket = dt;
        }

       

        /// <summary>
        /// ID是否收到过
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool IsReceive(object id)
        {
            LinkedDictionary<object, bool> dicId = _receiverID;
            if (dicId == null)
            {
                return false;
            }
            lock (dicId)
            {
                
                if (dicId.ContainsKey(id)) 
                {
                    return true;
                }
                dicId[id] = true;
                if (dicId.Count > 500)
                {
                    dicId.TrimCount(450);
                }
                return false;
            }
            
        }
        #endregion
    }
}
