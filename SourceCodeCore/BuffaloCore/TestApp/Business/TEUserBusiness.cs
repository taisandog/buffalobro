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
    ///  �û�ҵ���
    /// </summary>
    public class TEUserBusinessBase<T>: TestApp.Business.TEBaseBusinessBase<T> where T:TEUser,new()
    {
        //�����ʵ����Ҫ���̳����ڴ�д��ҵ�񷽷����������ҵ������ʹ��
    }
    
    /// <summary>
    ///  �û�ҵ���
    /// </summary>
    public class TEUserBusiness: TEUserBusinessBase<TEUser>
    {
        public TEUserBusiness()
        {
            
        }
        
    }
}



