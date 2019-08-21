using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using Buffalo.Kernel;
using System.Collections.Concurrent;
namespace Buffalo.Kernel.FastReflection
{
    public class FastValueGetSet
    {
        private static ConcurrentDictionary<string, FastInvokeHandler> dicMethod = new ConcurrentDictionary<string, FastInvokeHandler>();
        private static ConcurrentDictionary<string, PropertyInfoHandle> dicProperty = new ConcurrentDictionary<string, PropertyInfoHandle>();//���Ի���
        private static ConcurrentDictionary<string, CreateInstanceHandler> _invokerInstance = new ConcurrentDictionary<string, CreateInstanceHandler>();
        public const BindingFlags AllBindingFlags = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static | BindingFlags.SuppressChangeType | BindingFlags.Instance;
        /// <summary>
        /// ��ȡ���Ե���Ϣ
        /// </summary>
        /// <param name="proName">������</param>
        /// <param name="type">����</param>
        /// <returns></returns>
        public static PropertyInfoHandle GetPropertyInfoHandle(string proName, Type type)
        {
            string fullName = type.FullName + "." + proName;
            PropertyInfoHandle propertyHandle = null;

            if (!dicProperty.TryGetValue(fullName, out propertyHandle))
            {

                propertyHandle = GetPropertyInfoHandleWithOutCache(proName, type);
                dicProperty[fullName] = propertyHandle;

            }

            return propertyHandle;
        }


        /// <summary>
        /// ��ȡ���Ե���Ϣ(��ʹ�û���)
        /// </summary>
        /// <param name="proName">������</param>
        /// <param name="type">����</param>
        /// <returns></returns>
        public static PropertyInfoHandle GetPropertyInfoHandleWithOutCache(PropertyInfo pinfo)
        {
            Type classtype = pinfo.DeclaringType;
            FastPropertyHandler getHandle = null;
            MethodInfo method = pinfo.GetGetMethod();
            if (method != null)
            {
                getHandle = FastPropertyInvoke.GetMethodInvoker(method);
            }

            FastPropertyHandler setHandle = null;
           method = pinfo.GetSetMethod();
            if (method != null)
            {

                setHandle = FastPropertyInvoke.GetMethodInvoker(method);
            }
            PropertyInfoHandle propertyHandle = new PropertyInfoHandle(classtype, getHandle, setHandle, pinfo.PropertyType, pinfo.Name);
            return propertyHandle;
        }


        /// <summary>
        /// ��ȡ���Ե���Ϣ(��ʹ�û���)
        /// </summary>
        /// <param name="proName">������</param>
        /// <param name="type">����</param>
        /// <returns></returns>
        public static PropertyInfoHandle GetPropertyInfoHandleWithOutCache(string proName, Type type)
        {
            FastPropertyHandler getHandle = GetGetMethodInfo(proName, type);
            FastPropertyHandler setHandle = GetSetMethodInfo(proName, type);
            PropertyInfo pinf = type.GetProperty(proName, AllBindingFlags);//��ȡ��Ԫ�ؼ��ϵ�����
            Type proType = null;
            if (pinf != null)
            {
                proType = pinf.PropertyType;
            }
            PropertyInfoHandle propertyHandle = new PropertyInfoHandle(type, getHandle, setHandle, proType, proName);
            return propertyHandle;
        }

        /// <summary>
        /// ��ȡ��ȡֵ�ķ����ӿ�
        /// </summary>
        /// <param name="proName">������</param>
        /// <param name="type">����</param>
        /// <returns></returns>
        public static FastPropertyHandler GetGetMethodInfo(string proName, Type type)
        {
            MethodInfo methodInfo = type.GetMethod("get_" + proName,AllBindingFlags);
            if (methodInfo == null)
            {
                return null;
            }else if(methodInfo.GetParameters().Length>0)
            {
                return null;
            }
            FastPropertyHandler fastInvoker = FastPropertyInvoke.GetMethodInvoker(methodInfo);
            return fastInvoker;
        }


