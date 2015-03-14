using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection.Emit;
using System.Reflection;
using Buffalo.Kernel.FastReflection;

namespace Buffalo.Kernel.Win32
{
    public delegate int GetPtrHandler(object target);


    public class GetPtrCls 
    {

        /// <summary>
        /// 返回指针的函数
        /// </summary>
        /// <param name="ptr"></param>
        /// <returns></returns>
        public static int GetPtr(int ptr)
        {
            return ptr;
        }
    }

    /// <summary>
    /// 指针获取器
    /// </summary>
    public class PtrGeter
    {


        private static MethodInfo _getPtrMethod = InitGetPtrMethod();
        private static GetPtrHandler _ptrHandle = GetMethodInvoker();
        /// <summary>
        /// 初始化获取指针的方法
        /// </summary>
        /// <returns></returns>
        private static MethodInfo InitGetPtrMethod() 
        {
            Type type = typeof(GetPtrCls);
            return type.GetMethod("GetPtr", FastValueGetSet.AllBindingFlags);
        }

        /// <summary>
        /// 生成调用方法
        /// </summary>
        /// <param name="methodInfo"></param>
        /// <returns></returns>
        public static GetPtrHandler GetMethodInvoker()
        {
            DynamicMethod dynamicMethod = new DynamicMethod(string.Empty, typeof(int), new Type[] { typeof(object) }, _getPtrMethod.DeclaringType.Module);
            ILGenerator il = dynamicMethod.GetILGenerator();

            il.Emit(OpCodes.Ldarg_S, 0);
            il.EmitCall(OpCodes.Call, _getPtrMethod, null);
            il.Emit(OpCodes.Ret);
            GetPtrHandler invoder = (GetPtrHandler)dynamicMethod.CreateDelegate(typeof(GetPtrHandler));
            return invoder;
        }

        /// <summary>
        /// 获取对象的指针
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static int GetObjectPtr(object obj) 
        {
            return _ptrHandle(obj);
        }
    }
}
