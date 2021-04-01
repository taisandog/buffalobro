using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Data;

using Buffalo.DB.FaintnessSearchConditions;
using Buffalo.DB.DataBaseAdapter;
using Buffalo.DB.QueryConditions;
using Buffalo.DB.DbCommon;
using Buffalo.Kernel.Defaults;
using Buffalo.DB.EntityInfos;
using Buffalo.DB.BQLCommon.BQLConditionCommon;
using Buffalo.Kernel;
using Buffalo.DB.DataFillers;
using Buffalo.DB.BQLCommon;

namespace Buffalo.DB.CommBase.DataAccessBases
{
    public class DataAccessCommon
    {

        /// <summary>
        /// 获取条件连接字符串
        /// </summary>
        /// <param name="objScope"></param>
        /// <returns></returns>
        public static string GetConnectString(Scope objScope)
        {
            if (objScope.ConnectType == ConnectType.And)
            {
                return "And";
            }
            return "Or";
        }


        /// <summary>
        /// 格式化变量名称
        /// </summary>
        /// <param name="pam">变量</param>
        /// <param name="index">当前的标识</param>
        /// <returns></returns>
        public static string FormatParam(string pam, int index)
        {
            return pam + "_" + index.ToString();
        }

        /// <summary>
        /// 填充查询条件并返回条件的SQL语句( and 开头)
        /// </summary>
        /// <param name="lstParam">参数列表</param>
        /// <param name="lstScope">范围查询集合</param>
        /// <returns></returns>
        internal static string FillCondition(EntityInfoHandle curEntityInfo,ParamList lstParam, ScopeBaseList lstScope)
        {
            int index = 0;
            return FillCondition(curEntityInfo,lstParam, lstScope, ref index);
        }


        /// <summary>
        /// 填充查询条件并返回条件的SQL语句( and 开头)
        /// </summary>
        /// <param name="lstParam">参数列表</param>
        /// <param name="lstScope">范围查询集合</param>
        /// <param name="CurEntityInfo">当前实体信息</param>
        /// <param name="index">索引</param>
        /// <returns></returns>
        internal static string FillCondition(EntityInfoHandle curEntityInfo, ParamList lstParam, ScopeBaseList lstScope, ref int index)
        {
            if (lstScope == null)
            {
                return "";
            }
            StringBuilder ret = new StringBuilder();
            for (int i = 0; i < lstScope.Count; i++)
            {
                Scope objScope = lstScope[i];
                EntityPropertyInfo info = null;
                if (!string.IsNullOrEmpty(objScope.PropertyName))
                {
                    info = curEntityInfo.PropertyInfo[objScope.PropertyName];
                }

                if (objScope.ScopeType == ScopeType.Scope)
                {
                    ScopeList lstInnerScope = objScope.Value1 as ScopeList;
                    if (lstInnerScope != null)
                    {
                        string strSql = FillCondition(curEntityInfo,lstParam, lstInnerScope, ref index);
                        string connectString = DataAccessCommon.GetConnectString(objScope);
                        StringBuilder sbstrSQL = new StringBuilder(strSql);
                        DataAccessCommon.TrimAnd(sbstrSQL);
                        ret.Append(" ");
                        ret.Append(connectString);
                        ret.Append(" (" + sbstrSQL.ToString() + ")");
                    }
                }
                else
                {
                    string pName = (info != null ? info.ParamName : "");
                    DbType dbType = (info != null ? info.SqlType : DbType.Object);
                    ret.Append(FormatScorp(objScope, lstParam, pName, dbType, index, curEntityInfo.EntityType));
                }
                index++;
            }
            return ret.ToString();
        }
        /// <summary>
        /// 删除起始的条件符
        /// </summary>
        /// <param name="sb"></param>
        internal static void TrimHead(StringBuilder sb)
        {
            if (sb.Length == 0)
            {
                return;
            }
            int sindex = GetStartIndex(sb);
            const string sqlTrim = " 1=1 and";
            //const string sqlTrim = "1=1 and";
            int len = sb.Length > sqlTrim.Length ? sqlTrim.Length : sb.Length;
            len += sindex;
            string andSql = sb.ToString(0, len);
            
            if (andSql.IndexOf("1=1 and", sindex, StringComparison.CurrentCultureIgnoreCase) == sindex)
            {
                sb.Remove(sindex, sqlTrim.Length - 1);
                return;
            }
            if (andSql.IndexOf("1=1", sindex, StringComparison.CurrentCultureIgnoreCase) == sindex
                && len < sqlTrim.Length)
            {
                sb.Remove(sindex, 3);
                return;
            }
            if (andSql.IndexOf(" 1=1", sindex, StringComparison.CurrentCultureIgnoreCase) == sindex
                && len < sqlTrim.Length)
            {
                sb.Remove(sindex, 4);
                return;
            }
            if (andSql.IndexOf(" 1=1 and", sindex, StringComparison.CurrentCultureIgnoreCase) == sindex)
            {
                sb.Remove(sindex, sqlTrim.Length);
                return;
            }
        }

