using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Buffalo.Kernel;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using QCloud.CosApi.Api;
using QCloud.CosApi.Common;
using Buffalo.ArgCommon;
using System.IO;
using System.Net;
using System.Security.Cryptography;

namespace Buffalo.Storage.QCloud.CosApi
{
    //APPID:1251039330
//SecretId: AKID16AFMW1nu1IatBHnxbBev691prwr7PGz
//SecretKey: 01rdj6KMCuTt9GYnqmiV8ZxGN1Yu46fA
    //"http://" + local + ".file.myqcloud.com/files/v2/";

    /// <summary>
    /// 腾讯云适配器
    /// </summary>
    public class QCloudAdapter : IFileStorage
    {
        private CosCloud _cloud;
        /// <summary>
        /// APP ID
        /// </summary>
        private int _APPID;
        /// <summary>
        /// 安全ID
        /// </summary>
        private string _SECRETID ;
        /// <summary>
        /// 安全Key
        /// </summary>
        private string _SECRETKEY ;
        /// <summary>
        /// BucketName
        /// </summary>
        private string _bucketName;

        /// <summary>
        /// 超时(毫秒)
        /// </summary>
        private int _timeout = 0;

        /// <summary>
        /// 外网访问地址
        /// </summary>
        private string _internetUrl;
        /// <summary>
        /// 内网访问地址
        /// </summary>
        private string _lanUrl;
        /// <summary>
        /// 服务器地址
        /// </summary>
        private string _server;
        /// <summary> 
        /// 腾讯云适配器
        /// </summary>
        /// <param name="connString">连接字符串</param>
        public QCloudAdapter(string connString) 
        {
            Dictionary<string, string> hs = ConnStringFilter.GetConnectInfo(connString);
            _APPID = hs.GetMapValue<int>("AppId");
            _SECRETID = hs.GetMapValue<string>("SecretId");
            _SECRETKEY = hs.GetMapValue<string>("SecretKey");
            _timeout = hs.GetMapValue<int>("timeout",1000);
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
            _server = hs.GetMapValue<string>("Server");
        }
        /// <summary>
        /// 删除文件夹
        /// </summary>
        /// <param name="path">路径</param>
        public override APIResault RemoveDirectory(string path)
        {
            string result = _cloud.DeleteFolder(_bucketName, path);
            return GetCommResault(result);
        }


        public override APIResault AppendFile(string path, Stream content, long postion)
        {
            throw new System.NotSupportedException("腾讯对象存储不支持追加文件");
        }

        public override APIResault AppendFile(string path, Stream content)
        {
            throw new System.NotSupportedException("腾讯对象存储不支持追加文件");
        }
        /// <summary>
        /// 获取文件信息
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public override FileInfoBase GetFileInfo(string path)
        {
            string result = _cloud.GetFileStat(_bucketName, path);
            Dictionary<string, object> dicResault = ToDictionary(result);
            if (ValueConvertExtend.GetMapDataValue<int>(dicResault,"code") != 0)
            {
                return null;
            }
            object odata = ValueConvertExtend.GetMapDataValue<object>(dicResault, "data");
            Dictionary<string, object> data =JsonValueConvertExtend.ConvertJsonValue<Dictionary<string, object>>(odata);
            return ToFileBaseInfo(data,path);
        }
        public override APIResault Close()
        {
            _cloud = null;
            return ApiCommon.GetSuccess();
        }

