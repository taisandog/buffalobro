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
        Query=1,
        [Description("执行语句")]
        Execute=2,
        [Description("其他操作")]
        OtherOper=3,
        [Description("查询缓存")]
        QueryCache=4,
        [Description("缓存服务器异常")]
        CacheException=4,
        [Description("数据库操作异常")]
        DataBaseException=5,
    }
}
