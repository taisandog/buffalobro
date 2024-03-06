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
        private TimingCircle<ClientSocketBase> _resendTime;

        /// <summary>
        /// 心跳时间集合
        /// </summary>
        private TimingCircle<ClientSocketBase> _heartTime;
        /// <summary>
        /// 过期时间集合
        /// </summary>
        private TimingCircle<ClientSocketBase> _expiredTime;

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
        /// <param name="timeResend"></param>
        /// <param name="timeHeart"></param>
        /// <param name="timeOut"></param>
        /// <param name="scale"></param>
        public TimelineManager(int timeResend, int timeHeart, int timeOut, int scale)
        {
            _scale = scale;
            _clients = new ConcurrentDictionary<ClientSocketBase, TimeIndexColl>();

            if (timeResend > 0)
            {
                _resendTime = new TimingCircle<ClientSocketBase>(timeResend, scale);
            }
            if (timeHeart > 0)
            {
                _heartTime = new TimingCircle<ClientSocketBase>(timeHeart, scale);
            }
            if (timeOut > 0)
            {
                _expiredTime = new TimingCircle<ClientSocketBase>(timeOut, scale);
            }

        }
        /// <summary>
        /// 重置时间
        /// </summary>
        /// <param name="time"></param>
        public void Reset(long time)
        {
            if (_resendTime != null)
            {
                _resendTime.Reset(time);
            }
            if (_heartTime != null)
            {
                _heartTime.Reset(time);
            }
            if (_expiredTime != null)
            {
                _expiredTime.Reset(time);
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
            if (_resendTime != null)
            {
                indexColl.ResendTimeIndex = _resendTime.AddValue(socket);
            }

            if (_heartTime != null)
            {
                indexColl.HeartTimeIndex = _heartTime.AddValue(socket);
            }

            if (_expiredTime != null)
            {
                indexColl.ExpiredTime = _expiredTime.AddValue(socket);
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

            if (_resendTime != null && indexColl.ResendTimeIndex > 0)
            {
                _resendTime.DeleteValue(socket, indexColl.ResendTimeIndex);
            }

            if (_heartTime != null && indexColl.HeartTimeIndex > 0)
            {
                _heartTime.DeleteValue(socket, indexColl.HeartTimeIndex);
            }

            if (_expiredTime != null && indexColl.ExpiredTime > 0)
            {
                _expiredTime.DeleteValue(socket, indexColl.ExpiredTime);
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
        /// 重发时间线移动到此时间
        /// </summary>
        /// <param name="curTime">时间</param>
        /// <param name="queItems">填充列表</param>
        public void MoveToTimeResendTime(long curTime,  Queue<ConcurrentDictionary<ClientSocketBase, bool>> queItems)
        {
            MoveToTime(curTime,_resendTime,queItems);
        }

        /// <summary>
        /// 心跳时间线移动到此时间
        /// </summary>
        /// <param name="curTime">时间</param>
        /// <param name="queItems">填充列表</param>
        public void MoveToTimeHeartTime(long curTime, Queue<ConcurrentDictionary<ClientSocketBase, bool>> queItems)
        {
            MoveToTime(curTime, _heartTime, queItems);
        }
        /// <summary>
        /// 过期时间线移动到此时间
        /// </summary>
        /// <param name="curTime">时间</param>
        /// <param name="queItems">填充列表</param>
        public void MoveToTimeExpiredTime(long curTime, Queue<ConcurrentDictionary<ClientSocketBase, bool>> queItems)
        {
            MoveToTime(curTime, _expiredTime, queItems);
        }
        public void Clear()
        {
            if (_clients != null)
            {
                _clients.Clear();
            }
            _clients = null;

            if (_resendTime != null)
            {
                _resendTime.Clear();
            }
            _resendTime = null;

            if (_heartTime != null)
            {
                _heartTime.Clear();
            }
            _heartTime = null;

            if (_expiredTime != null)
            {
                _expiredTime.Clear();
            }
            _expiredTime = null;
        }

        public void Dispose()
        {
            if (_clients != null)
            {
                _clients.Clear();
            }
            _clients = null;

            if (_resendTime != null)
            {
                _resendTime.Dispose();
            }
            _resendTime = null;

            if (_heartTime != null)
            {
                _heartTime.Dispose();
            }
            _heartTime = null;

            if (_expiredTime != null)
            {
                _expiredTime.Dispose();
            }
            _expiredTime = null;
        }
    }
}
