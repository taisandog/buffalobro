using System;
using System.Collections.Generic;
using System.Text;

namespace Buffalo.Kernel.SmartSellMap
{
    /// <summary>
    /// 组合项
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class CombinationItem<T> where T :ICombinationItem
    {

        /// <summary>
        /// 组合项
        /// </summary>
        /// <param name="previous"></param>
        public CombinationItem(T item,int itemIndex,ICombinationState sender,CombinationItem<T> previous) 
        {
            _item = item;
            _previous = previous;
            _sender = sender;
            _itemIndex = itemIndex;
            if (previous != null)
            {
                TotlePoint = previous.TotlePoint;
            }
        }
        private int _itemIndex;
        /// <summary>
        /// 所属项索引
        /// </summary>
        public int ItemIndex
        {
            get { return _itemIndex; }
        }
        private T _item;

        /// <summary>
        /// 项
        /// </summary>
        public T Item
        {
            get { return _item; }
        }

        private CombinationItem<T> _previous;
        /// <summary>
        /// 上一个
        /// </summary>
        public CombinationItem<T> Previous
        {
            get { return _previous; }
        }

        private ICombinationState _sender;
        /// <summary>
        /// 购买者
        /// </summary>
        public ICombinationState Sender
        {
            get { return _sender; }
        }

        private decimal _totlePoint;
        /// <summary>
        /// 剩余积分
        /// </summary>
        public decimal TotlePoint
        {
            get { return _totlePoint; }
            set { _totlePoint = value; }
        }

        /// <summary>
        /// 返回组合合
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public List<T> ToList() 
        {
            List<T> lst = new List<T>();
            CombinationItem<T> item = this;
            do
            {

                lst.Add(item.Item);
                item = item.Previous;
            } while (item != null);
            return lst;
        }
    }
}
