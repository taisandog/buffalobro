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
    /// �ṩ��IList���ݹ��˺��������
    /// </summary>
    public class DataFilter
    {
        #region ����
        private static readonly Type compareType = typeof(IComparable);//�ɱȽ϶���Ľӿ�����
        
        /// <summary>
        /// ���˼���
        /// </summary>
        /// <param name="sourceList">Դ����</param>
        /// <param name="lstScope">��������</param>
        /// <returns></returns>
        public static IList RowFilter(IList sourceList, ScopeList lstScope) 
        {
            if(sourceList.Count<=0)
            {
                return sourceList;
            }
            IList retLst = (IList)Activator.CreateInstance(sourceList.GetType());
            Type objType=sourceList[0].GetType();
            //��ʼ��������Ϣ�б�
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
        /// �Ƿ�ͨ����������
        /// </summary>
        /// <param name="lstCompareItem">�ȽϽ������</param>
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
        /// �Ƿ�ͨ������
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

        #region ����
        /// <summary>
        /// ��ֵ���Ͻ�������
        /// </summary>
        /// <param name="lst">ֵ����</param>
        /// <param name="lstSort">����ʽ</param>
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
            
            //��ð�ݷ��Լ��Ͻ�������
            for (int i = 0; i < lst.Count - 1; i++)
            {
                for (int k = i + 1; k < lst.Count; k++)
                {
                    object val1 = lst[i];
                    object val2 = lst[i];

                    if (!IsAccord(lst[i], lst[k], objSort))//����ԱȲ�ͨ�����ͻ���ֵ
                    {
                        object tmp = lst[i];
                        lst[i] = lst[k];
                        lst[k] = tmp;
                    }
                }
            }
        }

        /// <summary>
        /// �Ƚ�����ֵ�Ƿ�Ϻ���������
        /// </summary>
        /// <param name="val1">ֵ1</param>
        /// <param name="val2">ֵ2</param>
        /// <param name="objType">��������</param>
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
        /// ��ʵ�弯�Ͻ�������
        /// </summary>
        /// <param name="lst">ʵ�弯��</param>
        /// <param name="lstSort">����ʽ</param>
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
                //��ð�ݷ��Լ��Ͻ�������
                for (int i = 0; i < lst.Count-1; i++) 
                {
                    for (int k = i + 1; k < lst.Count; k++) 
                    {
                        if(!IsAccordCompare(lst[i],lst[k],lstCompareItem))//����ԱȲ�ͨ�����ͻ���ֵ
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
        /// ��������ʽ�б��ȡ�����Get���Ծ���Ͷ�Ӧ����ʽ
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
        /// �������������Ա���������(�Ƿ����������������)
        /// </summary>
        /// <param name="val1">����1</param>
        /// <param name="val2">����2</param>
        /// <param name="lstSort">�Ƚ������б�</param>
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
                    if (ret > 0)//�������������ֱ�ӷ���flase
                    {
                        return false;
                    }
                    else if (ret < 0) //�������������ֱ�ӷ���true
                    {
                        return true;
                    }
                    //�������ֵ��ȲŻ�ѭ���ж���һ������
                }
                else 
                {
                    if (ret < 0)//�������������ֱ�ӷ���flase
                    {
                        return false;
                    }
                    else if (ret > 0)//�������������ֱ�ӷ���true
                    {
                        return true;
                    }
                    //�������ֵ��ȲŻ�ѭ���ж���һ������
                }
            }
            return true;
        }
        #endregion
    }
}
