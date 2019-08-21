using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DB.BQLCommon.BQLConditionCommon;
using Buffalo.DB.QueryConditions;
using System.Data;
using Buffalo.DB.EntityInfos;
using System.Collections;
using Buffalo.Kernel;

namespace Buffalo.DB.BQLCommon.BQLConditions
{
    internal delegate BQLCondition BQLScopeExplainer(Scope scope, DbType dbType, string paramName, BQLCondition handle, BQLTableHandle table, EntityInfoHandle entityInfo);

    internal class BQLExplainScope
    {

        private static BQLScopeExplainer[] arrExplainer = InitExplainer();

        /// <summary>
        /// ��ʼ������
        /// </summary>
        /// <returns></returns>
        private static BQLScopeExplainer[] InitExplainer()
        {
            arrExplainer = new BQLScopeExplainer[15];
            arrExplainer[(int)ScopeType.Between] = Between;
            arrExplainer[(int)ScopeType.Condition] = Condition;
            arrExplainer[(int)ScopeType.Contains] = Contains;
            arrExplainer[(int)ScopeType.EndWith] = EndWith;
            arrExplainer[(int)ScopeType.Equal] = Equal;
            arrExplainer[(int)ScopeType.IN] = IN;
            arrExplainer[(int)ScopeType.Less] = Less;
            arrExplainer[(int)ScopeType.LessThen] = LessThen;
            arrExplainer[(int)ScopeType.Like] = Like;
            arrExplainer[(int)ScopeType.More] = More;
            arrExplainer[(int)ScopeType.MoreThen] = MoreThen;
            arrExplainer[(int)ScopeType.NotEqual] = NotEqual;
            arrExplainer[(int)ScopeType.NotIn] = NotIn;
            arrExplainer[(int)ScopeType.Scope] = DoScope;
            arrExplainer[(int)ScopeType.StarWith] = StarWith;

            return arrExplainer;
        }

        /// <summary>
        /// ����������ȡ������
        /// </summary>
        /// <param name="objScope"></param>
        /// <returns></returns>
        internal static BQLScopeExplainer GetExplainer(Scope objScope)
        {
            int index = (int)objScope.ScopeType;
            if (index >= 0 && index < arrExplainer.Length)
            {
                return arrExplainer[index];
            }
            return null;
        }

        /// <summary>
        /// ����Between
        /// </summary>
        /// <param name="scope"></param>
        /// <param name="dbType"></param>
        /// <param name="paramName"></param>
        /// <param name="handle"></param>
        /// <param name="table"></param>
        /// <param name="entityInfo"></param>
        /// <returns></returns>
        internal static BQLCondition Between(Scope scope, DbType dbType, string paramName, BQLCondition handle, BQLTableHandle table, EntityInfoHandle entityInfo) 
        {
            BQLValueTypeItem cvalue1 = new BQLValueTypeItem(scope.Value1);
            BQLValueTypeItem cvalue2 = null;
            if (scope.Value2 != null)
            {
                cvalue2 = new BQLValueTypeItem(scope.Value2);
            }

            return table[paramName, dbType].Between(cvalue1, cvalue2);
        }

        /// <summary>
        /// ����IN
        /// </summary>
        /// <param name="scope"></param>
        /// <param name="dbType"></param>
        /// <param name="paramName"></param>
        /// <param name="handle"></param>
        /// <param name="table"></param>
        /// <param name="entityInfo"></param>
        /// <returns></returns>
        internal static BQLCondition IN(Scope scope, DbType dbType, string paramName, BQLCondition handle, BQLTableHandle table, EntityInfoHandle entityInfo)
        {
            BQLValueTypeItem cvalue1 = new BQLValueTypeItem(scope.Value1);

            return table[paramName, dbType].In(scope.Value1 as IEnumerable);
        }

