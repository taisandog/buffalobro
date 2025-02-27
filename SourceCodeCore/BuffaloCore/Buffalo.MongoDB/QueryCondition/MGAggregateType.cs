using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buffalo.MongoDB.QueryCondition
{
    /// <summary>
    /// 聚合函数
    /// </summary>
    public enum MGAggregateType:int
    {
        /// <summary>
        /// 无
        /// </summary>
        [System.ComponentModel.Description("")]
        None =0,
        /// <summary>
        /// 平均值
        /// </summary>
        [System.ComponentModel.Description("$avg")]
        Avg=1,
        /// <summary>
        /// 最大值
        /// </summary>
        [System.ComponentModel.Description("$max")]
        Max = 2,
        /// <summary>
        /// 最小值
        /// </summary>
        [System.ComponentModel.Description("$min")]
        Min = 3,
        /// <summary>
        /// 合计
        /// </summary>
        [System.ComponentModel.Description("$sum")]
        Sum = 4,
        /// <summary>
        /// 只显示
        /// </summary>
        [System.ComponentModel.Description("$push")]
        Push = 5,
        /// <summary>
        /// 去重显示
        /// </summary>
        [System.ComponentModel.Description("$addToSet")]
        AddToSet = 6,
    }
}
