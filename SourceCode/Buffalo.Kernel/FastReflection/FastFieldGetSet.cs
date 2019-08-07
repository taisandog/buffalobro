using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Reflection.Emit;
using System.Collections.Concurrent;

namespace Buffalo.Kernel.FastReflection
{
    /// <summary>
    /// ���������ֶ�ֵ��ί��
    /// </summary>
    /// <param name="obj">����</param>
    /// <param name="value">ֵ</param>
    public delegate void SetFieldValueHandle(object obj, object value);
    /// <summary>
    /// ���ٻ�ȡ�ֶ�ֵ��ί��
    /// </summary>
    /// <param name="obj">����</param>
    /// <returns></returns>
    public delegate object GetFieldValueHandle(object obj);
    public class FastFieldGetSet
    {
        private static ConcurrentDictionary<string, GetFieldValueHandle> _dicGet = new ConcurrentDictionary<string, GetFieldValueHandle>();
        private static ConcurrentDictionary<string, SetFieldValueHandle> _dicSet = new ConcurrentDictionary<string, SetFieldValueHandle>();

        /// <summary>
        /// ��ȡ�ֶλ�ȡֵ��ί��(������)
        /// </summary>
        /// <param name="info">�ֶ���Ϣ</param>
        /// <returns></returns>
        public static GetFieldValueHandle FindGetValueHandle(FieldInfo info)
        {
            GetFieldValueHandle ret=null;
            string key=FastValueGetSet.GetMethodInfoKey(info);
            if (!_dicGet.TryGetValue(key, out ret)) 
            {
                ret = GetGetValueHandle(info);
                _dicGet[key] = ret;
            }
            return ret;
        }
        /// <summary>
        /// ��ȡ�ֶ�����ֵ��ί��(������)
        /// </summary>
        /// <param name="info">�ֶ���Ϣ</param>
        /// <returns></returns>
        public static SetFieldValueHandle FindSetValueHandle(FieldInfo info)
        {
            SetFieldValueHandle ret = null;
            string key = FastValueGetSet.GetMethodInfoKey(info);
            if (!_dicSet.TryGetValue(key, out ret))
            {
                ret = GetSetValueHandle(info);
                _dicSet[key] = ret;
            }
            return ret;
        }
        /// <summary>
        /// ��ȡ�ֶλ�ȡֵ��ί��
        /// </summary>
        /// <param name="info">�ֶ���Ϣ</param>
        /// <returns></returns>
        public static GetFieldValueHandle GetGetValueHandle(FieldInfo info) 
        {
            if (info.IsLiteral)
            {
                return null;
            }
            Type sourceType = info.FieldType;

            DynamicMethod dynamicMethod = new DynamicMethod(string.Empty, typeof(object), new Type[] { typeof(object) }, info.DeclaringType.Module, true);
            ILGenerator il = dynamicMethod.GetILGenerator();
            //Label labRet = il.DefineLabel();
            il.Emit(OpCodes.Nop);
            if (info.IsStatic)
            {
                il.Emit(OpCodes.Ldsfld, info);
            }
            else
            {
                il.Emit(OpCodes.Ldarg_0);
                il.Emit(OpCodes.Ldfld, info);
            }
            if (sourceType.IsValueType)
            {
                il.Emit(OpCodes.Box, sourceType);
            }
            il.Emit(OpCodes.Ret);
            return (GetFieldValueHandle)dynamicMethod.CreateDelegate(typeof(GetFieldValueHandle));
        }

        /// <summary>
        /// ��ȡ�ֶλ�ȡֵ��ί��
        /// </summary>
        /// <param name="objType">��������</param>
        /// <param name="fieldName">�ֶ���</param>
        /// <returns></returns>
        public static GetFieldValueHandle GetGetValueHandle(Type objType, string fieldName)
        {
            FieldInfo info = objType.GetField(fieldName, FastValueGetSet.AllBindingFlags);
            return GetGetValueHandle(info);
        }

        /// <summary>
        /// ��ȡ�ֶ�����ֵ��ί��
        /// </summary>
        /// <param name="info">�ֶ���Ϣ</param>
        /// <returns></returns>
        public static SetFieldValueHandle GetSetValueHandle(FieldInfo info) 
        {
            if (info.IsInitOnly || info.IsLiteral) 
            {
                return null;
            }
            Type targetType = info.FieldType;
            //MethodInfo call = typeof(objType).GetMethod("WriteLine", new Type[] { typeof(string), typeof(string) });
            DynamicMethod dynamicMethod = new DynamicMethod(string.Empty, null, new Type[] { typeof(object), typeof(object) }, info.DeclaringType.Module, true);
            ILGenerator il = dynamicMethod.GetILGenerator();

            il.Emit(OpCodes.Nop);
            if (info.IsStatic)
            {
                il.Emit(OpCodes.Ldarg_0);
            }
            else
            {
                il.Emit(OpCodes.Ldarg_0);
                il.Emit(OpCodes.Ldarg_1);
            }
            if (targetType.IsValueType)
            {
                il.Emit(OpCodes.Unbox_Any, targetType);
            }
            else
            {
                il.Emit(OpCodes.Castclass, targetType);
            }
            if (info.IsStatic)
            {
                il.Emit(OpCodes.Stsfld,info);
            }
            else
            {
                il.Emit(OpCodes.Stfld, info);
            }
            il.Emit(OpCodes.Ret);
            return (SetFieldValueHandle)dynamicMethod.CreateDelegate(typeof(SetFieldValueHandle));
        }
        /// <summary>
        /// ��ȡ�ֶ�����ֵ��ί��
        /// </summary>
        /// <param name="objType">��������</param>
        /// <param name="fieldName">�ֶ���</param>
        /// <returns></returns>
        public static SetFieldValueHandle GetSetValueHandle(Type objType, string fieldName)
        {
            FieldInfo info = objType.GetField(fieldName, FastValueGetSet.AllBindingFlags);
            return GetSetValueHandle(info);
        }
    }
}
