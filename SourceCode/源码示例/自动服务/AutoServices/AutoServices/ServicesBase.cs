using System;
using System.Collections.Generic;
using System.Text;

namespace AutoServices
{
    public abstract class ServicesBase
    {
        /// <summary>
        /// 服务名称
        /// </summary>
        public virtual string ServiceName
        {
            get { return ""; } 
        }
        /// <summary>
        /// 首次开启服务
        /// </summary>
        /// <returns></returns>
        public virtual bool Start() 
        {
            return true;
        }
        /// <summary>
        /// 强制结束
        /// </summary>
        /// <returns></returns>
        public virtual bool Stop() 
        {
            return true;
        }
        /// <summary>
        /// 每隔一段时间执行
        /// </summary>
        /// <returns></returns>
        public virtual bool RunService() 
        {
            _isRunning = true;
            bool isRun=RunContext();
            _isRunning = false;
            if (isRun)
            {
                _lastAliveTime = DateTime.Now;
            }
            return true;
        }

        /// <summary>
        /// 需要定时执行的内容
        /// </summary>
        /// <returns></returns>
        protected virtual bool RunContext() 
        {
            return false;
        }

        protected bool _isRunning=false;
        /// <summary>
        /// 是否正在执行
        /// </summary>
        public virtual bool IsRunning 
        {
            get 
            {
                return _isRunning;
            }
        }


        protected DateTime _lastAliveTime;

        /// <summary>
        /// 最后活动时间
        /// </summary>
        public virtual DateTime LastAliveTime 
        {
            get { return _lastAliveTime; }
            
        }
    }
}
