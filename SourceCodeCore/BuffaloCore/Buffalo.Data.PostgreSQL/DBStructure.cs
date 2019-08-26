using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DB.DbCommon;
using System.Data;
using Buffalo.DB.DataBaseAdapter.IDbAdapters;
using Buffalo.DB.PropertyAttributes;
using Buffalo.DB.DataBaseAdapter;



namespace Buffalo.Data.PostgreSQL
{
    /// <summary>
    /// 数据库结构特性类
    /// </summary>
    public class DBStructure : IDBStructure
    {
       
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
            string sql = "select table_name,table_type from information_schema.tables where table_schema = 'public'";
            
            List<DBTableInfo> lstName = new List<DBTableInfo>();
            string inTable = Buffalo.DB.DataBaseAdapter.SqlServer2KAdapter.DBStructure.AllInTableNames(tableNames);
            if (!string.IsNullOrEmpty(inTable))
            {
                sql += " and table_name in(" + inTable + ")";
            }

            using (IDataReader reader = oper.Query(sql, lstParam,null))
            {
                while (reader.Read())
                {
                    DBTableInfo tableInfo = new DBTableInfo();
                    tableInfo.Name = reader[0] as string;
                    string type = reader[1] as string;
                    tableInfo.IsView = false;
                    if (!string.IsNullOrEmpty(type)) 
                    {
                        tableInfo.IsView = type.Equals("VIEW",StringComparison.CurrentCultureIgnoreCase);
                    }
                    tableInfo.Description = "";

                    lstName.Add(tableInfo);
                }
            }


            
            return lstName;

        }

        /// <summary>
        /// 添加字段的语句xrrrr

        /// <summary>
        /// 获取所有关系
        /// </summary>
        /// <param name="oper"> </param>
        /// <param name="info"> </param>
        /// <param name="childNames">null则查询所有表</param>
        /// <returns></returns>
        public List<TableRelationAttribute> GetRelation(DataBaseOperate oper, DBInfo info, IEnumerable<string> childNames)
        {
            StringBuilder sqlFk = new StringBuilder();
            StringBuilder sqlPk = new StringBuilder();
            string sql = "select px.conname as constraintname, px.contype, home.relname as thisname, fore.relname as theirname, px.conrelid as homeid, px.confrelid as foreid, px.conkey as thiscols, px.confkey as fcols, att.attname as colname, fatt.attname as fcolname, px.confupdtype, px.confdeltype from information_schema.table_constraints tc inner join pg_constraint px on (px.conname=tc.constraint_name) left join pg_class home on (home.oid = px.conrelid) left join pg_class fore on (fore.oid = px.confrelid) right join pg_attribute att on (att.attrelid = px.conrelid AND att.attnum = ANY(px.conkey)) right join pg_attribute fatt on (fatt.attrelid = px.confrelid AND fatt.attnum = ANY(px.confkey)) where tc.constraint_type='FOREIGN KEY'";
            //sql.Append("SELECT constraint_schema,constraint_name,unique_constraint_name,table_name,referenced_table_name FROM `information_schema`.`REFERENTIAL_CONSTRAINTS`;");
            sqlFk.Append(sql);
            sqlPk.Append(sql);
            ParamList lstParam = new ParamList();
            string childName = Buffalo.DB.DataBaseAdapter.SqlServer2KAdapter.DBStructure.AllInTableNames(childNames);

            if (!string.IsNullOrEmpty(childName))
            {
                sqlFk.Append(" and home.relname in(" + childName + ")");
            }
            if (!string.IsNullOrEmpty(childName))
            {
                sqlPk.Append(" and fore.relname in(" + childName + ")");
            }
            List<TableRelationAttribute> lst = new List<TableRelationAttribute>();

            using (IDataReader reader = info.DefaultOperate.Query(sqlFk.ToString(), lstParam, null))
            {
                while (reader.Read())
                {
                    TableRelationAttribute tinfo = new TableRelationAttribute();
                    tinfo.Name = reader["constraintname"] as string;
                    tinfo.SourceTable = reader["thisname"] as string;
                    tinfo.SourceName = reader["colname"] as string;
                    tinfo.TargetTable = reader["theirname"] as string;
                    tinfo.TargetName = reader["fcolname"] as string;
                    tinfo.IsParent = true;
                    lst.Add(tinfo);
                }
            }
            using (IDataReader reader = info.DefaultOperate.Query(sqlPk.ToString(), lstParam, null))
            {
                while (reader.Read())
                {
                    TableRelationAttribute tinfo = new TableRelationAttribute();
                    tinfo.Name = reader["constraintname"] as string;
                    tinfo.TargetTable = reader["thisname"] as string;
                    tinfo.TargetName = reader["colname"] as string;
                    tinfo.SourceTable = reader["theirname"] as string;
                    tinfo.SourceName = reader["fcolname"] as string;
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
            string sql = "select cols.table_name, cols.column_name, cols.is_nullable, cols.character_maximum_length, cols.numeric_precision, cols.numeric_scale, cols.data_type, cols.udt_name,pkt.constraint_type from information_schema.columns cols left join (select tc.constraint_type,tc.table_name,tc.constraint_name,ccu.column_name from information_schema.table_constraints tc inner join information_schema.constraint_column_usage ccu on ccu.constraint_name=tc.constraint_name where tc.constraint_schema='public' and tc.constraint_type='PRIMARY KEY') pkt on cols.column_name=pkt.column_name and cols.table_name=pkt.table_name where cols.table_schema = 'public'";
            if (!string.IsNullOrEmpty(inTable))
            {
                sql += " and cols.table_name in(" + inTable + ")";
            }
            Dictionary<string, DBTableInfo> dicTables = new Dictionary<string, DBTableInfo>();
            ParamList lstParam = new ParamList();
            

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
                    string tableName = reader["table_name"] as string;
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
            string prmName = reader["column_name"] as string;
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

            string columnKey = reader["constraint_type"] as string;
            if (!string.IsNullOrEmpty(columnKey))
            {
                isPrimaryKey = true;
                isIdentity = true;
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

            object val=reader["character_maximum_length"] ;
            if (!(val is DBNull)) 
            {
                prm.Length = ValueConvertExtend.ConvertValue<long>(val);
            }

            val = reader["is_nullable"];
            if (!(val is DBNull))
                prm.AllowNull = val.ToString().Equals("YES", StringComparison.CurrentCultureIgnoreCase);

            

            string strDataType = reader["udt_name"] as string;
            prm.SqlType = GetDbType(strDataType);
            table.Params.Add(prm);
        }


        private static DbType GetDbType(string type)
        {
            switch (type)
            {
                case "int8":
                    return DbType.Int64;

                case "bool":
                    return DbType.Boolean;

                case "bytea":
                    return DbType.Binary;

                case "date":
                    return DbType.Date;

                case "float8":
                    return DbType.Double;

                case "int4":
                    return DbType.Int32;

                case "money":
                    return DbType.Decimal;

                case "numeric":
                    return DbType.Decimal;

                case "float4":
                    return DbType.Single;

                case "int2":
                    return DbType.Int16;

                case "text":
                    return DbType.String;

                case "time":
                    return DbType.Time;

                case "timetz":
                    return DbType.Time;

                case "timestamp":
                    return DbType.DateTime;

                case "timestamptz":
                    return DbType.DateTime;

                case "varchar":
                    return DbType.String;

                case "character":
                    return DbType.String;

                case "bpchar":
                    return DbType.String;
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

        public string GetAddParamSQL()
        {
            return "ADD COLUMN";
        }

        #endregion
    }
}
