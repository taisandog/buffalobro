using System;
using System.Collections.Generic;
using System.Text;

namespace Buffalo.DB.DataBaseAdapter.IDbAdapters
{
    /// <summary>
    /// 数据库检查事件
    /// </summary>
    public enum CheckEvent
    {
        /// <summary>
        /// 表创建
        /// </summary>
        TableBeginCreate,

        /// <summary>
        /// 表创建后
        /// </summary>
        TableCreated,

        /// <summary>
        /// 表检查
        /// </summary>
        TablenBeginCheck,

        /// <summary>
        /// 表检查后
        /// </summary>
        TableChecked,
        /// <summary>
        /// 关系检查
        /// </summary>
        RelationBeginCheck,

        /// <summary>
        /// 关系检查后
        /// </summary>
        RelationChecked,

        /// <summary>
        /// 主键检查
        /// </summary>
        PrimaryChecke,
    }
}
