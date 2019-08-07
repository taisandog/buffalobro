using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using Buffalo.Kernel.FastReflection;
using Buffalo.DB.QueryConditions;
using Buffalo.Kernel.Defaults;
namespace Buffalo.DB.ListExtends
{
    /// <summary>
    /// 提供对IList数据过滤和排序的类
    /// </summary>
    public class DataFilter
    {
        #region 过滤
        private static readonly Type compareType = typeof(IComparable);//可比较对象的接口类型
        
        /// <summary>
        /// 过滤集合
        /// </summary>
        /// <param name="sourceList">源集合</param>
        /// <param name="lstScope">过滤条件</param>
        /// <returns></returns>
        public static IList RowFilter(IList sourceList, ScopeList lstScope) 
        {
            if(sourceList.Count<=0)
            {
                return sourceList;
            }
            IList retLst = (IList)Activator.CreateInstance(sourceList.GetType());
            Type objType=sourceList[0].GetType();
            //初始化属性信息列表
            List<CompareItemInfo> infos = new List<CompareItemInfo>();
            foreach (Scope objScope in lstScope)
            {
                PropertyInfoHandle pInfo = FastValueGetSet.GetPropertyInfoHandle(objScope.PropertyName, objType);
                infos.Add(new CompareItemInfo(pInfo,objScope));
            }
            foreach (object obj in sourceList) 
            {
                List<CompareItem> lstCompareItem = new List<CompareItem>();
                //Dictionary<string, Scope>.Enumerator enums = lstScope.GetEnumerator();
                foreach (CompareItemInfo itemInfo in infos) 
                {
                    bool resault = IsAccord(itemInfo.PropertyInfo.GetValue(obj), itemInfo.ScopeInfo);
                    lstCompareItem.Add(new CompareItem(resault, itemInfo.ScopeInfo.ConnectType));
                }
                if(IsAllAccord(lstCompareItem))
                {
                    retLst.Add(obj);
                }
            }
            return retLst;
        }

        /// <summary>
        /// 是否通过所有条件
        /// </summary>
        /// <param name="lstCompareItem">比较结果集合</param>
        /// <returns></returns>
        private static bool IsAllAccord(List<CompareItem> lstCompareItem) 
        {
            bool ret = true;
            foreach (CompareItem item in lstCompareItem) 
            {
                if (item.ConnectType == ConnectType.And)
                {
                    ret = (ret && item.CurCompare);
                }
                else 
                {
                    ret = (ret || item.CurCompare);
                }
            }
            return ret;
        }

        /// <summary>
        /// 是否通过条件
        /// </summary>
        /// <param name="value"></param>
        /// <param name="condition"></param>
        /// <returns></returns>
        private static bool IsAccord(object value, Scope condition) 
        {
            if (condition.ScopeType == ScopeType.Between) 
            {
                if (DefaultValue.Compare(value, condition.Value1) >= 0 && DefaultValue.Compare(value, condition.Value2) <= 0) 
                {
                    return true;
                }
            }

            else if (condition.ScopeType == ScopeType.Equal) 
            {
                if (condition.Value1 == null) 
                {
                    return (value == null);
                }
                return (DefaultValue.Compare(value, condition.Value1) == 0);
            }

            else if (condition.ScopeType == ScopeType.IN)
            {
                Hashtable hs=condition.GetInCollection();
                return hs.ContainsKey(value);
            }
            else if (condition.ScopeType == ScopeType.Like)
            {
                if (condition.Value1 == null)
                {
                    return (value == null);
                }
                if (condition.Value1.ToString().IndexOf(value.ToString()) > 0) 
                {
                    return true;
                }
            }
            else if (condition.ScopeType == ScopeType.Less)
            {
                return (DefaultValue.Compare(value, condition.Value1) < 0);
            }
            else if (condition.ScopeType == ScopeType.LessThen)
            {
                return (DefaultValue.Compare(value, condition.Value1) <= 0);
            }
            else if (condition.ScopeType == ScopeType.More)
            {
                return (DefaultValue.Compare(value, condition.Value1) > 0);
            }
            else if (condition.ScopeType == ScopeType.MoreThen)
            {
                return (DefaultValue.Compare(value, condition.Value1) >= 0);
            }
            else if (condition.ScopeType == ScopeType.NotEqual)
            {
                if (condition.Value1 == null)
                {
                    return (value != null);
                }
                return (DefaultValue.Compare(value, condition.Value1) != 0);
            }
            return false;
        }
        