        /// <summary>
        /// 获取真正开始的索引
        /// </summary>
        /// <param name="sb"></param>
        /// <returns></returns>
        private static int GetStartIndex(StringBuilder sb) 
        {
            char cur = '\0';
            for (int i = 0; i < sb.Length; i++) 
            {
                cur = sb[i];
                if (cur != ' ' && cur != '(') 
                {
                    return i;
                }
            }
            return 0;
        }

        /// <summary>
        /// 删除起始的条件符
        /// </summary>
        /// <param name="sb"></param>
        internal static void TrimAnd(StringBuilder sb)
        {
            if (sb.Length == 0) 
            {
                return;
            }
            int len = sb.Length > 5 ? 5 : sb.Length;
            string andSql = sb.ToString(0, len);
            if (andSql.IndexOf(" and ",StringComparison.CurrentCultureIgnoreCase)==0)
            {
                sb.Remove(0, 5);
            }
            else if (andSql.IndexOf("and ", StringComparison.CurrentCultureIgnoreCase) == 0) 
            {
                sb.Remove(0, 4);
            }
            else if (andSql.IndexOf(" or ", StringComparison.CurrentCultureIgnoreCase) == 0)
            {
                sb.Remove(0, 4);
            }
            else if (andSql.IndexOf("or ", StringComparison.CurrentCultureIgnoreCase) == 0)
            {
                sb.Remove(0, 3);
            }
        }

        /// <summary>
        /// 返回当前条件的字符串
        /// </summary>
        /// <param name="scope">条件类</param>
        /// <param name="list">参数列表</param>
        /// <param name="paramName">所属的字段名</param>
        /// <param name="type">当前的数据库类型</param>
        /// <param name="lstIndex">当前索引的标识未辨别同名字段的参数，可设置为0</param>
        /// <param name="entityType">当前实体的类型</param>
        /// <returns></returns>
        public static string FormatScorp(Scope scope, ParamList list, string paramName, DbType type, int lstIndex,Type entityType)
        {
            string sql = null;
            ScopeExplainer handle=ExplainScope.GetExplainer(scope);
            if (handle != null) 
            {
                sql = handle(scope, list, entityType, paramName, type, lstIndex);
            }
            
            
            return sql;
        }

        /// <summary>
        /// 把查询条件加到条件的字符串里
        /// </summary>
        /// <param name="value">值</param>
        /// <param name="type">数据库里边的类型</param>
        /// <returns></returns>
        public static string FormatValue(object value, DbType type,DBInfo db)
        {
            if (value == null)
            {
                return null;
            }

            switch (type)
            {
                case DbType.AnsiString:
                case DbType.AnsiStringFixedLength:
                case DbType.String:
                case DbType.StringFixedLength:
                    return "'" + value.ToString().Replace("'", "''") + "'";
                case DbType.Guid:
                    if (value is Guid)
                    {
                        return Buffalo.Kernel.CommonMethods.GuidToString((Guid)value);
                    }
                    return value.ToString();
                case DbType.DateTime:
                case DbType.Time:
                case DbType.Date:
                case DbType.DateTime2:
                case DbType.DateTimeOffset:
                    return db.CurrentDbAdapter.GetDateTimeString(value);
                case DbType.Decimal:
                case DbType.Double:
                case DbType.Int32:
                case DbType.Int16:
                case DbType.Int64:
                case DbType.SByte:
                case DbType.Byte:
                case DbType.Currency:
                case DbType.UInt16:
                case DbType.UInt32:
                case DbType.UInt64:
                case DbType.VarNumeric:
                case DbType.Single:
                    return value.ToString().Replace(" ","");
                case DbType.Binary:
                    byte[] binaryValue = value as byte[];
                    if (binaryValue != null)
                    {
                        string hexVal = CommonMethods.BytesToHexString(binaryValue,true);
                        return "0x" + hexVal;
                    }
                    return "";
                case DbType.Boolean:
                    bool valBool = Convert.ToBoolean(value);
                    if (valBool == true)
                    {
                        return "1";
                    }
                    else
                    {
                        return "0";
                    }
                default:
                    return null;
            }
        }
        #region 填充子类
        /// <summary>
        /// 填充子属性列表
        /// </summary>
        /// <param name="lst"></param>
        /// <param name="childName"></param>
        public static void FillEntityChidList(IList lst, ScopeList lstScope)
        {
            ShowChildCollection childs = lstScope.ShowChild;
            if(childs==null)
            {
                return;
            }
            foreach (ShowChildItem showTable in childs) 
            {

                FillEntityChidList(lst, showTable);
            }
        }