        public override List<string> GetDirectories(string path, System.IO.SearchOption searchOption)
        {
           

            List<string> ret = new List<string>();
            Dictionary<string, string> folderlistParasDic = new Dictionary<string, string>();
            folderlistParasDic[CosParameters.PARA_NUM] = "1000";
            folderlistParasDic[CosParameters.PARA_ORDER] = "0";
            folderlistParasDic[CosParameters.PARA_LIST_FLAG] = "1";
            folderlistParasDic[CosParameters.PARA_PATTERN] = FolderPattern.PATTERN_DIR;

            string curPath = path;
            if (string.IsNullOrWhiteSpace(curPath) || curPath == "/")
            {
                curPath = "/";
            }

            string result = _cloud.GetFolderList(_bucketName, curPath, folderlistParasDic);

            
            Dictionary<string, object> dicResault = ToDictionary(result);
            JObject json = dicResault.GetMapValue<JObject>("data");
            if (json == null) 
            {
                return ret;
            }
            JToken jt = json.GetValue("infos");
            List<Dictionary<string, object>> lstResault = jt.ToObject<List<Dictionary<string, object>>>();

            foreach (Dictionary<string, object> item in lstResault)
            {
                if (item.ContainsKey("sha")) //如果是文件就跳过
                {
                    continue;
                }
                string cur = item.GetMapValue<string>("name");
                ret.Add(cur);
            }

            return ret;
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
                    CommonMethods.CopyStreamData(readStream, stm, length, null);
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
            Dictionary<string, string> folderlistParasDic = new Dictionary<string, string>();
            folderlistParasDic[CosParameters.PARA_NUM] = "1000";
            folderlistParasDic[CosParameters.PARA_PATTERN] = FolderPattern.PATTERN_FILE;
            string curPath = path;
            if (string.IsNullOrWhiteSpace(curPath) || curPath == "/")
            {
                curPath = "/";
            }
            string result = _cloud.GetFolderList(_bucketName, curPath, folderlistParasDic);

            
            Dictionary<string, object> dicResault = ToDictionary(result);
            if (dicResault.GetMapValue<int>("code") < 0)
            {
                
                throw new Exception("code:"+dicResault.GetMapValue<int>("code")+" message" +dicResault.GetMapValue<string>("message"));
            }
            JObject json = dicResault.GetMapValue<JObject>("data");
            if (json == null)
            {
                return ret;
            }
            JToken jt = json.GetValue("infos");
            List<Dictionary<string, object>> lstResault = jt.ToObject<List<Dictionary<string, object>>>();

            foreach (Dictionary<string, object> item in lstResault)
            {
                if (!item.ContainsKey("sha"))
                {
                    continue;
                }
                string surl = item.GetMapValue<string>("source_url");
                string fileName = NetStorageFileInfo.GetFileName(surl);
                NetStorageFileInfo file = ToFileBaseInfo(item, curPath + fileName);
                ret.Add(file);
            }

            return ret;
        }

        /// <summary>
        /// 转换成文件信息对象
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private NetStorageFileInfo ToFileBaseInfo(Dictionary<string, object> data, string relativePath)
        {
            DateTime dtCreate = DefaultDate;
            double timespame = data.GetMapValue<double>("ctime");
            if (timespame > 0)
            {
                dtCreate = CommonMethods.ConvertIntDateTime(timespame);
            }

            DateTime dtUpdate = DefaultDate;
            timespame = data.GetMapValue<double>("mtime");
            if (timespame > 0)
            {
                dtUpdate = CommonMethods.ConvertIntDateTime(timespame);
            }
            string hash = data.GetMapValue<string>("sha", "");
           
            //string url = data.GetMapValue<string>("access_url");
            //string surl = data.GetMapValue<string>("source_url");

            string filePath = null;
            if (string.IsNullOrWhiteSpace(_internetUrl))
            {
                filePath = data.GetMapValue<string>("source_url", null);
            }
            else
            {
                filePath = _internetUrl  + relativePath;
            }
            string accessUrl = null;

            if (string.IsNullOrWhiteSpace(_lanUrl))
            {
                accessUrl = data.GetMapValue<string>("access_url", null);
            }
            else
            {
                accessUrl = _lanUrl + relativePath;
            }


            long len = data.GetMapValue<long>("filelen");
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
            if (_timeout <= 0)
            {
                _cloud = new CosCloud(_APPID, _SECRETID, _SECRETKEY, _server, CosCloud.Default_HTTP_TIMEOUT_TIME);
            }
            else 
            {
                _cloud = new CosCloud(_APPID, _SECRETID, _SECRETKEY, _server, _timeout);
            }
            return ApiCommon.GetSuccess();
        }
        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public override APIResault RemoveFile(string path)
        {
            string result = _cloud.DeleteFile(_bucketName, path);
            return GetCommResault(result);
        }

