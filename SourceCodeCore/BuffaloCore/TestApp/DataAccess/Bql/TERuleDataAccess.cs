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
    /// Êý¾Ý·ÃÎÊ²ã
    ///</summary>
    public class TERuleDataAccess :BQLDataAccessBase<TERule>
    {
        public TERuleDataAccess(DataBaseOperate oper)
            : base(oper)
        {
            
        }
        public TERuleDataAccess()
        {
            
        }
    }
}



