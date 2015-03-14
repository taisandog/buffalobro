using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DB.DbCommon;
using System.Data;
using Buffalo.DB.DataBaseAdapter.IDbAdapters;
using Buffalo.DB.PropertyAttributes;
using Buffalo.DB.DBCheckers;



namespace Buffalo.DB.DataBaseAdapter.Oracle9Adapter
{
    /// <summary>
    /// 数据库结构特性类
    /// </summary>
    public class DBStructure : IDBStructure
    {

        private static string _sqlTables = "select user_tables.\"TABLE_NAME\",user_tab_comments.\"COMMENTS\"  from user_tables left join user_tab_comments on user_tables.TABLE_NAME=user_tab_comments.TABLE_NAME where 1=1";
        private static string _sqlViews = "select user_views.\"VIEW_NAME\",user_tab_comments.\"COMMENTS\" from user_views left join user_tab_comments on user_views.VIEW_NAME=user_tab_comments.TABLE_NAME where 1=1";
        #region IDBStructure 成员

        public List<DBTableInfo> GetAllTableName(DataBaseOperate oper, DBInfo info)
        {
            return FillAllTableInfos(oper, info, null);
        }

        public string GetAddParamSQL()
        {
            return "add";
        }


        public List<TableRelationAttribute> GetRelation(DataBaseOperate oper, DBInfo info, IEnumerable<string> childNames)
        {
            string sql = "select b.table_name as pktable_name,b.column_name pkcolumn_name,c.table_name fktable_name,c.column_name fkcolumn_name,c.position ke_seq,c.constraint_name fk_name from (select * from user_cons_columns ) b left join (select * from user_constraints where user_constraints.constraint_type='R' ) a on  b.constraint_name=a.r_constraint_name left join user_cons_columns c on  c.constraint_name=a.constraint_name where c.position is not null and c.position=b.position ";
            StringBuilder sqlFk = new StringBuilder(1024);
            StringBuilder sqlPk = new StringBuilder(1024);
            ParamList lstParam = new ParamList();
            sqlFk.Append(sql);
            sqlPk.Append(sql);
            string childName = Buffalo.DB.DataBaseAdapter.SqlServer2KAdapter.DBStructure.AllInTableNames(childNames);
            if (!string.IsNullOrEmpty(childName)) 
            {
                sqlFk.Append("and c.table_name in(" + childName + ")");
            }
            if (!string.IsNullOrEmpty(childName))
            {
                sqlPk.Append("and  b.table_name in(" + childName + ")");
            }
            List<TableRelationAttribute> lst = new List<TableRelationAttribute>();

            using (IDataReader reader = info.DefaultOperate.Query(sqlFk.ToString(), lstParam, null))
            {
                while (reader.Read())
                {
                    TableRelationAttribute tinfo = new TableRelationAttribute();
                    tinfo.Name = reader["FK_NAME"] as string;
                    tinfo.SourceTable = reader["FKTABLE_NAME"] as string;
                    tinfo.SourceName = reader["FKCOLUMN_NAME"] as string;
                    tinfo.TargetTable = reader["PKTABLE_NAME"] as string;
                    tinfo.TargetName = reader["PKCOLUMN_NAME"] as string;
                    tinfo.IsParent = true;
                    lst.Add(tinfo);
                }
            }
            using (IDataReader reader = info.DefaultOperate.Query(sqlPk.ToString(), lstParam, null))
            {
                while (reader.Read())
                {
                    TableRelationAttribute tinfo = new TableRelationAttribute();
                    tinfo.Name = reader["FK_NAME"] as string;
                    tinfo.TargetTable = reader["FKTABLE_NAME"] as string;
                    tinfo.TargetName = reader["FKCOLUMN_NAME"] as string;
                    tinfo.SourceTable = reader["PKTABLE_NAME"] as string;
                    tinfo.SourceName = reader["PKCOLUMN_NAME"] as string;
                    tinfo.IsParent = false;
                    lst.Add(tinfo);
                }
            }
            return lst;

        }

