using Buffalo.Kernel;
using Buffalo.Kernel.Collections;
using Buffalo.IOCP.DataProtocol;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Timers;

namespace Buffalo.IOCP
{

    /// <summary>
    /// 心跳,超时,数据重发管理
    /// </summary>
    public class HeartManager
    {

        private int _timeOut;
        /// <summary>
        /// 超时时间
        /// </summary>
        public int TimeOut
        {

            get 
            {
                return _timeOut;
            }
        }

        private int _timeHeart;
        /// <summary>
        /// 心跳当前值
        /// </summary>
        public int TimeHeart
        {

            get 
            {
                return _timeHeart;
            }
        }
        private int _timeResend;
        /// <summary>
        /// 数据重发间隔
        /// </summary>
        public int TimeResend
        {

            get 
            {
                return _timeResend;
            }
        }
        private int _timeHeartSource;
        /// <summary>
        /// 心跳原始值
        /// </summary>
        public int TimeHeartSource
        {
            get 
            {
                return _timeHeartSource;
            }
        }
        /// <summary>
        /// 消息类
        /// </summary>
        private IConnectMessage _message;
        /// <summary>
        /// 消息
        /// </summary>
        public IConnectMessage Message
        {
            get
            {
                return _message;
            }
        }
        private AutoResetEvent _threadHandle;
        private TimelineManager _clients = null;
        public TimelineManager Clients
        {
            get
            {
                return _clients;
            }
        }
        private bool _needSendheart = true;
        /// <summary>
        /// 是否需要主动发心跳
        /// </summary>
        public bool NeedSendheart
        {
            get { return _needSendheart; }
            set { _needSendheart = value; }
        }
        /// <summary>
        /// 客户端数
        /// </summary>
        //public List<ClientSocket> Clients;
       

        /// <summary>
        /// 检查线程
        /// </summary>
        private Thread CheckThread;

        private object CheckThreadLok = new object();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="timeOut">超时时间</param>
        /// <param name="timeHreat">心跳时间</param>
        /// <param name="timeResend">重发间隔</param>
        /// <param name="timeCheckDisconnect">检查空连接间隔(如果前边三个值都为0时候才生效)</param>
        /// <param name="message">日志输出器</param>
        public HeartManager(int timeOut, int timeHeart, int timeResend,int timeCheckDisconnect, IConnectMessage message)
        {
            //_clients = new ConcurrentDictionary<ClientSocket, bool>();

            _timeResend = timeResend;
            _timeOut = timeOut;
            _timeHeart = timeHeart;
            _timeHeartSource = timeHeart;
            if(_timeHeart==0 && _timeResend==0&& _timeOut == 0) 
            {
                _timeHeart = timeCheckDisconnect;
            }
            _message = message;
            
        }

        /// <summary>
        /// 连接是否存在
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public bool ExistsClient(ClientSocketBase client)
        {
            if (!_running)
            {
                return false;
            }
            return _clients.Clients.ContainsKey(client);
        }

        /// <summary>
        /// 添加连接
        /// </summary>
        /// <param name="client"></param>
        public bool AddClient(ClientSocketBase client)
        {
            if (!_running)
            {
                return false;
            }
            CheckRunning();
            bool ret=_clients.AddClient(client);
            
            return ret;
        }
        /// <summary>
        /// 检测线程是否运行
        /// </summary>
        public void CheckRunning()
        {
            if (!_running)
            {
                return;
            }
            lock (CheckThreadLok)
            {
                if (CheckThread == null || !CheckThread.IsAlive)
                {
                    if (Util.HasShowWarning(_message))
                    {
                        _message.LogWarning("Start Heart");
                    }
                    StartHeart();
                }
            }
        }


