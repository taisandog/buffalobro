using System;
using System.Collections.Generic;

using System.Text;

namespace Buffalo.Kernel.SmartSellMap
{
    public class SellCombination<T> where T : ICombinationItem
    {
        List<T> _lstItems;
        Queue<CombinationItem<T>> _queItems = null;

        public Queue<CombinationItem<T>> QueItems
        {
            get { return _queItems; }
        }

        /// <summary>
        /// 销售组合
        /// </summary>
        /// <param name="sender">购买者</param>
        /// <param name="lstItems">组合集合</param>
        public SellCombination(IList<T> lstItems) 
        {
            _lstItems = new List<T>(lstItems.Count);
            _lstItems.AddRange(lstItems);
            SortItem();
        }

        /// <summary>
        /// 按消耗倒序排列
        /// </summary>
        private void SortItem() 
        {
            for (int i = 0; i < _lstItems.Count - 1; i++)
            {
                for (int j = i + 1; j < _lstItems.Count; j++)
                {
                    if (_lstItems[i].GetConsume() < _lstItems[j].GetConsume())
                    {
                        T tmp = _lstItems[i];
                        _lstItems[i] = _lstItems[j];
                        _lstItems[j] = tmp;
                    }
                }
            }
        }

        public CombinationItem<T> GetMaxPoint(ICombinationState sender) 
        {
            DoCombination(sender);
            if (_queItems.Count == 0) 
            {
                return null;
            }
            CombinationItem<T> maxPoint = _queItems.Peek();
            foreach (CombinationItem<T> obj in _queItems) 
            {
                if (obj.TotlePoint > maxPoint.TotlePoint) 
                {
                    maxPoint = obj;
                }
            }
            return maxPoint;
        }

        public Queue<CombinationItem<T>> GetAllPoint(ICombinationState sender)
        {
            DoCombination(sender);
            
            return _queItems;
        }
        Stack<CombinationItem<T>> _stk;
        /// <summary>
        /// 开始组合
        /// </summary>
        private void DoCombination(ICombinationState sender) 
        {
            _queItems = new Queue<CombinationItem<T>>();
            _stk = new Stack<CombinationItem<T>>();

            for (int i = 0;  i < _lstItems.Count; i++) 
            {
                ICombinationState curSender = sender.CopyCombination();
                CombinationItem<T> item = new CombinationItem<T>(_lstItems[i],i, curSender, null);
                _stk.Push(item);
            }
            ToCombination();

        }

        

        /// <summary>
        /// 组合
        /// </summary>
        /// <param name="curItem">当前项</param>
        /// <param name="itemIndex">项索引</param>
        private void ToCombination() 
        {
            while (_stk.Count > 0) 
            {
                CombinationItem<T> item = _stk.Pop();
                ICombinationState curSender = item.Sender;
                if (curSender.DoBuy(item.Item))
                {
                    item.TotlePoint += item.Item.GetPoint();
                    if (curSender.LeftMoney == 0)
                    {
                        _queItems.Enqueue(item);
                    }
                    else 
                    {
                        for (int i = item.ItemIndex; i < _lstItems.Count; i++)
                        {
                            ICombinationState newSender = curSender.CopyCombination();
                            CombinationItem<T> newitem = new CombinationItem<T>(_lstItems[i], i, newSender, item);
                            _stk.Push(newitem);
                        }
                    }
                }
            }

        }


    }
}