        /// <summary>
        /// 填充所有表信息
        /// </summary>
        private List<DBTableInfo> FillAllTableInfos(DataBaseOperate oper, DBInfo info, IEnumerable<string> tableNames) 
        {
            string inTable = Buffalo.DB.DataBaseAdapter.SqlServer2KAdapter.DBStructure.AllInTableNames(tableNames);

            ParamList lstParam = new ParamList();

            List<DBTableInfo> lstName = new List<DBTableInfo>();
            string sql = _sqlTables;
            if (!string.IsNullOrEmpty(inTable))
            {
                sql += " and user_tables.TABLE_NAME in(" + inTable + ")";
            }
            
            //填充表
            using (IDataReader reader = oper.Query(sql, lstParam, null))
            {
                while (reader.Read())
                {
                    DBTableInfo tableInfo = new DBTableInfo();
                    if (reader.IsDBNull(0))
                    {
                        continue;
                    }
                    tableInfo.Name = reader["TABLE_NAME"] as string;
                    tableInfo.Description = reader["COMMENTS"] as string;
                    tableInfo.IsView = false;

                    lstName.Add(tableInfo);
                }
            }
            sql = _sqlViews;
            if (!string.IsNullOrEmpty(inTable))
            {
                sql += " and user_views.VIEW_NAME in(" + inTable + ")";
            }
            //填充视图
            using (IDataReader reader = oper.Query(sql, lstParam, null))
            {
                while (reader.Read())
                {
                    DBTableInfo tableInfo = new DBTableInfo();
                    if (reader.IsDBNull(0))
                    {
                        continue;
                    }
                    tableInfo.Name = reader["VIEW_NAME"] as string;
                    tableInfo.Description = reader["COMMENTS"] as string;
                    tableInfo.IsView = true;

                    lstName.Add(tableInfo);
                }
            }

            return lstName;
        }

        /// <summary>
        /// 获取数据库表信息
        /// </summary>
        /// <param name="oper"></param>
        /// <param name="info"></param>
        /// <param name="tableNames"></param>
        /// <returns></returns>
        public List<DBTableInfo> GetTablesInfo(DataBaseOperate oper, DBInfo info, IEnumerable<string> tableNames)
        {
            string inTable = Buffalo.DB.DataBaseAdapter.SqlServer2KAdapter.DBStructure.AllInTableNames(tableNames);
            StringBuilder sql = new StringBuilder();
            sql.Append("SELECT USER_TAB_COLUMNS.TABLE_NAME,USER_TAB_COLUMNS.COLUMN_NAME , USER_TAB_COLUMNS.DATA_TYPE,USER_TAB_COLUMNS.DATA_LENGTH,USER_TAB_COLUMNS.NULLABLE,USER_TAB_COLUMNS.COLUMN_ID,user_col_comments.comments FROM USER_TAB_COLUMNS left join user_col_comments on user_col_comments.TABLE_NAME=USER_TAB_COLUMNS.TABLE_NAME and user_col_comments.COLUMN_NAME=USER_TAB_COLUMNS.COLUMN_NAME where 1=1");


            if (!string.IsNullOrEmpty(inTable))
            {
                sql.Append(" and USER_TAB_COLUMNS.TABLE_NAME in(" + inTable + ")");
            }

            List<DBTableInfo> lst = FillAllTableInfos(oper,info,tableNames);
            Dictionary<string, Dictionary<string,bool>> dicPkMap = GetPrimaryKeyMap(oper, info, tableNames);
            Dictionary<string, DBTableInfo> dicTables = new Dictionary<string, DBTableInfo>();

            foreach (DBTableInfo table in lst) 
            {
                dicTables[table.Name] = table;
                table.Params = new List<EntityParam>();
                table.RelationItems = new List<TableRelationAttribute>();
            }

            using (IDataReader reader = oper.Query(sql.ToString(), new ParamList(), null))
            {
                Dictionary<string, bool> dicPkNames = null;
                while (reader.Read())
                {
                    string tableName = reader["TABLE_NAME"] as string;
                    if (string.IsNullOrEmpty(tableName)) 
                    {
                        continue;
                    }
                    DBTableInfo table = null;
                    if (dicTables.TryGetValue(tableName, out table))
                    {

                        if (dicPkMap.TryGetValue(tableName, out dicPkNames))
                        {
                            FillParam(table, reader, dicPkNames);
                        }
                        else 
                        {
                            FillParam(table, reader, new Dictionary<string,bool>());
                        }
                    }
                }
            }

            List<TableRelationAttribute> lstRelation = GetRelation(oper, info, tableNames);
            Buffalo.DB.DataBaseAdapter.SqlServer2KAdapter.DBStructure.FillRelation(dicTables, lstRelation);

            return lst;
        }

