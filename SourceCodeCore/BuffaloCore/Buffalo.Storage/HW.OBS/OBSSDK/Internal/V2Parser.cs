namespace OBS.Internal
{
    using OBS.Model;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Xml;

    internal class V2Parser : IParser
    {
        protected IHeaders iheaders;

        protected V2Parser(IHeaders iheaders)
        {
            this.iheaders = iheaders;
        }

        public static IParser GetInstance(IHeaders iheaders)
        {
            return new V2Parser(iheaders);
        }

        private AccessControlList ParseAccessControlList(HttpResponse httpResponse)
        {
            using (XmlReader reader = XmlReader.Create(httpResponse.Content))
            {
                AccessControlList list = new AccessControlList();
                bool flag = false;
                Grant item = null;
                while (reader.Read())
                {
                    if ("Owner".Equals(reader.Name))
                    {
                        if (reader.IsStartElement())
                        {
                            list.Owner = new Owner();
                        }
                        flag = reader.IsStartElement();
                    }
                    else
                    {
                        if ("ID".Equals(reader.Name))
                        {
                            if (flag)
                            {
                                list.Owner.Id = reader.ReadString();
                            }
                            else
                            {
                                CanonicalGrantee grantee = item.Grantee as CanonicalGrantee;
                                if (grantee != null)
                                {
                                    grantee.Id = reader.ReadString();
                                }
                            }
                            continue;
                        }
                        if ("DisplayName".Equals(reader.Name))
                        {
                            if (flag)
                            {
                                list.Owner.DisplayName = reader.ReadString();
                            }
                            else
                            {
                                CanonicalGrantee grantee2 = item.Grantee as CanonicalGrantee;
                                if (grantee2 != null)
                                {
                                    grantee2.DisplayName = reader.ReadString();
                                }
                            }
                            continue;
                        }
                        if ("Grant".Equals(reader.Name))
                        {
                            if (reader.IsStartElement())
                            {
                                item = new Grant();
                                list.Grants.Add(item);
                            }
                            continue;
                        }
                        if ("Grantee".Equals(reader.Name))
                        {
                            if (reader.IsStartElement())
                            {
                                Grantee grantee3;
                                if (reader.GetAttribute("xsi:type").Equals("CanonicalUser"))
                                {
                                    grantee3 = new CanonicalGrantee();
                                }
                                else
                                {
                                    grantee3 = new GroupGrantee();
                                }
                                item.Grantee = grantee3;
                            }
                            continue;
                        }
                        if ("URI".Equals(reader.Name))
                        {
                            GroupGrantee grantee4 = item.Grantee as GroupGrantee;
                            if (grantee4 != null)
                            {
                                grantee4.GroupGranteeType = this.ParseGroupGrantee(reader.ReadString());
                            }
                        }
                        else if ("Permission".Equals(reader.Name))
                        {
                            item.Permission = this.ParsePermission(reader.ReadString());
                        }
                    }
                }
                return list;
            }
        }

        public AppendObjectResponse ParseAppendObjectResponse(HttpResponse httpResponse)
        {
            AppendObjectResponse response = new AppendObjectResponse();
            if (httpResponse.Headers.ContainsKey(this.iheaders.StorageClassHeader()))
            {
                response.StorageClass = this.ParseStorageClass(httpResponse.Headers[this.iheaders.StorageClassHeader()]);
            }
            if (httpResponse.Headers.ContainsKey("ETag"))
            {
                response.ETag = httpResponse.Headers["ETag"];
            }
            if (httpResponse.Headers.ContainsKey(this.iheaders.NextPositionHeader()))
            {
                response.NextPosition = Convert.ToInt64(httpResponse.Headers[this.iheaders.NextPositionHeader()]);
            }
            return response;
        }

        public CompleteMultipartUploadResponse ParseCompleteMultipartUploadResponse(HttpResponse httpResponse)
        {
            CompleteMultipartUploadResponse response = new CompleteMultipartUploadResponse();
            using (XmlReader reader = XmlReader.Create(httpResponse.Content))
            {
                while (reader.Read())
                {
                    if ("Location".Equals(reader.Name))
                    {
                        response.Location = reader.ReadString();
                    }
                    else
                    {
                        if ("Bucket".Equals(reader.Name))
                        {
                            response.BucketName = reader.ReadString();
                            continue;
                        }
                        if ("Key".Equals(reader.Name))
                        {
                            response.ObjectKey = reader.ReadString();
                            continue;
                        }
                        if ("ETag".Equals(reader.Name))
                        {
                            response.ETag = reader.ReadString();
                        }
                    }
                }
            }
            if (httpResponse.Headers.ContainsKey(this.iheaders.VersionIdHeader()))
            {
                response.VersionId = httpResponse.Headers[this.iheaders.VersionIdHeader()];
            }
            return response;
        }

        public CopyObjectResponse ParseCopyObjectResponse(HttpResponse httpResponse)
        {
            CopyObjectResponse response = new CopyObjectResponse();
            if (httpResponse.Headers.ContainsKey(this.iheaders.VersionIdHeader()))
            {
                response.VersionId = httpResponse.Headers[this.iheaders.VersionIdHeader()];
            }
            if (httpResponse.Headers.ContainsKey(this.iheaders.StorageClassHeader()))
            {
                response.StorageClass = this.ParseStorageClass(httpResponse.Headers[this.iheaders.StorageClassHeader()]);
            }
            if (httpResponse.Headers.ContainsKey(this.iheaders.CopySourceVersionIdHeader()))
            {
                response.SourceVersionId = httpResponse.Headers[this.iheaders.CopySourceVersionIdHeader()];
            }
            using (XmlReader reader = XmlReader.Create(httpResponse.Content))
            {
                while (reader.Read())
                {
                    if ("LastModified".Equals(reader.Name))
                    {
                        response.LastModified = CommonUtil.ParseToDateTime(reader.ReadString());
                    }
                    else if ("ETag".Equals(reader.Name))
                    {
                        response.ETag = reader.ReadString();
                    }
                }
            }
            return response;
        }

        public CopyPartResponse ParseCopyPartResponse(HttpResponse httpResponse)
        {
            CopyPartResponse response = new CopyPartResponse();
            using (XmlReader reader = XmlReader.Create(httpResponse.Content))
            {
                while (reader.Read())
                {
                    if ("LastModified".Equals(reader.Name))
                    {
                        response.LastModified = CommonUtil.ParseToDateTime(reader.ReadString());
                    }
                    else if ("ETag".Equals(reader.Name))
                    {
                        response.ETag = reader.ReadString();
                    }
                }
            }
            return response;
        }

        public DeleteObjectResponse ParseDeleteObjectResponse(HttpResponse httpResponse)
        {
            DeleteObjectResponse response = new DeleteObjectResponse();
            if (httpResponse.Headers.ContainsKey(this.iheaders.VersionIdHeader()))
            {
                response.VersionId = httpResponse.Headers[this.iheaders.VersionIdHeader()];
            }
            if (httpResponse.Headers.ContainsKey(this.iheaders.DeleteMarkerHeader()))
            {
                response.DeleteMarker = Convert.ToBoolean(httpResponse.Headers[this.iheaders.DeleteMarkerHeader()]);
            }
            return response;
        }

        public DeleteObjectsResponse ParseDeleteObjectsResponse(HttpResponse httpResponse)
        {
            DeleteObjectsResponse response = new DeleteObjectsResponse();
            using (XmlReader reader = XmlReader.Create(httpResponse.Content))
            {
                DeletedObject item = null;
                DeleteError error = null;
                bool flag = false;
                bool flag2 = false;
                while (reader.Read())
                {
                    if ("Deleted".Equals(reader.Name))
                    {
                        if (reader.IsStartElement())
                        {
                            item = new DeletedObject();
                            response.DeletedObjects.Add(item);
                        }
                        flag = reader.IsStartElement();
                    }
                    else
                    {
                        if ("Error".Equals(reader.Name))
                        {
                            if (reader.IsStartElement())
                            {
                                error = new DeleteError();
                                response.DeleteErrors.Add(error);
                            }
                            flag2 = reader.IsStartElement();
                            continue;
                        }
                        if ("Key".Equals(reader.Name))
                        {
                            if (flag)
                            {
                                item.ObjectKey = reader.ReadString();
                            }
                            else if (flag2)
                            {
                                error.ObjectKey = reader.ReadString();
                            }
                            continue;
                        }
                        if ("VersionId".Equals(reader.Name))
                        {
                            if (flag)
                            {
                                item.VersionId = reader.ReadString();
                            }
                            else if (flag2)
                            {
                                error.VersionId = reader.ReadString();
                            }
                            continue;
                        }
                        if ("Code".Equals(reader.Name))
                        {
                            error.Code = reader.ReadString();
                        }
                        else
                        {
                            if ("Message".Equals(reader.Name))
                            {
                                error.Message = reader.ReadString();
                                continue;
                            }
                            if ("DeleteMarker".Equals(reader.Name))
                            {
                                item.DeleteMarker = Convert.ToBoolean(reader.ReadString());
                                continue;
                            }
                            if ("DeleteMarkerVersionId".Equals(reader.Name))
                            {
                                item.DeleteMarkerVersionId = reader.ReadString();
                            }
                        }
                    }
                }
            }
            return response;
        }

        protected virtual EventTypeEnum? ParseEventTypeEnum(string value)
        {
            if (!EnumAdaptor.V2EventTypeEnumDict.ContainsKey(value))
            {
                return null;
            }
            return new EventTypeEnum?(EnumAdaptor.V2EventTypeEnumDict[value]);
        }

        protected FilterNameEnum? ParseFilterName(string value)
        {
            if (!EnumAdaptor.FilterNameEnumDict.ContainsKey(value))
            {
                return null;
            }
            return new FilterNameEnum?(EnumAdaptor.FilterNameEnumDict[value]);
        }

        public virtual GetBucketAclResponse ParseGetBucketAclResponse(HttpResponse httpResponse)
        {
            return new GetBucketAclResponse { AccessControlList = this.ParseAccessControlList(httpResponse) };
        }

        public GetBucketCorsResponse ParseGetBucketCorsResponse(HttpResponse httpResponse)
        {
            GetBucketCorsResponse response = new GetBucketCorsResponse();
            using (XmlReader reader = XmlReader.Create(httpResponse.Content))
            {
                CorsRule item = null;
                while (reader.Read())
                {
                    if ("CORSConfiguration".Equals(reader.Name))
                    {
                        if (reader.IsStartElement())
                        {
                            response.Configuration = new CorsConfiguration();
                        }
                    }
                    else
                    {
                        if ("CORSRule".Equals(reader.Name))
                        {
                            if (reader.IsStartElement())
                            {
                                item = new CorsRule();
                                response.Configuration.Rules.Add(item);
                            }
                            continue;
                        }
                        if ("AllowedMethod".Equals(reader.Name))
                        {
                            HttpVerb? nullable = this.ParseHttpVerb(reader.ReadString());
                            if (nullable.HasValue)
                            {
                                item.AllowedMethods.Add(nullable.Value);
                            }
                            continue;
                        }
                        if ("AllowedOrigin".Equals(reader.Name))
                        {
                            item.AllowedOrigins.Add(reader.ReadString());
                            continue;
                        }
                        if ("AllowedHeader".Equals(reader.Name))
                        {
                            item.AllowedHeaders.Add(reader.ReadString());
                            continue;
                        }
                        if ("MaxAgeSeconds".Equals(reader.Name))
                        {
                            item.MaxAgeSeconds = CommonUtil.ParseToInt32(reader.ReadString());
                            continue;
                        }
                        if ("ExposeHeader".Equals(reader.Name))
                        {
                            item.ExposeHeaders.Add(reader.ReadString());
                        }
                    }
                }
            }
            return response;
        }

        public GetBucketLifecycleResponse ParseGetBucketLifecycleResponse(HttpResponse httpResponse)
        {
            GetBucketLifecycleResponse response = new GetBucketLifecycleResponse();
            using (XmlReader reader = XmlReader.Create(httpResponse.Content))
            {
                LifecycleRule item = null;
                bool flag = false;
                bool flag2 = false;
                bool flag3 = false;
                bool flag4 = false;
                Transition transition = null;
                NoncurrentVersionTransition transition2 = null;
                while (reader.Read())
                {
                    if ("LifecycleConfiguration".Equals(reader.Name))
                    {
                        if (reader.IsStartElement())
                        {
                            response.Configuration = new LifecycleConfiguration();
                        }
                    }
                    else
                    {
                        if ("Rule".Equals(reader.Name))
                        {
                            if (reader.IsStartElement())
                            {
                                item = new LifecycleRule();
                                response.Configuration.Rules.Add(item);
                            }
                            continue;
                        }
                        if ("ID".Equals(reader.Name))
                        {
                            item.Id = reader.ReadString();
                            continue;
                        }
                        if ("Prefix".Equals(reader.Name))
                        {
                            item.Prefix = reader.ReadString();
                            continue;
                        }
                        if ("Status".Equals(reader.Name))
                        {
                            RuleStatusEnum? nullable = this.ParseRuleStatus(reader.ReadString());
                            if (nullable.HasValue)
                            {
                                item.Status = nullable.Value;
                            }
                            continue;
                        }
                        if ("Expiration".Equals(reader.Name))
                        {
                            if (reader.IsStartElement())
                            {
                                item.Expiration = new Expiration();
                            }
                            flag = reader.IsStartElement();
                            continue;
                        }
                        if ("NoncurrentVersionExpiration".Equals(reader.Name))
                        {
                            if (reader.IsStartElement())
                            {
                                item.NoncurrentVersionExpiration = new NoncurrentVersionExpiration();
                            }
                            flag2 = reader.IsStartElement();
                            continue;
                        }
                        if ("Transition".Equals(reader.Name))
                        {
                            if (reader.IsStartElement())
                            {
                                transition = new Transition();
                                item.Transitions.Add(transition);
                            }
                            flag3 = reader.IsStartElement();
                            continue;
                        }
                        if ("NoncurrentVersionTransition".Equals(reader.Name))
                        {
                            if (reader.IsStartElement())
                            {
                                transition2 = new NoncurrentVersionTransition();
                                item.NoncurrentVersionTransitions.Add(transition2);
                            }
                            flag4 = reader.IsStartElement();
                            continue;
                        }
                        if ("Days".Equals(reader.Name))
                        {
                            if (flag)
                            {
                                item.Expiration.Days = CommonUtil.ParseToInt32(reader.ReadString());
                            }
                            else if (flag3)
                            {
                                transition.Days = CommonUtil.ParseToInt32(reader.ReadString());
                            }
                            continue;
                        }
                        if ("Date".Equals(reader.Name))
                        {
                            if (flag)
                            {
                                item.Expiration.Date = CommonUtil.ParseToDateTime(reader.ReadString());
                            }
                            else if (flag3)
                            {
                                transition.Date = CommonUtil.ParseToDateTime(reader.ReadString());
                            }
                            continue;
                        }
                        if ("NoncurrentDays".Equals(reader.Name))
                        {
                            if (flag2)
                            {
                                item.NoncurrentVersionExpiration.NoncurrentDays = Convert.ToInt32(reader.ReadString());
                            }
                            else if (flag4)
                            {
                                transition2.NoncurrentDays = Convert.ToInt32(reader.ReadString());
                            }
                            continue;
                        }
                        if ("StorageClass".Equals(reader.Name))
                        {
                            if (flag3)
                            {
                                transition.StorageClass = this.ParseStorageClass(reader.ReadString());
                                continue;
                            }
                            if (flag4)
                            {
                                transition2.StorageClass = this.ParseStorageClass(reader.ReadString());
                            }
                        }
                    }
                }
            }
            return response;
        }

        public GetBucketLocationResponse ParseGetBucketLocationResponse(HttpResponse httpResponse)
        {
            GetBucketLocationResponse response = new GetBucketLocationResponse();
            using (XmlReader reader = XmlReader.Create(httpResponse.Content))
            {
                while (reader.Read())
                {
                    if (reader.Name.Equals(this.BucketLocationTag))
                    {
                        response.Location = reader.ReadString();
                    }
                }
            }
            return response;
        }

        public virtual GetBucketLoggingResponse ParseGetBucketLoggingResponse(HttpResponse httpResponse)
        {
            GetBucketLoggingResponse response = new GetBucketLoggingResponse();
            using (XmlReader reader = XmlReader.Create(httpResponse.Content))
            {
                Grant item = null;
                while (reader.Read())
                {
                    if ("BucketLoggingStatus".Equals(reader.Name))
                    {
                        if (reader.IsStartElement())
                        {
                            response.Configuration = new LoggingConfiguration();
                        }
                    }
                    else
                    {
                        if ("TargetBucket".Equals(reader.Name))
                        {
                            response.Configuration.TargetBucketName = reader.ReadString();
                            continue;
                        }
                        if ("TargetPrefix".Equals(reader.Name))
                        {
                            response.Configuration.TargetPrefix = reader.ReadString();
                            continue;
                        }
                        if ("Grant".Equals(reader.Name))
                        {
                            if (reader.IsStartElement())
                            {
                                item = new Grant();
                                response.Configuration.Grants.Add(item);
                            }
                            continue;
                        }
                        if ("Grantee".Equals(reader.Name))
                        {
                            if (reader.IsStartElement())
                            {
                                Grantee grantee;
                                if (reader.GetAttribute("xsi:type").Equals("CanonicalUser"))
                                {
                                    grantee = new CanonicalGrantee();
                                }
                                else
                                {
                                    grantee = new GroupGrantee();
                                }
                                item.Grantee = grantee;
                            }
                            continue;
                        }
                        if ("ID".Equals(reader.Name))
                        {
                            CanonicalGrantee grantee2 = item.Grantee as CanonicalGrantee;
                            if (grantee2 != null)
                            {
                                grantee2.Id = reader.ReadString();
                            }
                        }
                        else
                        {
                            if ("DisplayName".Equals(reader.Name))
                            {
                                CanonicalGrantee grantee3 = item.Grantee as CanonicalGrantee;
                                if (grantee3 != null)
                                {
                                    grantee3.DisplayName = reader.ReadString();
                                }
                                continue;
                            }
                            if ("URI".Equals(reader.Name))
                            {
                                GroupGrantee grantee4 = item.Grantee as GroupGrantee;
                                if (grantee4 != null)
                                {
                                    grantee4.GroupGranteeType = this.ParseGroupGrantee(reader.ReadString());
                                }
                                continue;
                            }
                            if ("Permission".Equals(reader.Name))
                            {
                                item.Permission = this.ParsePermission(reader.ReadString());
                            }
                        }
                    }
                }
            }
            return response;
        }

        public GetBucketMetadataResponse ParseGetBucketMetadataResponse(HttpResponse httpResponse)
        {
            string str;
            GetBucketMetadataResponse response = new GetBucketMetadataResponse();
            httpResponse.Headers.TryGetValue(this.iheaders.DefaultStorageClassHeader(), out str);
            response.StorageClass = this.ParseStorageClass(str);
            if (httpResponse.Headers.ContainsKey(this.iheaders.ServerVersionHeader()))
            {
                response.ObsVersion = httpResponse.Headers[this.iheaders.ServerVersionHeader()];
            }
            if (httpResponse.Headers.ContainsKey(this.iheaders.BucketRegionHeader()))
            {
                response.Location = httpResponse.Headers[this.iheaders.BucketRegionHeader()];
            }
            return response;
        }

        public GetBucketNotificationReponse ParseGetBucketNotificationReponse(HttpResponse httpResponse)
        {
            GetBucketNotificationReponse reponse = new GetBucketNotificationReponse();
            using (XmlReader reader = XmlReader.Create(httpResponse.Content))
            {
                TopicConfiguration item = null;
                bool flag = false;
                FilterRule rule = null;
                while (reader.Read())
                {
                    if ("NotificationConfiguration".Equals(reader.Name))
                    {
                        if (reader.IsStartElement())
                        {
                            reponse.Configuration = new NotificationConfiguration();
                        }
                    }
                    else
                    {
                        if ("TopicConfiguration".Equals(reader.Name))
                        {
                            if (reader.IsStartElement())
                            {
                                item = new TopicConfiguration();
                                reponse.Configuration.TopicConfigurations.Add(item);
                            }
                            flag = reader.IsStartElement();
                            continue;
                        }
                        if ("Id".Equals(reader.Name))
                        {
                            if (flag)
                            {
                                item.Id = reader.ReadString();
                            }
                        }
                        else
                        {
                            if ("FilterRule".Equals(reader.Name))
                            {
                                if (reader.IsStartElement() && flag)
                                {
                                    rule = new FilterRule();
                                    item.FilterRules.Add(rule);
                                }
                                continue;
                            }
                            if ("Name".Equals(reader.Name))
                            {
                                rule.Name = this.ParseFilterName(reader.ReadString());
                                continue;
                            }
                            if ("Name".Equals(reader.Value))
                            {
                                rule.Value = reader.ReadString();
                                continue;
                            }
                            if ("Topic".Equals(reader.Name))
                            {
                                item.Topic = reader.ReadString();
                                continue;
                            }
                            if ("Event".Equals(reader.Name) && flag)
                            {
                                EventTypeEnum? nullable = this.ParseEventTypeEnum(reader.ReadString());
                                if (nullable.HasValue)
                                {
                                    item.Events.Add(nullable.Value);
                                }
                            }
                        }
                    }
                }
            }
            return reponse;
        }

        public GetBucketPolicyResponse ParseGetBucketPolicyResponse(HttpResponse httpResponse)
        {
            GetBucketPolicyResponse response = new GetBucketPolicyResponse();
            using (StreamReader reader = new StreamReader(httpResponse.Content, Encoding.UTF8))
            {
                response.Policy = reader.ReadToEnd();
            }
            return response;
        }

        public GetBucketQuotaResponse ParseGetBucketQuotaResponse(HttpResponse httpResponse)
        {
            GetBucketQuotaResponse response = new GetBucketQuotaResponse();
            using (XmlReader reader = XmlReader.Create(httpResponse.Content))
            {
                while (reader.Read())
                {
                    if ("StorageQuota".Equals(reader.Name))
                    {
                        response.StorageQuota = Convert.ToInt64(reader.ReadString());
                    }
                }
            }
            return response;
        }

        public GetBucketReplicationResponse ParseGetBucketReplicationResponse(HttpResponse httpResponse)
        {
            GetBucketReplicationResponse response = new GetBucketReplicationResponse();
            using (XmlReader reader = XmlReader.Create(httpResponse.Content))
            {
                ReplicationRule item = null;
                while (reader.Read())
                {
                    if ("ReplicationConfiguration".Equals(reader.Name))
                    {
                        if (reader.IsStartElement())
                        {
                            response.Configuration = new ReplicationConfiguration();
                        }
                    }
                    else
                    {
                        if ("Agency".Equals(reader.Name))
                        {
                            response.Configuration.Agency = reader.ReadString();
                            continue;
                        }
                        if ("Rule".Equals(reader.Name))
                        {
                            if (reader.IsStartElement())
                            {
                                item = new ReplicationRule();
                                response.Configuration.Rules.Add(item);
                            }
                            continue;
                        }
                        if ("ID".Equals(reader.Name))
                        {
                            item.Id = reader.ReadString();
                            continue;
                        }
                        if ("Prefix".Equals(reader.Name))
                        {
                            item.Prefix = reader.ReadString();
                            continue;
                        }
                        if ("Status".Equals(reader.Name))
                        {
                            RuleStatusEnum? nullable = this.ParseRuleStatus(reader.ReadString());
                            if (nullable.HasValue)
                            {
                                item.Status = nullable.Value;
                            }
                            continue;
                        }
                        if ("Bucket".Equals(reader.Name))
                        {
                            item.TargetBucketName = reader.ReadString();
                            continue;
                        }
                        if ("StorageClass".Equals(reader.Name))
                        {
                            item.TargetStorageClass = this.ParseStorageClass(reader.ReadString());
                        }
                    }
                }
            }
            return response;
        }

        public GetBucketStorageInfoResponse ParseGetBucketStorageInfoResponse(HttpResponse httpResponse)
        {
            GetBucketStorageInfoResponse response = new GetBucketStorageInfoResponse();
            using (XmlReader reader = XmlReader.Create(httpResponse.Content))
            {
                while (reader.Read())
                {
                    if ("Size".Equals(reader.Name))
                    {
                        response.Size = Convert.ToInt64(reader.ReadString());
                    }
                    else if ("ObjectNumber".Equals(reader.Name))
                    {
                        response.ObjectNumber = Convert.ToInt64(reader.ReadString());
                    }
                }
            }
            return response;
        }

        public GetBucketStoragePolicyResponse ParseGetBucketStoragePolicyResponse(HttpResponse httpResponse)
        {
            GetBucketStoragePolicyResponse response = new GetBucketStoragePolicyResponse();
            using (XmlReader reader = XmlReader.Create(httpResponse.Content))
            {
                while (reader.Read())
                {
                    if (reader.Name.Equals(this.BucketStorageClassTag))
                    {
                        response.StorageClass = this.ParseStorageClass(reader.ReadString());
                    }
                }
            }
            return response;
        }

        public GetBucketTaggingResponse ParseGetBucketTaggingResponse(HttpResponse httpResponse)
        {
            GetBucketTaggingResponse response = new GetBucketTaggingResponse();
            using (XmlReader reader = XmlReader.Create(httpResponse.Content))
            {
                Tag item = null;
                while (reader.Read())
                {
                    if ("Tag".Equals(reader.Name))
                    {
                        if (reader.IsStartElement())
                        {
                            item = new Tag();
                            response.Tags.Add(item);
                        }
                    }
                    else
                    {
                        if ("Key".Equals(reader.Name))
                        {
                            item.Key = reader.ReadString();
                            continue;
                        }
                        if ("Value".Equals(reader.Name))
                        {
                            item.Value = reader.ReadString();
                        }
                    }
                }
            }
            return response;
        }

        public GetBucketVersioningResponse ParseGetBucketVersioningResponse(HttpResponse httpResponse)
        {
            GetBucketVersioningResponse response = new GetBucketVersioningResponse();
            using (XmlReader reader = XmlReader.Create(httpResponse.Content))
            {
                while (reader.Read())
                {
                    if ("VersioningConfiguration".Equals(reader.Name))
                    {
                        if (reader.IsStartElement())
                        {
                            response.Configuration = new VersioningConfiguration();
                        }
                    }
                    else if ("Status".Equals(reader.Name))
                    {
                        response.Configuration.Status = this.ParseVersionStatusEnum(reader.ReadString());
                    }
                }
            }
            return response;
        }

        public GetBucketWebsiteResponse ParseGetBucketWebsiteResponse(HttpResponse httpResponse)
        {
            GetBucketWebsiteResponse response = new GetBucketWebsiteResponse();
            using (XmlReader reader = XmlReader.Create(httpResponse.Content))
            {
                bool flag = false;
                RoutingRule item = null;
                while (reader.Read())
                {
                    if ("WebsiteConfiguration".Equals(reader.Name))
                    {
                        if (reader.IsStartElement())
                        {
                            response.Configuration = new WebsiteConfiguration();
                        }
                    }
                    else
                    {
                        if ("RedirectAllRequestsTo".Equals(reader.Name))
                        {
                            if (reader.IsStartElement())
                            {
                                response.Configuration.RedirectAllRequestsTo = new RedirectBasic();
                            }
                            flag = reader.IsStartElement();
                            continue;
                        }
                        if ("HostName".Equals(reader.Name))
                        {
                            if (flag)
                            {
                                response.Configuration.RedirectAllRequestsTo.HostName = reader.ReadString();
                            }
                            else
                            {
                                item.Redirect.HostName = reader.ReadString();
                            }
                            continue;
                        }
                        if ("Protocol".Equals(reader.Name))
                        {
                            if (flag)
                            {
                                response.Configuration.RedirectAllRequestsTo.Protocol = this.ParseProtocol(reader.ReadString());
                            }
                            else
                            {
                                item.Redirect.Protocol = this.ParseProtocol(reader.ReadString());
                            }
                            continue;
                        }
                        if ("Suffix".Equals(reader.Name))
                        {
                            response.Configuration.IndexDocument = reader.ReadString();
                        }
                        else
                        {
                            if ("Key".Equals(reader.Name))
                            {
                                response.Configuration.ErrorDocument = reader.ReadString();
                                continue;
                            }
                            if ("RoutingRule".Equals(reader.Name))
                            {
                                if (reader.IsStartElement())
                                {
                                    item = new RoutingRule();
                                    response.Configuration.RoutingRules.Add(item);
                                }
                                continue;
                            }
                            if ("Condition".Equals(reader.Name))
                            {
                                if (reader.IsStartElement())
                                {
                                    item.Condition = new Condition();
                                }
                                continue;
                            }
                            if ("KeyPrefixEquals".Equals(reader.Name))
                            {
                                item.Condition.KeyPrefixEquals = reader.ReadString();
                                continue;
                            }
                            if ("HttpErrorCodeReturnedEquals".Equals(reader.Name))
                            {
                                item.Condition.HttpErrorCodeReturnedEquals = reader.ReadString();
                                continue;
                            }
                            if ("Redirect".Equals(reader.Name))
                            {
                                if (reader.IsStartElement())
                                {
                                    item.Redirect = new Redirect();
                                }
                                continue;
                            }
                            if ("ReplaceKeyPrefixWith".Equals(reader.Name))
                            {
                                item.Redirect.ReplaceKeyPrefixWith = reader.ReadString();
                                continue;
                            }
                            if ("ReplaceKeyWith".Equals(reader.Name))
                            {
                                item.Redirect.ReplaceKeyWith = reader.ReadString();
                                continue;
                            }
                            if ("HttpRedirectCode".Equals(reader.Name))
                            {
                                item.Redirect.HttpRedirectCode = reader.ReadString();
                            }
                        }
                    }
                }
            }
            return response;
        }

        public virtual GetObjectAclResponse ParseGetObjectAclResponse(HttpResponse httpResponse)
        {
            return new GetObjectAclResponse { AccessControlList = this.ParseAccessControlList(httpResponse) };
        }

        public GetObjectMetadataResponse ParseGetObjectMetadataResponse(HttpResponse httpResponse)
        {
            GetObjectMetadataResponse response = new GetObjectMetadataResponse();
            this.ParseGetObjectMetadataResponse(httpResponse, response);
            return response;
        }

        protected void ParseGetObjectMetadataResponse(HttpResponse httpResponse, GetObjectMetadataResponse response)
        {
            if (httpResponse.Headers.ContainsKey("ETag"))
            {
                response.ETag = httpResponse.Headers["ETag"];
            }
            if (httpResponse.Headers.ContainsKey("Content-Length"))
            {
                response.ContentLength = Convert.ToInt64(httpResponse.Headers["Content-Length"]);
            }
            if (httpResponse.Headers.ContainsKey("Content-Type"))
            {
                response.ContentType = httpResponse.Headers["Content-Type"];
            }
            if (httpResponse.Headers.ContainsKey(this.iheaders.VersionIdHeader()))
            {
                response.VersionId = httpResponse.Headers[this.iheaders.VersionIdHeader()];
            }
            if (httpResponse.Headers.ContainsKey(this.iheaders.WebsiteRedirectLocationHeader()))
            {
                response.WebsiteRedirectLocation = httpResponse.Headers[this.iheaders.WebsiteRedirectLocationHeader()];
            }
            if (httpResponse.Headers.ContainsKey("Last-Modified"))
            {
                response.LastModified = CommonUtil.ParseToDateTime(httpResponse.Headers["Last-Modified"], @"ddd, dd MMM yyyy HH:mm:ss \G\M\T", @"yyyy-MM-dd\THH:mm:ss.fff\Z");
            }
            if (httpResponse.Headers.ContainsKey(this.iheaders.StorageClassHeader()))
            {
                response.StorageClass = this.ParseStorageClass(httpResponse.Headers[this.iheaders.StorageClassHeader()]);
            }
            if (httpResponse.Headers.ContainsKey(this.iheaders.DeleteMarkerHeader()))
            {
                response.DeleteMarker = Convert.ToBoolean(httpResponse.Headers[this.iheaders.DeleteMarkerHeader()]);
            }
            foreach (KeyValuePair<string, string> pair in httpResponse.Headers)
            {
                if ((pair.Key != null) && pair.Key.StartsWith(this.iheaders.HeaderMetaPrefix()))
                {
                    string name = pair.Key.Substring(this.iheaders.HeaderMetaPrefix().Length);
                    response.Metadata.Add(name, pair.Value);
                }
            }
            if (httpResponse.Headers.ContainsKey(this.iheaders.RestoreHeader()))
            {
                response.RestoreStatus = new RestoreStatus();
                string input = httpResponse.Headers[this.iheaders.RestoreHeader()];
                if (input.Contains("expiry-date"))
                {
                    Match match = Regex.Match(input, "ongoing-request=\"(?<ongoing>.+)\",\\s*expiry-date=\"(?<date>.+)\"");
                    if (match.Success)
                    {
                        response.RestoreStatus.Restored = !Convert.ToBoolean(match.Groups["ongoing"].Value);
                        response.RestoreStatus.ExpiryDate = CommonUtil.ParseToDateTime(match.Groups["date"].Value, @"ddd, dd MMM yyyy HH:mm:ss \G\M\T", @"yyyy-MM-dd\THH:mm:ss.fff\Z");
                    }
                }
                else
                {
                    Match match2 = Regex.Match(input, "ongoing-request=\"(?<ongoing>.+)\"");
                    if (match2.Success)
                    {
                        response.RestoreStatus.Restored = !Convert.ToBoolean(match2.Groups["ongoing"].Value);
                    }
                }
            }
            if (httpResponse.Headers.ContainsKey(this.iheaders.ExpirationHeader()))
            {
                Match match3 = Regex.Match(httpResponse.Headers[this.iheaders.ExpirationHeader()], "expiry-date=\"(?<date>.+)\",\\s*rule-id=\"(?<id>.+)\"");
                if (match3.Success)
                {
                    response.ExpirationDetail = new ExpirationDetail();
                    response.ExpirationDetail.RuleId = match3.Groups["id"].Value;
                    response.ExpirationDetail.ExpiryDate = CommonUtil.ParseToDateTime(match3.Groups["date"].Value, @"ddd, dd MMM yyyy HH:mm:ss \G\M\T", @"yyyy-MM-dd\THH:mm:ss.fff\Z");
                }
            }
            if (httpResponse.Headers.ContainsKey(this.iheaders.ObjectTypeHeader()))
            {
                response.Appendable = "Appendable".Equals(httpResponse.Headers[this.iheaders.ObjectTypeHeader()]);
            }
            if (httpResponse.Headers.ContainsKey(this.iheaders.NextPositionHeader()))
            {
                response.NextPosition = Convert.ToInt64(httpResponse.Headers[this.iheaders.NextPositionHeader()]);
            }
        }

        public GetObjectResponse ParseGetObjectResponse(HttpResponse httpResponse)
        {
            GetObjectResponse response = new GetObjectResponse();
            this.ParseGetObjectMetadataResponse(httpResponse, response);
            response.OutputStream = httpResponse.Content;
            return response;
        }

        protected virtual GroupGranteeEnum? ParseGroupGrantee(string value)
        {
            if (!EnumAdaptor.V2GroupGranteeEnumDict.ContainsKey(value))
            {
                return null;
            }
            return new GroupGranteeEnum?(EnumAdaptor.V2GroupGranteeEnumDict[value]);
        }

        protected HttpVerb? ParseHttpVerb(string value)
        {
            if (!EnumAdaptor.HttpVerbEnumDict.ContainsKey(value))
            {
                return null;
            }
            return new HttpVerb?(EnumAdaptor.HttpVerbEnumDict[value]);
        }

        public InitiateMultipartUploadResponse ParseInitiateMultipartUploadResponse(HttpResponse httpResponse)
        {
            InitiateMultipartUploadResponse response = new InitiateMultipartUploadResponse();
            using (XmlReader reader = XmlReader.Create(httpResponse.Content))
            {
                while (reader.Read())
                {
                    if ("Bucket".Equals(reader.Name))
                    {
                        response.BucketName = reader.ReadString();
                    }
                    else
                    {
                        if ("Key".Equals(reader.Name))
                        {
                            response.ObjectKey = reader.ReadString();
                            continue;
                        }
                        if ("UploadId".Equals(reader.Name))
                        {
                            response.UploadId = reader.ReadString();
                        }
                    }
                }
            }
            return response;
        }

        public ListBucketsResponse ParseListBucketsResponse(HttpResponse httpResponse)
        {
            ListBucketsResponse response = new ListBucketsResponse();
            using (XmlReader reader = XmlReader.Create(httpResponse.Content))
            {
                Owner owner = null;
                ObsBucket item = null;
                while (reader.Read())
                {
                    if ("Owner".Equals(reader.Name))
                    {
                        if (reader.IsStartElement())
                        {
                            owner = new Owner();
                            response.Owner = owner;
                        }
                    }
                    else
                    {
                        if ("ID".Equals(reader.Name))
                        {
                            if (owner != null)
                            {
                                owner.Id = reader.ReadString();
                            }
                            continue;
                        }
                        if ("DisplayName".Equals(reader.Name))
                        {
                            if (owner != null)
                            {
                                owner.DisplayName = reader.ReadString();
                            }
                            continue;
                        }
                        if ("Bucket".Equals(reader.Name))
                        {
                            if (reader.IsStartElement())
                            {
                                item = new ObsBucket();
                                response.Buckets.Add(item);
                            }
                            continue;
                        }
                        if ("Name".Equals(reader.Name))
                        {
                            item.BucketName = reader.ReadString();
                            continue;
                        }
                        if ("CreationDate".Equals(reader.Name))
                        {
                            item.CreationDate = CommonUtil.ParseToDateTime(reader.ReadString());
                            continue;
                        }
                        if ("Location".Equals(reader.Name))
                        {
                            item.Location = reader.ReadString();
                        }
                    }
                }
            }
            return response;
        }

        public ListMultipartUploadsResponse ParseListMultipartUploadsResponse(HttpResponse httpResponse)
        {
            ListMultipartUploadsResponse response = new ListMultipartUploadsResponse();
            using (XmlReader reader = XmlReader.Create(httpResponse.Content))
            {
                MultipartUpload item = null;
                bool flag = false;
                bool flag2 = false;
                while (reader.Read())
                {
                    if ("Bucket".Equals(reader.Name))
                    {
                        response.BucketName = reader.ReadString();
                    }
                    else
                    {
                        if ("KeyMarker".Equals(reader.Name))
                        {
                            response.KeyMarker = reader.ReadString();
                            continue;
                        }
                        if ("NextKeyMarker".Equals(reader.Name))
                        {
                            response.NextKeyMarker = reader.ReadString();
                            continue;
                        }
                        if ("UploadIdMarker".Equals(reader.Name))
                        {
                            response.UploadIdMarker = reader.ReadString();
                            continue;
                        }
                        if ("NextUploadIdMarker".Equals(reader.Name))
                        {
                            response.NextUploadIdMarker = reader.ReadString();
                            continue;
                        }
                        if ("MaxUploads".Equals(reader.Name))
                        {
                            response.MaxUploads = CommonUtil.ParseToInt32(reader.ReadString());
                            continue;
                        }
                        if ("Delimiter".Equals(reader.Name))
                        {
                            response.Delimiter = reader.ReadString();
                            continue;
                        }
                        if ("IsTruncated".Equals(reader.Name))
                        {
                            response.IsTruncated = Convert.ToBoolean(reader.ReadString());
                            continue;
                        }
                        if ("Upload".Equals(reader.Name))
                        {
                            if (reader.IsStartElement())
                            {
                                item = new MultipartUpload();
                                response.MultipartUploads.Add(item);
                            }
                            continue;
                        }
                        if ("Key".Equals(reader.Name))
                        {
                            item.ObjectKey = reader.ReadString();
                            continue;
                        }
                        if ("UploadId".Equals(reader.Name))
                        {
                            item.UploadId = reader.ReadString();
                            continue;
                        }
                        if ("Initiated".Equals(reader.Name))
                        {
                            item.Initiated = CommonUtil.ParseToDateTime(reader.ReadString());
                            continue;
                        }
                        if ("Initiator".Equals(reader.Name))
                        {
                            if (reader.IsStartElement())
                            {
                                item.Initiator = new Initiator();
                            }
                            flag2 = reader.IsStartElement();
                            continue;
                        }
                        if ("Owner".Equals(reader.Name))
                        {
                            if (reader.IsStartElement())
                            {
                                item.Owner = new Owner();
                            }
                            flag = reader.IsStartElement();
                            continue;
                        }
                        if ("ID".Equals(reader.Name))
                        {
                            if (flag2)
                            {
                                item.Initiator.Id = reader.ReadString();
                            }
                            else if (flag)
                            {
                                item.Owner.Id = reader.ReadString();
                            }
                            continue;
                        }
                        if ("DisplayName".Equals(reader.Name))
                        {
                            if (flag2)
                            {
                                item.Initiator.DisplayName = reader.ReadString();
                            }
                            else if (flag)
                            {
                                item.Owner.DisplayName = reader.ReadString();
                            }
                            continue;
                        }
                        if ("StorageClass".Equals(reader.Name))
                        {
                            item.StorageClass = this.ParseStorageClass(reader.ReadString());
                        }
                        else if ("Prefix".Equals(reader.Name))
                        {
                            response.CommonPrefixes.Add(reader.ReadString());
                        }
                    }
                }
            }
            return response;
        }

        public ListObjectsResponse ParseListObjectsResponse(HttpResponse httpResponse)
        {
            ListObjectsResponse response = new ListObjectsResponse();
            if (httpResponse.Headers.ContainsKey(this.iheaders.BucketRegionHeader()))
            {
                response.Location = httpResponse.Headers[this.iheaders.BucketRegionHeader()];
            }
            using (XmlReader reader = XmlReader.Create(httpResponse.Content))
            {
                ObsObject item = null;
                bool flag = false;
                while (reader.Read())
                {
                    if ("Name".Equals(reader.Name))
                    {
                        response.BucketName = reader.ReadString();
                    }
                    else
                    {
                        if ("Prefix".Equals(reader.Name))
                        {
                            if (flag)
                            {
                                response.CommonPrefixes.Add(reader.ReadString());
                            }
                            else
                            {
                                response.Prefix = reader.ReadString();
                            }
                            continue;
                        }
                        if ("Marker".Equals(reader.Name))
                        {
                            response.Marker = reader.ReadString();
                        }
                        else
                        {
                            if ("NextMarker".Equals(reader.Name))
                            {
                                response.NextMarker = reader.ReadString();
                                continue;
                            }
                            if ("MaxKeys".Equals(reader.Name))
                            {
                                response.MaxKeys = CommonUtil.ParseToInt32(reader.ReadString());
                                continue;
                            }
                            if ("Delimiter".Equals(reader.Name))
                            {
                                response.Delimiter = reader.ReadString();
                                continue;
                            }
                            if ("IsTruncated".Equals(reader.Name))
                            {
                                response.IsTruncated = Convert.ToBoolean(reader.ReadString());
                                continue;
                            }
                            if ("Contents".Equals(reader.Name))
                            {
                                if (reader.IsStartElement())
                                {
                                    item = new ObsObject();
                                    response.ObsObjects.Add(item);
                                }
                                continue;
                            }
                            if ("Key".Equals(reader.Name))
                            {
                                item.ObjectKey = reader.ReadString();
                                continue;
                            }
                            if ("LastModified".Equals(reader.Name))
                            {
                                item.LastModified = CommonUtil.ParseToDateTime(reader.ReadString());
                                continue;
                            }
                            if ("ETag".Equals(reader.Name))
                            {
                                item.ETag = reader.ReadString();
                                continue;
                            }
                            if ("Size".Equals(reader.Name))
                            {
                                item.Size = Convert.ToInt64(reader.ReadString());
                                continue;
                            }
                            if ("Type".Equals(reader.Name))
                            {
                                item.Appendable = "Appendable".Equals(reader.ReadString());
                                continue;
                            }
                            if ("Owner".Equals(reader.Name))
                            {
                                if (reader.IsStartElement())
                                {
                                    item.Owner = new Owner();
                                }
                                continue;
                            }
                            if ("ID".Equals(reader.Name))
                            {
                                item.Owner.Id = reader.ReadString();
                                continue;
                            }
                            if ("DisplayName".Equals(reader.Name))
                            {
                                item.Owner.DisplayName = reader.ReadString();
                                continue;
                            }
                            if ("StorageClass".Equals(reader.Name))
                            {
                                item.StorageClass = this.ParseStorageClass(reader.ReadString());
                                continue;
                            }
                            if ("CommonPrefixes".Equals(reader.Name))
                            {
                                flag = reader.IsStartElement();
                            }
                        }
                    }
                }
            }
            return response;
        }

        public ListPartsResponse ParseListPartsResponse(HttpResponse httpResponse)
        {
            ListPartsResponse response = new ListPartsResponse();
            using (XmlReader reader = XmlReader.Create(httpResponse.Content))
            {
                PartDetail item = null;
                bool flag = false;
                bool flag2 = false;
                while (reader.Read())
                {
                    if ("Bucket".Equals(reader.Name))
                    {
                        response.BucketName = reader.ReadString();
                    }
                    else
                    {
                        if ("Key".Equals(reader.Name))
                        {
                            response.ObjectKey = reader.ReadString();
                            continue;
                        }
                        if ("UploadId".Equals(reader.Name))
                        {
                            response.UploadId = reader.ReadString();
                            continue;
                        }
                        if ("Initiator".Equals(reader.Name))
                        {
                            if (reader.IsStartElement())
                            {
                                response.Initiator = new Initiator();
                            }
                            flag = reader.IsStartElement();
                            continue;
                        }
                        if ("Owner".Equals(reader.Name))
                        {
                            if (reader.IsStartElement())
                            {
                                response.Owner = new Owner();
                            }
                            flag2 = reader.IsStartElement();
                            continue;
                        }
                        if ("ID".Equals(reader.Name))
                        {
                            if (flag)
                            {
                                response.Initiator.Id = reader.ReadString();
                            }
                            else if (flag2)
                            {
                                response.Owner.Id = reader.ReadString();
                            }
                            continue;
                        }
                        if ("DisplayName".Equals(reader.Name))
                        {
                            if (flag)
                            {
                                response.Initiator.DisplayName = reader.ReadString();
                            }
                            else if (flag2)
                            {
                                response.Owner.DisplayName = reader.ReadString();
                            }
                            continue;
                        }
                        if ("StorageClass".Equals(reader.Name))
                        {
                            response.StorageClass = this.ParseStorageClass(reader.ReadString());
                        }
                        else
                        {
                            if ("PartNumberMarker".Equals(reader.Name))
                            {
                                response.PartNumberMarker = CommonUtil.ParseToInt32(reader.ReadString());
                                continue;
                            }
                            if ("NextPartNumberMarker".Equals(reader.Name))
                            {
                                response.NextPartNumberMarker = CommonUtil.ParseToInt32(reader.ReadString());
                                continue;
                            }
                            if ("MaxParts".Equals(reader.Name))
                            {
                                response.MaxParts = CommonUtil.ParseToInt32(reader.ReadString());
                                continue;
                            }
                            if ("IsTruncated".Equals(reader.Name))
                            {
                                response.IsTruncated = Convert.ToBoolean(reader.ReadString());
                                continue;
                            }
                            if ("Part".Equals(reader.Name))
                            {
                                if (reader.IsStartElement())
                                {
                                    item = new PartDetail();
                                    response.Parts.Add(item);
                                }
                                continue;
                            }
                            if ("PartNumber".Equals(reader.Name))
                            {
                                item.PartNumber = Convert.ToInt32(reader.ReadString());
                                continue;
                            }
                            if ("LastModified".Equals(reader.Name))
                            {
                                item.LastModified = CommonUtil.ParseToDateTime(reader.ReadString());
                                continue;
                            }
                            if ("ETag".Equals(reader.Name))
                            {
                                item.ETag = reader.ReadString();
                                continue;
                            }
                            if ("Size".Equals(reader.Name))
                            {
                                item.Size = Convert.ToInt64(reader.ReadString());
                            }
                        }
                    }
                }
            }
            return response;
        }

        public ListVersionsResponse ParseListVersionsResponse(HttpResponse httpResponse)
        {
            ListVersionsResponse response = new ListVersionsResponse();
            if (httpResponse.Headers.ContainsKey(this.iheaders.BucketRegionHeader()))
            {
                response.Location = httpResponse.Headers[this.iheaders.BucketRegionHeader()];
            }
            using (XmlReader reader = XmlReader.Create(httpResponse.Content))
            {
                ObsObjectVersion item = null;
                bool flag = false;
                while (reader.Read())
                {
                    if ("Name".Equals(reader.Name))
                    {
                        response.BucketName = reader.ReadString();
                    }
                    else
                    {
                        if ("Prefix".Equals(reader.Name))
                        {
                            if (flag)
                            {
                                response.CommonPrefixes.Add(reader.ReadString());
                            }
                            else
                            {
                                response.Prefix = reader.ReadString();
                            }
                            continue;
                        }
                        if ("KeyMarker".Equals(reader.Name))
                        {
                            response.KeyMarker = reader.ReadString();
                        }
                        else
                        {
                            if ("NextKeyMarker".Equals(reader.Name))
                            {
                                response.NextKeyMarker = reader.ReadString();
                                continue;
                            }
                            if ("VersionIdMarker".Equals(reader.Name))
                            {
                                response.VersionIdMarker = reader.ReadString();
                                continue;
                            }
                            if ("NextVersionIdMarker".Equals(reader.Name))
                            {
                                response.NextVersionIdMarker = reader.ReadString();
                                continue;
                            }
                            if ("MaxKeys".Equals(reader.Name))
                            {
                                response.MaxKeys = CommonUtil.ParseToInt32(reader.ReadString());
                                continue;
                            }
                            if ("Delimiter".Equals(reader.Name))
                            {
                                response.Delimiter = reader.ReadString();
                                continue;
                            }
                            if ("IsTruncated".Equals(reader.Name))
                            {
                                response.IsTruncated = Convert.ToBoolean(reader.ReadString());
                                continue;
                            }
                            if ("Version".Equals(reader.Name))
                            {
                                if (reader.IsStartElement())
                                {
                                    item = new ObsObjectVersion();
                                    response.Versions.Add(item);
                                }
                                continue;
                            }
                            if ("DeleteMarker".Equals(reader.Name))
                            {
                                if (reader.IsStartElement())
                                {
                                    item = new ObsObjectVersion {
                                        IsDeleteMarker = true
                                    };
                                    response.Versions.Add(item);
                                }
                                continue;
                            }
                            if ("Key".Equals(reader.Name))
                            {
                                item.ObjectKey = reader.ReadString();
                                continue;
                            }
                            if ("VersionId".Equals(reader.Name))
                            {
                                item.VersionId = reader.ReadString();
                                continue;
                            }
                            if ("LastModified".Equals(reader.Name))
                            {
                                item.LastModified = CommonUtil.ParseToDateTime(reader.ReadString());
                                continue;
                            }
                            if ("IsLatest".Equals(reader.Name))
                            {
                                item.IsLatest = Convert.ToBoolean(reader.ReadString());
                                continue;
                            }
                            if ("ETag".Equals(reader.Name))
                            {
                                item.ETag = reader.ReadString();
                                continue;
                            }
                            if ("Size".Equals(reader.Name))
                            {
                                item.Size = Convert.ToInt64(reader.ReadString());
                                continue;
                            }
                            if ("Type".Equals(reader.Name))
                            {
                                item.Appendable = "Appendable".Equals(reader.ReadString());
                                continue;
                            }
                            if ("Owner".Equals(reader.Name))
                            {
                                if (reader.IsStartElement())
                                {
                                    item.Owner = new Owner();
                                }
                                continue;
                            }
                            if ("ID".Equals(reader.Name))
                            {
                                item.Owner.Id = reader.ReadString();
                                continue;
                            }
                            if ("DisplayName".Equals(reader.Name))
                            {
                                item.Owner.DisplayName = reader.ReadString();
                                continue;
                            }
                            if ("StorageClass".Equals(reader.Name))
                            {
                                item.StorageClass = this.ParseStorageClass(reader.ReadString());
                                continue;
                            }
                            if ("CommonPrefixes".Equals(reader.Name))
                            {
                                flag = reader.IsStartElement();
                            }
                        }
                    }
                }
            }
            return response;
        }

        protected PermissionEnum? ParsePermission(string value)
        {
            if (!EnumAdaptor.PermissionEnumDict.ContainsKey(value))
            {
                return null;
            }
            return new PermissionEnum?(EnumAdaptor.PermissionEnumDict[value]);
        }

        protected ProtocolEnum? ParseProtocol(string value)
        {
            if (!EnumAdaptor.ProtocolEnumDict.ContainsKey(value))
            {
                return null;
            }
            return new ProtocolEnum?(EnumAdaptor.ProtocolEnumDict[value]);
        }

        public PutObjectResponse ParsePutObjectResponse(HttpResponse httpResponse)
        {
            PutObjectResponse response = new PutObjectResponse();
            if (httpResponse.Headers.ContainsKey(this.iheaders.VersionIdHeader()))
            {
                response.VersionId = httpResponse.Headers[this.iheaders.VersionIdHeader()];
            }
            if (httpResponse.Headers.ContainsKey(this.iheaders.StorageClassHeader()))
            {
                response.StorageClass = this.ParseStorageClass(httpResponse.Headers[this.iheaders.StorageClassHeader()]);
            }
            if (httpResponse.Headers.ContainsKey("ETag"))
            {
                response.ETag = httpResponse.Headers["ETag"];
            }
            return response;
        }

        protected RuleStatusEnum? ParseRuleStatus(string value)
        {
            if (!EnumAdaptor.RuleStatusEnumDict.ContainsKey(value))
            {
                return null;
            }
            return new RuleStatusEnum?(EnumAdaptor.RuleStatusEnumDict[value]);
        }

        protected virtual StorageClassEnum? ParseStorageClass(string value)
        {
            if (!EnumAdaptor.V2StorageClassEnumDict.ContainsKey(value))
            {
                return null;
            }
            return new StorageClassEnum?(EnumAdaptor.V2StorageClassEnumDict[value]);
        }

        public UploadPartResponse ParseUploadPartResponse(HttpResponse httpResponse)
        {
            UploadPartResponse response = new UploadPartResponse();
            if (httpResponse.Headers.ContainsKey("ETag"))
            {
                response.ETag = httpResponse.Headers["ETag"];
            }
            return response;
        }

        protected VersionStatusEnum? ParseVersionStatusEnum(string value)
        {
            if (!EnumAdaptor.VersionStatusEnumDict.ContainsKey(value))
            {
                return null;
            }
            return new VersionStatusEnum?(EnumAdaptor.VersionStatusEnumDict[value]);
        }

        protected virtual string BucketLocationTag
        {
            get
            {
                return "LocationConstraint";
            }
        }

        protected virtual string BucketStorageClassTag
        {
            get
            {
                return "DefaultStorageClass";
            }
        }
    }
}

