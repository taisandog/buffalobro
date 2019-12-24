using System;
using System.Collections.Generic;
using System.Text;

namespace Buffalo.DB.CacheManager.MemoryCacheUnit
{
    /// <summary>
    /// 用于管理数据内存缓存的管理器.
    /// </summary>
    public class MemoryStorageManager
    {
        private Dictionary<string, MemoryStorageItem> _cachePool = new Dictionary<string, MemoryStorageItem>();
        /// <summary>
        /// 清理间隔
        /// </summary>
        private double _cleanMilliseconds = 1000;
        /// <summary>
        /// 最后清理时间
        /// </summary>
        private DateTime _lastClean;
        /// <summary>
        /// 缓存管理器内存产生错误时触发此事件.
        /// </summary>
        public event Action<string, Exception> Error;

        /// <summary>
        /// 用于触发 <see cref="MemcacheManager.Error"/> 事件.
        /// </summary>
        /// <param name="title">错误标题.</param>
        /// <param name="Ex">包含错误信息的异常对象.</param>
        private void OnError(string title, Exception Ex)
        {
            if (Error != null)
            {
                Error(title, Ex);
            }
        }
        /// <summary>
        /// 用于管理数据内存缓存的管理器.
        /// </summary>
        public MemoryStorageManager(double cleanMilliseconds)
        {
            _cleanMilliseconds = cleanMilliseconds;
            _lastClean = DateTime.Now;
        }

        /// <summary>
        /// 将给定的数据加入到内存数据缓存池中（触发过期缓存清理）.
        /// </summary>
        /// <param name="key">数据在内存缓存池中的键名称.</param>
        /// <param name="data">要缓存的数据内容.</param>
        /// <param name="periodMilliseconds">若已启用强制过期策略时，该缓存数据将在此时间段后被销毁（以毫秒为单位，0则不超时）.</param>
        public void SetValue(string key, object data, double periodMilliseconds)
        {
            lock (_cachePool)
            {
                RunCleanTask();

                MemoryStorageItem cacheItem = null;
                if (_cachePool.TryGetValue(key, out cacheItem))
                {
                    cacheItem.FreeData();
                }
                else
                {
                    cacheItem = new MemoryStorageItem();
                    _cachePool[key] = cacheItem;
                }
                cacheItem.Data = data;
                if (periodMilliseconds > 0)
                {
                    cacheItem.PeriodMilliseconds = periodMilliseconds;
                    cacheItem.Period = DateTime.Now.AddMilliseconds(periodMilliseconds);
                }
                else
                {
                    cacheItem.Period = DateTime.MinValue;
                }
            }
            
        }

        /// <summary>
        /// 获取内存缓存数据中指定键的数据对象（触发过期缓存清理）.
        /// </summary>
        /// <typeparam name="T">缓存的目标对象类型名称.</typeparam>
        /// <param name="key">缓存键名称.</param>
        /// <param name="defaultValue">当未找到缓存或缓存为空时返回的默认值.</param>
        /// <returns></returns>
        public object GetValue(string key)
        {
            MemoryStorageItem cacheItem;
            lock (_cachePool)
            {
                RunCleanTask();
                if (_cachePool.TryGetValue(key, out cacheItem))
                {
                    if (cacheItem.PeriodMilliseconds > 0)
                    {
                        cacheItem.Period = DateTime.Now.AddMilliseconds(cacheItem.PeriodMilliseconds);
                    }
                    return cacheItem.Data;
                }
                
            }
            return null;
        }
        /// <summary>
        /// 删除缓存
        /// </summary>
        /// <param name="key">缓存键名称.</param>
        /// <returns></returns>
        public void DeleteValue(string key)
        {
            MemoryStorageItem cacheItem;
            lock (_cachePool)
            {
                if (_cachePool.TryGetValue(key, out cacheItem))
                {
                    cacheItem.FreeData();
                }
                _cachePool.Remove(key);
            }
        }
        /// <summary>
        /// 清除全部
        /// </summary>
        public void ClearAll()
        {
            lock (_cachePool)
            {
                foreach (KeyValuePair<string, MemoryStorageItem> kvp in _cachePool)
                {
                    kvp.Value.FreeData();
                }
                _cachePool.Clear();
            }
            
        }
        /// <summary>
        /// 清除全部
        /// </summary>
        public List<string> AllKeys()
        {
           
            lock (_cachePool)
            {
                List<string> lst = new List<string>(_cachePool.Count);
                foreach (KeyValuePair<string, MemoryStorageItem> kvp in _cachePool)
                {
                    lst.Add(kvp.Key);
                }
                return lst;
            }

        }
        /// <summary>
        /// 开始一轮过期缓存的清理任务.
        /// </summary>
        private void RunCleanTask()
        {
            if (DateTime.Now.Subtract(_lastClean).TotalMilliseconds < _cleanMilliseconds)
            {
                return;
            }
            try
            {
                List<string> lstNeedRemove = new List<string>();
                foreach (KeyValuePair<string, MemoryStorageItem> kvp in _cachePool)
                {
                    
                    if (kvp.Value.PeriodMilliseconds>0 && kvp.Value.Period < DateTime.Now)
                    {
                        kvp.Value.FreeData();
                        lstNeedRemove.Add(kvp.Key);
                    }
                }
                foreach (string key in lstNeedRemove)
                {
                    _cachePool.Remove(key);
                }
            }
            catch (Exception Ex)
            {
                OnError("在缓存清理任务中产生错误.", Ex);
            }

        }
    }
}
