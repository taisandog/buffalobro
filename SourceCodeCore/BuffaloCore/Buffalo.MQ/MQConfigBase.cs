using Buffalo.Kernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buffalo.MQ
{
    /// <summary>
    /// 队列配置基类
    /// </summary>
    public abstract class MQConfigBase
    {
        /// <summary>
        /// 队列配置基类
        /// </summary>
        /// <param name="connectString">连接字符串</param>
        public MQConfigBase(string connectString)
        {
            _configs = ConnStringFilter.GetConnectInfo(connectString);
            
        }
        protected Dictionary<string, string> _configs;
        /// <summary>
        /// 配置
        /// </summary>
        public Dictionary<string, string> Configs
        {
            get
            {
                return _configs;
            }
        }
        /// <summary>
        /// 创建连接
        /// </summary>
        /// <returns></returns>
        public abstract MQConnection CreateConnection();
        /// <summary>
        /// 创建监听器
        /// </summary>
        /// <returns></returns>
        public abstract MQListener CreateListener();
    }
}
