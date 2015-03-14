using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace Buffalo.DB.DataBaseAdapter.MySQL5Adapter
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
            ret.Replace("yy", "%y");
            ret.Replace("yyyy", "%Y");
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
            if (format != null) 
            {
                return "DATE_FORMAT("+dateTime+",'"+GetFormat(format)+"')";
            }
            return "DATE_FORMAT(" + dateTime + ",'%Y-%m-%d %H:%i:%S.%f')";
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
                return "CAST(" + value + " as char)";
            }
            else if (type == DbType.Int64 ||
                 type == DbType.Int32 || type == DbType.Int16  ||type == DbType.SByte)
            {
                return "CAST(" + value + " as SIGNED)";
            }
            else if (type == DbType.UInt32 || type == DbType.UInt64||type == DbType.UInt16||type == DbType.Byte  || type == DbType.Boolean)
            {
                return "CAST(" + value + " as UNSIGNED )";
            }
            else if (type == DbType.Decimal || type == DbType.Double || type == DbType.Double 
                || type == DbType.Currency || type == DbType.VarNumeric) 
            {
                return "CAST(" + value + " as DECIMAL)";
            }
            else if (type == DbType.DateTime || type == DbType.Time || type == DbType.Date)
            {
                return "CAST(" + value + " as DATETIME)";
            }
            return value;
        }

        
        
    }
}
