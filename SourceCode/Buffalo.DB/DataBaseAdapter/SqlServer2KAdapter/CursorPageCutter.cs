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
    /// 游标分页
    /// </summary>
    public class CursorPageCutter
    {
        private static bool isCheck = false;//是否已经检查过此存储过程
        /// <summary>
        /// 分页存储过程名称
        /// </summary>
        private const string ProcName = "proc_splitpage";

        /// <summary>
        /// 初始化分页存储过程
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
        /// 获取游标分页的存储过程的代码
        /// </summary>
        /// <returns></returns>
        private static string GetProcCode() 
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("CREATE procedure " + ProcName + "   \n");//游标分页存储过程（邹建）
            sb.Append("@sql ntext, --要执行的sql语句\n");
            sb.Append("@currentIndex int,  --要显示的页码\n");
            sb.Append("@pagesize int,  --每页的大小\n");
            sb.Append("@maxRecords int  --最大的记录数\n");
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
        /// 查询并且返回集合(游标分页)
        /// </summary>
        /// <param name="sql">要查询的SQL语句</param>
        /// <param name="objPage">分页对象</param>
        /// <param name="oper">数据库对象</param>
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
            if (reader.NextResult())//第二个结果集为查询记录数
            {
                if (objPage.IsFillTotalRecords)
                {
                    if (reader.Read())
                    {
                        int totalRecord = reader.GetInt32(0);
                        objPage.TotalRecords = totalRecord;
                        int totalPage = (int)Math.Ceiling((double)objPage.TotalRecords / (double)objPage.PageSize);
                        objPage.TotalPage = totalPage;
                        if (objPage.CurrentPage >= objPage.TotalPage - 1)
                        {
                            objPage.CurrentPage = objPage.TotalPage - 1;
                        }
                    }
                }
            }

            if (reader.NextResult())//第三个结果集为真正记录
            {
                return reader;
            }

            return null;

        }
        /// <summary>
        /// 查询并且返回DataSet(游标分页)
        /// </summary>
        /// <param name="sql">要查询的SQL语句</param>
        /// <param name="objPage">分页对象</param>
        /// <param name="oper">数据库对象</param>
        /// <param name="curType">映射的实体类型(如果用回数据库的原列名，则此为null)</param>
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
                if (reader.NextResult())//第二个结果集为查询结果
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
                if (reader.NextResult())//第三个结果集为总行数
                {
                    if (reader.Read())
                    {
                        int totalRecord = reader.GetInt32(0);
                        objPage.TotalRecords = totalRecord;
                        int totalPage = (int)Math.Ceiling((double)objPage.TotalRecords / (double)objPage.PageSize);
                        objPage.TotalPage = totalPage;
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
