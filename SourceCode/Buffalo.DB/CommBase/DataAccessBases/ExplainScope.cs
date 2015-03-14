using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DB.DbCommon;
using Buffalo.DB.DataBaseAdapter;
using System.Data;
using Buffalo.DB.EntityInfos;
using Buffalo.DB.QueryConditions;
using Buffalo.DB.BQLCommon.BQLConditionCommon;
using Buffalo.DB.FaintnessSearchConditions;
using System.Collections;
using Buffalo.Kernel;

namespace Buffalo.DB.CommBase.DataAccessBases
{
    internal delegate string ScopeExplainer(Scope scope, ParamList list, Type entityType, string paramName, DbType type, int lstIndex) ;
    /// <summary>
    /// 解释条件
    /// </summary>
    public class ExplainScope
    {

        private static ScopeExplainer[] arrExplainer = InitExplainer();

        /// <summary>
        /// 初始化条件
        /// </summary>
        /// <returns></returns>
        private static ScopeExplainer[] InitExplainer() 
        {
            arrExplainer = new ScopeExplainer[15];
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
            arrExplainer[(int)ScopeType.NotIn] = NotIN;
            //arrExplainer[(int)ScopeType.Scope] = DoScope;
            arrExplainer[(int)ScopeType.StarWith] = StarWith;
            return arrExplainer;
        }

        /// <summary>
        /// 根据条件获取处理函数
        /// </summary>
        /// <param name="objScope"></param>
        /// <returns></returns>
        internal static ScopeExplainer GetExplainer(Scope objScope) 
        {
            int index =(int) objScope.ScopeType;
            if (index >= 0 && index < arrExplainer.Length) 
            {
                return arrExplainer[index];
            }
            return null;
        }


        ///// <summary>
        ///// 解释Scope条件
        ///// </summary>
        ///// <param name="list"></param>
        ///// <param name="entityType"></param>
        ///// <param name="paramName"></param>
        ///// <param name="type"></param>
        ///// <param name="lstIndex"></param>
        //public static string DoScope(Scope scope, ParamList list, Type entityType, string paramName, DbType type, int lstIndex)
        //{
        //    StringBuilder ret = new StringBuilder(500);
        //    ScopeList lstInnerScope = scope.Value1 as ScopeList;
        //    if (lstInnerScope != null)
        //    {
        //        string strSql = FillCondition(lstParam, lstInnerScope, ref index);
        //        string connectString = DataAccessCommon.GetConnectString(objScope);
        //        ret.Append(" ");
        //        ret.Append(connectString);
        //        ret.Append(" (1=1 " + strSql + ")");
        //    }
        //    return ret.ToString();
        //}
        /// <summary>
        /// 解释Between条件
        /// </summary>
        /// <param name="list"></param>
        /// <param name="entityType"></param>
        /// <param name="paramName"></param>
        /// <param name="type"></param>
        /// <param name="lstIndex"></param>
        public static string Between(Scope scope, ParamList list, Type entityType, string paramName, DbType type, int lstIndex) 
        {
            DBInfo db=EntityInfoManager.GetEntityHandle(entityType).DBInfo;
            string paramVal = db.CurrentDbAdapter.FormatValueName(DataAccessCommon.FormatParam(paramName, lstIndex));
            string paramKey = db.CurrentDbAdapter.FormatParamKeyName(DataAccessCommon.FormatParam(paramName, lstIndex));
            StringBuilder sql = new StringBuilder(500);
            ScopeType ctype = scope.ScopeType;
            string connectString = DataAccessCommon.GetConnectString(scope);

            if (list != null)
            {
                sql.Append(" ");
                sql.Append(connectString);
                sql.Append(" (");
                sql.Append(db.CurrentDbAdapter.FormatParam(paramName));
                sql.Append(" between ");
                sql.Append(db.CurrentDbAdapter.FormatValueName(DataAccessCommon.FormatParam(paramName + "star", lstIndex)));
                sql.Append(" and ");
                sql.Append(db.CurrentDbAdapter.FormatValueName(DataAccessCommon.FormatParam(paramName + "end", lstIndex)));
                sql.Append(")");
                list.AddNew(db.CurrentDbAdapter.FormatParamKeyName(DataAccessCommon.FormatParam(paramName + "star", lstIndex)), type, scope.Value1);
                list.AddNew(db.CurrentDbAdapter.FormatParamKeyName(DataAccessCommon.FormatParam(paramName + "end", lstIndex)), type, scope.Value2);
            }
            else
            {
                sql.Append(" ");
                sql.Append(connectString);
                sql.Append(" (");
                sql.Append(db.CurrentDbAdapter.FormatParam(paramName));
                sql.Append(" between ");
                sql.Append(DataAccessCommon.FormatValue(scope.Value1, type, db));
                sql.Append(" and ");
                sql.Append(DataAccessCommon.FormatValue(scope.Value2, type, db));
                sql.Append(")");
            }
            return sql.ToString();
        }

