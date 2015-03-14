using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using Buffalo.Kernel.FastReflection;

namespace Buffalo.DB.PropertyAttributes
{
    public class AttributesGetter
    {
        /// <summary>
        /// 获取某个属性的实体映射标识
        /// </summary>
        /// <param name="pinf">属性的信息</param>
        /// <returns></returns>
        public static EntityParam GetEntityParam(FieldInfo finf)
        {
            object entityParam = FastInvoke.GetPropertyAttribute(finf, typeof(EntityParam));
            if (entityParam != null)
            {
                return (EntityParam)entityParam;
            }
            return null;
        }


        
        /// <summary>
        /// 获取该实体对应的表名
        /// </summary>
        /// <param name="type">实体类型</param>
        /// <returns></returns>
        public static TableAttribute GetTableAttribute(Type type)
        {
            object att = FastInvoke.GetClassAttribute(type, typeof(TableAttribute));
            if (att != null)
            {
                return (TableAttribute)att;
            }
            return null;
        }

        

    }
}
