using System;
using System.Collections.Generic;
using System.Text;

namespace Buffalo.DB.PropertyAttributes
{
    /// <summary>
    /// 实体的属性类型
    /// </summary>
    public enum EntityPropertyType:uint
    {
        /// <summary>
        /// 普通属性
        /// </summary>
        Normal=1,
        /// <summary>
        /// 主键属性
        /// </summary>
        PrimaryKey=2,
        /// <summary>
        /// 版本号属性
        /// </summary>
        Version=4,
        /// <summary>
        /// 自动增长字段
        /// </summary>
        Identity=8
    }
}
