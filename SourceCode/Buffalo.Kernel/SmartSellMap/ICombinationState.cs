using System;
using System.Collections.Generic;

using System.Text;

namespace Buffalo.Kernel.SmartSellMap
{
    /// <summary>
    /// 组合状态拷贝
    /// </summary>
    public interface ICombinationState
    {
        ICombinationState CopyCombination();
        /// <summary>
        /// 购买并返回是否成功
        /// </summary>
        /// <returns></returns>
        bool DoBuy(ICombinationItem item);

        decimal LeftMoney{get;}
    }
}
