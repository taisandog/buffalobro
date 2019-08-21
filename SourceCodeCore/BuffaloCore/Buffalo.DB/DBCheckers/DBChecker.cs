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
    /// 检查数据库
    /// </summary>
    public class DBChecker
    {

        /// <summary>
        /// 执行SQL语句
        /// </summary>
        /// <param name="oper">数据库连接</param>
        /// <param name="lstSQL">SQL语句</param>
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
                        resaults.Add("执行完毕;");
                    }
                    catch (Exception ex)
                    {
                        resaults.Add("执行错误：" + ex.Message);
                    }
                }
            }
            return resaults;
        }


        /// <summary>
        /// 检查数据库
        /// </summary>
        /// <param name="db">数据库</param>
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
        /// 填充字段信息
        /// </summary>
        /// <param name="tableInfo">表信息</param>
        /// <param name="entityInfo">实体信息</param>
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
        /// 填充关系
        /// </summary>
        /// <param name="tableInfo">表信息</param>
        /// <param name="entityInfo">实体信息</param>
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
