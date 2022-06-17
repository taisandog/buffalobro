using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Buffalo.Kernel.TreadPoolManager
{


    /// <summary>
    /// 带阻塞的线程
    /// </summary>
    public class BlockThread : IDisposable
    {
        /// <summary>
        /// 无参函数
        /// </summary>
        private ThreadStart _runMethod;

        private IBlockThreadMessage _messageHandle;

        public IBlockThreadMessage MessageHandle
        {
            get { return _messageHandle; }
            set { _messageHandle = value; }
            
        }
        /// <summary>
        /// 带参函数
        /// </summary>
        private ParameterizedThreadStart _runParamMethod;
        private BlockThread( ThreadStart method, ParameterizedThreadStart paramMethod, IBlockThreadMessage messageHandle=null) 
        {
            _thd = new Thread(new ParameterizedThreadStart(RunMethod));
            _runMethod = method;
            _runParamMethod = paramMethod;
            _messageHandle = messageHandle;
        }
        

        /// <summary>
        /// 创建线程信息
        /// </summary>
        /// <param name="thd"></param>
        /// <returns></returns>
        public static BlockThread Create(ThreadStart method, IBlockThreadMessage messageHandle = null) 
        {
            return new BlockThread(method,null, messageHandle);
        }
        /// <summary>
        /// 创建线程信息
        /// </summary>
        /// <param name="thd"></param>
        /// <returns></returns>
        public static BlockThread Create(ParameterizedThreadStart method, IBlockThreadMessage messageHandle = null)
        {
            return new BlockThread( null, method, messageHandle);
        }
        /// <summary>
        /// 执行函数
        /// </summary>
        /// <param name="args"></param>
        private static void RunMethod(object args)
        {
            BlockThread info = args as BlockThread;
            if (info == null)
            {
                return;
            }
            try
            {
                if(info._messageHandle != null) 
                {
                    info._messageHandle.OnThreadStart(info);
                }
                if (info._runMethod != null)
                {
                    info._runMethod();
                }
                else if (info._runParamMethod!=null)
                {
                    info._runParamMethod(info._args);
                }
            }
            finally
            {
                info._isRunning = false;
                info.UnLock();
                if (info._messageHandle != null)
                {
                    info._messageHandle.OnThreadEnd(info);
                }
            }
        }

        /// <summary>
        /// 开启线程
        /// </summary>
        public void StartThread(object args=null) 
        {
            if (IsRunning) 
            {
                return;
            }
            _args = args;
            _isRunning = true;
            _autoHandle = new AutoResetEvent(true);
            _autoHandle.Reset();
            
            _thd.Start(this);
        }
        /// <summary>
        /// 关闭线程
        /// </summary>
        public void StopThread() 
        {
            _isRunning = false;
            bool isStop = false;
            if (_autoHandle != null) 
            {
                isStop = _autoHandle.WaitOne(10000);
            }
            _autoHandle = null;
            if (!isStop && _thd != null && _thd.IsAlive) 
            {
                try
                {
                    _thd.Abort();
                }
                catch { }
            }
            _thd = null;
        }
        /// <summary>
        /// 告诉线程要关闭
        /// </summary>
        public void SendThreadStop()
        {
            _isRunning = false;
            
        }
        /// <summary>
        /// 通知线程已经执行完
        /// </summary>
        public void UnLock() 
        {
            if (_autoHandle != null)
            {
                _autoHandle.Set();
            }
        }
        private object _args;

        private bool _isRunning;

        private Thread _thd;
        /// <summary>
        /// 阻塞
        /// </summary>
        private AutoResetEvent _autoHandle = null;
        /// <summary>
        /// 当前线程
        /// </summary>
        public Thread CurrentThread
        {
            get
            {
                return _thd;
            }

           
        }

        /// <summary>
        /// 传入参数
        /// </summary>
        public object Args
        {
            get
            {
                return _args;
            }

            
        }

        /// <summary>
        /// 本线程是否正在运行运行
        /// </summary>
        public bool IsRunning
        {
            get
            {
                if (_isRunning) 
                {
                    return _thd.IsAlive;
                }
                return _isRunning;
            }

            
        }

        /// <summary>
        /// 阻塞
        /// </summary>
        public AutoResetEvent LockHandle 
        {
            get { return _autoHandle; }
        }

        public void Dispose()
        {
            StopThread();
        }
    }
    /// <summary>
    /// 线程消息
    /// </summary>
    public interface IBlockThreadMessage 
    {
        /// <summary>
        /// 线程开始
        /// </summary>
        /// <param name="thd"></param>
        void OnThreadStart(BlockThread thd);
        /// <summary>
        /// 线程结束
        /// </summary>
        /// <param name="thd"></param>
        void OnThreadEnd(BlockThread thd);
    }
}