        /// <summary>
        /// 获取表的主键映射
        /// </summary>
        /// <param name="lst"></param>
        /// <returns></returns>
        private Dictionary<string, Dictionary<string,bool>> GetPrimaryKeyMap(DataBaseOperate oper, DBInfo info, IEnumerable<string> lst) 
        {
            string inTable = Buffalo.DB.DataBaseAdapter.SqlServer2KAdapter.DBStructure.AllInTableNames(lst);
            StringBuilder sql = new StringBuilder();
            sql.Append("select cu.* from user_cons_columns cu, user_constraints au where cu.constraint_name = au.constraint_name and au.constraint_type = 'P'");
            if (!string.IsNullOrEmpty(inTable))
            {
                sql.Append(" and au.table_name  in (" + inTable + ")");
            }
            Dictionary<string, Dictionary<string, bool>> dic = new Dictionary<string, Dictionary<string, bool>>();

            using (IDataReader reader = oper.Query(sql.ToString(), new ParamList(), null))
            {

                while (reader.Read())
                {
                    string tableName = reader["TABLE_NAME"] as string;
                    if (string.IsNullOrEmpty(tableName))
                    {
                        continue;
                    }
                    string prmName = reader["COLUMN_NAME"] as string;
                    if (string.IsNullOrEmpty(prmName))
                    {
                        continue;
                    }
                    Dictionary<string, bool> dicKeys=null;
                    dic.TryGetValue(tableName, out dicKeys);
                    if (dicKeys == null) 
                    {
                        dicKeys = new Dictionary<string, bool>();
                        dic[tableName] = dicKeys;
                    }
                    dicKeys[prmName] = true;
                }
            }
            return dic;
        }

        /// <summary>
        /// 填充字段信息
        /// </summary>
        /// <param name="prm">字段信息</param>
        /// <param name="reader">reader</param>
        private void FillParam(DBTableInfo table, IDataReader reader, Dictionary<string, bool> pkNames)
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
            int isIdentity = 0;
            
            if (pkNames.ContainsKey(prmName))
            {
                type = EntityPropertyType.PrimaryKey;

                //isIdentity = 1;

            }
            //if (isIdentity == 1)
            //{
            //    type = type | EntityPropertyType.Identity;
            //}
            prm.PropertyType = type;
            prm.Length = Convert.ToInt64(reader["DATA_LENGTH"]);
            if (!table.IsView)
            {
                prm.Description = reader["COMMENTS"] as string;
            }

            object val = reader["NULLABLE"];
            bool isNull = false;
            if (!(val is DBNull))
            {
                isNull = val.ToString().Equals("Y", StringComparison.CurrentCultureIgnoreCase);
            }

            prm.AllowNull = isNull;

            string strDBType = reader["DATA_TYPE"] as string;
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
                case "BFILE":
                    return DbType.Binary;

                case "BLOB":
                    return DbType.Binary;

                case "CHAR":
                    return DbType.AnsiStringFixedLength;

                case "CLOB":
                    return DbType.AnsiString;

                case "DATE":
                    return DbType.DateTime;

                case "FLOAT":
                    return DbType.Decimal;

                case "INTEGER":
                    return DbType.Decimal;

                case "UNSIGNED INTEGER":
                    return DbType.Decimal;

                case "INTERVAL YEAR TO MONTH":
                    return DbType.Int32;

                case "INTERVAL DAY TO SECOND":
                    return DbType.Object;

                case "LONG":
                    return DbType.AnsiString;

                case "LONG RAW":
                    return DbType.Binary;

                case "NCHAR":
                    return DbType.StringFixedLength;

                case "NCLOB":
                    return DbType.String;

                case "NUMBER":
                    return DbType.VarNumeric;

                case "NVARCHAR2":
                    return DbType.String;

                case "RAW":
                    return DbType.Binary;

                case "REF CURSOR":
                    return DbType.Object;

                case "ROWID":
                    return DbType.AnsiString;

                case "TIMESTAMP":
                    return DbType.DateTime;

                case "TIMESTAMP WITH LOCAL TIME ZONE":
                    return DbType.DateTime;

                case "TIMESTAMP WITH TIME ZONE":
                    return DbType.DateTime;

                case "VARCHAR2":
                    return DbType.AnsiString;

                case "BINARY_DOUBLE":
                    return DbType.Decimal;

                case "BINARY_FLOAT":
                    return DbType.Decimal;

                case "BINARY_INTEGER":
                    return DbType.Decimal;

                case "PLS_INTEGER":
                    return DbType.Decimal;

                case "Collection":
                    return DbType.String;

                case "UROWID":
                    return DbType.String;

                case "XMLType":
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
