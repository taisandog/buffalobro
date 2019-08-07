using System;
using System.Collections.Generic;
using System.Text;

namespace Buffalo.Kernel
{
    public class ExceptionReader
    {
        /// <summary>
        /// 读取异常
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        public static string ReadException(Exception ex) 
        {
            StringBuilder sbRet = new StringBuilder();
            ReadInfo(ex,sbRet);
            return sbRet.ToString();
        }

        /// <summary>
        /// 读取异常信息
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="sbInfo"></param>
        /// <returns></returns>
        private static void ReadInfo(Exception ex, StringBuilder sbInfo) 
        {
            sbInfo.AppendLine(ex.ToString());
            if (ex.InnerException != null) 
            {
                sbInfo.AppendLine("\r\n\r\n InnerException:");
                ReadInfo(ex.InnerException, sbInfo);
            }
        }

    }
}
