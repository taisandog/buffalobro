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
    /// DllItem文件信息
    /// </summary>
    public class DllItemFile
    {
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
            set { _path = value; }
        }
        /// <summary>
        /// 输出的文件路径
        /// </summary>
        private string _targetPath;
        /// <summary>
        /// 输出的文件路径
        /// </summary>
        public string TargetPath
        {
            get { return _targetPath; }
        }

        private bool _isStandard = false;
        /// <summary>
        /// 是否Standard包
        /// </summary>
        public bool IsStandard
        {
            get { return _isStandard; }
        }
        private Dictionary<string, bool> _dicIgnore = new Dictionary<string, bool>(StringComparer.CurrentCultureIgnoreCase);
        /// <summary>
        /// 此项要忽略的版本
        /// </summary>
        public Dictionary<string, bool> Ignore
        {
            get { return _dicIgnore; }
        }

        private bool _isMain=false;
        /// <summary>
        /// 是否主文件
        /// </summary>
        public bool IsMain
        {
            get { return _isMain; }
        }
        /// <summary>
        /// 从XML节点读取类
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public static DllItemFile LoadForNode(XmlNode node) 
        {
            DllItemFile info = new DllItemFile();
            XmlAttribute att = node.Attributes["path"];
            if (att != null)
            {
                info._path = att.InnerText;
            }
            att = node.Attributes["main"];
            if (att != null)
            {
                info._isMain = att.InnerText=="1";
            }
            att = node.Attributes["target"];
            if (att != null)
            {
                info._targetPath = att.InnerText ;
            }
            att = node.Attributes["isStandard"];
            if (att != null)
            {
                info._isStandard = att.InnerText=="1";
            }
            att = node.Attributes["ignore"];
            if (att != null)
            {
                string ignore=att.InnerText;
                if (!string.IsNullOrWhiteSpace(ignore))
                {
                    string[] arrIgnore = ignore.Split('|');
                    foreach(string str in arrIgnore)
                    {
                        if (string.IsNullOrWhiteSpace(str))
                        {
                            continue;
                        }
                        info._dicIgnore[str.Trim()] = true;
                    }
                }
            }

            return info;
        }
    }
}
