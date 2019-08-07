namespace OBS.Model
{
    using OBS;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class CreateBucketRequest : ObsBucketWebServiceRequest
    {
        private IDictionary<ExtensionBucketPermissionEnum, IList<string>> extensionPermissionMap;

        internal override string GetAction()
        {
            return "CreateBucket";
        }

        public void GrantExtensionPermission(string domainId, ExtensionBucketPermissionEnum extensionPermissionEnum)
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

        public void WithDrawExtensionPermission(string domainId, ExtensionBucketPermissionEnum extensionPermissionEnum)
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

        public override string BucketName { get; set; }

        public CannedAclEnum? CannedAcl { get; set; }

        internal IDictionary<ExtensionBucketPermissionEnum, IList<string>> ExtensionPermissionMap
        {
            get
            {
                return (this.extensionPermissionMap ?? (this.extensionPermissionMap = new Dictionary<ExtensionBucketPermissionEnum, IList<string>>()));
            }
        }

        public string Location { get; set; }

        public StorageClassEnum? StorageClass { get; set; }
    }
}

