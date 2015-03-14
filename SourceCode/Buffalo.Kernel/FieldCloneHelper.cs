using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using Buffalo.Kernel.FastReflection;
using System.Runtime.Serialization;
using Buffalo.Kernel.Defaults;

namespace Buffalo.Kernel
{
    /// <summary>
    /// 对对象进行字段克隆
    /// </summary>
    public class FieldCloneHelper
    {
        private static Dictionary<string, List<FieldInfoHandle>> _dicFieldInfo 
            = new Dictionary<string, List<FieldInfoHandle>>();


        /// <summary>
        /// 拷贝数据
        /// </summary>
        /// <param name="source">源类</param>
        /// <param name="target">目标类</param>
        public static void CopyTo(object source, object target) 
        {
            if (source == null || target==null)
            {
                return;
            }
            Type objType = source.GetType();
            List<FieldInfoHandle> lstHandle = GetFieldInfos(objType);
            foreach (FieldInfoHandle fHandle in lstHandle) 
            {
                object value = fHandle.GetValue(source);
                fHandle.SetValue(target, value);
            }
        }

        /// <summary>
        /// 克隆
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static object Clone(object source) 
        {
            
            
            Type objType = source.GetType();
            object target=Activator.CreateInstance(objType);

            CopyTo(source, target);
            return target;
        }

        private static List<FieldInfoHandle> GetFieldInfos(Type objType) 
        {
            string key = objType.FullName;
            List<FieldInfoHandle> ret = null;

           
            
            if (!_dicFieldInfo.TryGetValue(key, out ret)) 
            {
                ret = new List<FieldInfoHandle>();
                _dicFieldInfo[key] = ret;

                Type currentType = objType;

                while (currentType != null)
                {
                    FieldInfo[] fInfos = currentType.GetFields(FastValueGetSet.AllBindingFlags);
                    foreach (FieldInfo finfo in fInfos)
                    {

                        GetFieldValueHandle getHandle=FastFieldGetSet.GetGetValueHandle(finfo);
                        SetFieldValueHandle setHandle = FastFieldGetSet.GetSetValueHandle(finfo);
                        FieldInfoHandle handle = new FieldInfoHandle(objType, getHandle, setHandle, finfo.FieldType, finfo.Name, finfo);
                        ret.Add(handle);
                        
                    }
                    currentType = currentType.BaseType;
                    if (currentType == typeof(object)) 
                    {
                        break;
                    }
                }
            }
            
            return ret;
        }

  

    }
}
