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
    /// DllItem�ļ���Ϣ
    /// </summary>
    public class DllItemFile
    {
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

        private bool _isMain=false;
        /// <summary>
        /// �Ƿ����ļ�
        /// </summary>
        public bool IsMain
        {
            get { return _isMain; }
        }
        /// <summary>
        /// ��XML�ڵ��ȡ��
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
