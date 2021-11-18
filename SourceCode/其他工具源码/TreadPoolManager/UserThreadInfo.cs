using Buffalo.ArgCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Buffalo.Kernel.TreadPoolManager
{
    /// <summary>
    /// 线程信息
    /// </summary>
    public class UserThreadInfo
    {
        private Thread _thd;
        /// <summary>
        /// 线程
        /// </summary>
        public Thread CurrentThread
        {
            get
            {
                return _thd;
            }
        }
        private AutoResetEvent _handle;
        /// <summary>
        /// 锁的阻塞
        /// </summary>
        public AutoResetEvent Handle
        {
            get
            {
                return _handle;
            }

        }
        
        private DateTime _createDate;
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateDate
        {
            get
            {
                return _createDate;
            }
        }
        /// <summary>
        /// 无参数函数
        /// </summary>
        private ThreadStart _method;
        /// <summary>
        /// 无参数函数
        /// </summary>
        public ThreadStart Method 
        {
            get 
            {
                return _method;
            }
        }
        /// <summary>
        /// 带参数函数
        /// </summary>
        private ParameterizedThreadStart _paramMethod;
        /// <summary>
        /// 带参数函数
        /// </summary>
        public ParameterizedThreadStart ParamMethod
        {
            get
            {
                return _paramMethod;
            }
        }

        /// <summary>
        /// 参数
        /// </summary>
        private object _param;
        /// <summary>
        /// 参数
        /// </summary>
        public object Param
        {
            get
            {
                return _param;
            }
        }
        /// <summary>
        /// 押注线程信息
        /// </summary>
        /// <param name="thd">线程</param>
        /// <param name="args">参数</param>
        public UserThreadInfo()
        {
            _handle = new AutoResetEvent(true);
            _handle.Reset();
            _createDate = DateTime.Now;
        }
        /// <summary>
        /// 执行无参数线程
        /// </summary>
        /// <param name="thd"></param>
        public void RunThread(ThreadStart method)
        {
            _method = method;
            _thd = new Thread(new ThreadStart(DoThread));
            _thd.Start();
        }
        /// <summary>
        /// 执行参数线程
        /// </summary>
        /// <param name="thd"></param>
        public void RunParamThread(ParameterizedThreadStart paramMethod,object args)
        {
            _paramMethod = paramMethod;
            _thd = new Thread(new ParameterizedThreadStart(DoParamThread));
            _thd.Start(args);
        }
        /// <summary>
        /// 执行无参数线程
        /// </summary>
        public void DoThread() 
        {
            try
            {
                _method();
            }
            finally 
            {
                UnLock();
            }
        }
        /// <summary>
        /// 执行有参数线程
        /// </summary>
        public void DoParamThread(object args)
        {
            try
            {
                _paramMethod(args);
            }
            finally
            {
                UnLock();
            }
        }
        /// <summary>
        /// 清空信息
        /// </summary>
        public void Clear()
        {
            _thd = null;
            
            if (_handle != null)
            {
                _handle.Close();
                _handle.Dispose();
            }
            _handle = null;
        }
        public void UnLock()
        {
            if (_handle != null)
            {
                _handle.Set();
            }
        }
        /// <summary>
        /// 是否已经结束
        /// </summary>
        public bool IsRunning
        {
            get 
            {
                return _thd != null && _thd.IsAlive;
            }
        }

        /// <summary>
        /// 关闭线程
        /// </summary>
        /// <returns></returns>
        public APIResault CloseThread()
        {
            bool hasClose = false;
            if (_handle == null || _thd == null)
            {
                return MU.GetSuccessValue(null, hasClose);
            }
            
            if (!_handle.WaitOne(100))
            {
                try
                {
                    _thd.Abort();
                    Thread.Sleep(10);
                    hasClose = true;
                }
                catch (Exception ex)
                {
                    return MU.GetFault(ex.ToString());
                }
            }
            Clear();
            return MU.GetSuccessValue(null, hasClose);
        }
    }
}
