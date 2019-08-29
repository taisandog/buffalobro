using System;
using System.Collections.Generic;

using System.Text;

namespace Buffalo.Storage
{
    /// <summary>
    /// 存储设备连接异常
    /// </summary>
    public class StorageConnectExceptin:Exception
    {
        public StorageConnectExceptin(string message) :base(message)
        { }
    }
}
