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
        /// �������ȡֵ���
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
        /// ����ʽ
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
