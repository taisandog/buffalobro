using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DB.EntityInfos;
using System.Data;
using Buffalo.DB.DbCommon;

namespace Buffalo.DB.DataBaseAdapter.Oracle9Adapter
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
            if (!string.IsNullOrEmpty(info.ParamInfo.SequenceName)) 
            {
                return info.ParamInfo.SequenceName;
            }
            return GetDefaultName(info.BelongInfo.TableName, info.ParamInfo.ParamName);
        }

        /// <summary>
        /// ��ȡĬ�ϵ�������
        /// </summary>
        /// <param name="tableName">��</param>
        /// <param name="paramName">�ֶ�</param>
        /// <returns></returns>
        public static string GetDefaultName(string tableName, string paramName) 
        {
            StringBuilder sbSeqName = new StringBuilder(20);
            //sbSeqName.Append("s_");
            sbSeqName.Append(tableName);
            sbSeqName.Append("_");
            sbSeqName.Append(paramName);
            sbSeqName.Replace(" ", "");
            string str=sbSeqName.ToString();
            if(str.Length>30)
            {
                str=str.Substring(0,30);
            }
            return str;
        }
        /// <summary>
        /// ��ʼ������
        /// </summary>
        /// <param name="seqName"></param>
        public static string GetInitSequence(string seqName, DataBaseOperate oper)
        {

            if (!IsSequenceExists(seqName, oper)) //�ж��Ƿ��Ѿ���������
            {
                string sql = "CREATE SEQUENCE \"" + seqName + "\" INCREMENT BY 1 START WITH 1  NOMAXVALUE  NOCYCLE  NoCACHE";//��������
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
        internal static bool IsSequenceExists(string seqName, DataBaseOperate oper)
        {
            string sql = "select SEQUENCE_NAME from user_sequences where SEQUENCE_NAME='" + seqName + "'";
            
            IDataReader reader = null;
            int count = 0;
            try
            {
                reader = oper.Query(sql, null,null);
                if (reader.Read())
                {
                    if (!reader.IsDBNull(0)) 
                    {
                        count = 1;
                    }
                   
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
