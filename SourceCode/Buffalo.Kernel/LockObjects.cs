using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Buffalo.Kernel
{
    /// <summary>
    /// 为某个值提供锁对象的管理器
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class LockObjects<T>
    {
        /// <summary>
        /// 真实数据的字典
        /// </summary>
        private LinkedDictionary<T, LockItem<T>> _dic;
        
        /// <summary>
        /// 最后清理时间
        /// </summary>
        private DateTime _lastClean;
        /// <summary>
        /// 清理间隔(秒)
        /// </summary>
        private const int CleanSeconds = 60;
        /// <summary>
        /// 内存锁定记录
        /// </summary>
        /// <param name="timeoutMillisecond">超时时间(毫秒数)</param>
        public LockObjects()
        {
            _dic = new LinkedDictionary<T, LockItem<T>>();
            _lastClean = DateTime.Now;
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
                ClearTimeout(nowDate);
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
        private void ClearTimeout(DateTime nowDate)
        {
            if (nowDate.Subtract(_lastClean).TotalSeconds < CleanSeconds)//自动清理很久不用的变量
            {
                return;
            }
            
            DateTime dt = DateTime.Now;
            NodeValue<T, LockItem<T>> node = null;
            do
            {
                node = _dic.HeadNode;
                if (node == null)
                {
                    break;
                }
                if (dt.Subtract(node.Value.LastTime).TotalSeconds<300)
                {
                    break;
                }
                DeleteObject(node.Key);
            } while (node != null);



            _lastClean = DateTime.Now;
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
        /// <summary>
        /// 键
        /// </summary>
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
