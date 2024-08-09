using System;
using System.IO;
using Buffalo.ArgCommon;
using System.Security.Cryptography;
using System.Collections.Generic;

namespace Buffalo.Storage
{
    public abstract class IFileStorage : IDisposable
    {
        /// <summary>
        /// 追加文件
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="content">文件内容</param>
        /// <param name="postion">写入起始位置</param>
        /// <returns></returns>
        public abstract APIResault AppendFile(string path, Stream content, long postion);
        /// <summary>
        /// 追加到文件末尾
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="content">文件内容</param>
        /// <returns></returns>
        public abstract APIResault AppendFile(string path, Stream content);
        /// <summary>
        /// 关闭连接
        /// </summary>
        public abstract APIResault Close();

        /// <summary>
        /// 哈希类型
        /// </summary>
        public abstract string HashName { get; }
        /// <summary>
        /// 获取哈希类
        /// </summary>
        public abstract HashAlgorithm GetHash();
        /// <summary>
        /// 配置信息
        /// </summary>
        public abstract object ConfigInfo
        {
            get;
        }
        /// <summary>
        /// 操作客户端
        /// </summary>
        public abstract object Client
        {
            get;
        }
        /// <summary>
        /// 获取所有目录
        /// </summary>
        /// <param name="path">基础目录</param>
        /// <param name="searchOption">查找选项</param>
        /// <returns></returns>
        public abstract System.Collections.Generic.List<string> GetDirectories(string path, System.IO.SearchOption searchOption);
        /// <summary>
        /// 获取文件流
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="postion"></param>
        /// <returns></returns>
        public abstract System.IO.Stream GetFileStream(string path);
        /// <summary>
        /// 获取文件流
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="postion">起始获取位置</param>
        /// <param name="length">要下多大(0则一直下完位置)</param>
        /// <returns></returns>
        public abstract System.IO.Stream GetFileStream(string path, long postion,long length);
        /// <summary>
        /// 把文件读取到流
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="stm">要写入的流</param>
        /// <param name="postion">读取的起始位置(0则从头开始)</param>
        /// <param name="length">要读取的长度(小于等于0则表示要读完)</param>
        public abstract void ReadFileToStream(string path, Stream stm, long postion, long length);

        /// <summary>
        /// 获取文件信息
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public abstract FileInfoBase GetFileInfo(string path);
        /// <summary>
        /// 获取所有文件
        /// </summary>
        /// <param name="path">文件夹</param>
        /// <param name="searchOption">查找选项</param>
        /// <returns></returns>
        public abstract System.Collections.Generic.List<FileInfoBase> GetFiles(string path, System.IO.SearchOption searchOption);
        /// <summary>
        /// 打开链接
        /// </summary>
        /// <returns></returns>
        public abstract APIResault Open();

        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="path">路径</param>
        public abstract APIResault RemoveFile(string path);
        /// <summary>
        /// 重命名文件
        /// </summary>
        /// <param name="source">原文件路径</param>
        /// <param name="target">目标文件路径</param>
        public abstract APIResault RenameFile(string source, string target);
        /// <summary>
        /// 保存文件
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="content">文件内容</param>
        /// <param name="contentLength">内容长度，为0时候则从流获取长度</param>
        /// <returns></returns>
        public abstract APIResault SaveFile(string path, Stream content,long contentLength);
        /// <summary>
        /// 保存文件
        /// </summary>
        /// <param name="sourcePath">原路径</param>
        /// <param name="targetPath">目标路径</param>
        /// <returns></returns>
        public abstract APIResault SaveFile(string sourcePath, string targetPath);
        /// <summary>
        /// 删除文件夹
        /// </summary>
        /// <param name="path">路径</param>
        public abstract APIResault RemoveDirectory(string path);
        /// <summary>
        /// 文件夹是否存在
        /// </summary>
        /// <param name="folder"></param>
        /// <returns></returns>
        public abstract bool ExistDirectory(string folder);
        /// <summary>
        /// 文件是否存在
        /// </summary>
        /// <param name="path">文件夹</param>
        /// <returns></returns>
        public abstract bool ExistsFile(string path);
        /// <summary>
        /// 创建文件夹
        /// </summary>
        /// <param name="folder"></param>
        /// <returns></returns>
        public abstract APIResault CreateDirectory(string folder);

