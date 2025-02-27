using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DB.QueryConditions;
using System.Data;
using Buffalo.DB.BQLCommon.BQLKeyWordCommon;
using Buffalo.DB.BQLCommon.BQLExtendFunction;
using Buffalo.DB.BQLCommon.BQLConditionCommon;
using System.Collections;
using Buffalo.Kernel.Defaults;
using Buffalo.DB.EntityInfos;
using Buffalo.Kernel;

namespace Buffalo.DB.BQLCommon.BQLConditions
{
    public class BQLConditionScope
    {

        /// <summary>
        /// 填充查询条件并返回条件的SQL语句( and 开头)
        /// </summary>
        /// <param name="lstParam">参数列表</param>
        /// <param name="lstScope">范围查询集合</param>
        /// <returns></returns>
        internal static BQLCondition FillCondition(BQLCondition condition, BQLTableHandle table, ScopeBaseList lstScope, EntityInfoHandle entityInfo)
        {
            BQLCondition ret = condition;
            if (lstScope == null)
            {
                
                return ret;
            }
            BQLCondition curHandle = null;
            for (int i = 0; i < lstScope.Count; i++)
            {
                Scope objScope = lstScope[i];
                EntityPropertyInfo info = null;
                if (entityInfo != null)
                {
                    if (objScope.ScopeType == ScopeType.Condition)
                    {
                        curHandle = objScope.Value1 as BQLCondition;
                    }
                    else
                    {
                        //info = entityInfo.PropertyInfo[objScope.PropertyName];
                        curHandle = FormatScorp(objScope, DbType.Object, objScope.PropertyName, ret, table, entityInfo);
                    }
                }
                else
                {
                    curHandle = FormatScorp(objScope, DbType.Object, objScope.PropertyName, ret, table, entityInfo);
                }

                if (!Buffalo.Kernel.CommonMethods.IsNull(curHandle))
                {
                    if (CommonMethods.IsNull(ret))
                    {
                        ret = curHandle;
                    }
                    else if (objScope.ConnectType == ConnectType.And)
                    {
                        ret = ret & curHandle;
                    }
                    else
                    {
                        ret = ret | curHandle;
                    }
                }
            }
            return ret;
        }

        /// <summary>
        /// 获取排序列表
        /// </summary>
        /// <param name="lstScort"></param>
        /// <param name="table"></param>
        /// <param name="entityType"></param>
        /// <returns></returns>
        internal static BQLParamHandle[] GetSort(SortList lstScort, BQLTableHandle table, EntityInfoHandle entityInfo) 
        {
            List<BQLParamHandle> lstHandles = new List<BQLParamHandle>();
            if (lstScort == null)
            {
                lstHandles.ToArray();
            }
            
            
            for (int i = 0; i < lstScort.Count; i++)
            {
                Sort objSort = lstScort[i];
                
                BQLParamHandle handle = null;
                if (!CommonMethods.IsNull(objSort.OrderHandle)) 
                {
                    handle = objSort.OrderHandle;
                }
                else if (entityInfo != null)
                {
                    //EntityPropertyInfo info = entityInfo.PropertyInfo[objSort.PropertyName];
                    if (objSort.SortType == SortType.ASC)
                    {
                        handle = table[objSort.PropertyName].ASC;
                    }
                    else 
                    {
                        handle = table[objSort.PropertyName].DESC;
                    }
                }
                else
                {
                    if (objSort.SortType == SortType.ASC)
                    {
                        handle = table[objSort.PropertyName].ASC;
                    }
                    else
                    {
                        handle = table[objSort.PropertyName].DESC;
                    }
                }
                lstHandles.Add(handle);
            }
            return lstHandles.ToArray();
        }

        private static BQLCondition FormatScorp(Scope scope, DbType dbType, string pro, BQLCondition handle, BQLTableHandle table, EntityInfoHandle entityInfo)
        {
            ScopeType ctype = scope.ScopeType;
            BQLScopeExplainer delFun=BQLExplainScope.GetExplainer(scope);
            if (delFun != null) 
            {
                //if (dbType == DbType.Object && scope.Value1 != null) 
                //{
                //    dbType = DefaultType.ToDbType(scope.Value1.GetType());
                //}
                handle = delFun(scope, dbType, pro, handle, table, entityInfo);
            }
            
            return handle;
        }

    }
}
