﻿using Buffalo.Kernel;
using Buffalo.MQ.KafkaMQ;
#if !NET_4_5
using Buffalo.MQ.RabbitMQ;
#endif
using Buffalo.MQ.RedisMQ;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buffalo.MQ
{
    public delegate MQConfigBase DelCreateConfig(string connectString);
    /// <summary>
    /// 队列创建器
    /// </summary>
    public class MQUnit
    {
        private static ConcurrentDictionary<string, MQInfoItem> _dic = new ConcurrentDictionary<string, MQInfoItem>();

        private static ConcurrentDictionary<string, DelCreateConfig> _dicConfigCreate = InitConfigCreate();

        /// <summary>
        /// 线程变量名
        /// </summary>
        private const string Tag = "$*_MQ_Conn&$";

        /// <summary>
        /// 获取本线程变量连接
        /// </summary>
        /// <returns></returns>
        private static Dictionary<string,MQConnection> GetStaticConnTable()
        {
            Dictionary<string, MQConnection> dic = ContextValue.Current[Tag] as Dictionary<string, MQConnection>;
            if (dic == null)
            {
                dic = new Dictionary<string, MQConnection>();
                ContextValue.Current[Tag] = dic;
            }
            return dic;
        }

        private static ConcurrentDictionary<string, DelCreateConfig> InitConfigCreate()
        {
            ConcurrentDictionary<string, DelCreateConfig> dic = new ConcurrentDictionary<string, DelCreateConfig>(StringComparer.CurrentCultureIgnoreCase);
            dic["kafkamq"] = GetKafkaConfig;
#if !NET_4_5
            dic["rabbitmq"] = GetRabbitMQConfig;
#endif
            dic["redismq"] = GetRedisConfig;
            return dic;
        }
        /// <summary>
        /// 添加队列信息
        /// </summary>
        /// <param name="name">标记唯一的名字</param>
        /// <param name="mqType">队列类型</param>
        /// <param name="connectString">连接字符串</param>
        public static void SetMQInfo(string name,string mqType,string connectString)
        {
            //if (_dic.ContainsKey(name))
            //{
            //    throw new ArgumentException(name+"的配置已经存在");
            //}
            MQConfigBase config = GetConfig(mqType, connectString);
            MQInfoItem item = new MQInfoItem(name, mqType, config);
            _dic[name]=item;
        }
        /// <summary>
        /// 获取生产者的连接
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static MQConnection GetMQConnection(string name)
        {
            Dictionary<string, MQConnection> dic = GetStaticConnTable();
            MQConnection conn = null;
            //StringBuilder sbKey = new StringBuilder();
            //if (hasTransaction)
            //{
            //    sbKey.Append("1.");
            //}
            //else
            //{
            //    sbKey.Append("0.");
            //}
            //sbKey.Append(name);
            string key = name;
            if (dic.TryGetValue(key, out conn))
            {
                return conn;
            }
            MQInfoItem item = null;
            if (!_dic.TryGetValue(name, out item))
            {
                throw new KeyNotFoundException("找不到名字为:" + name + "的队列配置");
            }

            conn = item.Config.CreateConnection();
            dic[key] = conn;
            return conn;
        }
        /// <summary>
        /// 获取消费者连接
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static MQListener GetMQListener(string name)
        {
            
            MQInfoItem item = null;
            if (!_dic.TryGetValue(name, out item))
            {
                throw new KeyNotFoundException("找不到名字为:" + name + "的队列配置");
            }
            
            
            return item.Config.CreateListener();
        }
        /// <summary>
        /// 获取队列配置
        /// </summary>
        /// <param name="mqType"></param>
        /// <param name="connectString"></param>
        /// <returns></returns>
        private static MQConfigBase GetConfig(string mqType, string connectString)
        {
            DelCreateConfig ret = null;
            if(!_dicConfigCreate.TryGetValue(mqType,out ret))
            {
                throw new NotSupportedException("不支持:" + mqType + " 类型的队列");
            }
            return ret(connectString);
        }

        private static MQConfigBase GetKafkaConfig( string connectString)
        {
            return new KafkaMQConfig(connectString);
        }
        private static MQConfigBase GetRedisConfig(string connectString)
        {
            return new RedisMQConfig(connectString);
        }
#if !NET_4_5
        private static MQConfigBase GetRabbitMQConfig(string connectString)
        {
            return new RabbitMQConfig(connectString);
        }
#endif
        /// <summary>
        /// 获取监听信息
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        internal static IEnumerable<string> GetLintenKeys(object args)
        {
            IEnumerable<string> topics = args as IEnumerable<string>;
            if (topics != null)
            {
                return topics;
            }
            IEnumerable<MQOffestInfo> topicsOffest = args as IEnumerable<MQOffestInfo>;
            if (topicsOffest == null)
            {
                return null;
            }
            List<string> lstKey = new List<string>();
            foreach(MQOffestInfo info in topicsOffest)
            {
                lstKey.Add(info.Key);
            }
            return lstKey;
        }
        /// <summary>
        /// 获取监听信息
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        internal static IEnumerable<MQOffestInfo> GetLintenOffest(object args)
        {
           
            IEnumerable<MQOffestInfo> topicsOffest = args as IEnumerable<MQOffestInfo>;
            return topicsOffest;
        }
    }
}