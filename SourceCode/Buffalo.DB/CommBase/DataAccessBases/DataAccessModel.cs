using System;
using System.Data;
using System.Data.SqlClient;
using Buffalo.DB;
using Buffalo.DB.CommBase;
using System.Collections.Generic;
using Buffalo.DB.QueryConditions;
using Buffalo.DB.DbCommon;

namespace Buffalo.DB.CommBase.DataAccessBases
{
    /// <summary>
    /// 数据访问层模版
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DataAccessModel<T> :DataAccessBaseForSelect<T>, IDataAccessModel<T> where T : EntityBase, new()
    {
        public DataAccessModel(DataBaseOperate oper):base(oper)
        {
            
        }
        public DataAccessModel():base()
        {
            
        }

        /////<summary>
        /////并发修改记录
        /////</summary>
        /////<param name="entity">实体</param>
        /////<returns></returns>
        //public int ConcurrencyUpdate(T entity)
        //{
        //    return base.Update(entity, null, true);
        //}
        
        

        ///<summary>
        ///修改记录
        ///</summary>
        ///<param name="entity">实体</param>
        ///<returns></returns>
        public int Update(T entity)
        {
            return Update(entity, null,null, true);
        }
        ///<summary>
        ///修改记录
        ///</summary>
        ///<param name="entity">实体</param>
        /// <param name="scopeList">范围更新的集合</param>
        ///<returns></returns>
        public int Update(T entity,ScopeList scopeList)
        {
            return Update(entity, scopeList,null,false);
        }

        /// <summary>
        /// 修改记录
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="scopeList">范围更新的集合</param>
        /// <param name="valueList">值集合</param>
        /// <param name="optimisticConcurrency">是否并发</param>
        /// <returns></returns>
        public int Update(T entity, ScopeList scopeList, ValueSetList valueList, bool optimisticConcurrency) 
        {
            return base.Update(entity, scopeList, valueList, optimisticConcurrency);
        }

        /// <summary>
        /// 插入记录
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns></returns>
        public new int Insert(T entity, bool fillIdentity)
        {
            return base.Insert(entity,null, fillIdentity);
        }

        /// <summary>
        /// 删除记录
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="scopeList">范围删除的集合</param>
        /// <returns></returns>
        public int Delete(T entity)
        {
            return base.Delete(entity, null,true);
        }

        ///// <summary>
        ///// 并发删除
        ///// </summary>
        ///// <param name="entity"></param>
        ///// <returns></returns>
        //public int ConcurrencyDelete(T entity)
        //{
        //    return base.Delete(entity, null, true);
        //}
        /// <summary>
        /// 范围删除记录(慎用)
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="scopeList">范围删除的集合</param>
        /// <returns></returns>
        public int Delete( ScopeList scopeList)
        {
            return base.Delete(default(T),scopeList,false);
        }
    }
}
