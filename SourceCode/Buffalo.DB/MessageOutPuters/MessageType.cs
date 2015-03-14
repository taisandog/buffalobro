using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace Buffalo.DB.MessageOutPuters
{
    /// <summary>
    /// 消息类型
    /// </summary>
    public enum MessageType
    {
        [Description("查询数据库")]
        Query,
        [Description("执行语句")]
        Execute,
        [Description("其他操作")]
        OtherOper,
        [Description("查询缓存")]
        QueryCache,
        [Description("缓存服务器异常")]
        CacheException,
    }
}
