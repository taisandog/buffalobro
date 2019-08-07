using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DB.CommBase.DataAccessBases;
using Buffalo.DB.QueryConditions;
using System.Data;
using Buffalo.DB.BQLCommon;
using Buffalo.Kernel.Defaults;
using Buffalo.DB.EntityInfos;
using Buffalo.DB.DataBaseAdapter;

namespace Buffalo.DB.CommBase.BusinessBases
{
    /// <summary>
    /// 小型架构基类
    /// </summary>
    public class ThinModelBase:EntityBase
    {
        private DataAccessSetBase _dal;

        /// <summary>
        /// 获取基础数据层
        /// </summary>
        /// <returns></returns>
        private DataAccessSetBase GetBaseDataAccess() 
        {
            if (_dal == null) 
            {
                EntityInfoHandle handle=EntityInfoManager.GetEntityHandle(CH.GetRealType(this));
                _dal = new DataAccessSetBase(handle);
                _dal.Oper = StaticConnection.GetStaticOperate(handle.DBInfo);
            }
            return _dal;
        }

        #region 数据修改
        /// <summary>
        /// 保存实体
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int Insert() 
        {
            return Insert(null,true);
        }

        /// <summary>
        /// 查询时候触发
        /// </summary>
        /// <param name="lstScope"></param>
        /// <returns></returns>
        protected internal virtual void OnSelect(ScopeList lstScope) 
        {
            
        }

        /// <summary>
        /// 范围更新前触发
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="scorpList">范围更新条件</param>
        /// <param name="lstValue">赋值</param>
        /// <param name="optimisticConcurrency">是否用并发锁</param>
        /// <returns>是否继续执行更新</returns>
        protected internal virtual bool BeforeUpdateByScope(EntityBase entity, ScopeList scorpList, ValueSetList lstValue, bool optimisticConcurrency)
        {
            return true;
        }


        /// <summary>
        /// 范围更新后触发
        /// </summary>
        /// <param name="affected">影响行数</param>
        /// <param name="entity">实体</param>
        /// <param name="scorpList">范围更新条件</param>
        /// <param name="lstValue">赋值</param>
        /// <param name="optimisticConcurrency">是否用并发锁</param>
        protected internal virtual void AfterUpdateByScope(int affected, EntityBase entity, ScopeList scorpList, ValueSetList lstValue, bool optimisticConcurrency)
        {

        }

        /// <summary>
        /// 范围删除前触发
        /// </summary>
        /// <param name="scorpList">范围更新条件</param>
        /// <returns>是否继续执行更新</returns>
        protected internal virtual bool BeforeDeleteByScope(ScopeList scorpList)
        {
            return true;
        }


        /// <summary>
        /// 范围删除后触发
        /// </summary>
        /// <param name="affected">影响行数</param>
        /// <param name="scorpList">范围更新条件</param>
        protected internal virtual void AfterDeleteByScope(int affected, ScopeList scorpList)
        {

        }

        /// <summary>
        /// 保存实体并填充ID
        /// </summary>
        /// <param name="fillPrimaryKey">是否填充实体</param>
        /// <returns></returns>
        public virtual int Insert(bool fillPrimaryKey)
        {
            return Insert(null, fillPrimaryKey);
        }
        /// <summary>
        /// 保存实体并填充ID
        /// </summary>
        /// <param name="fillPrimaryKey">是否填充实体</param>
        /// <returns></returns>
        public virtual int Insert(ValueSetList setList, bool fillPrimaryKey)
        {
            DataAccessSetBase dal = GetBaseDataAccess();

            return dal.DoInsert(this, setList, fillPrimaryKey);
        }
        /// <summary>
        /// 更新实体
        /// </summary>
        ///  <param name="optimisticConcurrency">是否并发控制</param>
        /// <returns></returns>
        public int Update(bool optimisticConcurrency) 
        {
            return Update(null, optimisticConcurrency);
        }
        /// <summary>
        /// 更新实体
        /// </summary>
        /// <param name="lstValue">强制设置值</param>
        ///  <param name="optimisticConcurrency">是否并发控制</param>
        /// <returns></returns>
        public virtual int Update(ValueSetList lstValue,bool optimisticConcurrency)
        {
            DataAccessSetBase dal = GetBaseDataAccess();
            foreach (EntityPropertyInfo epPk in dal.EntityInfo.PrimaryProperty)
            {
                object id = epPk.GetValue(this);
                if (DefaultType.IsDefaultValue(id))
                {
                    throw new Exception("主键:" + epPk.PropertyName + "的值不能为空");
                }
            }
            return dal.Update(this, null, lstValue, optimisticConcurrency);
        }
        /// <summary>
        /// 更新实体
        /// </summary>
        /// <returns></returns>
        public int Update()
        {
            return Update(null,false);
        }

        /// <summary>
        /// 并发删除
        /// </summary>
        /// <param name="optimisticConcurrency">是否并发控制</param>
        /// <returns></returns>
        public virtual int Delete(bool optimisticConcurrency) 
        {
            DataAccessSetBase dal = GetBaseDataAccess();
            ScopeList lstScope = new ScopeList();
            foreach (EntityPropertyInfo pInfo in dal.EntityInfo.PrimaryProperty)
            {
                object id = pInfo.GetValue(this);
                if (DefaultType.IsDefaultValue(id))
                {
                    throw new Exception("主键:" + pInfo.PropertyName + "的值不能为空");
                }
                lstScope.AddEqual(pInfo.PropertyName, id);
            }
            
            
            return dal.Delete(this, lstScope, optimisticConcurrency);
        }

        /// <summary>
        /// 并发删除
        /// </summary>
        /// <returns></returns>
        public int Delete()
        {
            return Delete(false);
        }


        
        #endregion
    }
}
