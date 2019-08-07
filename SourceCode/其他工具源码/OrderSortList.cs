using System;
using System.Collections.Generic;
using System.Text;

namespace Buffalo.Kernel
{
    public class OrderSortList<TKey, TValue> : List<KeyValuePair<TKey, TValue>>
        where TKey : IComparable
    {
        private SortListOrderType _orderType;
        /// <summary>
        /// 排序方式
        /// </summary>
        public SortListOrderType OrderType
        {
            get { return _orderType; }
        }

        /// <summary>
        /// 二分法的
        /// </summary>
        /// <param name="orderType"></param>
        public OrderSortList(SortListOrderType orderType)
        {
            _orderType = orderType;
        }
        public OrderSortList()
            :this(SortListOrderType.ASC)
        {
            
        }
        /// <summary>
        /// 添加一个项
        /// </summary>
        /// <param name="kp"></param>
        public new void Add(KeyValuePair<TKey, TValue> kp)
        {
            int iPos = 0;
            int middle = BinarySearch(kp.Key, ref iPos);

            if (middle >0) 
            {
                throw new System.ArgumentException("已存在相同键的项");
            }

            this.Insert(iPos, kp);
        }

        /// <summary>
        /// 添加一个项
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        public void Add(TKey key, TValue value) 
        {
            Add(new KeyValuePair<TKey,TValue>(key,value));
        }

        /// <summary>
        /// 设置一个项
        /// </summary>
        /// <param name="kp"></param>
        public void Set(KeyValuePair<TKey, TValue> kp)
        {
            int iPos = 0;
            int middle = BinarySearch(kp.Key, ref iPos);


            this.Insert(iPos, kp);
        }
        /// <summary>
        /// 设置一个项
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        public void Set(TKey key, TValue value)
        {
            Set(new KeyValuePair<TKey, TValue>(key, value));
        }

        /// <summary>
        /// 获取索引的值
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public new TValue this[int index] 
        {
            get 
            {
                return base[index].Value;
            }
        }

        /// <summary>
        /// 通过键获取值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool TryGetByKey(TKey key,out TValue value) 
        {
            int pos=0;
            int middle = BinarySearch(key, ref pos);
            value = default(TValue);
            if (middle < 0) 
            {
                return false;
            }
            value = base[middle].Value;
            return true;
        }

        /// <summary>
        /// 查找
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private int BinarySearch(TKey key,ref int iPos) 
        {
            int start=0;
            int end = Count-1;
            
            while (start <= end)
            {
                int middle = (start + end) / 2;
                int res = key.CompareTo(base[middle].Key);
                if (res==0)
                {
                    iPos = middle;
                    return middle;
                }
                if ((res>0 && _orderType == SortListOrderType.ASC) ||
                    (res<0 && _orderType == SortListOrderType.DESC))
                {
                    start = middle + 1;
                    iPos = middle + 1;
                }
                else
                {
                    end = middle - 1;
                    iPos=middle;
                }
            }
            return -1;
        }
        
    }

    public enum SortListOrderType
    {
        /// <summary>
        /// 顺序
        /// </summary>
        ASC,
        /// <summary>
        /// 倒序
        /// </summary>
        DESC
    }
}
