using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;


namespace Buffalo.MongoDB.QueryCondition
{
    /// <summary>
    /// ���������б�.
    /// </summary>
    public class MGSortList : List<MGSort>
    {
        

        public MGSortList()
        {
            
        }

        /// <summary>
        /// ����µ�����ʽ
        /// </summary>
        /// <param name="propertyName">������</param>
        /// <param name="type">��������</param>
        public void Add(string propertyName, MGSortType type)
        {
            MGSort objSort = new MGSort();
            objSort.PropertyName = propertyName;
            objSort.SortType = type;
            base.Add(objSort);
            
        }

      
        /// <summary>
        /// ������������ȡ����ʽ
        /// </summary>
        /// <param name="propertyName">������</param>
        /// <returns></returns>
        public MGSort this[string propertyName] 
        {
            get 
            {
                foreach (MGSort objSort in this) 
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
