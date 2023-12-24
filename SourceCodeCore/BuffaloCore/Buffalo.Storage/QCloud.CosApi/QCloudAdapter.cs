using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Buffalo.Kernel;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using Buffalo.ArgCommon;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using COSXML;
using static COSXML.CosXmlConfig;
using COSXML.Auth;
using COSXML.Model.Object;
using COSXML.Model.Bucket;
using COSXML.Model.Tag;
using COSXML.Model;

namespace Buffalo.Storage.QCloud.CosApi
{

    /// <summary>
    /// 腾讯云适配器
    /// </summary>
    public class QCloudAdapter : IFileStorage
    {
        
        private CosXml _cloud;
        /// <summary>
        /// APP ID
        /// </summary>
        private int _APPID;
        /// <summary>
        /// 配置
        /// </summary>
        private CosXmlConfig _cosConfig;
        /// <summary>
        /// 配置
        /// </summary>
        public CosXmlConfig CosConfig 
        { 
            get { return _cosConfig; } 
            set { _cosConfig = value; }
        }
        /// <summary>
        /// 登录构造器
        /// </summary>
        private QCloudCredentialProvider _cosCredentialProvider;
        /// <summary>
        /// 登录构造器
        /// </summary>
        public QCloudCredentialProvider CosCredentialProvider 
        {
            get { return _cosCredentialProvider; }
            set { _cosCredentialProvider = value; }
        }

        public override object ConfigInfo
        {
            get
            {
                return _cosConfig;
            }
        }
        /// <summary> 
        /// 腾讯云适配器
        /// </summary>
        /// <param name="connString">连接字符串</param>
        public QCloudAdapter(string connString) 
        {
            Dictionary<string, string> hs = ConnStringFilter.GetConnectInfo(connString);
            _APPID = hs.GetMapValue<int>("AppId");
            

            FillBaseConfig(hs);

            FillConfig();
        }

        /// <summary>
        /// 填充配置信息
        /// </summary>
        private void FillConfig() 
        {
            string region = GetRegion(_server);

            Builder builder = new CosXmlConfig.Builder()
              .IsHttps(true)  //设置默认 HTTPS 请求
              .SetRegion(region)  //设置一个默认的存储桶地域
              .SetDebugLog(true);  //显示日志
            if (!string.IsNullOrWhiteSpace(_proxyHost))
            {
                builder.SetProxyHost(_proxyHost);
                builder.SetProxyPort(_proxyPort);

                if (!string.IsNullOrWhiteSpace(_proxyUser))
                {
                    builder.SetProxyUserName(_proxyUser);
                    builder.SetProxyUserPassword(_proxyPass);
                }
            }
            if (_timeout > 0)
            {
                builder.SetConnectionTimeoutMs(_timeout);
            }
            _cosConfig = builder.Build();

            ReCredential();


        }
        /// <summary>
        /// 重新注册登录
        /// </summary>
        public void ReCredential() 
        {
            long durationSecond = 600;  //每次请求签名有效时长，单位为秒
            _cosCredentialProvider = new DefaultQCloudCredentialProvider(
              _secretId, _secretKey, durationSecond);
        }

        /// <summary>
        /// 删除文件夹
        /// </summary>
        /// <param name="path">路径</param>
        public override APIResault RemoveDirectory(string path)
        {
            DeleteObjectRequest request = new DeleteObjectRequest(_bucketName, path);
            //执行请求
            DeleteObjectResult result = _cloud.DeleteObject(request);
            return GetCommResault(result);
        }

       

        public override APIResault AppendFile(string path, Stream content, long postion)
        {
            byte[] bcontent = CommonMethods.LoadStreamData2(content);
            AppendObjectRequest request = new AppendObjectRequest(_bucketName, path, bcontent, postion);
            //设置进度回调
            request.SetCosProgressCallback(delegate (long completed, long total)
            {
                //Console.WriteLine(String.Format("progress = {0:##.##}%", completed * 100.0 / total));
            });
            AppendObjectResult result = _cloud.AppendObject(request);
            //获取下次追加位置
            long nextPosition = result.nextAppendPosition;
           
            return GetCommResault(result);
        }

        public override APIResault AppendFile(string path, Stream content)
        {
            return AppendFile(path, content, 0);
        }
        /// <summary>
        /// 获取文件信息
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public override FileInfoBase GetFileInfo(string path)
        {
            HeadObjectRequest request = new HeadObjectRequest(_bucketName, path);
            //执行请求
            HeadObjectResult result = _cloud.HeadObject(request);


            return ToFileBaseInfo(result, path);
        }
        public override APIResault Close()
        {
            _cloud = null;
            return ApiCommon.GetSuccess();
        }