        /// <summary>
        /// 获取通用操作结果
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        private APIResault GetCommResault(string result)
        {
            Dictionary<string, object> dicResault = ToDictionary(result);
            int code = dicResault.GetMapValue<int>("code", -1000);
            if (code != 0)
            {
                return ApiCommon.GetFault(dicResault.GetMapValue<string>("message"), dicResault);
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
            Dictionary<string, string> uploadParasDic = new Dictionary<string, string>();
            uploadParasDic[CosParameters.PARA_BIZ_ATTR] = "";
            uploadParasDic[CosParameters.PARA_INSERT_ONLY] = "0";
            
            string result = _cloud.UploadFile(_bucketName, targetPath, sourcePath, uploadParasDic);
            Dictionary<string, object> dicResault = ToDictionary(result);
            int code = dicResault.GetMapValue<int>("code", -1000);
            if (code != 0)
            {
                return ApiCommon.GetFault(dicResault.GetMapValue<string>("message"));
            }
            NetStorageFileInfo fileInfo = null;
            object odata = dicResault.GetMapValue<object>("data");
            Dictionary<string, object> data = JsonValueConvertExtend.ConvertJsonValue<Dictionary<string, object>>(odata);
            if (data != null)
            {
                fileInfo = ToFileInfo(data, targetPath);
            }
            return ApiCommon.GetSuccess(null, fileInfo);
        }
        /// <summary>
        /// 哈希类型
        /// </summary>
        public override string HashName
        {
            get
            {
                return "SHA1";
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
            Dictionary<string, string> uploadParasDic = new Dictionary<string, string>();
            uploadParasDic[CosParameters.PARA_BIZ_ATTR]= "";
            uploadParasDic[CosParameters.PARA_INSERT_ONLY]= "0";
            //stream.Position = 0;
            long len = contentLength;
            if (len <= 0)
            {
                len = stream.Length;
            }
            string result=_cloud.UploadFileStream(_bucketName, path, stream,len, uploadParasDic);
            Dictionary<string, object> dicResault = ToDictionary(result);
            int code = dicResault.GetMapValue<int>("code", -1000);
            if (code != 0)
            {
                return ApiCommon.GetFault(dicResault.GetMapValue<string>("message"));
            }
            NetStorageFileInfo fileInfo = null;
            object odata = dicResault.GetMapValue<object>("data");
            Dictionary<string, object> data = JsonValueConvertExtend.ConvertJsonValue<Dictionary<string, object>>(odata);
            if (data != null)
            {
                fileInfo = ToFileInfo(data,path);
            }
            return ApiCommon.GetSuccess(null, fileInfo);
        }
        /// <summary>
        /// Json转成文件信息
        /// </summary>
        /// <param name="dicData"></param>
        /// <returns></returns>
        private NetStorageFileInfo ToFileInfo(Dictionary<string, object> dicData, string relativePath)
        {
            DateTime createTime = dicData.GetMapValue<DateTime>("createTime", DefaultDate);
            DateTime updateTime = dicData.GetMapValue<DateTime>("updateTime", DefaultDate);
            string filePath = null;
            if (string.IsNullOrWhiteSpace(_internetUrl))
            {
                filePath = dicData.GetMapValue<string>("source_url", null);
            }
            else 
            {
                filePath = _internetUrl+ relativePath;
            }
            string accessUrl =null;

            if (string.IsNullOrWhiteSpace(_lanUrl))
            {
                accessUrl = dicData.GetMapValue<string>("access_url", null);
            }
            else
            {
                accessUrl = _lanUrl + relativePath;
            }

            string hash = dicData.GetMapValue<string>("vid", null);
            long length = dicData.GetMapValue<long>("length", 0);



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
            string result = _cloud.GetFolderStat(_bucketName, folder);
            Dictionary<string, object> dicResault = ToDictionary(result);
            int code= dicResault.GetMapValue<int>("code",-1000);
            return code == 0;
        }
        /// <summary>
        /// 创建文件夹
        /// </summary>
        /// <param name="folder"></param>
        /// <returns></returns>
        public override APIResault CreateDirectory(string folder)
        {
            string result = _cloud.CreateFolder(_bucketName, folder);
            Dictionary<string, object> dicResault = ToDictionary(result);
            int code = dicResault.GetMapValue<int>("code", -1000);
            
            return ApiCommon.GetSuccess();
        }
        /// <summary>
        /// 文件是否存在
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public override bool ExistsFile(string path)
        {
            string result = _cloud.GetFileStat(_bucketName, path);
            Dictionary<string, object> dicResault = ToDictionary(result);
            if (ValueConvertExtend.GetMapDataValue<int>(dicResault, "code") == 0)
            {
                return true;
            }
            return false;
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


                    CommonMethods.CopyStreamData(readStream, stm, length, null);

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
