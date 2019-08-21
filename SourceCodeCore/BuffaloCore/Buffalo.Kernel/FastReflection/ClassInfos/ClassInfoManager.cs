using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;


namespace Buffalo.Kernel.FastReflection.ClassInfos
{
    /// <summary>
    /// ʵ�����Թ���
    /// </summary>
    public class ClassInfoManager
    {
        private static Dictionary<string, ClassInfoHandle> dicClass = new Dictionary<string, ClassInfoHandle>();//��¼�Ѿ���ʼ����������

        /// <summary>
        /// ��ȡʵ������ߵ�������Ϣ
        /// </summary>
        /// <param name="type">����</param>
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
        /// ��ʼ�����͵�������Ϣ
        /// </summary>
        /// <param name="type">����</param>
        /// <returns>����Ѿ���ʼ�����෵��false</returns>
        private static void InitClassPropertyInfos(Type type)
        {
            string fullName = type.FullName;
            
            //ʵ���������͵ľ��
            CreateInstanceHandler createrHandel = FastValueGetSet.GetCreateInstanceHandlerWithOutCache(type);
            Dictionary<string, PropertyInfoHandle> dicPropertys = new Dictionary<string, PropertyInfoHandle>();
            Dictionary<string, FieldInfoHandle> dicField = new Dictionary<string, FieldInfoHandle>();
            
            //������Ϣ���
            PropertyInfo[] destproper = type.GetProperties(FastValueGetSet.AllBindingFlags);
            FieldInfo[] allField = type.GetFields(FastValueGetSet.AllBindingFlags);
            //int index = 0;
            ///��ȡ���Ա���
            foreach (PropertyInfo pinf in destproper)
            {
                ///ͨ������������
                string proName = pinf.Name;

                FastPropertyHandler getHandle = FastValueGetSet.GetGetMethodInfo(proName, type);
                FastPropertyHandler setHandle = FastValueGetSet.GetSetMethodInfo(proName, type);
                if (getHandle != null || setHandle != null)
                {
                    PropertyInfoHandle classProperty = new PropertyInfoHandle(type,getHandle, setHandle, pinf.PropertyType, pinf.Name);
                    dicPropertys.Add(pinf.Name, classProperty);
                }
            }

            ///��ȡ���Ա���
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
        /// �������Կ���(ͬ���ֶ�)
        /// </summary>
        /// <param name="source">Դ����</param>
        /// <param name="target">Ŀ�����</param>
        /// <returns></returns>
        public static int ObjectCopy(object source, object target)
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
                if (tInfo != null)
                {
                    if (tInfo.FieldType == fInfo.FieldType)
                    {
                        tInfo.SetValue(target, fInfo.GetValue(source));
                        ret++;
                    }
                }
            }


            return ret;
        }


    }
}