        /// <summary>
        /// 解释IN条件(如果集合为空，则返回1=2)
        /// </summary>
        /// <param name="list"></param>
        /// <param name="entityType"></param>
        /// <param name="paramName"></param>
        /// <param name="type"></param>
        /// <param name="lstIndex"></param>
        public static string IN(Scope scope, ParamList list, Type entityType, string paramName, DbType type, int lstIndex)
        {
            DBInfo db = EntityInfoManager.GetEntityHandle(entityType).DBInfo;
            string paramVal = db.CurrentDbAdapter.FormatValueName(DataAccessCommon.FormatParam(paramName, lstIndex));
            string paramKey = db.CurrentDbAdapter.FormatParamKeyName(DataAccessCommon.FormatParam(paramName, lstIndex));
            StringBuilder sql = new StringBuilder(500);
            ScopeType ctype = scope.ScopeType;
            string connectString = DataAccessCommon.GetConnectString(scope);
            string inValue = null;

            inValue = GetInString(scope.Value1, type, db);
            if (inValue != "" && inValue != null)
            {
                sql.Append(" ");
                sql.Append(connectString);
                sql.Append(" (");
                sql.Append(db.CurrentDbAdapter.FormatParam(paramName));
                sql.Append(" in (");
                sql.Append(inValue);
                sql.Append("))");
            }
            else //没有数据时候设置改条件不成立
            {
                sql.Append(" ");
                sql.Append(connectString);
                sql.Append(" 1=2");
            }
            return sql.ToString();
        }

        /// <summary>
        /// 解释NotIN条件(如果集合为空，则返回1=1)
        /// </summary>
        /// <param name="list"></param>
        /// <param name="entityType"></param>
        /// <param name="paramName"></param>
        /// <param name="type"></param>
        /// <param name="lstIndex"></param>
        public static string NotIN(Scope scope, ParamList list, Type entityType, string paramName, DbType type, int lstIndex)
        {
            DBInfo db = EntityInfoManager.GetEntityHandle(entityType).DBInfo;
            string paramVal = db.CurrentDbAdapter.FormatValueName(DataAccessCommon.FormatParam(paramName, lstIndex));
            string paramKey = db.CurrentDbAdapter.FormatParamKeyName(DataAccessCommon.FormatParam(paramName, lstIndex));
            StringBuilder sql = new StringBuilder(500);
            ScopeType ctype = scope.ScopeType;
            string connectString = DataAccessCommon.GetConnectString(scope);
            string inValue = null;

            inValue = GetInString(scope.Value1, type, db);
            if (inValue != "" && inValue != null)
            {
                sql.Append(" ");
                sql.Append(connectString);
                sql.Append(" (");
                sql.Append(db.CurrentDbAdapter.FormatParam(paramName));
                sql.Append(" not in (");
                sql.Append(inValue);
                sql.Append("))");
            }
            else //没有数据时候设置改条件不成立
            {
                sql.Append(" ");
                sql.Append(connectString);
                sql.Append(" 1=1");
            }
            return sql.ToString();
        }

