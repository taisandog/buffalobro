using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Reflection.Emit;

namespace Buffalo.Kernel.FastReflection
{
    /** 
     * @原作者:JeffreyZhao
     * @创建时间:2009-02-01
     * @链接:http://www.cnblogs.com/JeffreyZhao/archive/2009/02/01/Fast-Reflection-Library.html
     * @说明:.NET IL快速反射类
    */
    public delegate object CreateInstanceHandler();
    public delegate object FastInvokeHandler(object target, object[] paramters);
    public class FastInvoke
    {
        
        /// <summary>
        /// 委托获取实体类对象
        /// </summary>
        /// <returns></returns>
        public static readonly Type VoidType = Type.GetType("System.Void");
        static object InvokeMethod(FastInvokeHandler invoke, object target, params object[] paramters)
        {
            return invoke(null, paramters);
        }

        

        public static FastInvokeHandler GetMethodInvoker(MethodInfo methodInfo)
        {
            DynamicMethod dynamicMethod = new DynamicMethod(string.Empty, typeof(object), new Type[] { typeof(object), typeof(object[]) }, methodInfo.DeclaringType.Module);
            ILGenerator il = dynamicMethod.GetILGenerator();
            ParameterInfo[] ps = methodInfo.GetParameters();
            Type[] paramTypes = new Type[ps.Length];
            for (int i = 0; i < paramTypes.Length; i++)
            {
                if (ps[i].ParameterType.IsByRef)
                    paramTypes[i] = ps[i].ParameterType.GetElementType();
                else
                    paramTypes[i] = ps[i].ParameterType;
            }
            LocalBuilder[] locals = new LocalBuilder[paramTypes.Length];

            for (int i = 0; i < paramTypes.Length; i++)
            {
                locals[i] = il.DeclareLocal(paramTypes[i], true);
            }
            for (int i = 0; i < paramTypes.Length; i++)
            {
                il.Emit(OpCodes.Ldarg_1);
                EmitFastInt(il, i);
                il.Emit(OpCodes.Ldelem_Ref);
                EmitCastToReference(il, paramTypes[i]);
                il.Emit(OpCodes.Stloc, locals[i]);
            }
            if (!methodInfo.IsStatic)
            {
                il.Emit(OpCodes.Ldarg_0);
            }
            for (int i = 0; i < paramTypes.Length; i++)
            {
                if (ps[i].ParameterType.IsByRef)
                    il.Emit(OpCodes.Ldloca_S, locals[i]);
                else
                    il.Emit(OpCodes.Ldloc, locals[i]);
            }
            if (methodInfo.IsStatic)
                il.EmitCall(OpCodes.Call, methodInfo, null);
            else
                il.EmitCall(OpCodes.Callvirt, methodInfo, null);
            if (methodInfo.ReturnType == VoidType)
                il.Emit(OpCodes.Ldnull);
            else
                EmitBoxIfNeeded(il, methodInfo.ReturnType);

            for (int i = 0; i < paramTypes.Length; i++)
            {
                if (ps[i].ParameterType.IsByRef)
                {
                    il.Emit(OpCodes.Ldarg_1);
                    EmitFastInt(il, i);
                    il.Emit(OpCodes.Ldloc, locals[i]);
                    if (locals[i].LocalType.IsValueType)
                        il.Emit(OpCodes.Box, locals[i].LocalType);
                    il.Emit(OpCodes.Stelem_Ref);
                }
            }

            il.Emit(OpCodes.Ret);
            FastInvokeHandler invoder = (FastInvokeHandler)dynamicMethod.CreateDelegate(typeof(FastInvokeHandler));
            return invoder;
        }

        internal static void EmitCastToReference(ILGenerator il, System.Type type)
        {
            if (type.IsValueType)
            {
                il.Emit(OpCodes.Unbox_Any, type);
            }
            else
            {
                il.Emit(OpCodes.Castclass, type);
            }
        }

        internal static void EmitBoxIfNeeded(ILGenerator il, System.Type type)
        {
            if (type.IsValueType)
            {
                il.Emit(OpCodes.Box, type);
            }
        }

