using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DB.EntityInfos;
using System.Reflection;
using System.Reflection.Emit;
using Buffalo.Kernel.FastReflection;
using Buffalo.Kernel.Defaults;
using Buffalo.Kernel;
using Buffalo.DB.CommBase;

/** 
 * @原作者:benben
 * @创建时间:2012-2-19 09:02
 * @链接:http://www.189works.com/article-43203-1.html
 * @说明:.NET IL动态代理创建（修改版）
*/

namespace Buffalo.DB.ProxyBuilder
{
    public class EntityProxyBuilder
    {
        
        AssemblyName _assemblyName ;
        AssemblyBuilder _assemblyBuilder;
        ModuleBuilder _moduleBuilder;
        string _pnamespace = null;
        MethodInfo _updateMethod = null;
        MethodInfo _mapupdateMethod = null;
        MethodInfo _fillChildMethod = null;
        MethodInfo _fillParent = null;
        MethodInfo _getTypeMethod=null;
        MethodInfo _getBaseTypeMethod = null;
        /// <summary>
        /// 构建的类数量
        /// </summary>
        long _count=0;
        /// <summary>
        /// 接口类型
        /// </summary>
        private readonly static Type[] _entityInterface = new Type[] { typeof(IEntityProxy) };

        /// <summary>
        /// 代理建造类
        /// </summary>
        public EntityProxyBuilder() 
            :this("BuffaloProxy")
        {

        }
        /// <summary>
        /// 代理建造类
        /// </summary>
        public EntityProxyBuilder(string proxyNamespace)
        {
            _pnamespace = proxyNamespace; 
            _assemblyName = new AssemblyName(_pnamespace);
            _assemblyBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(_assemblyName,
                                                                            AssemblyBuilderAccess.RunAndSave);
            _moduleBuilder = _assemblyBuilder.DefineDynamicModule(_pnamespace);

            Type classType = typeof(EntityBase);
            Type objectType = typeof(object);
            Type typeType = typeof(Type);
            _updateMethod = classType.GetMethod("OnPropertyUpdated", FastValueGetSet.AllBindingFlags);
            _mapupdateMethod = classType.GetMethod("OnMapPropertyUpdated", FastValueGetSet.AllBindingFlags);
            _fillChildMethod = classType.GetMethod("FillChild", FastValueGetSet.AllBindingFlags);
            _fillParent = classType.GetMethod("FillParent", FastValueGetSet.AllBindingFlags);
            _getTypeMethod = objectType.GetMethod("GetType", FastValueGetSet.AllBindingFlags);
            _getBaseTypeMethod = typeType.GetMethod("get_BaseType", FastValueGetSet.AllBindingFlags);
        }

        /// <summary>
        /// 建造代理类
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public Type CreateProxyType(Type classType)
        {

            //string name = classType.Namespace + ".ProxyClass";
            _count++;
            string className = _pnamespace + "." + classType.Name + "_" + _count.ToString("X5");

            Type aopType = BulidType(classType, _moduleBuilder, className);
            
            //_assemblyBuilder.Save("bac.dll");
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
            //定义类型
            TypeBuilder typeBuilder = moduleBuilder.DefineType(className,
                                                       TypeAttributes.Public | TypeAttributes.Sealed | TypeAttributes.Class,
                                                       classType, _entityInterface);
            
            ////定义字段 _inspector
            //FieldBuilder inspectorFieldBuilder = typeBuilder.DefineField("_inspector", typeof(IInterceptor),
            //                                                    FieldAttributes.Public | FieldAttributes.InitOnly);
            ////构造函数
            //BuildCtor(classType, typeBuilder);

            //构造方法
            BuildMethod(classType, typeBuilder);
            BuildGetEntityType(typeBuilder);
            Type aopType = typeBuilder.CreateType();
            return aopType;
        }

