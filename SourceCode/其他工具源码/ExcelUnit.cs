using System;
using System.Collections.Generic;
using System.Text;
using System.Data.OleDb;
using System.Data;
using Buffalo.Kernel.Defaults;
using System.IO;

namespace Buffalo.Kernel
{
    /// <summary>
    /// Excel格式
    /// </summary>
    public enum ExcelFormat 
    {
        /// <summary>
        /// Excel2003的格式(xls)
        /// </summary>
        Excel2003,

        /// <summary>
        /// Excel2007、Excel2010格式(xlsx)
        /// </summary>
        Excel2010
    }

    /// <summary>
    /// 数据读取模式
    /// </summary>
    public enum IMEX
    {
        /// <summary>
        /// 只写模式,档案只能用来做“写入”用途
        /// </summary>
        ExportMode = 0,
        /// <summary>
        /// 只读模式,档案只能用来做“读取”用途
        /// </summary>
        ImportMode = 1,
        /// <summary>
        /// 读写模式，这个模式开启的 Excel 档案可同时支援“读取”与“写入”用途。
        /// </summary>
        LinkedMode = 2,
    }

    public class ExcelUnit
    {

        private static Encoding _defauleEncoding=Encoding.Default;
        private const string XLSProvider="Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties='Excel 8.0;HDR={1};IMEX={2};'";
        private const string XLSXProvider = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties='Excel 12.0 XML;HDR={1};IMEX={2};'";


        /// <summary>
        /// 获取连接字符串
        /// </summary>
        /// <param name="path">位置</param>
        /// <param name="fromat">excel格式</param>
        /// <param name="hasHeadText">是否把第一行作为表头</param>
        /// <param name="mode">读写模式</param>
        /// <returns></returns>
        private static string GetConnectString(string path, ExcelFormat fromat,bool hasHeadText,IMEX mode)
        {
            string provider = XLSProvider;
            if (fromat == ExcelFormat.Excel2010)
            {
                provider = XLSXProvider;
            }
            string connString = null;
            string hdr = "YES";
            if (!hasHeadText) 
            {
                hdr = "NO";
            }
            string strmode = ((int)mode).ToString();
            connString = string.Format(provider, path, hdr, strmode);
            return connString;
        }

