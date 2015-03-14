using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace Buffalo.Data.PostgreSQL
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
            ret.Replace("yyyy", "YYYY");
            ret.Replace("yy", "YY");
            ret.Replace("MM", "MM");
            ret.Replace("dd", "DD");
            ret.Replace("hh", "HH24");
            ret.Replace("mm", "MI");
            ret.Replace("ss", "SS");
            ret.Replace("ms", "MS");
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
            return "to_char(" + dateTime + ",'YYYY-MM-DD HH24:MI:SS.MS')";
        }

        /// <summary>
        /// 字符串转成日期
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public string StringToDateTime(string value,string format) 
        {
            return "timestamp(" + value + ")";
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
                return "CAST(" + value + " as varchar)";
            }
            else if (type == DbType.Int64 ||
                 type == DbType.Int32 || type == DbType.Int16  ||type == DbType.SByte)
            {
                return "CAST(" + value + " as float8)";
            }
            else if (type == DbType.UInt32 || type == DbType.UInt64||type == DbType.UInt16||type == DbType.Byte  || type == DbType.Boolean)
            {
                return "CAST(" + value + " as float8 )";
            }
            else if (type == DbType.Decimal || type == DbType.Double || type == DbType.Double 
                || type == DbType.Currency || type == DbType.VarNumeric) 
            {
                return "CAST(" + value + " as numeric)";
            }
            else if (type == DbType.DateTime || type == DbType.Time || type == DbType.Date)
            {
                return "CAST(" + value + " as timestamp)";
            }
            return value;
        }

        
        
    }
}
