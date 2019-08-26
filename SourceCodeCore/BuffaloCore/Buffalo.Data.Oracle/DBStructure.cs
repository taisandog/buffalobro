using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DB.DbCommon;
using System.Data;
using Buffalo.DB.DataBaseAdapter.IDbAdapters;
using Buffalo.DB.PropertyAttributes;
using Buffalo.DB.DBCheckers;
using Buffalo.DB.DataBaseAdapter;

namespace Buffalo.Data.Oracle
{
    /// <summary>
    /// ���ݿ�ṹ������
    /// </summary>
    public class DBStructure : IDBStructure
    {

        private static string _sqlTables = "select user_tables.\"TABLE_NAME\",user_tab_comments.\"COMMENTS\"  from user_tables left join user_tab_comments on user_tables.TABLE_NAME=user_tab_comments.TABLE_NAME where 1=1";
        private static string _sqlViews = "select user_views.\"VIEW_NAME\",user_tab_comments.\"COMMENTS\" from user_views left join user_tab_comments on user_views.VIEW_NAME=user_tab_comments.TABLE_NAME where 1=1";
        #region IDBStructure ��Ա

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
        /// ������б���Ϣ
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
            
            //����
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
            //�����ͼ
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
        /// ��ȡ���ݿ����Ϣ
        /// </summary>
        /// <param name="oper"></param>
        /// <param name="info"></param>
        /// <param name="tableNames"></param>
        /// <returns></returns>
        public List<DBTableInfo> GetTablesInfo(DataBaseOperate oper, DBInfo info, IEnumerable<string> tableNames)
        {
            string inTable = Buffalo.DB.DataBaseAdapter.SqlServer2KAdapter.DBStructure.AllInTableNames(tableNames);
            StringBuilder sql = new StringBuilder();
            sql.Append("SELECT USER_TAB_COLUMNS.TABLE_NAME,USER_TAB_COLUMNS.COLUMN_NAME , USER_TAB_COLUMNS.DATA_TYPE,USER_TAB_COLUMNS.DATA_LENGTH,USER_TAB_COLUMNS.DATA_PRECISION,USER_TAB_COLUMNS.DATA_SCALE,USER_TAB_COLUMNS.NULLABLE,USER_TAB_COLUMNS.COLUMN_ID,user_col_comments.comments FROM USER_TAB_COLUMNS left join user_col_comments on user_col_comments.TABLE_NAME=USER_TAB_COLUMNS.TABLE_NAME and user_col_comments.COLUMN_NAME=USER_TAB_COLUMNS.COLUMN_NAME where 1=1");


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
        /// ��ȡ�������ӳ��
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
        /// ����ֶ���Ϣ
        /// </summary>
        /// <param name="prm">�ֶ���Ϣ</param>
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
            prm.Length = ValueConvertExtend.ConvertValue<long>(reader["DATA_LENGTH"]);
            if (!table.IsView)
            {
                prm.Description = reader["COMMENTS"] as string;
            }

            int dataPrecision =0;//��ֵ����
            
            try 
            {
                object odataPrecision = reader["DATA_PRECISION"];
                if (odataPrecision != null && !(odataPrecision is DBNull))
                {
                    dataPrecision = Convert.ToInt32(odataPrecision);
                }
            }catch{};

            int dataScale =0;//С���㳤��
            try
            {
                object odataScale = reader["DATA_SCALE"];
                if (odataScale != null && !(odataScale is DBNull))
                {
                    dataScale = Convert.ToInt32(dataScale);
                }
            }
            catch { };

            object val = reader["NULLABLE"];
            bool isNull = false;
            if (!(val is DBNull))
            {
                isNull = val.ToString().Equals("Y", StringComparison.CurrentCultureIgnoreCase);
            }

            prm.AllowNull = isNull;

            string strDBType = reader["DATA_TYPE"] as string;
            prm.SqlType = GetDbType(strDBType, prm.Length, dataPrecision, dataScale);
            table.Params.Add(prm);
        }
        private static DbType GetDbType(string nativeType,long dataLength, int dataPrecision, int dataScale)
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

                case "UNSIGNED INTEGER":
                    return DbType.Int64;

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
                case "INTEGER":
                case "SIMPLE_INTEGER":
                case "PLS_INTEGER":
                case "BINARY_INTEGER":
                    return DbType.Int32;
                case "SIMPLE_FLOAT":
                case "PLS_FLOAT":
                case "BINARY_FLOAT":
                    return DbType.Single;

                case "BINARY_DOUBLE":
                case "PLS_DOUBLE":
                case "SIMPLE_DOUBLE":
                    return DbType.Decimal;

                case "NUMBER":
                    if (dataScale == 0)
                    {
                        if (dataPrecision == 0) //��ֵ����Ϊ0��ӳ����������
                        {
                            return DbType.Int64;
                        }
                        else if (dataPrecision == 1)
                        {
                            return DbType.Boolean;
                        }
                        else if (dataPrecision <= 3)
                        {
                            return DbType.Byte;
                        }
                        else if (dataPrecision <= 4)
                        {
                            return DbType.SByte;
                        }
                        else if (dataPrecision <= 6)
                        {
                            return DbType.Int16;
                        }
                        else if (dataPrecision <= 10)
                        {
                            return DbType.Int32;
                        }
                        else
                        {
                            return DbType.Int64;
                        }
                    }
                    else 
                    {
                        return DbType.Decimal;
                    }
                    //return DbType.VarNumeric;

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

        #region �����¼�
        /// <summary>
        /// ���ݿ���ʱ����½�
        /// </summary>
        /// <param name="arg">��ǰ����</param>
        /// <param name="dbInfo">���ݿ�����</param>
        /// <param name="type">�������</param>
        /// <param name="lstSQL">SQL���</param>
        public void OnCheckEvent(object arg, DBInfo dbInfo, CheckEvent type, List<string> lstSQL) 
        {

        }

        #endregion
    }
}
