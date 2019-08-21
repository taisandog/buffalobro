using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DB.DataBaseAdapter.IDbAdapters;
using Buffalo.DB.DataBaseAdapter;
using Buffalo.DB.FaintnessSearchConditions;
using Buffalo.Kernel;

namespace Buffalo.DB.BQLCommon.BQLConditions
{
    public class BQLConditionManager
    {
        /// <summary>
        /// ���ַ���������ָ��������������
        /// </summary>
        /// <param name="lstParam">�ַ�������</param>
        /// <param name="concat">ָ������</param>
        /// <returns></returns>
        private static string ConcatParam(string[] lstParam,string concat) 
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < lstParam.Length; i++) 
            {
                sb.Append(lstParam[i]);
                if (i < lstParam.Length - 1) 
                {
                    sb.Append(concat);
                }
            }
            return sb.ToString();
        }
        /// <summary>
        /// In����(�������Ϊ�գ��򷵻�1=2)
        /// </summary>
        /// <param name="lstParam"></param>
        /// <returns></returns>
        internal static string DoIn(string source, string[] lstParam,DBInfo db) 
        {
            if (CommonMethods.IsCollectionNullOrEmpty(lstParam)) 
            {
                return "1=2";
            }
            return source + " in (" + ConcatParam(lstParam, ",")+")";
        }
        /// <summary>
        /// NotIn����(�������Ϊ�գ��򷵻�1=1)
        /// </summary>
        /// <param name="lstParam"></param>
        /// <returns></returns>
        internal static string DoNotIn(string source, string[] lstParam, DBInfo db)
        {
            if (CommonMethods.IsCollectionNullOrEmpty(lstParam))
            {
                return "1=1";
            }
            return source + " not in (" + ConcatParam(lstParam, ",")+")";
        }

        /// <summary>
        /// Like����
        /// </summary>
        /// <param name="lstParam"></param>
        /// <returns></returns>
        internal static string DoLike(string source, string[] lstParam, DBInfo db)
        {

            return source + " like " + db.CurrentDbAdapter.ConcatString("'%'", lstParam[0], "'%'") ;
        }

        /// <summary>
        /// StarWith����
        /// </summary>
        /// <param name="lstParam"></param>
        /// <returns></returns>
        internal static string DoStarWith(string source, string[] lstParam, DBInfo db)
        {

            return source + " like " + db.CurrentDbAdapter.ConcatString(lstParam[0], "'%'");
        }
        /// <summary>
        /// EndWith����
        /// </summary>
        /// <param name="lstParam"></param>
        /// <returns></returns>
        internal static string DoEndWith(string source, string[] lstParam, DBInfo db)
        {

            return source + " like " + db.CurrentDbAdapter.ConcatString("'%'", lstParam[0]);
        }
        /// <summary>
        /// Between����
        /// </summary>
        /// <param name="lstParam"></param>
        /// <returns></returns>
        internal static string DoBetween(string source, string[] lstParam, DBInfo db)
        {
            return source + " between " + ConcatParam(lstParam, " and ");
        }

        /// <summary>
        /// ȫ�ļ���������
        /// </summary>
        /// <param name="lstParam"></param>
        /// <returns></returns>
        internal static string DoContains(string source, string[] lstParam, DBInfo db)
        {
            IDBAdapter ida = db.CurrentDbAdapter;
            if (!source.Contains("*"))
            {
                source = ida.FormatParam(source);
            }
            return ida.ContainsLike(source,lstParam[0]);
        }
        /// <summary>
        /// ȫ�ļ���������
        /// </summary>
        /// <param name="lstParam"></param>
        /// <returns></returns>
        internal static string DoFreeText(string source, string[] lstParam, DBInfo db)
        {
            IDBAdapter ida = db.CurrentDbAdapter;
            if (!source.Contains("*")) 
            {
                source = ida.FormatParam ( source );
            }
            return ida.FreeTextLike(source, lstParam[0]);
        }
    }
}