        public override List<string> GetDirectories(string path, SearchOption searchOption)
        {
            List<string> ret = new List<string>();
            FillDirectories(path, searchOption, ret);

            return ret;
        }

        /// <summary>
        /// 填充目录
        /// </summary>
        /// <param name="path"></param>
        /// <param name="searchOption"></param>
        /// <param name="ret"></param>
        private void FillDirectories(string path, SearchOption searchOption, List<string> ret) 
        {
            GetBucketRequest request = new GetBucketRequest(_bucketName);
            request.SetPrefix(path);
            request.SetDelimiter("/");
            GetBucketResult result = _cloud.GetBucket(request);
            ListBucket info = result.listBucket;
            List<ListBucket.CommonPrefixes> subDirs = info.commonPrefixesList;

            
            foreach (ListBucket.CommonPrefixes item in subDirs)
            {

                ret.Add(item.prefix);
            }
            if (searchOption != SearchOption.AllDirectories) 
            {
                return;
            }
            foreach (ListBucket.CommonPrefixes item in subDirs)
            {
                FillDirectories(path, searchOption, ret);
            }
        }


        /// <summary>
        /// 获取文件流
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public override System.IO.Stream GetFileStream(string path)
        {
            FileInfoBase info = GetFileInfo(path);
            if (info == null)
            {
                return null;
            }
            MemoryStream stm = new MemoryStream();
            HttpWebRequest myWebRequest = (HttpWebRequest)WebRequest.Create(info.FilePath);
            using (HttpWebResponse myWebResponse = (HttpWebResponse)myWebRequest.GetResponse())
            {
                using (Stream readStream = myWebResponse.GetResponseStream())
                {
                    CommonMethods.CopyStreamData(readStream, stm);
                }
            }
            stm.Position = 0;
            return stm;
        }

        public override System.IO.Stream GetFileStream(string path, long postion, long length)
        {
            FileInfoBase info = GetFileInfo(path);
            if (info == null)
            {
                return null;
            }
            MemoryStream stm = new MemoryStream();
            HttpWebRequest myWebRequest = (HttpWebRequest)WebRequest.Create(info.FilePath);
            using (HttpWebResponse myWebResponse = (HttpWebResponse)myWebRequest.GetResponse())
            {
                using (Stream readStream = myWebResponse.GetResponseStream())
                {
                    SkipToPostion(readStream, postion);
                    CommonMethods.CopyStreamData(readStream, stm, length);
                }
            }
            stm.Position = 0;
            return stm;
        }
        private static readonly DateTime DefaultDate = new DateTime(1970, 1, 1);
        /// <summary>
        /// 获取文件
        /// </summary>
        /// <param name="path"></param>
        /// <param name="searchOption"></param>
        /// <returns></returns>
        public override List<FileInfoBase> GetFiles(string path, System.IO.SearchOption searchOption)
        {
            
            List<FileInfoBase> ret = new List<FileInfoBase>();

            FillFiles(path, searchOption, ret);
            return ret;
        }

        /// <summary>
        /// 填充文件
        /// </summary>
        /// <param name="path"></param>
        /// <param name="searchOption"></param>
        /// <param name="ret"></param>
        private void FillFiles(string path, SearchOption searchOption, List<FileInfoBase> ret) 
        {
            GetBucketRequest request = new GetBucketRequest(_bucketName);
            if (!string.IsNullOrWhiteSpace(path) && path != "/")
            {
                request.SetPrefix(path);
            }
            request.SetDelimiter("/");
            GetBucketResult result = _cloud.GetBucket(request);
            ListBucket info = result.listBucket;
            List<ListBucket.Contents> objects = info.contentsList;

            foreach (ListBucket.Contents item in objects)
            {
                string fileName = item.key.Trim('\r', ' ', '\n');
                if (fileName.EndsWith('/'))//文件夹
                {
                    continue;
                }
                NetStorageFileInfo file = ToFileBaseInfo(item, fileName);
                ret.Add(file);
            }

            if(searchOption!= SearchOption.AllDirectories) 
            {
                return;
            }

            List<ListBucket.CommonPrefixes> subDirs = info.commonPrefixesList;
            foreach (ListBucket.CommonPrefixes item in subDirs)
            {
                FillFiles(item.prefix, searchOption, ret);
            }

        }

