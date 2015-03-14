using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceProcess;
using Microsoft.Win32;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using Buffalo.Win32Kernel.Win32;
using Buffalo.Kernel.Win32;

namespace Buffalo.Win32Kernel.ServicesUnit
{
    public class ServicesManager
    {
        private static Dictionary<string, ServiceController> _dicServices;


        private ServicesManager() 
        {
            
        }



        /// <summary>
        /// 获取服务集合
        /// </summary>
        private static void BindService()
        {
            ServiceController[] lstControl = ServiceController.GetServices();
            _dicServices = new Dictionary<string, ServiceController>();
            foreach (ServiceController sc in lstControl) 
            {
                _dicServices[sc.ServiceName] = sc;
            }
        }

        /// <summary>
        /// 获取服务
        /// </summary>
        /// <param name="servicesName"></param>
        /// <returns></returns>
        public static ServiceController GetController(string servicesName)
        {

            if (_dicServices == null)
            {
                BindService();
            }
            ServiceController ret = null;
            _dicServices.TryGetValue(servicesName, out ret);
            return ret;

        }

        /// <summary>
        /// 刷新服务缓存
        /// </summary>
        public static void RefreashServices() 
        {
            _dicServices = null;
        }
        
        /// <summary>
        /// 创建服务
        /// </summary>
        /// <param name="servicesName"></param>
        /// <returns></returns>
        public static string InstallServices(ServiceInfo info) 
        {
            IntPtr scmhandle = WindowsAPI.OpenSCManager(null, null, ControlManagerAccessTypes.SC_MANAGER_CREATE_SERVICE);
            if (scmhandle == IntPtr.Zero) 
            {
                return "获取信息失败";
            }
            int lpdwTagId = 0;
            IntPtr schandle = WindowsAPI.CreateService(scmhandle, info.Name, info.DisplayName, 
                ServicesAccessType.SERVICE_ALL_ACCESS,ServiceType.Win32OwnProcess,
                info.StarType, ServicesErrorControl.SERVICE_ERROR_NORMAL,
                info.Path,null, lpdwTagId, null, null, null);
            WindowsAPI.CloseServiceHandle(scmhandle);
            if (schandle == IntPtr.Zero) 
            {
                return "创建服务失败";
            }

            WindowsAPI.CloseServiceHandle(schandle);
            RefreashServices();
            return SetDescription(info); ;
        }
        /// <summary>
        /// 设置注释
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        private static string SetDescription(ServiceInfo info) 
        {

            RegistryKey scReg = GetServicesKey(info.Name);
            if (scReg != null) 
            {
                scReg.SetValue("Description", info.Description);
                return null;
            }
            return "找不到服务";
        }
        /// <summary>
        /// 获取服务所在的注册表
        /// </summary>
        /// <param name="servicesName"></param>
        /// <returns></returns>
        public static RegistryKey GetServicesKey(string servicesName) 
        {
            RegistryKey hkml = Registry.LocalMachine;
            RegistryKey scReg = hkml.OpenSubKey("SYSTEM\\CurrentControlSet\\Services\\" + servicesName, true);
            return scReg;
        }

        /// <summary>
        /// 卸载服务
        /// </summary>
        /// <param name="servicesName"></param>
        /// <returns></returns>
        public static string UnInstallServices(ServiceController sc) 
        {

            if (sc == null) 
            {
                return "服务不能为空";
            }
            WindowsAPI.DeleteService(sc.ServiceHandle.DangerousGetHandle());
            RefreashServices();
            return null;
        }


    }
}
