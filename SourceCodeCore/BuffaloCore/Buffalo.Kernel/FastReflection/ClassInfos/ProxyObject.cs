using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.IO;
namespace Buffalo.Kernel.FastReflection.ClassInfos
{
    public class ProxyObject : MarshalByRefObject,IDisposable
    {
        private ClassInfoHandle _classHandle = null;
        private object _instance;//ʵ��
        private AppDomain _app;
        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="filePath">�ļ���</param>
        /// <param name="TypeName">����</param>
        public ProxyObject(string filePath, string TypeName, string domainName, params object[] args)
        {
            Assembly assembly = null;
            if (!string.IsNullOrEmpty(domainName))
            {
                _app = AppDomain.CreateDomain(domainName);

                assembly = _app.Load(File.ReadAllBytes(filePath));
            }
            else 
            {
                 assembly = Assembly.LoadFrom(filePath);
            }
            Type tp = assembly.GetType(TypeName);
            Init(tp,args);
        }
        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="filePath">�ļ���</param>
        /// <param name="TypeName">����</param>
        public ProxyObject(string filePath, string TypeName, params object[] args)
            :this(filePath,TypeName,null,args)
        {
            
        }
        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="dllContent">�ļ�����</param>
        /// <param name="TypeName">����</param>
        public ProxyObject(byte[] dllContent, string TypeName,string domainName, params object[] args)
        {
            if (!string.IsNullOrEmpty(domainName))
            {
                _app = AppDomain.CreateDomain(domainName);
            }
            else 
            {
                _app = AppDomain.CreateDomain("domain" + TypeName);
            }
            Assembly assembly = _app.Load(dllContent);
            Type tp = assembly.GetType(TypeName);
            Init(tp, args);
            
        }
        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="dllContent">�ļ�����</param>
        /// <param name="TypeName">����</param>
        public ProxyObject(byte[] dllContent, string TypeName, params object[] args)
            :this(dllContent,TypeName,null,args)
        {

        }
        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="assembly">����</param>
        /// <param name="TypeName">����</param>
        public ProxyObject(Assembly assembly, string TypeName, params object[] args)
        {
            Type tp = assembly.GetType(TypeName);
            Init(tp, args);
        }
        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="tp">����</param>
        public ProxyObject(Type tp, params object[] args)
        {
            //Type tp = assembly.GetType(TypeName);
            Init(tp, args);
        }
        /// <summary>
        /// ��ʼ��
        /// </summary>
        /// <param name="assembly">����</param>
        /// <param name="TypeName">������</param>
        private void Init(Type tp, params object[] args) 
        {
            
            _classHandle = ClassInfoManager.GetClassHandle(tp);
            _instance = Activator.CreateInstance(tp,args);
        }

        /// <summary>
        /// ���иú���
        /// </summary>
        /// <param name="methodName">������</param>
        /// <param name="args">����</param>
        /// <returns></returns>
        public object Invoke(string methodName, params object[] args)
        {
            FastInvokeHandler fhandle = FastValueGetSet.GetCustomerMethodInfo(_classHandle.ClassType, methodName, GetParamTypes(args));
            return fhandle.Invoke(_instance, args);
        }

        /// <summary>
        /// ��ȡ����������
        /// </summary>
        /// <param name="args">��������</param>
        /// <returns></returns>
        private static Type[] GetParamTypes(object[] args) 
        {
            List<Type> lstType = new List<Type>(10);
            if (args != null) 
            {
                foreach (object obj in args) 
                {
                    lstType.Add(obj.GetType());
                }
            }
            return lstType.ToArray();
        }

        /// <summary>
        /// ���û��ȡ����
        /// </summary>
        /// <param name="propertyName">������</param>
        /// <returns></returns>
        public object this[string propertyName] 
        {
            get 
            {
                return _classHandle.PropertyInfo[propertyName].GetValue(_instance);
            }
            set 
            {
                 _classHandle.PropertyInfo[propertyName].SetValue(value,_instance);
            }
        }

        /// <summary>
        /// ������Ϣ
        /// </summary>
        public ClassInfoHandle ClassHandle
        {
            get { return _classHandle; }
            set { _classHandle = value; }
        }

        #region IDisposable ��Ա

        public void Dispose()
        {
            if (_app != null) 
            {
                AppDomain.Unload(_app);
            }
            GC.SuppressFinalize(this);
        }

        #endregion

        ~ProxyObject() 
        {
            Dispose();
        }
    }   
}
