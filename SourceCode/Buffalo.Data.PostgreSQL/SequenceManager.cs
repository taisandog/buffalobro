using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DB.EntityInfos;
using System.Data;
using Buffalo.DB.DbCommon;
using Buffalo.DB.PropertyAttributes;

namespace Buffalo.Data.PostgreSQL
{
    /// <summary>
    /// �������й���
    /// </summary>
    public class SequenceManager
    {
        //private static Dictionary<string, bool> dicSequence = new Dictionary<string, bool>();//�����Ѿ���ʼ���ļ���

        /// <summary>
        /// ��ȡ�����Ե�����
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public static string GetSequenceName(EntityPropertyInfo info) 
        {

            return Buffalo.DB.DataBaseAdapter.Oracle9Adapter.SequenceManager.GetSequenceName(info);
        }


        /// <summary>
        /// ��ȡĬ�ϵ�������
        /// </summary>
        /// <param name="tableName">��</param>
        /// <param name="paramName">�ֶ�</param>
        /// <returns></returns>
        internal static string GetDefaultName(string tableName, string paramName)
        {


            return Buffalo.DB.DataBaseAdapter.Oracle9Adapter.SequenceManager.GetDefaultName(tableName,paramName);
        }

        /// <summary>
        /// ��ʼ������
        /// </summary>
        /// <param name="seqName"></param>
        public static string GetInitSequence(string seqName,EntityParam prm, DataBaseOperate oper)
        {
            //string dbType = oper.DBInfo.CurrentDbAdapter.DBTypeToSQL(prm.SqlType,4,prm.AllowNull);

            if (!IsSequenceExists(seqName, oper)) //�ж��Ƿ��Ѿ���������
            {
                string sql = "CREATE SEQUENCE \"" + seqName + "\" INCREMENT 1 MINVALUE 1 MAXVALUE 9223372036854775807 START 1 CACHE 1";//��������
                return sql;
            }
            return null;


        }

        /// <summary>
        /// ��ȡ�����Ƿ����
        /// </summary>
        /// <param name="seqName">������</param>
        /// <param name="oper">���ݿ�����</param>
        /// <returns></returns>
        private static bool IsSequenceExists(string seqName, DataBaseOperate oper)
        {
            string sql = "select count(*) from information_schema.sequences where SEQUENCE_NAME='" + seqName + "'";

            IDataReader reader = null;
            int count = 0;
            try
            {

                reader = oper.Query(sql, null,null);
                if (reader.Read())
                {
                    count = Convert.ToInt32(reader[0]);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("��ѯ����ʱ����ִ���:" + ex.Message);
            }
            finally
            {
                reader.Close();
            }
            return count > 0;
        }

    }
}
