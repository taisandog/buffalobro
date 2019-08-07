using System;
using System.Data;
using System.Data.SqlClient;
using Buffalo.DB;
using Buffalo.DB.CommBase;
using System.Collections.Generic;
using Buffalo.DB.EntityInfos;
using Buffalo.DB.QueryConditions;
using Buffalo.DB.DbCommon;

namespace Buffalo.DB.CommBase.DataAccessBases
{
    /// <summary>
    /// 只有Select语句的数据访问层模版
    /// </summary>
    public class DataAccessBaseForSelect<T> : DataAccessBase<T>, IViewDataAccess<T> where T : EntityBase, new()
    {
        /// <summary>
        /// 只有Select语句的数据访问层模版
        /// </summary>
        /// <param name="oper"></param>
        public DataAccessBaseForSelect(DataBaseOperate oper)
        {
            Oper = oper;
            
            
            
        }
        /// <summary>
        /// 只有Select语句的数据访问层模版
        /// </summary>
        public DataAccessBaseForSelect()
        {
            Oper = StaticConnection.GetDefaultOperate<T>();
        }
        ///// <summary>
        ///// 根据条件查找实体
        ///// </summary>
        ///// <param name="lstScope">条件</param>
        ///// <returns></returns>
        //public virtual new T GetUnique(ScopeList lstScope)
        //{
        //    return base.GetUnique(lstScope);
        //}
        /// <summary>
        /// 根据主键查找实体
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns></returns>
        public virtual T GetEntityById(object id)
        {
            return base.GetObjectById(id,false);
        }
        
        /// <summary>
        /// 查找
        /// </summary>
        /// <param name="lstScope">范围集合</param>
        /// <returns></returns>
        public virtual new DataSet Select(ScopeList lstScope)
        {
            return base.Select(lstScope);
        }
        /// <summary>
        /// 查询集合
        /// </summary>
        /// <param name="scpoeList">范围查找的集合</param>
        /// <returns></returns>
        public virtual new List<T> SelectList(ScopeList scpoeList)
        {
            return base.SelectList(scpoeList);
        }
       
        /// <summary>
        /// 查询符合指定条件的记录条数
        /// </summary>
        /// <param name="scpoeList">范围查找的集合</param>
        /// <returns></returns>
        public virtual new long SelectCount(ScopeList scpoeList)
        {
            return base.SelectCount(scpoeList);
        }
        
        /// <summary>
        /// 查询符合指定条件的记录条数
        /// </summary>
        /// <param name="scpoeList">范围查找的集合</param>
        /// <returns></returns>
        public virtual new  bool ExistsRecord(ScopeList scpoeList)
        {
            return base.ExistsRecord(scpoeList);
        }
    }
}