        /// <summary>
        /// 填充子集合
        /// </summary>
        /// <param name="showTable"></param>
        /// <param name="?"></param>
        public static void FillEntityChidList(IList lst, ShowChildItem showTable)
        {
            Stack<BQLEntityTableHandle> stkTables = new Stack<BQLEntityTableHandle>();
            BQLEntityTableHandle curTable = showTable.ChildItem;
            while (true)
            {
                stkTables.Push(curTable);
                curTable = curTable.GetParentTable();
                if (CommonMethods.IsNull(curTable) || string.IsNullOrEmpty(curTable.GetPropertyName())) 
                {
                    break;
                }
            }
            IEnumerable curList = lst;
            Queue<object> lastObjects = new Queue<object>();
            ScopeList empty = new ScopeList();
            while (stkTables.Count > 0)
            {
                BQLEntityTableHandle table = stkTables.Pop();
                ScopeList lstScope = null;
                if (stkTables.Count == 0)
                {
                    lstScope = showTable.FilterScope;
                }
                else 
                {
                    lstScope = empty;
                }
                FillEntityChidList(curList, table.GetPropertyName(), lastObjects, table.GetParentTable().GetEntityInfo().EntityType, lstScope);
                curList = lastObjects;
                lastObjects=new Queue<object>();
            }

        }

        /// <summary>
        /// 填充子属性列表
        /// </summary>
        /// <param name="lst">集合</param>
        /// <param name="childPropertyName">子属性名</param>
        /// <param name="objs">实体</param>
        /// <param name="objType">类型</param>
        /// <param name="filter">筛选条件</param>
        private static void FillEntityChidList(IEnumerable lst, string childPropertyName, Queue<object> objs, Type objType,ScopeList filter)
        {
            if (lst == null)
            {
                return;
            }
            
            EntityInfoHandle entityInfo = EntityInfoManager.GetEntityHandle(objType);
            if (entityInfo == null)
            {
                throw new Exception("找不到类:" + objType.FullName + "的映射");
            }
            EntityMappingInfo mappingInfo = entityInfo.MappingInfo[childPropertyName];
            if (mappingInfo == null)
            {
                throw new Exception("找不到子属性:" + childPropertyName);
            }
            Queue<object> pks = CollectFks(lst, mappingInfo.SourceProperty);
            EntityInfoHandle childHandle = mappingInfo.TargetProperty.BelongInfo;//获取子元素的信息
            Dictionary<string, List<object>> dic = GetEntityDictionary(lst, mappingInfo);
            FillChilds(pks, childHandle, mappingInfo, dic, childPropertyName, objs, filter);
        }

        /// <summary>
        /// 把集合转成字典形式
        /// </summary>
        /// <param name="lst">集合</param>
        /// <param name="mappingInfo">映射</param>
        /// <returns></returns>
        private static Dictionary<string, List<object>> GetEntityDictionary(IEnumerable lst, EntityMappingInfo mappingInfo) 
        {
            Dictionary<string, List<object>> dic = new Dictionary<string, List<object>>();
            string propertyName=mappingInfo.PropertyName;

            List<object> lstCur = null;
            foreach (object obj in lst) 
            {
                EntityBase entity = obj as EntityBase;
                if(entity==null)
                {
                    continue;
                }
                string key = mappingInfo.SourceProperty.GetValue(entity).ToString();
                if (!dic.TryGetValue(key, out lstCur)) 
                {
                    lstCur = new List<object>();
                    dic[key] = lstCur;
                }
                lstCur.Add(entity);
                //创建子集合
                if (!mappingInfo.IsParent)
                {
                    object childlst = Activator.CreateInstance(mappingInfo.FieldType);
                    mappingInfo.SetValue(entity, childlst);
                    entity.OnFillChild(propertyName, new ScopeList());
                }
            }
            return dic;
        }

        /// <summary>
        /// 收集外键值
        /// </summary>
        /// <param name="lst"></param>
        /// <param name="pk"></param>
        /// <returns></returns>
        private static Queue<object> CollectFks(IEnumerable lst, EntityPropertyInfo pk)
        {
            Queue<object> pks = new Queue<object>();
            foreach (object obj in lst)
            {
                object value = pk.GetValue(obj);
                pks.Enqueue(value);
            }
            return pks;
        }

        private const int MaxID =500;

