using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using Buffalo.Kernel.Defaults;

namespace Buffalo.Kernel.AutoServicesLib
{
    /// <summary>
    /// 异常触发事件
    /// </summary>
    /// <param name="ex"></param>
    public delegate void ThrowExceptionHandle(AbsServicesHandle handle, Exception ex);


    /// <summary>
    /// 自动服务的容器
    /// </summary>
    public class ServicesLoader
    {
        Dictionary<string, AbsServicesHandle> _dicServices = new Dictionary<string, AbsServicesHandle>();

        /// <summary>
        /// 服务集合
        /// </summary>
        public Dictionary<string, AbsServicesHandle> Services
        {
            get { return _dicServices; }
        }

        /// <summary>
        /// 当抛出异常时候触发
        /// </summary>
        public event ThrowExceptionHandle OnThrowException;

        /// <summary>
        /// 运行自动服务
        /// </summary>
        public List<ServicesMessage> DoTick() 
        {
            List<ServicesMessage> lstMessage = new List<ServicesMessage>(_dicServices.Count);
            foreach (KeyValuePair<string, AbsServicesHandle> kvp in _dicServices) 
            {
                try
                {
                    ServicesMessage mess=kvp.Value.CheckRun();
                    if (mess != null) 
                    {
                        lstMessage.Add(mess);
                    }
                }
                catch (Exception ex) 
                {
                    if (OnThrowException != null) 
                    {
                        OnThrowException(kvp.Value, ex);
                    }
                }
            }
            return lstMessage;
        }

        /// <summary>
        /// 添加服务
        /// </summary>
        /// <param name="handle"></param>
        public void AddServices(AbsServicesHandle handle) 
        {
            string key = handle.ServicesID;
            AbsServicesHandle outHandle = null;
            if (_dicServices.TryGetValue(key, out outHandle))
            {
                if (outHandle.SerVersion < handle.SerVersion)
                {
                    _dicServices[key] = handle;
                }
            }
            else
            {
                _dicServices[key] = handle;
            }
        }

        /// <summary>
        /// 加载程序集
        /// </summary>
        /// <param name="ass"></param>
        public void LoadAssembly(Assembly ass)
        {
            Type inteType=typeof(AbsServicesHandle);
            Type[] classes=ass.GetTypes();
            foreach(Type objType in classes)
            {
                if(!objType.IsClass)
                {
                    continue;
                }
                if(!DefaultType.IsInherit(objType,inteType))
                {
                    continue;
                }
                AbsServicesHandle handle=Activator.CreateInstance(objType) as AbsServicesHandle;
                if(handle==null)
                {
                    continue;
                }
                AddServices(handle);
            }
        }
    }
}
