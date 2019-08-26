using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace Buffalo.Data.SQLite
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
            ret.Replace("yy", "%Y");
            ret.Replace("MM", "%m");
            ret.Replace("dd", "%d");
            ret.Replace("hh", "%H");
            ret.Replace("mm", "%M");
            ret.Replace("ss.ms", "%f");
            ret.Replace("ss", "%S");
            
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
                return "strftime('" + GetFormat(format) + "'," + dateTime + ")";
            }
            return "strftime('%Y-%m-%d %H:%M:%f'," + dateTime + ")";
        }

        /// <summary>
        /// 字符串转成日期
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public string StringToDateTime(string value,string format)
        {
            return "datetime(" + value + ")";
        }

        /// <summary>
        /// 把数据转成指定类型
        /// </summary>
        /// <param name="value">数据</param>
        /// <param name="dbType">指定类型</param>
        /// <returns></returns>
        public string ConvetTo(string value, DbType dbType) 
        {
            string typeName = GetSQLType(dbType);
            //SqlDbType sdb = (SqlDbType)DbAdapterLoader.CurrentDbAdapter.ToCurrentDbType(dbType);
            return "cast(" + value + " as " + typeName + ")";
        }

        /// <summary>
        /// 获取SQL语句中的数值类型
        /// </summary>
        /// <param name="dbType"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        private string GetSQLType(DbType dbType)
        {
            //int type = ToRealDbType(dbType, length);            
            //SqliteType stype = (SqlDbType)type;            
            switch (dbType)
            {
                case DbType.AnsiString:
                case DbType.AnsiStringFixedLength:
                        return "VARCHAR";
                case DbType.Binary:
                    return "BLOB";
                case DbType.Boolean:
                    return "BOOLEAN";
                case DbType.Byte:
                case DbType.Int16:
                case DbType.Int32:
                case DbType.UInt16:
                case DbType.SByte:
                    return "INTEGER";
                case DbType.Decimal:
                case DbType.Currency:
                case DbType.Double:
                case DbType.Int64:
                case DbType.UInt32:
                case DbType.UInt64:
                case DbType.VarNumeric:
                    return "REAL";
                case DbType.Date:
                    return "DATE";
                case DbType.DateTime2:
                case DbType.DateTimeOffset:
                case DbType.DateTime:
                    return "TIMESTAMP";
                case DbType.Guid:
                    return "VARCHAR";
                case DbType.Single:
                    return "FLOAT";
                case DbType.String:
                case DbType.StringFixedLength:
                    return "NVARCHAR";
                case DbType.Time:
                    return "TIME";
                default:
                    return "NUMERIC";
            }

        }
    }
}
