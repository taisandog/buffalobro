using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace Buffalo.Data.MySQL
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
        /// ����ת�ַ���
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
                return "CAST(" + value + " as DOUBLE)";
            }
            else if (type == DbType.DateTime || type == DbType.Time || type == DbType.Date)
            {
                return "CAST(" + value + " as DATETIME)";
            }
            return value;
        }

        
        
    }
}
