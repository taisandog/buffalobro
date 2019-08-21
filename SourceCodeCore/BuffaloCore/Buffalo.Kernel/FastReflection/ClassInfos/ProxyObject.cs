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
        private object _instance;//实例
        private AppDomain _app;
        /// <summary>
        /// 加载类型
        /// </summary>
        /// <param name="filePath">文件名</param>
        /// <param name="TypeName">类名</param>
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
        /// 加载类型
        /// </summary>
        /// <param name="filePath">文件名</param>
        /// <param name="TypeName">类名</param>
        public ProxyObject(string filePath, string TypeName, params object[] args)
            :this(filePath,TypeName,null,args)
        {
            
        }
        /// <summary>
        /// 加载类型
        /// </summary>
        /// <param name="dllContent">文件内容</param>
        /// <param name="TypeName">类名</param>
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
        /// 加载类型
        /// </summary>
        /// <param name="dllContent">文件内容</param>
        /// <param name="TypeName">类名</param>
        public ProxyObject(byte[] dllContent, string TypeName, params object[] args)
            :this(dllContent,TypeName,null,args)
        {

        }
        /// <summary>
        /// 加载类型
        /// </summary>
        /// <param name="assembly">程序集</param>
        /// <param name="TypeName">类名</param>
        public ProxyObject(Assembly assembly, string TypeName, params object[] args)
        {
            Type tp = assembly.GetType(TypeName);
            Init(tp, args);
        }
        /// <summary>
        /// 加载类型
        /// </summary>
        /// <param name="tp">类型</param>
        public ProxyObject(Type tp, params object[] args)
        {
            //Type tp = assembly.GetType(TypeName);
            Init(tp, args);
        }
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="assembly">程序集</param>
        /// <param name="TypeName">类型名</param>
        private void Init(Type tp, params object[] args) 
        {
            
            _classHandle = ClassInfoManager.GetClassHandle(tp);
            _instance = Activator.CreateInstance(tp,args);
        }

        /// <summary>
        /// 运行该函数
        /// </summary>
        /// <param name="methodName">函数名</param>
        /// <param name="args">参数</param>
        /// <returns></returns>
        public object Invoke(string methodName, params object[] args)
        {
            FastInvokeHandler fhandle = FastValueGetSet.GetCustomerMethodInfo(_classHandle.ClassType, methodName, GetParamTypes(args));
            return fhandle.Invoke(_instance, args);
        }

        /// <summary>
        /// 获取参数的类型
        /// </summary>
        /// <param name="args">参数类型</param>
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
        /// 设置或获取属性
        /// </summary>
        /// <param name="propertyName">属性名</param>
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
        /// 类型信息
        /// </summary>
        public ClassInfoHandle ClassHandle
        {
            get { return _classHandle; }
            set { _classHandle = value; }
        }

        #region IDisposable 成员

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
