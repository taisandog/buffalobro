using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DB.BQLCommon.BQLConditionCommon;
using Buffalo.DB.DataBaseAdapter;
using Buffalo.DB.BQLCommon.IdentityInfos;
using Buffalo.DB.DataBaseAdapter.IDbAdapters;
using Buffalo.DB.DbCommon;
using Buffalo.DB.QueryConditions;
using System.Data;
using Buffalo.DB.PropertyAttributes;
using Buffalo.Kernel;

namespace Buffalo.DB.BQLCommon.BQLKeyWordCommon
{
    /// <summary>
    /// 建表的字段集合
    /// </summary>
    public class KeyWordAddParamItem : BQLQuery
    {
        protected EntityParam _param;
        /// <summary>
        /// 字段
        /// </summary>
        public EntityParam Param
        {
            get { return _param; }
            set { _param = value; }
        }
        protected string _tableName;
        /// <summary>
        /// 表名
        /// </summary>
        public string TableName
        {
            get { return _tableName; }
        }
        /// <summary>
        /// Insert的字段关键字项
        /// </summary>
        /// <param name="paramHandles">字段集合</param>
        /// <param name="previous">上一个关键字</param>
        public KeyWordAddParamItem(string tableName, BQLQuery previous)
            : base(previous) 
        {
            _tableName = tableName;
        }
        /// <summary>
        /// Insert的字段关键字项
        /// </summary>
        /// <param name="paramHandles">字段集合</param>
        /// <param name="previous">上一个关键字</param>
        public KeyWordAddParamItem(EntityParam param, string tableName, BQLQuery previous)
            : base(previous)
        {
            _tableName = tableName;
            _param = param;
        }




        internal override void LoadInfo(KeyWordInfomation info)
        {

        }

        internal override void Tran(KeyWordInfomation info)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(_param.DisplayInfo(info, TableName));
            AlterTableCondition con = info.Condition as AlterTableCondition;
            if (con != null)
            {
                con.Operator.Append(info.DBInfo.DBStructure.GetAddParamSQL());
            }
            info.Condition.SqlParams.Append(sb.ToString());
        }
    }

    
}
