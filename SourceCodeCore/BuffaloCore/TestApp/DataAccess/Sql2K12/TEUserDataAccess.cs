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
    /// �û����ݷ��ʲ�
    ///</summary>
    public class TEUserDataAccess : DataAccessModel<TEUser>,ITEUserDataAccess
    {
        public TEUserDataAccess(DataBaseOperate oper): base(oper)
        {
            
        }
        public TEUserDataAccess(): base()
        {
        }
    }
}