        /// <summary>
        /// ����NotIn
        /// </summary>
        /// <param name="scope"></param>
        /// <param name="dbType"></param>
        /// <param name="paramName"></param>
        /// <param name="handle"></param>
        /// <param name="table"></param>
        /// <param name="entityInfo"></param>
        /// <returns></returns>
        internal static BQLCondition NotIn(Scope scope, DbType dbType, string paramName, BQLCondition handle, BQLTableHandle table, EntityInfoHandle entityInfo)
        {
            BQLValueTypeItem cvalue1 = new BQLValueTypeItem(scope.Value1);

            return table[paramName, dbType].NotIn(scope.Value1 as IEnumerable);
        }

        /// <summary>
        /// ����Less
        /// </summary>
        /// <param name="scope"></param>
        /// <param name="dbType"></param>
        /// <param name="paramName"></param>
        /// <param name="handle"></param>
        /// <param name="table"></param>
        /// <param name="entityInfo"></param>
        /// <returns></returns>
        internal static BQLCondition Less(Scope scope, DbType dbType, string paramName, BQLCondition handle, BQLTableHandle table, EntityInfoHandle entityInfo)
        {
            BQLValueTypeItem cvalue1 = new BQLValueTypeItem(scope.Value1);
            return table[paramName, dbType] < cvalue1;
        }


        /// <summary>
        /// ����LessThen
        /// </summary>
        /// <param name="scope"></param>
        /// <param name="dbType"></param>
        /// <param name="paramName"></param>
        /// <param name="handle"></param>
        /// <param name="table"></param>
        /// <param name="entityInfo"></param>
        /// <returns></returns>
        internal static BQLCondition LessThen(Scope scope, DbType dbType, string paramName, BQLCondition handle, BQLTableHandle table, EntityInfoHandle entityInfo)
        {
            BQLValueTypeItem cvalue1 = new BQLValueTypeItem(scope.Value1);


            return table[paramName, dbType] <= cvalue1;
        }

        /// <summary>
        /// ����More
        /// </summary>
        /// <param name="scope"></param>
        /// <param name="dbType"></param>
        /// <param name="paramName"></param>
        /// <param name="handle"></param>
        /// <param name="table"></param>
        /// <param name="entityInfo"></param>
        /// <returns></returns>
        internal static BQLCondition More(Scope scope, DbType dbType, string paramName, BQLCondition handle, BQLTableHandle table, EntityInfoHandle entityInfo)
        {
            BQLValueTypeItem cvalue1 = new BQLValueTypeItem(scope.Value1);
            return table[paramName, dbType] > cvalue1;
        }

        /// <summary>
        /// ����MoreThen
        /// </summary>
        /// <param name="scope"></param>
        /// <param name="dbType"></param>
        /// <param name="paramName"></param>
        /// <param name="handle"></param>
        /// <param name="table"></param>
        /// <param name="entityInfo"></param>
        /// <returns></returns>
        internal static BQLCondition MoreThen(Scope scope, DbType dbType, string paramName, BQLCondition handle, BQLTableHandle table, EntityInfoHandle entityInfo)
        {
            BQLValueTypeItem cvalue1 = new BQLValueTypeItem(scope.Value1);
            return table[paramName, dbType] >= cvalue1;
        }

        /// <summary>
        /// ����NotEqual
        /// </summary>
        /// <param name="scope"></param>
        /// <param name="dbType"></param>
        /// <param name="paramName"></param>
        /// <param name="handle"></param>
        /// <param name="table"></param>
        /// <param name="entityInfo"></param>
        /// <returns></returns>
        internal static BQLCondition NotEqual(Scope scope, DbType dbType, string paramName, BQLCondition handle, BQLTableHandle table, EntityInfoHandle entityInfo)
        {
            BQLValueTypeItem cvalue1 = new BQLValueTypeItem(scope.Value1);
            return table[paramName, dbType] != cvalue1;
        }

        /// <summary>
        /// ����Equal
        /// </summary>
        /// <param name="scope"></param>
        /// <param name="dbType"></param>
        /// <param name="paramName"></param>
        /// <param name="handle"></param>
        /// <param name="table"></param>
        /// <param name="entityInfo"></param>
        /// <returns></returns>
        internal static BQLCondition Equal(Scope scope, DbType dbType, string paramName, BQLCondition handle, BQLTableHandle table, EntityInfoHandle entityInfo)
        {
            BQLValueTypeItem cvalue1 = new BQLValueTypeItem(scope.Value1);
            return table[paramName, dbType] == cvalue1;
        }

