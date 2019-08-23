using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DB.DbCommon;
using System.Data;
using Buffalo.DB.DataBaseAdapter.IDbAdapters;
using Buffalo.DB.PropertyAttributes;
using Buffalo.DB.DataBaseAdapter;



namespace Buffalo.Data.MySQL
{
    /// <summary>
    /// 数据库结构特性类
    /// </summary>
    public class DBStructure : IDBStructure
    {
        private static string _sqlTables = "SELECT TABLE_NAME,TABLE_TYPE,TABLE_COMMENT FROM INFORMATION_SCHEMA.TABLES where TABLE_SCHEMA =?dbName";
        #region IDBStructure 成员

        

        /// <summary>
        /// 获取所有用户表
        /// </summary>
        /// <param name="oper"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        public List<DBTableInfo> GetAllTableName(DataBaseOperate oper, DBInfo info)
        {

            return GetTableNames(oper,info,null);

        }
        /// <summary>
        /// 获取所有用户表
        /// </summary>
        /// <param name="oper"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        private List<DBTableInfo> GetTableNames(DataBaseOperate oper, DBInfo info,IEnumerable<string> tableNames)
        {
            ParamList lstParam = new ParamList();

            lstParam.AddNew("?dbName", DbType.String, oper.DataBaseName);
            List<DBTableInfo> lstName = new List<DBTableInfo>();
            string inTable = Buffalo.DB.DataBaseAdapter.SqlServer2KAdapter.DBStructure.AllInTableNames(tableNames);
            string sql = _sqlTables;
            if (!string.IsNullOrEmpty(inTable))
            {
                sql += " and TABLE_NAME in(" + inTable + ")";
            }

            using (IDataReader reader = oper.Query(sql, lstParam, null))
            {
                while (reader.Read())
                {
                    DBTableInfo tableInfo = new DBTableInfo();
                    tableInfo.Name = reader[0] as string;
                    string type = reader[1] as string;
                    if (!string.IsNullOrEmpty(type))
                    {
                        if (type.Trim() == "VIEW")
                        {
                            tableInfo.IsView = true;
                        }
                    }
                    string comment = reader[2] as string;
                    tableInfo.Description = comment;

                    lstName.Add(tableInfo);
                }
            }
            return lstName;

        }

        /// <summary>
        /// 添加字段的语句
        /// </summary>
        /// <returns></returns>
        public string GetAddParamSQL()
        {
            return "add";
        }

