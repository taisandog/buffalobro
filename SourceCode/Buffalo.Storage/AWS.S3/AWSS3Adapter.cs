using Amazon;
using Amazon.S3;
using Amazon.S3.IO;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Buffalo.ArgCommon;
using Buffalo.Kernel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Buffalo.Storage.AWS.S3
{
    public class AWSS3Adapter : IFileStorage
    {
        /// <summary>
        /// 服务器地址
        /// </summary>
        private string _server;
        /// <summary>
        /// 客户端
        /// </summary>
        private AmazonS3Client _client;
        /// <summary>
        /// 安全ID
        /// </summary>
        private string _secretKey;
        /// <summary>
        /// 安全Key
        /// </summary>
        private string _accessKey;
        /// <summary>
        /// 超时(毫秒)
        /// </summary>
        private int _timeout = 0;
        /// <summary>
        /// BucketName
        /// </summary>
        private string _bucketName;
        /// <summary>
        /// 外网访问地址
        /// </summary>
        private string _internetUrl;
        /// <summary>
        /// 内网访问地址
        /// </summary>
        private string _lanUrl;


        private S3CannedACL _acl;
        /// <summary>
        /// 亚马逊适配
        /// </summary>
        /// <param name="connString"></param>
        public AWSS3Adapter(string connString)
        {
            Dictionary<string, string> hs = ConnStringFilter.GetConnectInfo(connString);
            _server = hs.GetMapValue<string>("Server");
            _accessKey = hs.GetMapValue<string>("AccessKey");
            _secretKey = hs.GetMapValue<string>("SecretKey");
            _bucketName = hs.GetMapValue<string>("BucketName");
            _internetUrl = hs.GetMapValue<string>("InternetUrl");
            _lanUrl = hs.GetMapValue<string>("LanUrl");
            _timeout = hs.GetMapValue<int>("timeout", 1000);
            _acl = GetACL(hs.GetMapValue<string>("acl"));
        }
        private S3CannedACL GetACL(string acl)
        {
            if(string.Equals(acl, "read", StringComparison.CurrentCultureIgnoreCase))
            {
                return S3CannedACL.PublicRead;
            }
            if (string.Equals(acl, "write", StringComparison.CurrentCultureIgnoreCase))
            {
                return S3CannedACL.PublicReadWrite;
            }
            return S3CannedACL.Private;
        }

        /// <summary>
        /// 哈希类型
        /// </summary>
        public override string HashName
        {
            get
            {
                return "MD5";
            }
        }
        /// <summary>
        /// 获取哈希类
        /// </summary>
        public override HashAlgorithm GetHash()
        {
            MD5 md5Hash = MD5CryptoServiceProvider.Create();
            return md5Hash;
        }

        /// <summary>
        /// 删除文件夹
        /// </summary>
        /// <param name="path">路径</param>
        public override APIResault RemoveDirectory(string path)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 指定位置追加文件内容
        /// </summary>
        /// <param name="path"></param>
        /// <param name="content"></param>
        /// <param name="postion"></param>
        /// <returns></returns>
        public override APIResault AppendFile(string path, Stream content, long postion)
        {
            throw new NotImplementedException();

        }
        /// <summary>
        /// 追加到末尾
        /// </summary>
        /// <param name="path"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public override APIResault AppendFile(string path, Stream content)
        {
            path = FormatKey(path);

            return AppendFile(path, content, GetFileLength(path));
        }

        /// <summary>
        /// 获取文件长度
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private long GetFileLength(string path)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 获取文件信息
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public override FileInfoBase GetFileInfo(string path)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 文件是否存在
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public override bool ExistsFile(string path)
        {
            throw new NotImplementedException();
        }
        public override APIResault Close()
        {
            _client = null;
            return ApiCommon.GetSuccess();
        }
        /// <summary>
        /// 获取文件夹
        /// </summary>
        /// <param name="path"></param>
        /// <param name="searchOption"></param>
        /// <returns></returns>
        public override List<string> GetDirectories(string path, System.IO.SearchOption searchOption)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 获取文件流
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public override System.IO.Stream GetFileStream(string path)
        {
            throw new NotImplementedException();
        }

        public override System.IO.Stream GetFileStream(string path, long postion)
        {
            throw new NotImplementedException();
        }
        private static readonly DateTime DefaultDate = new DateTime(1970, 1, 1);

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

        /// <summary>
        /// 获取地址
        /// </summary>
        /// <param name="url"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        public static string GetUrl(string url, string file)
        {
            if (file.StartsWith("/"))
            {
                file = file.TrimStart('/');
            }
            return url + "/" + file;
        }
        /// <summary>
        /// 格式化Key
        /// </summary>
        /// <param name="url"></param>
        private static string FormatKey(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                return url;
            }
            return url.TrimStart(' ', '/', '\\');
        }
        // <summary>
        /// 格式化Key
        /// </summary>
        /// <param name="url"></param>
        private static string FormatPathKey(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                return "/";
            }
            url = url.TrimStart(' ', '/', '\\');
            if (url[url.Length - 1] != '/')
            {
                url = url + "/";
            }
            return url;
        }
        /// <summary>
        /// 获取文件
        /// </summary>
        /// <param name="path"></param>
        /// <param name="searchOption"></param>
        /// <returns></returns>
        public override List<FileInfoBase> GetFiles(string path, System.IO.SearchOption searchOption)
        {
            throw new NotImplementedException();
        }




        public override APIResault Open()
        {
            AmazonS3Config config = new AmazonS3Config();
            config.ServiceURL = _server;
            _client = new AmazonS3Client(_accessKey,_secretKey,config);

            return ApiCommon.GetSuccess();
        }
        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public override APIResault RemoveFile(string path)
        {
            path = FormatKey(path);
            DeleteObjectRequest request = new DeleteObjectRequest();
            request.BucketName = _bucketName;
            request.Key = path;
            _client.DeleteObject(request);

            return ApiCommon.GetSuccess();
        }



        public override APIResault RenameFile(string source, string target)
        {

            throw new System.NotSupportedException("AWS不支持重命名文件");
        }

        /// <summary>
        /// 保存文件
        /// </summary>
        /// <param name="sourcePath">原路径</param>
        /// <param name="targetPath">目标路径</param>
        /// <returns></returns>
        public override APIResault SaveFile(string sourcePath, string targetPath)
        {
            targetPath = FormatKey(targetPath);
            using (FileStream file = new FileStream(sourcePath,FileMode.Open))
            {
                long len = file.Length;
                if (len < FileInfoBase.SLICE_UPLOAD_FILE_SIZE)
                {
                    SaveFileSingle(targetPath, file, len);
                }
                else
                {
                    SaveFileMultipart(targetPath, file, len);
                }
            }
            return ApiCommon.GetSuccess();
        }

        /// <summary>
        /// 单线上传文件
        /// </summary>
        /// <param name="path">目标目录</param>
        /// <param name="stream">流</param>
        /// <param name="contentLength">内容</param>
        /// <returns></returns>
        private APIResault SaveFileSingle(string path, Stream stream, long contentLength)
        {
            PutObjectRequest request = new PutObjectRequest();
            request.BucketName = _bucketName;
            request.Key = path;
            request.InputStream = stream;
            request.CannedACL = _acl;
            PutObjectResponse res = _client.PutObject(request);
            if (res.HttpStatusCode == HttpStatusCode.OK)
            {
                return ApiCommon.GetSuccess();
            }
            return ApiCommon.GetFault(null, res.HttpStatusCode);
        }
        /// <summary>
        /// 分块大小
        /// </summary>
        private const long PartSize = 5 * 1024 * 1024;
        /// <summary>
        /// 多块上传文件
        /// </summary>
        /// <param name="path">目标目录</param>
        /// <param name="stream">流</param>
        /// <param name="contentLength">内容</param>
        /// <returns></returns>
        private APIResault SaveFileMultipart(string path, Stream stream, long contentLength)
        {
            TransferUtilityUploadRequest uploadMultipartRequest = new TransferUtilityUploadRequest();
            uploadMultipartRequest.BucketName = _bucketName;
            uploadMultipartRequest.Key = path;
            uploadMultipartRequest.InputStream = stream;
            uploadMultipartRequest.PartSize = PartSize;
            uploadMultipartRequest.CannedACL = _acl;
            using (TransferUtility transferUtility = new TransferUtility(_client))
            {
                transferUtility.Upload(uploadMultipartRequest);
            }

            return ApiCommon.GetSuccess();
        }


        /// <summary>
        /// 保存文件
        /// </summary>
        /// <param name="path"></param>
        /// <param name="stream"></param>
        /// <returns></returns>
        public override APIResault SaveFile(string path, Stream stream, long contentLength)
        {
            path = FormatKey(path);
            if(contentLength< FileInfoBase.SLICE_UPLOAD_FILE_SIZE)
            {
                SaveFileSingle(path, stream, contentLength);
            }
            else
            {
                SaveFileMultipart(path, stream, contentLength);
            }
            return ApiCommon.GetSuccess();
        }

        

        public override void Dispose()
        {
            Close();
        }
        /// <summary>
        /// 文件夹是否存在
        /// </summary>
        /// <param name="folder"></param>
        /// <returns></returns>
        public override bool ExistDirectory(string folder)
        {
            folder = FormatPathKey(folder);
            GetObjectRequest request = new GetObjectRequest();

            request.BucketName = _bucketName;
            request.Key = folder;
            try
            {

                S3FileInfo file = new S3FileInfo(_client, _bucketName, folder);
                return file.Exists;

            }
            catch(Exception ex)
            {
                return false;
            }
           
        }
        /// <summary>
        /// 创建文件夹
        /// </summary>
        /// <param name="folder"></param>
        /// <returns></returns>
        public override APIResault CreateDirectory(string folder)
        {
            throw new NotImplementedException();
        }

        public override void ReadFileToStream(string path, Stream stm, long postion, long length)
        {
            throw new NotImplementedException();
        }
    }
}
