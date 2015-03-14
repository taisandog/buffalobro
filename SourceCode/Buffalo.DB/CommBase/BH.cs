using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DB.QueryConditions;
using Buffalo.DB.CommBase.BusinessBases;
using Buffalo.DB.DataBaseAdapter;
using Buffalo.DB.BQLCommon.BQLConditionCommon;
using Buffalo.DB.EntityInfos;
using Buffalo.Kernel.FastReflection;
using Buffalo.Kernel;

namespace Buffalo.DB.CommBase
{
    /// <summary>
    /// 业务辅助类(只限三层架构)
    /// </summary>
    public class BH
    {
        /// <summary>
        /// 查询集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="lstScope">条件</param>
        /// <returns></returns>
        public static List<T> SelectList<T>(ScopeList lstScope) where T : EntityBase, new()
        {
            string name=typeof(T).FullName;
            BusinessModelBase<T> bo = DataAccessLoader.GetBoInstance(name) as BusinessModelBase<T>;
            if (bo == null) 
            {
                throw new MissingMemberException("找不到:" + name + " 对应的业务类");
            }
            return bo.SelectList(lstScope);
        }
        /// <summary>
        /// 查询集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="lstScope">条件</param>
        /// <returns></returns>
        public static List<T> Find<T>(BQLCondition condition, PageContent pager) where T : EntityBase, new()
        {
            ScopeList lstScope = new ScopeList();
            if (!CommonMethods.IsNull(condition))
            {
                lstScope.Add(condition);
            }
            if (pager != null) 
            {
                lstScope.PageContent = pager;
            }
            return SelectList<T>(lstScope);
        }
        /// <summary>
        /// 查询集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="lstScope">条件</param>
        /// <returns></returns>
        public static List<T> Find<T>(BQLCondition condition) where T : EntityBase, new()
        {
            return Find<T>(condition,null);
        }

        /// <summary>
        /// 获取唯一
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="condition">条件</param>
        /// <returns></returns>
        public static T GetUnique<T>(BQLCondition condition) where T : EntityBase, new()
        {
            string name = typeof(T).FullName;
            BusinessModelBase<T> bo = DataAccessLoader.GetBoInstance(name) as BusinessModelBase<T>;
            if (bo == null)
            {
                throw new MissingMemberException("找不到:" + name + " 对应的业务类");
            }
            ScopeList lstScope = new ScopeList();
            if (!CommonMethods.IsNull(condition))
            {
                lstScope.Add(condition);
            }
            return bo.GetUnique(lstScope);
        }
        /// <summary>
        /// 获取唯一
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="condition">条件</param>
        /// <returns></returns>
        public static T GetById<T>(object id) where T : EntityBase, new()
        {
            string name = typeof(T).FullName;
            BusinessModelBase<T> bo = DataAccessLoader.GetBoInstance(name) as BusinessModelBase<T>;
            if (bo == null)
            {
                throw new MissingMemberException("找不到:" + name + " 对应的业务类");
            }

            return bo.GetEntityById(id);
        }
        /// <summary>
        /// 查询数目
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="entityType"></param>
        /// <returns></returns>
        public static long SelectCount<T>(BQLCondition condition) where T : EntityBase, new()
        {
            string name = typeof(T).FullName;
            BusinessModelBase<T> bo = DataAccessLoader.GetBoInstance(name) as BusinessModelBase<T>;
            if (bo == null)
            {
                throw new MissingMemberException("找不到:" + name + " 对应的业务类");
            }
            ScopeList lstScope = new ScopeList();
            if (!CommonMethods.IsNull(condition))
            {
                lstScope.Add(condition);
            }
            return bo.SelectCount(lstScope);
        }

        /// <summary>
        /// 更新一个实体
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static object Update(EntityBase entity) 
        {
            Type type = CH.GetRealType(entity);
            string name = type.FullName;
            object bo = DataAccessLoader.GetBoInstance(name);
            FastInvokeHandler inv=FastValueGetSet.GetCustomerMethodInfo(bo.GetType(), "Update", new Type[] { type });
            return inv(bo, new object[] { entity });
        }

        /// <summary>
        /// 删除一个实体
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static object Delete(EntityBase entity)
        {
            Type type = CH.GetRealType(entity);
            string name = type.FullName;
            object bo = DataAccessLoader.GetBoInstance(name);
            FastInvokeHandler inv = FastValueGetSet.GetCustomerMethodInfo(bo.GetType(), "Delete", new Type[] { type });
            return inv(bo, new object[] { entity });
        }
        /// <summary>
        /// 插入一个实体
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static object Insert(EntityBase entity,bool fillIdentity)
        {
            Type type = CH.GetRealType(entity);
            string name = type.FullName;
            object bo = DataAccessLoader.GetBoInstance(name);
            FastInvokeHandler inv = FastValueGetSet.GetCustomerMethodInfo(bo.GetType(), "Insert", new Type[] { type,typeof(bool) });
            return inv(bo, new object[] { entity, fillIdentity });
        }
    }
}