        /// <summary>
        /// 获取所有关系
        /// </summary>
        /// <param name="oper"> </param>
        /// <param name="info"> </param>
        /// <param name="childNames">null则查询所有表</param>
        /// <returns></returns>
        public List<TableRelationAttribute> GetRelation(DataBaseOperate oper, DBInfo info, IEnumerable<string> childNames)
        {
            string sql = "SELECT t1.CONSTRAINT_NAME,t1.TABLE_NAME, t1.COLUMN_NAME, t1.POSITION_IN_UNIQUE_CONSTRAINT,  t1.REFERENCED_TABLE_NAME, REFERENCED_COLUMN_NAME FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE t1  INNER JOIN INFORMATION_SCHEMA.TABLE_CONSTRAINTS t2  ON t2.TABLE_SCHEMA = t1.TABLE_SCHEMA  AND t2.TABLE_NAME = t1.TABLE_NAME  AND t2.CONSTRAINT_NAME = t1.CONSTRAINT_NAME WHERE t1.TABLE_SCHEMA = ?dbName  AND t2.CONSTRAINT_TYPE = 'FOREIGN KEY'";
            StringBuilder sqlFk = new StringBuilder();
            StringBuilder sqlPk = new StringBuilder();
            //sql.Append("SELECT constraint_schema,constraint_name,unique_constraint_name,table_name,referenced_table_name FROM `information_schema`.`REFERENTIAL_CONSTRAINTS`;");
            sqlFk.Append(sql);
            sqlPk.Append(sql);
            ParamList lstParam = new ParamList();
            lstParam.AddNew("?dbName", DbType.String, oper.DataBaseName);
            string childName = Buffalo.DB.DataBaseAdapter.SqlServer2KAdapter.DBStructure.AllInTableNames(childNames);

            if (!string.IsNullOrEmpty(childName))
            {
                sqlFk.Append(" and t1.TABLE_NAME in(" + childName + ")");
            }
            if (!string.IsNullOrEmpty(childName))
            {
                sqlPk.Append(" and t1.REFERENCED_TABLE_NAME in(" + childName + ")");
            }
            List<TableRelationAttribute> lst = new List<TableRelationAttribute>();

            using (IDataReader reader = info.DefaultOperate.Query(sqlFk.ToString(), lstParam, null))
            {
                while (reader.Read())
                {
                    TableRelationAttribute tinfo = new TableRelationAttribute();
                    tinfo.Name = reader["CONSTRAINT_NAME"] as string;
                    tinfo.SourceTable = reader["TABLE_NAME"] as string;
                    tinfo.SourceName = reader["COLUMN_NAME"] as string;
                    tinfo.TargetTable = reader["REFERENCED_TABLE_NAME"] as string;
                    tinfo.TargetName = reader["REFERENCED_COLUMN_NAME"] as string;
                    tinfo.IsParent = true;
                    lst.Add(tinfo);
                }
            }
            using (IDataReader reader = info.DefaultOperate.Query(sqlPk.ToString(), lstParam, null))
            {
                while (reader.Read())
                {
                    TableRelationAttribute tinfo = new TableRelationAttribute();
                    tinfo.Name = reader["CONSTRAINT_NAME"] as string;
                    tinfo.TargetTable = reader["TABLE_NAME"] as string;
                    tinfo.TargetName = reader["COLUMN_NAME"] as string;
                    tinfo.SourceTable = reader["REFERENCED_TABLE_NAME"] as string;
                    tinfo.SourceName  = reader["REFERENCED_COLUMN_NAME"] as string;
                    tinfo.IsParent = false;
                    lst.Add(tinfo);
                }
            }
            return lst;
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
            string inTable = Buffalo.DB.DataBaseAdapter.SqlServer2KAdapter.DBStructure.AllInTableNames(tableNames);
            string sql = "SELECT t1.TABLE_NAME,t1.COLUMN_NAME,t1.COLUMN_COMMENT, t1.DATA_TYPE, t1.CHARACTER_OCTET_LENGTH, t1.NUMERIC_PRECISION, t1.NUMERIC_SCALE, CASE t1.IS_NULLABLE WHEN 'NO' THEN 0 ELSE 1 END IS_NULLABLE, t1.COLUMN_TYPE,t1.COLUMN_KEY,t1.EXTRA FROM INFORMATION_SCHEMA.COLUMNS t1 where t1.TABLE_SCHEMA = ?dbName";
            if (!string.IsNullOrEmpty(inTable))
            {
                sql+= " and t1.TABLE_NAME in(" + inTable + ")";
            }
            Dictionary<string, DBTableInfo> dicTables = new Dictionary<string, DBTableInfo>();
            ParamList lstParam = new ParamList();
            lstParam.AddNew("?dbName", DbType.String, oper.DataBaseName);

            List<DBTableInfo> tables = GetTableNames(oper, info, tableNames);

            foreach(DBTableInfo table in tables)
            {
                dicTables[table.Name]=table;
                table.Params = new List<EntityParam>();
                table.RelationItems = new List<TableRelationAttribute>();
            }

            using (IDataReader reader = oper.Query(sql.ToString(), lstParam, null))
            {

                while (reader.Read())
                {
                    string tableName = reader["TABLE_NAME"] as string;
                    if (string.IsNullOrEmpty(tableName))
                    {
                        continue;
                    }
                    DBTableInfo table = null;
                    dicTables.TryGetValue(tableName, out table);
                    if (table == null)
                    {
                        continue;
                    }
                    FillParam(table, reader);

                }
            }
            List<TableRelationAttribute> lstRelation = GetRelation(oper, info, tableNames);
            Buffalo.DB.DataBaseAdapter.SqlServer2KAdapter.DBStructure.FillRelation(dicTables, lstRelation);

            return tables;
        }

