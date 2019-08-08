using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Diagnostics;

namespace AddInSetup
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
                    string ver=Assembly.GetExecutingAssembly().GetName().Version.ToString();
                    StringBuilder sbInfo = new StringBuilder();
                    sbInfo.Append("[Buffalo插件安装助手:");
                    sbInfo.Append(ver);
                    sbInfo.Append("]");
                    _versionInfo = sbInfo.ToString();
                }
                return _versionInfo;
            }
        }
        /// <summary>
        /// 获取版本号
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string GetVersion(string fileName) 
        {
            FileVersionInfo sourceVersionInfo = FileVersionInfo.GetVersionInfo(fileName);
            return sourceVersionInfo.FileVersion.ToString();
        }
    }
}
