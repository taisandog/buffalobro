using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QCloud.CosApi.Common;
using QCloud.CosApi.Util;
using System.Web;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Buffalo.Kernel;
using Buffalo.Storage;

namespace QCloud.CosApi.Api
{
    public partial class CosCloud
    {
        /// <summary>
        /// 分片上传 
        /// </summary>
        /// <param name="bucketName">bucket名称</param>
        /// <param name="remotePath">远程文件路径</param>
        /// <param name="localPath">本地文件路径</param>
        /// <param name="fileSha">文件的sha1</param>
        /// <param name="session">init请求返回的session</param>
        /// <param name="offset">分片的偏移量</param>
        /// <param name="sliceSize">切片大小（字节）,默认为1M</param>
        /// <param name="sign">签名</param>
        /// <returns></returns>
        public string SliceUploadDataStream(string bucketName, string remotePath, Stream stm, string fileSha, string session, long offset, int sliceSise, string sign)
        {
            var url = generateURL(bucketName, remotePath);
            var data = new Dictionary<string, object>();

            data.Add("op", "upload_slice_data");
            data.Add("session", session);
            data.Add("offset", offset);
            if (!string.IsNullOrWhiteSpace(fileSha))
            {
                data.Add("sha", fileSha);
            }

            var header = new Dictionary<string, string>();
            header.Add("Authorization", sign);
            return _httpRequest.SendStreamRequest(url, ref data, HttpMethod.Post, ref header, _timeOut, stm, offset, sliceSise);
        }
        /// <summary>
        /// 分片上传 finish
        /// </summary>
        /// <param name="bucketName">bucket名</param>
        /// <param name="remotePath">目标路径</param>
        /// <param name="localPath">本地路径</param>
        /// <param name="fileSha">文件的sha1</param>
        /// <param name="session">init请求返回的session</param>
        /// <returns></returns>
        public string SliceUploadStreamFinish(string bucketName, string remotePath, long fileSize, string fileSha, string session)
        {
            string url = generateURL(bucketName, remotePath);

            Dictionary<string, object> data = new Dictionary<string, object>();

            data.Add("op", "upload_slice_finish");
            data.Add("session", session);
            data.Add("fileSize", fileSize);
            data.Add("sha", fileSha);

            var header = new Dictionary<string, string>();
            var sign = Sign.Signature(_appId, _secretId, _secretKey, getExpiredTime(), bucketName);
            header.Add("Authorization", sign);
            return _httpRequest.SendRequest(url, ref data, HttpMethod.Post, ref header, _timeOut);
        }

        /// <summary>
        /// 分片上传
        /// </summary>
        /// <param name="bucketName">bucket名称</param>
        /// <param name="remotePath">远程文件路径</param>
        /// <param name="localPath">本地文件路径</param>
        /// <param name="bizAttribute">biz属性</param>
        /// <param name="sliceSize">切片大小（字节）,默认为1M,目前只支持1M</param>
        /// <param name="insertOnly">是否覆盖同名文件</param>
        /// <param name="paras"></param> 
        /// <returns></returns>
        public string SliceUploadFileStream(string bucketName, string remotePath, Stream stm,
                                       string bizAttribute = "", int sliceSize = SLICE_SIZE.SLIZE_SIZE_1M, int insertOnly = 1)
        {
            string fileSha = null;
            
                fileSha = GetSHA(stm);
            
            
            long fileSize = stm.Length;
            var result = SliceUploadInitStream(bucketName, remotePath, stm, fileSha, bizAttribute, sliceSize, insertOnly);
            var obj = (JObject)JsonConvert.DeserializeObject(result);
            if ((int)obj["code"] != 0)
            {
                return result;
            }

            var data = obj["data"];
            if (data["access_url"] != null)
            {
                var accessUrl = data["access_url"];
                //Console.WriteLine("命中秒传" + accessUrl);
                return result;
            }

            int retryCount = 0;
            var session = data["session"].ToString();
            sliceSize = (int)data["slice_size"];

            var sign = Sign.Signature(_appId, _secretId, _secretKey, getExpiredTime(), bucketName);

            //总共重试三次
            for (long offset = 0; offset < fileSize; offset += sliceSize)
            {
                result = SliceUploadDataStream(bucketName, remotePath, stm, fileSha, session, offset, sliceSize, sign);
                obj = (JObject)JsonConvert.DeserializeObject(result);
                if ((int)obj["code"] != 0)
                {
                    if (retryCount < 10)
                    {
                        ++retryCount;
                        offset -= sliceSize;
                        //Console.WriteLine("重试...");
                        continue;
                    }
                    else
                    {
                        //Console.WriteLine("upload fail");
                        return result;
                    }
                }
            }

            return SliceUploadFinishStream(bucketName, remotePath, stm.Length, fileSha, session);
        }



