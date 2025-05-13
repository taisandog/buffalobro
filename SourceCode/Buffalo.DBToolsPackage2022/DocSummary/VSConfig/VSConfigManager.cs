using System;
using System.Collections.Generic;
using System.Text;
using EnvDTE;

namespace Buffalo.DBTools.DocSummary.VSConfig
{
    /// <summary>
    /// VS配置信息
    /// </summary>
    public class VSConfigManager
    {
        private static IVSConfig _curConfig=null;
        /// <summary>
        /// 初始化当前配置
        /// </summary>
        /// <param name="curProject"></param>
        public static void InitConfig(string version)
        {
            if (_curConfig != null) 
            {
                return;
            }
            

            if (version == "8.0") 
            {
                _curConfig = new VS2005Config();
            }
            else if (version == "9.0")
            {
                _curConfig = new VS2008Config();
            }
            else if (version == "10.0")
            {
                _curConfig = new VS2010Config();
            }
            else 
            {
                _curConfig = new VS2010Config();
            }
        }
        /// <summary>
        /// 获取当前的配置
        /// </summary>
        public static IVSConfig CurConfig 
        {
            get 
            {
                return _curConfig;
            }
        }
    }
}
