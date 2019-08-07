using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;


namespace Buffalo.MongoDB.QueryCondition
{
    /// <summary>
    /// 排序条件列表.
    /// </summary>
    public class MGSortList : List<MGSort>
    {
        

        public MGSortList()
        {
            
        }

        /// <summary>
        /// 添加新的排序方式
        /// </summary>
        /// <param name="propertyName">属性名</param>
        /// <param name="type">排序类型</param>
        public void Add(string propertyName, MGSortType type)
        {
            MGSort objSort = new MGSort();
            objSort.PropertyName = propertyName;
            objSort.SortType = type;
            base.Add(objSort);
            
        }

      
        /// <summary>
        /// 根据属性名获取排序方式
        /// </summary>
        /// <param name="propertyName">属性名</param>
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
