using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DB.DbCommon;
using System.Data;
using Buffalo.DB.BQLCommon.BQLConditionCommon;
using Buffalo.DB.BQLCommon.BQLKeyWordCommon;
using Buffalo.DB.PropertyAttributes;
using Buffalo.DB.CommBase.DataAccessBases;
using Buffalo.DB.DataBaseAdapter.IDbAdapters;
using System.Data.OleDb;
using Buffalo.DB.CommBase.BusinessBases;



namespace Buffalo.DB.DataBaseAdapter.AccessAdapter
{
    


    /// <summary>
    /// 数据库结构特性类
    /// </summary>
    public class DBStructure : Buffalo.DB.DataBaseAdapter.IDbAdapters.IDBStructure
    {
        //private static string _sqlTables = "SELECT [name],[xtype] FROM [sysobjects] Where [xtype] in ('U','V') and [name] not in('dtproperties','sysdiagrams') ORDER BY [xtype],[crdate] desc";
        
        /// <summary>
        /// 获取所有用户表
        /// </summary>
        /// <returns></returns>
        public virtual List<DBTableInfo> GetAllTableName(DataBaseOperate oper, DBInfo info)
        {
            ParamList lstParam = new ParamList();
            //DataBaseOperate oper = info.DefaultOperate;
            List<DBTableInfo> lstName = new List<DBTableInfo>();
            using (BatchAction ba = oper.StarBatchAction())
            {
                OleDbConnection conn = oper.Connection as OleDbConnection;
                oper.ConnectDataBase();


                DataTable dt = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });
                foreach (DataRow row in dt.Rows)
                {
                    DBTableInfo tableInfo = new DBTableInfo();
                    if (row.IsNull("TABLE_NAME"))
                    {
                        continue;
                    }
                    tableInfo.Name = row["TABLE_NAME"].ToString();
                    tableInfo.IsView = false;
                    lstName.Add(tableInfo);
                }
                dt = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "VIEW" });

                foreach (DataRow row in dt.Rows)
                {
                    DBTableInfo tableInfo = new DBTableInfo();
                    if (row.IsNull("TABLE_NAME"))
                    {
                        continue;
                    }
                    tableInfo.Name = row["TABLE_NAME"].ToString();
                    tableInfo.IsView = true;
                    lstName.Add(tableInfo);
                }
            }
            return lstName;
        }



        /// <summary>
        /// 添加字段的语句
        /// </summary>
        /// <returns></returns>
        public virtual string GetAddParamSQL() 
        {
            return "add";
        }

        /// <summary>
        /// 获取要In的表名
        /// </summary>
        /// <param name="childName"></param>
        /// <returns></returns>
        internal static string AllInTableNames(IEnumerable<string> tableNames) 
        {
            if (tableNames == null) 
            {
                return null;
            }
            StringBuilder sbTables = new StringBuilder();
            foreach (string tableName in tableNames)
            {
                sbTables.Append("'");
                sbTables.Append(tableName.Replace("'","''"));
                sbTables.Append("',");
            }
            if (sbTables.Length > 0) 
            {
                sbTables.Remove(sbTables.Length - 1, 1);
            }
            return sbTables.ToString();
        }

        /// <summary>
        /// 获取所有关系
        /// </summary>
        /// <param name="chileName">null则查询所有表</param>
        /// <returns></returns>
        public List<TableRelationAttribute> GetRelation(DataBaseOperate oper, DBInfo info, IEnumerable<string> childNames) 
        {
            List<TableRelationAttribute> lst = new List<TableRelationAttribute>();
            using (BatchAction ba = oper.StarBatchAction())
            {
                OleDbConnection conn = oper.Connection as OleDbConnection;
                oper.ConnectDataBase();
                DataTable dt = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Foreign_Keys, null);

                foreach (DataRow dr in dt.Rows)
                {


                    TableRelationAttribute tinfo = new TableRelationAttribute();
                    tinfo.CreateName();
                    tinfo.SourceTable = dr["FK_TABLE_NAME"] as string;
                    tinfo.SourceName = dr["FK_COLUMN_NAME"] as string;
                    tinfo.TargetTable = dr["PK_TABLE_NAME"] as string;
                    tinfo.TargetName = dr["PK_COLUMN_NAME"] as string;
                    tinfo.IsParent = true;
                    lst.Add(tinfo);

                    tinfo = new TableRelationAttribute();
                    tinfo.CreateName();
                    tinfo.SourceTable = dr["PK_TABLE_NAME"] as string;
                    tinfo.SourceName = dr["PK_COLUMN_NAME"] as string;
                    tinfo.TargetTable = dr["FK_TABLE_NAME"] as string;
                    tinfo.TargetName = dr["FK_COLUMN_NAME"] as string;
                    tinfo.IsParent = false;
                    lst.Add(tinfo);
                }


            }
            return lst;

        }

        /// <summary>
        /// 初始化获取表名语句
        /// </summary>
        /// <returns></returns>
        protected virtual string GetTableParamsSQL()
        {
            
            return Resource.tableParam2000;
        }

       

        /// <summary>
        /// 获取表信息
        /// </summary>
        /// <param name="oper"></param>
        /// <param name="info"></param>
        /// <param name="tableNames"></param>
        /// <returns></returns>
        public List<DBTableInfo> GetTablesInfo(DataBaseOperate oper, DBInfo info, IEnumerable<string> tableNames)
        {
            Dictionary<string, DBTableInfo> dicTables = new Dictionary<string, DBTableInfo>();
            List<DBTableInfo> lst = new List<DBTableInfo>();

            
            using(BatchAction ba=oper.StarBatchAction())
            {
                OleDbConnection conn = oper.Connection as OleDbConnection;
                oper.ConnectDataBase();
                DataTable dtpk = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Primary_Keys, null);    //主键
                Dictionary<string, bool> dicPk = new Dictionary<string, bool>();
                foreach (DataRow row in dtpk.Rows) 
                {
                    string key=row["TABLE_NAME"].ToString()+":"+row["COLUMN_NAME"].ToString();
                    dicPk[key.ToLower()] = true;
                }

                foreach (string tableName in tableNames)
                {
                    DBTableInfo tableinfo = new DBTableInfo();
                    tableinfo.Params = new List<EntityParam>();
                    tableinfo.RelationItems = new List<TableRelationAttribute>();

                    tableinfo.IsView = false;
                    DataTable dtStr = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, tableName, "TABLE" });
                    if (dtStr.Rows.Count <= 0)
                    {
                        dtStr = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, tableName, "VIEW" });
                        tableinfo.IsView = true;
                    }
                    if (dtStr.Rows.Count <= 0)
                    {
                        continue;
                    }
                    tableinfo.Name = dtStr.Rows[0]["TABLE_NAME"] as string;
                    tableinfo.Description = dtStr.Rows[0]["DESCRIPTION"] as string;
                    DataTable dt = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Columns, new object[] { null, null, tableName, null });
                    
                    

                    //获取表结构
                    DataTable dtData = null;
                    string sql = "select * from [" + tableName + "]";
                    using (OleDbCommand comm = new OleDbCommand(sql, conn))
                    {
                        using (OleDbDataReader dr = comm.ExecuteReader(CommandBehavior.KeyInfo))
                        {
                            dtData = dr.GetSchemaTable();
                        }
                    }
                    DataView dv = dt.DefaultView;
                    dv.Sort = "ORDINAL_POSITION asc";
                    foreach (DataRowView row in dv)
                    {
                        FillParam(tableinfo, row.Row, dicPk, dtData);

                    }
                    lst.Add(tableinfo);
                }
            }
            foreach (DBTableInfo table in lst)
            {
                dicTables[table.Name] = table;
                
            }
            List<TableRelationAttribute> lstRelation = GetRelation(oper, info, tableNames);

            FillRelation(dicTables, lstRelation);
            return lst;
        }

        

        /// <summary>
        /// 填充关系信息
        /// </summary>
        /// <param name="dicTables"></param>
        /// <param name="lstRelation"></param>
        internal static void FillRelation(Dictionary<string, DBTableInfo> dicTables, List<TableRelationAttribute> lstRelation) 
        {
            if (lstRelation == null) 
            {
                return;
            }
            DBTableInfo ptable = null;
            DBTableInfo ctable = null;

            foreach (TableRelationAttribute tinfo in lstRelation)
            {

                if (dicTables.TryGetValue(tinfo.SourceTable, out ctable))
                {
                    ctable.RelationItems.Add(tinfo);
                }
                //else if (!tinfo.IsParent && dicTables.TryGetValue(tinfo.TargetTable, out ptable)) //填充父项
                //{
                //    //TableRelationAttribute cinfo = new TableRelationAttribute();
                //    //cinfo.SourceName = tinfo.TargetName;
                //    //cinfo.SourceTable = tinfo.TargetTable;
                //    //cinfo.TargetName = tinfo.SourceName;
                //    //cinfo.TargetTable = tinfo.SourceTable;
                //    //cinfo.IsParent = false;
                //    ptable.RelationItems.Add(tinfo);
                //}
               


            }
        }

        /// <summary>
        /// 判断是否自增长
        /// </summary>
        /// <param name="table"></param>
        /// <param name="colName"></param>
        /// <returns></returns>
        private bool IsIdentity(DataTable table,string colName)
        {
            foreach (DataRow item in table.Rows)  //重点语句，获得表架构
            {
                if (item.IsNull("IsAutoIncrement")) 
                {
                    continue;
                }
                bool auto = Convert.ToBoolean(item["IsAutoIncrement"]);
                if (auto)
                {
                    string name = item["ColumnName"] as string;  //字段名
                    if (colName.Equals(name, StringComparison.CurrentCultureIgnoreCase)) 
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        /// <summary>
        /// 填充字段信息
        /// </summary>
        /// <param name="prm">字段信息</param>
        /// <param name="reader">reader</param>
        private void FillParam(DBTableInfo table, DataRow row, Dictionary<string, bool> dicPk, DataTable dtData) 
        {
            string prmName = row["COLUMN_NAME"] as string;
            if(string.IsNullOrEmpty(prmName))
            {
                return;
            }

            foreach(EntityParam ep in table.Params)
            {
                if (ep.ParamName == prmName) 
                {
                    return;
                }
            }

            EntityParam prm = new EntityParam();
            prm.ParamName = prmName;
            
            EntityPropertyType type = EntityPropertyType.Normal;
            string key = table.Name + ":" + prmName;
            key = key.ToLower();

            bool isPrimary = dicPk.ContainsKey(key);
            bool isIdentity = IsIdentity(dtData,prmName);
            if (isPrimary) 
            {
                type = EntityPropertyType.PrimaryKey;
            }
            if (isIdentity )
            {
                type = type | EntityPropertyType.Identity;
            }
            
            prm.AllowNull = Convert.ToBoolean(row["IS_NULLABLE"]);
            prm.PropertyType = type;
            if (!row.IsNull("CHARACTER_MAXIMUM_LENGTH"))
            {
                prm.Length = Convert.ToInt64(row["CHARACTER_MAXIMUM_LENGTH"]);
            }
            if (!table.IsView)
            {
                prm.Description = row["DESCRIPTION"] as string;
            }
            int dbType = 0;
            if (!row.IsNull("DATA_TYPE"))
            {
                dbType = Convert.ToInt32(row["DATA_TYPE"]);
            }
            prm.SqlType = GetDbType(dbType);
            table.Params.Add(prm);
        }

        /// <summary>
        /// 获取DbType
        /// </summary>
        /// <param name="nativeType"></param>
        /// <returns></returns>
        private DbType GetDbType(int nativeType)
        {
            OleDbType type = (OleDbType)nativeType;
            
            switch (type)
            {
                case OleDbType.Empty:
                    return DbType.Object;

                case OleDbType.SmallInt:
                    return DbType.Int16;

                case OleDbType.Integer:
                    return DbType.Int32;

                case OleDbType.Single:
                    return DbType.Decimal;

                case OleDbType.Double:
                    return DbType.Decimal;

                case OleDbType.Currency:
                    return DbType.Currency;

                case OleDbType.Date:
                    return DbType.DateTime;

                case OleDbType.BSTR:
                    return DbType.AnsiString;

                case OleDbType.IDispatch:
                    return DbType.Object;

                case OleDbType.Error:
                    return DbType.Object;

                case OleDbType.Boolean:
                    return DbType.Boolean;

                case OleDbType.Variant:
                    return DbType.Object;

                case OleDbType.IUnknown:
                    return DbType.Object;

                case OleDbType.Decimal:
                    return DbType.Decimal;

                case OleDbType.TinyInt:
                    return DbType.SByte;

                case OleDbType.UnsignedTinyInt:
                    return DbType.Byte;

                case OleDbType.UnsignedSmallInt:
                    return DbType.UInt16;

                case OleDbType.UnsignedInt:
                    return DbType.UInt32;

                case OleDbType.BigInt:
                    return DbType.Int64;

                case OleDbType.UnsignedBigInt:
                    return DbType.UInt64;



                case OleDbType.Binary:
                    return DbType.Binary;

                case OleDbType.Char:
                    return DbType.AnsiStringFixedLength;

                case OleDbType.WChar:
                    return DbType.StringFixedLength;

                case OleDbType.Numeric:
                    return DbType.Decimal;

                

                case OleDbType.DBDate:
                    return DbType.DateTime;

                case OleDbType.DBTime:
                    return DbType.Time;

                case OleDbType.DBTimeStamp:
                    return DbType.Binary;

                

                case OleDbType.PropVariant:
                    return DbType.Object;

                case OleDbType.VarNumeric:
                    return DbType.Decimal;

                case OleDbType.Guid:
                    return DbType.Guid;

                case OleDbType.VarChar:
                    return DbType.AnsiString;

                case OleDbType.LongVarChar:
                    return DbType.String;

                case OleDbType.VarWChar:
                    return DbType.String;

                case OleDbType.LongVarWChar:
                    return DbType.String;

                case OleDbType.VarBinary:
                    return DbType.Binary;

                case OleDbType.LongVarBinary:
                    return DbType.Binary;
            }
            return DbType.Object;
        }

        #region 创建事件
        /// <summary>
        /// 数据库检查时候的事建
        /// </summary>
        /// <param name="arg">当前类型</param>
        /// <param name="dbInfo">数据库类型</param>
        /// <param name="type">检查类型</param>
        /// <param name="lstSQL">SQL语句</param>
        public void OnCheckEvent(object arg, DBInfo dbInfo, CheckEvent type, List<string> lstSQL) 
        {

        }
        #endregion

    }
}
