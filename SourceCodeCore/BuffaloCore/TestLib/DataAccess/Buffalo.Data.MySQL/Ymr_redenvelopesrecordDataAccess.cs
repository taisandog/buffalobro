using System;
using System.Data;
using Buffalo.DB.CommBase.DataAccessBases;
using Buffalo.DB.DbCommon;
using TestLib;
using System.Collections.Generic;
using TestLib.DataAccess.IDataAccess;

namespace TestLib.DataAccess.Buffalo.Data.MySQL
{
    ///<summary>
    /// Êý¾Ý·ÃÎÊ²ã
    ///</summary>
    public class Ymr_redenvelopesrecordDataAccess : DataAccessModel<Ymr_redenvelopesrecord>,IYmr_redenvelopesrecordDataAccess
    {
        public Ymr_redenvelopesrecordDataAccess(DataBaseOperate oper): base(oper)
        {
            
        }
        public Ymr_redenvelopesrecordDataAccess(): base()
        {
        }
    }
}



