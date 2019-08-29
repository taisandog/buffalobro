namespace OBS.Model
{
    using OBS;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class DeleteObjectsRequest : ObsBucketWebServiceRequest
    {
        private IList<KeyVersion> objects;

        private void AddKey(KeyVersion keyVersion)
        {
            this.Objects.Add(keyVersion);
        }

        public void AddKey(string key)
        {
            KeyVersion keyVersion = new KeyVersion {
                Key = key
            };
            this.AddKey(keyVersion);
        }

        public void AddKey(string key, string versionId)
        {
            KeyVersion keyVersion = new KeyVersion {
                Key = key,
                VersionId = versionId
            };
            this.AddKey(keyVersion);
        }

        internal override string GetAction()
        {
            return "DeleteObjects";
        }

        public IList<KeyVersion> Objects
        {
            get
            {
                return (this.objects ?? (this.objects = new List<KeyVersion>()));
            }
            set
            {
                this.objects = value;
            }
        }

        public bool? Quiet { get; set; }
    }
}

