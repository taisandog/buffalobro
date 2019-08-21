using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DB.DataBaseAdapter;
using Buffalo.DB.BQLCommon.BQLKeyWordCommon;
using Buffalo.DB.BQLCommon.BQLBaseFunction;
using Buffalo.DB.BQLCommon;
using Buffalo.DB.QueryConditions;
using System.Data;
using Buffalo.DB.PropertyAttributes;
using Buffalo.DB.DataBaseAdapter.IDbAdapters;
using Buffalo.Kernel;


namespace Buffalo.DB.DBCheckers
{
    public class TableChecker
    {
        /// <summary>
        /// ��ȡ���б�/��ͼ��Ϣ
        /// </summary>
        /// <returns></returns>
        public static List<DBTableInfo> GetAllTables(DBInfo info) 
        {

            List<DBTableInfo> tables = info.DBStructure.GetAllTableName(info.DefaultOperate,info);
            return tables;
        }

        /// <summary>
        /// ��ȡ����Ϣ
        /// </summary>
        /// <param name="info">���ݿ�</param>
        /// <param name="tableNames">����</param>
        /// <returns></returns>
        public static List<DBTableInfo> GetTableInfo(DBInfo info, List<string> tableNames)
        {
            List<DBTableInfo> lst = info.DBStructure.GetTablesInfo(info.DefaultOperate, info, tableNames);
            foreach (DBTableInfo tableInfo in lst) 
            {
                if (tableInfo.IsView && tableInfo.Params.Count > 0) 
                {
                    tableInfo.Params[0].PropertyType = EntityPropertyType.PrimaryKey;
                }
            }


            return lst;
        }

        /// <summary>
        /// ȥ���ظ��ı�
        /// </summary>
        /// <param name="lstTable"></param>
        /// <returns></returns>
        private static List<KeyWordTableParamItem> FilterDistinct(List<KeyWordTableParamItem> lstTable) 
        {
            Dictionary<string, KeyWordTableParamItem> dicTables = new Dictionary<string, KeyWordTableParamItem>();
            KeyWordTableParamItem curItem = null;
            List<KeyWordTableParamItem> lstTables = new List<KeyWordTableParamItem>(lstTable.Count);
            foreach (KeyWordTableParamItem item in lstTable) 
            {
                string key = item.TableName.ToLower();
                if (dicTables.TryGetValue(key, out curItem)) //������Ѿ����ڣ���ϲ�����ߵ��ֶκ͹�����Ϣ
                {
                    MergeTableInfo(curItem, item);
                }
                else 
                {
                    lstTables.Add(item);
                    dicTables[key] = item;
                }
            }
            return lstTables;
        }

        /// <summary>
        /// �ϲ�ͬ������Ϣ
        /// </summary>
        /// <param name="itemSource"></param>
        /// <param name="itemTarget"></param>
        private static void MergeTableInfo(KeyWordTableParamItem itemTarget,KeyWordTableParamItem itemSource)
        {
            if (itemSource.Params != null)
            {
                if (itemTarget.Params == null) 
                {
                    itemTarget.Params = new List<EntityParam>();
                }
                MergeEntityParam(itemSource.Params, itemTarget.Params);
            }

            if (itemSource.RelationItems != null)
            {
                if (itemTarget.RelationItems == null)
                {
                    itemTarget.RelationItems = new List<TableRelationAttribute>();
                }
                MergeRelation(itemSource.RelationItems, itemTarget.RelationItems);
            }

        }

        /// <summary>
        /// �ϲ���ϵ
        /// </summary>
        private static void MergeRelation(List<TableRelationAttribute> lstSource, List<TableRelationAttribute> lstTarget)
        {
            Queue<TableRelationAttribute> queNotExists = new Queue<TableRelationAttribute>();
            foreach (TableRelationAttribute trSource in lstSource)
            {
                if (!trSource.IsParent || !trSource.IsToDB) 
                {
                    continue;
                }


                bool exists = false;//�Ƿ��Ѿ�����
                foreach (TableRelationAttribute trTarget in lstTarget)
                {
                    if (trTarget.SourceName.Equals(trSource.SourceName,StringComparison.CurrentCultureIgnoreCase) &&
                        trTarget.SourceTable.Equals(trSource.SourceTable, StringComparison.CurrentCultureIgnoreCase) &&
                        trTarget.TargetName.Equals(trSource.TargetName, StringComparison.CurrentCultureIgnoreCase) &&
                        trTarget.TargetTable.Equals(trSource.TargetTable, StringComparison.CurrentCultureIgnoreCase))
                    {
                        exists = true;
                        break;
                    }
                }

                if (!exists) //���������ʱ����򿽱���ȥ
                {
                    queNotExists.Enqueue(trSource);
                }
            }
            lstTarget.AddRange(queNotExists);//���������ڵ��ֶ���Ϣ
        }
        /// <summary>
        /// �ϲ��ֶ�
        /// </summary>
        private static void MergeEntityParam(List<EntityParam> lstSource, List<EntityParam> lstTarget) 
        {
            Queue<EntityParam> queNotExists = new Queue<EntityParam>();
            foreach (EntityParam epSource in lstSource)
            {
                bool exists = false;//�Ƿ��Ѿ�����
                foreach (EntityParam epTarget in lstTarget)
                {
                    if (epTarget.ParamName.Equals(epSource.ParamName, StringComparison.CurrentCultureIgnoreCase))
                    {
                        exists = true;
                        break;
                    }
                }

                if (!exists) //���������ʱ����򿽱���ȥ
                {
                    queNotExists.Enqueue(epSource);
                }
            }
            lstTarget.AddRange(queNotExists);//���������ڵ��ֶ���Ϣ
        }


