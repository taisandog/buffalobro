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
    /// DllVer��Ϣ
    /// </summary>
    public class DllVerInfo
    {
        /// <summary>
        /// .net�汾
        /// </summary>
        private string _netVersion;
        /// <summary>
        /// .net�汾
        /// </summary>
        public string NetVersion
        {
            get { return _netVersion; }
        }

        /// <summary>
        /// ·��
        /// </summary>
        private string _path;
        /// <summary>
        /// ·��
        /// </summary>
        public string Path
        {
            get { return _path; }
        }
        /// <summary>
        /// ��ǰ����Ŀ¼
        /// </summary>
        public string CurPath 
        {
            get 
            {
                return ConfigLoader.BasePath + "\\" + _path;
            }
        }
        /// <summary>
        /// �汾����
        /// </summary>
        public string VerName 
        {
            get 
            {
                return ".Net Framework " + _netVersion;
            }
        }
        /// <summary>
        /// ��XML�ڵ��ȡ��
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
