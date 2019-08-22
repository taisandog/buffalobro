using System;
using System.Collections.Generic;
using System.Text;

namespace Buffalo.ArgCommon
{
    public enum ResaultType
    {
        /// <summary>
        /// 无
        /// </summary>
        None=0,
        /// <summary>
        /// 成功
        /// </summary>
        Success = 1,
        /// <summary>
        /// 错误
        /// </summary>
        Fault = 2,
        /// <summary>
        /// 异常
        /// </summary>
        Exception=3,
        /// <summary>
        /// 会话过期
        /// </summary>
        Timeout=4,
    }
}
