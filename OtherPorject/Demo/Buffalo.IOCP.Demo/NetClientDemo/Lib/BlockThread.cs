using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

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
        private object _args;

        private bool _isCancel=false;

        private Thread _thd;

        /// <summary>
        /// 是否要取消线程
        /// </summary>
        public bool IsCancel
        {
            get
            {
                return _isCancel;
            }
        }

        private bool _saveThreadInfo;

        /// <summary>
        /// 保存当前线程信息，此为true时候线程方法可以通过BlockThread.CurrentThreadInfo获取到当前实例
        /// </summary>
        public bool SaveThreadInfo
        {
            get 
            {
                return _saveThreadInfo;
            }
            set 
            {
                _saveThreadInfo = value;
            }
        }

        private static ThreadLocal<BlockThread> _tlThreadInfo=new ThreadLocal<BlockThread>();

        /// <summary>
        /// 当前线程的信息
        /// </summary>
        public static BlockThread CurrentThreadInfo 
        {
            get 
            {
                return _tlThreadInfo.IsValueCreated? _tlThreadInfo.Value : null;
            }
        }
        /// <summary>
        /// 当前线程是否需要取消
        /// </summary>
        public static bool CurrentThreadCancelled
        {
            get
            {
                return _tlThreadInfo.IsValueCreated ? _tlThreadInfo.Value._isCancel : false;
            }
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
                if (info._saveThreadInfo) 
                {
                    _tlThreadInfo.Value = info;
                }
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
                try
                {
                    info.UnLock();
                    if (info._messageHandle != null)
                    {
                        info._messageHandle.OnThreadEnd(info);
                    }
                }
                catch (Exception ex) { }
                ClearThread(info);
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
            ResetData();

            _args = args;
            _isCancel = false;
            _autoHandle = new ManualResetEvent(true);
            _autoHandle.Reset();
            
            _thd.Start(this);
        }
        /// <summary>
        /// 关闭线程
        /// </summary>
        public void StopThread(int millisecondsTimeout= 10000) 
        {
            bool isStop = false;
            if (_autoHandle != null) 
            {
                isStop = _autoHandle.WaitOne(millisecondsTimeout);
            }
           
            if (!isStop && _thd != null && _thd.IsAlive) 
            {
                try
                {
                    _thd.Abort();
                }
                catch { }
            }
            ClearThread(this);
        }

       

       /// <summary>
       /// 清理类信息
       /// </summary>
        private static void ClearThread(BlockThread info) 
        {
            if (info._autoHandle != null)
            {
                try
                {
                    info._autoHandle.Dispose();
                }
                catch { }
            }
            info._autoHandle = null;
            info._thd = null;
            info._messageHandle = null;
            if (info._saveThreadInfo)
            {
                _tlThreadInfo.Value = null;
            }
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
        /// 告诉线程要取消
        /// </summary>
        public void SendCancel()
        {
            _isCancel = true;
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
                Thread thd = _thd;
                return thd!=null && thd.IsAlive;
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
            ResetData();
            
            _runMethod = null;
            _runParamMethod = null;
            _runParamReturnMethod = null;
        }

        private void ResetData() 
        {
            _returnData = null;
            _args = null;

        }
        ~BlockThread() 
        {
            Dispose();
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
