using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DB.DbCommon;
using Buffalo.DB.DataBaseAdapter;

namespace Buffalo.DB.Exceptions
{
    /// <summary>
    /// SQL执行错误
    /// </summary>
    public class SQLRunningException:Exception
    {
        /// <summary>
        /// SQL执行错误
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="innerException">关联异常</param>
        /// <param name="prmList">变量集合</param>
        public SQLRunningException(string sql,ParamList prmList,DBInfo db, Exception innerException)
            : base(GetMessage(sql, prmList, db, innerException), innerException)
        {
            
        }

        

        /// <summary>
        /// 获取输出的信息
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="prmList"></param>
        /// <returns></returns>
        private static string GetMessage(string sql, ParamList prmList,DBInfo db,Exception innerException) 
        {
            StringBuilder sbRet = new StringBuilder(512);
            sbRet.Append("Error:");
            sbRet.AppendLine(innerException.Message);
            sbRet.Append("SQL:");
            sbRet.Append(sql);
            sbRet.Append(";");
            if (prmList != null)
            {
                sbRet.Append(prmList.GetParamString(db, db.ExceptionOption.ShowBinary, db.ExceptionOption.HideTextLength));
            }
            sbRet.AppendLine("");
            return sbRet.ToString();
        }
    }

    /// <summary>
    /// 异常输出信息的配置
    /// </summary>
    public class SQLRunningExceptionOption 
    {
        private bool _showBinary = false;
        /// <summary>
        /// 输出SQL时候是否输出二进制变量值的Hex
        /// </summary>
        public bool ShowBinary
        {
            get { return _showBinary; }
            set { _showBinary = value; }
        }

        private int _hideTextLength = 36;
        /// <summary>
        /// 输出SQL时候设置一个值，当字符串大于这个长度时候则隐藏值
        /// </summary>
        public int HideTextLength
        {
            get { return _hideTextLength; }
            set { _hideTextLength = value; }
        }
    }
}