        static Type[] _getEntityTypeParameterTypes = new Type[] { };
        /// <summary>
        /// 创建获取实体类型的方法
        /// </summary>
        /// <param name="typeBuilder"></param>
        private void BuildGetEntityType(TypeBuilder typeBuilder) 
        {
            
            MethodBuilder methodBuilder = typeBuilder.DefineMethod("GetEntityType",
                                                         MethodAttributes.Public | MethodAttributes.Virtual
                                                         , typeof(Type)
                                                         , _getEntityTypeParameterTypes);
            ILGenerator il = methodBuilder.GetILGenerator();
            LocalBuilder retVal = il.DeclareLocal(typeof(Type)); //result
            il.Emit(OpCodes.Ldarg_0);//this
            il.Emit(OpCodes.Call, _getTypeMethod);
            il.Emit(OpCodes.Callvirt, _getBaseTypeMethod);
            //il.Emit(OpCodes.Stloc, retVal);
            //il.Emit(OpCodes.Ldloc, retVal);
            il.Emit(OpCodes.Ret);
        }

        /// <summary>
        /// 建方法
        /// </summary>
        /// <param name="classType"></param>
        /// <param name="inspectorFieldBuilder"></param>
        /// <param name="typeBuilder"></param>
        private void BuildMethod(Type classType,  TypeBuilder typeBuilder)
        {
            EntityInfoHandle entityInfo = EntityInfoManager.GetEntityHandle(classType);
            MethodInfo method = null;
            foreach (EntityPropertyInfo pInfo in entityInfo.PropertyInfo) 
            {
                UpdatePropertyInfo updateInfo = entityInfo.GetUpdatePropertyInfo(pInfo.PropertyName);
                if (updateInfo != null)
                {
                    method = _mapupdateMethod;//如果是关联属性则调用OnMapPropertyUpdated
                }
                else 
                {
                    method = _updateMethod;//如果是一般属性则调用OnPropertyUpdated通知
                }
                BuildEmit(classType, pInfo.BelongPropertyInfo, typeBuilder, method);
            }

            bool needLazy = entityInfo.GetNeedLazy();

            foreach (EntityMappingInfo mInfo in entityInfo.MappingInfo)
            {
                FieldInfo finfo = mInfo.BelongFieldInfo;
                if (mInfo.IsParent)
                {
                    
                    BuildEmit(classType, mInfo.BelongPropertyInfo, typeBuilder, _mapupdateMethod);//创建set方法
                    if (needLazy)
                    {
                        BuildMapEmit(classType, mInfo.BelongPropertyInfo, finfo, typeBuilder, _fillParent);//创建get方法
                    }
                }
                else if (needLazy)
                {
                    BuildMapEmit(classType, mInfo.BelongPropertyInfo, finfo, typeBuilder, _fillChildMethod);//创建get方法
                }

            }

           
        }

       

