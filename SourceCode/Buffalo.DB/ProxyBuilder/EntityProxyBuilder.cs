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
 * @ԭ����:benben
 * @����ʱ��:2012-2-19 09:02
 * @����:http://www.189works.com/article-43203-1.html
 * @˵��:.NET IL��̬���������޸İ棩
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
        /// ������������
        /// </summary>
        long _count=0;
        /// <summary>
        /// �ӿ�����
        /// </summary>
        private readonly static Type[] _entityInterface = new Type[] { typeof(IEntityProxy) };

        /// <summary>
        /// ��������
        /// </summary>
        public EntityProxyBuilder() 
            :this("BuffaloProxy")
        {

        }
        /// <summary>
        /// ��������
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
        /// ���������
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
        /// ������
        /// </summary>
        /// <param name="classType"></param>
        /// <param name="moduleBuilder"></param>
        /// <returns></returns>
        private Type BulidType(Type classType, ModuleBuilder moduleBuilder,string className)
        {
            //��������
            TypeBuilder typeBuilder = moduleBuilder.DefineType(className,
                                                       TypeAttributes.Public | TypeAttributes.Sealed | TypeAttributes.Class,
                                                       classType, _entityInterface);
            
            ////�����ֶ� _inspector
            //FieldBuilder inspectorFieldBuilder = typeBuilder.DefineField("_inspector", typeof(IInterceptor),
            //                                                    FieldAttributes.Public | FieldAttributes.InitOnly);
            ////���캯��
            //BuildCtor(classType, typeBuilder);

            //���췽��
            BuildMethod(classType, typeBuilder);
            BuildGetEntityType(typeBuilder);
            Type aopType = typeBuilder.CreateType();
            return aopType;
        }

        static Type[] _getEntityTypeParameterTypes = new Type[] { };
        /// <summary>
        /// ������ȡʵ�����͵ķ���
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
        /// ������
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
                    method = _mapupdateMethod;//����ǹ������������OnMapPropertyUpdated
                }
                else 
                {
                    method = _updateMethod;//�����һ�����������OnPropertyUpdated֪ͨ
                }
                BuildEmit(classType, pInfo.BelongPropertyInfo, typeBuilder, method);
            }

            bool needLazy = entityInfo.GetNeedLazy();

            foreach (EntityMappingInfo mInfo in entityInfo.MappingInfo)
            {
                FieldInfo finfo = mInfo.BelongFieldInfo;
                if (mInfo.IsParent)
                {
                    
                    BuildEmit(classType, mInfo.BelongPropertyInfo, typeBuilder, _mapupdateMethod);//����set����
                    if (needLazy)
                    {
                        BuildMapEmit(classType, mInfo.BelongPropertyInfo, finfo, typeBuilder, _fillParent);//����get����
                    }
                }
                else if (needLazy)
                {
                    BuildMapEmit(classType, mInfo.BelongPropertyInfo, finfo, typeBuilder, _fillChildMethod);//����get����
                }

            }

           
        }

       

        /// <summary>
        /// ����IL
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
                throw new Exception("�����:" + classType .FullName+ " ������:" + propertyInfo.Name + " ����Ϊvirtual");
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

            LocalBuilder retVal=il.DeclareLocal(typeof(object)); //result ����Ϊ0
            //Call methodInfo
            il.Emit(OpCodes.Ldarg_0);
            for (int i = 0; i < parameterLength; i++)
            {
                il.Emit(OpCodes.Ldarg_S, (i + 1));//���صڼ�������

            }
            il.Emit(OpCodes.Call, methodInfo);//base.����();  ���������Callvirt�������ѭ�����ñ�����
            //������ֵѹ�� �ֲ�����1result void��ѹ��null
            if (!hasResult)
            {
                il.Emit(OpCodes.Ldnull);
            }
            else if (methodInfo.ReturnType.IsValueType)
            {
                il.Emit(OpCodes.Box, methodInfo.ReturnType);//��ֵ����װ��
            }

            il.Emit(OpCodes.Stloc, retVal);//����ֵ���浽retVal

            //callupdateMethod
            il.Emit(OpCodes.Ldarg_0);//this
            il.Emit(OpCodes.Ldstr, propertyInfo.Name);//����propertyName

            il.Emit(OpCodes.Callvirt, updateMethod);//����updateMethod

            //result
            if (hasResult)
            {
                il.Emit(OpCodes.Ldloc, retVal);//��voidȡ���ֲ�����1 result
                
                if (methodInfo.ReturnType.IsValueType)
                {
                    il.Emit(OpCodes.Unbox_Any, methodInfo.ReturnType);//��ֵ���Ͳ���
                }
            }
            il.Emit(OpCodes.Ret);
        }

        /// <summary>
        /// ����IL
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
                throw new Exception("�������:" + propertyInfo.Name + "����Ϊvirtual");
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

            LocalBuilder result = il.DeclareLocal(typeof(object)); //result ����Ϊ0
            //if(�ֶ�==null){������Ϣ}

            //if (finfo.IsFamily || finfo.IsPublic)
            //{
            //    Label falseLabel = il.DefineLabel();//��Ϊnullʱ�����ת��ǩ
            //    il.Emit(OpCodes.Ldarg_0);//this

            //    il.Emit(OpCodes.Ldfld, finfo);//��ȡ�ֶ�ֵ
            //    il.Emit(OpCodes.Ldnull);//��null�ŵ��ڶ���λ��
            //    il.Emit(OpCodes.Ceq);//�Ƚ����(����򷵻�1��������򷵻�0)
            //    il.Emit(OpCodes.Ldc_I4_0);//����ֵ0���͵�ջ
            //    il.Emit(OpCodes.Ceq);//�Ƚ����(����򷵻�1��������򷵻�0)
            //    il.Emit(OpCodes.Brtrue_S, falseLabel);
            //    //������亯��
            //    il.Emit(OpCodes.Ldarg_0);//this
            //    il.Emit(OpCodes.Ldstr, propertyInfo.Name);//����propertyName
            //    il.Emit(OpCodes.Callvirt, updateMethod);//����updateMethod
            //    il.MarkLabel(falseLabel);
            //}
            //else 
            //{
            //    //������亯��
            //    il.Emit(OpCodes.Ldarg_0);//this
            //    il.Emit(OpCodes.Ldstr, propertyInfo.Name);//����propertyName
            //    il.Emit(OpCodes.Callvirt, updateMethod);//����updateMethod
            //}

            
            //if(base.����==null){��������}
            Label falseLabel = il.DefineLabel();//��Ϊnullʱ�����ת��ǩ
            il.Emit(OpCodes.Ldarg_0);//this

            il.Emit(OpCodes.Call, methodInfo);
            il.Emit(OpCodes.Ldnull);//��null�ŵ��ڶ���λ��
            il.Emit(OpCodes.Ceq);//�Ƚ����(����򷵻�1��������򷵻�0)
            il.Emit(OpCodes.Ldc_I4_0);//����ֵ0���͵�ջ
            il.Emit(OpCodes.Ceq);//�Ƚ����(����򷵻�1��������򷵻�0)
            il.Emit(OpCodes.Brtrue_S, falseLabel);
            //������亯��
            il.Emit(OpCodes.Ldarg_0);//this
            il.Emit(OpCodes.Ldstr, propertyInfo.Name);//����propertyName
            il.Emit(OpCodes.Callvirt, updateMethod);//����updateMethod
            il.MarkLabel(falseLabel);


            //Call methodInfo
            il.Emit(OpCodes.Ldarg_0);
            for (int i = 0; i < parameterLength; i++)
            {
                il.Emit(OpCodes.Ldarg_S, (i + 1));//���صڼ�������
            }
            il.Emit(OpCodes.Call, methodInfo);
            //������ֵѹ�� �ֲ�����1result void��ѹ��null
            if (!hasResult)
            {
                il.Emit(OpCodes.Ldnull);
            }
            else if (methodInfo.ReturnType.IsValueType)
            {
                il.Emit(OpCodes.Box, methodInfo.ReturnType);//��ֵ����װ��
            }

            il.Emit(OpCodes.Stloc, result);



            //result
            if (hasResult)
            {
                il.Emit(OpCodes.Ldloc, result);//��voidȡ���ֲ�����1 result
                if (methodInfo.ReturnType.IsValueType)
                {
                    il.Emit(OpCodes.Unbox_Any, methodInfo.ReturnType);//��ֵ���Ͳ���
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
            il.Emit(OpCodes.Call, classType.GetConstructor(Type.EmptyTypes));//����base��Ĭ��ctor
            il.Emit(OpCodes.Ret);

        }
    }
}

//*************��������ʵ��������һ����ʵ������࣬���ɺ���������´���*****************

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