        private static void EmitFastInt(ILGenerator il, int value)
        {
            switch (value)
            {
                case -1:
                    il.Emit(OpCodes.Ldc_I4_M1);
                    return;
                case 0:
                    il.Emit(OpCodes.Ldc_I4_0);
                    return;
                case 1:
                    il.Emit(OpCodes.Ldc_I4_1);
                    return;
                case 2:
                    il.Emit(OpCodes.Ldc_I4_2);
                    return;
                case 3:
                    il.Emit(OpCodes.Ldc_I4_3);
                    return;
                case 4:
                    il.Emit(OpCodes.Ldc_I4_4);
                    return;
                case 5:
                    il.Emit(OpCodes.Ldc_I4_5);
                    return;
                case 6:
                    il.Emit(OpCodes.Ldc_I4_6);
                    return;
                case 7:
                    il.Emit(OpCodes.Ldc_I4_7);
                    return;
                case 8:
                    il.Emit(OpCodes.Ldc_I4_8);
                    return;
            }

            if (value > -129 && value < 128)
            {
                il.Emit(OpCodes.Ldc_I4_S, (SByte)value);
            }
            else
            {
                il.Emit(OpCodes.Ldc_I4, value);
            }
        }

        /// <summary>
        /// 生成实体类
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static CreateInstanceHandler GetInstanceCreator(Type type)
        {
            // generates a dynamic method to generate a FastCreateInstanceHandler delegate
            DynamicMethod dynamicMethod = new DynamicMethod(string.Empty, type, new Type[0], typeof(FastInvoke).Module);

            ILGenerator ilGenerator = dynamicMethod.GetILGenerator();
            // generates code to create a new object of the specified type using the default constructor
            ilGenerator.Emit(OpCodes.Newobj, type.GetConstructor(Type.EmptyTypes));
            // returns the value to the caller
            ilGenerator.Emit(OpCodes.Ret);
            // converts the DynamicMethod to a FastCreateInstanceHandler delegate to create the object
            CreateInstanceHandler creator = (CreateInstanceHandler)dynamicMethod.CreateDelegate(typeof(CreateInstanceHandler));
            return creator;
        }


        /// <summary>
        /// 获取某个属性的标识
        /// </summary>
        /// <param name="finf">属性的信息</param>
        /// <param name="attributeType">属性类型</param>
        /// <returns></returns>
        public static object GetPropertyAttribute(FieldInfo finf, Type attributeType)
        {
            object[] atts = finf.GetCustomAttributes(attributeType, true);
            if (atts.Length > 0)
            {
                return atts[0];
            }
            return null;
        }

        /// <summary>
        /// 获取某个属性的标识
        /// </summary>
        /// <param name="pinf">属性的信息</param>
        /// <param name="attributeType">属性类型</param>
        /// <returns></returns>
        public static object GetPropertyAttribute(PropertyInfo pinf, Type attributeType)
        {
            object[] atts = pinf.GetCustomAttributes(attributeType, true);
            if (atts.Length > 0)
            {
                return atts[0];
            }
            return null;
        }
        /// <summary>
        /// 获取某个属性的标识
        /// </summary>
        /// <param name="pinf">属性的信息</param>
        /// <param name="attributeType">属性类型</param>
        /// <returns></returns>
        public static T GetPropertyAttribute<T>(PropertyInfo pinf)
             where T : System.Attribute
        {
            object[] atts = pinf.GetCustomAttributes(typeof(T), true);
            if (atts.Length > 0)
            {
                return atts[0] as T;
            }
            return null;
        }
        /// <summary>
        /// 获取指定类的标识
        /// </summary>
        /// <param name="classType">指定类的类型</param>
        /// <param name="attributeType">标识类型</param>
        /// <returns></returns>
        public static object GetClassAttribute(Type classType, Type attributeType)
        {
            object[] atts = classType.GetCustomAttributes(attributeType, true);
            if (atts.Length > 0)
            {
                return atts[0];
            }
            return null;
        }
        /// <summary>
        /// 获取指定类的标识
        /// </summary>
        /// <param name="classType">指定类的类型</param>
        /// <returns></returns>
        public static T GetClassAttribute<T>(Type classType)
            where T : System.Attribute
        {
            object[] atts = classType.GetCustomAttributes(typeof(T), true);
            if (atts.Length > 0)
            {
                return atts[0] as T;
            }
            return null;
        }
    }
}
