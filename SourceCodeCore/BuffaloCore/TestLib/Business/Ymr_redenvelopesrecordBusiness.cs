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
    public class Ymr_redenvelopesrecordBusinessBase<T>: TestLib.Business.YMRBaseBusinessBase<T> where T:Ymr_redenvelopesrecord,new()
    {
        //�����ʵ����Ҫ���̳����ڴ�д��ҵ�񷽷����������ҵ������ʹ��
    }
    
    /// <summary>
    ///  ҵ���
    /// </summary>
    public class Ymr_redenvelopesrecordBusiness: Ymr_redenvelopesrecordBusinessBase<Ymr_redenvelopesrecord>
    {
        public Ymr_redenvelopesrecordBusiness()
        {
            
        }
        
    }
}



