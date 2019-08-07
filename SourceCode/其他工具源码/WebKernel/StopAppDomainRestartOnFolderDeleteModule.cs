using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using Buffalo.Kernel.FastReflection;
using System.Reflection;

namespace Buffalo.WebKernel.WebCommons
{
    /// <summary>
    /// 关闭IIS文件改变导致重启
    /// </summary>
    public class StopAppDomainRestartOnFolderDeleteModule : IHttpModule
    {

        #region IHttpModule 成员

        public void Dispose()
        {
            
        }
        public void Init(HttpApplication context)
        {
            PropertyInfoHandle fileChangesMonitorGet = FastValueGetSet.GetPropertyInfoHandle("FileChangesMonitor",typeof(HttpRuntime));
            object o = fileChangesMonitorGet.GetValue( null);
            GetFieldValueHandle f =FastFieldGetSet.FindGetValueHandle(o.GetType().GetField("_dirMonSubdirs", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.IgnoreCase));
            object monitor = f(o);
            FastInvokeHandler m =FastValueGetSet.GetCustomerMethodInfo(monitor.GetType().GetMethod("StopMonitoring", BindingFlags.Instance | BindingFlags.NonPublic));
            m.Invoke(monitor, new object[] { });
        }

        #endregion
    }
}

/*
 * IIS7以下：
    <system.web>
	  <httpModules>
		  <add name="stopAppDomainRestartOnFolderDelete" type="Buffalo.WebKernel.WebCommons.StopAppDomainRestartOnFolderDeleteModule,Buffalo.WebKernel"/>
	  </httpModules>
    </system.web>
 * 
 * IIS7
 <system.webServer>
	  <modules runAllManagedModulesForAllRequests="true">
		  <add name="stopAppDomainRestartOnFolderDelete" type="Buffalo.WebKernel.WebCommons.StopAppDomainRestartOnFolderDeleteModule,Buffalo.WebKernel"/>
	  </modules>
  </system.webServer>
*/