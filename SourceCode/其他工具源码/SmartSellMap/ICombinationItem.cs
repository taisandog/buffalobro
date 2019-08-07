using System;
using System.Collections.Generic;

using System.Text;

namespace Buffalo.Kernel.SmartSellMap
{
    /// <summary>
    /// 组合项
    /// </summary>
    public interface ICombinationItem
    {
        /// <summary>
        /// 获取消耗
        /// </summary>
        /// <returns></returns>
        decimal GetConsume();

        /// <summary>
        /// 获取得分
        /// </summary>
        /// <returns></returns>
        decimal GetPoint();



    }
}
