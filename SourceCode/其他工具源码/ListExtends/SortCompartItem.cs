using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.Kernel.FastReflection;
using Buffalo.DB.QueryConditions;
namespace Buffalo.DB.ListExtends
{
    class SortCompartItem
    {
        private PropertyInfoHandle getValueHandler;
        private SortType curSortType;

        /// <summary>
        /// 此排序的取值句柄
        /// </summary>
        public PropertyInfoHandle GetValueHandler 
        {
            get 
            {
                return getValueHandler;
            }
            set 
            {
                getValueHandler = value;
            }
        }

        /// <summary>
        /// 排序方式
        /// </summary>
        public SortType CurSortType 
        {
            get
            {
                return curSortType;
            }
            set 
            {
                curSortType = value;
            }
        }
    }
}
