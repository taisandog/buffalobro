using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Buffalo.Kernel.Replacer;

namespace Buffalo.DB.DataBaseAdapter.AccessAdapter
{
    /// <summary>
    /// 数值转换函数
    /// </summary>
    public class ConvertFunction :Buffalo.DB.DataBaseAdapter.IDbAdapters.IConvertFunction
    {
        
        /// <summary>
        /// 获取转换函数
        /// </summary>
        /// <param name="dbType"></param>
        /// <returns></returns>
        private string GetConvertMethod(DbType dbType) 
        {
            switch (dbType)
            {

                case DbType.AnsiString:
                case DbType.AnsiStringFixedLength:
                case DbType.String:
                case DbType.StringFixedLength:
                    return "CStr";

                case DbType.Boolean:
                    return "CBool";

                case DbType.Byte:
                    return "CByte";

                case DbType.Currency:
                    return "CCur";

                case DbType.Date:
                case DbType.DateTime:
                case DbType.DateTime2:
                case DbType.DateTimeOffset:
                case DbType.Time:
                    return "CDate";

                case DbType.Single:
                    return "CSng";
                case DbType.Decimal:
                    return "CDec";

                case DbType.Double:
                    return "CDbl";

                case DbType.SByte:
                case DbType.Int16:
                    return "CInt";

                case DbType.UInt16:
                case DbType.Int32:
                case DbType.Int64:
                case DbType.UInt32:
                case DbType.UInt64:
                case DbType.VarNumeric:
                    return "CLng";
                default:
                    return null;

            }
        }

        /// <summary>
        /// 日期转字符串
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public string DateTimeToString(string dateTime, string format) 
        {
            string sfor = format;
            sfor = sfor.Replace("YYYY", "yyyy");
            sfor = sfor.Replace("m", "n");
            sfor = sfor.Replace("M", "m");
            return "Format$(" + dateTime + ",\"" + sfor + "\")";

        }

        /// <summary>
        /// 字符串转成日期
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public string StringToDateTime(string value,string format)
        {
            //string sfor = format;
            //sfor = sfor.Replace("YYYY", "yyyy");
            //sfor = sfor.Replace("m", "n");
            //sfor = sfor.Replace("M", "m");

            return "cdata("+value+")";
        }

        /// <summary>
        /// 把数据转成指定类型
        /// </summary>
        /// <param name="value">数据</param>
        /// <param name="dbType">指定类型</param>
        /// <returns></returns>
        public string ConvetTo(string value, DbType dbType) 
        {
            string method = GetConvertMethod(dbType);
            if (method == null) 
            {
                return "";
            }
            return method + "(" + value + ")";
            
        }
    }
}
