using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using Buffalo.DB.CommBase;
using Buffalo.DB.DataFillers;
using Buffalo.DB.QueryConditions;
using Buffalo.DB.DbCommon;

namespace Buffalo.DB.DataBaseAdapter.SqlServer2KAdapter
{
    /// <summary>
    /// �α��ҳ
    /// </summary>
    public class CursorPageCutter
    {
        private static bool isCheck = false;//�Ƿ��Ѿ������˴洢����
        /// <summary>
        /// ��ҳ�洢��������
        /// </summary>
        private const string ProcName = "proc_splitpage";

        /// <summary>
        /// ��ʼ����ҳ�洢����
        /// </summary>
        private static void InitProc(DataBaseOperate oper)
        {
            if (isCheck)
            {
                return;
            }
            string sql = "select * from sysobjects where [xtype]='p' and [name]='" + ProcName + "'";
            bool hasProc = false;
            IDataReader reader = null;
            try
            {
                reader = oper.Query(sql, null,null);
                if (reader.Read())
                {
                    hasProc = true;
                }
                reader.Close();
                if (!hasProc)
                {
                    oper.Execute(GetProcCode(), null,null);
                }
            }
            finally
            {
                oper.AutoClose();

            }
            isCheck = true;
        }
        /// <summary>
        /// ��ȡ�α��ҳ�Ĵ洢���̵Ĵ���
        /// </summary>
        /// <returns></returns>
        private static string GetProcCode() 
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("CREATE procedure " + ProcName + "   \n");//�α��ҳ�洢���̣��޽���
            sb.Append("@sql ntext, --Ҫִ�е�sql���\n");
            sb.Append("@currentIndex int,  --Ҫ��ʾ��ҳ��\n");
            sb.Append("@pagesize int,  --ÿҳ�Ĵ�С\n");
            sb.Append("@maxRecords int  --���ļ�¼��\n");
            sb.Append("\n");
            sb.Append("as\n");
            sb.Append("declare @p1 int\n");
            sb.Append("declare @total int\n");
            sb.Append("if (@maxRecords>0) \n");
            sb.Append("begin \n");
            sb.Append("	set rowcount @maxRecords \n");
            sb.Append("end \n");
            sb.Append("exec sp_cursoropen @p1 output,@sql,@scrollopt=1,@ccopt=1,@total=@total output\n");
            sb.Append("select @total\n");
            sb.Append("exec sp_cursorfetch @p1,16,@currentIndex,@pagesize \n");
            sb.Append("exec sp_cursorclose @p1\n");
            sb.Append("if (@maxRecords>0) \n");
            sb.Append("begin \n");
            sb.Append("	set rowcount 0 \n");
            sb.Append("end \n");

            sb.Append("GO\n");
            return sb.ToString();
        }

        

        /// <summary>
        /// ��ѯ���ҷ��ؼ���(�α��ҳ)
        /// </summary>
        /// <param name="sql">Ҫ��ѯ��SQL���</param>
        /// <param name="objPage">��ҳ����</param>
        /// <param name="oper">���ݿ����</param>
        /// <returns></returns>
        public static IDataReader Query(string sql, PageContent objPage, DataBaseOperate oper)
        {
            ParamList lstParams = new ParamList();
            lstParams.AddNew("@sql", DbType.AnsiString, sql);
            lstParams.AddNew("@currentIndex", DbType.Int32, objPage.GetStarIndex() + 1);
            lstParams.AddNew("@pagesize", DbType.Int32, objPage.PageSize);
            lstParams.AddNew("@maxRecords", DbType.Int64, objPage.MaxSelectRecords);
            //lstParams.AddNew("@@total", DbType.Int64, 0, ParameterDirection.Output);
            IDataReader reader = null;

            InitProc(oper);
            reader = oper.Query(ProcName, lstParams, CommandType.StoredProcedure,null);
            if (reader.NextResult())//�ڶ��������Ϊ��ѯ��¼��
            {
                if (objPage.IsFillTotalRecords)
                {
                    if (reader.Read())
                    {
                        int totalRecord = reader.GetInt32(0);
                        objPage.TotalRecords = totalRecord;
                        //int totalPage = (int)Math.Ceiling((double)objPage.TotalRecords / (double)objPage.PageSize);
                        //objPage.TotalPage = totalPage;
                        if (objPage.CurrentPage >= objPage.TotalPage - 1)
                        {
                            objPage.CurrentPage = objPage.TotalPage - 1;
                        }
                    }
                }
            }

            if (reader.NextResult())//�����������Ϊ������¼
            {
                return reader;
            }

            return null;

        }
        /// <summary>
        /// ��ѯ���ҷ���DataSet(�α��ҳ)
        /// </summary>
        /// <param name="sql">Ҫ��ѯ��SQL���</param>
        /// <param name="objPage">��ҳ����</param>
        /// <param name="oper">���ݿ����</param>
        /// <param name="curType">ӳ���ʵ������(����û����ݿ��ԭ���������Ϊnull)</param>
        /// <returns></returns>
        public static DataTable QueryDataTable(string sql, PageContent objPage, DataBaseOperate oper,Type curType)
        {
            DataTable ret = new DataTable();
            ParamList lstParams = new ParamList();
            lstParams.AddNew("@sql", DbType.AnsiString, sql);
            lstParams.AddNew("@currentIndex", DbType.Int32, objPage.GetStarIndex() + 1);
            lstParams.AddNew("@pagesize", DbType.Int32, objPage.PageSize);
            lstParams.AddNew("@maxRecords", DbType.Int64, objPage.MaxSelectRecords);
            IDataReader reader = null;
            try
            {
                InitProc(oper);
                reader = oper.Query(ProcName, lstParams, CommandType.StoredProcedure,null);
                if (reader.NextResult())//�ڶ��������Ϊ��ѯ���
                {
                    if (curType == null)
                    {
                        ret = CacheReader.GenerateDataTable(reader, "newDt", false);
                    }
                    else 
                    {
                        ret = CacheReader.GenerateDataTable(reader, "newDt",curType, false);
                    }
                }
                if (reader.NextResult())//�����������Ϊ������
                {
                    if (reader.Read())
                    {
                        int totalRecord = reader.GetInt32(0);
                        objPage.TotalRecords = totalRecord;
                        //int totalPage = (int)Math.Ceiling((double)objPage.TotalRecords / (double)objPage.PageSize);
                        //objPage.TotalPage = totalPage;
                        if (objPage.CurrentPage >= objPage.TotalPage - 1)
                        {
                            objPage.CurrentPage = objPage.TotalPage - 1;
                        }
                    }
                }
            }
            finally
            {
                reader.Close();
            }
            return ret;
        }
    }
}
