using System;
using System.Collections.Generic;
using Buffalo.DB.BQLCommon.BQLKeyWordCommon;
using Buffalo.DB.PropertyAttributes;
using Buffalo.DB.DbCommon;

namespace Buffalo.DB.DataBaseAdapter.IDbAdapters
{
    public interface IDBStructure
    {
        /// <summary>
        /// 获取所有表名
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        List<DBTableInfo> GetAllTableName(DataBaseOperate oper, DBInfo info);
        /// <summary>
        /// 添加字段的语句
        /// </summary>
        /// <returns></returns>
        string GetAddParamSQL();
        /// <summary>
        /// 获取所有关系
        /// </summary>
        /// <param name="chileName">子表名，查询有这里为nulls</param>
        /// <returns></returns>
        List<TableRelationAttribute> GetRelation(DataBaseOperate oper, DBInfo info, IEnumerable<string> childName);
        /// <summary>
        /// 获取表信息
        /// </summary>
        /// <param name="oper"></param>
        /// <param name="info"></param>
        /// <param name="tableNames"></param>
        /// <returns></returns>
        List<DBTableInfo> GetTablesInfo(DataBaseOperate oper, DBInfo info, IEnumerable<string> tableNames);

        #region 创建事件
        /// <summary>
        /// 数据库检查时候的事建
        /// </summary>
        /// <param name="arg">当前类型</param>
        /// <param name="dbInfo">数据库类型</param>
        /// <param name="type">检查类型</param>
        /// <param name="lstSQL">SQL语句</param>
        void OnCheckEvent(object arg,DBInfo dbInfo,CheckEvent type,List<string> lstSQL);
        #endregion
    }
}
