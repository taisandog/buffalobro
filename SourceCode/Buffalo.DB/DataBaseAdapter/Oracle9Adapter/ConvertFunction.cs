using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace Buffalo.DB.DataBaseAdapter.Oracle9Adapter
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
            ret.Replace("hh", "hh24");
            ret.Replace("ms", "ff3");
            ret.Replace("mm", "mi");
            return ret.ToString();
        }
        /// <summary>
        /// 日期转字符串
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public string DateTimeToString(string dateTime, string format) 
        {
            if (format != null) 
            {
                return "to_char(" + dateTime + ",'" + GetFormat(format) + "')";
            }
            return "to_char(" + dateTime + ",'yyyy-mm-dd hh24-mi-ss.ff3')";
        }

        /// <summary>
        /// 字符串转成日期
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public string StringToDateTime(string value,string format)
        {
            if (format != null)
            {
                return "to_timestamp(" + value + ",'" + GetFormat(format) + "')";
            }
            return "to_timestamp(" + value + ",'yyyy-mm-dd hh24-mi-ss.ff3')";
        }
        /// <summary>
        /// 把数据转成指定类型
        /// </summary>
        /// <param name="value">数据</param>
        /// <param name="dbType">指定类型</param>
        /// <returns></returns>
        public string ConvetTo(string value, DbType type)
        {
            if (type == DbType.AnsiString || type == DbType.AnsiStringFixedLength || type == DbType.String ||
                type == DbType.StringFixedLength) 
            {
                return "TO_CHAR(" + value + ")";
            }else if (type == DbType.Int64 ||
                type == DbType.Decimal || type == DbType.Double || type == DbType.Int32 ||
            type == DbType.Int16 || type == DbType.Double ||
            type == DbType.SByte || type == DbType.Byte || type == DbType.Currency || type == DbType.UInt16
            || type == DbType.UInt32 || type == DbType.UInt64 || type == DbType.VarNumeric || type == DbType.Boolean
                )
            {
                return "TO_NUMBER(" + value + ")";
            }
            else if (type == DbType.DateTime || type == DbType.Time || type == DbType.Date)
            {
                return "TO_NUMBER(" + value + ")";
            }
            return value;
        }
    }
}