        /// <summary>
        /// ������Ϣ
        /// </summary>
        /// <param name="info">���ݿ�</param>
        /// <param name="lstTableInfos">��Ҫ���ı�</param>
        /// <returns></returns>
        public static List<string> CheckTable(DBInfo info, List<KeyWordTableParamItem> lstTableInfos) 
        {
            List<KeyWordTableParamItem> tableInfos=FilterDistinct(lstTableInfos);


            List<DBTableInfo> tables = GetAllTables(info) ;
            BQLDbBase db = new BQLDbBase(info.CreateOperate());
            List<KeyWordTableParamItem> lstNotExists = new List<KeyWordTableParamItem>();
            List<KeyWordTableParamItem> lstExists = new List<KeyWordTableParamItem>();
            FilteExistsTable(tableInfos, tables,lstExists,lstNotExists);

            List<string> lstRet = new List<string>();

            
            CreateTableSQL(lstRet,info, lstNotExists);

            foreach (KeyWordTableParamItem existsTable in lstExists)
            {
                CheckTableStruct(lstRet,info, existsTable);
            }
            foreach (KeyWordTableParamItem table in tableInfos)
            {
                CheckRelation(lstRet,info, table);
            }

            IDBAdapter idb = info.CurrentDbAdapter;
            
            foreach (KeyWordTableParamItem table in tableInfos)
            {
                foreach (EntityParam prm in table.PrimaryParam)
                {
                    //����������(��ʱֻ��Oracle)
                    if (prm.Identity && IsIdentityType(prm.SqlType))
                    {
                        string seqName = idb.GetDefaultSequenceName(table.TableName, prm.ParamName);
                        if (!string.IsNullOrEmpty(seqName))
                        {
                            string seqSql = idb.GetSequenceInit(seqName, prm, info.DefaultOperate);

                            if (!string.IsNullOrEmpty(seqSql))
                            {
                                lstRet.Add(seqSql);
                            }
                        }
                    }
                }

            }
            return lstRet;
        }

        /// <summary>
        /// �ж����ݿ������Ƿ����Ϊ����������
        /// </summary>
        /// <param name="dbType"></param>
        /// <returns></returns>
        internal static bool IsIdentityType(DbType dbType) 
        {
            switch (dbType) 
            {
                case DbType.Int32:
                case DbType.Int16:
                case DbType.Int64:
                case DbType.SByte:
                case DbType.Byte:
                case DbType.UInt16:
                case DbType.UInt32:
                case DbType.UInt64:
                case DbType.VarNumeric:
                case DbType.Currency:
                case DbType.Decimal:
                case DbType.Double:
                    return true;
                default:
                    return false;
            }
        }

        /// <summary>
        /// �������SQL
        /// </summary>
        /// <param name="sql">sql��伯��</param>
        /// <param name="dbInfo">������Ϣ</param>
        /// <param name="notExists">�����ڵı�</param>
        private static void CreateTableSQL(List<string> sql,DBInfo dbInfo, List<KeyWordTableParamItem> notExists)
        {
            IDBAdapter ida = dbInfo.CurrentDbAdapter;
            foreach (KeyWordTableParamItem table in notExists)
            {
                dbInfo.DBStructure.OnCheckEvent(table, dbInfo, CheckEvent.TableBeginCreate,sql);
                BQLQuery bql = BQL.CreateTable(table.TableName).Param(table.Params);
                AbsCondition con = BQLKeyWordManager.ToCondition(bql, dbInfo, null, true);
                sql.Add(con.GetSql(false));

                if (!string.IsNullOrEmpty(table.Description))//���ñ�ע��
                {
                    string desSQL = ida.GetAddDescriptionSQL(table,null, dbInfo);
                    if (!string.IsNullOrEmpty(desSQL))
                    {
                        sql.Add(desSQL);
                    }
                }

                foreach (EntityParam prm in table.Params) //�����ֶ�ע��
                {
                    if (!string.IsNullOrEmpty(prm.Description))
                    {
                        string desSQL = ida.GetAddDescriptionSQL(table, prm, dbInfo);
                        if (!string.IsNullOrEmpty(desSQL))
                        {
                            sql.Add(desSQL);
                        }
                    }
                }


                dbInfo.DBStructure.OnCheckEvent(table, dbInfo, CheckEvent.TableCreated, sql);
            }
        }

       

