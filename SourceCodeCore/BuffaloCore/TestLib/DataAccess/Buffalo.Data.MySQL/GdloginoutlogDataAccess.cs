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
    /// ���ݷ��ʲ�
    ///</summary>
    public class GdloginoutlogDataAccess : DataAccessModel<Gdloginoutlog>,IGdloginoutlogDataAccess
    {
        public GdloginoutlogDataAccess(DataBaseOperate oper): base(oper)
        {
            
        }
        public GdloginoutlogDataAccess(): base()
        {
        }
    }
}