        /// <summary>
        /// 转换成文件信息对象
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private NetStorageFileInfo ToFileBaseInfo(ListBucket.Contents result, string relativePath)
        {
            DateTime dtCreate = DefaultDate;

            DateTime dt = DateTime.MinValue;

            if (!DateTime.TryParse(result.lastModified, out dtCreate))
            {
                dtCreate = DefaultDate;
            }

            DateTime dtUpdate = DefaultDate;
            if (!DateTime.TryParse(result.lastModified, out dtUpdate))
            {
                dtCreate = DefaultDate;
            }
            string hash = "";
            string filePath = null;
            if (!string.IsNullOrWhiteSpace(_internetUrl))
            {
                filePath = FileInfoBase.CombineUriToString(_internetUrl , relativePath);
            }
            string accessUrl = null;

            if (!string.IsNullOrWhiteSpace(_lanUrl))
            {
                accessUrl = FileInfoBase.CombineUriToString(_lanUrl , relativePath);
            }
            else 
            {
                accessUrl = result.key;
            }


            long len = result.size;
            NetStorageFileInfo file = new NetStorageFileInfo(dtCreate, dtUpdate, relativePath, filePath, accessUrl, hash, len);
            return file;
        }

        /// <summary>
        /// 转换成文件信息对象
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private NetStorageFileInfo ToFileBaseInfo(HeadObjectResult result,string relativePath)
        {
            DateTime dtCreate = DefaultDate;

            DateTime dt = DateTime.MinValue;

            if(!DateTime.TryParse(result.lastModified,out dtCreate)) 
            {
                dtCreate = DefaultDate;
            }

            DateTime dtUpdate = DefaultDate;
            if (!DateTime.TryParse(result.lastModified, out dtUpdate))
            {
                dtCreate = DefaultDate;
            }
            string hash = result.crc64ecma;
           
            //string url = data.GetMapValue<string>("access_url");
            //string surl = data.GetMapValue<string>("source_url");

            string filePath = null;
            if (!string.IsNullOrWhiteSpace(_internetUrl))
            {
                filePath = _internetUrl  + relativePath;
            }
            string accessUrl = null;

            if (!string.IsNullOrWhiteSpace(_lanUrl))
            {
                accessUrl = _lanUrl + relativePath;
            }


            long len = result.size;
            NetStorageFileInfo file = new NetStorageFileInfo(dtCreate, dtUpdate, relativePath, filePath, accessUrl, hash, len);
            return file;
        }

        private Dictionary<string, object> ToDictionary(string result) 
        {
            JObject json = (JObject)JsonConvert.DeserializeObject(result);
            Dictionary<string, object> dicResault = new Dictionary<string, object>(json.ToObject<IDictionary<string, object>>(), StringComparer.CurrentCultureIgnoreCase);
            return dicResault;
        }
        public override APIResault Open()
        {
            _cloud= new CosXmlServer(_cosConfig, _cosCredentialProvider);
            return ApiCommon.GetSuccess();
        }

        private string GetRegion(string server) 
        {
            int start= server.IndexOf("://");
            if (start < 0) 
            {
                return server;
            }
            int end = server.IndexOf(':', start + 3);
            if (end < 0)
            {
                return server;
            }
            return server.Substring(start, end - start);
        }

        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public override APIResault RemoveFile(string path)
        {
            DeleteObjectRequest request = new DeleteObjectRequest(_bucketName, path);
            //执行请求
            DeleteObjectResult result = _cloud.DeleteObject(request);

            return GetCommResault(result);
        }

        /// <summary>
        /// 获取通用操作结果
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        private APIResault GetCommResault(CosResult result)
        {
            
            if (!result.IsSuccessful())
            {
                return ApiCommon.GetFault(result.httpMessage, result);
            }
            return ApiCommon.GetSuccess();
        }

        public override APIResault RenameFile(string source, string target)
        {
           
            throw new System.NotSupportedException("腾讯对象存储不支持重命名文件");
        }

