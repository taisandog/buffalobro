using Aliyun.OSS.Util;
using Buffalo.ArgCommon;
using Buffalo.Kernel;
using OBS;
using OBS.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buffalo.Storage.HW.OBS
{
    public partial class HWOBSAdapter
    {
        private const long PartSize = 8388608; // 每个段大小8 MB
        /// <summary>
        /// 分段上传
        /// </summary>
        /// <param name="path"></param>
        /// <param name="stm"></param>
        private APIResault FilePartUpload(string path, string filePath)
        {
            
            try
            {
                // 1. 初始化分段上传任务
                InitiateMultipartUploadRequest initiateRequest = new InitiateMultipartUploadRequest
                {
                    BucketName = _bucketName,
                    ObjectKey = path
                };
                
                InitiateMultipartUploadResponse initResponse = _client.InitiateMultipartUpload(initiateRequest);
                if (string.IsNullOrWhiteSpace(initResponse.UploadId))
                {
                    return ApiCommon.GetFault("初始化上传失败");
                }

                FileInfo finfo = new FileInfo(filePath);
                long contentLength = finfo.Length;

                List<UploadPartResponse> uploadResponses = new List<UploadPartResponse>();
                long filePosition = 0;
                for (int i = 1; filePosition < contentLength; i++)
                {
                    UploadPartRequest uploadRequest = new UploadPartRequest
                    {
                        BucketName = _bucketName,
                        ObjectKey = path,
                        UploadId = initResponse.UploadId,
                        PartNumber = i,
                        PartSize = PartSize,
                        Offset = filePosition,
                        FilePath = filePath,
                        
                    };
                    uploadResponses.Add(_client.UploadPart(uploadRequest));

                    filePosition += PartSize;
                }

                // 3.合并段

                CompleteMultipartUploadRequest completeRequest = new CompleteMultipartUploadRequest()
                {
                    BucketName = _bucketName,
                    ObjectKey = path,
                    UploadId = initResponse.UploadId,
                };
                completeRequest.AddPartETags(uploadResponses);
                CompleteMultipartUploadResponse completeUploadResponse = _client.CompleteMultipartUpload(completeRequest);
            }
            catch (ObsException ex)
            {
                return ApiCommon.GetException(ex);
            }
            return ApiCommon.GetSuccess();
        }
        /// <summary>
        /// 分段上传
        /// </summary>
        /// <param name="path"></param>
        /// <param name="stm"></param>
        private APIResault StreamPartUpload(string path, Stream fileStm, long contentLength)
        {

            try
            {
                // 1. 初始化分段上传任务
                InitiateMultipartUploadRequest initiateRequest = new InitiateMultipartUploadRequest
                {
                    BucketName = _bucketName,
                    ObjectKey = path
                };

                InitiateMultipartUploadResponse initResponse = _client.InitiateMultipartUpload(initiateRequest);
                if (string.IsNullOrWhiteSpace(initResponse.UploadId))
                {
                    return ApiCommon.GetFault("初始化上传失败");
                }

                List<UploadPartResponse> uploadResponses = new List<UploadPartResponse>();
                long filePosition = 0;
                for (int i = 1; filePosition < contentLength; i++)
                {
                    UploadPartRequest uploadRequest = new UploadPartRequest
                    {
                        BucketName = _bucketName,
                        ObjectKey = path,
                        UploadId = initResponse.UploadId,
                        PartNumber = i,
                        PartSize = PartSize,
                        Offset = filePosition,
                        InputStream = fileStm,
                        
                        //InputStreamLength = contentLength
                    };
                    uploadResponses.Add(_client.UploadPart(uploadRequest));

                    filePosition += PartSize;
                }

                // 3.合并段

                CompleteMultipartUploadRequest completeRequest = new CompleteMultipartUploadRequest()
                {
                    BucketName = _bucketName,
                    ObjectKey = path,
                    UploadId = initResponse.UploadId,
                };
                completeRequest.AddPartETags(uploadResponses);
                CompleteMultipartUploadResponse completeUploadResponse = _client.CompleteMultipartUpload(completeRequest);
            }
            catch (ObsException ex)
            {
                return ApiCommon.GetException(ex);
            }
            return ApiCommon.GetSuccess();
        }
        /// <summary>
        /// 普通上传
        /// </summary>
        /// <param name="path"></param>
        /// <param name="stm"></param>
        private APIResault FileUpload(string path, string filePath)
        {
            PutObjectRequest request = new PutObjectRequest()
            {
                BucketName = _bucketName,
                ObjectKey = path,
                FilePath = filePath,

            };
            if (_needHash)
            {
                using (FileStream stm = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    string md5 = OssUtils.ComputeContentMd5(stm, stm.Length); ;
                    request.ContentMd5 = md5;
                }

            }

            PutObjectResponse response = _client.PutObject(request);
            return ApiCommon.GetSuccess();
        }
        /// <summary>
        /// 上传
        /// </summary>
        /// <param name="path"></param>
        /// <param name="stm"></param>
        private APIResault StreamUpload(string path,Stream stm)
        {

            PutObjectRequest request = new PutObjectRequest()
            {
                BucketName = _bucketName,
                ObjectKey = path,
                InputStream = stm,
                
            };
            if (_needHash)
            {
                stm.Position = 0;
                string md5 = OssUtils.ComputeContentMd5(stm, stm.Length); ;
                request.ContentMd5 = md5;
                stm.Position = 0;
            }

            PutObjectResponse response = _client.PutObject(request);
            return ApiCommon.GetSuccess();
        }
    }
}
