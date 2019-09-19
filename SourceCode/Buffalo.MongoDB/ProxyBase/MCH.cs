using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buffalo.MongoDB.ProxyBase
{
    public class MCH
    {

        /// <summary>
        /// 创建类
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="isProxy">是否代理类</param>
        /// <returns></returns>
        public static T Create<T>(bool isProxy) where T : MongoEntityBase
        {
            return Create(typeof(T),isProxy) as T;
        }
        /// <summary>
        /// 创建类
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T Create<T>() where T : MongoEntityBase
        {
            return Create(typeof(T), false) as T;
        }
        /// <summary>
        /// 创建类
        /// </summary>
        /// <param name="objType"></param>
        /// <param name="isProxy">是否代理类</param>
        /// <returns></returns>
        public static object Create(Type objType,bool isProxy)
        {
            if (isProxy)
            {
                MongoEntityInfo info = MongoEntityInfoManager.GetEntityHandle(objType);
                if (info == null)
                {
                    return null;
                }
                return info.CreateProxyInstance();
            }
            return Activator.CreateInstance(objType);
        }
        /// <summary>
        /// 创建类
        /// </summary>
        /// <param name="objType"></param>
        /// <returns></returns>
        public static object Create(Type objType)
        {
            return Create(objType,false);
        }
        private static Type _baseType = typeof(MongoEntityBase);
        /// <summary>
        /// 获取真正的类型
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static Type GetRealType(object obj)
        {
            Type type = obj.GetType();
            Type curType = type;
            while(curType.BaseType!= _baseType)
            {
                if (curType == null)
                {
                    return type;
                }
                curType = curType.BaseType;
            }
            
            return curType;
        }
    }
}
