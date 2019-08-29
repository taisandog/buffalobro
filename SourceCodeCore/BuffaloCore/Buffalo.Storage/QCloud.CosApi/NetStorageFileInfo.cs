using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buffalo.Storage.QCloud.CosApi
{
    /// <summary>
    /// 网络存储文件信息
    /// </summary>
    public class NetStorageFileInfo : FileInfoBase
    {
        /// <summary>
        /// 文件信息
        /// </summary>
        /// <param name="createTime">创建时间</param>
        /// <param name="updateTime">更新时间</param>
        /// <param name="relativePath">相对路径</param>
        /// <param name="filePath">路径</param>
        /// <param name="accessUrl">ass路径</param>
        /// <param name="hash">哈希码</param>
        /// <param name="length">长度</param>
        public NetStorageFileInfo(DateTime createTime, DateTime updateTime,string relativePath,
            string filePath,string accessUrl,string hash, long length) 
        {
            _createTime=createTime;
            _updateTime=updateTime;
            _filePath = filePath;
            FillPathInfo(relativePath);
            _hash = hash;
            _length = length;
            _accessUrl = accessUrl;
            _relativePath = relativePath;
        }

        private string _accessUrl;
        /// <summary>
        /// 内部路径
        /// </summary>
        public string AccessUrl
        {
            get { return _accessUrl; }
        }

        /// <summary>
        ///  填充信息
        /// </summary>
        /// <param name="filePath"></param>
        private void FillPathInfo(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
            {
                return;
            }
            
            _fileName = GetFileName(filePath);
            if (string.IsNullOrWhiteSpace(_fileName))
            {
                return;
            }
            string[] namePart = _fileName.Split('.');
            if (namePart.Length > 1)
            {
                _extendName = namePart[namePart.Length - 1];
            }
        }
        
        private string _hash;
        /// <summary>
        /// 哈希码
        /// </summary>
        public override string Hash
        {
            get { return _hash; }
        }
    }
}
