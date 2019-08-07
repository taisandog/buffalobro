using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Reflection.Emit;

namespace Buffalo.Kernel.FastReflection
{
    public delegate object FastPropertyHandler(object target, object paramters);

    public class FastPropertyInvoke
    {
        /// <summary>
        /// 生成调用方法
        /// </summary>
        /// <param name="methodInfo"></param>
        /// <returns></returns>
        public static FastPropertyHandler GetMethodInvoker(MethodInfo methodInfo)
        {
            DynamicMethod dynamicMethod = new DynamicMethod(string.Empty, typeof(object), new Type[] { typeof(object), typeof(object) }, methodInfo.DeclaringType.Module);
            ILGenerator il = dynamicMethod.GetILGenerator();
            ParameterInfo[] ps = methodInfo.GetParameters();
            //LocalBuilder ret=il.DeclareLocal(typeof(object));


            if (!methodInfo.IsStatic)
            {
                il.Emit(OpCodes.Ldarg_0);
            }
            for (int i = 0; i < ps.Length; i++)
            {

                il.Emit(OpCodes.Ldarg_S, (i + 1));//加载第几个参数
                FastInvoke.EmitCastToReference(il, ps[i].ParameterType);
            }
            if (methodInfo.IsStatic)
            {
                il.Emit(OpCodes.Call, methodInfo);
            }
            else
            {
                il.Emit(OpCodes.Callvirt, methodInfo);
            }
            Type retType = methodInfo.ReturnType;
            if (retType == FastInvoke.VoidType)
            {
                il.Emit(OpCodes.Ldnull);
            }
            else
            {
                FastInvoke.EmitBoxIfNeeded(il, retType);
            }
            il.Emit(OpCodes.Ret);
            FastPropertyHandler invoder = (FastPropertyHandler)dynamicMethod.CreateDelegate(typeof(FastPropertyHandler));
            return invoder;
        }


    }
}
