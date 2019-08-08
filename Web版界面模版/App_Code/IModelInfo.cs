using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Models
{
    public interface IModelInfo
    {
        /// <summary>
        /// 位置字符串
        /// </summary>
        string LocationString
        {
            get;
            set;
        }

        /// <summary>
        /// 是否查询页
        /// </summary>
        bool IsSearch { get; }
    }
}