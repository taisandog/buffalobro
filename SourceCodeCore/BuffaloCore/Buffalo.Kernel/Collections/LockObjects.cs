﻿using System;
using System.Collections.Generic;

namespace Buffalo.Kernel.Collections
{
    /// <summary>
    /// 为某个值提供锁对象的管理器
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class LockObjects<K> : LockObjects<K, object>
    {
        
    }
    /// <summary>
    /// 为某个值提供锁对象的管理器
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class LockObjects<K, V>: IDisposable 
    {
        private LinkedDictionary<K, LockItem<K, V>> _dic;
        /// <summary>
        /// 最后清理时间
        /// </summary>
        private DateTime _lastClean;
        /// <summary>
        /// 清理间隔(秒)
        /// </summary>
        protected virtual int CleanSeconds
        {
            get {  return 60; }
        }
        /// <summary>
        /// 内存锁定记录
        /// </summary>
        public LockObjects()
        {
            _dic = new LinkedDictionary<K, LockItem<K, V>>();
            _lastClean = DateTime.Now;
        }
        /// <summary>
        /// 获取要锁的对象
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public V GetObject(K key)
        {
            LockItem<K, V> ret = null;
            lock (_dic)
            {
                DateTime nowDate = DateTime.Now;
                ClearTimeout(nowDate);
                if (!_dic.TryGetValue(key, out ret))
                {
                    ret = new LockItem<K, V>();
                    ret.Key = key;
                    ret.LockObject = CreateInstance();

                    _dic[key] = ret;
                }

                ret.LastTime = nowDate;

                return ret.LockObject;
            }
        }

        protected virtual V CreateInstance() 
        {
            return (V)Activator.CreateInstance(typeof(V));
        }
        /// <summary>
        /// 清除超时
        /// </summary>
        private void ClearTimeout(DateTime nowDate)
        {
            int clearSeconds = CleanSeconds;
            if (nowDate.Subtract(_lastClean).TotalSeconds < clearSeconds)//自动清理很久不用的变量
            {
                return;
            }
            Queue<K> queNeedDelete = new Queue<K>();
            DateTime dt = DateTime.Now;
            
            foreach (LinkedValueNode<K, LockItem<K, V>> kvp in _dic.GetEnumeratorOldToNew())
            {
                if (dt.Subtract(kvp.Value.LastTime).TotalSeconds < clearSeconds)
                {
                    break;
                }
                queNeedDelete.Enqueue(kvp.Key);
            }
            LockItem<K, V> val =null;
            foreach (K key in queNeedDelete)
            {
                _dic.RemoveValue(key,out val);
                if(val != null) 
                {
                    ReleaseValue(val);
                }
            }
            _lastClean = DateTime.Now;
        }

        protected virtual void ReleaseValue(LockItem<K, V> value) 
        {

        }
        /// <summary>
        /// 释放对象
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public void DeleteObject(K key)
        {
            LockItem<K, V> val = null;
            lock (_dic)
            {
                _dic.Remove(key);
                if (val != null)
                {
                    ReleaseValue(val);
                }
            }
        }

        public void Dispose()
        {
            _dic.Dispose();
            _dic = null;
        }
    }
    /// <summary>
    /// 要锁的项
    /// </summary>
    public class LockItem<K, V>
    {
        public K Key;
        /// <summary>
        /// 要锁的类
        /// </summary>
        public V LockObject;
        /// <summary>
        /// 最后访问时间
        /// </summary>
        public DateTime LastTime;
    }
}
