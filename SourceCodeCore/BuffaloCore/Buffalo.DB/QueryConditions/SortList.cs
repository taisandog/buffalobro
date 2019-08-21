using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DB.BQLCommon.BQLConditionCommon;
using Buffalo.Kernel;

namespace Buffalo.DB.QueryConditions
{
    /// <summary>
    /// ���������б�.
    /// </summary>
    public class SortList : List<Sort>
    {
        ScopeList _lstScope;
        public SortList(ScopeList lstScope) :base()
        {
            _lstScope = lstScope;
        }

        public SortList()
            :this(null)
        {
        }

        /// <summary>
        /// ����µ�����ʽ
        /// </summary>
        /// <param name="propertyName">������</param>
        /// <param name="type">��������</param>
        public void Add(string propertyName,SortType type)
        {
            Sort objSort = new Sort();
            objSort.PropertyName = propertyName;
            objSort.SortType = type;
            base.Add(objSort);
            
        }

        public void Add(BQLOrderByHandle orderBy) 
        {
            //BQLEntityParamHandle handle=orderBy.Param as BQLEntityParamHandle;
            //if (CommonMethods.IsNull(handle)) 
            //{
            //    throw new Exception("���ñ���ִ��ֶν�������");
            //}
            //Add(handle.PInfo.PropertyName, orderBy.SortType);
            Sort objSort = new Sort();
            objSort.OrderHandle = orderBy;
            if (_lstScope != null) 
            {
                _lstScope.HasInner = true;
            }
            base.Add(objSort);
        }
        /// <summary>
        /// ����µķ�Χ
        /// </summary>
        /// <param name="property">����</param>
        /// <param name="type">��������</param>
        /// <returns>�����Ƿ���ӳɹ�</returns>
        public void Add(BQLEntityParamHandle property, SortType type)
        {
            if (type == SortType.DESC)
            {
                Add(property.DESC);
            }
            else 
            {
                Add(property.ASC);
            }
        }
        ///// <summary>
        ///// ����µ�����ʽ
        ///// </summary>
        ///// <param name="sort">��������</param>
        //public new void Add(Sort sort)
        //{
        //    if (this[sort.PropertyName] != null)
        //    {
        //        throw new Exception("���ظ������ֶΣ�");
        //    }

        //    base.Add(sort);

        //}
        /// <summary>
        /// ������������ȡ����ʽ
        /// </summary>
        /// <param name="propertyName">������</param>
        /// <returns></returns>
        public Sort this[string propertyName] 
        {
            get 
            {
                foreach (Sort objSort in this) 
                {
                    if (objSort.PropertyName == propertyName) 
                    {
                        return objSort;
                    }
                }
                return null;
            }
        }


    }
}
