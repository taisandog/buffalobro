using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Buffalo.Storage.LocalFileManager;
using Buffalo.Storage.QCloud.CosApi;
using Buffalo.Storage.AliCloud.OssAPI;
using Buffalo.Storage.HW.OBS;
using Buffalo.Storage.AWS.S3;

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
            else if (type.Equals("COS", StringComparison.CurrentCultureIgnoreCase)) 
            {
                return new QCloudAdapter(connString);
            }
            else if (type.Equals("OSS", StringComparison.CurrentCultureIgnoreCase))
            {
                return new OSSAdapter(connString);
            }
            else if (type.Equals("OBS", StringComparison.CurrentCultureIgnoreCase))
            {
                return new HWOBSAdapter(connString);
            }
            else if (type.Equals("AWSS3", StringComparison.CurrentCultureIgnoreCase))
            {
                return new AWSS3Adapter(connString);
            }
            throw new Exception("不支持:" + type + "的存储类型");
        }
    }
}
