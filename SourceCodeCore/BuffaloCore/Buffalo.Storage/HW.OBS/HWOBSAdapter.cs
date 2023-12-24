using Buffalo.ArgCommon;
using Buffalo.Kernel;
using Buffalo.Storage.AliCloud.OssAPI;
using Buffalo.Storage.QCloud.CosApi;
using OBS;
using OBS.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Buffalo.Storage.HW.OBS
{
    /// <summary>
    /// 华为OBS适配器
    /// </summary>
    public partial class HWOBSAdapter : IFileStorage
    {


        private ObsClient _client;

        private ObsConfig _config;
        public override object ConfigInfo
        {
            get
            {
                return _config;
            }
        }
        /// <summary> 
        /// 阿里云适配器
        /// </summary>
        /// <param name="connString">连接字符串</param>
        public HWOBSAdapter(string connString)
        {
            Dictionary<string, string> hs = ConnStringFilter.GetConnectInfo(connString);
            FillBaseConfig(hs);

            _config = CreateConfig();
        }

        private ObsConfig CreateConfig() 
        {
            ObsConfig config = new ObsConfig();
            config.Timeout = _timeout;
            config.Endpoint = _server;
            if (!string.IsNullOrWhiteSpace(_proxyHost))
            {
                config.ProxyHost = _proxyHost;
                config.ProxyPort = _proxyPort;
                if (!string.IsNullOrWhiteSpace(_proxyUser))
                {
                    config.ProxyUserName = _proxyUser;
                    config.ProxyPassword = _proxyPass;
                }
            }
            return config;
        }
        /// <summary>
        /// 打开
        /// </summary>
        /// <returns></returns>
        public override APIResault Open()
        {
            

            _client = new ObsClient(_secretId, _secretKey, _config);
            return ApiCommon.GetSuccess();
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
        /// 删除文件
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public override APIResault RemoveFile(string path)
        {
            path = GetFilePath(path);
            DeleteObjectRequest request = new DeleteObjectRequest()
            {
                BucketName = _bucketName,
                ObjectKey = path,
            };
            try
            {
                DeleteObjectResponse response = _client.DeleteObject(request);
                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    return ApiCommon.GetFault("删除错误:" + response.StatusCode);
                }
            }
            catch (ObsException ex)
            {
                return ApiCommon.GetException(ex);
            }
            return ApiCommon.GetSuccess();
        }

        /// <summary>
        /// 删除文件夹
        /// </summary>
        /// <param name="path">路径</param>
        public override APIResault RemoveDirectory(string path)
        {

            path = GetPath(path);


            ListObjectsRequest request = new ListObjectsRequest()
            {
                BucketName = _bucketName,
                Prefix = path,

            };
            
            ListObjectsResponse response = null;
            List<FileInfoBase> lst = new List<FileInfoBase>();
            request.MaxKeys = 20;
            do
            {
                response = _client.ListObjects(request);

                DeleteObjectsRequest drequest = new DeleteObjectsRequest();
                drequest.BucketName = _bucketName;
                drequest.Quiet = true;


                foreach (ObsObject entry in response.ObsObjects)
                {
                    drequest.AddKey(entry.ObjectKey);
                }
                DeleteObjectsResponse dresponse = _client.DeleteObjects(drequest);
                request.Marker = response.NextMarker;
            }
            while (response.IsTruncated);

            
            return ApiCommon.GetSuccess();
        }

        public override APIResault AppendFile(string path, Stream content, long postion)
        {
            path = GetFilePath(path);
            long pos = 0;
            try
            {
                GetObjectMetadataResponse metadata = _client.GetObjectMetadata(_bucketName, path);

                if (metadata != null)
                {
                    pos = metadata.NextPosition;
                }
            }
            catch { }
            AppendObjectRequest request = new AppendObjectRequest()
            {
                BucketName = _bucketName,
                ObjectKey = path,
                InputStream = content,
                Position = pos
            };
            
            AppendObjectResponse response = _client.AppendObject(request);
            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                return ApiCommon.GetFault("追加错误:" + response.StatusCode);
            }
            return ApiCommon.GetSuccess();
        }

        public override APIResault AppendFile(string path, Stream content)
        {
            return AppendFile(path, content, 0);
        }

        public override APIResault Close()
        {
            _client = null;
            return ApiCommon.GetSuccess();
        }

        public override List<string> GetDirectories(string path, SearchOption searchOption)
        {
            List<string> lst = new List<string>();
            ListObjectsRequest listObjectsRequest = new ListObjectsRequest();
            string curPath =  GetPath(path); 
            

            string nextMarker = string.Empty;
            listObjectsRequest.Prefix = curPath;
            listObjectsRequest.Delimiter = "/";
            listObjectsRequest.BucketName = _bucketName;

            ListObjectsResponse result = _client.ListObjects(listObjectsRequest);

            foreach (string prefix in result.CommonPrefixes)
            {

                lst.Add(prefix);
            }


            return lst;
        }

        public override Stream GetFileStream(string path)
        {
            path = GetFilePath(path);
            GetObjectRequest request = new GetObjectRequest()
            {
                BucketName = _bucketName,
                ObjectKey = path,

            };
            MemoryStream stm = new MemoryStream();
            using (GetObjectResponse response = _client.GetObject(request))
            {
                CommonMethods.CopyStreamData(response.OutputStream, stm);
            }
            return stm;
        }

        public override Stream GetFileStream(string path, long postion, long length)
        {
            path = GetFilePath(path);
            FileInfoBase info = GetFileInfo(path);
            if (info == null)
            {
                return null;
            }
            long end = FileInfoBase.GetRangeEnd(postion, length, info.Length);
            ByteRange byteRange = new ByteRange(postion, end);
            GetObjectRequest request = new GetObjectRequest()
            {
                BucketName = _bucketName,
                ObjectKey = path,
                ByteRange = byteRange,
            };
            MemoryStream stm = new MemoryStream();
            using (GetObjectResponse response = _client.GetObject(request))
            {
                CommonMethods.CopyStreamData(response.OutputStream, stm);
            }
            return stm;
        }

        public override FileInfoBase GetFileInfo(string path)
        {
            path = GetFilePath(path);
            try
            {
                GetObjectMetadataRequest request = new GetObjectMetadataRequest()
                {
                    BucketName = _bucketName,
                    ObjectKey = path
                };
                GetObjectMetadataResponse response = _client.GetObjectMetadata(request);
                string url = FileInfoBase.CombineUriToString(_internetUrl, request.ObjectKey);
                string accessUrl = FileInfoBase.CombineUriToString(_internetUrl, request.ObjectKey);

                NetStorageFileInfo info = new NetStorageFileInfo(response.LastModified.GetValueOrDefault(), response.LastModified.GetValueOrDefault(),
                path + "/" + response.ObjectKey, url, accessUrl, response.ETag, response.ContentLength);

                return info;
                
            }
            catch (ObsException ex)
            {
                return null;
            }
        }
        internal static string GetPath(string path)
        {
            path = path.TrimStart('/', '\\', ' ');
            if (!string.IsNullOrWhiteSpace(path))
            {
                if (path[path.Length - 1] != '/')
                {
                    path = path + "/";
                }
            }
            return path;
        }
        private static string GetFilePath(string path)
        {
            path = path.Trim('/', '\\', ' ');
            
            return path;
        }
        public override List<FileInfoBase> GetFiles(string path, SearchOption searchOption)
        {

            path = GetPath(path);


            ListObjectsRequest request = new ListObjectsRequest()
            {
                BucketName = _bucketName,
                Prefix = path,
                
            };
            if (searchOption == SearchOption.TopDirectoryOnly)
            {
                request.Delimiter = "/";
            }
            ListObjectsResponse response = null;
            List<FileInfoBase> lst = new List<FileInfoBase>();
            request.MaxKeys = 50;
            do
            {
                response = _client.ListObjects(request);
                string url = null;
                string accessUrl = null;

                foreach (ObsObject entry in response.ObsObjects)
                {
                    if (entry.ObjectKey.EndsWith("/"))
                    {
                        continue;
                    }
                    url = FileInfoBase.CombineUriToString(_internetUrl, entry.ObjectKey);
                    accessUrl = FileInfoBase.CombineUriToString(_lanUrl, entry.ObjectKey);
                    NetStorageFileInfo info = new NetStorageFileInfo(entry.LastModified.GetValueOrDefault(), entry.LastModified.GetValueOrDefault(),
                            entry.ObjectKey, url, accessUrl, entry.ETag, entry.Size);
                    lst.Add(info);

                }
                request.Marker = response.NextMarker;
            }
            while (response.IsTruncated);

            return lst;
        }

        public override APIResault RenameFile(string source, string target)
        {
            throw new System.NotSupportedException("OBS不支持重命名文件");
        }

        public override APIResault SaveFile(string path, Stream content,long contentLength)
        {
            path = GetFilePath(path);
            long len = content.Length;
            if (len > FileInfoBase.SLICE_UPLOAD_FILE_SIZE)
            {
                return StreamPartUpload(path, content, contentLength);
            }
            return StreamUpload(path, content);
        }

        public override APIResault SaveFile(string sourcePath, string targetPath)
        {
            targetPath = GetFilePath(targetPath);
            FileInfo finfo = new FileInfo(sourcePath);
            long len = finfo.Length;
            if (len > FileInfoBase.SLICE_UPLOAD_FILE_SIZE)
            {
                return FilePartUpload(targetPath, sourcePath);
            }
            return FileUpload(targetPath, sourcePath);
        }

        public override bool ExistDirectory(string folder)
        {
            folder = GetPath(folder);
            if (string.IsNullOrWhiteSpace(folder) || folder == "/")
            {
                return true;
            }
            ListObjectsRequest request = new ListObjectsRequest()
            {
                BucketName = _bucketName,
                Prefix = folder,

            };
            request.Delimiter = "/";

            ListObjectsResponse response = null;
            List<FileInfoBase> lst = new List<FileInfoBase>();
            request.MaxKeys = 1;
            response = _client.ListObjects(request);

            if (response.ObsObjects.Count > 0)
            {
                return true;
            }
            return false;
        }

        public override bool ExistsFile(string path)
        {
            path = GetFilePath(path);
           
            try
            {
                GetObjectMetadataResponse metadata = _client.GetObjectMetadata(_bucketName, path);

                if (metadata != null)
                {
                    return true;
                }
            }
            catch { }
            return false;
        }

        public override APIResault CreateDirectory(string folder)
        {
            folder = GetPath(folder);
            PutObjectRequest request = new PutObjectRequest()
            {
                BucketName = _bucketName,
                ObjectKey = folder,
                
            };
            PutObjectResponse response = _client.PutObject(request);
            return ApiCommon.GetSuccess();
        }

        public override void Dispose()
        {
            Close();
        }

        public override void ReadFileToStream(string path, Stream stm, long postion, long length)
        {
            path = GetFilePath(path);
            FileInfoBase info = GetFileInfo(path);
            if (info == null)
            {
                throw new FileNotFoundException(path+" 不存在");
            }
            long end = FileInfoBase.GetRangeEnd(postion, length, info.Length);
            ByteRange byteRange = new ByteRange(postion, end);

            GetObjectRequest request = new GetObjectRequest()
            {
                BucketName = _bucketName,
                ObjectKey = path,
                ByteRange = byteRange
            };
            
            using (GetObjectResponse response = _client.GetObject(request))
            {
                CommonMethods.CopyStreamData(response.OutputStream, stm);
            }
        }
    }
}
