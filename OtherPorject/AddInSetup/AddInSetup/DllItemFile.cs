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
            return info;
        }
    }
}