        /// <summary>
        /// 分片上传 init,如果上一次分片上传未完成，会返回{"code":-4019,"message":"_ERROR_FILE_NOT_FINISH_UPLOAD"}
        /// </summary>
        /// <param name="bucketName">bucket名称</param>
        /// <param name="remotePath">远程文件路径</param>
        /// <param name="localPath">本地文件路径</param>
        /// <param name="bizAttribute">biz属性</param>
        /// <param name="sliceSize">切片大小（字节）,默认为1M,目前只支持1M</param>
        /// <param name="insertOnly">是否覆盖同名文件</param>
        /// <returns></returns>
        public string SliceUploadInitStream(string bucketName, string remotePath, Stream stm, string fileSha,
                                       string bizAttribute = "", int sliceSize = SLICE_SIZE.SLIZE_SIZE_1M, int insertOnly = 1)
        {
            sliceSize = SLICE_SIZE.SLIZE_SIZE_1M;
            var url = generateURL(bucketName, remotePath);

            

            var data = new Dictionary<string, object>();

            data.Add("op", "upload_slice_init");
            data.Add("filesize", stm.Length);
            data.Add("sha", fileSha);
            data.Add("biz_attr", bizAttribute);
            data.Add("slice_size", sliceSize);
            data.Add("insertOnly", insertOnly);
            string uploadParts = computeUploadPartsStream(stm, sliceSize);
            data.Add("uploadparts", uploadParts);


            var expired = getExpiredTime();
            var header = new Dictionary<string, string>();
            var sign = Sign.Signature(_appId, _secretId, _secretKey, expired, bucketName);
            header.Add("Authorization", sign);

            //Console.WriteLine(sign);
            //Console.WriteLine(url + _appId + " " + _secretId + _secretId + expired + bucketName);

            return _httpRequest.SendRequest(url, ref data, HttpMethod.Post, ref header, _timeOut);
        }
        /// <summary>
        /// 单个文件上传
        /// </summary>
        /// <param name="bucketName">bucket名称</param>
        /// <param name="remotePath">远程文件路径</param>
        /// <param name="localPath">本地文件路径</param>
        /// <param name="biz_attr">biz_attr属性</param>
        /// <param name="insertOnly">同名文件是否覆盖</param>
        /// <returns></returns>
        public string UploadStream(string bucketName, string remotePath, Stream fileStream,
                                  string bizAttribute = "", int insertOnly = 1)
        {
            var url = generateURL(bucketName, remotePath);
            //var sha1 = SHA1.GetFileSHA1(localPath);
            Dictionary<string, object> data = new Dictionary<string, object>();
            
                fileStream.Seek(0, SeekOrigin.Begin);
                var sha1 = GetSHA(fileStream);
                fileStream.Seek(0, SeekOrigin.Begin);
                data.Add("sha", sha1);
            
            data.Add("op", "upload");
            
            data.Add("biz_attr", bizAttribute);
            data.Add("insertOnly", insertOnly);

            var expired = getExpiredTime();
            var sign = Sign.Signature(_appId, _secretId, _secretKey, expired, bucketName);
            var header = new Dictionary<string, string>();
            header.Add("Authorization", sign);
            return _httpRequest.SendStreamRequest(url, ref data, HttpMethod.Post, ref header, _timeOut, fileStream);
        }
        /// <summary>
        /// 获取SHA
        /// </summary>
        /// <param name="stm"></param>
        /// <returns></returns>
        private string GetSHA(Stream stm) 
        {
            stm.Seek(0, SeekOrigin.Begin);
            string fileSha = PasswordHash.ToSHA1String(stm,false);
            stm.Seek(0, SeekOrigin.Begin);
            return fileSha;
        }

