using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Amazon.Util;
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
            key = EncodeKey(key);
            try
            {
                GetObjectMetadataRequest request = new GetObjectMetadataRequest()
                {
                    BucketName = _bucketName,
                    Key = key
                };
                ((Amazon.Runtime.Internal.IAmazonWebServiceRequest)request).AddBeforeRequestHandler(FileIORequestEventHandler);
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
        internal static void FileIORequestEventHandler(object sender, RequestEventArgs args)
        {
            WebServiceRequestEventArgs wsArgs = args as WebServiceRequestEventArgs;
            if (wsArgs != null)
            {
                string currentUserAgent = wsArgs.Headers[AWSSDKUtils.UserAgentHeader];
                wsArgs.Headers[AWSSDKUtils.UserAgentHeader] = currentUserAgent + " FileIO";
            }
        }
        internal static string EncodeKey(string key)
        {
            return key.Replace('\\', '/');
        }
        internal static string DecodeKey(string key)
        {
            return key.Replace('/', '\\');
        }
    }
}
