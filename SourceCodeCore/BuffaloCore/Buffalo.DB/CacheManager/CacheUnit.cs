using Buffalo.DB.DataBaseAdapter;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Buffalo.DB.CacheManager
{
    /// <summary>
    /// 缓存创建器
    /// </summary>
    public class CacheUnit
    {
        /// <summary>
        /// 创建缓存类
        /// </summary>
        /// <param name="cacheType">缓存类型(BuffaloCacheTypes.System、BuffaloCacheTypes.Web)</param>
        /// <param name="connectionString">连接字符串</param>
        /// <returns></returns>
        public static QueryCache CreateCache(string cacheType,string connectionString)
        {
            string name = "CacheDB" + DateTime.Now.ToString("yyyyMMddHHmmss");
            return CreateCache(name, cacheType, connectionString);
        }
        /// <summary>
        /// 创建缓存类
        /// </summary>
        /// <param name="name">缓存名</param>
        /// <param name="cacheType">缓存类型(BuffaloCacheTypes.System、BuffaloCacheTypes.Web)</param>
        /// <param name="connectionString">连接字符串</param>
        /// <returns></returns>
        public static QueryCache CreateCache(string name,string cacheType, string connectionString)
        {
            DBInfo info = new DBInfo(name, "", "", "Sql2K", LazyType.Disable);
            ICacheAdaper ica = QueryCache.GetCache(info, cacheType, connectionString);
            info.SetQueryCache(ica, false);
            return info.QueryCache;
        }

        /// <summary>
        /// 加密连接字符串裁剪值
        /// </summary>
        /// <param name="part"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string EncodeString(string str)
        {
            string ret = str.Replace("%", "%25");
            ret = ret.Replace(";", "%3b");
            ret = ret.Replace("=", "%3d");
            return ret;
        }
        /// <summary>
        /// 解密连接字符串裁剪值
        /// </summary>
        /// <param name="part"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string DecodeString(string str)
        {
            string ret = str;
            ret = ret.Replace("%3b", ";");
            ret = ret.Replace("%3d", "=");
            ret = ret.Replace("%25", "%"); 
            return ret;
        }
        /// <summary>
        /// 连接字符串裁剪值
        /// </summary>
        /// <param name="part"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string CutString(string part, int length)
        {
            if (string.IsNullOrEmpty(part))
            {
                return part;
            }
            string ret = part.Substring(length);
            if (string.IsNullOrEmpty(part))
            {
                return ret;
            }

            ret = CacheUnit.DecodeString(ret);
            return ret;
        }

        
    }
    /// <summary>
    /// 缓存类型
    /// </summary>
    public class BuffaloCacheTypes 
    {
        /// <summary>
        /// 系统内存
        /// </summary>
        public const string System = "system";
        /// <summary>
        /// Web Cache(Winform可用)
        /// </summary>
        public const string Web = "web";
#if (NET_2_0)
#else
        /// <summary>
        /// memcached
        /// </summary>
        public const string Memcached = "memcached";
        /// <summary>
        /// Redis
        /// </summary>
        public const string Redis = "redis";
#endif
    }
    /// <summary>
    /// 设置值的方式
    /// </summary>
    [Description("设置值的方式")]
    public enum SetValueType : int
    {
        /// <summary>
        /// 新增或覆盖，任何时候都设置成功
        /// </summary>
        [Description("新增或覆盖")]
        Set = 0,
        /// <summary>
        /// 覆盖模式，当存在此Key时候才能覆盖成功
        /// </summary>
        [Description("覆盖模式")]
        Replace = 1,
        /// <summary>
        /// 新增模式，当不存在此Key时候才能新增成功
        /// </summary>
        [Description("新增模式")]
        AddNew = 2,
    }
}