        /// <summary>
        /// ��ȡ����ֵ�ķ����ӿ�
        /// </summary>
        /// <param name="proName">������</param>
        /// <param name="type">����</param>
        /// <returns></returns>
        public static FastPropertyHandler GetSetMethodInfo(string proName, Type type)
        {
            MethodInfo methodInfo = type.GetMethod("set_" + proName, AllBindingFlags);
            if (methodInfo == null) 
            {
                return null;
            }
            else if (methodInfo.GetParameters().Length != 1)
            {
                return null;
            }
            FastPropertyHandler fastInvoker = FastPropertyInvoke.GetMethodInvoker(methodInfo);

            return fastInvoker;
        }

        /// <summary>
        /// ��ȡ�����ļ�
        /// </summary>
        /// <param name="methodInfo">��������</param>
        /// <param name="objectType">����</param>
        /// <returns></returns>
        internal static string GetMethodInfoKey(MemberInfo methodInfo) 
        {
            StringBuilder sbRet = new StringBuilder();
            sbRet.Append(methodInfo.DeclaringType.FullName);
            sbRet.Append(":");
            sbRet.Append(methodInfo.ToString());
            return sbRet.ToString();
        }

        /// <summary>
        /// ��ȡ�����͵�ָ��������ί��
        /// </summary>
        /// <param name="objectType">�����������</param>
        /// <param name="methodName">������</param>
        /// <param name="parametersType">�����б�</param>
        /// <returns></returns>
        public static FastInvokeHandler GetCustomerMethodInfo(Type objectType,string methodName,Type[] parametersType)
        {
            MethodInfo methodInfo = objectType.GetMethod(methodName, AllBindingFlags, null, parametersType, null);
            if (methodInfo != null)
            {
                return GetCustomerMethodInfo(methodInfo);
            }
            return null;
        }

        /// <summary>
        /// ��ȡ�����͵�ָ��������ί��
        /// </summary>
        /// <param name="methodInfo">������Ϣ</param>
        /// <param name="parametersType">�����б�</param>
        /// <returns></returns>
        public static FastInvokeHandler GetCustomerMethodInfo(MethodInfo methodInfo)
        {
            FastInvokeHandler fastInvokerHandle = null;
            string key = GetMethodInfoKey(methodInfo);
            if (dicMethod.TryGetValue(key, out fastInvokerHandle))
            {
                return fastInvokerHandle;
            }
            fastInvokerHandle = FastInvoke.GetMethodInvoker(methodInfo);
            dicMethod[key] = fastInvokerHandle;
            
            return fastInvokerHandle;
        }

        /// <summary>
        /// �Զ���ֵ
        /// </summary>
        /// <param name="args">����</param>
        /// <param name="value">ֵ</param>
        /// <param name="propertyName">������</param>
        /// <param name="objType">��������</param>
        public static void SetValue(object args, object value, string propertyName,Type objType) 
        {
            PropertyInfoHandle fastInvoker = GetPropertyInfoHandle(propertyName, objType);
            fastInvoker.SetValue(args,value);
        }

        /// <summary>
        /// ��ȡ����ֵ
        /// </summary>
        /// <param name="args">����</param>
        /// <param name="value">ֵ</param>
        /// <param name="propertyName">������</param>
        /// <param name="objType">��������</param>
        public static object GetValue(object args, string propertyName, Type objType)
        {
            PropertyInfoHandle fastInvoker = GetPropertyInfoHandle(propertyName, objType);
            return fastInvoker.GetValue(args);
        }

        
        /// <summary>
        /// ����ʵ����
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static object CreateObject(Type type)
        {
            CreateInstanceHandler create = GetCreateInstanceHandler(type);
            return create.Invoke();
        }

        /// <summary>
        /// �������Ͳ���ָ���Ĵ�������Ĵ���
        /// </summary>
        /// <param name="type">����</param> 
        /// <returns></returns> 
        public static CreateInstanceHandler GetCreateInstanceHandler(Type type)
        {
            CreateInstanceHandler create = null;
            string key = type.FullName;

            if (!_invokerInstance.TryGetValue(key, out create))
            {

                create = GetCreateInstanceHandlerWithOutCache(type);
                _invokerInstance[key] = create;

            }
            return create;
        }

        /// <summary>
        /// �������Ͳ���ָ���Ĵ�������Ĵ���
        /// </summary>
        /// <param name="type">����</param> 
        /// <returns></returns> 
        public static CreateInstanceHandler GetCreateInstanceHandlerWithOutCache(Type type)
        {
            CreateInstanceHandler create = null;
            create = FastInvoke.GetInstanceCreator(type);
            return create;
        }
    }
}
