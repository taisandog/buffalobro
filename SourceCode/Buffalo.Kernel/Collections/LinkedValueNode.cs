using System;
using System.Collections.Generic;
using System.Text;

namespace Buffalo.Kernel.Collections
{
    public class LinkedValueNode<TKey, TValue> 
    {
        private KeyValuePair<TKey, TValue> _kvp;
        private DateTime _expiredDate;

        /// <summary>
        /// 过期时间
        /// </summary>
        public DateTime ExpiredDate
        {
            get { return _expiredDate; }
            internal set { _expiredDate = value; }
        }
        /// <summary>
        /// 节点
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public LinkedValueNode(TKey key, TValue value) 
        {
            _kvp = new KeyValuePair<TKey, TValue>(key, value);

        }

       /// <summary>
       /// 键
       /// </summary>
        public TKey Key
        {
            get { return _kvp.Key; }
        }
        /// <summary>
        /// 值
        /// </summary>
        public TValue Value
        {
            get { return _kvp.Value; }
        }

        /// <summary>
        /// Key/Value值
        /// </summary>
        public KeyValuePair<TKey, TValue> KeyValue 
        {
            get 
            {
                return _kvp;
            }
        }
    }
}