        /// <summary>
        /// 解释Less条件
        /// </summary>
        /// <param name="list"></param>
        /// <param name="entityType"></param>
        /// <param name="paramName"></param>
        /// <param name="type"></param>
        /// <param name="lstIndex"></param>
        public static string Less(Scope scope, ParamList list, Type entityType, string paramName, DbType type, int lstIndex)
        {
            DBInfo db = EntityInfoManager.GetEntityHandle(entityType).DBInfo;
            string paramVal = db.CurrentDbAdapter.FormatValueName(DataAccessCommon.FormatParam(paramName, lstIndex));
            string paramKey = db.CurrentDbAdapter.FormatParamKeyName(DataAccessCommon.FormatParam(paramName, lstIndex));
            StringBuilder sql = new StringBuilder(500);
            ScopeType ctype = scope.ScopeType;
            string connectString = DataAccessCommon.GetConnectString(scope);

            if (list != null)
            {
                sql.Append(" ");
                sql.Append(connectString);
                sql.Append(" (");
                sql.Append(db.CurrentDbAdapter.FormatParam(paramName));
                sql.Append(" < ");
                sql.Append(paramVal);
                sql.Append(")");
                list.AddNew(paramKey, type, scope.Value1);

            }
            else
            {
                sql.Append(" ");
                sql.Append(connectString);
                sql.Append(" (");
                sql.Append(db.CurrentDbAdapter.FormatParam(paramName));
                sql.Append(" < "); sql.Append(DataAccessCommon.FormatValue(scope.Value1, type, db));
                sql.Append(")");
            }
            return sql.ToString();
        }


        /// <summary>
        /// 解释LessThen条件
        /// </summary>
        /// <param name="list"></param>
        /// <param name="entityType"></param>
        /// <param name="paramName"></param>
        /// <param name="type"></param>
        /// <param name="lstIndex"></param>
        public static string LessThen(Scope scope, ParamList list, Type entityType, string paramName, DbType type, int lstIndex)
        {
            DBInfo db = EntityInfoManager.GetEntityHandle(entityType).DBInfo;
            string paramVal = db.CurrentDbAdapter.FormatValueName(DataAccessCommon.FormatParam(paramName, lstIndex));
            string paramKey = db.CurrentDbAdapter.FormatParamKeyName(DataAccessCommon.FormatParam(paramName, lstIndex));
            StringBuilder sql = new StringBuilder(500);
            ScopeType ctype = scope.ScopeType;
            string connectString = DataAccessCommon.GetConnectString(scope);

            if (list != null)
            {
                sql.Append(" ");
                sql.Append(connectString);
                sql.Append(" (");
                sql.Append(db.CurrentDbAdapter.FormatParam(paramName));
                sql.Append(" <= ");
                sql.Append(paramVal);
                sql.Append(")");
                list.AddNew(paramKey, type, scope.Value1);

            }
            else
            {
                sql.Append(" ");
                sql.Append(connectString);
                sql.Append(" (");
                sql.Append(db.CurrentDbAdapter.FormatParam(paramName));
                sql.Append(" <= " + DataAccessCommon.FormatValue(scope.Value1, type, db));
                sql.Append(")");
            }
            return sql.ToString();
        }


        /// <summary>
        /// 解释More条件
        /// </summary>
        /// <param name="list"></param>
        /// <param name="entityType"></param>
        /// <param name="paramName"></param>
        /// <param name="type"></param>
        /// <param name="lstIndex"></param>
        public static string More(Scope scope, ParamList list, Type entityType, string paramName, DbType type, int lstIndex)
        {
            DBInfo db = EntityInfoManager.GetEntityHandle(entityType).DBInfo;
            string paramVal = db.CurrentDbAdapter.FormatValueName(DataAccessCommon.FormatParam(paramName, lstIndex));
            string paramKey = db.CurrentDbAdapter.FormatParamKeyName(DataAccessCommon.FormatParam(paramName, lstIndex));
            StringBuilder sql = new StringBuilder(500);
            ScopeType ctype = scope.ScopeType;
            string connectString = DataAccessCommon.GetConnectString(scope);

            if (list != null)
            {
                sql.Append(" ");
                sql.Append(connectString);
                sql.Append(" (");
                sql.Append(db.CurrentDbAdapter.FormatParam(paramName));
                sql.Append(" > ");
                sql.Append(paramVal);
                sql.Append(")");
                list.AddNew(paramVal, type, scope.Value1);

            }
            else
            {
                sql.Append(" ");
                sql.Append(connectString);
                sql.Append(" (");
                sql.Append(db.CurrentDbAdapter.FormatParam(paramName));
                sql.Append(" > ");
                sql.Append(DataAccessCommon.FormatValue(scope.Value1, type, db));
                sql.Append(")");
            }
            return sql.ToString();
        }

