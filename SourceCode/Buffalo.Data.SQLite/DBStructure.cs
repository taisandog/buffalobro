using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DB.DbCommon;
using System.Data;
using Buffalo.DB.DataBaseAdapter.IDbAdapters;
using Buffalo.DB.PropertyAttributes;
using System.Text.RegularExpressions;
using Buffalo.DB.DataBaseAdapter;



namespace Buffalo.Data.SQLite
{
    /// <summary>
    /// 数据库结构特性类
    /// </summary>
    public class DBStructure : IDBStructure
    {
        private static string _sqlTables = "SELECT * FROM [SQLITE_MASTER] WHERE 1=1";

        #region IDBStructure 成员

        /// <summary>
        /// 获取数据库中的所有表      
        /// </summary>
        /// <param name="oper"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        public List<DBTableInfo> GetAllTableName(DataBaseOperate oper, DBInfo info)
        {

            return GetTableNames(oper,info,null);
        }
        private static string[] SysTables ={ "sqlite_sequence", "sqlite_master" };
        /// <summary>
        /// 是否系统表名
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static bool IsSysTable(string tableName) 
        {
            foreach (string systab in SysTables) 
            {
                if (tableName.Equals(systab, StringComparison.CurrentCultureIgnoreCase)) 
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 获取表信息
        /// </summary>
        /// <param name="oper"></param>
        /// <param name="info"></param>
        /// <param name="tableNames"></param>
        /// <returns></returns>
        private List<DBTableInfo> GetTableNames(DataBaseOperate oper, DBInfo info, IEnumerable<string> tableNames) 
        {
            ParamList lstParam = new ParamList();
            string inTables=Buffalo.DB.DataBaseAdapter.SqlServer2KAdapter.DBStructure.AllInTableNames(tableNames);

            StringBuilder sbSQL = new StringBuilder(_sqlTables);
            if (string.IsNullOrEmpty(inTables))
            {
                sbSQL.Append(" and [sql]<>''");
            }
            else 
            {
                sbSQL.Append(" and [Name] IN(" + inTables + ")");
            }


            List<DBTableInfo> lstName = new List<DBTableInfo>();
            ///只能获取表名,其它的没用
            using (IDataReader reader = oper.Query(sbSQL.ToString(), lstParam,null))
            {
                while (reader.Read())
                {
                    DBTableInfo tableInfo = new DBTableInfo();
                    tableInfo.Name = reader["name"] as string;
                    tableInfo.IsView = (reader["type"] as string) == "view";
                    if (IsSysTable(tableInfo.Name)) 
                    {
                        continue;
                    }
                    lstName.Add(tableInfo);
                }
            }
            return lstName;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string GetAddParamSQL()
        {
            return "add column";
        }
        /// <summary>
        /// 获取所有关系
        /// </summary>
        /// <param name="oper"></param>
        /// <param name="info"></param>
        /// <param name="childName"></param>
        /// <returns></returns>
        public List<TableRelationAttribute> GetRelation(DataBaseOperate oper, DBInfo info, IEnumerable<string> childName)
        {
            return null;
            DataTable dtRelation = oper.GetSchema("ForeignKeys");
            List<TableRelationAttribute> lstRet = new List<TableRelationAttribute>();
            foreach (DataRow row in dtRelation.Rows)
            {
                TableRelationAttribute tr = new TableRelationAttribute();
                tr.Name=row["CONSTRAINT_NAME"] as string;
                tr.SourceName=row["FKEY_FROM_COLUMN"] as string;
                tr.SourceTable=row["TABLE_NAME"] as string;
                tr.TargetName=row["FKEY_TO_COLUMN"] as string;
                tr.TargetTable =row["FKEY_TO_TABLE"] as string;
                tr.IsParent = true;
                lstRet.Add(tr);

                tr = new TableRelationAttribute();
                tr.Name = row["CONSTRAINT_NAME"] as string;
                tr.TargetName = row["FKEY_FROM_COLUMN"] as string;
                tr.TargetTable = row["TABLE_NAME"] as string;
                tr.SourceName = row["FKEY_TO_COLUMN"] as string;
                tr.SourceTable = row["FKEY_TO_TABLE"] as string;
                tr.IsParent = false;
                lstRet.Add(tr);
            }
            return lstRet;
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
            List<DBTableInfo> lst = GetTableNames(oper, info, tableNames);
            DataTable dtSchema = oper.GetSchema("Columns");

            DataTable dtDataTypes = oper.GetSchema("DataTypes");

            foreach (DBTableInfo table in lst)
            {
                dicTables[table.Name] = table;
                table.Params = new List<EntityParam>();
                table.RelationItems = new List<TableRelationAttribute>();
            }
            foreach (DataRow dr in dtSchema.Rows)
            {
                string tableName = dr["TABLE_NAME"] as string;
                if (string.IsNullOrEmpty(tableName))
                {
                    continue;
                }
                DBTableInfo table = null;
                if (dicTables.TryGetValue(tableName, out table))
                {
                    FillParam(table, dr, dtDataTypes);
                }
            }
            List<TableRelationAttribute> lstRelation = GetRelation(oper, info, tableNames);

            Buffalo.DB.DataBaseAdapter.SqlServer2KAdapter.DBStructure.FillRelation(dicTables, lstRelation);

            return lst;
        }

        

        /// <summary>
        /// 填充字段信息
        /// </summary>
        /// <param name="table"></param>
        /// <param name="reader"></param>
        private void FillParam(DBTableInfo table, DataRow dr, DataTable dtDataTypes)
        {

            string prmName = dr["COLUMN_NAME"] as string;
            if (string.IsNullOrEmpty(prmName))
            {
                return;
            }

            foreach (EntityParam ep in table.Params)
            {
                if (ep.ParamName == prmName)
                {
                    return;
                }
            }

            EntityParam prm = new EntityParam();
            prm.ParamName = prmName;

            EntityPropertyType type = EntityPropertyType.Normal;
            bool isPrimary = (!dr.IsNull("PRIMARY_KEY")) && ((bool)dr["PRIMARY_KEY"]);

            if (isPrimary)
            {
                type = EntityPropertyType.PrimaryKey;

            }
            bool isIdentity = (!dr.IsNull("AUTOINCREMENT")) && ((bool)dr["AUTOINCREMENT"]);
            if (isIdentity)
            {
                type = type | EntityPropertyType.Identity;
            }
            bool allowNull=(!dr.IsNull("IS_NULLABLE")) && ((bool)dr["IS_NULLABLE"]);
            prm.AllowNull = allowNull;
            prm.PropertyType = type;
            prm.Length = ValueConvertExtend.ConvertValue<long>(dr["CHARACTER_MAXIMUM_LENGTH"]);
            string strDBType = dr.IsNull("DATA_TYPE") ? "text" : (dr["DATA_TYPE"] as string);
            FillDbType(strDBType, prm, dtDataTypes);
            table.Params.Add(prm);

        }



        /// <summary>
        /// 填充数据库类型
        /// </summary>
        /// <param name="nativeType"></param>
        /// <returns></returns>
        private void FillDbType(string nativeType, EntityParam prm, DataTable dtDataTypes)
        {
            DataRow[] rowArray = dtDataTypes.Select(string.Format("TypeName = '{0}'", nativeType.ToLowerInvariant()));
            if ((rowArray != null) && (rowArray.Length > 0))
            {
                prm.SqlType = (DbType)rowArray[0]["ProviderDbType"];
                return;
            }
            if (Regex.IsMatch(nativeType, "int", RegexOptions.IgnoreCase))
            {
                prm.SqlType = DbType.Int64;
                return;
            }
            if (Regex.IsMatch(nativeType, "real|floa|doub", RegexOptions.IgnoreCase))
            {
                prm.SqlType = DbType.Double;
                return;
            }
            if (Regex.IsMatch(nativeType, "numeric", RegexOptions.IgnoreCase))
            {
                prm.SqlType = DbType.Decimal;
                return;
            }
            if (Regex.IsMatch(nativeType, "char|clob|text", RegexOptions.IgnoreCase))
            {
                prm.SqlType = DbType.String;
                return;
            }
            prm.SqlType = DbType.Object;
            
            //prm.SqlType=type;
            //prm.Length=length;
        }

        #endregion

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
            if (type == CheckEvent.RelationBeginCheck) 
            {
                lstSQL.Add("PRAGMA foreign_keys = ON;");
            }
        }

        #endregion
    }
}
