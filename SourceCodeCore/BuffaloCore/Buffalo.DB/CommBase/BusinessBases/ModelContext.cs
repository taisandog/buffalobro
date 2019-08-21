using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DB.CommBase.DataAccessBases;
using Buffalo.DB.BQLCommon;
using Buffalo.DB.QueryConditions;
using System.Data;
using Buffalo.DB.EntityInfos;

namespace Buffalo.DB.CommBase.BusinessBases
{
    /// <summary>
    /// 模型层辅助类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ModelContext<T> where T : ThinModelBase, new()
    {
        T obj = null;
        protected internal readonly static EntityInfoHandle CurEntityInfo = EntityInfoManager.GetEntityHandle(typeof(T));
        /// <summary>
        /// 模型层辅助类
        /// </summary>
        public ModelContext() 
        {
            obj = (T)Activator.CreateInstance(typeof(T));

        }
        private DataAccessSetBase _dal;

        /// <summary>
        /// 获取数据层(用作运行SQL)
        /// </summary>
        /// <returns></returns>
        public DataAccessSetBase GetDAL()
        {
            if (_dal == null)
            {
                EntityInfoHandle handle = EntityInfoManager.GetEntityHandle(typeof(T));
                _dal = new DataAccessSetBase(handle);
                _dal.Oper = StaticConnection.GetStaticOperate(handle.DBInfo);
            }
            return _dal;
        }
        /// <summary>
        /// 获取数据层基类
        /// </summary>
        /// <returns></returns>
        public DataAccessBase<T> GetBaseContext()
        {
            DataAccessBase<T> baseDal = new DataAccessBase<T>();
            baseDal.Oper = StaticConnection.GetDefaultOperate<T>();
            return baseDal;
        }

        /// <summary>
        /// 获取执行语法的上下文
        /// </summary>
        /// <returns></returns>
        public BQLDbBase GetContext()
        {
            return GetBaseContext().ContextDAL;
        }

        /// <summary>
        /// 开启事务
        /// </summary>
        /// <returns></returns>
        public DBTransaction StartTransaction()
        {
            DataAccessBase<T> baseDal = GetBaseContext();

            return baseDal.Oper.StartTransaction();
        }
        /// <summary>
        /// 查找
        /// </summary>
        /// <param name="lstScope">范围集合</param>
        /// <returns></returns>
        public DataSet Select(ScopeList lstScope)
        {
            obj.OnSelect(lstScope);
            return GetBaseContext().Select(lstScope);
        }


        /// <summary>
        /// 查询符合指定条件的记录条数
        /// </summary>
        /// <param name="scpoeList">范围查找的集合</param>
        /// <returns></returns>
        public long SelectCount(ScopeList scpoeList)
        {
            obj.OnSelect(scpoeList);
            return GetBaseContext().SelectCount(scpoeList);
        }

        
        /// <summary>
        /// 查询集合
        /// </summary>
        /// <param name="scpoeList">范围查找的集合</param>
        /// <returns></returns>
        public List<T> SelectList(ScopeList scpoeList)
        {
            obj.OnSelect(scpoeList);
            return GetBaseContext().SelectList(scpoeList);
        }
        /// <summary>
        /// 开始非事务的批量动作
        /// </summary>
        /// <returns></returns>
        public BatchAction StartBatchAction()
        {
            return GetBaseContext().Oper.StarBatchAction();
        }

        /// <summary>
        /// 当前实体的转换值
        /// </summary>
        private T GetThisValue()
        {
            return this as T;
        }

        /// <summary>
        /// 范围更新(慎用)
        /// </summary>
        /// <param name="lstValue">设置值</param>
        /// <param name="lstScope">条件</param>
        /// <returns></returns>
        public int UpdateByScope(T obj , ValueSetList lstValue, ScopeList lstScope)
        {
            int ret = 0;
            bool conUpdate=this.obj.BeforeUpdateByScope(obj, lstScope, lstValue,false);
            if (conUpdate)
            {
                ret = GetBaseContext().Update(obj, lstScope, lstValue, false);
            }
            this.obj.AfterUpdateByScope(ret,obj, lstScope, lstValue, false);
            return ret;

        }
        /// <summary>
        /// 范围更新(慎用)
        /// </summary>
        /// <param name="lstScope">条件</param>
        /// <returns></returns>
        public int UpdateByScope(T obj, ScopeList lstScope)
        {
            int ret = 0;
            bool conUpdate = this.obj.BeforeUpdateByScope(obj, lstScope, null, false);
            if (conUpdate)
            {
                ret = GetBaseContext().Update(obj, lstScope, null, false);
            }
            this.obj.AfterUpdateByScope(ret,obj, lstScope, null, false);
            return ret;

        }

        /// <summary>
        /// 查询是否有记录符合此条件
        /// </summary>
        /// <param name="scpoeList">范围查找的集合</param>
        /// <returns></returns>
        public bool ExistsRecord(ScopeList scpoeList)
        {
            obj.OnSelect(scpoeList);
            return GetBaseContext().ExistsRecord(scpoeList);
        }

        /// <summary>
        /// 范围删除(慎用)
        /// </summary>
        /// <param name="lstScope"></param>
        /// <returns></returns>
        public int DeleteByScope(ScopeList lstScope)
        {
            int ret = 0;
            bool conUpdate = this.obj.BeforeDeleteByScope( lstScope);
            if (conUpdate)
            {
                ret = GetBaseContext().Delete(null, lstScope, false);
            }
            this.obj.AfterDeleteByScope(ret, lstScope);
            return ret;
        }

        /// <summary>
        /// 根据主键查找实体
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns></returns>
        public T GetEntityById(object id)
        {
            ScopeList lstScope = new ScopeList();
            PrimaryKeyInfo info = id as PrimaryKeyInfo;
            if (info == null)
            {
                lstScope.AddEqual(CurEntityInfo.PrimaryProperty[0].PropertyName, id);
            }
            else 
            {
                info.FillScope(CurEntityInfo.PrimaryProperty, lstScope, true);
            }



            return GetUnique(lstScope);
        }
        /// <summary>
        /// 根据条件查找实体
        /// </summary>
        /// <param name="lstScope">条件</param>
        /// <returns></returns>
        public T GetUnique(ScopeList lstScope)
        {
            //obj.OnSelect(lstScope);
            lstScope.PageContent.PageSize = 1;
            lstScope.PageContent.CurrentPage = 0;
            lstScope.PageContent.IsFillTotalRecords = false;
            List<T> lst = SelectList(lstScope);
            if (lst.Count > 0)
            {
                return lst[0];
            }

            return null;
        }

    }
}
