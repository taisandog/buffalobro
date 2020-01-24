using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Buffalo.ArgCommon;
using Buffalo.Kernel;
using Buffalo.Storage.AliCloud.OssAPI;
using Buffalo.Storage.QCloud.CosApi;
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
    public partial class AWSS3Adapter : IFileStorage
    {
        /// <summary>
        /// 对象是否存在
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        internal bool ExistsMetadata(string key)
        {
            
            try
            {
                GetObjectMetadataRequest request = new GetObjectMetadataRequest()
                {
                    BucketName = _bucketName,
                    Key = key
                };
                GetObjectMetadataResponse response = _client.GetObjectMetadataAsync(request).Result;
                return true;
            }
            catch (AmazonS3Exception e)
            {
                if (string.Equals(e.ErrorCode, "NoSuchBucket"))
                {
                    
                    return false;
                }
                else if (string.Equals(e.ErrorCode, "NotFound"))
                {
                    return false;
                }
                throw;
            }
        }

    }
}
