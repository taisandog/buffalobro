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
        /// ������ѯ����ת��Ϣ��
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
        /// ����ؼ���ת�����SQL���
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
        /// ����ת��
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
