using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Buffalo.WebKernel.JsonUnit
{
    /// <summary>
    /// Json返回结果
    /// </summary>
    public enum JsonStatus:int
    {
        /// <summary>
        /// 错误
        /// </summary>
        Error=0,
        /// <summary>
        /// 成功
        /// </summary>
        Success=1,
        /// <summary>
        /// 异常
        /// </summary>
        Exception=2
    }
}