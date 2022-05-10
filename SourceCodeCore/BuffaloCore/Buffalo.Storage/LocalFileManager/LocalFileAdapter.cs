
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Collections;
using Buffalo.Kernel;
using Buffalo.ArgCommon;
using System.Security.Cryptography;

namespace Buffalo.Storage.LocalFileManager
{
    /// <summary>
    /// �����ļ�������
    /// </summary>
    public class LocalFileAdapter : IFileStorage
    {
        /// <summary>
        /// �洢��Ŀ¼
        /// </summary>
        private string _fileRoot;
        /// <summary>
        /// �û���
        /// </summary>
        private string _userName;

        /// <summary>
        /// ����
        /// </summary>
        private string _password;

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="fileName"></param>
        ///// <param name="userName"></param>
        ///// <param name="password"></param>
        //public LocalFileAdapter(string fileName, string userName, string password) 
        //{
        //    _fileRoot = fileName;
        //    _userName = userName;
        //    _password = password;
        //}

        public LocalFileAdapter(string connectString) 
        {

            Dictionary<string, string> hs = ConnStringFilter.GetConnectInfo(connectString);
            _fileRoot = hs.GetMapValue<string>("path");
            if (!string.IsNullOrEmpty(_fileRoot)) 
            {
                _fileRoot = GetRealRoot(_fileRoot);
                //if(!_fileRoot.EndsWith("\\"))
                //{
                //    _fileRoot=_fileRoot+"\\";
                //}
            }
            _userName = hs.GetMapValue<string>("user");
            _password = hs.GetMapValue<string>("pwd");
            
        }

        /// <summary>
        /// ��ȡ����·��
        /// </summary>
        /// <param name="path">Ŀ¼</param>
        /// <returns></returns>
        private string GetLocal(string path) 
        {
            //StringBuilder sbRet=new StringBuilder(200);
            //sbRet.Append(_fileRoot);
            
            //sbRet.Append(path);
           
            return Path.Combine(_fileRoot, path);
        }
        /// <summary>
        /// ��ϣ����
        /// </summary>
        public override string HashName
        {
            get
            {
                return "SHA1";
            }
        }
        /// <summary>
        /// ��ȡ��ϣ��
        /// </summary>
        public override HashAlgorithm GetHash()
        {
            System.Security.Cryptography.SHA1 sha1Hash = SHA1CryptoServiceProvider.Create();
            return sha1Hash;
        }

        /// <summary>
        /// ��ȡ��ʵ·��
        /// </summary>
        /// <param name="root"></param>
        /// <returns></returns>
        private string GetRealRoot(string root) 
        {
            char start=root[0];
            if (root[0] == '\\' && root[1] == '\\') 
            {
                return root;
            }
            if (root[0] == '/' && root[1] == '/')
            {
                return root;
            }
            if (start=='.' || start=='\\' || start=='/') 
            {
                string mroot =Path.Combine(CommonMethods.GetBaseRoot(), root);
               
                return mroot;
            }
            return root;
        }

        /// <summary>
        /// ������
        /// </summary>
        /// <returns></returns>
        public override APIResault Open() 
        {
            if (string.IsNullOrEmpty(_userName)) 
            {
                return ApiCommon.GetSuccess();
            }
            return FileAPI.WNetAddConnection(_userName, _password, _fileRoot, null);
            
        }

        /// <summary>
        /// �ر�����
        /// </summary>
        public override APIResault Close() 
        {
            if (string.IsNullOrEmpty(_userName))
            {
                return ApiCommon.GetSuccess();
            }
            return FileAPI.WNetCancelConnection(_fileRoot, 1, true); //ȡ��ӳ��
            
        }


        /// <summary>
        /// ��ȡ�ļ���
        /// </summary>
        /// <param name="path">·��</param>
        /// <param name="postion"></param>
        /// <returns></returns>
        public override Stream GetFileStream(string path)
        {
            FileStream fs = new FileStream(GetLocal(path), FileMode.Open);
            return fs;
        }

        /// <summary>
        /// �ļ��Ƿ����
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public override bool ExistsFile(string path)
        {
            
            return File.Exists(path);
        }
        /// <summary>
        /// ��ȡ�ļ���
        /// </summary>
        /// <param name="path">·��</param>
        /// <param name="postion">λ��</param>
        /// <returns></returns>
        public override Stream GetFileStream(string path,long postion, long length)
        {
            FileStream fs = new FileStream(GetLocal(path), FileMode.Open);
            fs.Position = postion;
            return fs;
        }
        /// <summary>
        /// ����ļ���
        /// </summary>
        /// <param name="path"></param>
        private void CheckDirectory(string path) 
        {
            //string curPath = GetLocal(path);
            FileInfo finf = new FileInfo(path);
            DirectoryInfo info = new DirectoryInfo(finf.DirectoryName);
            if (!info.Exists) 
            {
                info.Create();
            }
        }

