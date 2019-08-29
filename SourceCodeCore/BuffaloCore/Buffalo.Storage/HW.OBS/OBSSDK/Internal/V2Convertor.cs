namespace OBS.Internal
{
    using OBS.Model;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Xml;

    internal class V2Convertor : IConvertor
    {
        protected IHeaders iheaders;

        protected V2Convertor(IHeaders iheaders)
        {
            this.iheaders = iheaders;
        }

        public static IConvertor GetInstance(IHeaders iheaders)
        {
            return new V2Convertor(iheaders);
        }

        public HttpRequest Trans(AbortMultipartUploadRequest request)
        {
            HttpRequest request1 = new HttpRequest {
                BucketName = request.BucketName,
                ObjectKey = request.ObjectKey,
                Method = HttpVerb.DELETE
            };
            request1.Params.Add("uploadId", request.UploadId);
            return request1;
        }

        public HttpRequest Trans(AppendObjectRequest request)
        {
            HttpRequest request1 = this.Trans((PutObjectRequest) request);
            request1.Method = HttpVerb.POST;
            request1.Params.Add(EnumAdaptor.GetStringValue(SubResourceEnum.Append), null);
            request1.Params.Add("position", request.Position.ToString());
            return request1;
        }

        public HttpRequest Trans(CompleteMultipartUploadRequest request)
        {
            HttpRequest httpRequest = new HttpRequest {
                BucketName = request.BucketName,
                ObjectKey = request.ObjectKey,
                Method = HttpVerb.POST
            };
            httpRequest.Params.Add("uploadId", request.UploadId);
            List<PartETag> temp = request.PartETags as List<PartETag>;
            if (temp == null)
            {
                temp = new List<PartETag>();
                foreach (PartETag tag in request.PartETags)
                {
                    temp.Add(tag);
                }
            }
            temp.Sort(Traner.Comparison1 ?? (Traner.Comparison1 = new Comparison<PartETag>(Traner.CurTraner.DoTrans)));
            this.TransContent(httpRequest, delegate (XmlWriter xmlWriter) {
                xmlWriter.WriteStartElement("CompleteMultipartUpload");
                foreach (PartETag tag in temp)
                {
                    xmlWriter.WriteStartElement("Part");
                    xmlWriter.WriteElementString("PartNumber", tag.PartNumber.ToString());
                    if (!string.IsNullOrEmpty(tag.ETag))
                    {
                        xmlWriter.WriteElementString("ETag", tag.ETag);
                    }
                    xmlWriter.WriteEndElement();
                }
                xmlWriter.WriteEndElement();
            });
            return httpRequest;
        }

        public HttpRequest Trans(CopyObjectRequest request)
        {
            HttpRequest httpRequest = new HttpRequest();
            this.TransPutObjectBasicRequest(request, httpRequest);
            httpRequest.Method = HttpVerb.PUT;
            string str = string.Format("{0}/{1}", request.SourceBucketName, request.SourceObjectKey);
            if (!string.IsNullOrEmpty(request.SourceVersionId))
            {
                str = str + "?versionId" + request.SourceVersionId;
            }
            CommonUtil.AddHeader(httpRequest, this.iheaders.CopySourceHeader(), str);
            if (!string.IsNullOrEmpty(request.ContentType))
            {
                CommonUtil.AddHeader(httpRequest, "Content-Type", request.ContentType);
            }
            CommonUtil.AddHeader(httpRequest, this.iheaders.MetadataDirectiveHeader(), request.MetadataDirective.ToString().ToUpper());
            this.TransSourceSseCHeader(httpRequest, request.SourceSseCHeader);
            if (!string.IsNullOrEmpty(request.IfMatch))
            {
                CommonUtil.AddHeader(httpRequest, this.iheaders.CopySourceIfMatchHeader(), request.IfMatch);
            }
            if (!string.IsNullOrEmpty(request.IfNoneMatch))
            {
                CommonUtil.AddHeader(httpRequest, this.iheaders.CopySourceIfNoneMatchHeader(), request.IfNoneMatch);
            }
            if (request.IfModifiedSince.HasValue)
            {
                CommonUtil.AddHeader(httpRequest, this.iheaders.CopySourceIfModifiedSinceHeader(), request.IfModifiedSince.Value.ToString(@"ddd, dd MMM yyyy HH:mm:ss \G\M\T", Constants.CultureInfo));
            }
            if (request.IfUnmodifiedSince.HasValue)
            {
                CommonUtil.AddHeader(httpRequest, this.iheaders.CopySourceIfUnmodifiedSinceHeader(), request.IfUnmodifiedSince.Value.ToString(@"ddd, dd MMM yyyy HH:mm:ss \G\M\T", Constants.CultureInfo));
            }
            return httpRequest;
        }

        public HttpRequest Trans(CopyPartRequest request)
        {
            HttpRequest request2 = new HttpRequest {
                Method = HttpVerb.PUT,
                BucketName = request.BucketName,
                ObjectKey = request.ObjectKey
            };
            request2.Params.Add("uploadId", request.UploadId);
            request2.Params.Add("partNumber", request.PartNumber.ToString());
            string str = string.Format("{0}/{1}", request.SourceBucketName, request.SourceObjectKey);
            if (!string.IsNullOrEmpty(request.SourceVersionId))
            {
                str = str + "?versionId" + request.SourceVersionId;
            }
            CommonUtil.AddHeader(request2, this.iheaders.CopySourceHeader(), str);
            if (((request.ByteRange != null) && (request.ByteRange.Start >= 0L)) && (request.ByteRange.Start <= request.ByteRange.End))
            {
                CommonUtil.AddHeader(request2, this.iheaders.CopySourceRangeHeader(), string.Format("bytes={0}-{1}", request.ByteRange.Start, request.ByteRange.End));
            }
            this.TransSourceSseCHeader(request2, request.SourceSseCHeader);
            this.TransSseCHeader(request2, request.DestinationSseCHeader);
            return request2;
        }

        public HttpRequest Trans(CreateBucketRequest request)
        {
            HttpRequest request2 = new HttpRequest {
                Method = HttpVerb.PUT,
                BucketName = request.BucketName
            };
            if (request.CannedAcl.HasValue)
            {
                CommonUtil.AddHeader(request2, this.iheaders.AclHeader(), this.TransBucketCannedAcl(request.CannedAcl.Value));
            }
            if (request.StorageClass.HasValue)
            {
                CommonUtil.AddHeader(request2, this.iheaders.DefaultStorageClassHeader(), this.TransStorageClass(request.StorageClass.Value));
            }
            foreach (KeyValuePair<ExtensionBucketPermissionEnum, IList<string>> pair in request.ExtensionPermissionMap)
            {
                string str = null;
                switch (pair.Key)
                {
                    case ExtensionBucketPermissionEnum.GrantRead:
                        str = this.iheaders.GrantReadHeader();
                        break;

                    case ExtensionBucketPermissionEnum.GrantWrite:
                        str = this.iheaders.GrantWriteHeader();
                        break;

                    case ExtensionBucketPermissionEnum.GrantReadAcp:
                        str = this.iheaders.GrantReadAcpHeader();
                        break;

                    case ExtensionBucketPermissionEnum.GrantWriteAcp:
                        str = this.iheaders.GrantWriteAcpHeader();
                        break;

                    case ExtensionBucketPermissionEnum.GrantFullControl:
                        str = this.iheaders.GrantFullControlHeader();
                        break;

                    case ExtensionBucketPermissionEnum.GrantReadDelivered:
                        str = this.iheaders.GrantReadDeliveredHeader();
                        break;

                    case ExtensionBucketPermissionEnum.GrantFullControlDelivered:
                        str = this.iheaders.GrantFullControlDeliveredHeader();
                        break;
                }
                if ((!string.IsNullOrEmpty(str) && (pair.Value != null)) && (pair.Value.Count > 0))
                {
                    string[] strArray = new string[pair.Value.Count];
                    for (int i = 0; i < pair.Value.Count; i++)
                    {
                        strArray[i] = "id=" + pair.Value[i];
                    }
                    CommonUtil.AddHeader(request2, str, string.Join(",", strArray));
                }
            }
            if (!string.IsNullOrEmpty(request.Location))
            {
                this.TransContent(request2, delegate (XmlWriter xmlWriter) {
                    xmlWriter.WriteStartElement("CreateBucketConfiguration");
                    xmlWriter.WriteElementString(this.BucketLocationTag, request.Location);
                    xmlWriter.WriteEndElement();
                });
            }
            return request2;
        }

        public HttpRequest Trans(DeleteBucketCorsRequest request)
        {
            HttpRequest request1 = new HttpRequest {
                BucketName = request.BucketName,
                Method = HttpVerb.DELETE
            };
            request1.Params.Add(EnumAdaptor.GetStringValue(SubResourceEnum.Cors), null);
            return request1;
        }

        public HttpRequest Trans(DeleteBucketLifecycleRequest request)
        {
            HttpRequest request1 = new HttpRequest {
                BucketName = request.BucketName,
                Method = HttpVerb.DELETE
            };
            request1.Params.Add(EnumAdaptor.GetStringValue(SubResourceEnum.Lifecyle), null);
            return request1;
        }

        public HttpRequest Trans(DeleteBucketPolicyRequest request)
        {
            HttpRequest request1 = new HttpRequest {
                BucketName = request.BucketName,
                Method = HttpVerb.DELETE
            };
            request1.Params.Add(EnumAdaptor.GetStringValue(SubResourceEnum.Policy), null);
            return request1;
        }

        public HttpRequest Trans(DeleteBucketReplicationRequest request)
        {
            HttpRequest request1 = new HttpRequest {
                Method = HttpVerb.DELETE,
                BucketName = request.BucketName
            };
            request1.Params.Add(EnumAdaptor.GetStringValue(SubResourceEnum.Replication), null);
            return request1;
        }

        public HttpRequest Trans(DeleteBucketRequest request)
        {
            return new HttpRequest { Method = HttpVerb.DELETE, BucketName = request.BucketName };
        }

        public HttpRequest Trans(DeleteBucketTaggingRequest request)
        {
            HttpRequest request1 = new HttpRequest {
                BucketName = request.BucketName,
                Method = HttpVerb.DELETE
            };
            request1.Params.Add(EnumAdaptor.GetStringValue(SubResourceEnum.Tagging), null);
            return request1;
        }

        public HttpRequest Trans(DeleteBucketWebsiteRequest request)
        {
            HttpRequest request1 = new HttpRequest {
                BucketName = request.BucketName,
                Method = HttpVerb.DELETE
            };
            request1.Params.Add(EnumAdaptor.GetStringValue(SubResourceEnum.Website), null);
            return request1;
        }

        public HttpRequest Trans(DeleteObjectRequest request)
        {
            HttpRequest request2 = new HttpRequest {
                BucketName = request.BucketName,
                ObjectKey = request.ObjectKey,
                Method = HttpVerb.DELETE
            };
            if (!string.IsNullOrEmpty(request.VersionId))
            {
                request2.Params.Add("versionId", request.VersionId);
            }
            return request2;
        }

        public HttpRequest Trans(DeleteObjectsRequest request)
        {
            HttpRequest httpRequest = new HttpRequest {
                BucketName = request.BucketName,
                Method = HttpVerb.POST
            };
            httpRequest.Params.Add(EnumAdaptor.GetStringValue(SubResourceEnum.Delete), null);
            this.TransContent(httpRequest, delegate (XmlWriter xmlWriter) {
                xmlWriter.WriteStartElement("Delete");
                if (request.Quiet.HasValue)
                {
                    xmlWriter.WriteElementString("Quiet", request.Quiet.Value.ToString().ToLower());
                }
                foreach (KeyVersion version in request.Objects)
                {
                    if (version != null)
                    {
                        xmlWriter.WriteStartElement("Object");
                        if (version.Key != null)
                        {
                            xmlWriter.WriteElementString("Key", version.Key);
                        }
                        if (!string.IsNullOrEmpty(version.VersionId))
                        {
                            xmlWriter.WriteElementString("VersionId", version.VersionId);
                        }
                        xmlWriter.WriteEndElement();
                    }
                }
                xmlWriter.WriteEndElement();
            }, true);
            return httpRequest;
        }

        public HttpRequest Trans(GetBucketAclRequest request)
        {
            HttpRequest request1 = new HttpRequest {
                Method = HttpVerb.GET
            };
            request1.Params.Add(EnumAdaptor.GetStringValue(SubResourceEnum.Acl), null);
            request1.BucketName = request.BucketName;
            return request1;
        }

        public HttpRequest Trans(GetBucketCorsRequest request)
        {
            HttpRequest request1 = new HttpRequest {
                BucketName = request.BucketName,
                Method = HttpVerb.GET
            };
            request1.Params.Add(EnumAdaptor.GetStringValue(SubResourceEnum.Cors), null);
            return request1;
        }

        public HttpRequest Trans(GetBucketLifecycleRequest request)
        {
            HttpRequest request1 = new HttpRequest {
                BucketName = request.BucketName,
                Method = HttpVerb.GET
            };
            request1.Params.Add(EnumAdaptor.GetStringValue(SubResourceEnum.Lifecyle), null);
            return request1;
        }

        public HttpRequest Trans(GetBucketLocationRequest request)
        {
            HttpRequest request1 = new HttpRequest {
                Method = HttpVerb.GET,
                BucketName = request.BucketName
            };
            request1.Params.Add(EnumAdaptor.GetStringValue(SubResourceEnum.Location), null);
            return request1;
        }

        public HttpRequest Trans(GetBucketLoggingRequest request)
        {
            HttpRequest request1 = new HttpRequest {
                BucketName = request.BucketName,
                Method = HttpVerb.GET
            };
            request1.Params.Add(EnumAdaptor.GetStringValue(SubResourceEnum.Logging), null);
            return request1;
        }

        public HttpRequest Trans(GetBucketMetadataRequest request)
        {
            HttpRequest request2 = new HttpRequest {
                Method = HttpVerb.HEAD,
                BucketName = request.BucketName
            };
            if (!string.IsNullOrEmpty(request.Origin))
            {
                CommonUtil.AddHeader(request2, "Origin", request.Origin);
            }
            foreach (string str in request.AccessControlRequestHeaders)
            {
                if (!string.IsNullOrEmpty(str))
                {
                    if (!request2.Headers.ContainsKey("Access-Control-Request-Headers"))
                    {
                        CommonUtil.AddHeader(request2, "Access-Control-Request-Headers", str);
                    }
                    else
                    {
                        IDictionary<string, string> headers = request2.Headers;
                        headers["Access-Control-Request-Headers"] = headers["Access-Control-Request-Headers"] + "," + str;
                    }
                }
            }
            return request2;
        }

        public HttpRequest Trans(GetBucketNotificationRequest request)
        {
            HttpRequest request1 = new HttpRequest {
                BucketName = request.BucketName,
                Method = HttpVerb.GET
            };
            request1.Params.Add(EnumAdaptor.GetStringValue(SubResourceEnum.Notification), null);
            return request1;
        }

        public HttpRequest Trans(GetBucketPolicyRequest request)
        {
            HttpRequest request1 = new HttpRequest {
                BucketName = request.BucketName,
                Method = HttpVerb.GET
            };
            request1.Params.Add(EnumAdaptor.GetStringValue(SubResourceEnum.Policy), null);
            return request1;
        }

        public HttpRequest Trans(GetBucketQuotaRequest request)
        {
            HttpRequest request1 = new HttpRequest {
                Method = HttpVerb.GET
            };
            request1.Params.Add(EnumAdaptor.GetStringValue(SubResourceEnum.Quota), null);
            request1.BucketName = request.BucketName;
            return request1;
        }

        public HttpRequest Trans(GetBucketReplicationRequest request)
        {
            HttpRequest request1 = new HttpRequest {
                Method = HttpVerb.GET,
                BucketName = request.BucketName
            };
            request1.Params.Add(EnumAdaptor.GetStringValue(SubResourceEnum.Replication), null);
            return request1;
        }

        public HttpRequest Trans(GetBucketStorageInfoRequest request)
        {
            HttpRequest request1 = new HttpRequest {
                Method = HttpVerb.GET,
                BucketName = request.BucketName
            };
            request1.Params.Add(EnumAdaptor.GetStringValue(SubResourceEnum.StorageInfo), null);
            return request1;
        }

        public HttpRequest Trans(GetBucketStoragePolicyRequest request)
        {
            HttpRequest request1 = new HttpRequest {
                Method = HttpVerb.GET,
                BucketName = request.BucketName
            };
            request1.Params.Add(this.BucketStoragePolicyParam, null);
            return request1;
        }

        public HttpRequest Trans(GetBucketTaggingRequest request)
        {
            HttpRequest request1 = new HttpRequest {
                BucketName = request.BucketName,
                Method = HttpVerb.GET
            };
            request1.Params.Add(EnumAdaptor.GetStringValue(SubResourceEnum.Tagging), null);
            return request1;
        }

        public HttpRequest Trans(GetBucketVersioningRequest request)
        {
            HttpRequest request1 = new HttpRequest {
                BucketName = request.BucketName,
                Method = HttpVerb.GET
            };
            request1.Params.Add(EnumAdaptor.GetStringValue(SubResourceEnum.Versioning), null);
            return request1;
        }

        public HttpRequest Trans(GetBucketWebsiteRequest request)
        {
            HttpRequest request1 = new HttpRequest {
                BucketName = request.BucketName,
                Method = HttpVerb.GET
            };
            request1.Params.Add(EnumAdaptor.GetStringValue(SubResourceEnum.Website), null);
            return request1;
        }

        public HttpRequest Trans(GetObjectAclRequest request)
        {
            HttpRequest request2 = new HttpRequest {
                BucketName = request.BucketName,
                ObjectKey = request.ObjectKey,
                Method = HttpVerb.GET
            };
            request2.Params.Add(EnumAdaptor.GetStringValue(SubResourceEnum.Acl), null);
            if (!string.IsNullOrEmpty(request.VersionId))
            {
                request2.Params.Add("versionId", request.VersionId);
            }
            return request2;
        }

        public HttpRequest Trans(GetObjectMetadataRequest request)
        {
            HttpRequest httpRequest = new HttpRequest {
                Method = HttpVerb.HEAD,
                BucketName = request.BucketName,
                ObjectKey = request.ObjectKey
            };
            if (!string.IsNullOrEmpty(request.VersionId))
            {
                httpRequest.Params.Add("versionId", request.VersionId);
            }
            this.TransSseCHeader(httpRequest, request.SseCHeader);
            return httpRequest;
        }

        public HttpRequest Trans(GetObjectRequest request)
        {
            HttpRequest request2 = this.Trans((GetObjectMetadataRequest) request);
            request2.Method = HttpVerb.GET;
            if (!string.IsNullOrEmpty(request.ImageProcess))
            {
                request2.Params.Add("x-image-process", request.ImageProcess);
            }
            if (request.ResponseHeaderOverrides != null)
            {
                if (!string.IsNullOrEmpty(request.ResponseHeaderOverrides.CacheControl))
                {
                    request2.Params.Add("response-cache-control", request.ResponseHeaderOverrides.CacheControl);
                }
                if (!string.IsNullOrEmpty(request.ResponseHeaderOverrides.ContentDisposition))
                {
                    request2.Params.Add("response-content-disposition", request.ResponseHeaderOverrides.ContentDisposition);
                }
                if (!string.IsNullOrEmpty(request.ResponseHeaderOverrides.ContentEncoding))
                {
                    request2.Params.Add("response-content-encoding", request.ResponseHeaderOverrides.ContentEncoding);
                }
                if (!string.IsNullOrEmpty(request.ResponseHeaderOverrides.ContentLanguage))
                {
                    request2.Params.Add("response-content-language", request.ResponseHeaderOverrides.ContentLanguage);
                }
                if (!string.IsNullOrEmpty(request.ResponseHeaderOverrides.ContentType))
                {
                    request2.Params.Add("response-content-type", request.ResponseHeaderOverrides.ContentType);
                }
                if (!string.IsNullOrEmpty(request.ResponseHeaderOverrides.Expires))
                {
                    request2.Params.Add("response-expires", request.ResponseHeaderOverrides.Expires);
                }
            }
            if (!string.IsNullOrEmpty(request.IfMatch))
            {
                CommonUtil.AddHeader(request2, "If-Match", request.IfMatch);
            }
            if (!string.IsNullOrEmpty(request.IfNoneMatch))
            {
                CommonUtil.AddHeader(request2, "If-None-Match", request.IfNoneMatch);
            }
            if (request.IfModifiedSince.HasValue)
            {
                CommonUtil.AddHeader(request2, "If-Modified-Since", request.IfModifiedSince.Value.ToString(@"ddd, dd MMM yyyy HH:mm:ss \G\M\T", Constants.CultureInfo));
            }
            if (request.IfUnmodifiedSince.HasValue)
            {
                CommonUtil.AddHeader(request2, "If-Unmodified-Since", request.IfUnmodifiedSince.Value.ToString(@"ddd, dd MMM yyyy HH:mm:ss \G\M\T", Constants.CultureInfo));
            }
            if (((request.ByteRange != null) && (request.ByteRange.Start >= 0L)) && (request.ByteRange.Start <= request.ByteRange.End))
            {
                CommonUtil.AddHeader(request2, "Range", string.Format("bytes={0}-{1}", request.ByteRange.Start, request.ByteRange.End));
            }
            return request2;
        }

        public HttpRequest Trans(HeadBucketRequest request)
        {
            return new HttpRequest { Method = HttpVerb.HEAD, BucketName = request.BucketName };
        }

        public HttpRequest Trans(InitiateMultipartUploadRequest request)
        {
            HttpRequest httpRequest = new HttpRequest();
            this.TransPutObjectBasicRequest(request, httpRequest);
            httpRequest.Method = HttpVerb.POST;
            httpRequest.Params.Add(EnumAdaptor.GetStringValue(SubResourceEnum.Uploads), null);
            if (string.IsNullOrEmpty(request.ContentType))
            {
                string key = request.ObjectKey.Substring(request.ObjectKey.LastIndexOf(".") + 1);
                if (Constants.MimeTypes.ContainsKey(key))
                {
                    request.ContentType = Constants.MimeTypes[key];
                }
            }
            if (!string.IsNullOrEmpty(request.ContentType))
            {
                CommonUtil.AddHeader(httpRequest, "Content-Type", request.ContentType);
            }
            if (request.Expires.HasValue)
            {
                CommonUtil.AddHeader(httpRequest, this.iheaders.ExpiresHeader(), request.Expires.Value.ToString());
            }
            return httpRequest;
        }

        public HttpRequest Trans(ListBucketsRequest request)
        {
            HttpRequest request2 = new HttpRequest {
                Method = HttpVerb.GET
            };
            if (request.IsQueryLocation)
            {
                CommonUtil.AddHeader(request2, this.iheaders.LocationHeader(), "true");
            }
            return request2;
        }

        public HttpRequest Trans(ListMultipartUploadsRequest request)
        {
            HttpRequest request2 = new HttpRequest {
                Method = HttpVerb.GET
            };
            request2.Params.Add(EnumAdaptor.GetStringValue(SubResourceEnum.Uploads), null);
            request2.BucketName = request.BucketName;
            if (request.Prefix != null)
            {
                request2.Params.Add("prefix", request.Prefix);
            }
            if (!string.IsNullOrEmpty(request.Delimiter))
            {
                request2.Params.Add("delimiter", request.Delimiter);
            }
            if (request.KeyMarker != null)
            {
                request2.Params.Add("key-marker", request.KeyMarker);
            }
            if (request.MaxUploads.HasValue)
            {
                request2.Params.Add("max-keys", request.MaxUploads.ToString());
            }
            if (!string.IsNullOrEmpty(request.UploadIdMarker))
            {
                request2.Params.Add("upload-id-marker", request.UploadIdMarker);
            }
            return request2;
        }

        public HttpRequest Trans(ListObjectsRequest request)
        {
            HttpRequest request2 = new HttpRequest {
                Method = HttpVerb.GET,
                BucketName = request.BucketName
            };
            if (request.Prefix != null)
            {
                request2.Params.Add("prefix", request.Prefix);
            }
            if (!string.IsNullOrEmpty(request.Delimiter))
            {
                request2.Params.Add("delimiter", request.Delimiter);
            }
            if (request.Marker != null)
            {
                request2.Params.Add("marker", request.Marker);
            }
            if (request.MaxKeys.HasValue)
            {
                request2.Params.Add("max-keys", request.MaxKeys.ToString());
            }
            return request2;
        }

        public HttpRequest Trans(ListPartsRequest request)
        {
            HttpRequest request2 = new HttpRequest {
                BucketName = request.BucketName,
                ObjectKey = request.ObjectKey,
                Method = HttpVerb.GET
            };
            request2.Params.Add("uploadId", request.UploadId);
            if (request.MaxParts.HasValue)
            {
                request2.Params.Add("max-parts", request.MaxParts.Value.ToString());
            }
            if (request.PartNumberMarker.HasValue)
            {
                request2.Params.Add("part-number-marker", request.PartNumberMarker.Value.ToString());
            }
            return request2;
        }

        public HttpRequest Trans(ListVersionsRequest request)
        {
            HttpRequest request2 = new HttpRequest {
                Method = HttpVerb.GET,
                BucketName = request.BucketName
            };
            request2.Params.Add(EnumAdaptor.GetStringValue(SubResourceEnum.Versions), null);
            if (request.Prefix != null)
            {
                request2.Params.Add("prefix", request.Prefix);
            }
            if (!string.IsNullOrEmpty(request.Delimiter))
            {
                request2.Params.Add("delimiter", request.Delimiter);
            }
            if (request.KeyMarker != null)
            {
                request2.Params.Add("key-marker", request.KeyMarker);
            }
            if (request.MaxKeys.HasValue)
            {
                request2.Params.Add("max-keys", request.MaxKeys.ToString());
            }
            if (!string.IsNullOrEmpty(request.VersionIdMarker))
            {
                request2.Params.Add("version-id-marker", request.VersionIdMarker);
            }
            return request2;
        }

        public HttpRequest Trans(PutObjectRequest request)
        {
            HttpRequest httpRequest = new HttpRequest();
            this.TransPutObjectBasicRequest(request, httpRequest);
            httpRequest.Method = HttpVerb.PUT;
            if (string.IsNullOrEmpty(request.ContentType))
            {
                string key = request.ObjectKey.Substring(request.ObjectKey.LastIndexOf(".") + 1);
                if (Constants.MimeTypes.ContainsKey(key))
                {
                    request.ContentType = Constants.MimeTypes[key];
                }
            }
            if (request.InputStream != null)
            {
                httpRequest.Content = request.InputStream;
                httpRequest.NeedCloseContent = false;//外部传进来的流不需要关闭
            }
            else if (!string.IsNullOrEmpty(request.FilePath))
            {
                if (string.IsNullOrEmpty(request.ContentType))
                {
                    string str2 = request.FilePath.Substring(request.FilePath.LastIndexOf(".") + 1);
                    if (Constants.MimeTypes.ContainsKey(str2))
                    {
                        request.ContentType = Constants.MimeTypes[str2];
                    }
                }
                httpRequest.Content = new FileStream(request.FilePath, FileMode.Open, FileAccess.Read);
                httpRequest.NeedCloseContent = true;//自己建立的流需要关闭
            }
            if (request.ContentLength.HasValue)
            {
                CommonUtil.AddHeader(httpRequest, "Content-Length", request.ContentLength.Value.ToString());
            }
            if (!string.IsNullOrEmpty(request.ContentType))
            {
                CommonUtil.AddHeader(httpRequest, "Content-Type", request.ContentType);
            }
            if (!string.IsNullOrEmpty(request.ContentMd5))
            {
                CommonUtil.AddHeader(httpRequest, "Content-MD5", request.ContentMd5);
            }
            if (request.Expires.HasValue)
            {
                CommonUtil.AddHeader(httpRequest, this.iheaders.ExpiresHeader(), request.Expires.Value.ToString());
            }
            return httpRequest;
        }

        public HttpRequest Trans(RestoreObjectRequest request)
        {
            HttpRequest httpRequest = new HttpRequest {
                BucketName = request.BucketName,
                ObjectKey = request.ObjectKey,
                Method = HttpVerb.POST
            };
            httpRequest.Params.Add(EnumAdaptor.GetStringValue(SubResourceEnum.Restore), null);
            if (!string.IsNullOrEmpty(request.VersionId))
            {
                httpRequest.Params.Add("versionId", request.VersionId);
            }
            this.TransContent(httpRequest, delegate (XmlWriter xmlWriter) {
                xmlWriter.WriteStartElement("RestoreRequest");
                xmlWriter.WriteElementString("Days", request.Days.ToString());
                this.TransTier(request.Tier, xmlWriter);
                xmlWriter.WriteEndElement();
            }, true);
            return httpRequest;
        }

        public HttpRequest Trans(SetBucketAclRequest request)
        {
            HttpRequest request2 = new HttpRequest {
                Method = HttpVerb.PUT
            };
            request2.Params.Add(EnumAdaptor.GetStringValue(SubResourceEnum.Acl), null);
            request2.BucketName = request.BucketName;
            if (request.CannedAcl.HasValue)
            {
                CommonUtil.AddHeader(request2, this.iheaders.AclHeader(), this.TransBucketCannedAcl(request.CannedAcl.Value));
                return request2;
            }
            if (request.AccessControlList != null)
            {
                this.TransAccessControlList(request2, request.AccessControlList, true);
            }
            return request2;
        }

        public HttpRequest Trans(SetBucketCorsRequest request)
        {
            HttpRequest httpRequest = new HttpRequest {
                BucketName = request.BucketName,
                Method = HttpVerb.PUT
            };
            httpRequest.Params.Add(EnumAdaptor.GetStringValue(SubResourceEnum.Cors), null);
            this.TransContent(httpRequest, delegate (XmlWriter xmlWriter) {
                xmlWriter.WriteStartElement("CORSConfiguration");
                if (request.Configuration != null)
                {
                    foreach (CorsRule rule in request.Configuration.Rules)
                    {
                        xmlWriter.WriteStartElement("CORSRule");
                        if (!string.IsNullOrEmpty(rule.Id))
                        {
                            xmlWriter.WriteElementString("ID", rule.Id);
                        }
                        foreach (HttpVerb verb in rule.AllowedMethods)
                        {
                            xmlWriter.WriteElementString("AllowedMethod", verb.ToString());
                        }
                        foreach (string str in rule.AllowedOrigins)
                        {
                            if (!string.IsNullOrEmpty(str))
                            {
                                xmlWriter.WriteElementString("AllowedOrigin", str);
                            }
                        }
                        foreach (string str2 in rule.AllowedHeaders)
                        {
                            if (!string.IsNullOrEmpty(str2))
                            {
                                xmlWriter.WriteElementString("AllowedHeader", str2);
                            }
                        }
                        if (rule.MaxAgeSeconds.HasValue)
                        {
                            xmlWriter.WriteElementString("MaxAgeSeconds", rule.MaxAgeSeconds.Value.ToString());
                        }
                        foreach (string str3 in rule.ExposeHeaders)
                        {
                            if (!string.IsNullOrEmpty(str3))
                            {
                                xmlWriter.WriteElementString("ExposeHeader", str3);
                            }
                        }
                        xmlWriter.WriteEndElement();
                    }
                }
                xmlWriter.WriteEndElement();
            }, true);
            return httpRequest;
        }

        public HttpRequest Trans(SetBucketLifecycleRequest request)
        {
            HttpRequest httpRequest = new HttpRequest {
                BucketName = request.BucketName,
                Method = HttpVerb.PUT
            };
            httpRequest.Params.Add(EnumAdaptor.GetStringValue(SubResourceEnum.Lifecyle), null);
            this.TransContent(httpRequest, delegate (XmlWriter xmlWriter) {
                xmlWriter.WriteStartElement("LifecycleConfiguration");
                if (request.Configuration != null)
                {
                    foreach (LifecycleRule rule in request.Configuration.Rules)
                    {
                        xmlWriter.WriteStartElement("Rule");
                        if (!string.IsNullOrEmpty(rule.Id))
                        {
                            xmlWriter.WriteElementString("ID", rule.Id);
                        }
                        if (rule.Prefix != null)
                        {
                            xmlWriter.WriteElementString("Prefix", rule.Prefix);
                        }
                        xmlWriter.WriteElementString("Status", rule.Status.ToString());
                        if (rule.Expiration != null)
                        {
                            xmlWriter.WriteStartElement("Expiration");
                            if (rule.Expiration.Days.HasValue)
                            {
                                xmlWriter.WriteElementString("Days", rule.Expiration.Days.Value.ToString());
                            }
                            else if (rule.Expiration.Date.HasValue)
                            {
                                xmlWriter.WriteStartElement("Date", rule.Expiration.Date.Value.ToString(@"yyyy-MM-dd\T00:00:00\Z", Constants.CultureInfo));
                            }
                            xmlWriter.WriteEndElement();
                        }
                        if (rule.NoncurrentVersionExpiration != null)
                        {
                            xmlWriter.WriteStartElement("NoncurrentVersionExpiration");
                            xmlWriter.WriteElementString("NoncurrentDays", rule.NoncurrentVersionExpiration.NoncurrentDays.ToString());
                            xmlWriter.WriteEndElement();
                        }
                        foreach (Transition transition in rule.Transitions)
                        {
                            if (transition != null)
                            {
                                xmlWriter.WriteStartElement("Transition");
                                if (transition.Days.HasValue)
                                {
                                    xmlWriter.WriteElementString("Days", transition.Days.Value.ToString());
                                }
                                else if (transition.Date.HasValue)
                                {
                                    xmlWriter.WriteStartElement("Date", transition.Date.Value.ToString(@"yyyy-MM-dd\T00:00:00\Z", Constants.CultureInfo));
                                }
                                if (transition.StorageClass.HasValue)
                                {
                                    xmlWriter.WriteElementString("StorageClass", this.TransStorageClass(transition.StorageClass.Value));
                                }
                                xmlWriter.WriteEndElement();
                            }
                        }
                        foreach (NoncurrentVersionTransition transition2 in rule.NoncurrentVersionTransitions)
                        {
                            if (transition2 != null)
                            {
                                xmlWriter.WriteStartElement("NoncurrentVersionTransition");
                                xmlWriter.WriteElementString("NoncurrentDays", transition2.NoncurrentDays.ToString());
                                if (transition2.StorageClass.HasValue)
                                {
                                    xmlWriter.WriteElementString("StorageClass", this.TransStorageClass(transition2.StorageClass.Value));
                                }
                                xmlWriter.WriteEndElement();
                            }
                        }
                        xmlWriter.WriteEndElement();
                    }
                }
                xmlWriter.WriteEndElement();
            }, true);
            return httpRequest;
        }

        public HttpRequest Trans(SetBucketLoggingRequest request)
        {
            HttpRequest httpRequest = new HttpRequest {
                BucketName = request.BucketName,
                Method = HttpVerb.PUT
            };
            httpRequest.Params.Add(EnumAdaptor.GetStringValue(SubResourceEnum.Logging), null);
            this.TransLoggingConfiguration(httpRequest, request.Configuration);
            return httpRequest;
        }

        public HttpRequest Trans(SetBucketNotificationRequest request)
        {
            HttpRequest httpRequest = new HttpRequest {
                BucketName = request.BucketName,
                Method = HttpVerb.PUT
            };
            httpRequest.Params.Add(EnumAdaptor.GetStringValue(SubResourceEnum.Notification), null);
            this.TransContent(httpRequest, delegate (XmlWriter xmlWriter) {
                xmlWriter.WriteStartElement("NotificationConfiguration");
                if (request.Configuration != null)
                {
                    foreach (TopicConfiguration configuration in request.Configuration.TopicConfigurations)
                    {
                        if (configuration != null)
                        {
                            xmlWriter.WriteStartElement("TopicConfiguration");
                            if (!string.IsNullOrEmpty(configuration.Id))
                            {
                                xmlWriter.WriteElementString("Id", configuration.Id);
                            }
                            if (configuration.FilterRules.Count > 0)
                            {
                                xmlWriter.WriteStartElement("Filter");
                                xmlWriter.WriteStartElement(this.FilterContainerTag);
                                foreach (FilterRule rule in configuration.FilterRules)
                                {
                                    if (rule != null)
                                    {
                                        xmlWriter.WriteStartElement("FilterRule");
                                        if (rule.Name.HasValue)
                                        {
                                            xmlWriter.WriteElementString("Name", rule.Name.Value.ToString().ToLower());
                                        }
                                        if (rule.Value != null)
                                        {
                                            xmlWriter.WriteElementString("Value", rule.Value);
                                        }
                                        xmlWriter.WriteEndElement();
                                    }
                                }
                                xmlWriter.WriteEndElement();
                                xmlWriter.WriteEndElement();
                            }
                            if (!string.IsNullOrEmpty(configuration.Topic))
                            {
                                xmlWriter.WriteElementString("Topic", configuration.Topic);
                            }
                            foreach (EventTypeEnum enum3 in configuration.Events)
                            {
                                xmlWriter.WriteElementString("event", EnumAdaptor.GetStringValue(enum3));
                            }
                            xmlWriter.WriteEndElement();
                        }
                    }
                }
                xmlWriter.WriteEndElement();
            });
            return httpRequest;
        }

        public HttpRequest Trans(SetBucketPolicyRequest request)
        {
            HttpRequest request2 = new HttpRequest {
                BucketName = request.BucketName,
                Method = HttpVerb.PUT
            };
            request2.Params.Add(EnumAdaptor.GetStringValue(SubResourceEnum.Policy), null);
            if (!string.IsNullOrEmpty(request.Policy))
            {
                request2.Content = new MemoryStream(Encoding.UTF8.GetBytes(request.Policy));
                CommonUtil.AddHeader(request2, "Content-Length", request2.Content.Length.ToString());
            }
            if (!string.IsNullOrEmpty(request.ContentMD5))
            {
                CommonUtil.AddHeader(request2, "Content-MD5", request.ContentMD5);
            }
            return request2;
        }

        public HttpRequest Trans(SetBucketQuotaRequest request)
        {
            HttpRequest httpRequest = new HttpRequest {
                Method = HttpVerb.PUT
            };
            httpRequest.Params.Add(EnumAdaptor.GetStringValue(SubResourceEnum.Quota), null);
            httpRequest.BucketName = request.BucketName;
            this.TransContent(httpRequest, delegate (XmlWriter xmlWriter) {
                xmlWriter.WriteStartElement("Quota");
                xmlWriter.WriteElementString("StorageQuota", request.StorageQuota.ToString());
                xmlWriter.WriteEndElement();
            });
            return httpRequest;
        }

        public HttpRequest Trans(SetBucketReplicationRequest request)
        {
            HttpRequest httpRequest = new HttpRequest {
                Method = HttpVerb.PUT,
                BucketName = request.BucketName
            };
            httpRequest.Params.Add(EnumAdaptor.GetStringValue(SubResourceEnum.Replication), null);
            this.TransContent(httpRequest, delegate (XmlWriter xmlWriter) {
                xmlWriter.WriteStartElement("ReplicationConfiguration");
                if (request.Configuration != null)
                {
                    if (!string.IsNullOrEmpty(request.Configuration.Agency))
                    {
                        xmlWriter.WriteElementString("Agency", request.Configuration.Agency);
                    }
                    foreach (ReplicationRule rule in request.Configuration.Rules)
                    {
                        if (rule != null)
                        {
                            xmlWriter.WriteStartElement("Rule");
                            if (!string.IsNullOrEmpty(rule.Id))
                            {
                                xmlWriter.WriteElementString("ID", rule.Id);
                            }
                            if (!string.IsNullOrEmpty(rule.Prefix))
                            {
                                xmlWriter.WriteElementString("Prefix", rule.Prefix);
                            }
                            xmlWriter.WriteElementString("Status", rule.Status.ToString());
                            if (!string.IsNullOrEmpty(rule.TargetBucketName))
                            {
                                xmlWriter.WriteStartElement("Destination");
                                xmlWriter.WriteElementString("Bucket", rule.TargetBucketName);
                                if (rule.TargetStorageClass.HasValue)
                                {
                                    xmlWriter.WriteElementString("StorageClass", this.TransStorageClass(rule.TargetStorageClass.Value));
                                }
                                xmlWriter.WriteEndElement();
                            }
                            xmlWriter.WriteEndElement();
                        }
                    }
                }
                xmlWriter.WriteEndElement();
            }, true);
            return httpRequest;
        }

        public HttpRequest Trans(SetBucketStoragePolicyRequest request)
        {
            HttpRequest httpRequest = new HttpRequest {
                Method = HttpVerb.PUT,
                BucketName = request.BucketName
            };
            httpRequest.Params.Add(this.BucketStoragePolicyParam, null);
            if (request.StorageClass.HasValue)
            {
                this.TransContent(httpRequest, delegate (XmlWriter xmlWriter) {
                    this.TransSetBucketStoragePolicyContent(xmlWriter, request.StorageClass.Value);
                });
            }
            return httpRequest;
        }

        public HttpRequest Trans(SetBucketTaggingRequest request)
        {
            HttpRequest httpRequest = new HttpRequest {
                BucketName = request.BucketName,
                Method = HttpVerb.PUT
            };
            httpRequest.Params.Add(EnumAdaptor.GetStringValue(SubResourceEnum.Tagging), null);
            this.TransContent(httpRequest, delegate (XmlWriter xmlWriter) {
                xmlWriter.WriteStartElement("Tagging");
                if (request.Tags.Count > 0)
                {
                    xmlWriter.WriteStartElement("TagSet");
                    foreach (Tag tag in request.Tags)
                    {
                        if (((tag != null) && !string.IsNullOrEmpty(tag.Key)) && (tag.Value != null))
                        {
                            xmlWriter.WriteStartElement("Tag");
                            xmlWriter.WriteElementString("Key", tag.Key);
                            xmlWriter.WriteElementString("Value", tag.Value);
                            xmlWriter.WriteEndElement();
                        }
                    }
                    xmlWriter.WriteEndElement();
                }
                xmlWriter.WriteEndElement();
            }, true);
            return httpRequest;
        }

        public HttpRequest Trans(SetBucketVersioningRequest request)
        {
            HttpRequest httpRequest = new HttpRequest {
                BucketName = request.BucketName,
                Method = HttpVerb.PUT
            };
            httpRequest.Params.Add(EnumAdaptor.GetStringValue(SubResourceEnum.Versioning), null);
            this.TransContent(httpRequest, delegate (XmlWriter xmlWriter) {
                xmlWriter.WriteStartElement("VersioningConfiguration");
                if ((request.Configuration != null) && request.Configuration.Status.HasValue)
                {
                    xmlWriter.WriteElementString("Status", request.Configuration.Status.Value.ToString());
                }
                xmlWriter.WriteEndElement();
            });
            return httpRequest;
        }

        public HttpRequest Trans(SetBucketWebsiteRequest request)
        {
            HttpRequest httpRequest = new HttpRequest {
                BucketName = request.BucketName,
                Method = HttpVerb.PUT
            };
            httpRequest.Params.Add(EnumAdaptor.GetStringValue(SubResourceEnum.Website), null);
            this.TransContent(httpRequest, delegate (XmlWriter xmlWriter) {
                xmlWriter.WriteStartElement("WebsiteConfiguration");
                if (request.Configuration != null)
                {
                    if (request.Configuration.RedirectAllRequestsTo != null)
                    {
                        xmlWriter.WriteStartElement("RedirectAllRequestsTo");
                        if (!string.IsNullOrEmpty(request.Configuration.RedirectAllRequestsTo.HostName))
                        {
                            xmlWriter.WriteElementString("HostName", request.Configuration.RedirectAllRequestsTo.HostName);
                        }
                        if (request.Configuration.RedirectAllRequestsTo.Protocol.HasValue)
                        {
                            xmlWriter.WriteElementString("Protocol", request.Configuration.RedirectAllRequestsTo.Protocol.Value.ToString().ToLower());
                        }
                        xmlWriter.WriteEndElement();
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(request.Configuration.IndexDocument))
                        {
                            xmlWriter.WriteStartElement("IndexDocument");
                            xmlWriter.WriteElementString("Suffix", request.Configuration.IndexDocument);
                            xmlWriter.WriteEndElement();
                        }
                        if (!string.IsNullOrEmpty(request.Configuration.ErrorDocument))
                        {
                            xmlWriter.WriteStartElement("ErrorDocument");
                            xmlWriter.WriteElementString("Key", request.Configuration.ErrorDocument);
                            xmlWriter.WriteEndElement();
                        }
                        if (request.Configuration.RoutingRules.Count > 0)
                        {
                            xmlWriter.WriteStartElement("RoutingRules");
                            foreach (RoutingRule rule in request.Configuration.RoutingRules)
                            {
                                if (rule != null)
                                {
                                    xmlWriter.WriteStartElement("RoutingRule");
                                    if (rule.Condition != null)
                                    {
                                        xmlWriter.WriteStartElement("Condition");
                                        if (rule.Condition.KeyPrefixEquals != null)
                                        {
                                            xmlWriter.WriteElementString("KeyPrefixEquals", rule.Condition.KeyPrefixEquals);
                                        }
                                        if (!string.IsNullOrEmpty(rule.Condition.HttpErrorCodeReturnedEquals))
                                        {
                                            xmlWriter.WriteElementString("HttpErrorCodeReturnedEquals", rule.Condition.HttpErrorCodeReturnedEquals);
                                        }
                                        xmlWriter.WriteEndElement();
                                    }
                                    if (rule.Redirect != null)
                                    {
                                        xmlWriter.WriteStartElement("Redirect");
                                        if (rule.Redirect.Protocol.HasValue)
                                        {
                                            xmlWriter.WriteElementString("Protocol", rule.Redirect.Protocol.Value.ToString().ToLower());
                                        }
                                        if (!string.IsNullOrEmpty(rule.Redirect.HostName))
                                        {
                                            xmlWriter.WriteElementString("HostName", rule.Redirect.HostName);
                                        }
                                        if (!string.IsNullOrEmpty(rule.Redirect.ReplaceKeyPrefixWith))
                                        {
                                            xmlWriter.WriteElementString("ReplaceKeyPrefixWith", rule.Redirect.ReplaceKeyPrefixWith);
                                        }
                                        if (!string.IsNullOrEmpty(rule.Redirect.ReplaceKeyWith))
                                        {
                                            xmlWriter.WriteElementString("ReplaceKeyWith", rule.Redirect.ReplaceKeyWith);
                                        }
                                        if (!string.IsNullOrEmpty(rule.Redirect.HttpRedirectCode))
                                        {
                                            xmlWriter.WriteElementString("HttpRedirectCode", rule.Redirect.HttpRedirectCode);
                                        }
                                        xmlWriter.WriteEndElement();
                                    }
                                    xmlWriter.WriteEndElement();
                                }
                            }
                            xmlWriter.WriteEndElement();
                        }
                    }
                }
                xmlWriter.WriteEndElement();
            });
            return httpRequest;
        }

        public HttpRequest Trans(SetObjectAclRequest request)
        {
            HttpRequest request2 = new HttpRequest {
                BucketName = request.BucketName,
                ObjectKey = request.ObjectKey,
                Method = HttpVerb.PUT
            };
            request2.Params.Add(EnumAdaptor.GetStringValue(SubResourceEnum.Acl), null);
            if (!string.IsNullOrEmpty(request.VersionId))
            {
                request2.Params.Add("versionId", request.VersionId);
            }
            if (request.CannedAcl.HasValue)
            {
                CommonUtil.AddHeader(request2, this.iheaders.AclHeader(), this.TransObjectCannedAcl(request.CannedAcl.Value));
                return request2;
            }
            if (request.AccessControlList != null)
            {
                this.TransAccessControlList(request2, request.AccessControlList, false);
            }
            return request2;
        }

        public HttpRequest Trans(UploadPartRequest request)
        {
            HttpRequest httpRequest = new HttpRequest {
                Method = HttpVerb.PUT,
                BucketName = request.BucketName,
                ObjectKey = request.ObjectKey
            };
            httpRequest.Params.Add("uploadId", request.UploadId);
            httpRequest.Params.Add("partNumber", request.PartNumber.ToString());
            this.TransSseCHeader(httpRequest, request.SseCHeader);
            if (!string.IsNullOrEmpty(request.ContentMd5))
            {
                CommonUtil.AddHeader(httpRequest, "Content-MD5", request.ContentMd5);
            }
            if (request.InputStream != null)
            {
                long length = request.InputStreamLength;
                if (length <= 0)
                {
                    length = request.InputStream.Length;
                }
                httpRequest.Content = request.InputStream;

                FillRequestInfo(request, httpRequest, length);

                return httpRequest;
            }
            if (!string.IsNullOrEmpty(request.FilePath))
            {
                long length = new FileInfo(request.FilePath).Length;
                httpRequest.Content = new FileStream(request.FilePath, FileMode.Open, FileAccess.Read);
                FillRequestInfo(request, httpRequest, length);
            }
            return httpRequest;
        }
        /// <summary>
        /// 填充请求内容
        /// </summary>
        /// <param name="request">上传请求</param>
        /// <param name="httpRequest">http请求</param>
        /// <param name="totalLength">长度</param>
        private void FillRequestInfo(UploadPartRequest request, HttpRequest httpRequest, long totalLength)
        {
            
            long offset = request.Offset.HasValue ? request.Offset.Value : 0L;
            offset = ((offset >= 0L) && (offset < totalLength)) ? offset : 0L;
            long num4 = request.PartSize.HasValue ? request.PartSize.Value : 0L;
            num4 = ((num4 > 0L) && (num4 <= (totalLength - offset))) ? num4 : (totalLength - offset);
            if (httpRequest.Content.CanSeek)
            {
                httpRequest.Content.Seek(offset, SeekOrigin.Begin);
            }
            CommonUtil.AddHeader(httpRequest, "Content-Length", num4.ToString());
        }

        protected virtual void TransAccessControlList(HttpRequest httpRequest, AccessControlList acl, bool isBucket)
        {
            this.TransContent(httpRequest, delegate (XmlWriter xmlWriter) {
                xmlWriter.WriteStartElement("AccessControlPolicy");
                if ((acl.Owner != null) && !string.IsNullOrEmpty(acl.Owner.Id))
                {
                    xmlWriter.WriteStartElement("Owner");
                    xmlWriter.WriteElementString("ID", acl.Owner.Id);
                    if (!string.IsNullOrEmpty(acl.Owner.DisplayName))
                    {
                        xmlWriter.WriteElementString("DisplayName", acl.Owner.DisplayName);
                    }
                    xmlWriter.WriteEndElement();
                }
                if (acl.Grants.Count > 0)
                {
                    this.TransGrants(xmlWriter, acl.Grants, "AccessControlList");
                }
                xmlWriter.WriteEndElement();
            });
        }

        protected virtual string TransBucketCannedAcl(CannedAclEnum cannedAcl)
        {
            if (cannedAcl == CannedAclEnum.PublicReadDelivered)
            {
                cannedAcl = CannedAclEnum.PublicRead;
            }
            else if (cannedAcl == CannedAclEnum.PublicReadWriteDelivered)
            {
                cannedAcl = CannedAclEnum.PublicReadWrite;
            }
            return EnumAdaptor.GetStringValue(cannedAcl);
        }

        protected void TransContent(HttpRequest httpRequest, TransContentDelegate transContentDelegate)
        {
            this.TransContent(httpRequest, transContentDelegate, false);
        }

        protected void TransContent(HttpRequest httpRequest, TransContentDelegate transContentDelegate, bool md5)
        {
            StringWriter output = new StringWriter();
            XmlWriterSettings settings = new XmlWriterSettings {
                Encoding = Encoding.UTF8,
                OmitXmlDeclaration = true
            };
            using (XmlWriter writer2 = XmlWriter.Create(output, settings))
            {
                transContentDelegate(writer2);
            }
            byte[] bytes = Encoding.UTF8.GetBytes(output.ToString());
            httpRequest.Content = new MemoryStream(bytes);
            if (md5)
            {
                CommonUtil.AddHeader(httpRequest, "Content-MD5", CommonUtil.Base64Md5(bytes));
            }
            CommonUtil.AddHeader(httpRequest, "Content-Length", httpRequest.Content.Length.ToString());
        }

        private void TransGrants(XmlWriter xmlWriter, IList<Grant> grants, string startElementName)
        {
            xmlWriter.WriteStartElement(startElementName);
            foreach (Grant grant in grants)
            {
                if ((grant.Grantee != null) && grant.Permission.HasValue)
                {
                    xmlWriter.WriteStartElement("Grant");
                    xmlWriter.WriteStartElement("Grantee");
                    if (grant.Grantee is GroupGrantee)
                    {
                        GroupGrantee grantee = grant.Grantee as GroupGrantee;
                        if (grantee.GroupGranteeType.HasValue)
                        {
                            xmlWriter.WriteAttributeString("xsi", "type", "http://www.w3.org/2001/XMLSchema-instance", "Group");
                            xmlWriter.WriteElementString("URI", EnumAdaptor.GetStringValue((Enum) grantee.GroupGranteeType));
                        }
                    }
                    else if (grant.Grantee is CanonicalGrantee)
                    {
                        xmlWriter.WriteAttributeString("xsi", "type", "http://www.w3.org/2001/XMLSchema-instance", "CanonicalUser");
                        CanonicalGrantee grantee2 = grant.Grantee as CanonicalGrantee;
                        xmlWriter.WriteElementString("ID", grantee2.Id);
                        if (!string.IsNullOrEmpty(grantee2.DisplayName))
                        {
                            xmlWriter.WriteElementString("DisplayName", grantee2.DisplayName);
                        }
                    }
                    xmlWriter.WriteEndElement();
                    xmlWriter.WriteElementString("Permission", EnumAdaptor.GetStringValue((Enum) grant.Permission));
                    xmlWriter.WriteEndElement();
                }
            }
            xmlWriter.WriteEndElement();
        }

        protected virtual void TransLoggingConfiguration(HttpRequest httpRequest, LoggingConfiguration configuration)
        {
            this.TransContent(httpRequest, delegate (XmlWriter xmlWriter) {
                xmlWriter.WriteStartElement("BucketLoggingStatus");
                if ((configuration != null) && (!string.IsNullOrEmpty(configuration.TargetBucketName) || (configuration.TargetPrefix != null)))
                {
                    xmlWriter.WriteStartElement("LoggingEnabled");
                    if (!string.IsNullOrEmpty(configuration.TargetBucketName))
                    {
                        xmlWriter.WriteElementString("TargetBucket", configuration.TargetBucketName);
                    }
                    if (configuration.TargetPrefix != null)
                    {
                        xmlWriter.WriteElementString("TargetPrefix", configuration.TargetPrefix);
                    }
                    if (configuration.Grants.Count > 0)
                    {
                        this.TransGrants(xmlWriter, configuration.Grants, "TargetGrants");
                    }
                    xmlWriter.WriteEndElement();
                }
                xmlWriter.WriteEndElement();
            });
        }

        protected virtual string TransObjectCannedAcl(CannedAclEnum cannedAcl)
        {
            return this.TransBucketCannedAcl(cannedAcl);
        }

        protected void TransPutObjectBasicRequest(PutObjectBasicRequest request, HttpRequest httpRequest)
        {
            httpRequest.BucketName = request.BucketName;
            httpRequest.ObjectKey = request.ObjectKey;
            if (!string.IsNullOrEmpty(request.WebsiteRedirectLocation))
            {
                CommonUtil.AddHeader(httpRequest, this.iheaders.WebsiteRedirectLocationHeader(), request.WebsiteRedirectLocation);
            }
            if (!string.IsNullOrEmpty(request.SuccessRedirectLocation))
            {
                CommonUtil.AddHeader(httpRequest, this.iheaders.SuccessRedirectLocationHeader(), request.SuccessRedirectLocation);
            }
            if (request.StorageClass.HasValue)
            {
                CommonUtil.AddHeader(httpRequest, this.iheaders.StorageClassHeader(), this.TransStorageClass(request.StorageClass.Value));
            }
            if (request.CannedAcl.HasValue)
            {
                CommonUtil.AddHeader(httpRequest, this.iheaders.AclHeader(), this.TransObjectCannedAcl(request.CannedAcl.Value));
            }
            foreach (KeyValuePair<ExtensionObjectPermissionEnum, IList<string>> pair in request.ExtensionPermissionMap)
            {
                string str = null;
                switch (pair.Key)
                {
                    case ExtensionObjectPermissionEnum.GrantRead:
                        str = this.iheaders.GrantReadHeader();
                        break;

                    case ExtensionObjectPermissionEnum.GrantReadAcp:
                        str = this.iheaders.GrantReadAcpHeader();
                        break;

                    case ExtensionObjectPermissionEnum.GrantWriteAcp:
                        str = this.iheaders.GrantWriteAcpHeader();
                        break;

                    case ExtensionObjectPermissionEnum.GrantFullControl:
                        str = this.iheaders.GrantFullControlHeader();
                        break;
                }
                if ((!string.IsNullOrEmpty(str) && (pair.Value != null)) && (pair.Value.Count > 0))
                {
                    string[] strArray = new string[pair.Value.Count];
                    for (int i = 0; i < pair.Value.Count; i++)
                    {
                        strArray[i] = "id=" + pair.Value[i];
                    }
                    CommonUtil.AddHeader(httpRequest, str, string.Join(",", strArray));
                }
            }
            foreach (KeyValuePair<string, string> pair2 in request.Metadata.KeyValuePairs)
            {
                if (!string.IsNullOrEmpty(pair2.Key))
                {
                    string key = pair2.Key;
                    if (!pair2.Key.StartsWith(this.iheaders.HeaderMetaPrefix(), StringComparison.OrdinalIgnoreCase) && !pair2.Key.StartsWith("x-obs-meta-", StringComparison.OrdinalIgnoreCase))
                    {
                        key = this.iheaders.HeaderMetaPrefix() + key;
                    }
                    CommonUtil.AddHeader(httpRequest, key, pair2.Value);
                }
            }
            if (request.SseHeader != null)
            {
                SseCHeader sseHeader = request.SseHeader as SseCHeader;
                if (sseHeader != null)
                {
                    CommonUtil.AddHeader(httpRequest, this.iheaders.SseCHeader(), this.TransSseCAlgorithmEnum(sseHeader.Algorithm));
                    if (sseHeader.Key != null)
                    {
                        CommonUtil.AddHeader(httpRequest, this.iheaders.SseCKeyHeader(), Convert.ToBase64String(sseHeader.Key));
                        CommonUtil.AddHeader(httpRequest, this.iheaders.SseCKeyMd5Header(), CommonUtil.Base64Md5(sseHeader.Key));
                    }
                    else if (!string.IsNullOrEmpty(sseHeader.KeyBase64))
                    {
                        CommonUtil.AddHeader(httpRequest, this.iheaders.SseCKeyHeader(), sseHeader.KeyBase64);
                        CommonUtil.AddHeader(httpRequest, this.iheaders.SseCKeyMd5Header(), CommonUtil.Base64Md5(Convert.FromBase64String(sseHeader.KeyBase64)));
                    }
                }
                else
                {
                    SseKmsHeader header2 = request.SseHeader as SseKmsHeader;
                    if (header2 != null)
                    {
                        CommonUtil.AddHeader(httpRequest, this.iheaders.SseKmsHeader(), this.TransSseKmsAlgorithm(header2.Algorithm));
                        if (!string.IsNullOrEmpty(header2.Key))
                        {
                            CommonUtil.AddHeader(httpRequest, this.iheaders.SseKmsKeyHeader(), header2.Key);
                        }
                    }
                }
            }
        }

        protected virtual void TransSetBucketStoragePolicyContent(XmlWriter xmlWriter, StorageClassEnum storageClass)
        {
            xmlWriter.WriteStartElement("StoragePolicy");
            xmlWriter.WriteElementString("DefaultStorageClass", this.TransStorageClass(storageClass));
            xmlWriter.WriteEndElement();
        }

        protected void TransSourceSseCHeader(HttpRequest httpRequest, SseCHeader ssec)
        {
            if (ssec != null)
            {
                CommonUtil.AddHeader(httpRequest, this.iheaders.CopySourceSseCHeader(), this.TransSseCAlgorithmEnum(ssec.Algorithm));
                if (ssec.Key != null)
                {
                    CommonUtil.AddHeader(httpRequest, this.iheaders.CopySourceSseCKeyHeader(), Convert.ToBase64String(ssec.Key));
                    CommonUtil.AddHeader(httpRequest, this.iheaders.CopySourceSseCKeyMd5Header(), CommonUtil.Base64Md5(ssec.Key));
                }
                else if (!string.IsNullOrEmpty(ssec.KeyBase64))
                {
                    CommonUtil.AddHeader(httpRequest, this.iheaders.CopySourceSseCKeyHeader(), ssec.KeyBase64);
                    CommonUtil.AddHeader(httpRequest, this.iheaders.CopySourceSseCKeyMd5Header(), CommonUtil.Base64Md5(Convert.FromBase64String(ssec.KeyBase64)));
                }
            }
        }

        protected virtual string TransSseCAlgorithmEnum(SseCAlgorithmEnum algorithm)
        {
            return algorithm.ToString().ToUpper();
        }

        protected void TransSseCHeader(HttpRequest httpRequest, SseCHeader ssec)
        {
            if (ssec != null)
            {
                CommonUtil.AddHeader(httpRequest, this.iheaders.SseCHeader(), this.TransSseCAlgorithmEnum(ssec.Algorithm));
                if (ssec.Key != null)
                {
                    CommonUtil.AddHeader(httpRequest, this.iheaders.SseCKeyHeader(), Convert.ToBase64String(ssec.Key));
                    CommonUtil.AddHeader(httpRequest, this.iheaders.SseCKeyMd5Header(), CommonUtil.Base64Md5(ssec.Key));
                }
                else if (!string.IsNullOrEmpty(ssec.KeyBase64))
                {
                    CommonUtil.AddHeader(httpRequest, this.iheaders.SseCKeyHeader(), ssec.KeyBase64);
                    CommonUtil.AddHeader(httpRequest, this.iheaders.SseCKeyMd5Header(), CommonUtil.Base64Md5(Convert.FromBase64String(ssec.KeyBase64)));
                }
            }
        }

        protected virtual string TransSseKmsAlgorithm(SseKmsAlgorithmEnum algorithm)
        {
            return ("aws:" + EnumAdaptor.GetStringValue(algorithm));
        }

        protected virtual string TransStorageClass(StorageClassEnum storageClass)
        {
            return EnumAdaptor.GetStringValue(storageClass);
        }

        protected virtual void TransTier(RestoreTierEnum? tier, XmlWriter xmlWriter)
        {
            if (tier.HasValue)
            {
                xmlWriter.WriteStartElement("GlacierJobParameters");
                xmlWriter.WriteElementString("Tier", tier.Value.ToString());
                xmlWriter.WriteEndElement();
            }
        }

        protected virtual string BucketLocationTag
        {
            get
            {
                return "LocationConstraint";
            }
        }

        protected virtual string BucketStoragePolicyParam
        {
            get
            {
                return EnumAdaptor.GetStringValue(SubResourceEnum.StoragePolicy);
            }
        }

        protected virtual string FilterContainerTag
        {
            get
            {
                return "S3Key";
            }
        }

        [Serializable, CompilerGenerated]
        private sealed class Traner
        {
            public static readonly V2Convertor.Traner CurTraner = new V2Convertor.Traner();
            public static Comparison<PartETag> Comparison1;

            internal int DoTrans(PartETag part1, PartETag part2)
            {
                return part1.PartNumber.CompareTo(part2.PartNumber);
            }
        }
    }
}

