using System;
using System.Data;
using System.Configuration;
using Buffalo.DB.BQLCommon;
using Buffalo.DB.BQLCommon.BQLKeyWordCommon;
using Buffalo.DB.BQLCommon.BQLConditionCommon;
using System.Collections.Generic;
using Buffalo.DB.QueryConditions;
using TestLib.BQLEntity;
using TestLib;
using Buffalo.DB.DbCommon;
namespace TestLib.DataAccess.Bql
{
    ///<summary>
    /// ���ݷ��ʲ�
    ///</summary>
    public class YmrrobotroomdataDataAccess :BQLDataAccessBase<Ymrrobotroomdata>
    {
        public YmrrobotroomdataDataAccess(DataBaseOperate oper)
            : base(oper)
        {
            
        }
        public YmrrobotroomdataDataAccess()
        {
            
        }
    }
}



