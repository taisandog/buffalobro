namespace OBS.Internal
{
    using OBS.Model;
    using System;
    using System.Collections.Generic;
    using System.Xml;

    internal class ObsConvertor : V2Convertor
    {
        protected ObsConvertor(IHeaders iheaders) : base(iheaders)
        {
        }

        public static IConvertor GetInstance(IHeaders iheaders)
        {
            return new ObsConvertor(iheaders);
        }

        protected override void TransAccessControlList(HttpRequest httpRequest, AccessControlList acl, bool isBucket)
        {
            base.TransContent(httpRequest, delegate (XmlWriter xmlWriter) {
                xmlWriter.WriteStartElement("AccessControlPolicy");
                if ((acl.Owner != null) && !string.IsNullOrEmpty(acl.Owner.Id))
                {
                    xmlWriter.WriteStartElement("Owner");
                    xmlWriter.WriteElementString("ID", acl.Owner.Id);
                    xmlWriter.WriteEndElement();
                }
                if (!isBucket)
                {
                    xmlWriter.WriteElementString("Delivered", acl.Delivered.ToString().ToLower());
                }
                if (acl.Grants.Count > 0)
                {
                    this.TransGrants(xmlWriter, acl.Grants, isBucket, "AccessControlList");
                }
                xmlWriter.WriteEndElement();
            });
        }

        protected override string TransBucketCannedAcl(CannedAclEnum cannedAcl)
        {
            return EnumAdaptor.GetStringValue(cannedAcl);
        }

        private void TransGrants(XmlWriter xmlWriter, IList<Grant> grants, bool isBucket, string startElementName)
        {
            xmlWriter.WriteStartElement(startElementName);
            foreach (Grant grant in grants)
            {
                if ((grant.Grantee != null) && grant.Permission.HasValue)
                {
                    xmlWriter.WriteStartElement("Grant");
                    if (grant.Grantee is GroupGrantee)
                    {
                        GroupGranteeEnum? groupGranteeType = (grant.Grantee as GroupGrantee).GroupGranteeType;
                        GroupGranteeEnum allUsers = GroupGranteeEnum.AllUsers;
                        if ((((GroupGranteeEnum) groupGranteeType.GetValueOrDefault()) == allUsers) ? groupGranteeType.HasValue : false)
                        {
                            xmlWriter.WriteStartElement("Grantee");
                            xmlWriter.WriteElementString("Canned", "Everyone");
                            xmlWriter.WriteEndElement();
                        }
                    }
                    else if (grant.Grantee is CanonicalGrantee)
                    {
                        xmlWriter.WriteStartElement("Grantee");
                        CanonicalGrantee grantee = grant.Grantee as CanonicalGrantee;
                        xmlWriter.WriteElementString("ID", grantee.Id);
                        xmlWriter.WriteEndElement();
                    }
                    xmlWriter.WriteElementString("Permission", EnumAdaptor.GetStringValue((Enum) grant.Permission));
                    if (isBucket)
                    {
                        xmlWriter.WriteElementString("Delivered", grant.Delivered.ToString().ToLower());
                    }
                    xmlWriter.WriteEndElement();
                }
            }
            xmlWriter.WriteEndElement();
        }

        protected override void TransLoggingConfiguration(HttpRequest httpRequest, LoggingConfiguration configuration)
        {
            base.TransContent(httpRequest, delegate (XmlWriter xmlWriter) {
                xmlWriter.WriteStartElement("BucketLoggingStatus");
                if (!string.IsNullOrEmpty(configuration.Agency))
                {
                    xmlWriter.WriteElementString("Agency", configuration.Agency);
                }
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
                        this.TransGrants(xmlWriter, configuration.Grants, false, "TargetGrants");
                    }
                    xmlWriter.WriteEndElement();
                }
                xmlWriter.WriteEndElement();
            });
        }

        protected override string TransObjectCannedAcl(CannedAclEnum cannedAcl)
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

        protected override void TransSetBucketStoragePolicyContent(XmlWriter xmlWriter, StorageClassEnum storageClass)
        {
            xmlWriter.WriteElementString("StorageClass", this.TransStorageClass(storageClass));
        }

        protected override string TransSseKmsAlgorithm(SseKmsAlgorithmEnum algorithm)
        {
            return EnumAdaptor.GetStringValue(algorithm);
        }

        protected override string TransStorageClass(StorageClassEnum storageClass)
        {
            return storageClass.ToString().ToUpper();
        }

        protected override void TransTier(RestoreTierEnum? tier, XmlWriter xmlWriter)
        {
            if (tier.HasValue && (((RestoreTierEnum) tier.Value) != RestoreTierEnum.Bulk))
            {
                xmlWriter.WriteStartElement("GlacierJobParameters");
                xmlWriter.WriteElementString("Tier", tier.Value.ToString());
                xmlWriter.WriteEndElement();
            }
        }

        protected override string BucketLocationTag
        {
            get
            {
                return "Location";
            }
        }

        protected override string BucketStoragePolicyParam
        {
            get
            {
                return EnumAdaptor.GetStringValue(SubResourceEnum.StorageClass);
            }
        }

        protected override string FilterContainerTag
        {
            get
            {
                return "Object";
            }
        }
    }
}