        /// <summary>
        /// 解释MoreThen条件
        /// </summary>
        /// <param name="list"></param>
        /// <param name="entityType"></param>
        /// <param name="paramName"></param>
        /// <param name="type"></param>
        /// <param name="lstIndex"></param>
        public static string MoreThen(Scope scope, ParamList list, Type entityType, string paramName, DbType type, int lstIndex)
        {
            DBInfo db = EntityInfoManager.GetEntityHandle(entityType).DBInfo;
            string paramVal = db.CurrentDbAdapter.FormatValueName(DataAccessCommon.FormatParam(paramName, lstIndex));
            string paramKey = db.CurrentDbAdapter.FormatParamKeyName(DataAccessCommon.FormatParam(paramName, lstIndex));
            StringBuilder sql = new StringBuilder(500);
            ScopeType ctype = scope.ScopeType;
            string connectString = DataAccessCommon.GetConnectString(scope);

            if (list != null)
            {
                sql.Append(" ");
                sql.Append(connectString);
                sql.Append(" (");
                sql.Append(db.CurrentDbAdapter.FormatParam(paramName));
                sql.Append(" >= ");
                sql.Append(paramVal);
                sql.Append(")");
                list.AddNew(paramKey, type, scope.Value1);

            }
            else
            {
                sql.Append(" ");
                sql.Append(connectString);
                sql.Append(" (");
                sql.Append(db.CurrentDbAdapter.FormatParam(paramName));
                sql.Append(" >= ");
                sql.Append(DataAccessCommon.FormatValue(scope.Value1, type, db));
                sql.Append(")");
            }
            return sql.ToString();
        }

        /// <summary>
        /// 解释NotEqual条件
        /// </summary>
        /// <param name="list"></param>
        /// <param name="entityType"></param>
        /// <param name="paramName"></param>
        /// <param name="type"></param>
        /// <param name="lstIndex"></param>
        public static string NotEqual(Scope scope, ParamList list, Type entityType, string paramName, DbType type, int lstIndex)
        {
            DBInfo db = EntityInfoManager.GetEntityHandle(entityType).DBInfo;
            string paramVal = db.CurrentDbAdapter.FormatValueName(DataAccessCommon.FormatParam(paramName, lstIndex));
            string paramKey = db.CurrentDbAdapter.FormatParamKeyName(DataAccessCommon.FormatParam(paramName, lstIndex));
            StringBuilder sql = new StringBuilder(500);
            ScopeType ctype = scope.ScopeType;
            string connectString = DataAccessCommon.GetConnectString(scope);

            if (scope.Value1 == null)
            {
                sql.Append(" ");
                sql.Append(connectString);
                sql.Append(" (");
                sql.Append(db.CurrentDbAdapter.FormatParam(paramName));
                sql.Append(" is not null)");
            }
            else
            {
                if (list != null)
                {
                    sql.Append(" ");
                    sql.Append(connectString);
                    sql.Append(" (");
                    sql.Append(db.CurrentDbAdapter.FormatParam(paramName));
                    sql.Append(" <> ");
                    sql.Append(paramVal);
                    sql.Append(")");
                    list.AddNew(paramKey, type, scope.Value1);

                }
                else
                {
                    sql.Append(" ");
                    sql.Append(connectString);
                    sql.Append(" (");
                    sql.Append(db.CurrentDbAdapter.FormatParam(paramName));
                    sql.Append(" <> ");
                    sql.Append(DataAccessCommon.FormatValue(scope.Value1, type, db));
                    sql.Append(")");
                }
            }
            return sql.ToString();
        }

