using System;
using System.Data;
using System.Data.SqlClient;
using Buffalo.DB;
using Buffalo.DB.CommBase;
using System.Collections.Generic;
using Buffalo.DB.QueryConditions;
using Buffalo.DB.DbCommon;
using System.Threading.Tasks;

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
        /// <summary>
        /// 范围删除记录(慎用)
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="scopeList">范围删除的集合</param>
        /// <returns></returns>
        public Task<int> DeleteAsync(ScopeList scopeList)
        {
            return base.DeleteAsync(default(T), scopeList, false);
        }
    }
}
