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
#if (NET_2_0)
            throw new NotSupportedException("不支持:" + type + " 的缓存类型，当前只支持system、memcached类型的缓存");
#else
            else if (dtype.Equals("redis", StringComparison.CurrentCultureIgnoreCase))//redis
            {
                return new RedisAdaperByServiceStack(connectionString, info);
            }
#endif
            return null;
        }
    }
}
