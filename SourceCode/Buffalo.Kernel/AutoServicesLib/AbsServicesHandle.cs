using System;
using System.Collections.Generic;
using System.Text;

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
        /// 间隔时间(毫秒)
        /// </summary>
        public abstract long Interval
        {
            get ;
            
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
        /// 是否已经到时间
        /// </summary>
        public bool Tick 
        {
            get 
            {
                TimeSpan ts=DateTime.Now.Subtract(_lasRun);
                if (ts.TotalMilliseconds >= Interval) 
                {
                    return true;
                }
                return false;
            }
        }

        /// <summary>
        /// 检查是否要运行(每隔一段时间执行)
        /// </summary>
        /// <returns></returns>
        public ServicesMessage CheckRun() 
        {
            ServicesMessage ret = null;
            if (Tick) 
            {
                ret = new ServicesMessage(this);
                if (!DoAction(ret)) 
                {
                    ret = null;
                }
                _lasRun = DateTime.Now;
            }
            return ret;
            
        }

        /// <summary>
        /// 执行方法
        /// </summary>
        protected abstract bool DoAction(ServicesMessage message);
    }
}
