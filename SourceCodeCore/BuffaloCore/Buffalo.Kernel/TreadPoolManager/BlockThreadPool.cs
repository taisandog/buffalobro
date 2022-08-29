
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
    public class BlockThreadPool: IBlockThreadMessage
    {
        /// <summary>
        /// 线程信息
        /// </summary>
        private ConcurrentDictionary<BlockThread,bool> _que = new ConcurrentDictionary<BlockThread, bool>();

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
        /// 用户线程池
        /// </summary>
        /// <param name="checkmilliseconds">检查间隔</param>
        /// <param name="threadTimeOut">线程过时</param>
        public BlockThreadPool(int checkmilliseconds=1000) 
        {
            _checkmilliseconds = checkmilliseconds;
            
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

            ConcurrentDictionary<BlockThread, bool> que = _que;
            Queue<BlockThread> queDelete = new Queue<BlockThread>();
            foreach(KeyValuePair<BlockThread,bool> kvp in que)
            {
                einfo = kvp.Key;
                if (!einfo.IsRunning) 
                {
                    queDelete.Enqueue(einfo);
                }
               
            }

            foreach(BlockThread thd in queDelete) 
            {
                que.TryRemove(thd, out _);
                thd.StopThread();
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
                ConcurrentDictionary<BlockThread,bool> que = _que;
                if (que != null)
                {
                    BlockThread einfo = null;
                    foreach (KeyValuePair<BlockThread, bool> kvp in que)
                    {
                        einfo = kvp.Key;
                        einfo.SendCancel();
                    }
                    foreach (KeyValuePair<BlockThread, bool> kvp in que)
                    {
                        einfo = kvp.Key;
                        try
                        {
                            einfo.StopThread();
                        }
                        catch { }
                    }
                    
                }
                que.Clear();
            }
            catch { }
        }
        /// <summary>
        /// 增加线程信息
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public void AppendThreadInfo(BlockThread info) 
        {
            info.MessageHandle = this;
            CleanTimeout();
            _que[info]=true;
        }

        /// <summary>
        /// 告诉线程池本线程已经完结
        /// </summary>
        /// <param name="info"></param>
        public void OnThreadEnd(BlockThread info) 
        {
            _que.TryRemove(info, out _);
        }

        /// <summary>
        /// 启动无参数线程
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public BlockThread RunThread(ThreadStart method)
        {

            BlockThread info = BlockThread.Create(method,this);
            AppendThreadInfo(info);
            info.StartThread();
            return info;
        }
        /// <summary>
        /// 启动有参数线程
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public BlockThread RunParamThread(ParameterizedThreadStart method,object args)
        {

            BlockThread info = BlockThread.Create(method,this);
            AppendThreadInfo(info);
            info.StartThread(args);
            return info;
        }
        /// <summary>
        /// 启动有参数带返回的线程
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public BlockThread RunParamReturnThread(ParameterizedReturnThreadStart method, object args)
        {

            BlockThread info = BlockThread.Create(method, this);
            AppendThreadInfo(info);
            info.StartThread(args);
            return info;
        }

        public void OnThreadStart(BlockThread thd)
        {
            
        }
    }
}
