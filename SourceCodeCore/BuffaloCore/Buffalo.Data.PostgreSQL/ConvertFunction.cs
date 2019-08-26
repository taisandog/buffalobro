using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace Buffalo.Data.PostgreSQL
{
    /// <summary>
    /// ��ֵת������
    /// </summary>
    public class ConvertFunction :Buffalo.DB.DataBaseAdapter.IDbAdapters.IConvertFunction
    {

        /// <summary>
        /// ��ʽ���ַ���
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
        /// ����ת�ַ���
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
        /// �ַ���ת������
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public string StringToDateTime(string value,string format) 
        {
            return "timestamp(" + value + ")";
        }

        /// <summary>
        /// ������ת��ָ������
        /// </summary>
        /// <param name="value">����</param>
        /// <param name="dbType">ָ������</param>
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
