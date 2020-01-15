using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DB.BQLCommon.BQLBaseFunction;
using Buffalo.DB.BQLCommon.BQLConditionCommon;
using Buffalo.DB.QueryConditions;
using System.Data;
using Buffalo.DB.EntityInfos;
using Buffalo.DB.BQLCommon.BQLConditions;
using Buffalo.DB.BQLCommon.BQLKeyWordCommon;
using Buffalo.DB.CommBase.DataAccessBases.AliasTableMappingManagers;
using Buffalo.DB.DbCommon;
using Buffalo.DB.CommBase;
using Buffalo.DB.DataBaseAdapter;
using Buffalo.Kernel;
using Buffalo.DB.CommBase.BusinessBases;
using Buffalo.DB.CommBase.DataAccessBases;

namespace Buffalo.DB.BQLCommon
{
    /// <summary>
    /// 数据操作基类
    /// </summary>
    public class BQLDbBase
    {
        private DataBaseOperate _oper;

        /// <summary>
        /// 数据层基类
        /// </summary>
        ///  <param name="info">数据库信息</param>
        public BQLDbBase(DBInfo info)
        {
            this._oper = info.DefaultOperate;
        }

        /// <summary>
        /// 数据层基类
        /// </summary>
        /// <param name="entityType">关联实体</param>
        public BQLDbBase(Type entityType) 
            :this(EntityInfoManager.GetEntityHandle(entityType).DBInfo)
        {

        }

        /// <summary>
        /// 数据层基类
        /// </summary>
        /// <param name="oper"></param>
        public BQLDbBase(DataBaseOperate oper) 
        {
            this._oper = oper;
            
        }


        /// <summary>
        /// 直接查询数据库视图
        /// </summary>
        /// <param name="tableName">表名称</param>
        /// <param name="lstScope">条件</param>
        /// <param name="vParams">字段列表</param>
        /// <returns></returns>
        public virtual DataSet SelectTable(string tableName, ScopeList lstScope, Type entityType)
        {
            return SelectTable(BQL.ToTable(tableName), lstScope, entityType);
        }


        /// <summary>
        /// 查询总条数
        /// </summary>
        /// <param name="lstScope"></param>
        /// <returns></returns>
        public virtual long SelectCount<E>(ScopeList lstScope) 
        {
            long ret = 0;
            Type eType = typeof(E);
            TableAliasNameManager aliasManager = new TableAliasNameManager(new BQLEntityTableHandle(EntityInfoManager.GetEntityHandle(typeof(E))));
            BQLEntityTableHandle table = _oper.DBInfo.FindTable(eType);
            if (CommonMethods.IsNull(table))
            {
                _oper.DBInfo.ThrowNotFondTable(eType);
            }

            BQLCondition where = BQLCondition.TrueValue;
            where = FillCondition(where, table, lstScope);
            BQLQuery bql = BQL.Select(BQL.Count())
           .From(table)
           .Where(where);

            //if(lstScope.GroupBy

            AbsCondition con = BQLKeyWordManager.ToCondition(bql, _oper.DBInfo, aliasManager, true);
            Dictionary<string, bool> cacheTables = null;
            if (lstScope.UseCache)
            {
                cacheTables = con.CacheTables;
                
            }
            using (IDataReader reader = _oper.Query(con.GetSql(lstScope.UseCache), con.DbParamList, cacheTables)) 
            {
                if (reader.Read()) 
                {
                    ret = Convert.ToInt64(reader[0]);
                }
            }
            return ret;
        }

        