        /// <summary>
        /// 创建IL
        /// </summary>
        /// <param name="classType"></param>
        /// <param name="pInfo"></param>
        /// <param name="typeBuilder"></param>
        /// <param name="updateMethod"></param>
        /// <param name="methodName"></param>
        private void BuildEmit(Type classType,PropertyInfo propertyInfo,
            TypeBuilder typeBuilder, MethodInfo updateMethod)
        {

            MethodInfo methodInfo = propertyInfo.GetSetMethod(true);
            if (!methodInfo.IsVirtual && !methodInfo.IsAbstract)
            {
                throw new Exception("请把类:" + classType .FullName+ " 的属性:" + propertyInfo.Name + " 设置为virtual");
            }

           
            ParameterInfo[] parameterInfos = methodInfo.GetParameters();
            List<Type> lstType = new List<Type>(parameterInfos.Length);
            foreach (ParameterInfo info in parameterInfos)
            {
                lstType.Add(info.ParameterType);
            }

            Type[] parameterTypes = lstType.ToArray();
            int parameterLength = parameterTypes.Length;
            bool hasResult = methodInfo.ReturnType !=FastInvoke.VoidType;

            MethodBuilder methodBuilder = typeBuilder.DefineMethod(methodInfo.Name,
                                                         MethodAttributes.Public |
                                                         MethodAttributes.Virtual
                                                         , methodInfo.ReturnType
                                                         , parameterTypes);
            
            ILGenerator il = methodBuilder.GetILGenerator();

            LocalBuilder retVal=il.DeclareLocal(typeof(object)); //result 索引为0
            //Call methodInfo
            il.Emit(OpCodes.Ldarg_0);
            for (int i = 0; i < parameterLength; i++)
            {
                il.Emit(OpCodes.Ldarg_S, (i + 1));//加载第几个参数

            }
            il.Emit(OpCodes.Call, methodInfo);//base.方法();  这里如果用Callvirt则会无限循环调用本函数
            //将返回值压入 局部变量1result void就压入null
            if (!hasResult)
            {
                il.Emit(OpCodes.Ldnull);
            }
            else if (methodInfo.ReturnType.IsValueType)
            {
                il.Emit(OpCodes.Box, methodInfo.ReturnType);//对值类型装箱
            }

            il.Emit(OpCodes.Stloc, retVal);//返回值保存到retVal

            //callupdateMethod
            il.Emit(OpCodes.Ldarg_0);//this
            il.Emit(OpCodes.Ldstr, propertyInfo.Name);//参数propertyName

            il.Emit(OpCodes.Callvirt, updateMethod);//调用updateMethod

            //result
            if (hasResult)
            {
                il.Emit(OpCodes.Ldloc, retVal);//非void取出局部变量1 result
                
                if (methodInfo.ReturnType.IsValueType)
                {
                    il.Emit(OpCodes.Unbox_Any, methodInfo.ReturnType);//对值类型拆箱
                }
            }
            il.Emit(OpCodes.Ret);
        }

