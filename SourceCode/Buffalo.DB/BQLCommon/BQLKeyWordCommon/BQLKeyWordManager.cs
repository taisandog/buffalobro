using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DB.BQLCommon.BQLConditionCommon;
using Buffalo.DB.DbCommon;
using Buffalo.DB.DataBaseAdapter;
using Buffalo.DB.QueryConditions;
using Buffalo.DB.CommBase.DataAccessBases.AliasTableMappingManagers;

namespace Buffalo.DB.BQLCommon.BQLKeyWordCommon
{
    public delegate string DelKeyWordHandle(BQLQuery handle);
    public class BQLKeyWordManager
    {

        /// <summary>
        /// 创建查询的中转信息类
        /// </summary>
        /// <returns></returns>
        private static KeyWordInfomation CreateKeywordInfo(DBInfo db) 
        {
            KeyWordInfomation info = new KeyWordInfomation();
            info.DBInfo = db;
            info.Infos = new BQLInfos();
            return info;
        }
        /// <summary>
        /// 输出关键字转换后的SQL语句
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public static AbsCondition ToCondition(BQLQuery item, DBInfo db, TableAliasNameManager aliasManager, bool isPutPropertyName)
        {
            KeyWordInfomation info = CreateKeywordInfo(db);
            info.AliasManager = aliasManager;
            info.Infos.IsPutPropertyName = isPutPropertyName;
            return DoConver(info, item);
        }
        /// <summary>
        /// 进行转换
        /// </summary>
        /// <param name="info"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        private static AbsCondition DoConver(KeyWordInfomation info, BQLQuery item) 
        {
            KeyWordConver conver = new KeyWordConver();
            AbsCondition con=conver.ToConver(item, info);
            con.AliasManager = info.AliasManager;
            con.DbParamList = info.ParamList;
            con.CacheTables = info.ContainTables;
            return con;
        }
    }
}
