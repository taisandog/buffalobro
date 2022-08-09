using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Buffalo.Kernel.TreadPoolManager
{
    /// <summary>
    /// 带参数带返回的线程类
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public delegate object ParameterizedReturnThreadStart(object obj);
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
        /// 返回值
        /// </summary>
        private object _returnData;
        /// <summary>
        /// 返回值
        /// </summary>
        public object ReturnData 
        {
            get 
            {
                return _returnData;
            }
        }
        /// <summary>
        /// 带参函数
        /// </summary>
        private ParameterizedThreadStart _runParamMethod;

        /// <summary>
        /// 带参带返回函数
        /// </summary>
        private ParameterizedReturnThreadStart _runParamReturnMethod;
        private BlockThread( ThreadStart method, ParameterizedThreadStart paramMethod, ParameterizedReturnThreadStart paramReturnMethod,
            IBlockThreadMessage messageHandle=null) 
        {
            _thd = new Thread(new ParameterizedThreadStart(RunMethod));
            _runMethod = method;
            _runParamMethod = paramMethod;
            _runParamReturnMethod = paramReturnMethod;
            _messageHandle = messageHandle;
        }
        

        /// <summary>
        /// 创建线程信息
        /// </summary>
        /// <param name="thd"></param>
        /// <returns></returns>
        public static BlockThread Create(ThreadStart method, IBlockThreadMessage messageHandle = null) 
        {
            return new BlockThread(method,null,null, messageHandle);
        }
        /// <summary>
        /// 创建线程信息
        /// </summary>
        /// <param name="thd"></param>
        /// <returns></returns>
        public static BlockThread Create(ParameterizedThreadStart method, IBlockThreadMessage messageHandle = null)
        {
            return new BlockThread( null, method,null, messageHandle);
        }
        /// <summary>
        /// 创建线程信息
        /// </summary>
        /// <param name="thd"></param>
        /// <returns></returns>
        public static BlockThread Create(ParameterizedReturnThreadStart method, IBlockThreadMessage messageHandle = null)
        {
            return new BlockThread(null, null, method, messageHandle);
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
                else if (info._runParamReturnMethod != null)
                {
                    info._returnData=info._runParamReturnMethod(info._args);
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
            _autoHandle = new ManualResetEvent(true);
            _autoHandle.Reset();
            
            _thd.Start(this);
        }
        /// <summary>
        /// 关闭线程
        /// </summary>
        public void StopThread(int millisecondsTimeout= 10000) 
        {
            _isRunning = false;
            bool isStop = false;
            if (_autoHandle != null) 
            {
                isStop = _autoHandle.WaitOne(millisecondsTimeout);
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
        /// 等待线程结束
        /// </summary>
        public bool Wait(int millisecondsTimeout)
        {
            if (_autoHandle != null)
            {
                return _autoHandle.WaitOne(millisecondsTimeout);
            }
            return true;
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
        private ManualResetEvent _autoHandle = null;
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
        public ManualResetEvent LockHandle 
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
