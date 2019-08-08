using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml;
using System.Windows.Forms;
using System.Diagnostics;

namespace AddInSetup
{
    /// <summary>
    /// DllVer信息
    /// </summary>
    public class DllVerInfo
    {
        /// <summary>
        /// .net版本
        /// </summary>
        private string _netVersion;
        /// <summary>
        /// .net版本
        /// </summary>
        public string NetVersion
        {
            get { return _netVersion; }
        }

        /// <summary>
        /// 路径
        /// </summary>
        private string _path;
        /// <summary>
        /// 路径
        /// </summary>
        public string Path
        {
            get { return _path; }
        }
        /// <summary>
        /// 当前物理目录
        /// </summary>
        public string CurPath 
        {
            get 
            {
                return ConfigLoader.BasePath + "\\" + _path;
            }
        }
        /// <summary>
        /// 版本名称
        /// </summary>
        public string VerName 
        {
            get 
            {
                return ".Net Framework " + _netVersion;
            }
        }
        /// <summary>
        /// 从XML节点读取类
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public static DllVerInfo LoadForNode(XmlNode node) 
        {
            DllVerInfo info = new DllVerInfo();
            XmlAttribute att = node.Attributes["ver"];
            if (att != null)
            {
                info._netVersion = att.InnerText;
            }
            att = node.Attributes["path"];
            if (att != null)
            {
                info._path = att.InnerText;
            }
            return info;
        }
    }
}
