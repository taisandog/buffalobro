using Buffalo.DB.QueryConditions;
using System;
using System.Threading.Tasks;
namespace Buffalo.DB.CommBase.DataAccessBases
{

    public interface IDataAccessModel<T> : IViewDataAccess<T> 
     where T :Buffalo.DB.CommBase.EntityBase, new()
    {
        /// <summary>
        /// 删除记录
        /// </summary>
        /// <param name="obj">实体</param>
        /// <param name="scopeList">条件</param>
        /// <param name="isConcurrency">是否并发</param>
        /// <returns></returns>
        int Delete(EntityBase obj, ScopeList scopeList = null, bool isConcurrency = false);
        /// <summary>
        /// 删除记录
        /// </summary>
        /// <param name="obj">实体</param>
        /// <param name="scopeList">条件</param>
        /// <param name="isConcurrency">是否并发</param>
        /// <returns></returns>
        Task<int> DeleteAsync(EntityBase obj, ScopeList scopeList = null, bool isConcurrency = false);
        /// <summary>
        /// 删除记录
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="scopeList">范围删除的集合</param>
        /// <returns></returns>
        int Delete(Buffalo.DB.QueryConditions.ScopeList scopeList);
        /// <summary>
        /// 删除记录
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="scopeList">范围删除的集合</param>
        /// <returns></returns>
        Task<int> DeleteAsync(Buffalo.DB.QueryConditions.ScopeList scopeList);
        /// <summary>
        /// 根据ID删除记录
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns></returns>
        int DeleteById(object id);
        /// <summary>
        /// 根据ID删除记录
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns></returns>
        Task<int> DeleteByIdAsync(object id);
        /// <summary>
        /// 插入记录
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns></returns>
        int Insert(T obj, ValueSetList setList = null, bool fillIdentity = false);
        /// <summary>
        /// 插入记录
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns></returns>
        Task<int> InsertAsync(T obj, ValueSetList setList = null, bool fillIdentity = false);
        ///<summary>
        ///修改记录
        ///</summary>
        ///<param name="entity">实体</param>
        /// <param name="scopeList">范围更新的集合</param>
        /// <param name="optimisticConcurrency">是否进行并发控制</param>
        ///<returns></returns>
        int Update(EntityBase entity, ScopeList scopeList = null, ValueSetList setList = null, bool optimisticConcurrency = false);
        ///<summary>
        ///修改记录
        ///</summary>
        ///<param name="entity">实体</param>
        /// <param name="scopeList">范围更新的集合</param>
        /// <param name="optimisticConcurrency">是否进行并发控制</param>
        ///<returns></returns>
        Task<int> UpdateAsync(EntityBase entity, ScopeList scopeList = null, ValueSetList setList = null, bool optimisticConcurrency = false);
    }
}
