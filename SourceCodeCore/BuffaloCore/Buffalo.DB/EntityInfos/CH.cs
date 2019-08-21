using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DB.CommBase;
using Buffalo.DB.ProxyBuilder;

namespace Buffalo.DB.EntityInfos
{
    /// <summary>
    /// 代理类创建器
    /// </summary>
    public class CH
    {
        /// <summary>
        /// 创建代理类
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T Create<T>() where T:EntityBase
        {
            Type objType=typeof(T);
            EntityInfoHandle handle = EntityInfoManager.GetEntityHandle(objType);
            if (handle != null) 
            {
                return handle.CreateProxyInstance() as T;
            }
            return null;
        }
        /// <summary>
        /// 创建代理类
        /// </summary>
        /// <returns></returns>
        public static object Create(Type objType)
        {
            EntityInfoHandle handle = EntityInfoManager.GetEntityHandle(objType);
            if (handle != null)
            {
                return handle.CreateProxyInstance();
            }
            return null;
        }

        /// <summary>
        /// 获取代理类类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static Type GetProxyType<T>() where T : EntityBase
        {
            Type objType = typeof(T);
            EntityInfoHandle handle = EntityInfoManager.GetEntityHandle(objType);
            if (handle != null)
            {
                return handle.ProxyType;
            }
            return null;
        }
        /// <summary>
        /// 获取代理类类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static Type GetProxyType(Type objType)
        {
            EntityInfoHandle handle = EntityInfoManager.GetEntityHandle(objType);
            if (handle != null)
            {
                return handle.ProxyType;
            }
            return null;
        }
       

        /// <summary>
        /// 获取真正的类型
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static Type GetRealType(object obj)
        {
            IEntityProxy iep = obj as IEntityProxy;
            if (iep != null) 
            {
                return iep.GetEntityType();
            }
            return obj.GetType();
        }

        /// <summary>
        /// 判断实体主键是否为未赋值(关联查询出来实际不存在的实体)
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static bool IsPrimaryNULL(EntityBase entity) 
        {
            if (entity == null) 
            {
                return true;
            }
            foreach(EntityPropertyInfo info in entity.GetEntityBaseInfo().EntityInfo.PrimaryProperty)
            {
                object val=info.GetValue(entity);

                if (!info.BelongFieldInfo.FieldType.IsValueType) 
                {
                    if (val != null) 
                    {
                        return false;
                    }
                }
                else 
                {
                    if (!val.Equals(Activator.CreateInstance(val.GetType()))) 
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }
}
