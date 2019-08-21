using System;
using System.Data;
using System.Configuration;
using Buffalo.DB.BQLCommon;
using Buffalo.DB.BQLCommon.BQLKeyWordCommon;
using Buffalo.DB.BQLCommon.BQLConditionCommon;
using System.Collections.Generic;
using Buffalo.DB.QueryConditions;
using TestApp.BQLEntity;
using TestApp;
using Buffalo.DB.DbCommon;
namespace TestApp.DataAccess.Bql
{
    ///<summary>
    /// 用户数据访问层
    ///</summary>
    public class TEUserDataAccess :BQLDataAccessBase<TEUser>
    {
        public TEUserDataAccess(DataBaseOperate oper)
            : base(oper)
        {
            
        }
        public TEUserDataAccess()
        {
            
        }
    }
}



