using System;
using System.Collections.Generic;

namespace Buffalo.Kernel
{
    /// <summary>
    /// 为某个值提供锁对象的管理（等于锁一个对象）
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class LockObjects<T>
    {
        private Dictionary<T, LockItem<T>> _dic;
        /// <summary>
        /// 最后清理时间
        /// </summary>
        private DateTime _lastClean;
        /// <summary>
        /// 清理间隔(秒)
        /// </summary>
        private int _cleanSeconds = 60;
        /// <summary>
        /// 为某个值提供锁对象的管理
        /// </summary>
        /// <param name="cleanSeconds">自动检测清理的秒数</param>
        public LockObjects(int cleanSeconds)
        {
            _dic = new Dictionary<T, LockItem<T>>();
            _lastClean = DateTime.Now;
            _cleanSeconds = cleanSeconds;
        }/// <summary>
        /// 为某个值提供锁对象的管理
        /// </summary>
        /// <param name="cleanSeconds">自动检测清理的秒数</param>
        public LockObjects():this(60)
        {
            
        }
        /// <summary>
        /// 获取要锁的对象
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public object GetObject(T key)
        {
            LockItem<T> ret = null;
            lock (_dic)
            {
                DateTime nowDate = DateTime.Now;
                if (nowDate.Subtract(_lastClean).TotalSeconds >= _cleanSeconds)//自动清理很久不用的变量
                {
                    ClearTimeout();
                }

                if (!_dic.TryGetValue(key, out ret))
                {
                    ret = new LockItem<T>();
                    ret.Key = key;
                    ret.LockObject = new object();

                    _dic[key] = ret;
                }
                
                ret.LastTime = nowDate;

                return ret.LockObject;
            }
        }
        /// <summary>
        /// 清除超时
        /// </summary>
        private void ClearTimeout()
        {
            Queue<T> queNeedDelete = new Queue<T>();
            DateTime dt = DateTime.Now;
            foreach (KeyValuePair<T, LockItem<T>> kvp in _dic)
            {
                if (dt.Subtract(kvp.Value.LastTime).TotalSeconds >= _cleanSeconds)
                {
                    queNeedDelete.Enqueue(kvp.Key);
                }
            }
            foreach (T key in queNeedDelete)
            {
                _dic.Remove(key);
            }
        }

        /// <summary>
        /// 释放对象
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public void DeleteObject(T key)
        {
            
            lock (_dic)
            {
                _dic.Remove(key);
            }
        }

    }
    /// <summary>
    /// 要锁的项
    /// </summary>
    public class LockItem<T>
    {
        public T Key;
        /// <summary>
        /// 要锁的类
        /// </summary>
        public object LockObject;
        /// <summary>
        /// 最后访问时间
        /// </summary>
        public DateTime LastTime;
    }
}
