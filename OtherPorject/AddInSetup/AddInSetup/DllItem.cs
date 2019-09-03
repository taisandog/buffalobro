using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml;
using System.Windows.Forms;
using System.Diagnostics;
using System.Reflection;

namespace AddInSetup
{
    /// <summary>
    /// DllItem��Ϣ
    /// </summary>
    public class DllItem
    {
        /// <summary>
        /// Dll��Ϣ����
        /// </summary>
        private string _name;
        /// <summary>
        /// Dll��Ϣ����
        /// </summary>
        public string Name
        {
            get { return _name; }
        }

        /// <summary>
        /// ��ע
        /// </summary>
        private string _description;
        /// <summary>
        /// ��ע
        /// </summary>
        public string Description
        {
            get { return _description; }
        }

        /// <summary>
        /// ��ѡ��
        /// </summary>
        private bool _selected;
        /// <summary>
        /// ��ѡ��
        /// </summary>
        public bool Selected
        {
            get { return _selected; }
        }

        private DllItemFile _mainFile;
        /// <summary>
        /// ���ļ�
        /// </summary>
        public DllItemFile MainFile
        {
            get { return _mainFile; }
        }
        /// <summary>
        /// ��ȡ���ļ��汾��
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public string GetMainVersion(DllVerInfo info) 
        {
            if (_mainFile == null)
            {
                return "û�����ļ�";
            }
            string verPath = info.CurPath;
            if (verPath[verPath.Length - 1] != '\\')
            {
                verPath += '\\';
            }
            
            string sourceFile = verPath + _mainFile.Path;
            if (!File.Exists(sourceFile)) 
            {
                return "���ļ�������";
            }
            
            return ToolVersionInfo.GetVersion(sourceFile);
        }

        /// <summary>
        /// ��ȡ���ļ��汾��
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public bool ExistsFile(DllVerInfo info)
        {
            if (_mainFile == null)
            {
                return false;
            }
            string verPath = info.CurPath;
            if (verPath[verPath.Length - 1] != '\\')
            {
                verPath += '\\';
            }

            string sourceFile = verPath + _mainFile.Path;
            if (!File.Exists(sourceFile))
            {
                return false;
            }

            return true;
        }

        private List<DllItemFile> _lstFiles=new List<DllItemFile>();
        /// <summary>
        /// �ļ��б�
        /// </summary>
        public List<DllItemFile> Files
        {
            get { return _lstFiles; }
        }
        /// <summary>
        /// ���Եİ汾
        /// </summary>
        private Dictionary<string,bool> _ignore=new Dictionary<string,bool>(StringComparer.CurrentCultureIgnoreCase);
        /// <summary>
        /// ���Եİ汾
        /// </summary>
        public Dictionary<string, bool> Ignore
        {
            get { return _ignore; }
        }

        /// <summary>
        /// ����ļ�
        /// </summary>
        /// <param name="verPath">�汾Ŀ¼</param>
        /// <param name="outputPath">���Ŀ¼</param>
        public void PutFiles(DllVerInfo verInfo, string outputPath) 
        {
            string verPath = verInfo.CurPath;
            if (verPath[verPath.Length-1] != '\\') 
            {
                verPath += '\\';
            }
            if (outputPath[outputPath.Length-1] != '\\')
            {
                outputPath += '\\';
            }
            foreach (DllItemFile fileInfo in _lstFiles) 
            {
                if (fileInfo.Ignore.ContainsKey(verInfo.NetVersion))
                {
                    continue;
                }
                string sourceFile =Path.Combine(verPath, fileInfo.Path);
                if (!File.Exists(sourceFile))
                {
                    continue;
                }

                string tpath = fileInfo.TargetPath;
                if (string.IsNullOrWhiteSpace(tpath))
                {
                    tpath = fileInfo.Path;
                }
                string targetFile = Path.Combine(outputPath, tpath);
                FileInfo finfo = new FileInfo(targetFile);
                if (!finfo.Directory.Exists) 
                {
                    finfo.Directory.Create();
                }
                
                File.Copy(sourceFile, targetFile,true);
            }
        }

        /// <summary>
        /// ��XML�ڵ��ȡ��
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public static DllItem LoadForNode(XmlNode node) 
        {
            DllItem info = new DllItem();
            XmlAttribute att = node.Attributes["name"];
            if (att != null)
            {
                info._name = att.InnerText;
            }
            att = node.Attributes["des"];
            if (att != null)
            {
                info._description = att.InnerText;
            }
            att = node.Attributes["ignore"];
            if (att != null)
            {
                string[] texts = att.InnerText.Split(',');
                foreach (string txt in texts) 
                {
                    info._ignore[txt.Trim()] = true;
                }
            }
            att = node.Attributes["selected"];
            if (att != null)
            {
                info._selected = att.InnerText == "1";
            }
            foreach (XmlNode fileNode in node.ChildNodes) 
            {
                DllItemFile file = DllItemFile.LoadForNode(fileNode);

                info._lstFiles.Add(file);
                if (file.IsMain) 
                {
                    info._mainFile = file;
                }
            }
            return info;
        }
    }
}
