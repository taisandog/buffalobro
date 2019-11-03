using System;
using System.Threading;
using Library;

namespace Buffalo.Kernel.AutoServicesLib
{
    /// <summary>
    /// 服务基类
    /// </summary>
    public abstract class AbsServicesHandle
    {
        /// <summary>
        /// 服务名
        /// </summary>
        public abstract string ServicesName
        {
            get;
        }
        /// <summary>
        /// 服务标识
        /// </summary>
        public abstract string ServicesID
        {
            get;
        }
        /// <summary>
        /// 版本
        /// </summary>
        public abstract Version SerVersion
        {
            get;
        }
        /// <summary>
        /// 排序(越大越先执行，默认是0)
        /// </summary>
        public virtual int Order
        {
            get
            {
                return 0;
            }
        }
        /// <summary>
        /// 间隔时间(毫秒)
        /// </summary>
        public abstract long Interval
        {
            get;

        }
        private DateTime _lasRun;
        /// <summary>
        /// 上次执行时间
        /// </summary>
        public DateTime LasRun
        {
            get { return _lasRun; }
        }
        /// <summary>
        /// 超时(毫秒)
        /// </summary>
        private int _timeout = 10 * 60 * 1000;
        /// <summary>
        /// 超时(毫秒)
        /// </summary>
        public virtual int Timeout
        {
            get { return _timeout; }
        }
        /// <summary>
        /// 是否异步执行(此任务用新线程执行，不堵塞任务主线程,默认:false)
        /// </summary>
        public virtual bool IsAsync
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// 是否已经到时间
        /// </summary>
        public bool Tick
        {
            get
            {
                TimeSpan ts = DateTime.Now.Subtract(_lasRun);
                if (ts.TotalMilliseconds >= Interval)
                {
                    return true;
                }
                return false;
            }
        }

        public IShowMessage _messageShow;
        /// <summary>
        /// 日志输出
        /// </summary>
        public IShowMessage MessageShow
        {
            get { return _messageShow; }
            set { _messageShow = value; }
        }

        private AutoResetEvent _threadlock = new AutoResetEvent(false);
        Thread _anycThread = null;
        /// <summary>
        /// 检查是否要运行(每隔一段时间执行)
        /// </summary>
        /// <returns></returns>
        public ServicesMessage CheckRun()
        {
            ServicesMessage ret = null;
            DateTime curDate = DateTime.Now;
            if (Tick)
            {
                ret = new ServicesMessage(this, curDate, _messageShow);
                try
                {
                    if (IsAsync)
                    {
                        if (!AnycRunning)
                        {
                            AsyncTick(ret);
                        }
                    }
                    else
                    {
                        SynchronizeTick(ret);
                    }
                }
                catch (Exception ex)
                {
                    if (_messageShow != null && _messageShow.ShowError)
                    {
                        _messageShow.LogError(ex.ToString());
                    }
                }
                finally
                {
                    _lasRun = DateTime.Now;
                }
            }
            return ret;

        }

        /// <summary>
        /// 异步执行
        /// </summary>
        private void AsyncTick(ServicesMessage ret)
        {
            ClearAnyc();
            ParameterizedThreadStart parStart = new ParameterizedThreadStart(SynchronizeTick);
            _anycThread = new Thread(parStart);
            _anycThread.Start(ret);
        }

        /// <summary>
        /// 同步堵塞执行
        /// </summary>
        private void SynchronizeTick(object val)
        {
            ServicesMessage ret = val as ServicesMessage;
            
            ParameterizedThreadStart parStart = new ParameterizedThreadStart(DoThread);
            Thread myThread = new Thread(parStart);
            _threadlock.Reset();
            myThread.Start(ret);
            if (!_threadlock.WaitOne(Timeout))
            {
                if (_messageShow != null && _messageShow.ShowError)
                {
                    _messageShow.LogError(ServicesName + "执行超时,执行时间超过" + Timeout + "毫秒");
                    myThread.Abort();
                    Thread.Sleep(100);
                }
            }
            if (ret.Tag != null)
            {
                bool isSuccess = (bool)ret.Tag;
            }

            myThread = null;
        }

        /// <summary>
        /// 开始线程
        /// </summary>
        /// <param name="val"></param>
        private void DoThread(object val)
        {
            ServicesMessage message = val as ServicesMessage;
            if (message == null)
            {
                _threadlock.Set();
                return;
            }
            message.Tag = false;
            try
            {
                message.Tag=DoAction(message);
            }
            catch (Exception ex)
            {
                if (_messageShow != null && _messageShow.ShowError)
                {
                    _messageShow.LogError(ex.ToString());
                }
            }
            finally
            {
                _threadlock.Set();
                ClearAnyc();
            }
            
        }
        /// <summary>
        /// 是否异步执行中
        /// </summary>
        public bool AnycRunning
        {
            get
            {
                return _anycThread != null && _anycThread.IsAlive;

            }
        }
        /// <summary>
        /// 清除异步线程
        /// </summary>
        public void ClearAnyc()
        {
            if (_anycThread != null&& _anycThread.IsAlive)
            {
                try
                {
                    _anycThread.Abort();
                    Thread.Sleep(100);
                }
                catch { }
            }
            _anycThread = null;
        }

        /// <summary>
        /// 执行方法
        /// </summary>
        protected abstract bool DoAction(ServicesMessage message);
    }
}
