using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DB.DbCommon;
using System.Data;
using Buffalo.DB.DataBaseAdapter.IDbAdapters;
using Buffalo.DB.PropertyAttributes;
using Buffalo.DB.DataBaseAdapter;



namespace Buffalo.Data.DB2
{
    /// <summary>
    /// 数据库结构特性类
    /// </summary>
    public class DBStructure : IDBStructure
    {
        private static string _sqlTable = "select TABNAME,TYPE,REMARKS from SYSCAT.TABLES where TABSCHEMA=current schema";
        #region IDBStructure 成员

        public List<DBTableInfo> GetAllTableName(DataBaseOperate oper, DBInfo info)
        {
            List<DBTableInfo> lstName = new List<DBTableInfo>();
            ParamList lstParam = new ParamList();
            //填充表
            using (IDataReader reader = oper.Query(_sqlTable, lstParam,null))
            {
                while (reader.Read())
                {
                    DBTableInfo tableInfo = new DBTableInfo();
                    if (reader.IsDBNull(0))
                    {
                        continue;
                    }
                    tableInfo.Name = reader["TABNAME"] as string;
                    tableInfo.Description = reader["REMARKS"] as string;
                    tableInfo.IsView=false;
                    string tableType = reader["TYPE"] as string;
                    if(tableType!=null && tableType.Trim().Equals("V",StringComparison.CurrentCultureIgnoreCase))
                    {
                        tableInfo.IsView = true;
                    }

                    lstName.Add(tableInfo);
                }
            }
            return lstName;
        }

        public string GetAddParamSQL()
        {
            return "ADD COLUMN";
        }

        public List<TableRelationAttribute> GetRelation(DataBaseOperate oper, DBInfo info, IEnumerable<string> childNames)
        {
            string sql = "select CONSTNAME,FK_COLNAMES,TABSCHEMA,TABNAME,REFTABSCHEMA, REFTABNAME,REFKEYNAME,PK_COLNAMES from SYSCAT.REFERENCES where 1=1";
            StringBuilder sqlFk = new StringBuilder();
            StringBuilder sqlPk = new StringBuilder();
            sqlFk.Append(sql);
            sqlPk.Append(sql);
            ParamList lstParam=new ParamList();
            string childName = Buffalo.DB.DataBaseAdapter.SqlServer2KAdapter.DBStructure.AllInTableNames(childNames);
            if(!string.IsNullOrEmpty(childName))
            {
                sqlFk.Append(" and TABNAME in(" + childName + ")");
            }
            if (!string.IsNullOrEmpty(childName))
            {
                sqlPk.Append(" and REFTABNAME in(" + childName + ")");
            }
            List<TableRelationAttribute> lst = new List<TableRelationAttribute>();

            using (IDataReader reader = info.DefaultOperate.Query(sqlFk.ToString(), lstParam, null)) 
            {
                while (reader.Read()) 
                {
                    TableRelationAttribute tinfo = new TableRelationAttribute();
                    tinfo.Name = (reader["CONSTNAME"] as string);
                    if (!string.IsNullOrEmpty(tinfo.Name)) 
                    {
                        tinfo.Name=tinfo.Name.Trim();
                    }
                    tinfo.SourceTable = reader["TABNAME"] as string;
                    if (!string.IsNullOrEmpty(tinfo.SourceTable))
                    {
                        tinfo.SourceTable = tinfo.SourceTable.Trim();
                    }
                    tinfo.SourceName = reader["FK_COLNAMES"] as string;
                    if (!string.IsNullOrEmpty(tinfo.SourceName))
                    {
                        tinfo.SourceName = tinfo.SourceName.Trim();
                    }
                    tinfo.TargetTable = reader["REFTABNAME"] as string;
                    if (!string.IsNullOrEmpty(tinfo.TargetTable))
                    {
                        tinfo.TargetTable = tinfo.TargetTable.Trim();
                    }
                    tinfo.TargetName = reader["PK_COLNAMES"] as string;
                    if (!string.IsNullOrEmpty(tinfo.TargetName))
                    {
                        tinfo.TargetName= tinfo.TargetName.Trim();
                    }
                    tinfo.IsParent = true;
                    lst.Add(tinfo);
                }
            }
            using (IDataReader reader = info.DefaultOperate.Query(sqlPk.ToString(), lstParam, null))
            {
                while (reader.Read())
                {
                    TableRelationAttribute tinfo = new TableRelationAttribute();
                    tinfo.Name = (reader["CONSTNAME"] as string);
                    if (!string.IsNullOrEmpty(tinfo.Name))
                    {
                        tinfo.Name = tinfo.Name.Trim();
                    }
                    tinfo.SourceTable = reader["REFTABNAME"] as string;
                    if (!string.IsNullOrEmpty(tinfo.SourceTable))
                    {
                        tinfo.SourceTable = tinfo.SourceTable.Trim();
                    }
                    tinfo.SourceName = reader["PK_COLNAMES"] as string;
                    if (!string.IsNullOrEmpty(tinfo.SourceName))
                    {
                        tinfo.SourceName = tinfo.SourceName.Trim();
                    }
                    tinfo.TargetTable = reader["TABNAME"] as string;
                    if (!string.IsNullOrEmpty(tinfo.TargetTable))
                    {
                        tinfo.TargetTable = tinfo.TargetTable.Trim();
                    }
                    tinfo.TargetName = reader["FK_COLNAMES"] as string;
                    if (!string.IsNullOrEmpty(tinfo.TargetName))
                    {
                        tinfo.TargetName = tinfo.TargetName.Trim();
                    }
                    tinfo.IsParent = false;
                    lst.Add(tinfo);
                }
            }
            return lst;
        }


