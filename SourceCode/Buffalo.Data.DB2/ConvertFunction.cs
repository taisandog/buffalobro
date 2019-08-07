using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using IBM.Data.DB2;

namespace Buffalo.Data.DB2
{
    /// <summary>
    /// 数值转换函数
    /// </summary>
    public class ConvertFunction :Buffalo.DB.DataBaseAdapter.IDbAdapters.IConvertFunction
    {

        /// <summary>
        /// 格式化字符串
        /// </summary>
        /// <returns></returns>
        private static string GetFormat(string format)
        {
            StringBuilder ret = new StringBuilder(format);
            ret.Replace("%", "%%");
            ret.Replace("yyyy", "%Y");
            ret.Replace("yy", "%y");
            ret.Replace("MM", "%m");
            ret.Replace("dd", "%d");
            ret.Replace("hh", "%H");
            ret.Replace("mm", "%i");
            ret.Replace("ss", "%s");
            ret.Replace("ms", "%f");
            return ret.ToString();
        }

        /// <summary>
        /// 日期转字符串
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public string DateTimeToString(string dateTime, string format) 
        {
            if (!string.IsNullOrEmpty(format)) 
            {
                return "to_char(" + dateTime + ",'" + format.ToLower().Replace("hh","hh24") + "')";
            }
            return "to_char("+dateTime+")";
        }

        /// <summary>
        /// 字符串转成日期
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public string StringToDateTime(string value,string format)
        {
            return "TIMESTAMP('" + value + "')";
        }

        /// <summary>
        /// 把数据转成指定类型
        /// </summary>
        /// <param name="value">数据</param>
        /// <param name="dbType">指定类型</param>
        /// <returns></returns>
        public string ConvetTo(string value, DbType dbType) 
        {
            DB2Type sdb = (DB2Type)dbType;
            return "cast("+value+" as "+sdb+")";
        }
    }
}
