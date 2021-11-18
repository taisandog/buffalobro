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
    public class BlockThreadPool
    {
        /// <summary>
        /// 线程信息
        /// </summary>
        private ConcurrentQueue<BlockThread> _que = new ConcurrentQueue<BlockThread>();

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
        public BlockThreadPool(int checkmilliseconds=1000,int threadTimeOut=2000) 
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
            BlockThread einfo = null;
            int num = 0;
            ConcurrentQueue<BlockThread> que = _que;
            while (que != null && que.Count > 0)
            {
                if (!que.TryPeek(out einfo))
                {
                    break;
                }
                if ( einfo.IsRunning)
                {
                    break;
                }
                if (!que.TryDequeue(out einfo))
                {
                    break;
                }
                num++;
                einfo.StopThread();
                einfo = null;
            }
            _lastClean = DateTime.Now;
        }

        /// <summary>
        /// 停止所有线程
        /// </summary>
        public void StopAll()
        {
            try
            {
                ConcurrentQueue<BlockThread> que = _que;
                Queue<BlockThread> beStop = new Queue<BlockThread>();
                if (que != null)
                {
                    BlockThread einfo = null;
                    while (que.Count > 0)
                    {
                        if (!_que.TryDequeue(out einfo))
                        {
                            break;
                        }
                        einfo.SendThreadStop();
                        beStop.Enqueue(einfo);
                    }
                    while (beStop.Count > 0)
                    {
                        einfo = beStop.Dequeue();
                        if (einfo==null)
                        {
                            break;
                        }
                        einfo.StopThread();
                        beStop.Enqueue(einfo);
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
        public BlockThread AppendThreadInfo(BlockThread info) 
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
        public BlockThread RunThread(ThreadStart method)
        {

            BlockThread info = BlockThread.Create(method);
            info.StartThread();
            return AppendThreadInfo(info);
        }
        /// <summary>
        /// 启动有参数线程
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public BlockThread RunParamThread(ParameterizedThreadStart method,object args)
        {

            BlockThread info = BlockThread.Create(method);
            info.StartThread(args);
            return AppendThreadInfo(info);
        }

    }
}
