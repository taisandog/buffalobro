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
    ///  ҵ���
    /// </summary>
    public class TEAdminBusinessBase<T>: TestApp.Business.TEBaseBusinessBase<T> where T:TEAdmin,new()
    {
        //�����ʵ����Ҫ���̳����ڴ�д��ҵ�񷽷����������ҵ������ʹ��
    }
    
    /// <summary>
    ///  ҵ���
    /// </summary>
    public class TEAdminBusiness: TEAdminBusinessBase<TEAdmin>
    {
        public TEAdminBusiness()
        {
            
        }
        
    }
}



