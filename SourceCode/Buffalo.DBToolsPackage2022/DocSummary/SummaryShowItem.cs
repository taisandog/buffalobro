using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;


namespace Buffalo.DBTools.DocSummary
{
    /// <summary>
    /// 注释显示的项数
    /// </summary>
    public enum SummaryShowItem:int
    {
        /// <summary>
        /// 变量类型
        /// </summary>
        [Description("变量类型")]
        TypeName=1,
        /// <summary>
        /// 变量名
        /// </summary>
        [Description("变量名")]
        MemberName=2,
        /// <summary>
        /// 注释
        /// </summary>
        [Description("注释")]
        Summary=4,
        /// <summary>
        /// 所有
        /// </summary>
        [Description("所有")]
        All=TypeName|MemberName|Summary
    }
}
