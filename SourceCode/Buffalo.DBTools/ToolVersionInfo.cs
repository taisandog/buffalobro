using System;
using System.Collections.Generic;
using System.Text;

namespace Buffalo.DBTools
{
    /// <summary>
    /// 版本信息
    /// </summary>
    public class ToolVersionInfo
    {
        private static string _versionInfo;
        /// <summary>
        /// 软件版本信息
        /// </summary>
        public static string ToolVerInfo 
        {
            get 
            {
                if (_versionInfo == null) 
                {
                    string ver=System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
                    StringBuilder sbInfo = new StringBuilder();
                    sbInfo.Append("[Buffalo配置工具:");
                    sbInfo.Append(ver);
                    sbInfo.Append("]");
                    _versionInfo = sbInfo.ToString();
                }
                return _versionInfo;
            }
        }
    }
}
