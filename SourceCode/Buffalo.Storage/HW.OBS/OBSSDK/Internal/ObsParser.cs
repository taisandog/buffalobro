namespace OBS.Internal
{
    using OBS.Model;
    using System;
    using System.Xml;

    internal class ObsParser : V2Parser
    {
        private ObsParser(IHeaders iheaders) : base(iheaders)
        {
        }

        public static IParser GetInstance(IHeaders iheaders)
        {
            return new ObsParser(iheaders);
        }

        private AccessControlList ParseAccessControlList(HttpResponse httpResponse, bool isBucket)
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
                                CanonicalGrantee grantee = new CanonicalGrantee {
                                    Id = reader.ReadString()
                                };
                                item.Grantee = grantee;
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
                        }
                        else
                        {
                            if ("Canned".Equals(reader.Name))
                            {
                                GroupGrantee grantee2 = new GroupGrantee {
                                    GroupGranteeType = this.ParseGroupGrantee(reader.ReadString())
                                };
                                item.Grantee = grantee2;
                                continue;
                            }
                            if ("Permission".Equals(reader.Name))
                            {
                                item.Permission = base.ParsePermission(reader.ReadString());
                                continue;
                            }
                            if ("Delivered".Equals(reader.Name))
                            {
                                if (isBucket)
                                {
                                    item.Delivered = Convert.ToBoolean(reader.ReadString());
                                    continue;
                                }
                                list.Delivered = Convert.ToBoolean(reader.ReadString());
                            }
                        }
                    }
                }
                return list;
            }
        }

        public override GetBucketAclResponse ParseGetBucketAclResponse(HttpResponse httpResponse)
        {
            return new GetBucketAclResponse { AccessControlList = this.ParseAccessControlList(httpResponse, true) };
        }

        public override GetBucketLoggingResponse ParseGetBucketLoggingResponse(HttpResponse httpResponse)
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
                        if ("Agency".Equals(reader.Name))
                        {
                            response.Configuration.Agency = reader.ReadString();
                            continue;
                        }
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
                        if ("ID".Equals(reader.Name))
                        {
                            CanonicalGrantee grantee = new CanonicalGrantee {
                                Id = reader.ReadString()
                            };
                            item.Grantee = grantee;
                            continue;
                        }
                        if ("Canned".Equals(reader.Name))
                        {
                            GroupGrantee grantee2 = new GroupGrantee {
                                GroupGranteeType = this.ParseGroupGrantee(reader.ReadString())
                            };
                            item.Grantee = grantee2;
                            continue;
                        }
                        if ("Permission".Equals(reader.Name))
                        {
                            item.Permission = base.ParsePermission(reader.ReadString());
                        }
                    }
                }
            }
            return response;
        }

        public override GetObjectAclResponse ParseGetObjectAclResponse(HttpResponse httpResponse)
        {
            return new GetObjectAclResponse { AccessControlList = this.ParseAccessControlList(httpResponse, false) };
        }

        protected override GroupGranteeEnum? ParseGroupGrantee(string value)
        {
            if (!EnumAdaptor.ObsGroupGranteeEnumDict.ContainsKey(value))
            {
                return null;
            }
            return new GroupGranteeEnum?(EnumAdaptor.ObsGroupGranteeEnumDict[value]);
        }

        protected override StorageClassEnum? ParseStorageClass(string value)
        {
            if (!EnumAdaptor.ObsStorageClassEnumDict.ContainsKey(value))
            {
                return null;
            }
            return new StorageClassEnum?(EnumAdaptor.ObsStorageClassEnumDict[value]);
        }

        protected override string BucketLocationTag
        {
            get
            {
                return "Location";
            }
        }

        protected override string BucketStorageClassTag
        {
            get
            {
                return "StorageClass";
            }
        }
    }
}

