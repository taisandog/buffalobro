using Aliyun.OSS;
using Aliyun.OSS.Common;
using Aliyun.OSS.Util;
using Buffalo.ArgCommon;
using Buffalo.Storage.QCloud.CosApi;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Buffalo.Kernel;

namespace Buffalo.Storage.AliCloud.OssAPI
{
    //SecretId=LTAIPM7uERWhDShn;SecretKey=YZiGsJMxUSxEQYeDJHmuSkgQp8HG9z
    public class OSSAdapter : IFileStorage
    {

        private OssClient _cloud;
        /// <summary>
        /// 服务器地址
        /// </summary>
        private string _server;
        /// <summary>
        /// 安全ID
        /// </summary>
        private string _SECRETID;
        /// <summary>
        /// 安全Key
        /// </summary>
        private string _SECRETKEY;
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
        /// <summary>
        /// 需要上传效验
        /// </summary>
        private bool _needHash;
        /// <summary> 
        /// 阿里云适配器
        /// </summary>
        /// <param name="connString">连接字符串</param>
        public OSSAdapter(string connString)
        {
            Dictionary<string, string> hs = ConnStringFilter.GetConnectInfo(connString);
            _server = hs.GetMapValue<string>("Server");
            _SECRETID = hs.GetMapValue<string>("SecretId");
            _SECRETKEY = hs.GetMapValue<string>("SecretKey");
            _bucketName = hs.GetMapValue<string>("bucketName");
            _internetUrl= hs.GetMapValue<string>("InternetUrl");
            _lanUrl = hs.GetMapValue<string>("LanUrl");
            _needHash= hs.GetMapValue<int>("needHash")==1;
            _timeout = hs.GetMapValue<int>("timeout",1000);
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
            ListObjectsRequest listObjectsRequest = new ListObjectsRequest(_bucketName);
            string nextMarker = string.Empty;
            string curPath = FormatKey(path);
            ObjectListing result = null;
            listObjectsRequest.Prefix = curPath;
            listObjectsRequest.MaxKeys = 100;
            List<string> curKeys = new List<string>();
            do
            {
                
                listObjectsRequest.Marker = nextMarker;
                result = _cloud.ListObjects(listObjectsRequest);
                curKeys.Clear();
                foreach (OssObjectSummary summary in result.ObjectSummaries)
                {
                    curKeys.Add(summary.Key);
                }
                DeleteObjectsRequest drequest = new DeleteObjectsRequest(_bucketName, curKeys,true);
                DeleteObjectsResult dres=_cloud.DeleteObjects(drequest);
                
                nextMarker = result.NextMarker;
            } while (result.IsTruncated);
            _cloud.DeleteObject(_bucketName, curPath);
            curKeys = null;
            return ApiCommon.GetSuccess();
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
            path = FormatKey(path);

            AppendObjectRequest request = new AppendObjectRequest(_bucketName, path);

            request.ObjectMetadata = new ObjectMetadata();
            request.Content = content;
            request.Position = postion;

            AppendObjectResult result = _cloud.AppendObject(request);
            // 设置下次追加文件时的position位置
            postion = result.NextAppendPosition;

            return ApiCommon.GetSuccess();

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
            long position = 0;
            
            try
            {
                ObjectMetadata metadata = _cloud.GetObjectMetadata(_bucketName, path);
                position = metadata.ContentLength;
            }
            catch { }
            return position;
        }

