using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Buffalo.Kernel;

namespace Buffalo.Storage.LocalFileManager
{
    public class LocalFileInfo : FileInfoBase
    {
        /// <summary>
        /// 文件信息
        /// </summary>
        /// <param name="createTime">创建时间</param>
        /// <param name="updateTime">更新时间</param>
        /// <param name="filePath">文件名(包含路径)</param>
        /// <param name="length">长度</param>
        public LocalFileInfo(DateTime createTime,DateTime updateTime,string filePath,long length) 
        {
            _createTime=createTime;
            _updateTime=updateTime;
            _filePath = filePath;
            FillPathInfo(filePath);
            _length = length;
            _relativePath = filePath;
        }
        /// <summary>
        ///  填充信息
        /// </summary>
        /// <param name="filePath"></param>
        private void FillPathInfo(string filePath) 
        {
            if(string.IsNullOrWhiteSpace(filePath))
            {
                return;
            }
            string[] paths = filePath.Split('/','\\');
            _fileName = paths[paths.Length - 1];
            string[] namePart = _fileName.Split('.');
            if (namePart.Length > 1) 
            {
                _extendName = namePart[namePart.Length - 1];
            }
        }

        private string _hash = null;
        /// <summary>
        /// 哈希码
        /// </summary>
        public override string Hash
        {
            get 
            {
                if (!string.IsNullOrWhiteSpace(_hash)) 
                {
                    return _hash;
                }
                if (string.IsNullOrWhiteSpace(_filePath)) 
                {
                    return null;
                }
                if(!File.Exists(_filePath))
                {
                     return null;
                }
                using (FileStream file = new FileStream(_filePath, FileMode.Open, FileAccess.Read))
                {
                    _hash = PasswordHash.ToSHA1String(file,false);
                }
                return _hash;
            }
        }

       
    }
}
