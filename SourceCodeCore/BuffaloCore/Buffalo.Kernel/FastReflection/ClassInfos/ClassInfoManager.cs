using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;


namespace Buffalo.Kernel.FastReflection.ClassInfos
{
    /// <summary>
    /// 实体属性管理
    /// </summary>
    public class ClassInfoManager
    {
        private static Dictionary<string, ClassInfoHandle> dicClass = new Dictionary<string, ClassInfoHandle>();//记录已经初始化过的类型

        /// <summary>
        /// 获取实体类里边得属性信息
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns></returns>
        public static ClassInfoHandle GetClassHandle(Type type)
        {
            string fullName = type.FullName;
            ClassInfoHandle classHandle = null;

            if (!dicClass.TryGetValue(fullName, out classHandle))
            {
                InitClassPropertyInfos(type);
                classHandle = dicClass[fullName];
            }

            return classHandle;
        }
        /// <summary>
        /// 初始化类型的属性信息
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns>如果已经初始化过侧返回false</returns>
        private static void InitClassPropertyInfos(Type type)
        {
            string fullName = type.FullName;
            
            //实例化本类型的句柄
            CreateInstanceHandler createrHandel = FastValueGetSet.GetCreateInstanceHandlerWithOutCache(type);
            Dictionary<string, PropertyInfoHandle> dicPropertys = new Dictionary<string, PropertyInfoHandle>();
            Dictionary<string, FieldInfoHandle> dicField = new Dictionary<string, FieldInfoHandle>();
            
            //属性信息句柄
            PropertyInfo[] destproper = type.GetProperties(FastValueGetSet.AllBindingFlags);
            FieldInfo[] allField = type.GetFields(FastValueGetSet.AllBindingFlags);
            //int index = 0;
            ///读取属性别名
            foreach (PropertyInfo pinf in destproper)
            {
                ///通过属性来反射
                string proName = pinf.Name;

                FastPropertyHandler getHandle = FastValueGetSet.GetGetMethodInfo(proName, type);
                FastPropertyHandler setHandle = FastValueGetSet.GetSetMethodInfo(proName, type);
                if (getHandle != null || setHandle != null)
                {
                    PropertyInfoHandle classProperty = new PropertyInfoHandle(type,getHandle, setHandle, pinf.PropertyType, pinf.Name);
                    dicPropertys.Add(pinf.Name, classProperty);
                }
            }

            ///读取属性别名
            foreach (FieldInfo fInf in allField)
            {
                string proName = fInf.Name;

                GetFieldValueHandle getHandle = FastFieldGetSet.GetGetValueHandle(fInf);
                SetFieldValueHandle setHandle = FastFieldGetSet.GetSetValueHandle(fInf);
                if (getHandle != null || setHandle != null)
                {
                    FieldInfoHandle fieldInfo = new FieldInfoHandle(type, getHandle, setHandle, fInf.FieldType, fInf.Name,fInf);
                    dicField.Add(fInf.Name, fieldInfo);
                }
            }


            ClassInfoHandle classInfo = new ClassInfoHandle(type, createrHandel, dicPropertys, dicField);
            dicClass.Add(fullName, classInfo);
        }



        /// <summary>
        /// 对象字段拷贝(同名字段)
        /// </summary>
        /// <param name="source">源对象</param>
        /// <param name="target">目标对象</param>
        /// <returns></returns>
        public static int ObjectCopyByField(object source, object target)
        {
            if (source == null || target == null)
            {
                return 0;
            }
            int ret = 0;
            Type sourceType = source.GetType();
            Type targetType = source.GetType();

            ClassInfoHandle sourceInfo = ClassInfoManager.GetClassHandle(sourceType);
            ClassInfoHandle targetInfo = ClassInfoManager.GetClassHandle(targetType);

            foreach (FieldInfoHandle fInfo in sourceInfo.FieldInfo)
            {
                FieldInfoHandle tInfo = targetInfo.FieldInfo[fInfo.FieldName];
                if (tInfo == null)
                {
                    continue;
                }

                object sourceValue = fInfo.GetValue(source);
                if (tInfo.FieldType != fInfo.FieldType)
                {

                    try
                    {
                        sourceValue = ValueConvertExtend.ConvertValue(sourceValue, tInfo.FieldType, null);
                    }
                    catch (Exception ex)
                    {
                        continue;
                    }
                }
                tInfo.SetValue(target, sourceValue);
                ret++;


            }


            return ret;
        }
        /// <summary>
        /// 对象属性拷贝(同名属性)
        /// </summary>
        /// <param name="source">源对象</param>
        /// <param name="target">目标对象</param>
        /// <returns></returns>
        public static int ObjectCopyByProperty(object source, object target)
        {
            if (source == null || target == null)
            {
                return 0;
            }
            int ret = 0;
            Type sourceType = source.GetType();
            Type targetType = source.GetType();

            ClassInfoHandle sourceInfo = ClassInfoManager.GetClassHandle(sourceType);
            ClassInfoHandle targetInfo = ClassInfoManager.GetClassHandle(targetType);

            foreach (PropertyInfoHandle sInfo in sourceInfo.PropertyInfo)
            {
                PropertyInfoHandle tInfo = targetInfo.PropertyInfo[sInfo.PropertyName];
                if (tInfo == null)
                {
                    continue;
                }
                if (CopyProperty(source, target, sInfo, tInfo))
                {
                    ret++;
                }

            }
            return ret;
        }

        /// <summary>
        /// 复制属性
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        /// <param name="sourceHandle"></param>
        /// <param name="targetHandle"></param>
        /// <returns></returns>
        private static bool CopyProperty(object source, object target, PropertyInfoHandle sourceHandle, PropertyInfoHandle targetHandle) 
        {
            if (!sourceHandle.HasGetHandle) 
            {
                return false;
            }
            if (!targetHandle.HasSetHandle)
            {
                return false;
            }
            object sourceValue = sourceHandle.GetValue(source);
            if(sourceHandle.PropertyType != targetHandle.PropertyType) 
            {
                try
                {
                    sourceValue = ValueConvertExtend.ConvertValue(sourceValue, targetHandle.PropertyType, null);

                }
                catch (Exception ex)
                {
                    return false;
                }
            }
            targetHandle.SetValue(target, sourceValue);
            return true;
        }

    }
}