        /// <summary>
        /// 获取文件信息
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public override FileInfoBase GetFileInfo(string path)
        {
            path = FormatKey(path);
            
            OssObject result = _cloud.GetObject(_bucketName, path);

            IDictionary<string, string> dic = result.ResponseMetadata;

            string url = GetUrl(_internetUrl, result.Key);
            string accessUrl = GetUrl(_lanUrl, result.Key);
            DateTime ctime = ValueConvertExtend.GetDicValue<string,string>(dic, "Date").ConvertTo<DateTime>();
            DateTime utime = ValueConvertExtend.GetDicValue<string, string>(dic, "Date").ConvertTo<DateTime>();
            string etag = ValueConvertExtend.GetDicValue<string, string>(dic, "ETag");
            long len = result.ContentLength;
            NetStorageFileInfo info = new NetStorageFileInfo(ctime, utime,
                path + "/" + result.Key, url, accessUrl, etag, len);

            return info;
        }
        /// <summary>
        /// 文件是否存在
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public override bool ExistsFile(string path)
        {
            path = FormatKey(path);
            return _cloud.DoesObjectExist(_bucketName, path);
        }
        public override APIResault Close()
        {
            _cloud = null;
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
            List<string> lst = new List<string>();
            ListObjectsRequest listObjectsRequest = new ListObjectsRequest(_bucketName);
            string curPath =FormatPathKey(path);
            //if (string.IsNullOrWhiteSpace(curPath) || curPath == "/")
            //{
            //    curPath = "";
            //}

            string nextMarker = string.Empty;
            listObjectsRequest.Prefix = curPath;

            listObjectsRequest.Delimiter = "/";

            ObjectListing result = _cloud.ListObjects(listObjectsRequest);

            

            foreach (string prefix in result.CommonPrefixes)
            {

                lst.Add(prefix);
            }


            return lst;
        }
        /// <summary>
        /// 获取文件流
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public override System.IO.Stream GetFileStream(string path)
        {
            path = FormatKey(path);
            OssObject obj = _cloud.GetObject(_bucketName, path);
            return obj.Content;
        }

