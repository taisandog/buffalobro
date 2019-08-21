using System;
using System.Collections.Generic;
using System.Text;

namespace Buffalo.DB.CacheStorage
{
    /// <summary>
    /// 缓存项
    /// </summary>
    public class CacheStorageItem<T>
    {
        /// <summary>
        /// 键
        /// </summary>
        private string _key;
        /// <summary>
        /// 键
        /// </summary>
        public string Key 
        {
            get 
            {
                return _key;
            }
            set 
            {
                _key = value;
            }
        }

        private int _version;
        /// <summary>
        /// 版本
        /// </summary>
        public int Version
        {
            get { return _version; }
            set { _version = value; }
        }
        private T _value;
        /// <summary>
        /// 值
        /// </summary>
        public T Value
        {
            get { return _value; }
            set { _value = value; }
        }
    }
}
