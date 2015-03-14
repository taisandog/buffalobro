using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using Buffalo.DB.PropertyAttributes;
using System.Collections;
using Buffalo.DB.CacheManager;
using Buffalo.DB.ContantSearchs;
using Buffalo.DB.EntityInfos;
using Buffalo.DB.DataBaseAdapter;
using Buffalo.DB.DataFillers;
using Buffalo.DB.QueryConditions;
using Buffalo.DB.DbCommon;
using Buffalo.Kernel.Defaults;
using Buffalo.Kernel;
using Buffalo.DB.BQLCommon;
using Buffalo.DB.CommBase.BusinessBases;
using Buffalo.DB.BQLCommon.BQLConditionCommon;

namespace Buffalo.DB.CommBase.DataAccessBases
{
    /// <summary>
    /// 数据层基类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DataAccessBase<T> : DataAccessSetBase
        where T : EntityBase, new()
    {

        
        /// <summary>
        /// 当前类型的信息
        /// </summary>
        protected internal readonly static EntityInfoHandle CurEntityInfo = EntityInfoManager.GetEntityHandle(typeof(T));
        private static string allParams=null;

        /// <summary>
        /// 数据层基类
        /// </summary>
        public DataAccessBase()
            : base(CurEntityInfo)
        {

        }
        /// <summary>
        /// 当前实体信息
        /// </summary>
        public override EntityInfoHandle EntityInfo
        {
            get
            {
                return CurEntityInfo;
            }
        }

        /// <summary>
        /// 所有字段名
        /// </summary>
        protected static string AllParamNames 
        {
            get 
            {
                if (allParams == null) 
                {
                    StringBuilder ret = new StringBuilder();
                    foreach (EntityPropertyInfo info in CurEntityInfo.PropertyInfo) 
                    {
                        ret.Append(CurEntityInfo.DBInfo.CurrentDbAdapter.FormatParam(info.ParamName) + ",");
                    }
                    if (ret.Length > 0)
                    {
                        allParams = ret.ToString(0, ret.Length - 1);
                    }
                }
                return allParams;
            }
        }

        
        
        #region 基本方法

        #region 数据定义

        /// <summary>
        /// 获取属性名集合对应的字段名集合
        /// </summary>
        /// <param name="propertyNames">属性名集合</param>
        /// <returns></returns>
        private static List<PropertyParamMapping> GetParamNameList(List<string> propertyNames)
        {
            
            List<PropertyParamMapping> lstParamMapping = new List<PropertyParamMapping>(propertyNames.Count);
            foreach (string propertyName in propertyNames)
            {
                EntityPropertyInfo info = CurEntityInfo.PropertyInfo[propertyName];
                if (info!=null)
                {
                    PropertyParamMapping objMapping = new PropertyParamMapping();
                    objMapping.PropertyName = propertyName;
                    objMapping.ParamName = info.ParamName;
                    objMapping.DataType = info.SqlType;
                    lstParamMapping.Add(objMapping);
                }
            }
            return lstParamMapping;
        }
        #endregion
        #region 查询方法
        /// <summary>
        /// 执行sql语句，返回List
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="list">参数列表</param>
        /// <param name="commandType">语句类型</param>
        public List<T> QueryList(string sql, ParamList list, CommandType commandType, Dictionary<string, bool> cachetables)
        {
            List<T> retlist = null;
            using (IDataReader reader = _oper.Query(sql, list, commandType,cachetables))
            {

                retlist = LoadFromReaderList(reader);
            }

            return retlist;
        }
        /// <summary>
        /// 执行sql语句，分页返回List(游标分页)
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="objPage">分页对象</param>
        public List<T> QueryList(string sql, PageContent objPage)
        {
            List<T> retlist = null;


            using (IDataReader reader = EntityInfo.DBInfo.CurrentDbAdapter.Query(sql, objPage, _oper))
            {

                retlist = LoadFromReaderList(reader);
            }
            return retlist;
        }

        /// <summary>
        /// 执行sql语句，分页返回List(游标分页)
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="lstParam">参数集合</param>
        /// <param name="objPage">分页对象</param>
        public List<T> QueryList(string sql, ParamList lstParam, PageContent objPage)
        {
            List<T> retlist = null;
            using (IDataReader reader = EntityInfo.DBInfo.CurrentDbAdapter.Query(sql, lstParam, objPage, _oper))
            {

                retlist = LoadFromReaderList(reader);
            }
            return retlist;
        }
        #endregion
        #region 条件填充




        /// <summary>
        /// 获取排序条件的SQL语句(不带OrderBy)
        /// </summary>
        /// <param name="lstSort">排序条件集合</param>
        /// <returns></returns>
        protected static string GetSortCondition(SortList lstSort)
        {
            if (lstSort == null)
            {
                return "";
            }
            //排序方式
            StringBuilder orderBy = new StringBuilder(500);
            foreach (Sort objSort in lstSort)
            {
                EntityPropertyInfo info = CurEntityInfo.PropertyInfo[objSort.PropertyName];
                if (info!=null)
                {
                    string strSort = "ASC";
                    if (objSort.SortType == SortType.DESC)
                    {
                        strSort = "DESC";
                    }
                    orderBy.Append(CurEntityInfo.DBInfo.CurrentDbAdapter.FormatParam(info.ParamName));
                    orderBy.Append(" ");
                    orderBy.Append(strSort);
                    orderBy.Append(",");
                }
                else
                {
                    throw new Exception("在排序条件集合里找不到属性：" + objSort.PropertyName);
                }
            }
            if (orderBy.Length>0)
            {
                orderBy.Remove(orderBy.Length - 1, 1);
                //orderBy = orderBy.Substring(0, orderBy.Length - 1);
            }
            return orderBy.ToString();
        }
        
        /// <summary>
        /// 获取本次查询需要显示的字段集合
        /// </summary>
        /// <param name="lstScope">范围查询集合</param>
        /// <returns></returns>
        private static string GetSelectParams(ScopeList lstScope) 
        {
            
            if (lstScope == null) 
            {
                return AllParamNames;
            }
            
            StringBuilder ret = new StringBuilder();

            BQLEntityTableHandle table = CurEntityInfo.DBInfo.FindTable(typeof(T));
            if (CommonMethods.IsNull(table))
            {
                CurEntityInfo.DBInfo.ThrowNotFondTable(typeof(T));
            }
            List<BQLParamHandle> propertyNames = lstScope.GetShowProperty(table);


            if (propertyNames.Count > 0)
            {
                foreach (BQLParamHandle property in propertyNames)
                {
                    BQLEntityParamHandle eproperty = property as BQLEntityParamHandle;
                    if (CommonMethods.IsNull(eproperty))
                    {
                        continue;
                    }
                    EntityPropertyInfo info = eproperty.PInfo;
                    if (info != null)
                    {
                        ret.Append(CurEntityInfo.DBInfo.CurrentDbAdapter.FormatParam(info.ParamName) + ",");
                    }
                }
            }
            else 
            {
                foreach (EntityPropertyInfo info in CurEntityInfo.PropertyInfo)
                {
                    if (info != null)
                    {
                        ret.Append(CurEntityInfo.DBInfo.CurrentDbAdapter.FormatParam(info.ParamName) + ",");
                    }
                }
            }


            if (ret.Length > 0) 
            {
                return ret.ToString(0, ret.Length - 1);
            }
            return AllParamNames;
        }

        /// <summary>
        /// 获取全部查询的条件
        /// </summary>
        /// <param name="list">参数列表</param>
        /// <param name="scopeList">范围查找的集合</param>
        /// <returns></returns>
        protected string GetSelectPageContant(ParamList list, ScopeList scopeList)
        {

            SelectCondition condition = new SelectCondition(CurEntityInfo.DBInfo);
            condition.Oper = this._oper;
            condition.Tables.Append(CurEntityInfo.DBInfo.CurrentDbAdapter.FormatTableName(CurEntityInfo.TableName));
            condition.SqlParams.Append(GetSelectParams(scopeList));
            if (scopeList.UseCache)
            {
                condition.CacheTables = CurEntityInfo.DBInfo.QueryCache.CreateMap(CurEntityInfo.TableName);
            }
            condition.Condition.Append("1=1");
            foreach (EntityPropertyInfo ep in CurEntityInfo.PrimaryProperty)
            {
                condition.PrimaryKey.Add(CurEntityInfo.DBInfo.CurrentDbAdapter.FormatParam(ep.ParamName));
            }
            string conditionWhere = "";

            SortList sortList = scopeList.OrderBy;


            if (scopeList != null)
            {
                condition.Condition.Append(DataAccessCommon.FillCondition(CurEntityInfo, list, scopeList));
            }

            if (conditionWhere.Length > 0)
            {
                condition.Condition.Append(conditionWhere);
            }
            //排序方式
            if (sortList != null && sortList.Count > 0)
            {
                string orderBy = GetSortCondition(sortList);
                if (orderBy != "")
                {
                    if (condition.Orders.Length > 0)
                    {
                        condition.Orders.Append("," + orderBy);
                    }
                    else
                    {
                        condition.Orders.Append(orderBy);
                    }
                }
            }
            condition.PageContent = scopeList.PageContent;
            //throw new Exception("");
            condition.DbParamList = list;

            //if (scopeList.UseCache)
            //{

            //    cachetables[CurEntityInfo.DBInfo.CurrentDbAdapter.FormatTableName(CurEntityInfo.TableName)]=true;
            //}

            return condition.GetSql(true);
        }

        

        /// <summary>
        /// 获取全部查询的条件
        /// </summary>
        /// <param name="list">参数列表</param>
        /// <param name="scopeList">范围查找的集合</param>
        /// <param name="param">输出字段</param>
        /// <returns></returns>
        protected SelectCondition GetSelectContant(ParamList list, ScopeList scopeList, string param)
        {
            string conditionWhere = "";
            string orderBy = "";
            SelectCondition condition = new SelectCondition(CurEntityInfo.DBInfo);
            if (condition.SqlParams.Length > 0)
            {
                condition.SqlParams.Append(",");
            }
            condition.SqlParams.Append(param);

            condition.Tables.Append(CurEntityInfo.DBInfo.CurrentDbAdapter.FormatTableName(CurEntityInfo.TableName));

            condition.Condition.Append("1=1");

            if (scopeList.UseCache)
            {
                condition.CacheTables = CurEntityInfo.DBInfo.QueryCache.CreateMap(CurEntityInfo.TableName);
            }

            if (scopeList != null)
            {
                condition.Condition.Append(DataAccessCommon.FillCondition(CurEntityInfo, list, scopeList));
            }
            if (conditionWhere.Length > 0)
            {
                condition.Condition.Append(conditionWhere);
            }
            SortList sortList = scopeList.OrderBy;
            if (sortList != null && sortList.Count > 0)
            {
                if (orderBy != "")
                {
                    orderBy = orderBy + "," + GetSortCondition(sortList);
                }
                else
                {
                    orderBy = GetSortCondition(sortList);
                }
            }

            if (orderBy != "")
            {
                condition.Orders.Append(orderBy);
            }
            condition.DbParamList = list;
            return condition;
        }
        #endregion
        #region 数据集填充
        /// <summary>
        /// 从Reader里边读取数据
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="reader">reader</param>
        /// <returns></returns>
        protected T LoadFromReader(IDataReader reader)
        {
            return CacheReader.LoadFormReader<T>(reader, CurEntityInfo); ;
        }

        /// <summary>
        /// 从Reader里边读取数据
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="reader">reader</param>
        /// <returns></returns>
        protected List<T> LoadFromReaderList(IDataReader reader)
        {
            return CacheReader.LoadFormReaderList<T>(reader);
        }

        
        #endregion
        

        #endregion
       /// <summary>
        /// 根据ID获取记录
       /// </summary>
       /// <param name="id">ID</param>
       /// <param name="isSearchByCache">是否缓存搜索</param>
       /// <returns></returns>
        public T GetObjectById(object id, bool isSearchByCache)
        {
            
            ParamList list = null;
            T ret = default(T);
            list = new ParamList();
            string tabName = CurEntityInfo.DBInfo.CurrentDbAdapter.FormatTableName(CurEntityInfo.TableName);
            ScopeList lstScope = new ScopeList();
            lstScope.UseCache = isSearchByCache;
            PrimaryKeyInfo pkInfo = id as PrimaryKeyInfo;
            if (pkInfo == null)
            {
                lstScope.AddEqual(CurEntityInfo.PrimaryProperty[0].PropertyName, id);
            }
            else 
            {
                pkInfo.FillScope(CurEntityInfo.PrimaryProperty, lstScope, true);
            }
            SelectCondition sc = GetSelectContant(list, lstScope, GetSelectParams(lstScope));
            //sql.Append( DataAccessCommon.FillCondition(CurEntityInfo,list, lstScope));

            Dictionary<string,bool> cacheTables=null;
            if(lstScope.UseCache)
            {
                cacheTables = _oper.DBInfo.QueryCache.CreateMap(CurEntityInfo.TableName);
            }
            using (IDataReader reader = _oper.Query(sc.GetSql(lstScope.UseCache), list, cacheTables))
            {
                if (reader.Read())
                {
                    ret = LoadFromReader(reader);
                }
            }
            
            return ret;
        }
        // <summary>
        // 根据条件获取第一条记录
        // </summary>
        // <param name="scopeList">查询信息</param>
        // <returns></returns>
        //public T GetUnique(ScopeList scopeList)
        //{
        //    if (scopeList.HasInner)
        //    {
                
        //        return _cdal.GetUnique<T>(scopeList);
        //    }
        //    ParamList list = null;
        //    T ret = default(T);
        //    list = new ParamList();

        //    string sql = null;

        //    SelectCondition sc = GetSelectContant(list, scopeList, GetSelectParams(scopeList));
        //    sql = CurEntityInfo.DBInfo.CurrentDbAdapter.GetTopSelectSql(sc, 1);
        //    Dictionary<string,bool> cacheTables=null;
        //    if(scopeList.UseCache)
        //    {
        //        cacheTables=_oper.DBInfo.QueryCache.CreateMap(CurEntityInfo.TableName);
        //    }
        //    using (IDataReader reader = _oper.Query(sql, list, cacheTables))
        //    {
        //        if (reader.Read())
        //        {
        //            ret = LoadFromReader(reader);

        //        }
        //    }

        //    return ret;
        //}

        /// <summary>
        /// 修改记录
        /// </summary>
        /// <param name="obj">修改的对象</param>
        /// <param name="scopeList">条件列表</param>
        /// <param name="optimisticConcurrency">是否进行并发控制</param>
        /// <returns></returns>
        public int Update(T obj, ScopeList scopeList,ValueSetList lstValue, bool optimisticConcurrency)
        {
            return base.Update(obj, scopeList,lstValue, optimisticConcurrency);
        }

        /// <summary>
        /// 插入一个记录
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int Insert(T obj,ValueSetList setList, bool fillIdentity)
        {
            int ret = -1;
            ret = DoInsert(obj, setList, fillIdentity);
            return ret;
        }

        #region Select




        /// <summary>
        /// 查询表
        /// </summary>
        /// <param name="scopeList">范围查找的集合</param>
        /// <returns></returns>
        public DataSet Select(ScopeList scopeList)
        {
            if (scopeList.HasInner )
            {
                if (scopeList.OrderBy.Count <= 0 && scopeList.HasPage)
                {
                    foreach (EntityPropertyInfo pInfo in CurEntityInfo.PrimaryProperty)
                    {
                        scopeList.OrderBy.Add(pInfo.PropertyName, SortType.ASC);
                    }
                }
                return _cdal.SelectDataSet<T>(scopeList);
            }

            ParamList list = null;

            list = new ParamList();

            string sql = null;
            PageContent objPage = scopeList.PageContent;
            using (BatchAction ba = Oper.StarBatchAction())
            {
                if (objPage == null)//判断是否分页查询
                {
                    sql = GetSelectContant(list, scopeList, GetSelectParams(scopeList)).GetSql(scopeList.UseCache);
                }
                else
                {
                    sql = GetSelectPageContant(list, scopeList);
                }


                DataSet ds = null;
                Dictionary<string, bool> cacheTables = null;
                if (scopeList.UseCache)
                {
                    cacheTables = _oper.DBInfo.QueryCache.CreateMap(CurEntityInfo.TableName);
                }
                ds = _oper.QueryDataSet(sql, list, CommandType.Text,cacheTables);

                return ds;
            }
        }

       

        /// <summary>
        /// 分页查询表(返回List)
        /// </summary>
        /// <param name="scopeList">范围查找的集合</param>
        /// <returns></returns>
        public List<T> SelectList(ScopeList scopeList)
        {
            if (scopeList.HasPage) 
            {
                if (!scopeList.HasSort) 
                {
                    foreach (EntityPropertyInfo pInfo in CurEntityInfo.PrimaryProperty)
                    {
                        scopeList.OrderBy.Add(pInfo.PropertyName, SortType.ASC);
                    }
                }
            }

            if (scopeList.HasInner)
            {
                return _cdal.SelectList<T>(scopeList);
            }

            ParamList list = null;

            list = new ParamList();
            string sql = null;
            using (BatchAction ba = Oper.StarBatchAction())
            {
                if (!scopeList.HasPage)//判断是否分页查询
                {
                    sql = GetSelectContant(list, scopeList, GetSelectParams(scopeList)).GetSql(scopeList.UseCache);
                }
                else
                {

                    sql = GetSelectPageContant(list, scopeList);
                }

                List<T> retlist = null;

                Dictionary<string, bool> cacheTables = null;
                if (scopeList.UseCache)
                {
                    cacheTables = _oper.DBInfo.QueryCache.CreateMap(CurEntityInfo.TableName);
                }
                retlist = QueryList(sql, list, CommandType.Text,cacheTables);
                DataAccessCommon.FillEntityChidList(retlist, scopeList);
                return retlist;
            }
        }
        #endregion
        #region SelectCount
        /// <summary>
        /// 查询符合指定条件的记录条数
        /// </summary>
        /// <param name="scopeList">范围查找的集合</param>
        /// <returns></returns>
        public long SelectCount(ScopeList scopeList)
        {
            if (scopeList.HasInner)
            {
                return _cdal.SelectCount<T>(scopeList);
            }
            ParamList list = null;
                list = new ParamList();

            string sql = GetSelectContant(list,scopeList, "count(*)").GetSql(scopeList.UseCache);
            long count = 0;
            Dictionary<string,bool> cacheTables=null;
            if(scopeList.UseCache)
            {
                cacheTables=_oper.DBInfo.QueryCache.CreateMap(CurEntityInfo.TableName);
            }
            //try
            //{
                using (IDataReader reader = _oper.Query(sql, list,cacheTables))
                {
                    if (reader.Read())
                    {
                        if (!reader.IsDBNull(0))
                        {
                            count = Convert.ToInt64(reader[0]);
                        }
                    }
                }
            return count;
        }

        #endregion

        #region SelectExists
        /// <summary>
        /// 查询是否存在符合条件的记录
        /// </summary>
        /// <param name="scopeList">范围查找的集合</param>
        /// <returns></returns>
        public bool ExistsRecord(ScopeList scopeList)
        {
            if (scopeList.HasInner)
            {
                return _cdal.ExistsRecord<T>(scopeList);
            }
            ParamList list = null;

            SelectCondition sc = GetSelectContant(list, scopeList, CurEntityInfo.DBInfo.CurrentDbAdapter.FormatParam(CurEntityInfo.PrimaryProperty[0].ParamName));
            string sql = CurEntityInfo.DBInfo.CurrentDbAdapter.GetTopSelectSql(sc, 1);
            bool exists = false;
            Dictionary<string, bool> cacheTables = null;
            if (scopeList.UseCache)
            {
                cacheTables = _oper.DBInfo.QueryCache.CreateMap(CurEntityInfo.TableName);
            }
            using (IDataReader reader = _oper.Query(sql, list,cacheTables))
            {
                if (reader.Read())
                {
                    exists = true;
                }
            }
            return exists;
        }

        #endregion

    }
}