        /// <summary>
        /// 创建IL
        /// </summary>
        /// <param name="classType"></param>
        /// <param name="pInfo"></param>
        /// <param name="typeBuilder"></param>
        /// <param name="updateMethod"></param>
        /// <param name="methodName"></param>
        private void BuildMapEmit(Type classType, PropertyInfo propertyInfo, FieldInfo finfo,
            TypeBuilder typeBuilder, MethodInfo updateMethod)
        {
            MethodInfo methodInfo = propertyInfo.GetGetMethod(true);
            if (!methodInfo.IsVirtual && !methodInfo.IsAbstract)
            {
                throw new Exception("请把属性:" + propertyInfo.Name + "设置为virtual");
                return;
            }


            ParameterInfo[] parameterInfos = methodInfo.GetParameters();
            List<Type> lstType = new List<Type>(parameterInfos.Length);
            foreach (ParameterInfo info in parameterInfos)
            {
                lstType.Add(info.ParameterType);
            }

            Type[] parameterTypes = lstType.ToArray();
            int parameterLength = parameterTypes.Length;
            bool hasResult = methodInfo.ReturnType != FastInvoke.VoidType;

            MethodBuilder methodBuilder = typeBuilder.DefineMethod(methodInfo.Name,
                                                         MethodAttributes.Public |
                                                         MethodAttributes.Virtual
                                                         , methodInfo.ReturnType
                                                         , parameterTypes);

            ILGenerator il = methodBuilder.GetILGenerator();

            LocalBuilder result = il.DeclareLocal(typeof(object)); //result 索引为0
            //if(字段==null){加载信息}

            //if (finfo.IsFamily || finfo.IsPublic)
            //{
            //    Label falseLabel = il.DefineLabel();//不为null时候的跳转标签
            //    il.Emit(OpCodes.Ldarg_0);//this

            //    il.Emit(OpCodes.Ldfld, finfo);//获取字段值
            //    il.Emit(OpCodes.Ldnull);//把null放到第二个位置
            //    il.Emit(OpCodes.Ceq);//比较相等(相等则返回1，不想等则返回0)
            //    il.Emit(OpCodes.Ldc_I4_0);//把数值0推送到栈
            //    il.Emit(OpCodes.Ceq);//比较相等(相等则返回1，不想等则返回0)
            //    il.Emit(OpCodes.Brtrue_S, falseLabel);
            //    //调用填充函数
            //    il.Emit(OpCodes.Ldarg_0);//this
            //    il.Emit(OpCodes.Ldstr, propertyInfo.Name);//参数propertyName
            //    il.Emit(OpCodes.Callvirt, updateMethod);//调用updateMethod
            //    il.MarkLabel(falseLabel);
            //}
            //else 
            //{
            //    //调用填充函数
            //    il.Emit(OpCodes.Ldarg_0);//this
            //    il.Emit(OpCodes.Ldstr, propertyInfo.Name);//参数propertyName
            //    il.Emit(OpCodes.Callvirt, updateMethod);//调用updateMethod
            //}

            
            //if(base.属性==null){加载数据}
            Label falseLabel = il.DefineLabel();//不为null时候的跳转标签
            il.Emit(OpCodes.Ldarg_0);//this

            il.Emit(OpCodes.Call, methodInfo);
            il.Emit(OpCodes.Ldnull);//把null放到第二个位置
            il.Emit(OpCodes.Ceq);//比较相等(相等则返回1，不想等则返回0)
            il.Emit(OpCodes.Ldc_I4_0);//把数值0推送到栈
            il.Emit(OpCodes.Ceq);//比较相等(相等则返回1，不想等则返回0)
            il.Emit(OpCodes.Brtrue_S, falseLabel);
            //调用填充函数
            il.Emit(OpCodes.Ldarg_0);//this
            il.Emit(OpCodes.Ldstr, propertyInfo.Name);//参数propertyName
            il.Emit(OpCodes.Callvirt, updateMethod);//调用updateMethod
            il.MarkLabel(falseLabel);


            //Call methodInfo
            il.Emit(OpCodes.Ldarg_0);
            for (int i = 0; i < parameterLength; i++)
            {
                il.Emit(OpCodes.Ldarg_S, (i + 1));//加载第几个参数
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

            il.Emit(OpCodes.Stloc, result);



            //result
            if (hasResult)
            {
                il.Emit(OpCodes.Ldloc, result);//非void取出局部变量1 result
                if (methodInfo.ReturnType.IsValueType)
                {
                    il.Emit(OpCodes.Unbox_Any, methodInfo.ReturnType);//对值类型拆箱
                }
            }
            il.Emit(OpCodes.Ret);
        }

        

        private void BuildCtor(Type classType,  TypeBuilder typeBuilder)
        {

            ConstructorBuilder ctorBuilder = typeBuilder.DefineConstructor(
                MethodAttributes.Public, CallingConventions.HasThis, Type.EmptyTypes);

            ILGenerator il = ctorBuilder.GetILGenerator();

            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Call, classType.GetConstructor(Type.EmptyTypes));//调用base的默认ctor
            il.Emit(OpCodes.Ret);

        }
    }
}

//*************代理类其实就是生成一个该实体的子类，生成后的子类如下代码*****************

//namespace BuffaloProxyBuilder
//{
//    public class User8765493877954906FE4576:User
//    {
//        public override string Name
//        {
//            get 
//            {
//                return base.Name;
//            }
//            set 
//            {
//                base.Name = value;
//                OnPropertyUpdated("Name");
//            }
//        }

//        public override ScClass BelongClass
//        {
//            get
//            {
//                if (base.BelongClass == null) 
//                {
//                    FillParent("BelongClass");
//                }
//                return base.BelongClass;
//            }
//            set
//            {
//                base.BelongClass = value;
//                OnMapPropertyUpdated("BelongClass");
//            }
//        }

//        public override List<ScScore> LstScore
//        {
//            get 
//            {
//                if (base.LstScore == null)
//                {
//                    FillChild("LstScore");
//                }
//                return base.LstScore;
//            }
//            set 
//            {
//                base.LstScore = value;
//            }
//        }
//    }
//}