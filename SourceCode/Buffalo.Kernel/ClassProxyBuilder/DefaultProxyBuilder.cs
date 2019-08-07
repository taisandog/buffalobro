using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Reflection.Emit;
/** 
 * @原作者:benben
 * @创建时间:2012-2-19 09:02
 * @链接:http://www.189works.com/article-43203-1.html
 * @说明:.NET IL动态代理创建
*/
namespace Buffalo.Kernel.ClassProxyBuilder
{
    /// <summary>
    /// 代理建造类
    /// </summary>
    public class DefaultProxyBuilder
    {
        private static readonly Type VoidType = Type.GetType("System.Void");
        AssemblyName _assemblyName ;
        AssemblyBuilder _assemblyBuilder;
        ModuleBuilder _moduleBuilder;
        Type _interceptorType;

        MethodInfo _getDefaultMethod;
        MethodInfo _afterCallMethod;
        MethodInfo _beforeCallMethod;
        /// <summary>
        /// 代理建造类
        /// </summary>
        /// <param name="classNamespace">类的命名空间</param>
        /// <param name="interceptorType">拦截器</param>
        public DefaultProxyBuilder(string classNamespace, Type interceptorType) 
        {
            _assemblyName = new AssemblyName(classNamespace);
            _assemblyBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(_assemblyName,
                                                                            AssemblyBuilderAccess.RunAndSave);
            _moduleBuilder = _assemblyBuilder.DefineDynamicModule(classNamespace);
            _interceptorType = interceptorType;
            //_getDefaultMethod = _interceptorType.GetMethod("GetDefault");
            _afterCallMethod = _interceptorType.GetMethod("AfterCall");
            _beforeCallMethod = _interceptorType.GetMethod("BeforeCall");
        }

        /// <summary>
        /// 建造代理类
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public Type CreateProxyType(Type classType,string className)
        {

            //string name = classType.Namespace + ".ProxyClass";
            if (string.IsNullOrEmpty(className)) 
            {
                className = classType.Namespace + ".ProxyClass";
            }
           
            Type aopType = BulidType(classType, _moduleBuilder, className);
            return aopType;
        }
        /// <summary>
        /// 建造类
        /// </summary>
        /// <param name="classType"></param>
        /// <param name="moduleBuilder"></param>
        /// <returns></returns>
        private Type BulidType(Type classType, ModuleBuilder moduleBuilder,string className)
        {
            //string className = classType.Name + "_Proxy";
            if (string.IsNullOrEmpty(className)) 
            {
                className = classType.Name + "_Proxy";
            }
            //定义类型
            TypeBuilder typeBuilder = moduleBuilder.DefineType(className,
                                                       TypeAttributes.Public | TypeAttributes.Sealed | TypeAttributes.Class,
                                                       classType);
            //定义字段 _inspector
            FieldBuilder inspectorFieldBuilder = typeBuilder.DefineField("_inspector", typeof(IInterceptor),
                                                                FieldAttributes.Public | FieldAttributes.InitOnly);
            //构造函数
            //BuildCtor(classType, inspectorFieldBuilder, typeBuilder);

            //构造方法
            BuildMethod(classType, inspectorFieldBuilder, typeBuilder);
            Type aopType = typeBuilder.CreateType();
            return aopType;
        }

