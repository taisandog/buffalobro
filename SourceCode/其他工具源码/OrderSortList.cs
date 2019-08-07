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
        /// ����ʽ
        /// </summary>
        public SortListOrderType OrderType
        {
            get { return _orderType; }
        }

        /// <summary>
        /// ���ַ���
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
        /// ���һ����
        /// </summary>
        /// <param name="kp"></param>
        public new void Add(KeyValuePair<TKey, TValue> kp)
        {
            int iPos = 0;
            int middle = BinarySearch(kp.Key, ref iPos);

            if (middle >0) 
            {
                throw new System.ArgumentException("�Ѵ�����ͬ������");
            }

            this.Insert(iPos, kp);
        }

        /// <summary>
        /// ���һ����
        /// </summary>
        /// <param name="key">��</param>
        /// <param name="value">ֵ</param>
        public void Add(TKey key, TValue value) 
        {
            Add(new KeyValuePair<TKey,TValue>(key,value));
        }

        /// <summary>
        /// ����һ����
        /// </summary>
        /// <param name="kp"></param>
        public void Set(KeyValuePair<TKey, TValue> kp)
        {
            int iPos = 0;
            int middle = BinarySearch(kp.Key, ref iPos);


            this.Insert(iPos, kp);
        }
        /// <summary>
        /// ����һ����
        /// </summary>
        /// <param name="key">��</param>
        /// <param name="value">ֵ</param>
        public void Set(TKey key, TValue value)
        {
            Set(new KeyValuePair<TKey, TValue>(key, value));
        }

        /// <summary>
        /// ��ȡ������ֵ
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
        /// ͨ������ȡֵ
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
        /// ����
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
        /// ˳��
        /// </summary>
        ASC,
        /// <summary>
        /// ����
        /// </summary>
        DESC
    }
}
