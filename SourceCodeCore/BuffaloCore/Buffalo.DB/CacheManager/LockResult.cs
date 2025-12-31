using System;
using System.Collections.Generic;
using System.Text;

namespace Buffalo.DB.CacheManager
{
    /// <summary>
    /// 锁结果
    /// </summary>
    public enum LockResult:int
    {
        /// <summary>
        /// 获取锁成功
        /// </summary>
        Success = 1,
        /// <summary>
        /// 已经锁定
        /// </summary>
        AlreadyLocked = 2,
        /// <summary>
        /// 获取锁失败
        /// </summary>
        Failed = 3,
    }
    /// <summary>
    /// 解锁结果
    /// </summary>
    public enum UnlockResult : int
    {
        /// <summary>
        /// 解锁成功
        /// </summary>
        Success = 1,
        /// <summary>
        /// 锁已过期
        /// </summary>
        Expired = 2,
        /// <summary>
        /// 解锁失败
        /// </summary>
        Failed = 3,
    }
}