        /// <summary>
        /// 开始心跳管理
        /// </summary>
        /// <param name="totalCircleTime">时间轮的一圈时间</param>
        /// <param name="scale">刻度</param>
        public void StartHeart(int totalCircleTime, int scale)
        {
            StopHeart();
            if (totalCircleTime > 0 && scale > 0)
            {
                _clients = CreateTimelineManager(totalCircleTime, scale);
            }
            else 
            {
                _clients = CreateTimelineManager();
            }
            _threadHandle = new AutoResetEvent(false);
            _running = true;
            CheckThread = new Thread(new ThreadStart(DoRun));

            CheckThread.Start();

        }

        /// <summary>
        /// 开始心跳管理
        /// </summary>
        public void StartHeart()
        {
            StartHeart(0,0);
        }
        private bool _running = false;
        /// <summary>
        /// 运行中
        /// </summary>
        public bool Running 
        {
            get { return _running; }
        }

        public int Count 
        {
            get 
            {
                if (!_running)
                {
                    return 0;
                }
                return _clients.Clients.Count;
            }
        }
        /// <summary>
        /// 关闭心跳管理
        /// </summary>
        public void StopHeart()
        {
            _running = false;
            
            try
            {
                if (CheckThread!=null )
                {
                    if(!StopCheckThreadSuccess()) 
                    {
                        try
                        {
                            CheckThread.Abort();
                            Thread.Sleep(100);
                        }
                        catch { }
                    }
                }
                CheckThread = null;
                _threadHandle = null;
                if (_clients != null) 
                {
                    _clients.Dispose();
                }
                _clients = null;
            }
            catch (Exception ex)
            {
                //if (_message.ShowWarning)
                //{
                //    _message.LogWarning("关闭心跳管理EX:" + ex);
                //}
            }


        }

        private bool StopCheckThreadSuccess()
        {
            if (_threadHandle == null)
            {
                return false;
            }
            return _threadHandle.WaitOne(3000);
        }

        /// <summary>
        /// 创建时间线
        /// </summary>
        /// <returns></returns>
        private TimelineManager CreateTimelineManager() 
        {
            int scale = 0;

            int minNumber = _timeResend;
            if(minNumber <= 0|| minNumber > _timeHeart) 
            {
                minNumber = _timeHeart;
            }
            if (minNumber <= 0 || minNumber > _timeOut)
            {
                minNumber = _timeOut;
            }

            minNumber = minNumber / 2;

            scale = minNumber / 10;
            


            if (scale > 200)
            {
                scale = 200;
            }
            if (scale < 10)
            {
                scale = 10;
            }
            return CreateTimelineManager(minNumber, scale);
        }
        /// <summary>
        /// 创建时间线
        /// </summary>
        /// <param name="time">时间轮一圈的时间</param>
        /// <param name="scale">刻度</param>
        /// <returns></returns>
        private TimelineManager CreateTimelineManager(int totalTime,int scale) 
        {
            TimelineManager time = new TimelineManager(totalTime, scale);
            long nowtime = (long)CommonMethods.ConvertDateTimeInt(DateTime.Now, false, true);
            time.Reset(nowtime);
            return time;
        }

        /// <summary>
        /// 删除心跳管理的连接
        /// </summary>
        /// <param name="socket"></param>
        public bool RemoveSocket(ClientSocketBase socket)
        {
            if (!_running)
            {
                return true;
            }
            return _clients.RemoveSocket(socket);
        }
        private void DoRun()
        {
            int sleep = _clients.Scale;
            try
            {
                
                _threadHandle.Reset();

                DateTime nowDate = DateTime.Now;
                long time = (long)CommonMethods.ConvertDateTimeInt(nowDate, false, true);
                _clients.Reset(time);
                
                while (_running)
                {
                    try
                    {
                        nowDate = DateTime.Now;
                       

                        CheckTime(nowDate);
                    }
                    catch (Exception ex)
                    {
                        if (Util.HasShowError(_message))
                        {
                            _message.LogError(ex.ToString());
                        }
                    }
                    finally 
                    {
                        Thread.Sleep(sleep);
                    }
                }

               

            }
            finally
            {
                _threadHandle.Set();
            }
            CheckThread = null;
        }