        /// <summary>
        /// 查询表
        /// </summary>
        /// <typeparam name="E"></typeparam>
        /// <param name="lstScope">条件</param>
        /// <returns></returns>
        public List<E> SelectList<E>(ScopeList lstScope)
            where E : EntityBase, new()
        {
            Type eType = typeof(E);
            List<E> retlist = null;
            BQLEntityTableHandle table = _oper.DBInfo.FindTable(eType);
            if (CommonMethods.IsNull(table)) 
            {
                _oper.DBInfo.ThrowNotFondTable(eType);
            }
            BQLQuery BQL = GetSelectSql(lstScope, table);
            if (!lstScope.HasPage)
            {
                retlist = QueryList<E>(BQL, lstScope.ShowEntity, lstScope.UseCache);
                DataAccessCommon.FillEntityChidList(retlist, lstScope);
                return retlist;
            }
            using (BatchAction ba = _oper.StarBatchAction())
            {
                retlist = QueryPageList<E>(BQL, lstScope.PageContent, lstScope.ShowEntity, lstScope.UseCache);
                DataAccessCommon.FillEntityChidList(retlist, lstScope);
                return retlist;
            }
        }

        /// <summary>
        /// 获取要显示的字段
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="propertys"></param>
        /// <returns></returns>
        private List<BQLParamHandle> GetParam(BQLTableHandle handle, ScopeList lstScope) 
        {
            List<BQLParamHandle> lstParams = lstScope.GetShowProperty(handle);

            
            return lstParams;
        }

