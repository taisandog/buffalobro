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
    }
}