        /// <summary>
        /// 保存文件
        /// </summary>
        /// <param name="sourcePath">原路径</param>
        /// <param name="targetPath">目标路径</param>
        /// <returns></returns>
        public override APIResault SaveFile(string sourcePath, string targetPath)
        {
            PutObjectRequest request = new PutObjectRequest(_bucketName, targetPath, sourcePath);
            //设置进度回调
            request.SetCosProgressCallback(delegate (long completed, long total)
            {
                //Console.WriteLine(String.Format("progress = {0:##.##}%", completed * 100.0 / total));
            });
            //执行请求
            PutObjectResult result = _cloud.PutObject(request);

            if (!result.IsSuccessful())
            {
                return ApiCommon.GetFault(result.httpMessage);
            }
            NetStorageFileInfo fileInfo = fileInfo = ToFileInfo(result, targetPath);
            return ApiCommon.GetSuccess(null, fileInfo);
        }
        /// <summary>
        /// 哈希类型
        /// </summary>
        public override string HashName
        {
            get
            {
                return "crc64ecma";
            }
        }
        /// <summary>
        /// 获取哈希类
        /// </summary>
        public override HashAlgorithm GetHash()
        {
            System.Security.Cryptography.SHA1 sha1Hash = SHA1CryptoServiceProvider.Create();
            return sha1Hash;
        }

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="path"></param>
        /// <param name="stream"></param>
        /// <returns></returns>
        public override APIResault SaveFile(string path, Stream stream,long contentLength)
        {
            PutObjectRequest request = new PutObjectRequest(_bucketName, path, stream);
            //设置进度回调
            request.SetCosProgressCallback(delegate (long completed, long total)
            {
                //Console.WriteLine(String.Format("progress = {0:##.##}%", completed * 100.0 / total));
            });
            //执行请求
            PutObjectResult result = _cloud.PutObject(request);

            if (!result.IsSuccessful())
            {
                return ApiCommon.GetFault(result.httpMessage);
            }
            NetStorageFileInfo fileInfo = fileInfo = ToFileInfo(result, path);
            return ApiCommon.GetSuccess(null, fileInfo);
        }
        /// <summary>
        /// Json转成文件信息
        /// </summary>
        /// <param name="dicData"></param>
        /// <returns></returns>
        private NetStorageFileInfo ToFileInfo(PutObjectResult result, string relativePath)
        {
            DateTime createTime = DateTime.Now;
            DateTime updateTime = DateTime.Now;
            string filePath = _internetUrl+ relativePath;
            
            string accessUrl = _lanUrl + relativePath;
            

            string hash = result.crc64ecma;
            long length = 0;



            return new NetStorageFileInfo(createTime, updateTime, relativePath, filePath, accessUrl, hash, length);
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
            DoesObjectExistRequest request = new DoesObjectExistRequest(_bucketName, folder);
            return _cloud.DoesObjectExist(request);
        }
        /// <summary>
        /// 创建文件夹
        /// </summary>
        /// <param name="folder"></param>
        /// <returns></returns>
        public override APIResault CreateDirectory(string folder)
        {
            PutObjectRequest request = new PutObjectRequest(_bucketName, folder, new byte[]{ });
            
            //执行请求
            PutObjectResult result = _cloud.PutObject(request);

            if (!result.IsSuccessful())
            {
                return ApiCommon.GetFault(result.httpMessage);
            }
           
            return ApiCommon.GetSuccess();
        }
        /// <summary>
        /// 文件是否存在
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public override bool ExistsFile(string path)
        {
            DoesObjectExistRequest request = new DoesObjectExistRequest(_bucketName, path);
            //执行请求
            return _cloud.DoesObjectExist(request);
        }
        /// <summary>
        /// 创建文件夹
        /// </summary>
        /// <param name="folder"></param>
        /// <returns></returns>
        private void CheckDirectory(string folder)
        {
            CreateDirectory(folder);
        }

        public override void ReadFileToStream(string path, Stream stm, long postion, long length)
        {
            NetStorageFileInfo info = GetFileInfo(path) as NetStorageFileInfo;
            if (info == null)
            {
                throw new FileNotFoundException("找不到文件:"+path);
            }
           
            HttpWebRequest myWebRequest = (HttpWebRequest)WebRequest.Create(info.AccessUrl);
            using (HttpWebResponse myWebResponse = (HttpWebResponse)myWebRequest.GetResponse())
            {
                using (Stream readStream = myWebResponse.GetResponseStream())
                {
                    SkipToPostion(readStream, postion);


                    CommonMethods.CopyStreamData(readStream, stm, length);

                }
            }
            
        }
        /// <summary>
        /// 跳到指定位置
        /// </summary>
        /// <param name="readStream"></param>
        /// <param name="postion"></param>
        private void SkipToPostion(Stream readStream,long postion)
        {
            if (postion <= 0)
            {
                return;
            }
            long left = postion;
            byte[] buffer = new byte[1024];
            int reader = 0;
            int len = buffer.Length;
            while (left > 0)
            {
                if (left < len)
                {
                    len = (int)left;
                }
                reader = readStream.Read(buffer, 0, len);
                left -= reader;
            }
        }
    }
}