        /// <summary>
        /// 检查过期
        /// </summary>
        /// <param name="nowDate">当前时间</param>
        private void CheckTime( DateTime nowDate)
        {
            long time = (long)CommonMethods.ConvertDateTimeInt(nowDate, false, true);
            Queue<ClientSocketBase> lstRemove = new Queue<ClientSocketBase>();
            Queue<ClientSocketBase> lstClose = new Queue<ClientSocketBase>();
            Queue<ConcurrentDictionary<ClientSocketBase, bool>> queItems = new Queue<ConcurrentDictionary<ClientSocketBase, bool>>();
            _clients.MoveToTimeCheckTime(time, queItems);

            ConcurrentDictionary<ClientSocketBase, bool> dic = null;
            ClientSocketBase connection = null;
            while (queItems.Count > 0)
            {
                dic = queItems.Dequeue();
                foreach (KeyValuePair<ClientSocketBase, bool> kvp in dic)
                {
                    connection = kvp.Key;
                    if (!connection.Connected)//空连接
                    {
                        lstRemove.Enqueue(connection);
                        continue;
                    }
                    if (_timeOut > 0 && nowDate.Subtract(connection.LastReceiveTime).TotalMilliseconds >= _timeOut)//检查过期
                    {
                        lstClose.Enqueue(connection);
                    }
                    if (_needSendheart && _timeHeartSource > 0 && nowDate.Subtract(connection.LastSendTime).TotalMilliseconds >= _timeHeartSource)//检查心跳发送
                    {
                        try
                        {
                            connection.SendHeard();
                        }
                        catch { }
                    }
                    if (_timeResend > 0 )//检查重发
                    {
                        try
                        {
                            connection.DataManager.CheckResend(_timeResend, nowDate);
                        }
                        catch(Exception ex)
                        {

                         }
                    }
                }
                RemoveEmpty(lstRemove, dic);
                CloseConnection(lstClose, dic);
            }
            
            lstRemove=null;
            lstClose = null;
            queItems = null;
        }

        /// <summary>
        /// 删除空连接
        /// </summary>
        /// <param name="lstRemove"></param>
        /// <param name="dic"></param>
        private void RemoveEmpty(Queue<ClientSocketBase> lstRemove, ConcurrentDictionary<ClientSocketBase, bool> dic)
        {
            ClientSocketBase connection = null;
            int count = lstRemove.Count;
            while (lstRemove.Count > 0)
            {
                connection = lstRemove.Dequeue();
                connection.HandleClose("Destroy invalid connection:" + connection.HostIP);//通知断开
                _clients.RemoveSocket(connection, dic);
            }
            if (Util.HasShowWarning(_message) && count > 0)
            {
                if (Util.HasShowWarning(_message))
                {
                    _message.LogWarning("Clear destroyed connection:" + count);
                }
            }
        }
        /// <summary>
        /// 删除空连接
        /// </summary>
        /// <param name="lstRemove"></param>
        /// <param name="dic"></param>
        private void CloseConnection(Queue<ClientSocketBase> lstClose, ConcurrentDictionary<ClientSocketBase, bool> dic)
        {
            ClientSocketBase connection = null;
            while (lstClose.Count > 0)
            {
                connection = lstClose.Dequeue();
                connection.HandleClose("Connection timedout:" + connection.HostIP);//通知断开
                connection.Close();
                _clients.RemoveSocket(connection, dic);

                if (Util.HasShowWarning(_message))
                {
                    string key = connection.SocketKey;
                    int tid = 0;
                    if (!string.IsNullOrWhiteSpace(key))
                    {
                        try
                        {
                            tid = Convert.ToInt32(connection.SocketKey, 16);
                        }
                        catch { }
                    }
                    if (Util.HasShowWarning(_message))
                    {
                        StringBuilder sb = new StringBuilder(50);
                        sb.Append("Connection timedout:ID:");
                        sb.Append(tid);
                        sb.Append(",IP:");
                        sb.Append(connection.HostIP);
                        _message.LogWarning(sb.ToString());
                    }
                }
            }
        }
    }


}
