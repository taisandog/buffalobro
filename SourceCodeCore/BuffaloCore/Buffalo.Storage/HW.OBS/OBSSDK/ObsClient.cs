namespace OBS
{
    using OBS.Internal;
    //using OBS.Internal.Log;
    using OBS.Model;
    using System;
    using System.Net;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Text;

    public class ObsClient
    {
        internal readonly object _lock;
        private HttpClient httpClient;

        public ObsClient(string accessKeyId, string secretAccessKey, OBS.ObsConfig obsConfig) : this(accessKeyId, secretAccessKey, "", obsConfig)
        {
        }

        public ObsClient(string accessKeyId, string secretAccessKey, string endpoint) : this(accessKeyId, secretAccessKey, "", endpoint)
        {
        }

        public ObsClient(string accessKeyId, string secretAccessKey, string securityToken, OBS.ObsConfig obsConfig)
        {
            this._lock = new object();
            this.init(accessKeyId, secretAccessKey, securityToken, obsConfig);
        }

        public ObsClient(string accessKeyId, string secretAccessKey, string securityToken, string endpoint)
        {
            this._lock = new object();
            OBS.ObsConfig obsConfig = new OBS.ObsConfig {
                Endpoint = endpoint
            };
            this.init(accessKeyId, secretAccessKey, securityToken, obsConfig);
        }

        public AbortMultipartUploadResponse AbortMultipartUpload(AbortMultipartUploadRequest request)
        {
            return this.doRequest<AbortMultipartUploadRequest, AbortMultipartUploadResponse>(request, delegate {
                if (request.ObjectKey == null)
                {
                    throw new ObsException("object key is null", ErrorType.Sender, "InvalidObjectKey", "");
                }
                if (string.IsNullOrEmpty(request.UploadId))
                {
                    throw new ObsException("upload id is not valid", ErrorType.Sender, "InvalidUploadId", "");
                }
            });
        }

        public AppendObjectResponse AppendObject(AppendObjectRequest request)
        {
            return this.doRequest<AppendObjectRequest, AppendObjectResponse>(request, delegate {
                if (request.ObjectKey == null)
                {
                    throw new ObsException("object key is null", ErrorType.Sender, "InvalidObjectKey", "");
                }
            });
        }

        public CompleteMultipartUploadResponse CompleteMultipartUpload(CompleteMultipartUploadRequest request)
        {
            return this.doRequest<CompleteMultipartUploadRequest, CompleteMultipartUploadResponse>(request, delegate {
                if (request.ObjectKey == null)
                {
                    throw new ObsException("object key is null", ErrorType.Sender, "InvalidObjectKey", "");
                }
                if (string.IsNullOrEmpty(request.UploadId))
                {
                    throw new ObsException("upload id is not valid", ErrorType.Sender, "InvalidUploadId", "");
                }
            });
        }

        public CopyObjectResponse CopyObject(CopyObjectRequest request)
        {
            return this.doRequest<CopyObjectRequest, CopyObjectResponse>(request, delegate {
                if (request.ObjectKey == null)
                {
                    throw new ObsException("object key is null", ErrorType.Sender, "InvalidObjectKey", "");
                }
                if (string.IsNullOrEmpty(request.SourceBucketName))
                {
                    throw new ObsException("source object key is null", ErrorType.Sender, "InvalidBucketName", "");
                }
                if (request.SourceObjectKey == null)
                {
                    throw new ObsException("source bucket name is null", ErrorType.Sender, "InvalidObjectKey", "");
                }
            });
        }

        public CopyPartResponse CopyPart(CopyPartRequest request)
        {
            CopyPartResponse local1 = this.doRequest<CopyPartRequest, CopyPartResponse>(request, delegate {
                if (request.ObjectKey == null)
                {
                    throw new ObsException("object key is null", ErrorType.Sender, "InvalidObjectKey", "");
                }
                if (string.IsNullOrEmpty(request.UploadId))
                {
                    throw new ObsException("upload id is not valid", ErrorType.Sender, "InvalidUploadId", "");
                }
                if (request.PartNumber <= 0)
                {
                    throw new ObsException("part number is not valid", ErrorType.Sender, "InvalidPartNumber", "");
                }
                if (string.IsNullOrEmpty(request.SourceBucketName))
                {
                    throw new ObsException("source object key is null", ErrorType.Sender, "InvalidBucketName", "");
                }
                if (request.SourceObjectKey == null)
                {
                    throw new ObsException("source bucket name is null", ErrorType.Sender, "InvalidObjectKey", "");
                }
            });
            local1.PartNumber = request.PartNumber;
            return local1;
        }

        public CreateBucketResponse CreateBucket(CreateBucketRequest request)
        {
            return this.doRequest<CreateBucketRequest, CreateBucketResponse>(request);
        }

        public CreatePostSignatureResponse CreatePostSignature(CreatePostSignatureRequest request)
        {
            throw new NotImplementedException();
        }

        public CreateTemporarySignatureResponse CreateTemporarySignature(CreateTemporarySignatureRequest request)
        {
            throw new NotImplementedException();
        }

        public DeleteBucketResponse DeleteBucket(DeleteBucketRequest request)
        {
            return this.doRequest<DeleteBucketRequest, DeleteBucketResponse>(request);
        }

        public DeleteBucketCorsResponse DeleteBucketCors(DeleteBucketCorsRequest request)
        {
            return this.doRequest<DeleteBucketCorsRequest, DeleteBucketCorsResponse>(request);
        }

        public DeleteBucketLifecycleResponse DeleteBucketLifecycle(DeleteBucketLifecycleRequest request)
        {
            return this.doRequest<DeleteBucketLifecycleRequest, DeleteBucketLifecycleResponse>(request);
        }

        public DeleteBucketPolicyResponse DeleteBucketPolicy(DeleteBucketPolicyRequest request)
        {
            return this.doRequest<DeleteBucketPolicyRequest, DeleteBucketPolicyResponse>(request);
        }

        public DeleteBucketReplicationResponse DeleteBucketReplication(DeleteBucketReplicationRequest request)
        {
            return this.doRequest<DeleteBucketReplicationRequest, DeleteBucketReplicationResponse>(request);
        }

        public DeleteBucketTaggingResponse DeleteBucketTagging(DeleteBucketTaggingRequest request)
        {
            return this.doRequest<DeleteBucketTaggingRequest, DeleteBucketTaggingResponse>(request);
        }

        public DeleteBucketWebsiteResponse DeleteBucketWebsite(DeleteBucketWebsiteRequest request)
        {
            return this.doRequest<DeleteBucketWebsiteRequest, DeleteBucketWebsiteResponse>(request);
        }

        public DeleteObjectResponse DeleteObject(DeleteObjectRequest request)
        {
            return this.doRequest<DeleteObjectRequest, DeleteObjectResponse>(request, delegate {
                if (request.ObjectKey == null)
                {
                    throw new ObsException("object key is null", ErrorType.Sender, "InvalidObjectKey", "");
                }
            });
        }

        public DeleteObjectsResponse DeleteObjects(DeleteObjectsRequest request)
        {
            return this.doRequest<DeleteObjectsRequest, DeleteObjectsResponse>(request);
        }

        internal K doRequest<T, K>(T request) where T: ObsWebServiceRequest where K: ObsWebServiceResponse
        {
            return this.doRequest<T, K>(request, null);
        }

        internal K doRequest<T, K>(T request, DoValidateDelegate doValidateDelegate) where T: ObsWebServiceRequest where K: ObsWebServiceResponse
        {
            K local2;
            if (request == null)
            {
                throw new ObsException("request is null", ErrorType.Sender, "NullRequest", "");
            }
            ObsBucketWebServiceRequest request2 = request as ObsBucketWebServiceRequest;
            if ((request2 != null) && string.IsNullOrEmpty(request2.BucketName))
            {
                throw new ObsException("bucket name is not valid", ErrorType.Sender, "InvalidBucketName", "");
            }
            if (doValidateDelegate != null)
            {
                doValidateDelegate();
            }
            //if (LoggerMgr.IsInfoEnabled)
            //{
            //    LoggerMgr.Info(request.GetAction() + " begin.");
            //}
            DateTime now = DateTime.Now;
            HttpRequest request3 = null;
            try
            {
                K local;
                IConvertor iConvertor = this.GetIConvertor();
                object[] parameters = new object[] { request };
                request3 = CommonUtil.GetTransMethodInfo(request.GetType(), iConvertor).Invoke(iConvertor, parameters) as HttpRequest;
                if (request3 == null)
                {
                    throw new ObsException(string.Format("Cannot trans request:{0} to HttpRequest", request.GetType()), ErrorType.Sender, "Trans error", "");
                }
                request3.Endpoint = this.ObsConfig.Endpoint;
                request3.PathStyle = this.ObsConfig.PathStyle;
                HttpContext context = new HttpContext(this.SecurityProvider, this.ObsConfig);
                HttpResponse httpResponse = this.httpClient.PerformRequest(request3, context);
                IParser iParser = this.GetIParser();
                Type responseType = typeof(K);
                MethodInfo parseMethodInfo = CommonUtil.GetParseMethodInfo(responseType, iParser);
                if (parseMethodInfo != null)
                {
                    object[] objArray2 = new object[] { httpResponse };
                    local = parseMethodInfo.Invoke(iParser, objArray2) as K;
                }
                else
                {
                    local = responseType.GetConstructor(new Type[0]).Invoke(null) as K;
                }
                if (local == null)
                {
                    throw new ObsException(string.Format("Cannot parse HttpResponse to {0}", typeof(K)), ErrorType.Sender, "Parse error", "");
                }
                CommonParser.ParseObsWebServiceResponse(httpResponse, local, this.httpClient.GetIHeaders());
                local2 = local;
            }
            catch (ObsException exception)
            {
                //if (LoggerMgr.IsErrorEnabled)
                //{
                //    LoggerMgr.Error(string.Format("{0} exception code: {1}, with message: {2}", request.GetAction(), exception.ErrorCode, exception.Message));
                //}
                throw exception;
            }
            finally
            {
                if (request3 != null)
                {
                    request3.Dispose();
                }
                //if (LoggerMgr.IsInfoEnabled)
                //{
                //    LoggerMgr.Info(string.Format("{0} end, cost {1} ms", request.GetAction(), (DateTime.Now.Ticks - now.Ticks) / 0x2710L));
                //}
            }
            return local2;
        }

        public GetObjectMetadataResponse DownloadFile(DownloadFileRequest request)
        {
            throw new NotImplementedException();
        }

        public GetBucketAclResponse GetBucketAcl(GetBucketAclRequest request)
        {
            return this.doRequest<GetBucketAclRequest, GetBucketAclResponse>(request);
        }

        public GetBucketCorsResponse GetBucketCors(GetBucketCorsRequest request)
        {
            return this.doRequest<GetBucketCorsRequest, GetBucketCorsResponse>(request);
        }

        public GetBucketLifecycleResponse GetBucketLifecycle(GetBucketLifecycleRequest request)
        {
            return this.doRequest<GetBucketLifecycleRequest, GetBucketLifecycleResponse>(request);
        }

        public GetBucketLocationResponse GetBucketLocation(GetBucketLocationRequest request)
        {
            return this.doRequest<GetBucketLocationRequest, GetBucketLocationResponse>(request);
        }

        public GetBucketLoggingResponse GetBucketLogging(GetBucketLoggingRequest request)
        {
            return this.doRequest<GetBucketLoggingRequest, GetBucketLoggingResponse>(request);
        }

        public GetBucketMetadataResponse GetBucketMetadata(GetBucketMetadataRequest request)
        {
            return this.doRequest<GetBucketMetadataRequest, GetBucketMetadataResponse>(request);
        }

        public GetBucketNotificationReponse GetBucketNotification(GetBucketNotificationRequest request)
        {
            return this.doRequest<GetBucketNotificationRequest, GetBucketNotificationReponse>(request);
        }

        public GetBucketPolicyResponse GetBucketPolicy(GetBucketPolicyRequest request)
        {
            return this.doRequest<GetBucketPolicyRequest, GetBucketPolicyResponse>(request);
        }

        public GetBucketQuotaResponse GetBucketQuota(GetBucketQuotaRequest request)
        {
            return this.doRequest<GetBucketQuotaRequest, GetBucketQuotaResponse>(request);
        }

        public GetBucketReplicationResponse GetBucketReplication(GetBucketReplicationRequest request)
        {
            return this.doRequest<GetBucketReplicationRequest, GetBucketReplicationResponse>(request);
        }

        public GetBucketStorageInfoResponse GetBucketStorageInfo(GetBucketStorageInfoRequest request)
        {
            return this.doRequest<GetBucketStorageInfoRequest, GetBucketStorageInfoResponse>(request);
        }

        public GetBucketStoragePolicyResponse GetBucketStoragePolicy(GetBucketStoragePolicyRequest request)
        {
            return this.doRequest<GetBucketStoragePolicyRequest, GetBucketStoragePolicyResponse>(request);
        }

        public GetBucketTaggingResponse GetBucketTagging(GetBucketTaggingRequest request)
        {
            return this.doRequest<GetBucketTaggingRequest, GetBucketTaggingResponse>(request);
        }

        public GetBucketVersioningResponse GetBucketVersioning(GetBucketVersioningRequest request)
        {
            return this.doRequest<GetBucketVersioningRequest, GetBucketVersioningResponse>(request);
        }

        public GetBucketWebsiteResponse GetBucketWebsite(GetBucketWebsiteRequest request)
        {
            return this.doRequest<GetBucketWebsiteRequest, GetBucketWebsiteResponse>(request);
        }

        internal IConvertor GetIConvertor()
        {
            if (this.ObsConfig.AuthType <= AuthTypeEnum.V4)
            {
                return V2Convertor.GetInstance(this.httpClient.GetIHeaders());
            }
            return ObsConvertor.GetInstance(this.httpClient.GetIHeaders());
        }

        internal IParser GetIParser()
        {
            if (this.ObsConfig.AuthType <= AuthTypeEnum.V4)
            {
                return V2Parser.GetInstance(this.httpClient.GetIHeaders());
            }
            return ObsParser.GetInstance(this.httpClient.GetIHeaders());
        }

        public GetObjectResponse GetObject(GetObjectRequest request)
        {
            GetObjectResponse local1 = this.doRequest<GetObjectRequest, GetObjectResponse>(request, delegate {
                if (request.ObjectKey == null)
                {
                    throw new ObsException("object key is null", ErrorType.Sender, "InvalidObjectKey", "");
                }
            });
            local1.BucketName = request.BucketName;
            local1.ObjectKey = request.ObjectKey;
            return local1;
        }

        public GetObjectAclResponse GetObjectAcl(GetObjectAclRequest request)
        {
            return this.doRequest<GetObjectAclRequest, GetObjectAclResponse>(request, delegate {
                if (request.ObjectKey == null)
                {
                    throw new ObsException("object key is null", ErrorType.Sender, "InvalidObjectKey", "");
                }
            });
        }

        public GetObjectMetadataResponse GetObjectMetadata(GetObjectMetadataRequest request)
        {
            GetObjectMetadataResponse local1 = this.doRequest<GetObjectMetadataRequest, GetObjectMetadataResponse>(request, delegate {
                if (request.ObjectKey == null)
                {
                    throw new ObsException("object key is null", ErrorType.Sender, "InvalidObjectKey", "");
                }
            });
            local1.BucketName = request.BucketName;
            local1.ObjectKey = request.ObjectKey;
            return local1;
        }

        public GetObjectMetadataResponse GetObjectMetadata(string bucketName, string objectKey)
        {
            GetObjectMetadataRequest request = new GetObjectMetadataRequest {
                BucketName = bucketName,
                ObjectKey = objectKey
            };
            return this.GetObjectMetadata(request);
        }

        public GetObjectMetadataResponse GetObjectMetadata(string bucketName, string objectKey, string versionId)
        {
            GetObjectMetadataRequest request = new GetObjectMetadataRequest {
                BucketName = bucketName,
                ObjectKey = objectKey,
                VersionId = versionId
            };
            return this.GetObjectMetadata(request);
        }

        public bool HeadBucket(HeadBucketRequest request)
        {
            try
            {
                this.doRequest<HeadBucketRequest, ObsWebServiceResponse>(request);
                return true;
            }
            catch (ObsException exception)
            {
                if (exception.StatusCode != HttpStatusCode.NotFound)
                {
                    throw exception;
                }
                return false;
            }
        }

        internal void init(string accessKeyId, string secretAccessKey, string securityToken, OBS.ObsConfig obsConfig)
        {
            if ((obsConfig == null) || string.IsNullOrEmpty(obsConfig.Endpoint))
            {
                throw new ObsException("endpoint is not valid", ErrorType.Sender, "InvalidEndpoint", "");
            }
            OBS.Internal.SecurityProvider provider = new OBS.Internal.SecurityProvider {
                Ak = accessKeyId,
                Sk = secretAccessKey,
                Token = securityToken
            };
            this.SecurityProvider = provider;
            this.ObsConfig = obsConfig;
            this.httpClient = new HttpClient(this.ObsConfig);
            //LoggerMgr.Initialize();
            //if (LoggerMgr.IsWarnEnabled)
            //{
            //    StringBuilder builder1 = new StringBuilder();
            //    builder1.Append("[OBS SDK Version=").Append("3.0.0").Append("];[").Append("Endpoint=").Append(obsConfig.Endpoint).Append("];[").Append("Access Mode=").Append(obsConfig.PathStyle ? "Path" : "Virtual Hosting").Append("]");
            //    LoggerMgr.Warn(builder1.ToString());
            //}
        }

        public InitiateMultipartUploadResponse InitiateMultipartUpload(InitiateMultipartUploadRequest request)
        {
            return this.doRequest<InitiateMultipartUploadRequest, InitiateMultipartUploadResponse>(request, delegate {
                if (request.ObjectKey == null)
                {
                    throw new ObsException("object key is null", ErrorType.Sender, "InvalidObjectKey", "");
                }
            });
        }

        public ListBucketsResponse ListBuckets(ListBucketsRequest request)
        {
            return this.doRequest<ListBucketsRequest, ListBucketsResponse>(request);
        }

        public ListMultipartUploadsResponse ListMultipartUploads(ListMultipartUploadsRequest request)
        {
            return this.doRequest<ListMultipartUploadsRequest, ListMultipartUploadsResponse>(request);
        }

        public ListObjectsResponse ListObjects(ListObjectsRequest request)
        {
            return this.doRequest<ListObjectsRequest, ListObjectsResponse>(request);
        }

        public ListPartsResponse ListParts(ListPartsRequest request)
        {
            return this.doRequest<ListPartsRequest, ListPartsResponse>(request, delegate {
                if (request.ObjectKey == null)
                {
                    throw new ObsException("object key is null", ErrorType.Sender, "InvalidObjectKey", "");
                }
                if (string.IsNullOrEmpty(request.UploadId))
                {
                    throw new ObsException("upload id is not valid", ErrorType.Sender, "InvalidUploadId", "");
                }
            });
        }

        public ListVersionsResponse ListVersions(ListVersionsRequest request)
        {
            return this.doRequest<ListVersionsRequest, ListVersionsResponse>(request);
        }

        public PutObjectResponse PutObject(PutObjectRequest request)
        {
            return this.doRequest<PutObjectRequest, PutObjectResponse>(request, delegate {
                if (request.ObjectKey == null)
                {
                    throw new ObsException("object key is null", ErrorType.Sender, "InvalidObjectKey", "");
                }
            });
        }

        public void Refresh(string accessKeyId, string secretAccessKey, string securityToken)
        {
            object obj2 = this._lock;
            lock (obj2)
            {
                OBS.Internal.SecurityProvider provider = new OBS.Internal.SecurityProvider {
                    Ak = accessKeyId,
                    Sk = secretAccessKey,
                    Token = securityToken
                };
                this.SecurityProvider = provider;
            }
        }

        public RestoreObjectResponse RestoreObject(RestoreObjectRequest request)
        {
            return this.doRequest<RestoreObjectRequest, RestoreObjectResponse>(request, delegate {
                if (request.ObjectKey == null)
                {
                    throw new ObsException("object key is null", ErrorType.Sender, "InvalidObjectKey", "");
                }
            });
        }

        public SetBucketAclResponse SetBucketAcl(SetBucketAclRequest request)
        {
            return this.doRequest<SetBucketAclRequest, SetBucketAclResponse>(request);
        }

        public SetBucketCorsResponse SetBucketCors(SetBucketCorsRequest request)
        {
            return this.doRequest<SetBucketCorsRequest, SetBucketCorsResponse>(request);
        }

        public SetBucketLifecycleResponse SetBucketLifecycle(SetBucketLifecycleRequest request)
        {
            return this.doRequest<SetBucketLifecycleRequest, SetBucketLifecycleResponse>(request);
        }

        public SetBucketLoggingResponse SetBucketLogging(SetBucketLoggingRequest request)
        {
            return this.doRequest<SetBucketLoggingRequest, SetBucketLoggingResponse>(request);
        }

        public SetBucketNotificationResponse SetBucketNotification(SetBucketNotificationRequest request)
        {
            return this.doRequest<SetBucketNotificationRequest, SetBucketNotificationResponse>(request);
        }

        public SetBucketPolicyResponse SetBucketPolicy(SetBucketPolicyRequest request)
        {
            return this.doRequest<SetBucketPolicyRequest, SetBucketPolicyResponse>(request);
        }

        public SetBucketQuotaResponse SetBucketQuota(SetBucketQuotaRequest request)
        {
            return this.doRequest<SetBucketQuotaRequest, SetBucketQuotaResponse>(request);
        }

        public SetBucketReplicationResponse SetBucketReplication(SetBucketReplicationRequest request)
        {
            return this.doRequest<SetBucketReplicationRequest, SetBucketReplicationResponse>(request);
        }

        public SetBucketStoragePolicyResponse SetBucketStoragePolicy(SetBucketStoragePolicyRequest request)
        {
            return this.doRequest<SetBucketStoragePolicyRequest, SetBucketStoragePolicyResponse>(request);
        }

        public SetBucketTaggingResponse SetBucketTagging(SetBucketTaggingRequest request)
        {
            return this.doRequest<SetBucketTaggingRequest, SetBucketTaggingResponse>(request);
        }

        public SetBucketVersioningResponse SetBucketVersioning(SetBucketVersioningRequest request)
        {
            return this.doRequest<SetBucketVersioningRequest, SetBucketVersioningResponse>(request);
        }

        public SetBucketWebsiteResponse SetBucketWebsiteConfiguration(SetBucketWebsiteRequest request)
        {
            return this.doRequest<SetBucketWebsiteRequest, SetBucketWebsiteResponse>(request);
        }

        public SetObjectAclResponse SetObjectAcl(SetObjectAclRequest request)
        {
            return this.doRequest<SetObjectAclRequest, SetObjectAclResponse>(request, delegate {
                if (request.ObjectKey == null)
                {
                    throw new ObsException("object key is null", ErrorType.Sender, "InvalidObjectKey", "");
                }
            });
        }

        public CompleteMultipartUploadResponse UploadFile(UploadFileRequest request)
        {
            throw new NotImplementedException();
        }

        public UploadPartResponse UploadPart(UploadPartRequest request)
        {
            UploadPartResponse local1 = this.doRequest<UploadPartRequest, UploadPartResponse>(request, delegate {
                if (request.ObjectKey == null)
                {
                    throw new ObsException("object key is null", ErrorType.Sender, "InvalidObjectKey", "");
                }
                if (string.IsNullOrEmpty(request.UploadId))
                {
                    throw new ObsException("upload id is not valid", ErrorType.Sender, "InvalidUploadId", "");
                }
                if (request.PartNumber <= 0)
                {
                    throw new ObsException("part number is not valid", ErrorType.Sender, "InvalidPartNumber", "");
                }
            });
            local1.PartNumber = request.PartNumber;
            return local1;
        }

        internal OBS.ObsConfig ObsConfig { get; set; }

        internal OBS.Internal.SecurityProvider SecurityProvider { get; set; }

        internal delegate void DoValidateDelegate();
    }
}

