using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Reflection.Emit;
using Buffalo.Kernel.FastReflection;
using Buffalo.Kernel.Defaults;
using Buffalo.Kernel;
using MongoDB.Bson.Serialization.Attributes;
using System.Threading;
using MongoDB.Bson.Serialization;

/** 
 * @原作者:benben
 * @创建时间:2012-2-19 09:02
 * @链接:http://www.189works.com/article-43203-1.html
 * @说明:.NET IL动态代理创建（修改版）
*/

namespace Buffalo.MongoDB.ProxyBase
{
    /// <summary>
    /// Mongo实体代理建造
    /// </summary>
    public class MongoEntityProxyBuilder
    {
        
        private AssemblyName _assemblyName ;
        private AssemblyBuilder _assemblyBuilder;
        private ModuleBuilder _moduleBuilder;
        string _pnamespace = null;
        private MethodInfo _updateMethod = null;
        /// <summary>
        /// 构建的类数量
        /// </summary>
        long _count=0;
        /// <summary>
        /// 程序集名
        /// </summary>
        public AssemblyName ProxyAssemblyName
        {
            get
            {
                return _assemblyName;
            }
        }

       

        /// <summary>
        /// 代理建造类
        /// </summary>
        public MongoEntityProxyBuilder() 
            :this("Buffalo.MongoDB.ProxyEntity")
        {

        }
        /// <summary>
        /// 代理建造类
        /// </summary>
        public MongoEntityProxyBuilder(string proxyNamespace)
        {
            _pnamespace = proxyNamespace; 
            _assemblyName = new AssemblyName(_pnamespace);
            _assemblyBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(_assemblyName,
                                                                            AssemblyBuilderAccess.RunAndSave);
            _moduleBuilder = _assemblyBuilder.DefineDynamicModule(_pnamespace);

            Type classType = typeof(MongoEntityBase);
            Type objectType = typeof(object);
            Type typeType = typeof(Type);
            _updateMethod = classType.GetMethod("OnPropertyUpdated", FastValueGetSet.AllBindingFlags);
            if (_updateMethod == null)
            {
                throw new NotSupportedException("实体必须继承MongoEntityBase");
            }
        }

        /// <summary>
        /// 建造代理类
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public Type CreateProxyType(Type classType)
        {
            _count++;
            //string name = classType.Namespace + ".ProxyClass";
            string className = _pnamespace + "." + classType.Name+ "_" + _count.ToString("X5"); ;
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
            
           //定义类型
           TypeBuilder typeBuilder = moduleBuilder.DefineType(className,
                                                       TypeAttributes.Public | TypeAttributes.Sealed | TypeAttributes.Class,
                                                       classType);

            //AddDiscriminator(typeBuilder, classType);
            //构造方法
            BuildMethod(classType, typeBuilder);
            
            Type aopType = typeBuilder.CreateType();
            //BsonSerializer.RegisterDiscriminator(aopType, classType.Name);
            return aopType;
        }

        private static ConstructorInfo _constructor = null;
        private static PropertyInfo[] _pInfos = null;
        /// <summary>
        /// 添加鉴别器的声明
        /// </summary>
        /// <param name="typeBuilder"></param>
        /// <param name="classType"></param>
        private void AddDiscriminator(TypeBuilder typeBuilder, Type classType)
        {
            string name = classType.Name;
            bool required = false;
            bool rootClass = false;
            Type attType = typeof(BsonDiscriminatorAttribute);
            BsonDiscriminatorAttribute att=classType.GetCustomAttribute(attType,true) as BsonDiscriminatorAttribute;
            if (att != null)
            {
                if (!string.IsNullOrWhiteSpace(att.Discriminator))
                {
                    name = att.Discriminator;
                }
                required = att.Required;
            }

            if (_constructor == null)
            {
                ConstructorInfo[] arrconstructor = attType.GetConstructors();
                foreach (ConstructorInfo info in arrconstructor)
                {
                    ParameterInfo[] pinfos = info.GetParameters();
                    if (pinfos.Length == 1 && pinfos[0].ParameterType == typeof(string))
                    {
                        _constructor = info;
                        break;
                    }
                }
            }
            if (_pInfos == null)
            {
                PropertyInfo pinfo1 = attType.GetProperty("Required");
                PropertyInfo pinfo2 = attType.GetProperty("RootClass");
                _pInfos = new PropertyInfo[] { pinfo1, pinfo2 };
            }

            CustomAttributeBuilder customAttributeBuilder = new CustomAttributeBuilder(
                _constructor, new object[] { name },
                _pInfos,new object[] { required , rootClass });
            
            typeBuilder.SetCustomAttribute(customAttributeBuilder);
            
        }

        static Type[] _getEntityTypeParameterTypes = new Type[] { };


        /// <summary>
        /// 建方法
        /// </summary>
        /// <param name="classType"></param>
        /// <param name="inspectorFieldBuilder"></param>
        /// <param name="typeBuilder"></param>
        private void BuildMethod(Type classType,  TypeBuilder typeBuilder)
        {
            PropertyInfo[] infos = classType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            MethodInfo method = null;
            foreach (PropertyInfo pInfo in infos)
            {
                if (pInfo.GetMethod == null || pInfo.SetMethod==null)
                {
                    continue;
                }
                MethodInfo metinfo = pInfo.GetMethod;
                if (metinfo == null)
                {
                    continue;
                }
                ParameterInfo[] mpinfos = metinfo.GetParameters();
                if (mpinfos != null && mpinfos.Length > 0) //带参数的属性，例如this[arg]
                {
                    continue;
                }

                BsonIgnoreAttribute ignore = pInfo.GetCustomAttribute(typeof(BsonIgnoreAttribute)) as BsonIgnoreAttribute;
                if (ignore != null)//忽略属性
                {
                    continue;
                }


                method = _updateMethod;//如果是一般属性则调用OnPropertyUpdated通知

                BuildEmit(classType, pInfo, typeBuilder, method);
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