        /// <summary>
        /// 解释Equal条件
        /// </summary>
        /// <param name="list"></param>
        /// <param name="entityType"></param>
        /// <param name="paramName"></param>
        /// <param name="type"></param>
        /// <param name="lstIndex"></param>
        public static string Equal(Scope scope, ParamList list, Type entityType, string paramName, DbType type, int lstIndex)
        {
            DBInfo db = EntityInfoManager.GetEntityHandle(entityType).DBInfo;
            string paramVal = db.CurrentDbAdapter.FormatValueName(DataAccessCommon.FormatParam(paramName, lstIndex));
            string paramKey = db.CurrentDbAdapter.FormatParamKeyName(DataAccessCommon.FormatParam(paramName, lstIndex));
            StringBuilder sql = new StringBuilder(500);
            ScopeType ctype = scope.ScopeType;
            string connectString = DataAccessCommon.GetConnectString(scope);

            if (scope.Value1 == null)
            {
                sql.Append(" ");
                sql.Append(connectString);
                sql.Append(" (");
                sql.Append(db.CurrentDbAdapter.FormatParam(paramName));
                sql.Append(" is null)");
            }
            else
            {
                if (list != null)
                {
                    sql.Append(" ");
                    sql.Append(connectString);
                    sql.Append(" (");
                    sql.Append(db.CurrentDbAdapter.FormatParam(paramName));
                    sql.Append(" = ");
                    sql.Append(paramVal);
                    sql.Append(")");
                    list.AddNew(paramKey, type, scope.Value1);
                }
                else
                {
                    sql.Append(" ");
                    sql.Append(connectString);
                    sql.Append(" (");
                    sql.Append(db.CurrentDbAdapter.FormatParam(paramName));
                    sql.Append(" = ");
                    sql.Append(DataAccessCommon.FormatValue(scope.Value1, type, db));
                    sql.Append(")");
                }
            }
            return sql.ToString();
        }

        /// <summary>
        /// 解释Like条件
        /// </summary>
        /// <param name="list"></param>
        /// <param name="entityType"></param>
        /// <param name="paramName"></param>
        /// <param name="type"></param>
        /// <param name="lstIndex"></param>
        public static string Like(Scope scope, ParamList list, Type entityType, string paramName, DbType type, int lstIndex)
        {
            DBInfo db = EntityInfoManager.GetEntityHandle(entityType).DBInfo;
            string paramVal = db.CurrentDbAdapter.FormatValueName(DataAccessCommon.FormatParam(paramName, lstIndex));
            string paramKey = db.CurrentDbAdapter.FormatParamKeyName(DataAccessCommon.FormatParam(paramName, lstIndex));
            StringBuilder sql = new StringBuilder(500);
            ScopeType ctype = scope.ScopeType;
            string connectString = DataAccessCommon.GetConnectString(scope);

            sql.Append(FullTextConfigManager.GetLikeSql(scope, list, paramName, type, lstIndex, entityType, connectString, false));
            return sql.ToString();
        }


        /// <summary>
        /// 解释Contains条件
        /// </summary>
        /// <param name="list"></param>
        /// <param name="entityType"></param>
        /// <param name="paramName"></param>
        /// <param name="type"></param>
        /// <param name="lstIndex"></param>
        public static string Contains(Scope scope, ParamList list, Type entityType, string paramName, DbType type, int lstIndex)
        {
            DBInfo db = EntityInfoManager.GetEntityHandle(entityType).DBInfo;
            string paramVal = db.CurrentDbAdapter.FormatValueName(DataAccessCommon.FormatParam(paramName, lstIndex));
            string paramKey = db.CurrentDbAdapter.FormatParamKeyName(DataAccessCommon.FormatParam(paramName, lstIndex));
            StringBuilder sql = new StringBuilder(500);
            ScopeType ctype = scope.ScopeType;
            string connectString = DataAccessCommon.GetConnectString(scope);

            sql.Append(FullTextConfigManager.GetLikeSql(scope, list, paramName, type, lstIndex, entityType, connectString, true));
            return sql.ToString();
        }

        /// <summary>
        /// 解释StarWith条件
        /// </summary>
        /// <param name="list"></param>
        /// <param name="entityType"></param>
        /// <param name="paramName"></param>
        /// <param name="type"></param>
        /// <param name="lstIndex"></param>
        public static string StarWith(Scope scope, ParamList list, Type entityType, string paramName, DbType type, int lstIndex)
        {
            DBInfo db = EntityInfoManager.GetEntityHandle(entityType).DBInfo;
            string paramVal = db.CurrentDbAdapter.FormatValueName(DataAccessCommon.FormatParam(paramName, lstIndex));
            string paramKey = db.CurrentDbAdapter.FormatParamKeyName(DataAccessCommon.FormatParam(paramName, lstIndex));
            StringBuilder sql = new StringBuilder(500);
            ScopeType ctype = scope.ScopeType;
            string connectString = DataAccessCommon.GetConnectString(scope);

            if (list != null)
            {
                sql.Append(" ");
                sql.Append(connectString);
                sql.Append(" (" + db.CurrentDbAdapter.FormatParam(paramName) + " like " + db.CurrentDbAdapter.ConcatString(paramVal, "'%'") + ")");
                list.AddNew(paramKey, type, scope.Value1);

            }
            else
            {
                string curValue = scope.Value1.ToString();
                sql.Append(" ");
                sql.Append(connectString);
                sql.Append(" (" + db.CurrentDbAdapter.FormatParam(paramName) + " like '" + curValue + "%')");
            }
            return sql.ToString();
        }

