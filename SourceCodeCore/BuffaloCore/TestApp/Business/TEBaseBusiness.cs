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
    public class TEBaseBusinessBase<T>: BusinessModelBase<T> where T:TEBase,new()
    {
        //�����ʵ����Ҫ���̳����ڴ�д��ҵ�񷽷����������ҵ������ʹ��

        public override object Insert(T entity, ValueSetList setList, bool fillIdentity)
        {
            if(entity!=null && !entity.GetEntityBaseInfo().HasPropertyChange("CreateDate"))
            {
                entity.CreateDate = DateTime.Now;
            }
            return base.Insert(entity, setList, fillIdentity);
        }
    }
}



