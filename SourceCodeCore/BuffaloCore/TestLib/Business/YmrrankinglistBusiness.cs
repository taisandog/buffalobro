using System;
using TestLib;
using System.Collections.Generic;
using Buffalo.DB.DataBaseAdapter;
using Buffalo.DB.CommBase.BusinessBases;
using Buffalo.DB.CommBase.DataAccessBases;
using Buffalo.DB.DbCommon;
using Buffalo.DB.QueryConditions;
using Buffalo.DB.CommBase;
namespace TestLib.Business
{
    /// <summary>
    ///  业务层
    /// </summary>
    public class YmrrankinglistBusinessBase<T>: TestLib.Business.YMRBaseBusinessBase<T> where T:Ymrrankinglist,new()
    {
        //如果此实体需要被继承则在此写的业务方法能在子类的业务类中使用
    }
    
    /// <summary>
    ///  业务层
    /// </summary>
    public class YmrrankinglistBusiness: YmrrankinglistBusinessBase<Ymrrankinglist>
    {
        public YmrrankinglistBusiness()
        {
            
        }
        
    }
}



