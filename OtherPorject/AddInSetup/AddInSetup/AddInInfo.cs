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
    /// �����Ϣ
    /// </summary>
    public class AddInInfo
    {
        private string _fileName;
        /// <summary>
        /// �ļ�·��
        /// </summary>
        public string FileName
        {
            get { return _fileName; }
        }

        private string _version;

        /// <summary>
        /// �汾��
        /// </summary>
        public string Version
        {
            get { return _version; }
        }

        private bool _isvsix=false;

        /// <summary>
        /// �Ƿ�VSIX
        /// </summary>
        public bool IsVSIX
        {
            get { return _isvsix; }
            set { _isvsix = value; }
        }

        private string _name;

        /// <summary>
        /// ����
        /// </summary>
        public string Name
        {
            get { return _name; }
        }

        private bool _isSetup;
        /// <summary>
        /// �Ƿ�װ
        /// </summary>
        public bool IsSetup
        {
            get { return _isSetup; }
        }

        /// <summary>
        /// ��ť�ı�
        /// </summary>
        public string ButtonText 
        {
            get 
            {
                if (_isvsix) 
                {
                    return "��װ";
                }
                if (_isSetup) 
                {
                    return "ж��";
                }
                return "��װ";
            }
        }
        /// <summary>
        /// �ҵ��ĵ�·��
        /// </summary>
        private static string _myDocumentPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal).TrimEnd('\\');
        /// <summary>
        /// ��װ
        /// </summary>
        /// <returns></returns>
        public string Install() 
        {
            if (IsInstall()) 
            {
                return null;
            }

            string fileName =Path.Combine( ConfigLoader.BasePath , _fileName);
            if (!File.Exists(fileName)) 
            {
                return "�������ļ�:" + fileName;
            }

            if (_isvsix)
            {
                Process.Start(fileName);
                return null;
            }

            string fileContent = AddInSetup.Properties.Resources.DBTools;

            fileContent=fileContent.Replace("<%=Version%>", Version);
            fileContent=fileContent.Replace("<%=FileName%>", fileName);

            string dic=GetAddInDirectory();
            if (!Directory.Exists(dic)) 
            {
                Directory.CreateDirectory(dic);
            }
            File.WriteAllText(GetAddInFileName(), fileContent, Encoding.BigEndianUnicode);
            _isSetup = true;
            return null;
        }



        /// <summary>
        /// ����װ
        /// </summary>
        /// <returns></returns>
        public string UnInstall() 
        {
            if (!_isvsix)
            {
                if (IsInstall())
                {
                    File.Delete(GetAddInFileName());
                }
            }
            _isSetup = false;
            return null;
        }
        /// <summary>
        /// ��ȡ���������·��
        /// </summary>
        /// <returns></returns>
        private string GetAddInFileName()
        {
            return GetAddInDirectory() +ConfigLoader.AddInName;
        }
        /// <summary>
        /// ��ȡ������ļ���·��
        /// </summary>
        /// <returns></returns>
        private string GetAddInDirectory() 
        {
            return _myDocumentPath + "\\" + _name + "\\AddIns\\";
        }

        /// <summary>
        /// �ж��Ƿ��Ѿ���װ
        /// </summary>
        /// <returns></returns>
        public bool IsInstall() 
        {
            if (_isvsix) 
            {
                return false;
            }
            string addInFile = GetAddInFileName();
            return File.Exists(addInFile);
        }

        /// <summary>
        /// ��XML�ڵ��ȡ��
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public static AddInInfo LoadForNode(XmlNode node)
        {
            AddInInfo info = new AddInInfo();
            XmlAttribute att = node.Attributes["file"];
            if (att != null)
            {
                info._fileName = att.InnerText;
            }
            att = node.Attributes["version"];
            if (att != null)
            {
                info._version = att.InnerText;
            }
            att = node.Attributes["addin"];
            if (att != null)
            {
                info._name = att.InnerText;
            }
            att = node.Attributes["isvsix"];
            if (att != null)
            {
                info._isvsix = att.InnerText == "1";
            }
            info._isSetup = info.IsInstall();
            return info;
        }

        
    }
}