        /// 填充字段信息
        /// </summary>
        /// <param name="prm">字段信息</param>
        /// <param name="reader">reader</param>
        private void FillParam(DBTableInfo table, IDataReader reader)
        {
            string prmName = reader["COLUMN_NAME"] as string;
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
            bool isIdentity = false;
            bool isPrimaryKey = false;
            string extra=reader["EXTRA"] as string;
            if (!string.IsNullOrEmpty(extra) && extra.Trim().Equals("auto_increment",StringComparison.CurrentCultureIgnoreCase)) 
            {
                isIdentity = true;
            }
            string columnKey = reader["COLUMN_KEY"] as string;
            if (!string.IsNullOrEmpty(columnKey) && columnKey.Trim().Equals("PRI", StringComparison.CurrentCultureIgnoreCase))
            {
                isPrimaryKey = true;
            }
            if (isPrimaryKey)
            {
                type = EntityPropertyType.PrimaryKey;

            }
            if (isIdentity)
            {
                type = type | EntityPropertyType.Identity;
            }
            prm.PropertyType = type;

            if (!(reader["CHARACTER_OCTET_LENGTH"] is DBNull)) 
            {
                prm.Length = ValueConvertExtend.ConvertValue<long>(reader["CHARACTER_OCTET_LENGTH"]);
            }
            
            if (!table.IsView)
            {
                prm.Description = reader["COLUMN_COMMENT"] as string;
            }

            prm.AllowNull = ValueConvertExtend.ConvertValue<int>(reader["IS_NULLABLE"]) == 1;

            string strDBType = reader["COLUMN_TYPE"] as string;
            bool isUnsigned = strDBType.IndexOf("unsigned") > -1;

            string strDataType= reader["DATA_TYPE"] as string;
            prm.SqlType = GetDbType(strDataType, isUnsigned);
            table.Params.Add(prm);
        }
        /// <summary>
        /// 获取数据库类型
        /// </summary>
        /// <param name="type"></param>
        /// <param name="isUnsigned"></param>
        /// <returns></returns>
        private static DbType GetDbType(string type, bool isUnsigned)
        {
            switch (type.ToLower())
            {
                case "bit":
                    return DbType.Boolean;

                case "tinyint":
                    if (isUnsigned)
                    {
                        return DbType.Byte;
                    }
                    return DbType.SByte;

                case "smallint":
                    if (isUnsigned)
                    {
                        return DbType.UInt16;
                    }
                    return DbType.Int16;

                case "mediumint":
                case "int":
                    if (isUnsigned)
                    {
                        return DbType.UInt32;
                    }
                    return DbType.Int32;

                case "bigint":
                    if (isUnsigned)
                    {
                        return DbType.UInt64;
                    }
                    return DbType.Int64;

                case "float":
                    return DbType.Single;

                case "double":
                    return DbType.Double;

                case "decimal":
                    return DbType.Decimal;

                case "date":
                    return DbType.Date;

                case "datetime":
                    return DbType.DateTime;

                case "timestamp":
                    return DbType.DateTime;

                case "time":
                    return DbType.Time;

                case "tinytext":
                case "mediumtext":
                case "longtext":
                case "text":
                    return DbType.String;
                case "char":
                    return DbType.AnsiStringFixedLength;
                case "varchar":
                    return DbType.String;

                case "blob":
                    return DbType.Binary;
            }
            return DbType.Object;
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

        }

        #endregion
    }
}
