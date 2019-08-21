using System;
using TestApp;
using System.Collections.Generic;
using Buffalo.DB.DataBaseAdapter;
using Buffalo.DB.CommBase.BusinessBases;
using Buffalo.DB.CommBase.DataAccessBases;
using Buffalo.DB.DbCommon;
using Buffalo.DB.QueryConditions;
using Buffalo.DB.CommBase;
namespace TestApp.Business
{
    /// <summary>
    ///  用户业务层
    /// </summary>
    public class TEUserBusinessBase<T>: TestApp.Business.TEBaseBusinessBase<T> where T:TEUser,new()
    {
        //如果此实体需要被继承则在此写的业务方法能在子类的业务类中使用
    }
    
    /// <summary>
    ///  用户业务层
    /// </summary>
    public class TEUserBusiness: TEUserBusinessBase<TEUser>
    {
        public TEUserBusiness()
        {
            
        }
        
    }
}