        /// <summary>
        /// ׷���ļ�
        /// </summary>
        /// <param name="path">·��</param>
        /// <param name="content">�ļ�����</param>
        /// <param name="postion">д����ʼλ��</param>
        /// <returns></returns>
        public override APIResault AppendFile(string path, Stream content, long postion) 
        {
            string curpath=GetLocal(path);
            CheckDirectory(curpath);
            using (FileStream file = new FileStream(curpath, FileMode.Open, FileAccess.Write))
            {
                //file.Seek(postion, SeekOrigin.End);
                if (postion < 0)
                {
                    file.Position = file.Length - 1;
                }
                else
                {
                    file.Position = postion;
                }
                CommonMethods.CopyStreamData(content, file,-1);
                //file.Write(content, 0, content.Length);
            }
            return ApiCommon.GetSuccess();
        }
        /// <summary>
        /// ׷�ӵ��ļ�ĩβ
        /// </summary>
        /// <param name="path">·��</param>
        /// <param name="content">�ļ�����</param>
        /// <returns></returns>
        public override APIResault AppendFile(string path, Stream content)
        {
            return AppendFile(path, content, -1);
        }
        /// <summary>
        /// �����ļ�
        /// </summary>
        /// <param name="path">·��</param>
        /// <param name="content">�ļ�����</param>
        /// <returns></returns>
        public override APIResault SaveFile(string path, Stream content,long contentLength)
        {
            string curpath = GetLocal(path);
            CheckDirectory(curpath);
            curpath = curpath.TrimEnd('\\');
            using (FileStream file = new FileStream(curpath, FileMode.Create, FileAccess.Write))
            {
                //file.Seek(postion, SeekOrigin.End);
                CommonMethods.CopyStreamData(content, file);
            }
            return ApiCommon.GetSuccess();
        }
        /// <summary>
        /// �����ļ�
        /// </summary>
        /// <param name="sourcePath">ԭ·��</param>
        /// <param name="targetPath">Ŀ��·��</param>
        /// <returns></returns>
        public override APIResault SaveFile(string sourcePath, string targetPath)
        {
            string curpath = GetLocal(targetPath);
            CheckDirectory(curpath);
            curpath = curpath.TrimEnd('\\');
            File.Copy(sourcePath, curpath);
            return ApiCommon.GetSuccess();
        }
        /// <summary>
        /// ɾ���ļ�
        /// </summary>
        /// <param name="path">·��</param>
        public override APIResault RemoveFile(string path) 
        {
            File.Delete(GetLocal(path));
            return ApiCommon.GetSuccess();
        }

        /// <summary>
        /// ɾ���ļ�
        /// </summary>
        /// <param name="path">·��</param>
        public override APIResault RemoveDirectory(string path)
        {
            Directory.Delete(GetLocal(path));
            return ApiCommon.GetSuccess();
        }
        /// <summary>
        /// �������ļ�
        /// </summary>
        /// <param name="source">ԭ�ļ�·��</param>
        /// <param name="target">Ŀ���ļ�·��</param>
        public override APIResault RenameFile(string source, string target) 
        {
            string tpath = GetLocal(target);
            CheckDirectory(tpath);
            File.Move(GetLocal(source), tpath);
            return ApiCommon.GetSuccess();
        }

        /// <summary>
        /// ��ȡ�����ļ�
        /// </summary>
        /// <param name="path">�ļ���</param>
        /// <param name="searchOption">����ѡ��</param>
        /// <returns></returns>
        public override List<FileInfoBase> GetFiles(string path, SearchOption searchOption) 
        {
            string sfilePath = GetLocal(path);

            string[] files = Directory.GetFiles(sfilePath, "*.*", searchOption);
            List<FileInfoBase> ret = new List<FileInfoBase>(files.Length);
            foreach (string spath in files) 
            {

                FileInfoBase fInfo = GetFileInfo(path);

                ret.Add(fInfo);
            }
            return ret;
        }
        /// <summary>
        /// ��ȡ�ļ���Ϣ
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public override FileInfoBase GetFileInfo(string path) 
        {
            FileInfo finfo = new FileInfo(path);
            FileInfoBase fInfo = new LocalFileInfo(finfo.CreationTime, finfo.LastWriteTime, path, finfo.Length);
            return fInfo;
        }
        /// <summary>
        /// ��ȡ�����ļ���
        /// </summary>
        /// <param name="path">�ļ���</param>
        /// <param name="searchOption">����ѡ��</param>
        /// <returns></returns>
        public override List<string> GetDirectories(string path, SearchOption searchOption)
        {
            string sfilePath = GetLocal(path);
            string[] files = Directory.GetDirectories(sfilePath, "*", searchOption);
            List<string> ret = new List<string>(files.Length);
            foreach (string spath in files)
            {
                string curPath = spath.Substring(_fileRoot.Length);
                //if (curPath[0] != '\\')
                //{
                //    curPath = '\\' + curPath;
                //}
                ret.Add(curPath);
            }
            return ret;
        }


        #region IDisposable ��Ա

        public override void Dispose()
        {
            Close();
        }

        #endregion



        /// <summary>
        /// �ļ����Ƿ����
        /// </summary>
        /// <param name="folder"></param>
        /// <returns></returns>
        public override bool ExistDirectory(string folder)
        {
            return Directory.Exists(folder);
        }
        /// <summary>
        /// �����ļ���
        /// </summary>
        /// <param name="folder"></param>
        /// <returns></returns>
        public override APIResault CreateDirectory(string folder)
        {
            Directory.CreateDirectory(folder);
            return ApiCommon.GetSuccess();
        }

        public override void ReadFileToStream(string path, Stream stm, long postion, long length)
        {
            path = GetLocal(path);
            if (!ExistsFile(path))
            {
                throw new FileNotFoundException("�Ҳ����ļ�:" + path);
            }
            using (FileStream fs = new FileStream(path, FileMode.Open))
            {
                if (postion > 0)
                {
                    fs.Position = postion;
                }
                if (length <= 0)
                {
                    length = -1;
                }
                CommonMethods.CopyStreamData(fs, stm, length);
            }
        }
    }
}
