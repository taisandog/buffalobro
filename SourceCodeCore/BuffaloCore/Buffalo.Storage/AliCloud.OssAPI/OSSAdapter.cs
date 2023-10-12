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
using System.Net;

namespace Buffalo.Storage.AliCloud.OssAPI
{
    public class OSSAdapter : IFileStorage
    {

        private OssClient _cloud;
        private string _checkPointDir = null;
        

        /// <summary> 
        /// 阿里云适配器
        /// </summary>
        /// <param name="connString">连接字符串</param>
        public OSSAdapter(string connString)
        {
            Dictionary<string, string> hs = ConnStringFilter.GetConnectInfo(connString);
            FillBaseConfig(hs);
            _checkPointDir = hs.GetDicValue<string, string>("cpDir");


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
            string curPath = FileInfoBase.FormatKey(path);
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
            path = FileInfoBase.FormatKey(path);

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
            path = FileInfoBase.FormatKey(path);

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
            path = FileInfoBase.FormatKey(path);
            
            OssObject result = _cloud.GetObject(_bucketName, path);

            IDictionary<string, string> dic = result.ResponseMetadata;

            string url = FileInfoBase.CombineUriToString(_internetUrl, result.Key);
            string accessUrl = FileInfoBase.CombineUriToString(_lanUrl, result.Key);
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
            path = FileInfoBase.FormatKey(path);
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
            string curPath = FileInfoBase.FormatPathKey(path);
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
            path = FileInfoBase.FormatKey(path);
            OssObject obj = _cloud.GetObject(_bucketName, path);
            return obj.Content;
        }

        public override System.IO.Stream GetFileStream(string path, long postion,long length)
        {
            path = FileInfoBase.FormatKey(path);
            long filelength = GetFileLength(path);

            long end = FileInfoBase.GetRangeEnd(postion, length, filelength);

            GetObjectRequest getObjectRequest = new GetObjectRequest(_bucketName, path);
            getObjectRequest.SetRange(postion, end);
            using (OssObject obj = _cloud.GetObject(getObjectRequest))
            {
                MemoryStream stm = new MemoryStream();
                CommonMethods.CopyStreamData(obj.Content, stm, -1);
                return stm;
            }
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

            string curPath = FileInfoBase.FormatKey(path);

            

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
                    url = FileInfoBase.CombineUriToString(_internetUrl ,summary.Key);
                    accessUrl= FileInfoBase.CombineUriToString(_lanUrl , summary.Key);
                    
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

            if (!string.IsNullOrWhiteSpace(_proxyHost))
            {
                conf.ProxyHost = _proxyHost;
                conf.ProxyPort = _proxyPort;
                if (!string.IsNullOrWhiteSpace(_proxyUser))
                {
                    conf.ProxyUserName = _proxyUser;
                    conf.ProxyPassword = _proxyPass;
                }
            }
            _cloud = new OssClient(_server, _secretId, _secretKey, conf);

            
            return ApiCommon.GetSuccess();
        }
        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public override APIResault RemoveFile(string path)
        {
            path = FileInfoBase.FormatKey(path);
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
            targetPath = FileInfoBase.FormatKey(targetPath);
            PutObjectResult res = null;
            FileInfo finfo=new FileInfo(sourcePath);
            long len=finfo.Length;
            ObjectMetadata objectMeta = new ObjectMetadata();
            if (_needHash)
            {
                
                
                using (FileStream fs = File.Open(sourcePath, FileMode.Open))
                {
                    string md5 = OssUtils.ComputeContentMd5(fs, fs.Length);
                    objectMeta.ContentMd5 = md5;
                }

                //res = _cloud.PutObject(_bucketName, targetPath, sourcePath, objectMeta);
                if (len > FileInfoBase.SLICE_UPLOAD_FILE_SIZE)
                {
                    res = _cloud.ResumableUploadObject(_bucketName, targetPath, sourcePath, objectMeta,_checkPointDir);
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
                    res = _cloud.ResumableUploadObject(_bucketName, targetPath, sourcePath, objectMeta, _checkPointDir);
                }
                else
                {
                    res = _cloud.PutObject(_bucketName, targetPath, sourcePath, objectMeta);
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
            path = FileInfoBase.FormatKey(path);
            PutObjectResult res = null;
            long len = contentLength;
            if (len <= 0)
            {
                len = stream.Length;
            }
            ObjectMetadata objectMeta = new ObjectMetadata();
            if (objectMeta.ContentType == null)
            {
                objectMeta.ContentType = HttpUtils.GetContentType(path, path);
            }
            if (_needHash)
            {
                using (MemoryStream mstm = new MemoryStream())
                {
                    CommonMethods.CopyStreamData(stream, mstm);
                    string md5 = OssUtils.ComputeContentMd5(mstm, len);

                    objectMeta.ContentMd5 = md5;
                    mstm.Position = 0;
                    //if (len > FileInfoBase.SLICE_UPLOAD_FILE_SIZE)
                    //{
                    //    res = _cloud.ResumableUploadObject(_bucketName, path, mstm, objectMeta, _checkPointDir);
                    //}
                    //else
                    //{
                        res = _cloud.PutObject(_bucketName, path, mstm, objectMeta);
                    //}
                }
            }
            else
            {
                stream.Position = 0;
                //if (len > FileInfoBase.SLICE_UPLOAD_FILE_SIZE)
                //{

                //    //res = _cloud.PutBigObject(_bucketName, path, stream, objectMeta);
                //    res= _cloud.ResumableUploadObject(_bucketName, path, stream, objectMeta, _checkPointDir);
                //}
                //else
                //{
                    res = _cloud.PutObject(_bucketName, path, stream, objectMeta);
                //}

            }
            if(res.HttpStatusCode!= HttpStatusCode.OK) 
            {
                return ApiCommon.GetFault(res.HttpStatusCode.ToString());
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
            folder = FileInfoBase.FormatPathKey(folder);
            return _cloud.DoesObjectExist(_bucketName,folder);
        }
        /// <summary>
        /// 创建文件夹
        /// </summary>
        /// <param name="folder"></param>
        /// <returns></returns>
        public override APIResault CreateDirectory(string folder)
        {
            folder = FileInfoBase.FormatPathKey(folder);
            ObjectMetadata objectMeta = new ObjectMetadata();
            using (MemoryStream stm = new MemoryStream())
            {
                _cloud.PutObject(_bucketName, folder, stm, objectMeta);
            }
            return ApiCommon.GetSuccess();
        }

        public override void ReadFileToStream(string path, Stream stm, long postion, long length)
        {
            path = FileInfoBase.FormatKey(path);

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