        /// <summary>
        /// 判断需要重建函数
        /// </summary>
        /// <param name="methodInfo"></param>
        protected virtual bool NeedBuildBeforeMethod(Type classType, MethodInfo methodInfo)
        {
            return true;
        }
        /// <summary>
        /// 判断需要重建函数
        /// </summary>
        /// <param name="methodInfo"></param>
        protected virtual bool NeedBuildAfterMethod(Type classType, MethodInfo methodInfo)
        {
            return true;
        }
        /// <summary>
        /// 建方法
        /// </summary>
        /// <param name="classType"></param>
        /// <param name="inspectorFieldBuilder"></param>
        /// <param name="typeBuilder"></param>
        private void BuildMethod(Type classType, FieldBuilder inspectorFieldBuilder, TypeBuilder typeBuilder)
        {
            MethodInfo[] methodInfos = classType.GetMethods();
            foreach (MethodInfo methodInfo in methodInfos)
            {
                if (!methodInfo.IsVirtual && !methodInfo.IsAbstract) continue;
                if (methodInfo.Name == "ToString") continue;
                if (methodInfo.Name == "GetHashCode") continue;
                if (methodInfo.Name == "Equals") continue;

                bool needBeford = NeedBuildBeforeMethod(classType, methodInfo);
                bool needAfter = NeedBuildAfterMethod(classType, methodInfo);

                if (!needBeford && !needAfter) 
                {
                    continue;
                }

                ParameterInfo[] parameterInfos = methodInfo.GetParameters();
                List<Type> lstType=new List<Type>(parameterInfos.Length);
                foreach(ParameterInfo info in parameterInfos)
                {
                    lstType.Add(info.ParameterType);
                }

                Type[] parameterTypes = lstType.ToArray();
                int parameterLength = parameterTypes.Length;
                bool hasResult = methodInfo.ReturnType != VoidType;

                MethodBuilder methodBuilder = typeBuilder.DefineMethod(methodInfo.Name,
                                                             MethodAttributes.Public | MethodAttributes.Final |
                                                             MethodAttributes.Virtual
                                                             , methodInfo.ReturnType
                                                             , parameterTypes);

                ILGenerator il = methodBuilder.GetILGenerator();


                if (needBeford)
                {
                    //局部变量
                    il.DeclareLocal(typeof(object)); //correlationState
                    il.DeclareLocal(typeof(object)); //result
                    il.DeclareLocal(typeof(object[])); //parameters

                    //BeforeCall(string operationName, object[] inputs);
                    il.Emit(OpCodes.Ldarg_0);

                    il.Emit(OpCodes.Ldfld, inspectorFieldBuilder);//获取字段_inspector

                    //il.Emit(OpCodes.Ldarg_0);//this
                    il.Emit(OpCodes.Ldstr, methodInfo.Name);//参数operationName

                    if (parameterLength == 0)//判断方法参数长度
                    {
                        il.Emit(OpCodes.Ldnull);//null -> 参数 inputs
                    }
                    else
                    {
                        //创建new object[parameterLength];
                        il.Emit(OpCodes.Ldc_I4, parameterLength);
                        il.Emit(OpCodes.Newarr, typeof(Object));
                        il.Emit(OpCodes.Stloc_2);//压入局部变量2 parameters

                        for (int i = 0, j = 1; i < parameterLength; i++, j++)
                        {
                            //object[i] = arg[j]
                            il.Emit(OpCodes.Ldloc_2);
                            il.Emit(OpCodes.Ldc_I4, 0);
                            il.Emit(OpCodes.Ldarg, j);
                            if (parameterTypes[i].IsValueType) il.Emit(OpCodes.Box, parameterTypes[i]);//对值类型装箱
                            il.Emit(OpCodes.Stelem_Ref);
                        }
                        il.Emit(OpCodes.Ldloc_2);//取出局部变量2 parameters-> 参数 inputs
                    }

                    il.Emit(OpCodes.Callvirt, _beforeCallMethod);//调用BeforeCall
                    il.Emit(OpCodes.Stloc_0);//建返回压入局部变量0 correlationState
                }


                //Call methodInfo
                il.Emit(OpCodes.Ldarg_0);
                //获取参数表
                for (int i = 1, length = parameterLength + 1; i < length; i++)
                {
                    il.Emit(OpCodes.Ldarg_S, i);
                }
                il.Emit(OpCodes.Call, methodInfo);
                //将返回值压入 局部变量1result void就压入null
                if (!hasResult)
                {
                    il.Emit(OpCodes.Ldnull);
                }
                else if (methodInfo.ReturnType.IsValueType)
                {
                    il.Emit(OpCodes.Box, methodInfo.ReturnType);//对值类型装箱
                }

                il.Emit(OpCodes.Stloc_1);

                if (needAfter)
                {
                    //AfterCall(string operationName, object returnValue, object correlationState);
                    il.Emit(OpCodes.Ldarg_0);
                    il.Emit(OpCodes.Ldfld, inspectorFieldBuilder);//获取字段_inspector
                    il.Emit(OpCodes.Ldarg_0);//this
                    il.Emit(OpCodes.Ldstr, methodInfo.Name);//参数 operationName
                    il.Emit(OpCodes.Ldloc_1);//局部变量1 result
                    il.Emit(OpCodes.Ldloc_0);// 局部变量0 correlationState
                    il.Emit(OpCodes.Callvirt, _afterCallMethod);
                }
                //result
                if (!hasResult)
                {
                    il.Emit(OpCodes.Ret);
                    return;
                }
                il.Emit(OpCodes.Ldloc_1);//非void取出局部变量1 result
                if (methodInfo.ReturnType.IsValueType)
                {
                    il.Emit(OpCodes.Unbox_Any, methodInfo.ReturnType);//对值类型拆箱
                }
                il.Emit(OpCodes.Ret);
            }
        }

        private void BuildCtor(Type classType, FieldBuilder inspectorFieldBuilder, TypeBuilder typeBuilder)
        {

            ConstructorBuilder ctorBuilder = typeBuilder.DefineConstructor(
                MethodAttributes.Public, CallingConventions.HasThis, Type.EmptyTypes);

            ILGenerator il = ctorBuilder.GetILGenerator();

            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Call, classType.GetConstructor(Type.EmptyTypes));//调用base的默认ctor
            //il.Emit(OpCodes.Ldarg_0);
            ////将typeof(classType)压入计算堆
            //il.Emit(OpCodes.Ldtoken, classType);
            //il.Emit(OpCodes.Call, typeof(Type).GetMethod("GetTypeFromHandle", new Type[] { typeof(RuntimeTypeHandle) }));
            //调用DefaultInterceptorFactory.Create(type)
            LocalBuilder aopmessage=il.DeclareLocal(typeof(IInterceptor));
            il.Emit(OpCodes.Call, _getDefaultMethod);
            il.Emit(OpCodes.Ldarg_0);
            //将结果保存到字段_inspector
            il.Emit(OpCodes.Stfld, inspectorFieldBuilder);
            il.Emit(OpCodes.Ret);

        }
    }
}
