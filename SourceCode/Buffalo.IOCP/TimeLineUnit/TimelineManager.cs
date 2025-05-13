using GameBoxIOCP.Sources.TimeLineUnit;
using Buffalo.IOCP.DataProtocol;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buffalo.Kernel.Collections
{
    public class TimelineManager:IDisposable
    {
        /// <summary>
        /// 所有客户端
        /// </summary>
        private ConcurrentDictionary<ClientSocketBase, TimeIndexColl> _clients = null;
        /// <summary>
        /// 所有客户端
        /// </summary>
        public ConcurrentDictionary<ClientSocketBase, TimeIndexColl> Clients
        {
            get { return _clients; }
        }

        /// <summary>
        /// 重发时间集合
        /// </summary>
        private TimingCircle<ClientSocketBase> _checkTime;

       

        private int _scale;
        /// <summary>
        /// 刻度
        /// </summary>
        public int Scale 
        {
            get { return _scale; }
        }

        /// <summary>
        /// 时间类管理
        /// </summary>
        /// <param name="time">一轮总时间</param>
        /// <param name="scale">刻度</param>
        public TimelineManager(int time, int scale)
        {
            _scale = scale;
            _clients = new ConcurrentDictionary<ClientSocketBase, TimeIndexColl>();

            _checkTime = new TimingCircle<ClientSocketBase>(time, scale);



        }
        /// <summary>
        /// 重置时间
        /// </summary>
        /// <param name="time"></param>
        public void Reset(long time)
        {
            if (_checkTime != null)
            {
                _checkTime.Reset(time);
            }
           
        }

        /// <summary>
        /// 添加客户端
        /// </summary>
        /// <param name="socket"></param>
        /// <returns></returns>
        public bool AddClient(ClientSocketBase socket)
        {
            TimeIndexColl indexColl = new TimeIndexColl();
            if (!_clients.TryAdd(socket, indexColl))
            {
                return false;
            }
            indexColl.Socket = socket;
            if (_checkTime != null)
            {
                indexColl.TimeIndex = _checkTime.AddValue(socket);
            }

            
            return true;
        }
        /// <summary>
        /// 移除客户端
        /// </summary>
        /// <param name="socket">连接</param>
        /// <param name="forceDelete">需要从此集合强行删除</param>
        /// <returns></returns>
        public bool RemoveSocket(ClientSocketBase socket, ConcurrentDictionary<ClientSocketBase, bool> forceDelete = null)
        {
            if (forceDelete != null)
            {
                forceDelete.TryRemove(socket, out _);
            }

            TimeIndexColl indexColl = null;
            if (!_clients.TryRemove(socket, out indexColl))
            {
                return false;
            }

            if (_checkTime != null && indexColl.TimeIndex > 0)
            {
                _checkTime.DeleteValue(socket, indexColl.TimeIndex);
            }
            return true;
        }

        /// <summary>
        /// 移动到此时间
        /// </summary>
        /// <param name="curTime">时间</param>
        /// <param name="queItems">填充列表</param>
        private void MoveToTime(long curTime, TimingCircle<ClientSocketBase> timeCircle, Queue<ConcurrentDictionary<ClientSocketBase, bool>> queItems)
        {
            if(timeCircle == null) 
            {
                return;
            }
            timeCircle.MoveToTime(curTime, queItems);
        }
       
        /// <summary>
        /// 过期时间线移动到此时间
        /// </summary>
        /// <param name="curTime">时间</param>
        /// <param name="queItems">填充列表</param>
        public void MoveToTimeCheckTime(long curTime, Queue<ConcurrentDictionary<ClientSocketBase, bool>> queItems)
        {
            MoveToTime(curTime, _checkTime, queItems);
        }
        public void Clear()
        {
            if (_clients != null)
            {
                _clients.Clear();
            }
            _clients = null;

            if (_checkTime != null)
            {
                _checkTime.Clear();
            }
            _checkTime = null;

            
        }

        public void Dispose()
        {
            Clear();
        }
        ~TimelineManager() 
        {
            Clear();
        }
    }
}
