namespace OBS.Model
{
    using OBS;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public abstract class PutObjectBasicRequest : ObsBucketWebServiceRequest
    {
        private IDictionary<ExtensionObjectPermissionEnum, IList<string>> extensionPermissionMap;
        private MetadataCollection metadataCollection;

        protected PutObjectBasicRequest()
        {
        }

        public void GrantExtensionPermission(string domainId, ExtensionObjectPermissionEnum extensionPermissionEnum)
        {
            if (!string.IsNullOrEmpty(domainId))
            {
                IList<string> list;
                this.ExtensionPermissionMap.TryGetValue(extensionPermissionEnum, out list);
                if (list == null)
                {
                    list = new List<string>();
                    this.ExtensionPermissionMap.Add(extensionPermissionEnum, list);
                }
                domainId = domainId.Trim();
                if (!list.Contains(domainId))
                {
                    list.Add(domainId);
                }
            }
        }

        public void WithDrawExtensionPermission(string domainId, ExtensionObjectPermissionEnum extensionPermissionEnum)
        {
            if (!string.IsNullOrEmpty(domainId))
            {
                IList<string> list;
                this.ExtensionPermissionMap.TryGetValue(extensionPermissionEnum, out list);
                domainId = domainId.Trim();
                if ((list != null) && list.Contains(domainId))
                {
                    list.Remove(domainId);
                }
            }
        }

        public CannedAclEnum? CannedAcl { get; set; }

        public string ContentType { get; set; }

        internal IDictionary<ExtensionObjectPermissionEnum, IList<string>> ExtensionPermissionMap
        {
            get
            {
                return (this.extensionPermissionMap ?? (this.extensionPermissionMap = new Dictionary<ExtensionObjectPermissionEnum, IList<string>>()));
            }
        }

        public MetadataCollection Metadata
        {
            get
            {
                return (this.metadataCollection ?? (this.metadataCollection = new MetadataCollection()));
            }
            internal set
            {
                this.metadataCollection = value;
            }
        }

        public string ObjectKey { get; set; }

        public OBS.Model.SseHeader SseHeader { get; set; }

        public StorageClassEnum? StorageClass { get; set; }

        public string SuccessRedirectLocation { get; set; }

        public string WebsiteRedirectLocation { get; set; }
    }
}