        public override System.IO.Stream GetFileStream(string path, long postion)
        {
            path = FormatKey(path);
            long filelength = GetFileLength(path);

            long end = FileInfoBase.GetRangeEnd(postion, -1, filelength);

            GetObjectRequest getObjectRequest = new GetObjectRequest(_bucketName, path);
            getObjectRequest.SetRange(postion, end);
            using (OssObject obj = _cloud.GetObject(getObjectRequest))
            {
                MemoryStream stm = new MemoryStream();
                CommonMethods.CopyStreamData(obj.Content, stm, -1, null);
                return stm;
            }
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
        public static string GetUrl(string url,string file)
        {
            if (file.StartsWith("/"))
            {
                file = file.TrimStart('/');
            }
            return url+"/" + file;
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
            url= url.TrimStart(' ', '/', '\\');
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

            List<FileInfoBase> lst = new List<FileInfoBase>();
            ListObjectsRequest listObjectsRequest = new ListObjectsRequest(_bucketName);
            string nextMarker = string.Empty;

            string curPath = FormatKey(path);

            

            ObjectListing result = null;
            listObjectsRequest.Prefix = curPath;
            if (searchOption == SearchOption.TopDirectoryOnly)
            {
                listObjectsRequest.Delimiter = "/";
            }
            listObjectsRequest.MaxKeys = 100;
            string url = null;
            string accessUrl = null;
            do
            {
                
                listObjectsRequest.Marker = nextMarker;
                result = _cloud.ListObjects(listObjectsRequest);

                foreach (OssObjectSummary summary in result.ObjectSummaries)
                {
                    url = GetUrl(_internetUrl ,summary.Key);
                    accessUrl= GetUrl(_lanUrl , summary.Key);
                    
                    NetStorageFileInfo info = new NetStorageFileInfo(summary.LastModified, summary.LastModified,
                        summary.Key,url,accessUrl, summary.ETag,summary.Size);
                    lst.Add(info);
                }
                nextMarker = result.NextMarker;
            } while (result.IsTruncated);
            return lst;
        }




        public override APIResault Open()
        {
            ClientConfiguration conf = new ClientConfiguration();
            conf.MaxErrorRetry = 3;
            conf.ConnectionTimeout = _timeout;
            
            _cloud = new OssClient(_server, _SECRETID, _SECRETKEY, conf);

            
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
            _cloud.DeleteObject(_bucketName, path);
            return ApiCommon.GetSuccess();
        }



        public override APIResault RenameFile(string source, string target)
        {
            
            throw new System.NotSupportedException("OSS不支持重命名文件");
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
            PutObjectResult res = null;
            FileInfo finfo=new FileInfo(sourcePath);
            long len=finfo.Length;
            if (_needHash)
            {
                ObjectMetadata objectMeta = new ObjectMetadata();
                using (FileStream fs = File.Open(sourcePath, FileMode.Open))
                {
                    string md5 = OssUtils.ComputeContentMd5(fs, fs.Length);
                    objectMeta.ContentMd5 = md5;
                }

                //res = _cloud.PutObject(_bucketName, targetPath, sourcePath, objectMeta);
                if (len > FileInfoBase.SLICE_UPLOAD_FILE_SIZE)
                {
                    res = _cloud.PutBigObject(_bucketName, targetPath, sourcePath, objectMeta);
                }
                else 
                {
                    res = _cloud.PutObject(_bucketName, targetPath, sourcePath, objectMeta);
                }
            }
            else
            {
                if (len > FileInfoBase.SLICE_UPLOAD_FILE_SIZE)
                {
                    res = _cloud.PutBigObject(_bucketName, targetPath, sourcePath, new ObjectMetadata());
                }
                else
                {
                    res = _cloud.PutObject(_bucketName, targetPath, sourcePath, new ObjectMetadata());
                }
            }
            return ApiCommon.GetSuccess();
        }
        /// <summary>
        /// 保存文件
        /// </summary>
        /// <param name="path"></param>
        /// <param name="stream"></param>
        /// <returns></returns>
        public override APIResault SaveFile(string path, Stream stream,long contentLength)
        {
            path = FormatKey(path);
            PutObjectResult res = null;
            long len = contentLength;
            if (len <= 0)
            {
                len = stream.Length;
            }
            if (_needHash)
            {
                string md5 = OssUtils.ComputeContentMd5(stream, len);

                ObjectMetadata objectMeta = new ObjectMetadata();

                objectMeta.ContentMd5 = md5;
                if (len > FileInfoBase.SLICE_UPLOAD_FILE_SIZE)
                {
                    res = _cloud.PutBigObject(_bucketName, path, stream, objectMeta);
                }
                else
                {
                    res = _cloud.PutObject(_bucketName, path, stream, objectMeta);
                }
            }
            else
            {
                if (len > FileInfoBase.SLICE_UPLOAD_FILE_SIZE)
                {
                    res = _cloud.PutBigObject(_bucketName, path, stream, new ObjectMetadata());
                }
                else
                {
                    res = _cloud.PutObject(_bucketName, path, stream, new ObjectMetadata());
                }

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
            return _cloud.DoesObjectExist(_bucketName,folder);
        }
        /// <summary>
        /// 创建文件夹
        /// </summary>
        /// <param name="folder"></param>
        /// <returns></returns>
        public override APIResault CreateDirectory(string folder)
        {
            folder = FormatPathKey(folder);
            ObjectMetadata objectMeta = new ObjectMetadata();
            using (MemoryStream stm = new MemoryStream())
            {
                _cloud.PutObject(_bucketName, folder, stm, objectMeta);
            }
            return ApiCommon.GetSuccess();
        }

        public override void ReadFileToStream(string path, Stream stm, long postion, long length)
        {
            path = FormatKey(path);

            FileInfoBase info = GetFileInfo(path);
            if (info == null)
            {
                throw new FileNotFoundException(path + " 不存在");
            }
            long end = FileInfoBase.GetRangeEnd(postion, length, info.Length);
            GetObjectRequest getObjectRequest = new GetObjectRequest(_bucketName, path);
            getObjectRequest.SetRange(postion, end);
            using (OssObject obj = _cloud.GetObject(getObjectRequest))
            {
                CommonMethods.CopyStreamData(obj.Content, stm);
            }
        }
    }
}
