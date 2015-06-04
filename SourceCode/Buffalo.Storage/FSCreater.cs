using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Buffalo.Storage.LocalFileManager;

namespace Buffalo.Storage
{
    /// <summary>
    /// 存储操作创建类
    /// </summary>
    public class FSCreater
    {
        /// <summary>
        /// 创建存储操作类
        /// </summary>
        /// <param name="connString">连接字符串</param>
        /// <returns></returns>
        public static IFileStorage Create(string type,string connString) 
        {
            if (type.Equals("local", StringComparison.CurrentCultureIgnoreCase)) 
            {
                return new LocalFileAdapter(connString);
            }
            throw new Exception("不支持:" + type + "的存储类型");
        }
    }
}
