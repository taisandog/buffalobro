using System;
using System.Collections.Generic;
using System.Text;

namespace Buffalo.DB.CacheManager.MemoryCacheUnit
{
    /// <summary>
    /// 表示用于在内存中缓存数据的缓存元素.
    /// </summary>
    internal class MemoryStorageItem
    {
        /// <summary>
        /// 获了或设置缓存条目的有效期.
        /// </summary>
        internal DateTime Period { get; set; }


 

        /// <summary>
        /// 获取或设置缓存的数据.
        /// </summary>
        public object Data { get; set; }
        /// <summary>
        /// 获取或设置缓存条目的超时毫秒数.
        /// </summary>
        internal double PeriodMilliseconds { get; set; }

        /// <summary>
        /// 创建一个 <see cref="MemcacheItem"/> 对象实例.
        /// </summary>
        internal MemoryStorageItem()
        {
        }
        /// <summary>
        /// 删除数据
        /// </summary>
        internal void FreeData()
        {
            Data = null;
        }
        ~MemoryStorageItem()
        {
            FreeData();
        }
    }
}