        #endregion

        #region 排序
        /// <summary>
        /// 对值集合进行排序
        /// </summary>
        /// <param name="lst">值集合</param>
        /// <param name="lstSort">排序方式</param>
        public static void SortList(IList lst, SortType objSort)
        {
           
            if (lst == null)
            {
                return;
            }
            if (lst.Count <= 0)
            {
                return;
            }
            
            //用冒泡法对集合进行排序
            for (int i = 0; i < lst.Count - 1; i++)
            {
                for (int k = i + 1; k < lst.Count; k++)
                {
                    object val1 = lst[i];
                    object val2 = lst[i];

                    if (!IsAccord(lst[i], lst[k], objSort))//如果对比不通过，就互换值
                    {
                        object tmp = lst[i];
                        lst[i] = lst[k];
                        lst[k] = tmp;
                    }
                }
            }
        }

        /// <summary>
        /// 比较两个值是否合乎排序条件
        /// </summary>
        /// <param name="val1">值1</param>
        /// <param name="val2">值2</param>
        /// <param name="objType">排序条件</param>
        /// <returns></returns>
        private static bool IsAccord(object val1, object val2, SortType objType) 
        {
            int ret = DefaultValue.Compare(val1, val2);
            if (objType == SortType.ASC && ret > 0)
            {
                return false;
            }
            else if (objType == SortType.DESC && ret < 0)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 对实体集合进行排序
        /// </summary>
        /// <param name="lst">实体集合</param>
        /// <param name="lstSort">排序方式</param>
        public static void SortList(IList lst,SortList lstSort) 
        {
            List<SortCompartItem> lstCompareItem = null;
            if (lst==null) 
            {
                return;
            }
            if (lst.Count <= 0) 
            {
                return;
            }
            lstCompareItem = GetSortComparts(lst[0], lstSort);
            if (lstCompareItem.Count > 0 && lst.Count>=2) 
            {
                //用冒泡法对集合进行排序
                for (int i = 0; i < lst.Count-1; i++) 
                {
                    for (int k = i + 1; k < lst.Count; k++) 
                    {
                        if(!IsAccordCompare(lst[i],lst[k],lstCompareItem))//如果对比不通过，就互换值
                        {
                            object tmp = lst[i];
                            lst[i] = lst[k];
                            lst[k] = tmp;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 根据排序方式列表获取对象的Get属性句柄和对应排序方式
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="lstSort"></param>
        /// <returns></returns>
        private static List<SortCompartItem> GetSortComparts(object obj, SortList lstSort) 
        {
            Type type = obj.GetType();
            List<SortCompartItem> lstSCompare = new List<SortCompartItem>();
            foreach (Sort objSort in lstSort) 
            {
                PropertyInfoHandle getHandle = FastValueGetSet.GetPropertyInfoHandle(objSort.PropertyName, type);
                SortCompartItem item = new SortCompartItem();
                item.CurSortType = objSort.SortType;
                item.GetValueHandler = getHandle;
                lstSCompare.Add(item);
            }
            return lstSCompare;
        }

        

        /// <summary>
        /// 根据排序条件对比两个对象(是否符合所有排序条件)
        /// </summary>
        /// <param name="val1">对象1</param>
        /// <param name="val2">对象2</param>
        /// <param name="lstSort">比较条件列表</param>
        /// <returns></returns>
        private static bool IsAccordCompare(object obj1, object obj2, List<SortCompartItem> lstCompers)
        {
            foreach (SortCompartItem item in lstCompers) 
            {
                object val1 = item.GetValueHandler.GetValue(obj1);
                object val2 = item.GetValueHandler.GetValue(obj2);
                int ret = DefaultValue.Compare(val1, val2);
                if (item.CurSortType == SortType.ASC)
                {
                    if (ret > 0)//如果不符合条件直接返回flase
                    {
                        return false;
                    }
                    else if (ret < 0) //如果不符合条件直接返回true
                    {
                        return true;
                    }
                    //如果两个值相等才会循环判断下一个条件
                }
                else 
                {
                    if (ret < 0)//如果不符合条件直接返回flase
                    {
                        return false;
                    }
                    else if (ret > 0)//如果不符合条件直接返回true
                    {
                        return true;
                    }
                    //如果两个值相等才会循环判断下一个条件
                }
            }
            return true;
        }
        #endregion
    }
}
