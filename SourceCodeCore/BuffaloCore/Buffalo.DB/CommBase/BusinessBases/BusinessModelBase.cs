using System;
using System.Data;
using System.Data.SqlClient;
using Buffalo.DB;
using System.Collections.Generic;
using Buffalo.DB.DataBaseAdapter;
using Buffalo.DB.CommBase.DataAccessBases;
using Buffalo.DB.DbCommon;
using Buffalo.DB.QueryConditions;
using Buffalo.Kernel.Defaults;
using Buffalo.DB.EntityInfos;
using Buffalo.DB.CacheManager;
using System.Threading.Tasks;

namespace Buffalo.DB.CommBase.BusinessBases
{
    public abstract class BusinessModelBase<T> : BusinessModelBaseForSelect<T>
        where T : EntityBase, new()
    {
        public BusinessModelBase()
		{
			
		}
        /// <summary>
        /// 更新和添加时候判断该对象是否已经存在
        /// </summary>
        /// <param name="entity">对象</param>
        /// <param name="oper">数据链接对象</param>
        /// <returns>没有存在就返回null，否则返回一个值</returns>
        protected virtual object Exists(T entity) 
        {
            return null;
        }
        /// <summary>
        /// 更新和添加时候判断该对象是否已经存在
        /// </summary>
        /// <param name="entity">对象</param>
        /// <param name="oper">数据链接对象</param>
        /// <returns>没有存在就返回null，否则返回一个值</returns>
        protected virtual async Task<object> ExistsAsync(T entity)
        {
            return null;
        }

        /// <summary>
        /// 删除时候判断该对象是否还存在子记录
        /// </summary>
        /// <param name="entity">对象</param>
        /// <param name="oper">数据链接对象</param>
        /// <returns>没有存在就返回null，否则返回一个值</returns>
        protected virtual object HasChild(T entity,ScopeList lstScope) 
        {
            return null;
        }
        /// <summary>
        /// 删除时候判断该对象是否还存在子记录
        /// </summary>
        /// <param name="entity">对象</param>
        /// <param name="oper">数据链接对象</param>
        /// <returns>没有存在就返回null，否则返回一个值</returns>
        protected virtual async Task<object> HasChildAsync(T entity, ScopeList lstScope)
        {
            return null;
        }
       
        /// <summary>
        /// 删除时候判断该对象是否还存在子记录
        /// </summary>
        /// <param name="entity">对象</param>
        /// <param name="oper">数据链接对象</param>
        /// <returns>没有存在就返回null，否则返回一个值</returns>
        protected virtual object HasChildById(object id)
        {
            return null;
        }
        /// <summary>
        /// 删除时候判断该对象是否还存在子记录
        /// </summary>
        /// <param name="entity">对象</param>
        /// <param name="oper">数据链接对象</param>
        /// <returns>没有存在就返回null，否则返回一个值</returns>
        protected virtual async Task<object> HasChildByIdAsync(object id)
        {
            return null;
        }
        /// <summary>
        ///  此次执行的影响行数
        /// </summary>
        protected int _affectedRows=-1;

        /// <summary>
        /// 此次执行的影响行数
        /// </summary>
        public int AffectedRows
        {
            get
            {
                return _affectedRows;
            }
        }





        #region Update
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity">对象</param>
        /// <param name="scorpList">范围更新的列表 不为null时候 entity.主键 条件无效</param>
        /// <param name="lstValue">set实体 此列表的更新值优先于entity</param>
        /// <param name="optimisticConcurrency">并发控制</param>
        /// <returns>null:更新完毕,不为null:更新失败</returns>
        public virtual object Update( ScopeList scorpList = null, ValueSetList lstValue = null, bool optimisticConcurrency = false)
        {
            _affectedRows = 0;
            DataAccessModel<T> entityDao = new DataAccessModel<T>();
            object ret = null;
           
            _affectedRows = entityDao.Update(null, scorpList, lstValue, optimisticConcurrency);
            return ret;

        }
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity">对象</param>
        /// <param name="scorpList">范围更新的列表 不为null时候 entity.主键 条件无效</param>
        /// <param name="lstValue">set实体 此列表的更新值优先于entity</param>
        /// <param name="optimisticConcurrency">并发控制</param>
        /// <returns>null:更新完毕,不为null:更新失败</returns>
        public virtual async Task<object> UpdateAsync(ScopeList scorpList = null, ValueSetList lstValue = null, bool optimisticConcurrency = false)
        {
            _affectedRows = 0;
            DataAccessModel<T> entityDao = new DataAccessModel<T>();
            object ret = null;

            _affectedRows = await entityDao.UpdateAsync(null, scorpList, lstValue, optimisticConcurrency);
            return ret;

        }
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity">对象</param>
        /// <param name="scorpList">范围更新的列表 不为null时候 entity.主键 条件无效</param>
        /// <param name="lstValue">set实体 此列表的更新值优先于entity</param>
        /// <param name="optimisticConcurrency">并发控制</param>
        /// <returns>null:更新完毕,不为null:更新失败</returns>
        public virtual object Update(T entity, ScopeList scorpList=null, ValueSetList lstValue = null, bool optimisticConcurrency=false)
        {
            _affectedRows = 0;
            DataAccessModel<T> entityDao = new DataAccessModel<T>();
            object ret = null;
            if (entity != null)
            {
                ret = Exists(entity);
                if (ret != null)
                {
                    return ret;
                }
            }
            _affectedRows = entityDao.Update(entity, scorpList, lstValue, optimisticConcurrency);
            return ret;

        }
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity">对象</param>
        /// <param name="scorpList">范围更新的列表 不为null时候 entity.主键 条件无效</param>
        /// <param name="lstValue">set实体 此列表的更新值优先于entity</param>
        /// <param name="optimisticConcurrency">并发控制</param>
        /// <returns>null:更新完毕,不为null:更新失败</returns>
        public virtual async Task<object> UpdateAsync(T entity, ScopeList scorpList = null, ValueSetList lstValue = null, bool optimisticConcurrency = false)
        {
            _affectedRows = 0;
            DataAccessModel<T> entityDao = new DataAccessModel<T>();
            object ret = null;
            if (entity != null)
            {
                ret = await ExistsAsync(entity);
                if (ret != null)
                {
                    return ret;
                }
            }

            _affectedRows = await entityDao.UpdateAsync(entity, scorpList, lstValue, optimisticConcurrency);


            return ret;

        }
       
        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="lst">对象列表</param>
        /// <returns>null:更新完毕,不为null:更新失败</returns>
        public virtual object Update(List<T> lst)
        {
            _affectedRows = 0;

            DataAccessModel<T> entityDao = new DataAccessModel<T>();

            object ret = null;
            
            foreach (T entity in lst)
            {
                ret = Exists(entity);
                if (ret != null)
                {
                    continue;
                }

                _affectedRows += entityDao.Update(entity);

            }

            return ret;

        }
        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="lst">对象列表</param>
        /// <returns>null:更新完毕,不为null:更新失败</returns>
        public virtual async Task<object> UpdateAsync(List<T> lst)
        {
            _affectedRows = 0;

            DataAccessModel<T> entityDao = new DataAccessModel<T>();

            object ret = null;

            foreach (T entity in lst)
            {
                ret = ExistsAsync(entity);
                if (ret != null)
                {
                    continue;
                }

                _affectedRows += await entityDao.UpdateAsync(entity,null,null,false);

            }

            return ret;

        }

        #endregion

        #region Insert
        
        /// <summary>
        /// 插入一条数据
        /// </summary>
        /// <param name="entity">对象</param>
        /// <param name="setList">set数值</param>
        /// <param name="fillIdentity">是否填充自增长属性</param>
        /// <returns>null:插入完毕,不为null:插入失败</returns>
        public virtual object Insert(T entity,ValueSetList setList=null, bool fillIdentity = false)
        {
            _affectedRows = 0;
            DataAccessModel<T> entityDao = new DataAccessModel<T>();

            object ret = Exists(entity);
            if (ret != null)
            {
                return ret;
            }

            _affectedRows = entityDao.Insert(entity, setList, fillIdentity);

            return ret;
        }

        /// <summary>
        /// 插入一条数据
        /// </summary>
        /// <param name="entity">对象</param>
        /// <param name="setList">set数值</param>
        /// <param name="fillIdentity">是否填充自增长属性</param>
        /// <returns>null:插入完毕,不为null:插入失败</returns>
        public virtual async Task<object> InsertAsync(T entity, ValueSetList setList = null, bool fillIdentity = false)
        {
            _affectedRows = 0;
            DataAccessModel<T> entityDao = new DataAccessModel<T>();

            object ret = await ExistsAsync(entity);
            if (ret != null)
            {
                return ret;
            }

            _affectedRows = await entityDao.InsertAsync(entity, setList, fillIdentity);

            return ret;
        }

        #endregion

        #region Delete
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entity">对象</param>
        /// <param name="isConcurrency">是否处理并发</param>
        /// <returns>null:删除完毕,不为null:删除失败</returns>
        public virtual object Delete(T entity,bool isConcurrency = false)
        {
            _affectedRows = 0;
            DataAccessModel<T> entityDao = new DataAccessModel<T>();
            object ret = HasChild(entity,null);
            if (ret != null)
            {
                return ret;
            }

            _affectedRows = entityDao.Delete(entity, null, isConcurrency);
            return ret;
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entity">对象</param>
        /// <param name="isConcurrency">是否处理并发</param>
        /// <returns>null:删除完毕,不为null:删除失败</returns>
        public virtual async Task<object> DeleteAsync(T entity, bool isConcurrency = false)
        {
            _affectedRows = 0;
            DataAccessModel<T> entityDao = new DataAccessModel<T>();
            object ret = await HasChildAsync(entity, null);
            if (ret != null)
            {
                return ret;
            }

            _affectedRows = await entityDao.DeleteAsync(entity, null, isConcurrency);
            return ret;
        }

        /// <summary>
        /// 清空表
        /// </summary>

        public void TruncateTable()
        {
            DataAccessModel<T> entityDao = new DataAccessModel<T>();
            _affectedRows = entityDao.TruncateTable();

        }
        /// <summary>
        /// 清空表
        /// </summary>

        public async Task TruncateTableAsync()
        {
            DataAccessModel<T> entityDao = new DataAccessModel<T>();
            _affectedRows = await entityDao.TruncateTableAsync();

        }

        /// <summary>
        /// 根据ID删除记录
        /// </summary>
        /// <param name="id">要删除的记录ID</param>
        /// <returns>null:删除完毕,不为null:删除失败</returns>
        public virtual object DeleteById(object id)
        {
            _affectedRows = 0;
            DataAccessModel<T> entityDao = new DataAccessModel<T>();
            object ret = HasChildById(id);
            if (ret != null)
            {
                return ret;
            }
            _affectedRows = entityDao.DeleteById(id);

            return ret;
        }
        /// <summary>
        /// 根据ID删除记录
        /// </summary>
        /// <param name="id">要删除的记录ID</param>
        /// <returns>null:删除完毕,不为null:删除失败</returns>
        public virtual async Task<object> DeleteByIdAsync(object id)
        {
            _affectedRows = 0;
            DataAccessModel<T> entityDao = new DataAccessModel<T>();
            object ret = await HasChildByIdAsync(id);
            if (ret != null)
            {
                return ret;
            }
            _affectedRows = await entityDao.DeleteByIdAsync(id);

            return ret;
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="lstScope">批量删除的条件集合</param>
        /// <returns>null:删除完毕,不为null:删除失败</returns>
        public virtual object Delete(ScopeList lstScope)
        {
            _affectedRows = 0;
            DataAccessModel<T> entityDao = new DataAccessModel<T>();


            object ret = HasChild(null,lstScope);
            if (ret != null)
            {
                return ret;
            }

            
            _affectedRows = entityDao.Delete(lstScope);
            return ret;
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="lstScope">批量删除的条件集合</param>
        /// <returns>null:删除完毕,不为null:删除失败</returns>
        public virtual async Task<object> DeleteAsync(ScopeList lstScope)
        {
            _affectedRows = 0;
            DataAccessModel<T> entityDao = new DataAccessModel<T>();


            object ret = await HasChildAsync(null, lstScope);
            if (ret != null)
            {
                return ret;
            }


            _affectedRows = await entityDao.DeleteAsync(lstScope);
            return ret;
        }
        /// <summary>
        /// 删除一组数据
        /// </summary>
        /// <param name="lst">数据集合</param>
        /// <returns>大于0:删除完毕,小于0:删除失败</returns>
        public virtual object Delete(List<T> lst)
        {
            _affectedRows = 0;
            DataAccessModel<T> entityDao = new DataAccessModel<T>();
            object res = null;
            
            foreach (T entity in lst)
            {
                res = HasChild(entity, null);
                if (res != null)
                {
                    continue;
                }
                _affectedRows += entityDao.Delete(entity);
            }

            return res;
        }
        /// <summary>
        /// 删除一组数据
        /// </summary>
        /// <param name="lst">数据集合</param>
        /// <returns>大于0:删除完毕,小于0:删除失败</returns>
        public virtual async Task<object> DeleteAsync(List<T> lst)
        {
            _affectedRows = 0;
            DataAccessModel<T> entityDao = new DataAccessModel<T>();
            object res = null;

            foreach (T entity in lst)
            {
                res = await HasChildAsync(entity, null);
                if (res != null)
                {
                    continue;
                }
                _affectedRows += await entityDao.DeleteAsync(entity);
            }

            return res;
        }
        #endregion
    }
}