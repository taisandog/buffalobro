using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DB.BQLCommon.BQLKeyWordCommon;
using Buffalo.DB.BQLCommon.BQLConditionCommon;
using Buffalo.DB.QueryConditions;
using Buffalo.DB.BQLCommon.BQLBaseFunction;
using Buffalo.DB.BQLCommon.BQLExtendFunction;
using Buffalo.DB.DataBaseAdapter;
using Buffalo.DB.DbCommon;
using System.Data;
using Buffalo.DB.DataFillers;
using Buffalo.DB.BQLCommon.BQLConditions;
using Buffalo.DB.EntityInfos;
using Buffalo.DB.DataBaseAdapter.IDbAdapters;
using Buffalo.DB.BQLCommon.BQLAggregateFunctions;
using Buffalo.Kernel.Defaults;
using Buffalo.DB.CommBase;
using Buffalo.DB.CommBase.DataAccessBases.AliasTableMappingManagers;

namespace Buffalo.DB.BQLCommon
{
    /// <summary>
    /// BQL数据访问层的基类
    /// </summary>
    public class BQLDataAccessBase<T> : BQLDbBase
        where T : EntityBase, new()
    {
        /// <summary>
        /// 数据层基类
        /// </summary>
        public BQLDataAccessBase()
            : base(typeof(T))
        {
        }
        /// <summary>
        /// 数据层基类
        /// </summary>
        /// <param name="oper"></param>
        public BQLDataAccessBase(DataBaseOperate oper)
            : base(oper)
        {

        }
        
        /// <summary>
        /// 获取第一条记录
        /// </summary>
        /// <param name="BQL"></param>
        /// <returns></returns>
        public virtual T GetUnique(BQLQuery BQL,bool useCache) 
        {
            return this.GetUnique<T>(BQL,useCache);
        }
        /// <summary>
        /// 获取第一条记录
        /// </summary>
        /// <param name="BQL"></param>
        /// <returns></returns>
        public T GetUnique(BQLQuery BQL) 
        {
            return this.GetUnique<T>(BQL,false);
        }
        /// <summary>
        /// 执行sql语句，分页返回List
        /// </summary>
        /// <param name="BQL">BQL</param>
        /// <param name="objPage">分页数据</param>
        /// <returns></returns>
        public virtual List<T> QueryPageList(BQLQuery BQL, PageContent objPage,bool useCache)
        {
            return QueryPageList<T>(BQL, objPage, null, useCache);
        }
        /// <summary>
        /// 执行sql语句，返回List
        /// </summary>
        /// <typeparam name="E">实体类型</typeparam>
        /// <param name="BQL">BQL</param>
        /// <returns></returns>
        public virtual List<T> QueryList(BQLQuery BQL)
        {
            return QueryList<T>(BQL, null,false);
        }

        
    }
}