        public List<DBTableInfo> GetTablesInfo(DataBaseOperate oper, DBInfo info, IEnumerable<string> tableNames)
        {
            string inTable = Buffalo.DB.DataBaseAdapter.SqlServer2KAdapter.DBStructure.AllInTableNames(tableNames);
            string sql = "SELECT  cols.TABNAME, cols.COLNAME, cols.TYPENAME, cols.LENGTH, cols.REMARKS, cols.IDENTITY, cols.KEYSEQ, cols.NULLS,tables.TYPE,tables.REMARKS as TABREMARK FROM SYSCAT.COLUMNS as cols inner join SYSCAT.TABLES as tables on tables.TABNAME=cols.TABNAME where cols.TABSCHEMA=current schema";
            string tableNamesSql = "";
            if (!string.IsNullOrEmpty(inTable))
            {
                sql += " and cols.TABNAME in(" + inTable + ")";
            }

            List<DBTableInfo> lst = new List<DBTableInfo>();
            Dictionary<string, DBTableInfo> dicTables = new Dictionary<string, DBTableInfo>();
            using (IDataReader reader = oper.Query(sql.ToString(), new ParamList(),null))
            {

                while (reader.Read())
                {
                    string tableName = reader["TABNAME"] as string;
                    if (string.IsNullOrEmpty(tableName))
                    {
                        continue;
                    }
                    DBTableInfo table = null;
                    dicTables.TryGetValue(tableName, out table);
                    if (table == null)
                    {
                        table = new DBTableInfo();
                        table.Name = tableName;

                        string type = reader["TYPE"] as string;
                        table.IsView = false;
                        if (!string.IsNullOrEmpty(type))
                        {
                            if (type.Trim() == "V")
                            {
                                table.IsView = true;
                            }
                        }
                        if (!table.IsView)
                        {
                            table.Description = reader["TABREMARK"] as string;
                        }
                        
                        table.RelationItems = new List<TableRelationAttribute>();
                        table.Params = new List<EntityParam>();
                        lst.Add(table);
                        dicTables[table.Name] = table;
                    }
                    FillParam(table, reader);

                }
            }

            List<TableRelationAttribute> lstRelation = GetRelation(oper, info, tableNames);

            Buffalo.DB.DataBaseAdapter.SqlServer2KAdapter.DBStructure.FillRelation(dicTables, lstRelation);
            return lst;
        }
        /// <summary>
        /// 填充字段信息
        /// </summary>
        /// <param name="prm">字段信息</param>
        /// <param name="reader">reader</param>
        private void FillParam(DBTableInfo table, IDataReader reader)
        {
            string prmName = reader["COLNAME"] as string;
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
            int isPrimary = 0;
            object val =reader["KEYSEQ"];
            if (!(val is DBNull)) 
            {
                isPrimary = Convert.ToInt32(val);
            }

            val = reader["IDENTITY"];
            bool isIdentity =false;
            if (!(val is DBNull))
            {
                isIdentity = val.ToString().Equals("Y", StringComparison.CurrentCultureIgnoreCase);
            }
            if (isPrimary == 1)
            {
                type = EntityPropertyType.PrimaryKey;
            }
            if (isIdentity )
            {
                type = type | EntityPropertyType.Identity;
            }

            val = reader["NULLS"];
            bool isNull = false;
            if (!(val is DBNull))
            {
                isNull = val.ToString().Equals("Y", StringComparison.CurrentCultureIgnoreCase);
            }
            prm.AllowNull = isNull;
            prm.PropertyType = type;
            prm.Length = Convert.ToInt64(reader["LENGTH"]);
            if (!table.IsView)
            {
                prm.Description = reader["REMARKS"] as string;
            }
            string strDBType = reader["TYPENAME"] as string;
            prm.SqlType = GetDbType(strDBType);
            table.Params.Add(prm);
        }

        private static DbType GetDbType(string nativeType)
        {
            int index = nativeType.IndexOf('(');
            if (index > 0)
            {
                nativeType = nativeType.Substring(0, index);
            }

            switch (nativeType.Trim().ToUpper())
            {
                case "BIGINT":
                    return DbType.Int64;

                case "BLOB":
                    return DbType.Binary;

                case "CHAR":
                case "CHARACTER":
                    return DbType.AnsiStringFixedLength;
                case "CLOB":
                    return DbType.AnsiString;

                case "DATE":
                    return DbType.Date;

                case "DBCLOB":
                    return DbType.String;
                case "DECIMAL":
                    return DbType.Decimal;
                case "DOUBLE":
                    return DbType.Double;
                case "GRAPHIC":
                    return DbType.StringFixedLength;
                case "INTEGER":
                    return DbType.Int32;
                case "LONG VARCHAR":
                    return DbType.AnsiString;
                case "LONG VARGRAPHIC":
                    return DbType.String;
                case "REAL":
                    return DbType.Single;
                case "SMALLINT":
                    return DbType.Int16;
                case "TIME":
                    return DbType.Time;
                case "TIMESTAMP":
                    return DbType.DateTime;
                case "VARCHAR":
                    return DbType.AnsiString;
                case "VARGRAPHIC":
                    return DbType.String;
                case "XML":
                    return DbType.Xml;
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
