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
    ///  ҵ���
    /// </summary>
    public class YqsrobotconfigBusinessBase<T>: TestLib.Business.YMRBaseBusinessBase<T> where T:Yqsrobotconfig,new()
    {
        //�����ʵ����Ҫ���̳����ڴ�д��ҵ�񷽷����������ҵ������ʹ��
    }
    
    /// <summary>
    ///  ҵ���
    /// </summary>
    public class YqsrobotconfigBusiness: YqsrobotconfigBusinessBase<Yqsrobotconfig>
    {
        public YqsrobotconfigBusiness()
        {
            
        }
        
    }
}



