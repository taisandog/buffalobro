using System;
using System.Collections.Generic;
using System.Text;

namespace Buffalo.DB.BQLCommon.BQLConditionCommon
{
    /// <summary>
    /// 运算符优先级接口
    /// </summary>
    public interface IOperatorPriorityLevel
    {
        /// <summary>
        /// 优先级
        /// </summary>
        int PriorityLevel { get;}
    }
}
