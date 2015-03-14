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
        /// 删除时候判断该对象是否还存在子记录
        /// </summary>
        /// <param name="entity">对象</param>
        /// <param name="oper">数据链接对象</param>
        /// <returns>没有存在就返回null，否则返回一个值</returns>
        protected virtual object HasChild(ScopeList lstScope) 
        {
            return null;
        }

        /// <summary>
        /// 删除时候判断该对象是否还存在子记录
        /// </summary>
        /// <param name="entity">对象</param>
        /// <param name="oper">数据链接对象</param>
        /// <returns>没有存在就返回null，否则返回一个值</returns>
        protected virtual object HasChild(T entity)
        {
            return null;
        }
        /// <summary>
        /// 删除时候判断该对象是否还存在子记录
        /// </summary>
        /// <param name="entity">对象</param>
        /// <param name="oper">数据链接对象</param>
        /// <returns>没有存在就返回null，否则返回一个值</returns>
        protected virtual object HasChild(object id)
        {
            return null;
        }
        

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
        /// <returns>大于0:更新完毕,小于0:更新失败</returns>
        public object Update(T entity)
        {


           


            return Update(entity,null,null,false);

        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity">对象</param>
        /// <param name="scorpList">范围更新的列表</param>
        /// <returns>大于0:更新完毕,小于0:更新失败</returns>
        public object Update(T entity, ScopeList scorpList)
        {



            return Update(entity,scorpList,null,false);

        }
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity">对象</param>
        /// <param name="scorpList">范围更新的列表</param>
        /// <param name="lstValue">set实体</param>
        /// <param name="optimisticConcurrency">并发控制</param>
        /// <returns></returns>
        public virtual object Update(T entity, ScopeList scorpList, ValueSetList lstValue, bool optimisticConcurrency)
        {
            DataAccessModel<T> entityDao = new DataAccessModel<T>();

            object ret = Exists(entity);
            if (ret != null)
            {
                return ret;
            }

            _affectedRows = entityDao.Update(entity, scorpList, lstValue, optimisticConcurrency);


            return ret;

        }
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity">对象</param>
        /// <param name="scorpList">范围更新的列表</param>
        /// <returns>大于0:更新完毕,小于0:更新失败</returns>
        public object Update(T entity, ScopeList scorpList, ValueSetList lstValue)
        {



            return Update(entity, scorpList, lstValue, false);

        }
        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="entity">对象</param>
        /// <returns></returns>
        public virtual object Update(List<T> lst)
        {


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
        /// <param name="lst">要更新的实体集合</param>
        /// <param name="scorpList"></param>
        /// <returns></returns>
        public virtual object Update(List<T> lst, ScopeList scorpList)
        {


            DataAccessModel<T> entityDao = new DataAccessModel<T>();

            object ret = null;
            foreach (T entity in lst)
            {

                ret = Exists(entity);
                if (ret != null)
                {
                    continue;
                }
                _affectedRows += entityDao.Update(entity, scorpList);
            }

            return ret;
        }
        #endregion

        #region Insert

        

        /// <summary>
        /// 插入一条数据
        /// </summary>
        /// <param name="entity">对象</param>
        /// <returns>大于0:插入完毕,小于0:插入失败</returns>
        public object Insert(T entity)
        {

            return Insert(entity, null, false);
        }

        /// <summary>
        /// 插入一条数据
        /// </summary>
        /// <param name="entity">对象</param>
        /// <returns>大于0:插入完毕,小于0:插入失败</returns>
        public object Insert(T entity,bool fillIdentity)
        {



            return Insert(entity,null,fillIdentity);
        }
        /// 插入一条数据
        /// </summary>
        /// <param name="entity">对象</param>
        /// <returns>大于0:插入完毕,小于0:插入失败</returns>
        public virtual object Insert(T entity,ValueSetList setList, bool fillIdentity)
        {

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
        /// 插入一组数据
        /// </summary>
        /// <param name="lst">对象</param>
        /// <returns>大于0:插入完毕,小于0:插入失败</returns>
        public virtual object Insert(List<T> lst, bool fillIdentity)
        {

            DataAccessModel<T> entityDao = new DataAccessModel<T>();

            object ret = null;
            foreach (T entity in lst)
            {

                ret = Exists(entity);

                if (ret != null)
                {
                    continue;
                }
                _affectedRows += entityDao.Insert(entity, fillIdentity);

            }
            return ret;
        }

        /// <summary>
        /// 插入一组数据
        /// </summary>
        /// <param name="lst">对象</param>
        /// <returns>大于0:插入完毕,小于0:插入失败</returns>
        public virtual object Insert(List<T> lst)
        {
            return Insert(lst, true);
        }
        #endregion

        #region Delete
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entity">对象</param>
        /// <returns></returns>
        public virtual object Delete(T entity,bool isConcurrency)
        {

            DataAccessModel<T> entityDao = new DataAccessModel<T>();
            object ret = HasChild(entity);
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
        /// <returns></returns>
        public object Delete(T entity)
        {
            return Delete(entity, false);
        }
        

        /// <summary>
        /// 根据ID删除记录
        /// </summary>
        /// <param name="id">要删除的记录ID</param>
        /// <returns></returns>
        public virtual object DeleteById(object id)
        {

            DataAccessModel<T> entityDao = new DataAccessModel<T>();
            object ret = HasChild(id);
            if (ret != null)
            {
                return ret;
            }
            _affectedRows = entityDao.DeleteById(id);

            return ret;
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="lstScope">批量删除的条件集合</param>
        /// <returns>大于0:删除完毕,小于0:删除失败</returns>
        public virtual object Delete(ScopeList lstScope)
        {
            return Delete(lstScope, false);
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="lstScope">批量删除的条件集合</param>
        /// <returns>大于0:删除完毕,小于0:删除失败</returns>
        public virtual object Delete(ScopeList lstScope, bool isConcurrency)
        {

            DataAccessModel<T> entityDao = new DataAccessModel<T>();


            object ret = HasChild(lstScope);
            if (ret != null)
            {
                return ret;
            }

            
            _affectedRows = entityDao.Delete(lstScope);
            return ret;
        }

        /// <summary>
        /// 删除一组数据
        /// </summary>
        /// <param name="lst">数据集合</param>
        /// <returns>大于0:删除完毕,小于0:删除失败</returns>
        public virtual object Delete(List<T> lst)
        {

            DataAccessModel<T> entityDao = new DataAccessModel<T>();
            object res = null;
            foreach (T entity in lst)
            {
                res = HasChild(entity);
                if (res != null)
                {
                    continue;
                }
                _affectedRows += entityDao.Delete(entity);


            }

            return res;
        }
        #endregion
    }
}