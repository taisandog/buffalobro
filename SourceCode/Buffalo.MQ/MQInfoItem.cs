using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buffalo.MQ
{
    /// <summary>
    /// 队列信息
    /// </summary>
    public class MQInfoItem
    {
        /// <summary>
        /// 配置
        /// </summary>
        private MQConfigBase _config;
        /// <summary>
        /// 获取配置
        /// </summary>
        /// <returns></returns>
        public MQConfigBase Config
        {
            get
            {
                return _config;
            }
        }
        /// <summary>
        /// 队列信息
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="type">类型</param>
        /// <param name="connectString">连接字符串</param>
        public MQInfoItem(string name, string type, MQConfigBase config)
        {
            _name = name;
            _mqType = type;
            _config = config;

        }

        private string _name;
        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            get
            {
                return _name;
            }
        }

        private string _mqType;
        /// <summary>
        /// 队列类型
        /// </summary>
        public string MQType
        {
            get
            {
                return _mqType;
            }
        }

        


    }
}