        /// <summary>
        /// 填充字类列表
        /// </summary>
        /// <param name="pks">ID集合</param>
        /// <param name="childHandle">子元素的信息句柄</param>
        /// <param name="mappingInfo">映射信息</param>
        /// <param name="dicElement">元素</param>
        /// <param name="propertyName">属性名</param>
        /// <param name="curObjs"></param>
        /// <param name="filter"></param>
        private static void FillChilds(Queue<object> pks, EntityInfoHandle childHandle, EntityMappingInfo mappingInfo,
            Dictionary<string, List<object>> dicEntity, string propertyName, Queue<object> curObjs,ScopeList filter)
        {

            DBInfo db = childHandle.DBInfo;
            DataBaseOperate oper = childHandle.DBInfo.DefaultOperate;
            BQLDbBase dao = new BQLDbBase(oper);
            Queue<object> needCollect = null;
            List<EntityPropertyInfo> lstParamNames=null;
            try
            {
                while (pks.Count>0) 
                {
                    needCollect = GetCurPks(pks);
                    FillChildReader(needCollect, mappingInfo, dicEntity, dao, ref lstParamNames, db,curObjs,filter);
                }
            }
            finally 
            {
                oper.AutoClose();
            }
        }
        /// <summary>
        /// 获取当前需要查询的主键集合
        /// </summary>
        /// <param name="pks"></param>
        /// <returns></returns>
        private static Queue<object> GetCurPks(Queue<object> pks) 
        {
            Queue<object> needCollect = null;
            int cur = pks.Count;
            if (cur > MaxID) 
            {
                cur = MaxID;
            }
            needCollect = new Queue<object>();
            for (int i = 0; i < cur; i++) 
            {
                needCollect.Enqueue(pks.Dequeue());
            }
            return needCollect;
        }

        /// <summary>
        /// 查询并填充子类信息
        /// </summary>
        /// <param name="pks"></param>
        /// <param name="mappingInfo"></param>
        /// <param name="dicEntity"></param>
        /// <param name="dao"></param>
        /// <param name="lstParamNames"></param>
        /// <param name="db"></param>
        /// <param name="curObjs"></param>
        /// <param name="filter"></param>
        private static void FillChildReader(Queue<object> pks, EntityMappingInfo mappingInfo, Dictionary<string, List<object>> dicEntity, BQLDbBase dao,
            ref List<EntityPropertyInfo> lstParamNames, DBInfo db,Queue<object> curObjs,ScopeList filter) 
        {
            EntityInfoHandle childInfo = mappingInfo.TargetProperty.BelongInfo;
            string fullName = mappingInfo.TargetProperty.BelongInfo.EntityType.FullName;
            Type childType = mappingInfo.TargetProperty.BelongInfo.EntityType;
            List<object> senders = null;

            while (pks.Count > 0)
            {
                Queue<object> searchPks = GetSearchPKs(pks);
                if (searchPks.Count <= 0) 
                {
                    break;
                }

                ScopeList lstScope = new ScopeList();
                lstScope.AddScopeList(filter);
                lstScope.AddIn(mappingInfo.TargetProperty.PropertyName, searchPks);
                using (IDataReader reader = dao.QueryReader(lstScope, childInfo.EntityType))
                {
                    //获取子表的get列表
                    if (lstParamNames == null)
                    {
                        lstParamNames = CacheReader.GenerateCache(reader, childInfo);//创建一个缓存数值列表
                    }

                    
                    while (reader.Read())
                    {
                        string fk = reader[mappingInfo.TargetProperty.ParamName].ToString();
                        if (!dicEntity.TryGetValue(fk, out senders))
                        {
                            continue;
                        }
                        object obj = childInfo.CreateSelectProxyInstance();

                        if (curObjs != null)
                        {
                            curObjs.Enqueue(obj);
                        }
                        CacheReader.FillObjectFromReader(reader, lstParamNames, obj, db);

                        foreach (object sender in senders)
                        {
                            if (mappingInfo.IsParent)
                            {
                                mappingInfo.SetValue(sender, obj);
                            }
                            else
                            {
                                IList lst = (IList)mappingInfo.GetValue(sender);
                                lst.Add(obj);
                            }
                        }
                    }
                }
            }
        
        }

        /// <summary>
        /// 每次查询的条数
        /// </summary>
        internal const int PreSearch=50;
        /// <summary>
        /// 获取需要查询的主键
        /// </summary>
        /// <param name="pks"></param>
        /// <returns></returns>
        private static Queue<object> GetSearchPKs(Queue<object> pks) 
        {
            Queue<object> searchPks = new Queue<object>();
            int copyed = 0;
            while (pks.Count > 0) 
            {
                searchPks.Enqueue(pks.Dequeue());
                copyed++;
                if (copyed >= PreSearch) 
                {
                    break;
                }
            }
            return searchPks;
        }

        #endregion
    }
}
