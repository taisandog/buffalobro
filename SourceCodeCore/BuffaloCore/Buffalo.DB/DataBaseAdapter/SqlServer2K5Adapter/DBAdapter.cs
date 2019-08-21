using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DB.QueryConditions;
using Buffalo.DB.DbCommon;
using System.Data.SqlClient;

namespace Buffalo.DB.DataBaseAdapter.SqlServer2K5Adapter
{
    public class DBAdapter : SqlServer2KAdapter.DBAdapter
    {
        public override string CreatePageSql(ParamList list, DataBaseOperate oper, 
            SelectCondition objCondition, PageContent objPage,bool useCache)
        {
            return CutPageSqlCreater.CreatePageSql(list, oper, objCondition, objPage, useCache);
        }
        /// <summary>
        /// ȫ������ʱ�������ֶ��Ƿ���ʾ���ʽ
        /// </summary>
        public override bool IsShowExpression
        {
            get
            {
                return true;
            }
        }
        ///// <summary>
        ///// ��ȡ�����б�
        ///// </summary>
        //public override ParamList BQLSelectParamList
        //{
        //    get
        //    {
        //        return new ParamList();
        //    }
        //}
        
    }
}