        /// <summary>
        /// 文件上传
        /// 说明: 根据文件大小判断使用单文件上传还是分片上传,当文件大于8M时,内部会进行分片上传,可以携带分片大小sliceSize
        /// 其中分片上传使用SliceUploadInit SliceUploadData SliceUploadFinihs
        /// </summary>
        /// <param name="bucketName">bucket名称</param>
        /// <param name="remotePath">远程文件路径</param>
        /// <param name="localPath">本地文件路径</param>
        /// <param name="parameterDic">参数Dictionary</param>
        /// 包含如下可选参数
        /// bizAttribute：文件属性
        /// insertOnly： 0:同名文件覆盖, 1:同名文件不覆盖,默认1
        /// sliceSize: 分片大小，可选取值为:64*1024 512*1024，1*1024*1024，2*1024*1024，3*1024*1024
        /// <returns></returns>
        public string UploadFileStream(string bucketName, string remotePath, Stream file,long contentLength, Dictionary<string, string> parameterDic = null)
        {
           

            if (remotePath.EndsWith("/"))
            {
                return constructResult(ERRORCode.ERROR_CODE_PARAMETER_ERROE, "file path can not end with '/'");
            }

            string bizAttribute = "";
            if (parameterDic != null && parameterDic.ContainsKey(CosParameters.PARA_BIZ_ATTR))
                bizAttribute = parameterDic[CosParameters.PARA_BIZ_ATTR];

            int insertOnly = 1;
            if (parameterDic != null && parameterDic.ContainsKey(CosParameters.PARA_INSERT_ONLY))
            {
                try
                {
                    insertOnly = Int32.Parse(parameterDic[CosParameters.PARA_INSERT_ONLY]);
                }
                catch (Exception e)
                {
                    //Console.WriteLine(e.Message);
                    return constructResult(ERRORCode.ERROR_CODE_PARAMETER_ERROE, "parameter insertOnly value invalidate");
                }
            }

            long fileSize = contentLength;
            

            
            if (fileSize <= FileInfoBase.SLICE_UPLOAD_FILE_SIZE)
            {
                return UploadStream(bucketName, remotePath, file, bizAttribute, insertOnly);
            }
            else
            {
                //分片上传
                int sliceSize = SLICE_SIZE.SLIZE_SIZE_1M;

                if (parameterDic != null && parameterDic.ContainsKey(CosParameters.PARA_SLICE_SIZE))
                {
                    sliceSize = Int32.Parse(parameterDic[CosParameters.PARA_SLICE_SIZE]);
                    //Console.WriteLine("slice size:" + sliceSize);
                }
                int slice_size = getSliceSize(sliceSize);
                return SliceUploadFileStream(bucketName, remotePath, file, bizAttribute, slice_size, insertOnly);
            }
        }

        /// <summary>
        /// 计算块
        /// </summary>
        /// <param name="fileStream"></param>
        /// <param name="sliceSize"></param>
        /// <returns></returns>
        private string computeUploadPartsStream(Stream fileStream, int sliceSize)
        {
            try
            {
                byte[] buffer = new byte[sliceSize + 1];
                long offset = 0;    //文件的偏移
                long totalLen = 0;  //总共读取的字节数
                int readLen = 0;
                
                CosSha1Pure sha = new CosSha1Pure();
                StringBuilder jsonStr = new StringBuilder();

                jsonStr.Append("[");

                for (int i = 0; offset < fileStream.Length; ++i, offset += readLen)
                {
                    fileStream.Seek(offset, SeekOrigin.Begin);
                    readLen = fileStream.Read(buffer, 0, sliceSize);
                    totalLen += readLen;
                    string dataSha;

                    sha.HashCore(buffer, 0, readLen);
                    if ((readLen < sliceSize) || (readLen == sliceSize && totalLen == fileStream.Length))
                    {
                        //最后一片
                        dataSha = sha.FinalHex();
                    }
                    else
                    {
                        //中间的分片
                        dataSha = sha.GetDigest();
                    }

                    if (i != 0)
                    {
                        jsonStr.Append(",{\"offset\":" + offset + "," +
                                       "\"datalen\":" + readLen + "," +
                                       "\"datasha\":\"" + dataSha + "\"}");
                    }
                    else
                    {
                        jsonStr.Append("{\"offset\":" + offset + "," +
                                       "\"datalen\":" + readLen + "," +
                                       "\"datasha\":\"" + dataSha + "\"}");
                    }
                }


                jsonStr.Append("]");
                return jsonStr.ToString();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 分片上传 finish
        /// </summary>
        /// <param name="bucketName">bucket名</param>
        /// <param name="remotePath">目标路径</param>
        /// <param name="localPath">本地路径</param>
        /// <param name="fileSha">文件的sha1</param>
        /// <param name="session">init请求返回的session</param>
        /// <returns></returns>
        public string SliceUploadFinishStream(string bucketName, string remotePath, long fileSize, string fileSha, string session)
        {
            var url = generateURL(bucketName, remotePath);
            var data = new Dictionary<string, object>();

            data.Add("op", "upload_slice_finish");
            data.Add("session", session);
            data.Add("fileSize", fileSize);
            data.Add("sha", fileSha);

            var header = new Dictionary<string, string>();
            var sign = Sign.Signature(_appId, _secretId, _secretKey, getExpiredTime(), bucketName);
            header.Add("Authorization", sign);
            return _httpRequest.SendRequest(url, ref data, HttpMethod.Post, ref header, _timeOut);
        }
    }
}