        /// <summary>
        /// �ҳ������ڵı�
        /// </summary>
        /// <param name="tableName">Ҫͬ���ı�</param>
        /// <param name="dbTables">���ݿ�ı�</param>
        private static void FilteExistsTable(List<KeyWordTableParamItem> tableNames,
            List<DBTableInfo> dbTables, List<KeyWordTableParamItem> lstExists,
            List<KeyWordTableParamItem> lstNotExists) 

        {
            
            Dictionary<string, bool> dic = new Dictionary<string, bool>();
            foreach (DBTableInfo dbTable in dbTables) 
            {
                dic[dbTable.Name.ToLower()] = true;
            }

            foreach (KeyWordTableParamItem table in tableNames) 
            {
                string key = table.TableName.ToLower();
                if (!dic.ContainsKey(key))
                {
                    lstNotExists.Add(table);
                }
                else 
                {
                    lstExists.Add(table);
                }
            }
        }
        
        /// <summary>
        /// ����ṹ
        /// </summary>
        /// <param name="lstSql">��Ҫ���µ�SQL</param>
        /// <param name="dbInfo">���ݿ���Ϣ</param>
        /// <param name="table">Ҫ���ı�</param>
        private static void CheckTableStruct(List<string> lstSql,DBInfo dbInfo,KeyWordTableParamItem table) 
        {
            
            string tableName = table.TableName;
            BQLQuery bql=BQL.Select(BQL.ToTable(tableName)._).From(BQL.ToTable(tableName));
            SelectCondition con = BQLKeyWordManager.ToCondition(bql, dbInfo, null, true) as SelectCondition;
            string sql = dbInfo.CurrentDbAdapter.GetTopSelectSql(con, 1);
            Dictionary<string, bool> dic = new Dictionary<string, bool>();

            using (IDataReader reader = dbInfo.DefaultOperate.Query(sql, new Buffalo.DB.DbCommon.ParamList(),null)) 
            {
                for (int i = 0; i < reader.FieldCount; i++) 
                {
                    dic[reader.GetName(i).ToLower()] = true;
                }
            }

            StringBuilder sbSql = new StringBuilder();
            string desSQL = null;
            IDBAdapter idb = dbInfo.CurrentDbAdapter;
            foreach (EntityParam pInfo in table.Params) 
            {
                if (!dic.ContainsKey(pInfo.ParamName.ToLower())) 
                {
                    dbInfo.DBStructure.OnCheckEvent(pInfo, dbInfo, CheckEvent.TablenBeginCheck, lstSql);
                    bql = BQL.AlterTable(tableName).AddParam(pInfo);
                    AbsCondition acon = BQLKeyWordManager.ToCondition(bql, dbInfo, null, true);
                    lstSql.Add(acon.GetSql(false));
                    if (!string.IsNullOrEmpty(pInfo.Description))//���ע��
                    {
                        desSQL = idb.GetAddDescriptionSQL(table, pInfo, dbInfo);
                        if (!string.IsNullOrEmpty(desSQL))
                        {
                            lstSql.Add(desSQL);
                        }
                    }
                    dbInfo.DBStructure.OnCheckEvent(pInfo, dbInfo, CheckEvent.TableChecked, lstSql);
                }
            }
           
        }

        /// <summary>
        /// ����ϵ
        /// </summary>
        /// <param name="dbInfo">���ݿ�</param>
        /// <param name="table">Ҫ���ı�</param>
        /// <returns></returns>
        private static void CheckRelation(List<string> lstSql,DBInfo dbInfo, KeyWordTableParamItem table) 
        {
            List<TableRelationAttribute> lstRelation = dbInfo.DBStructure.GetRelation(dbInfo.DefaultOperate, dbInfo, new string[] { table.TableName });
            if (lstRelation == null) 
            {
                return;
            }
            foreach (TableRelationAttribute item in table.RelationItems) 
            {
                bool exists = false;
                foreach (TableRelationAttribute existsItem in lstRelation) 
                {
                    if (item.SourceName.Equals(existsItem.SourceName, StringComparison.CurrentCultureIgnoreCase)) 
                    {
                        exists = true;
                        break;
                    }
                }
                if (!exists) 
                {
                    dbInfo.DBStructure.OnCheckEvent(table, dbInfo, CheckEvent.RelationBeginCheck, lstSql);
                    item.CreateName();
                    BQLQuery bql = BQL.AlterTable(table.TableName).AddForeignkey(item);
                    AbsCondition con = BQLKeyWordManager.ToCondition(bql, dbInfo, null, true);
                    lstSql.Add(con.GetSql(false));
                    dbInfo.DBStructure.OnCheckEvent(table, dbInfo, CheckEvent.RelationChecked, lstSql);
                }
            }

        }



    }

    
}