        /// <summary>
        /// 直接查询数据库视图
        /// </summary>
        /// <param name="table">表</param>
        /// <param name="lstScope">条件</param>
        /// <param name="vParams">字段列表</param>
        /// <param name="lstSort">排序类型</param>
        /// <param name="objPage">分页对象</param>
        /// <returns></returns>
        public DataSet SelectTable(BQLOtherTableHandle table, ScopeList lstScope,Type entityType)
        {
            List<BQLParamHandle> lstParams = GetParam(table, lstScope);

            List<BQLParamHandle> lstOrders = new List<BQLParamHandle>();
            BQLParamHandle order = null;
            foreach (Sort objSort in lstScope.OrderBy)
            {
                order = table[objSort.PropertyName];
                if (objSort.SortType == SortType.ASC)
                {
                    order = order.ASC;
                }
                else
                {
                    order = order.DESC;
                }
                lstOrders.Add(order);
            }

            BQLCondition where = BQLCondition.TrueValue;
            where = FillCondition(where, table, lstScope, entityType);

            BQLQuery bql = BQL.Select(lstParams.ToArray()).From(table).Where(where).OrderBy(lstOrders.ToArray());

            if (lstScope.HasPage)
            {
                using (BatchAction ba = _oper.StarBatchAction())
                {
                    return QueryDataSet(bql, null, lstScope.PageContent, lstScope.UseCache);
                }
            }
            return QueryDataSet(bql, null,lstScope.UseCache);
        }
        /// <summary>
        /// 转成条件信息
        /// </summary>
        /// <param name="BQL"></param>
        /// <param name="db"></param>
        /// <param name="aliasManager"></param>
        /// <returns></returns>
        private AbsCondition ToCondition(BQLQuery BQL, IEnumerable<BQLEntityTableHandle> outPutTables,
            bool isPutPropertyName, Type entityType)
        {
            TableAliasNameManager aliasManager = null;
            if (entityType != null) 
            {
                aliasManager = new TableAliasNameManager(new BQLEntityTableHandle(EntityInfoManager.GetEntityHandle(entityType)));
            }
            if (outPutTables != null)
            {
                FillOutPutTables(outPutTables, aliasManager);
            }
            AbsCondition con = BQLKeyWordManager.ToCondition(BQL, _oper.DBInfo, aliasManager, isPutPropertyName);
            return con;
        }
        /// <summary>
        /// 执行sql语句，分页返回List
        /// </summary>
        /// <typeparam name="E">实体类型</typeparam>
        /// <param name="BQL">BQL</param>
        /// <param name="objPage">分页数据</param>
        /// <param name="outPutTables">输出表</param>
        /// <returns></returns>
        public List<E> QueryPageList<E>(BQLQuery BQL, PageContent objPage,
            IEnumerable<BQLEntityTableHandle> outPutTables,bool useCache)
            where E : EntityBase, new()
        {
            AbsCondition con = ToCondition(BQL, outPutTables,false,typeof(E));
            con.PageContent = objPage;

            
            

            List<E> retlist = null;
            IDataReader reader = null;
            try
            {
                Dictionary<string, bool> cacheTables = null;
                if (useCache)
                {
                    cacheTables = con.CacheTables;
                }

                if (con.DbParamList != null)
                {
                    con.PageContent = objPage;
                    con.Oper = _oper;
                    string sql = con.GetSql(useCache);

                    reader = _oper.Query(sql, con.DbParamList, cacheTables);
                    
                }
                else
                {
                    SelectCondition sCon = con as SelectCondition;
                    reader = con.DBinfo.CurrentDbAdapter.Query(sCon.GetSelect(), objPage, _oper);
                }
                retlist = LoadFromReader<E>(con.AliasManager, reader);
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
            return retlist;
        }
        /// <summary>
        /// 执行sql语句，返回List
        /// </summary>
        /// <typeparam name="E">实体类型</typeparam>
        /// <param name="BQL">BQL</param>
        /// <returns></returns>
        public List<E> QueryList<E>(BQLQuery BQL, IEnumerable<BQLEntityTableHandle> outPutTables, bool useCache)
            where E : EntityBase, new()
        {
            AbsCondition con = ToCondition(BQL, outPutTables, false, typeof(E));
            List<E> retlist = null;
            using (BatchAction ba = _oper.StarBatchAction())
            {
                IDataReader reader = null;
                try
                {
                    con.Oper = _oper;
                    Dictionary<string, bool> cacheTables = null;
                    if (useCache)
                    {
                        cacheTables = con.CacheTables;
                    }
                    if (con.DbParamList != null)
                    {
                        reader = _oper.Query(con.GetSql(useCache), con.DbParamList, cacheTables);
                    }
                    else
                    {
                        SelectCondition sCon = con as SelectCondition;
                        reader = con.DBinfo.CurrentDbAdapter.Query(sCon.GetSelect(), con.PageContent, _oper);
                    }
                    retlist = LoadFromReader<E>(con.AliasManager, reader);
                }

                finally
                {
                    if (reader != null)
                    {
                        reader.Close();
                    }
                }
            }
            return retlist;
        }
        /// <summary>
        /// 填充要输出的表
        /// </summary>
        /// <param name="outPutTables"></param>
        /// <param name="aliasManager"></param>
        private void FillOutPutTables(IEnumerable<BQLEntityTableHandle> outPutTables, TableAliasNameManager aliasManager)
        {
            if (outPutTables == null) 
            {
                return;
            }
            foreach (BQLEntityTableHandle table in outPutTables)
            {
                aliasManager.AddChildTable(table);
            }
        }
        /// <summary>
        /// 加载数据
        /// </summary>
        /// <typeparam name="E"></typeparam>
        /// <param name="aliasManager"></param>
        /// <param name="reader"></param>
        /// <returns></returns>
        private List<E> LoadFromReader<E>(TableAliasNameManager aliasManager, IDataReader reader)
            where E:EntityBase
        {
            List<E> lst = new List<E>();
            if (reader != null && !reader.IsClosed)
            {
                aliasManager.InitMapping(reader);
                while (reader.Read())
                {

                    object value = aliasManager.LoadFromReader(reader);
                    if (value != null)
                    {
                        E obj = value as E;
                        //obj.SetBaseList(lst);
                        lst.Add(obj);
                    }
                }
            }
            return lst;
        }

        /// <summary>
        /// 获取范围表对应的BQL
        /// </summary>
        /// <param name="lstScope"></param>
        /// <param name="table"></param>
        /// <returns></returns>
        private BQLQuery GetSelectSql(ScopeList lstScope, BQLEntityTableHandle table) 
        {

            List<BQLParamHandle> lstParams = GetParam(table, lstScope);
            BQLCondition where = BQLCondition.TrueValue;
            where = FillCondition(where, table, lstScope);
            BQLQuery bql = BQL.Select(lstParams.ToArray())
           .From(table)
           .Where(where);

            if (lstScope.GroupBy.Count > 0)
            {
                bql = new KeyWordGroupByItem(lstScope.GroupBy, bql);
            }
            if (lstScope.OrderBy != null && lstScope.OrderBy.Count > 0)
            {
                bql = new KeyWordOrderByItem(GetSort(lstScope.OrderBy, table), bql);
            }

            return bql;
        }

        /// <summary>
        /// 查询并返回DataSet
        /// </summary>
        /// <typeparam name="E"></typeparam>
        /// <param name="lstScope">条件集合</param>
        /// <returns></returns>
        public DataSet SelectDataSet<E>(ScopeList lstScope) 
        {
            Type eType = typeof(E);
            BQLEntityTableHandle table = _oper.DBInfo.FindTable(eType);
            if (CommonMethods.IsNull(table))
            {
                _oper.DBInfo.ThrowNotFondTable(eType);
            }
           // List<BQLParamHandle> lstParams = GetParam(table, lstScope);
           // BQLCondition where = BQLCondition.TrueValue;
           // where = FillCondition(where, table, lstScope);
           // BQLQuery BQL = BQL.Select(lstParams.ToArray())
           //.From(table)
           //.Where(where);

           // if (lstScope.GroupBy.Count > 0) 
           // {
           //     BQL = new KeyWordGroupByItem(lstScope.GroupBy, BQL);
           // }
           // if (lstScope.OrderBy != null && lstScope.OrderBy.Count > 0) 
           // {
           //     BQL = new KeyWordOrderByItem(GetSort(lstScope.OrderBy, table), BQL);
           // }
            BQLQuery BQL = GetSelectSql(lstScope, table);
           //.OrderBy(GetSort(lstScope.OrderBy, table));
            if (!lstScope.HasPage)
            {
                return QueryDataSet<E>(BQL,lstScope.UseCache);
            }
            using (BatchAction ba = _oper.StarBatchAction())
            {
                return QueryDataSet<E>(BQL, lstScope.PageContent, lstScope.UseCache);
            }
        }


        /// <summary>
        /// 执行sql语句，分页返回DataSet
        /// </summary>
        /// <param name="BQL">sql语句</param>
        public DataSet QueryDataSet(BQLQuery BQL,Type tableType,bool useCache)
        {
            AbsCondition con = ToCondition(BQL, null, true, tableType);
            DataSet ds = null;

            con.Oper = _oper;
            Dictionary<string, bool> cacheTables = null;
            if (useCache)
            {
                cacheTables = con.CacheTables;

            }
            if (con.DbParamList != null)
            {
                ds = _oper.QueryDataSet(con.GetSql(useCache), con.DbParamList, cacheTables);
            }
            else
            {
                SelectCondition sCon = con as SelectCondition;
                DataTable dt = con.DBinfo.CurrentDbAdapter.QueryDataTable(sCon.GetSelect(), sCon.PageContent, _oper, null);
                dt.TableName = "newTable";
                ds = new DataSet();
                ds.Tables.Add(dt);
            }

            return ds;
        }
        /// <summary>
        /// 执行sql语句，分页返回DataSet
        /// </summary>
        /// <param name="BQL">sql语句</param>
        public DataSet QueryDataSet<E>(BQLQuery BQL,bool useCache)
        {
            AbsCondition con = ToCondition(BQL, null, true, typeof(E));
            DataSet ds = null;
            Dictionary<string, bool> cacheTables = null;
            if (useCache)
            {
                cacheTables = con.CacheTables;

            }
            con.Oper = _oper;
            if (con.DbParamList != null)
            {
                ds = _oper.QueryDataSet(con.GetSql(useCache), con.DbParamList, cacheTables);
            }
            else
            {
                SelectCondition sCon = con as SelectCondition;
                DataTable dt = con.DBinfo.CurrentDbAdapter.QueryDataTable(sCon.GetSelect(), sCon.PageContent, _oper, null);
                dt.TableName = "newTable";
                ds = new DataSet();
                ds.Tables.Add(dt);
            }

            return ds;
        }
        /// <summary>
        /// 执行sql语句，分页返回DataSet
        /// </summary>
        /// <param name="BQL">sql语句</param>
        /// <param name="objPage">分页对象</param>
        public DataSet QueryDataSet<E>(BQLQuery BQL, PageContent objPage,bool useCache)
        {

            AbsCondition con = ToCondition(BQL,null, true, typeof(E));
            DataSet ds = null;
            Dictionary<string, bool> cacheTables = null;
            if (useCache)
            {
                cacheTables = con.CacheTables;

            }
            using (BatchAction ba = _oper.StarBatchAction())
            {
                if (con.DbParamList != null)
                {
                    con.PageContent = objPage;
                    con.Oper = _oper;
                    string sql = con.GetSql(useCache);
                    ds = _oper.QueryDataSet(sql, con.DbParamList,cacheTables);
                }
                else
                {
                    SelectCondition sCon = con as SelectCondition;
                    DataTable dt = con.DBinfo.CurrentDbAdapter.QueryDataTable(sCon.GetSelect(), objPage, _oper, null);
                    dt.TableName = "newTable";
                    ds = new DataSet();
                    ds.Tables.Add(dt);
                }
            }
            return ds;
        }

        /// <summary>
        /// 执行sql语句，分页返回DataSet
        /// </summary>
        /// <param name="BQL">sql语句</param>
        /// <param name="objPage">分页对象</param>
        public DataSet QueryDataSet(BQLQuery bql, Type tableType, PageContent objPage, bool useCache)
        {
            AbsCondition con = ToCondition(bql, null, true, tableType);
            Dictionary<string, bool> cacheTables = null;
            if (useCache)
            {
                cacheTables = con.CacheTables;

            }
            DataSet ds = null;
            using (BatchAction ba = _oper.StarBatchAction())
            {
                if (con.DbParamList != null)
                {
                    con.PageContent = objPage;
                    con.Oper = _oper;
                    string sql = con.GetSql(useCache);
                    ds = _oper.QueryDataSet(sql, con.DbParamList,cacheTables);
                }
                else
                {
                    SelectCondition sCon = con as SelectCondition;
                    DataTable dt = con.DBinfo.CurrentDbAdapter.QueryDataTable(sCon.GetSelect(), objPage, _oper, null);
                    dt.TableName = "newTable";
                    ds = new DataSet();
                    ds.Tables.Add(dt);
                }
            }
            return ds;
        }

        /// <summary>
        /// 执行sql语句，分页返回Reader
        /// </summary>
        /// <param name="BQL">sql语句</param>
        /// <param name="objPage">分页对象</param>
        /// <param name="tableType">表对应的实体类型</param>
        public IDataReader QueryReader(ScopeList lstScope, PageContent objPage,Type tableType)
        {
            BQLEntityTableHandle table = _oper.DBInfo.FindTable(tableType);
            if (CommonMethods.IsNull(table))
            {
                _oper.DBInfo.ThrowNotFondTable(tableType);
            }
            
            BQLQuery BQL = GetSelectSql(lstScope, table);
            return QueryReader(BQL, objPage, tableType, lstScope.UseCache);
        }

        /// <summary>
        /// 执行sql语句，分页返回Reader
        /// </summary>
        /// <param name="BQL">sql语句</param>
        /// <param name="objPage">分页对象</param>
        /// <param name="tableType">表对应的实体类型</param>
        public IDataReader QueryReader(BQLQuery BQL, PageContent objPage, Type tableType,bool useCache)
        {
            AbsCondition con = null;
            if (tableType == null)
            {
                con = BQLKeyWordManager.ToCondition(BQL, _oper.DBInfo, null, true);

            }
            else 
            {
                con = ToCondition(BQL, new BQLEntityTableHandle[] { },true,tableType);
            }
            Dictionary<string, bool> cacheTables = null;
            if (useCache)
            {
                cacheTables = con.CacheTables;

            }
            con.PageContent = objPage;
            IDataReader reader = null;

            con.PageContent = objPage;
            con.Oper = _oper;
            string sql = con.GetSql(useCache);
            reader = _oper.Query(sql, con.DbParamList,cacheTables);

            return reader;
        }
        /// <summary>
        /// 执行sql语句，返回Reader
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="objPage">分页对象</param>
        public IDataReader QueryReader(ScopeList lstScope, Type tableType)
        {
            BQLEntityTableHandle table = _oper.DBInfo.FindTable(tableType);
            if (CommonMethods.IsNull(table))
            {
                _oper.DBInfo.ThrowNotFondTable(tableType);
            }
            
            BQLQuery BQL = GetSelectSql(lstScope, table);
            return QueryReader(BQL, null, lstScope.UseCache);
        }

        /// <summary>
        /// 执行sql语句，返回Reader
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="objPage">分页对象</param>
        public IDataReader QueryReader(BQLQuery BQL, Type tableType,bool useCache)
        {
           
             AbsCondition con = ToCondition(BQL, null, true, tableType);
             Dictionary<string, bool> cacheTables = null;
             if (useCache)
             {
                 cacheTables = con.CacheTables;

             }
            IDataReader reader = null;
            con.Oper = _oper;
            reader = _oper.Query(con.GetSql(useCache), con.DbParamList, cacheTables);

            return reader;
        }


        /// <summary>
        /// 执行Sql命令
        /// </summary>
        /// <param name="BQL">sql语句</param>
        public int ExecuteCommand(BQLQuery BQL)
        {
            AbsCondition con = BQLKeyWordManager.ToCondition(BQL, _oper.DBInfo, null, true);
            Dictionary<string, bool> cacheTables = null;

                cacheTables = con.CacheTables;

            
            int ret = -1;
            con.Oper = _oper;
            ret = _oper.Execute(con.GetSql(true), con.DbParamList,cacheTables);
            return ret;
        }
        
        /// <summary>
        /// 查询是否存在符合条件的记录
        /// </summary>
        /// <param name="BQL">sql语句</param>
        /// <returns></returns>
        public bool ExistsRecord<E>(ScopeList lstScope)
            where E : EntityBase, new() 
        {
            Type eType = typeof(E);
            BQLEntityTableHandle table = _oper.DBInfo.FindTable(eType);
            if (CommonMethods.IsNull(table))
            {
                _oper.DBInfo.ThrowNotFondTable(eType);
            }
            List<BQLParamHandle> lstParams = new List<BQLParamHandle>();
            if (table.GetEntityInfo().PrimaryProperty.Count<=0)
            {
                throw new MissingPrimaryKeyException("找不到：" + eType.FullName + "的关联主键");
            }

            lstParams.Add(table[table.GetEntityInfo().PrimaryProperty[0].PropertyName]);
                
            BQLCondition where = BQLCondition.TrueValue;
            where = FillCondition(where, table, lstScope);
            BQLQuery bql = BQL.Select(lstParams.ToArray())
           .From(table)
           .Where(where)
           .OrderBy(GetSort(lstScope.OrderBy, table));
            return ExistsRecord<E>(bql,lstScope.UseCache);
        }

        /// <summary>
        /// 查询是否存在符合条件的记录
        /// </summary>
        /// <param name="BQL">sql语句</param>
        /// <returns></returns>
        public bool ExistsRecord<E>(BQLQuery BQL,bool useCache)
            where E : EntityBase, new()
        {
            Type tableType = typeof(E);
            AbsCondition con = ToCondition(BQL, null, true, tableType);
            string sql = con.DBinfo.CurrentDbAdapter.GetTopSelectSql(con as SelectCondition, 1);
            bool exists = false;
            IDataReader reader = null;
            Dictionary<string, bool> cacheTables = null;
            if (useCache)
            {
                cacheTables = con.CacheTables;

            }
            try
            {
                con.Oper = _oper;
                reader = _oper.Query(sql, con.DbParamList,cacheTables);
                exists = reader.Read();
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
            return exists;
        }


        /// <summary>
        /// 查询表
        /// </summary>
        /// <typeparam name="E"></typeparam>
        /// <param name="lstScope">条件</param>
        /// <returns></returns>
        public E GetUnique<E>(ScopeList lstScope)
            where E : EntityBase, new()
        {
            Type eType = typeof(E);
            BQLEntityTableHandle table = _oper.DBInfo.FindTable(eType);
            if (CommonMethods.IsNull(table))
            {
                _oper.DBInfo.ThrowNotFondTable(eType);
            }
            List<BQLParamHandle> lstParams = GetParam(table, lstScope);
            BQLCondition where = BQLCondition.TrueValue;
            where = FillCondition(where, table, lstScope);
            BQLQuery bql = BQL.Select(lstParams.ToArray())
           .From(table)
           .Where(where)
           .OrderBy(GetSort(lstScope.OrderBy, table));
            return GetUnique<E>(bql,lstScope.UseCache);
        }

        /// <summary>
        /// 获取第一条记录
        /// </summary>
        /// <typeparam name="E"></typeparam>
        /// <param name="BQL"></param>
        /// <returns></returns>
        public E GetUnique<E>(BQLQuery BQL,bool useCache)
            where E : EntityBase, new()
        {
            Type tableType = typeof(E);
            TableAliasNameManager aliasManager = new TableAliasNameManager(new BQLEntityTableHandle(EntityInfoManager.GetEntityHandle(tableType)));
            

            AbsCondition con = BQLKeyWordManager.ToCondition(BQL, _oper.DBInfo, aliasManager, true);
            Dictionary<string, bool> cacheTables = null;
            if (useCache)
            {
                cacheTables = con.CacheTables;

            }
            string sql = con.DBinfo.CurrentDbAdapter.GetTopSelectSql(con as SelectCondition, 1);
            E ret = default(E);
            IDataReader reader = _oper.Query(sql, con.DbParamList,cacheTables);
            try
            {
                con.Oper = _oper;
                bool hasValue=true;
                aliasManager.InitMapping(reader);
                if (reader.Read())
                {
                    ret = aliasManager.LoadFromReader(reader) as E;
                }
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
            return ret;
        }

        /// <summary>
        /// 填充信息
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="table"></param>
        /// <param name="lstScope"></param>
        /// <param name="entityType"></param>
        public BQLCondition FillCondition(BQLCondition condition, BQLTableHandle table, ScopeList lstScope, Type entityType)
        {
            BQLCondition ret;
            EntityInfoHandle entityInfo = null;
            if (entityType != null)
            {
                entityInfo = EntityInfoManager.GetEntityHandle(entityType);
            }
            ret = BQLConditionScope.FillCondition(condition, table, lstScope, entityInfo);
            return ret;
        }
        /// <summary>
        /// 填充信息
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="table"></param>
        /// <param name="lstScope"></param>
        /// <param name="entityType"></param>
        public BQLCondition FillCondition(BQLCondition condition, BQLEntityTableHandle table, ScopeList lstScope)
        {
            return BQLConditionScope.FillCondition(condition, table, lstScope, table.GetEntityInfo());
        }

        /// <summary>
        /// 获取排序
        /// </summary>
        /// <param name="lstScort"></param>
        /// <param name="table"></param>
        /// <param name="entityType"></param>
        /// <returns></returns>
        protected BQLParamHandle[] GetSort(SortList lstScort, BQLTableHandle table, Type entityType)
        {
            EntityInfoHandle entityInfo = null;
            if (entityType != null)
            {
                entityInfo = EntityInfoManager.GetEntityHandle(entityType);
            }
            return BQLConditionScope.GetSort(lstScort, table, entityInfo);
        }
        /// <summary>
        /// 转换排序信息
        /// </summary>
        /// <param name="lstScort"></param>
        /// <param name="table"></param>
        /// <returns></returns>
        protected BQLParamHandle[] GetSort(SortList lstScort, BQLEntityTableHandle table)
        {
            return BQLConditionScope.GetSort(lstScort, table, table.GetEntityInfo());
        }

        /// <summary>
        /// 填充GroupBy
        /// </summary>
        /// <param name="BQL"></param>
        /// <param name="groupBy"></param>
        /// <returns></returns>
        protected static BQLQuery FillGroupBy(BQLQuery BQL, ScopePropertyCollection groupBy)
        {
            if (groupBy == null || groupBy.Count > 0) 
            {
                return BQL;
            }

            return new KeyWordGroupByItem(groupBy.ToArray(), BQL);
            
        }

    }
}
