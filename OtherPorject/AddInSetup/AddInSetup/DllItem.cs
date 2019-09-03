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
    /// DllItem信息
    /// </summary>
    public class DllItem
    {
        /// <summary>
        /// Dll信息名称
        /// </summary>
        private string _name;
        /// <summary>
        /// Dll信息名称
        /// </summary>
        public string Name
        {
            get { return _name; }
        }

        /// <summary>
        /// 备注
        /// </summary>
        private string _description;
        /// <summary>
        /// 备注
        /// </summary>
        public string Description
        {
            get { return _description; }
        }

        /// <summary>
        /// 已选中
        /// </summary>
        private bool _selected;
        /// <summary>
        /// 已选中
        /// </summary>
        public bool Selected
        {
            get { return _selected; }
        }

        private DllItemFile _mainFile;
        /// <summary>
        /// 主文件
        /// </summary>
        public DllItemFile MainFile
        {
            get { return _mainFile; }
        }
        /// <summary>
        /// 获取主文件版本号
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public string GetMainVersion(DllVerInfo info) 
        {
            if (_mainFile == null)
            {
                return "没有主文件";
            }
            string verPath = info.CurPath;
            if (verPath[verPath.Length - 1] != '\\')
            {
                verPath += '\\';
            }
            
            string sourceFile = verPath + _mainFile.Path;
            if (!File.Exists(sourceFile)) 
            {
                return "主文件不存在";
            }
            
            return ToolVersionInfo.GetVersion(sourceFile);
        }

        /// <summary>
        /// 获取主文件版本号
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
        /// 文件列表
        /// </summary>
        public List<DllItemFile> Files
        {
            get { return _lstFiles; }
        }
        /// <summary>
        /// 忽略的版本
        /// </summary>
        private Dictionary<string,bool> _ignore=new Dictionary<string,bool>(StringComparer.CurrentCultureIgnoreCase);
        /// <summary>
        /// 忽略的版本
        /// </summary>
        public Dictionary<string, bool> Ignore
        {
            get { return _ignore; }
        }

        /// <summary>
        /// 输出文件
        /// </summary>
        /// <param name="verPath">版本目录</param>
        /// <param name="outputPath">输出目录</param>
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
        /// 从XML节点读取类
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
