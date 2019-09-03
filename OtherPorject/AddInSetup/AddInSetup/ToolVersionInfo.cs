using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Diagnostics;
using System.IO;

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
            string[] names = fileName.Split('.');
            if (string.Equals(names[names.Length-1], "nupkg"))
            {
                StringBuilder sbVer = new StringBuilder();
                
                string tmp = null;
                foreach(string namePart in names)
                {
                    
                    tmp = namePart.Trim();
                    if (!IsNumber(tmp))
                    {
                        continue;
                    }
                    sbVer.Append(tmp);
                    sbVer.Append(".");
                }
                if (sbVer.Length > 0)
                {
                    sbVer.Remove(sbVer.Length - 1, 1);
                }
                return sbVer.ToString();
            }

            FileVersionInfo sourceVersionInfo = FileVersionInfo.GetVersionInfo(fileName);
            return sourceVersionInfo.FileVersion.ToString();
        }

        /// <summary>
        /// 字符串是否数字
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private static bool IsNumber(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return false;
            }
            foreach(char chr in str)
            {
                if (!char.IsDigit(chr))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
