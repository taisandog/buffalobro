using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Buffalo.Kernel.TreadPoolManager
{
    /// <summary>
    ///  用户线程池
    /// </summary>
    public class UserThreadPool
    {
        /// <summary>
        /// 线程信息
        /// </summary>
        private ConcurrentQueue<UserThreadInfo> _que = new ConcurrentQueue<UserThreadInfo>();

        /// <summary>
        /// 最后清理日期
        /// </summary>
        private DateTime _lastClean = DateTime.MinValue;
        private int _checkmilliseconds = 0;
        /// <summary>
        /// 检查间隔
        /// </summary>
        public int Checkmilliseconds 
        {
            get 
            {
                return _checkmilliseconds;
            }
        }
        /// <summary>
        /// 线程过时
        /// </summary>
        private int _threadTimeOut = 0;
        /// <summary>
        /// 线程过时
        /// </summary>
        public int ThreadTimeOut
        {
            get
            {
                return _threadTimeOut;
            }
        }
        
       

        /// <summary>
        /// 用户线程池
        /// </summary>
        /// <param name="checkmilliseconds">检查间隔</param>
        /// <param name="threadTimeOut">线程过时</param>
        public UserThreadPool(int checkmilliseconds=1000,int threadTimeOut=2000) 
        {
            _checkmilliseconds = checkmilliseconds;
            _threadTimeOut = threadTimeOut;
            
        }


        /// <summary>
        /// 清除过时线程
        /// </summary>
        public void CleanTimeout()
        {
            DateTime now = DateTime.Now;
            if (now.Subtract(_lastClean).TotalMilliseconds < _checkmilliseconds)
            {
                return;
            }
            UserThreadInfo einfo = null;
            int num = 0;
            ConcurrentQueue<UserThreadInfo> que = _que;
            while (que != null && que.Count > 0)
            {
                if (!que.TryPeek(out einfo))
                {
                    break;
                }
                if (now.Subtract(einfo.CreateDate).TotalMilliseconds < ThreadTimeOut && einfo.IsRunning)
                {
                    break;
                }
                if (!que.TryDequeue(out einfo))
                {
                    break;
                }
                num++;
                einfo.CloseThread();
                einfo = null;
            }
            _lastClean = DateTime.Now;
        }

        /// <summary>
        /// 清空所有线程
        /// </summary>
        public void CleanAll()
        {
            try
            {
                ConcurrentQueue<UserThreadInfo> que = _que;
                if (que != null)
                {
                    UserThreadInfo einfo = null;
                    while (que.Count > 0)
                    {
                        if (!_que.TryDequeue(out einfo))
                        {
                            break;
                        }
                        einfo.CloseThread();
                    }
                    
                }
            }
            catch { }
        }
        /// <summary>
        /// 增加线程信息
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public UserThreadInfo AppendThreadInfo(UserThreadInfo info) 
        {
            CleanTimeout();
            _que.Enqueue(info);
            return info;
        }
        /// <summary>
        /// 启动无参数线程
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public UserThreadInfo RunThread(ThreadStart method)
        {
            
            UserThreadInfo info = new UserThreadInfo();
            info.RunThread(method);
            return AppendThreadInfo(info);
        }
        /// <summary>
        /// 启动有参数线程
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public UserThreadInfo RunParamThread(ParameterizedThreadStart method,object args)
        {

            UserThreadInfo info = new UserThreadInfo();
            info.RunParamThread(method, args);
            return AppendThreadInfo(info);
        }

    }
}
