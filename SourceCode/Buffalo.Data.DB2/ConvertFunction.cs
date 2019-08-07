using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using IBM.Data.DB2;

namespace Buffalo.Data.DB2
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
            if (!string.IsNullOrEmpty(format)) 
            {
                return "to_char(" + dateTime + ",'" + format.ToLower().Replace("hh","hh24") + "')";
            }
            return "to_char("+dateTime+")";
        }

        /// <summary>
        /// �ַ���ת������
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public string StringToDateTime(string value,string format)
        {
            return "TIMESTAMP('" + value + "')";
        }

        /// <summary>
        /// ������ת��ָ������
        /// </summary>
        /// <param name="value">����</param>
        /// <param name="dbType">ָ������</param>
        /// <returns></returns>
        public string ConvetTo(string value, DbType dbType) 
        {
            DB2Type sdb = (DB2Type)dbType;
            return "cast("+value+" as "+sdb+")";
        }
    }
}
