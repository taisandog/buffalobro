using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using Buffalo.DB.BQLCommon.BQLKeyWordCommon;
using Buffalo.DB.PropertyAttributes;
using Buffalo.DB.BQLCommon.BQLConditionCommon;
using Buffalo.DB.DataBaseAdapter;
using Buffalo.DB.EntityInfos;
using Buffalo.DB.DbCommon;
using Buffalo.DB.CommBase.BusinessBases;

namespace Buffalo.DB.DBCheckers
{
    /// <summary>
    /// ������ݿ�
    /// </summary>
    public class DBChecker
    {

        /// <summary>
        /// ִ��SQL���
        /// </summary>
        /// <param name="oper">���ݿ�����</param>
        /// <param name="lstSQL">SQL���</param>
        /// <returns></returns>
        public static List<string> ExecuteSQL(DataBaseOperate oper,List<string> lstSQL) 
        {
            List<string> resaults = new List<string>();
            using (BatchAction ba = oper.StarBatchAction())
            {
                foreach (string sql in lstSQL)
                {
                    try
                    {
                        int row = oper.Execute(sql, new Buffalo.DB.DbCommon.ParamList(),null);
                        resaults.Add("ִ�����;");
                    }
                    catch (Exception ex)
                    {
                        resaults.Add("ִ�д���" + ex.Message);
                    }
                }
            }
            return resaults;
        }


        /// <summary>
        /// ������ݿ�
        /// </summary>
        /// <param name="db">���ݿ�</param>
        /// <returns></returns>
        public static List<string> CheckDataBase(DBInfo db) 
        {
            List<BQLEntityTableHandle> tables = db.GetAllTables();
            List<KeyWordTableParamItem> lstTable=new List<KeyWordTableParamItem>();
            
            foreach (BQLEntityTableHandle entity in tables) 
            {
                EntityInfoHandle entityInfo = entity.GetEntityInfo();
                string tableName = entityInfo.TableName;
                if (string.IsNullOrEmpty(tableName)) 
                {
                    continue;
                }
                
                KeyWordTableParamItem tableInfo = new KeyWordTableParamItem(tableName, null);
                FillParamInfos(tableInfo, entityInfo);
                FillRelation(tableInfo, entityInfo);
                lstTable.Add(tableInfo);
            }
            List<string> sqls = TableChecker.CheckTable(db, lstTable);
            return sqls;
        }

        /// <summary>
        /// ����ֶ���Ϣ
        /// </summary>
        /// <param name="tableInfo">����Ϣ</param>
        /// <param name="entityInfo">ʵ����Ϣ</param>
        private static void FillParamInfos(KeyWordTableParamItem tableInfo,EntityInfoHandle entityInfo) 
        {
            List<EntityParam> prms = new List<EntityParam>();
            foreach (EntityPropertyInfo pkInfo in entityInfo.PrimaryProperty)
            {
                prms.Add(pkInfo.ParamInfo);
            }
            foreach ( EntityPropertyInfo pInfo in entityInfo.PropertyInfo) 
            {

                if (pInfo.IsPrimaryKey) 
                {
                    continue;
                }
                prms.Add(pInfo.ParamInfo);
            }
            tableInfo.Params = prms;
        }

        /// <summary>
        /// ����ϵ
        /// </summary>
        /// <param name="tableInfo">����Ϣ</param>
        /// <param name="entityInfo">ʵ����Ϣ</param>
        private static void FillRelation(KeyWordTableParamItem tableInfo, EntityInfoHandle entityInfo) 
        {
            List<TableRelationAttribute> trs = new List<TableRelationAttribute>();

            foreach ( EntityMappingInfo info in entityInfo.MappingInfo)
            {
                TableRelationAttribute tableAttr = info.MappingInfo;
                if (!tableAttr.IsParent || !tableAttr.IsToDB)
                {
                    continue;
                }
                trs.Add(tableAttr);
            }
            tableInfo.RelationItems = trs;
        }
    }
}
