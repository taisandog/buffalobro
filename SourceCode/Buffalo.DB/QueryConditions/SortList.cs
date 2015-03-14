using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DB.BQLCommon.BQLConditionCommon;
using Buffalo.Kernel;

namespace Buffalo.DB.QueryConditions
{
    /// <summary>
    /// 排序条件列表.
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
        /// 添加新的排序方式
        /// </summary>
        /// <param name="propertyName">属性名</param>
        /// <param name="type">排序类型</param>
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
            //    throw new Exception("请用表的现存字段进行排序！");
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
        /// 添加新的范围
        /// </summary>
        /// <param name="property">属性</param>
        /// <param name="type">排序类型</param>
        /// <returns>返回是否添加成功</returns>
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
        ///// 添加新的排序方式
        ///// </summary>
        ///// <param name="sort">排序条件</param>
        //public new void Add(Sort sort)
        //{
        //    if (this[sort.PropertyName] != null)
        //    {
        //        throw new Exception("有重复排序字段！");
        //    }

        //    base.Add(sort);

        //}
        /// <summary>
        /// 根据属性名获取排序方式
        /// </summary>
        /// <param name="propertyName">属性名</param>
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
