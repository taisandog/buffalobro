using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using Buffalo.Kernel.Defaults;
using Library;

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
        /// 所属服务
        /// </summary>
        private AutoServicesRun _belongServices;
        /// <summary>
        /// 自动服务的容器
        /// </summary>
        /// <param name="message"></param>
        public ServicesLoader(AutoServicesRun belongServices)
        {
            _belongServices = belongServices;
        }
        /// <summary>
        /// 服务集合
        /// </summary>
        public Dictionary<string, AbsServicesHandle> Services
        {
            get { return _dicServices; }
        }
        
        private List<AbsServicesHandle> _lstHandle;
        /// <summary>
        /// 自动服务单元
        /// </summary>
        public List<AbsServicesHandle> ServicesHandles 
        {
            get
            {
                if (_lstHandle == null)
                {
                    _lstHandle = new List<AbsServicesHandle>();
                    foreach (KeyValuePair<string, AbsServicesHandle> kvp in _dicServices)
                    {
                        _lstHandle.Add(kvp.Value);
                    }
                    AbsServicesHandle tmp = null;
                    for (int i = 0; i < _lstHandle.Count - 1; i++) 
                    {
                        for (int j = i+1; j < _lstHandle.Count ; j++)
                        {
                            if (_lstHandle[i].Order < _lstHandle[j].Order) 
                            {
                                tmp = _lstHandle[i];
                                _lstHandle[i] = _lstHandle[j];
                                _lstHandle[j] = tmp;
                            }
                        }
                    }
                }
                return _lstHandle;
            }
        }

        /// <summary>
        /// 当抛出异常时候触发
        /// </summary>
        public event ThrowExceptionHandle OnThrowException;

        /// <summary>
        /// 运行自动服务
        /// </summary>
        public void DoTick()
        {
            //List<ServicesMessage> lstMessage = new List<ServicesMessage>(_dicServices.Count);
            List<AbsServicesHandle> lstHandles = ServicesHandles;
            IStatePanel _panel = _belongServices.StatePanel;
            foreach (AbsServicesHandle handle in lstHandles)
            {
                try
                {
                    if (_panel != null)
                    {
                        _panel.ShowState(handle.ServicesName,(int)ShowStateType.AutoTask);
                    }
                    ServicesMessage mess = handle.CheckRun();
                    if (_panel != null)
                    {
                        _panel.ShowState("", (int)ShowStateType.AutoTask);
                    }
                }
                catch (Exception ex)
                {
                    if (OnThrowException != null)
                    {
                        OnThrowException(handle, ex);
                    }
                }
            }
            //return lstMessage;
        }
        /// <summary>
        /// 清理所有异步线程
        /// </summary>
        public void ClearAnyc()
        {
            List<AbsServicesHandle> lstHandles = ServicesHandles;IStatePanel _panel = _belongServices.StatePanel;
            foreach (AbsServicesHandle handle in lstHandles)
            {
                try
                {
                    handle.ClearAnyc();
                }
                catch (Exception ex)
                {
                    if (OnThrowException != null)
                    {
                        OnThrowException(handle, ex);
                    }
                }
            }
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
            _lstHandle = null;
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
                handle.MessageShow = _belongServices.Message;
                AddServices(handle);
            }
        }
    }
}