        /// <summary>
        /// ����Like
        /// </summary>
        /// <param name="scope"></param>
        /// <param name="dbType"></param>
        /// <param name="paramName"></param>
        /// <param name="handle"></param>
        /// <param name="table"></param>
        /// <param name="entityInfo"></param>
        /// <returns></returns>
        internal static BQLCondition Like(Scope scope, DbType dbType, string paramName, BQLCondition handle, BQLTableHandle table, EntityInfoHandle entityInfo)
        {
            BQLValueTypeItem cvalue1 = new BQLValueTypeItem(scope.Value1);
            return table[paramName, dbType].Like(cvalue1);
        }

        /// <summary>
        /// ����Contains
        /// </summary>
        /// <param name="scope"></param>
        /// <param name="dbType"></param>
        /// <param name="paramName"></param>
        /// <param name="handle"></param>
        /// <param name="table"></param>
        /// <param name="entityInfo"></param>
        /// <returns></returns>
        internal static BQLCondition Contains(Scope scope, DbType dbType, string paramName, BQLCondition handle, BQLTableHandle table, EntityInfoHandle entityInfo)
        {
            BQLValueTypeItem cvalue1 = new BQLValueTypeItem(scope.Value1);
            return table[paramName, dbType].Contains(cvalue1);
        }

        /// <summary>
        /// ����StarWith
        /// </summary>
        /// <param name="scope"></param>
        /// <param name="dbType"></param>
        /// <param name="paramName"></param>
        /// <param name="handle"></param>
        /// <param name="table"></param>
        /// <param name="entityInfo"></param>
        /// <returns></returns>
        internal static BQLCondition StarWith(Scope scope, DbType dbType, string paramName, BQLCondition handle, BQLTableHandle table, EntityInfoHandle entityInfo)
        {
            BQLValueTypeItem cvalue1 = new BQLValueTypeItem(scope.Value1);
            return table[paramName, dbType].StarWith(cvalue1);
        }

        /// <summary>
        /// ����EndWith
        /// </summary>
        /// <param name="scope"></param>
        /// <param name="dbType"></param>
        /// <param name="paramName"></param>
        /// <param name="handle"></param>
        /// <param name="table"></param>
        /// <param name="entityInfo"></param>
        /// <returns></returns>
        internal static BQLCondition EndWith(Scope scope, DbType dbType, string paramName, BQLCondition handle, BQLTableHandle table, EntityInfoHandle entityInfo)
        {
            BQLValueTypeItem cvalue1 = new BQLValueTypeItem(scope.Value1);
            return table[paramName, dbType].EndWith(cvalue1);
        }

        /// <summary>
        /// ����Condition
        /// </summary>
        /// <param name="scope"></param>
        /// <param name="dbType"></param>
        /// <param name="paramName"></param>
        /// <param name="handle"></param>
        /// <param name="table"></param>
        /// <param name="entityInfo"></param>
        /// <returns></returns>
        internal static BQLCondition Condition(Scope scope, DbType dbType, string paramName, BQLCondition handle, BQLTableHandle table, EntityInfoHandle entityInfo)
        {
            BQLCondition fhandle = scope.Value1 as BQLCondition;
            return fhandle;
        }

        /// <summary>
        /// ����Scope
        /// </summary>
        /// <param name="scope"></param>
        /// <param name="dbType"></param>
        /// <param name="paramName"></param>
        /// <param name="handle"></param>
        /// <param name="table"></param>
        /// <param name="entityInfo"></param>
        /// <returns></returns>
        internal static BQLCondition DoScope(Scope scope, DbType dbType, string paramName, BQLCondition handle, BQLTableHandle table, EntityInfoHandle entityInfo)
        {
            ScopeList lstInnerScope = scope.Value1 as ScopeList;
            handle = BQLConditionScope.FillCondition(handle, table, lstInnerScope, entityInfo);
            return handle;
        }

    }
}
