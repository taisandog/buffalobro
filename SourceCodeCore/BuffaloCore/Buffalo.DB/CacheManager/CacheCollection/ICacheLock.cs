using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buffalo.DB.CacheManager.CacheCollection
{
    /// <summary>
    /// 缓存锁
    /// </summary>
    public interface ICacheLock:IDisposable
    {
        /// <summary>
        /// 是否锁了
        /// </summary>
        bool Islock { get; }
        /// <summary>
        /// 锁定用户  
        /// </summary>
        /// <returns></returns>
        bool Lock(long millisecondsTimeout = -1, int pollingMillisecond =-1);
        /// <summary>
        /// 解锁用户  
        /// </summary>
        /// <returns></returns>
        bool UnLock();

        /// <summary>
        /// 锁定用户  
        /// </summary>
        /// <returns></returns>
        Task<bool> LockAsync(long millisecondsTimeout = -1, int pollingMillisecond = -1);
        /// <summary>
        /// 解锁用户  
        /// </summary>
        /// <returns></returns>
        Task<bool> UnLockAsync();

    }
}