        /// <summary>
        /// 读取Excel到DataSet
        /// </summary>
        /// <param name="path">文件</param>
        /// <param name="hasHeadText">是否有表头</param>
        /// <returns></returns>
        public static DataSet LoadXLS(string path, bool hasHeadText) 
        {
            ExcelFormat format = ExcelFormat.Excel2003;
            FileInfo finf = new FileInfo(path);
            string exName = finf.Extension;
            if (!string.IsNullOrEmpty(exName)) 
            {
                if (exName.Equals(".xlsx", StringComparison.CurrentCultureIgnoreCase)) 
                {
                    format = ExcelFormat.Excel2010;
                }
            }
            return LoadXLS(path, format,false,hasHeadText,IMEX.ImportMode);
        }
        const string FilterString = "#_FilterDatabase";
        /// <summary>
        /// 读取Excel到DataSet
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="fromat">格式</param>
        /// <param name="showFilter">是否显示废弃表</param>
        /// <param name="hasHeadText">是否把第一行作为表头</param>
        /// <param name="mode">打开模式</param>
        /// <returns></returns>
        public static DataSet LoadXLS(string path, ExcelFormat fromat, bool showFilter, bool hasHeadText, IMEX mode)
        {
            DataSet ds = new DataSet();
            string connString = GetConnectString(path, fromat, hasHeadText, mode);
            using (OleDbConnection conn = new OleDbConnection(connString))
            {
                conn.Open();
                DataTable dtTables = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });
                if (dtTables == null)
                {
                    return ds;
                }
                foreach (DataRow row in dtTables.Rows)
                {
                    string tableName = row["TABLE_NAME"].ToString();
                    if (!showFilter)
                    {
                        int index = tableName.LastIndexOf(FilterString, StringComparison.CurrentCultureIgnoreCase);
                        if (tableName.Length - FilterString.Length == index)
                        {
                            continue;
                        }
                    }
                    if (!string.IsNullOrEmpty(tableName))
                    {
                        OleDbDataAdapter oda = new OleDbDataAdapter("select * from [" + tableName + "]", conn);
                        oda.Fill(ds, tableName);
                    }
                }
            }
            return ds;
        }

        /// <summary>
        /// 把DataTable保存到Excel文件
        /// </summary>
        /// <param name="dt">数据源</param>
        /// <param name="excelPath">Excel文件位置</param>
        /// <returns></returns>
        public static int ToExcel(DataSet ds, string excelPath) 
        {
            return ToExcel(ds, excelPath, ExcelFormat.Excel2003);
        }

        /// <summary>
        /// 把DataTable保存到Excel文件
        /// </summary>
        /// <param name="dt">数据源</param>
        /// <param name="excelPath">Excel文件位置</param>
        /// <returns></returns>
        public static int ToExcel(DataSet ds, string excelPath, ExcelFormat fromat)
        {
            if (ds == null)
            {
                return -1;
            }
            StringBuilder sb = new StringBuilder();
            string connString = GetConnectString(excelPath, fromat,true,IMEX.ExportMode);

            using (OleDbConnection objConn = new OleDbConnection(connString))
            {
                objConn.Open();
                foreach (DataTable dt in ds.Tables)
                {
                    int rows = dt.Rows.Count;
                    int cols = dt.Columns.Count;

                    sb.Remove(0, sb.Length);
                    //生成创建表的脚本
                    sb.Append("CREATE TABLE [");
                    sb.Append(dt.TableName + "] ( ");

                    for (int i = 0; i < cols; i++)
                    {
                        if (i < cols - 1)
                        {
                            sb.Append(string.Format("[{0}] varchar,", dt.Columns[i].ColumnName));
                        }
                        else
                        {
                            sb.Append(string.Format("[{0}] varchar)", dt.Columns[i].ColumnName));
                        }
                    }


                    OleDbCommand objCmd = new OleDbCommand();
                    objCmd.Connection = objConn;
                    objCmd.CommandText = sb.ToString();
                    objCmd.ExecuteNonQuery();

                    #region 生成插入数据脚本
                    sb.Remove(0, sb.Length);
                    sb.Append("INSERT INTO [");
                    sb.Append(dt.TableName + "] ( ");

                    for (int i = 0; i < cols; i++)
                    {
                        if (i < cols - 1)
                            sb.Append("[" + dt.Columns[i].ColumnName + "],");
                        else
                            sb.Append("[" + dt.Columns[i].ColumnName + "]) values (");
                    }

                    for (int i = 0; i < cols; i++)
                    {
                        if (i < cols - 1)
                        {
                            sb.Append("@" + dt.Columns[i].ColumnName + ",");
                        }
                        else
                        {
                            sb.Append("@" + dt.Columns[i].ColumnName + ")");
                        }
                    }
                    #endregion


                    //建立插入动作的Command
                    objCmd.CommandText = sb.ToString();
                    OleDbParameterCollection param = objCmd.Parameters;

                    for (int i = 0; i < cols; i++)
                    {
                        param.Add(new OleDbParameter("@" + dt.Columns[i].ColumnName, OleDbType.VarChar));
                    }
                    string value = "";
                    //遍历DataTable将数据插入新建的Excel文件中
                    foreach (DataRow row in dt.Rows)
                    {
                        for (int i = 0; i < param.Count; i++)
                        {

                            if (row[i] != null)
                            {
                                value = row[i].ToString();
                            }
                            else
                            {
                                value = "";
                            }
                            param[i].Value = value;
                        }

                        objCmd.ExecuteNonQuery();
                    }
                }
                return 1;
            }
        }



        /// <summary>
        /// 读取CSV
        /// </summary>
        /// <param name="stm"></param>
        /// <returns></returns>
        public static DataTable ReadCSVFromDT(string path)
        {
            using (FileStream stm = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                return ReadCSVFromDT(stm,_defauleEncoding);
            }
        }
        /// <summary>
        /// 读取CSV
        /// </summary>
        /// <param name="stm"></param>
        /// <returns></returns>
        public static DataTable ReadCSVFromDT(string path, Encoding encoding)
        {
            using (FileStream stm = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                return ReadCSVFromDT(stm, encoding);
            }
        }
        /// <summary>
        /// 读取CSV
        /// </summary>
        /// <param name="stm"></param>
        /// <returns></returns>
        public static DataTable ReadCSVFromDT(Stream stm,Encoding encoding) 
        {
            

            DataTable dt = new DataTable();
            List<string[]> lst = ReadCSV(stm, encoding);
            if (lst.Count <= 0)
            {
                return null;
            }
            //读取列头
            string[] dataRow = lst[0];
            foreach (string data in dataRow) 
            {
                dt.Columns.Add(data, typeof(string));
            }

            //读取数据
            for (int i = 1; i < lst.Count; i++) 
            {
                dataRow = lst[i];
                DataRow dr = dt.NewRow();
                for (int k = 0; k < dt.Columns.Count; k++) 
                {
                    if (k >= dataRow.Length) 
                    {
                        break;
                    }
                    string data = dataRow[k];
                    if (data != null)
                    {
                        dr[k] = data;
                    }
                    else 
                    {
                        dr[k] = "";
                    }
                }
                dt.Rows.Add(dr);
            }
            return dt;
        }


        /// <summary>
        /// 把DataTable导出成CSV文件
        /// </summary>
        /// <param name="stm"></param>
        /// <param name="dt"></param>
        public static void WriteCSVToDT(string path, DataTable dt)
        {
            using (FileStream stm = new FileStream(path, FileMode.Create, FileAccess.Write))
            {
                WriteCSVToDT(stm, dt,_defauleEncoding);
            }

        }
        /// <summary>
        /// 把DataTable导出成CSV文件
        /// </summary>
        /// <param name="stm"></param>
        /// <param name="dt"></param>
        public static void WriteCSVToDT(string path, DataTable dt, Encoding encoding)
        {
            using (FileStream stm = new FileStream(path, FileMode.Create, FileAccess.Write))
            {
                WriteCSVToDT(stm, dt, encoding);
            }

        }

        /// <summary>
        /// 把DataTable导出成CSV文件
        /// </summary>
        /// <param name="stm"></param>
        /// <param name="dt"></param>
        public static void WriteCSVToDT(Stream stm, DataTable dt,Encoding encoding) 
        {
            List<string[]> lst = new List<string[]>();

            string[] tmpRow = new string[dt.Columns.Count];
            //导出列头
            for (int i=0;i< dt.Columns.Count;i++) 
            {
                DataColumn col=dt.Columns[i];
                tmpRow[i] = col.ColumnName;
            }
            lst.Add(tmpRow);

            //导出数据
            foreach (DataRow dr in dt.Rows) 
            {
                tmpRow = new string[dt.Columns.Count];
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    if (!dr.IsNull(i))
                    {
                        tmpRow[i] = dr[i].ToString();
                    }
                    else 
                    {
                        tmpRow[i] ="";
                    }
                }
                lst.Add(tmpRow);
            }
            WriteCSV(stm, lst, encoding);

        }


        /// <summary>
        /// 读取CSV
        /// </summary>
        /// <param name="stm">文件流</param>
        /// <param name="encoding">编码</param>
        /// <returns></returns>
        public static List<string[]> ReadCSV(string path)
        {
            List<string[]> lst = null;
            using (FileStream stm = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                lst = ReadCSV(stm, _defauleEncoding);
            }
            return lst;
        }


        /// <summary>
        /// 写入CSV
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <param name="ls">数据</param>
        public static void WriteCSV(string path, List<string[]> ls)
        {
            using (FileStream stm = new FileStream(path, FileMode.Create,FileAccess.Write))
            {
                WriteCSV(stm, ls, _defauleEncoding);
            }
        }



        /// <summary>
        /// 写入CSV
        /// </summary>
        /// <param name="stm">文件流</param>
        /// <param name="ls">数据</param>
        /// <param name="encoding">编码</param>
        public static void WriteCSV(Stream stm, List<string[]> ls, Encoding encoding)
        {
            using (StreamWriter fileWriter = new StreamWriter(stm, encoding))
            {
                StringBuilder sbTmp = new StringBuilder();
                foreach (string[] strArr in ls)
                {
                    
                    foreach (string str in strArr)
                    {
                        sbTmp.Append("\""+str.Replace("\"", "\"\"")+"\",");
                        
                    }
                    fileWriter.WriteLine(sbTmp.ToString());
                    sbTmp.Remove(0, sbTmp.Length);
                }
            }
        }

        /// <summary>
        /// 读取CSV
        /// </summary>
        /// <param name="stm">文件流</param>
        /// <param name="encoding">编码</param>
        /// <returns></returns>
        public static List<String[]> ReadCSV(Stream stm, Encoding encoding)
        {
            List<string[]> ls = new List<String[]>();
            using (StreamReader fileReader = new StreamReader(stm, encoding))
            {
                string strLine = fileReader.ReadToEnd();
                CSVReader reader = new CSVReader(strLine);
                string[] row = null;
                while ((row = reader.ReaderRow())!=null) 
                {
                    if (row.Length > 0)
                    {
                        ls.Add(row);
                    }
                }
            }
            return ls;
        }
    }
}


//*****************Excel例子*************************
//导出：
//DataSet ds=...;
//ExcelUnit.ToExcel(ds,"D:\aa.xls");

//导入:
//DataSet ds=ExcelUnit.LoadXLS("D:\aa.xls");


//*****************CSV例子*************************

//写入：
        //UsersBusiness bo = new UsersBusiness();
        //DataSet ds = bo.Select(new ScopeList());
        //ExcelUnit.WriteCSVToDT("d:\\a.csv", ds.Tables[0]);

//读取：
        //DataTable ds = ExcelUnit.ReadCSVFromDT("d:\\a.csv");
        //GridView1.DataSource = ds;
        //GridView1.DataBind();