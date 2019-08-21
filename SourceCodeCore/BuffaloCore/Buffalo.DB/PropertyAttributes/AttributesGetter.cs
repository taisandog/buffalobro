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
        /// ��ȡĳ�����Ե�ʵ��ӳ���ʶ
        /// </summary>
        /// <param name="pinf">���Ե���Ϣ</param>
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
        /// ��ȡ��ʵ���Ӧ�ı���
        /// </summary>
        /// <param name="type">ʵ������</param>
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
