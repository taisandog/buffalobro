using Buffalo.DB.DbCommon;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;


namespace Buffalo.DB.CacheManager
{
    /// <summary>
    /// 本地缓存基类
    /// </summary>
    public abstract class LocalCacheBase
    {
        /// <summary>
        /// 获取过期时间
        /// </summary>
        /// <param name="expiration"></param>
        /// <param name="expir"></param>
        /// <returns></returns>
        public static TimeSpan GetExpir(TimeSpan expiration, TimeSpan expir)
        {
            if (expir > TimeSpan.MinValue)
            {
                return expir;
            }
            return expiration;
        }
    }
}
