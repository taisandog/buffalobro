using System;
using System.Data;
using Buffalo.DB.CommBase.DataAccessBases;
using Buffalo.DB.DbCommon;
using TestApp;
using System.Collections.Generic;
using TestApp.DataAccess.IDataAccess;

namespace TestApp.DataAccess.Sql2K12
{
    ///<summary>
    /// Êý¾Ý·ÃÎÊ²ã
    ///</summary>
    public class TERuleDataAccess : DataAccessModel<TERule>,ITERuleDataAccess
    {
        public TERuleDataAccess(DataBaseOperate oper): base(oper)
        {
            
        }
        public TERuleDataAccess(): base()
        {
        }
    }
}