        /// <summary>
        /// 解释EndWith条件
        /// </summary>
        /// <param name="list"></param>
        /// <param name="entityType"></param>
        /// <param name="paramName"></param>
        /// <param name="type"></param>
        /// <param name="lstIndex"></param>
        public static string EndWith(Scope scope, ParamList list, Type entityType, string paramName, DbType type, int lstIndex)
        {
            DBInfo db = EntityInfoManager.GetEntityHandle(entityType).DBInfo;
            string paramVal = db.CurrentDbAdapter.FormatValueName(DataAccessCommon.FormatParam(paramName, lstIndex));
            string paramKey = db.CurrentDbAdapter.FormatParamKeyName(DataAccessCommon.FormatParam(paramName, lstIndex));
            StringBuilder sql = new StringBuilder(500);
            ScopeType ctype = scope.ScopeType;
            string connectString = DataAccessCommon.GetConnectString(scope);

            if (list != null)
            {
                sql.Append(" ");
                sql.Append(connectString);
                sql.Append(" (" + db.CurrentDbAdapter.FormatParam(paramName) + " like " + db.CurrentDbAdapter.ConcatString("'%'", paramVal) + ")");
                list.AddNew(paramKey, type, scope.Value1);

            }
            else
            {
                sql.Append(" ");
                sql.Append(connectString);
                string curValue = scope.Value1.ToString();
                sql.Append(" (" + db.CurrentDbAdapter.FormatParam(paramName) + " like '%" + scope.Value1 + "')");
            }
            return sql.ToString();
        }

        /// <summary>
        /// 解释Condition条件
        /// </summary>
        /// <param name="scope">条件</param>
        /// <param name="list"></param>
        /// <param name="entityType"></param>
        /// <param name="paramName"></param>
        /// <param name="type"></param>
        /// <param name="lstIndex"></param>
        public static string Condition(Scope scope, ParamList list, Type entityType, string paramName, DbType type, int lstIndex)
        {
            DBInfo db = EntityInfoManager.GetEntityHandle(entityType).DBInfo;
            string paramVal = db.CurrentDbAdapter.FormatValueName(DataAccessCommon.FormatParam(paramName, lstIndex));
            string paramKey = db.CurrentDbAdapter.FormatParamKeyName(DataAccessCommon.FormatParam(paramName, lstIndex));
            StringBuilder sql = new StringBuilder(500);
            ScopeType ctype = scope.ScopeType;
            string connectString = DataAccessCommon.GetConnectString(scope);

            BQLCondition fhandle = scope.Value1 as BQLCondition;
            if (!CommonMethods.IsNull(fhandle))
            {
                KeyWordInfomation info = new KeyWordInfomation();
                info.Infos = new Buffalo.DB.BQLCommon.BQLInfos();
                info.DBInfo = db;

                info.ParamList = list;
                sql.Append(" ");
                sql.Append(connectString);

                sql.Append(" (" + fhandle.DisplayValue(info) + ")");
            }
            return sql.ToString();
        }


        /// <summary>
        /// 根据数据类型自动拼合in字符串
        /// </summary>
        /// <param name="collection">值集合</param>
        /// <param name="type">类型</param>
        /// <returns></returns>
        private static string GetInString(object collection, DbType type, DBInfo info)
        {
            IEnumerable enumValues = (IEnumerable)collection;
            string ret = "";
            foreach (object item in enumValues)
            {
                ret += DataAccessCommon.FormatValue(item, type, info) + ",";
            }
            if (ret.Length > 0)
            {
                ret = ret.Substring(0, ret.Length - 1);
            }
            return ret;
        }


    }
}
