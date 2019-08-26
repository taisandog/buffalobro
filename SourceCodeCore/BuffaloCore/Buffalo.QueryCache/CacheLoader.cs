using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DB.CacheManager;
using Buffalo.DB.DataBaseAdapter;

namespace Buffalo.QueryCache
{
    /// <summary>
    /// 获取缓存的加载器
    /// </summary>
    public class CacheLoader
    {
        /// <summary>
        /// 根据类型创建缓存适配器
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="connectionString">连接字符串</param>
        /// <returns></returns>
        public static ICacheAdaper GetCache(DBInfo info, string type, string connectionString)
        {
            string dtype = type.Trim();
            if (dtype.Equals("memcached", StringComparison.CurrentCultureIgnoreCase))//memcached
            {
                return new MemCachedAdaper(connectionString, info);
            }

            else if (dtype.Equals("redis", StringComparison.CurrentCultureIgnoreCase))//redis
            {
                return new RedisAdaperByStackExchange(connectionString, info);
            }
            else if (dtype.Equals("web", StringComparison.CurrentCultureIgnoreCase))//redis
            {
                return new RedisAdaperByStackExchange(connectionString, info);
            }
            return null;
        }
    }
}
