using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceProcess;
using Buffalo.Win32Kernel.Win32;
using Microsoft.Win32;
using Buffalo.Kernel.Win32;

namespace Buffalo.Win32Kernel.ServicesUnit
{
    
    /// <summary>
    /// 服务安装信息
    /// </summary>
    public class ServiceInfo
    {
        private ServiceInfo() 
        {
            
        }

        /// <summary>
        /// 获取服务信息
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static ServiceInfo GetServicesByName(string name) 
        {
            ServiceInfo info = new ServiceInfo();
            info._name = name;
            ServiceController sc = ServicesManager.GetController(name);
            if (sc == null) 
            {
                return info;
            }
            FillServicesInfo(info);
            return info;

        }



        /// <summary>
        /// 填充服务的信息
        /// </summary>
        /// <param name="info"></param>
        private static void FillServicesInfo(ServiceInfo info) 
        {
            RegistryKey rkey = ServicesManager.GetServicesKey(info.Name);
            if (rkey != null) 
            {
                info._description = rkey.GetValue("Description").ToString();
                info._path = rkey.GetValue("ImagePath").ToString();
                info._starType = (ServicesStartType)Convert.ToUInt32(rkey.GetValue("Start"));
                info._displayName = rkey.GetValue("DisplayName").ToString();
            }
        }

        string _name;
        /// <summary>
        /// 服务名
        /// </summary>
        public string Name
        {
            get { return _name; }
        }
        string _displayName;
        /// <summary>
        /// 显示名
        /// </summary>
        public string DisplayName
        {
            get { return _displayName; }
            set { _displayName = value; }
        }

        string _description;
        /// <summary>
        /// 描述
        /// </summary>
        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        private ServicesStartType _starType = ServicesStartType.SERVICE_AUTO_START;

        /// <summary>
        /// 启动类型
        /// </summary>
        public ServicesStartType StarType
        {
            get { return _starType; }
            set { _starType = value; }
        }

        public override string ToString()
        {
            return string.Format("{0}[{1}]", DisplayName, StateInfo);
        }

        /// <summary>
        /// 当前服务状态
        /// </summary>
        public string StateInfo 
        {
            get 
            {
                ServiceController sc = GetController();
                if (sc == null) 
                {
                    return "未安装";
                }
                switch (sc.Status) 
                {
                    case ServiceControllerStatus.Stopped:
                        return "已停止";
                    case ServiceControllerStatus.StartPending:
                        return "正启动";
                    case ServiceControllerStatus.StopPending:
                        return "停止中";
                    case ServiceControllerStatus.Running:
                        return "服务中";
                    case ServiceControllerStatus.ContinuePending:
                        return "将继续";
                    case ServiceControllerStatus.PausePending:
                        return "将暂停";
                    case ServiceControllerStatus.Paused:
                        return "已停止";
                    default:
                        return "获取中";

                }
            }
        }

        private string _path;
        /// <summary>
        /// 服务路径
        /// </summary>
        public string Path
        {
            get { return _path; }
            set { _path = value; }
        }


        /// <summary>
        /// 是否存在
        /// </summary>
        public bool Exists
        {
            get { return GetController() != null; }
        }



        /// <summary>
        /// 服务控制器
        /// </summary>
        public ServiceController GetController()
        {
            return ServicesManager.GetController(Name);
        }

        /// <summary>
        /// 删除此服务
        /// </summary>
        /// <returns></returns>
        public string Remove() 
        {
            ServiceController sc = GetController();
            if (sc!=null) 
            {
                return ServicesManager.UnInstallServices(sc);
            }
            return "不存在服务:" + Name;
        }

        /// <summary>
        /// 保存配置
        /// </summary>
        /// <returns></returns>
        public string Save() 
        {
            if (Exists)//如果存在则保存配置
            {
                RegistryKey rkey = ServicesManager.GetServicesKey(Name);
                if (rkey == null)
                {
                    return "保存失败，找不到对应服务的注册表配置";
                }
                rkey.SetValue("Description", _description);
                rkey.SetValue("ImagePath", _path);
                rkey.SetValue("Start", (uint)_starType);
                rkey.SetValue("DisplayName", _displayName);

                return null;
            }
            else //否则就新建服务
            {
                return ServicesManager.InstallServices(this);
            }
        }
    }
}