        public abstract void Dispose();
        /// <summary>
        /// 服务器地址
        /// </summary>
        protected string _server;
        /// <summary>
        /// 安全账号
        /// </summary>
        protected string _secretId = null;
        /// <summary>
        /// 安全Key
        /// </summary>
        protected string _secretKey = null;
        /// <summary>
        /// 超时(毫秒)
        /// </summary>
        protected int _timeout = 0;
        /// <summary>
        /// BucketName
        /// </summary>
        protected string _bucketName;
        /// <summary>
        /// 外网访问地址
        /// </summary>
        protected string _internetUrl;
        /// <summary>
        /// 内网访问地址
        /// </summary>
        protected string _lanUrl;
        /// <summary>
        /// 需要上传效验
        /// </summary>
        protected bool _needHash;
        /// <summary>
        /// 代理服务
        /// </summary>
        protected string _proxyHost;
        /// <summary>
        /// 代理服务端口
        /// </summary>
        protected int _proxyPort;
        /// <summary>
        /// 代理服务的用户名
        /// </summary>
        protected string _proxyUser;
        /// <summary>
        /// 代理服务的用户名
        /// </summary>
        protected string _proxyPass;

        /// <summary>
        /// 互联网地址
        /// </summary>
        public string InternetUrl
        {
            get
            {
                return _internetUrl;
            }
        }
        /// <summary>
        /// 局域网地址
        /// </summary>
        public string LanUrl
        {
            get
            {
                return _lanUrl;
            }
        }
        ///// <summary>
        ///// 服务器
        ///// </summary>
        //public string Server
        //{
        //    get
        //    {
        //        return _server;
        //    }
        //}
        /// <summary>
        /// 安全账号
        /// </summary>
        public string SecretId
        {
            get
            {
                return _secretId;
            }
        }
        /// <summary>
        /// 安全Key
        /// </summary>
        public string SecretKey
        {
            get
            {
                return _secretKey;
            }
        }
        /// <summary>
        /// 超时(毫秒)
        /// </summary>
        public int Timeout
        {
            get
            {
                return _timeout;
            }
        }
        /// <summary>
        /// 桶
        /// </summary>
        public string BucketName
        {
            get
            {
                return _bucketName;
            }
        }
        /// <summary>
        /// 需要哈希
        /// </summary>
        public bool NeedHash
        {
            get
            {
                return _needHash;
            }
        }
        ///// <summary>
        ///// 代理
        ///// </summary>
        //public string ProxyHost
        //{
        //    get
        //    {
        //        return _proxyHost;
        //    }
        //}
        ///// <summary>
        ///// 登录代理密码
        ///// </summary>
        //public string ProxyPass
        //{
        //    get
        //    {
        //        return _proxyPass;
        //    }
        //}
        ///// <summary>
        ///// 代理端口
        ///// </summary>
        //public int ProxyPort
        //{
        //    get
        //    {
        //        return _proxyPort;
        //    }
        //}
        ///// <summary>
        ///// 登录代理用户
        ///// </summary>
        //public string ProxyUser
        //{
        //    get
        //    {
        //        return _proxyUser;
        //    }
        //}
       
        /// <summary>
        /// 把文件读取到流
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="stm">要写入的流</param>
        public void ReadFileToStream(string path, Stream stm)
        {
            ReadFileToStream(path, stm, 0, -1);
        }
        /// <summary>
        /// 把文件读取到流
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="stm">要写入的流</param>
        /// <param name="postion">读取的起始位置(0则从头开始)</param>
        public void ReadFileToStream(string path, Stream stm,long postion)
        {
            ReadFileToStream(path, stm, postion, -1);
        }

        /// <summary>
        /// 填充配置
        /// </summary>
        /// <param name="hs"></param>
        protected void FillBaseConfig(Dictionary<string, string> hs)
        {
            _server = hs.GetMapValue<string>("Server");
            _bucketName = hs.GetMapValue<string>("bucketName");
            _internetUrl = hs.GetMapValue<string>("InternetUrl");
            if (!string.IsNullOrWhiteSpace(_internetUrl))
            {
                _internetUrl = _internetUrl.TrimEnd(' ', '\r', '\n', '/', '\\');
            }

            _lanUrl = hs.GetMapValue<string>("LanUrl");
            if (!string.IsNullOrWhiteSpace(_lanUrl))
            {
                _lanUrl = _lanUrl.TrimEnd(' ', '\r', '\n', '/', '\\');
            }

            _needHash = hs.GetMapValue<int>("needHash") == 1;
            _secretId= GetAccessKey(hs);
            _secretKey = hs.GetMapValue<string>("SecretKey");
            _timeout = hs.GetMapValue<int>("timeout", 1000);
            _proxyHost = hs.GetMapValue<string>("ProxyHost");
            _proxyPort = hs.GetMapValue<int>("ProxyPort");
            _proxyUser = hs.GetMapValue<string>("ProxyUser");
            _proxyPass = hs.GetMapValue<string>("ProxyPass");
        }
        
        private string GetAccessKey(Dictionary<string,string> hs)
        {
            string[] arrKeyName = { "SecretId", "AccessKey" };
            string ret = null;
            foreach (string arr in arrKeyName)
            {
                ret= hs.GetMapValue<string>(arr);
                if (!string.IsNullOrWhiteSpace(ret))
                {
                    return ret;
                }
            }
            return ret;

        }
    }
}
