namespace OBS.Model
{
    using OBS;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class GetBucketMetadataRequest : ObsBucketWebServiceRequest
    {
        private IList<string> accessControlRequestHeaders;

        internal override string GetAction()
        {
            return "GetBucketMetadata";
        }

        public IList<string> AccessControlRequestHeaders
        {
            get
            {
                return (this.accessControlRequestHeaders ?? (this.accessControlRequestHeaders = new List<string>()));
            }
            set
            {
                this.accessControlRequestHeaders = value;
            }
        }

        public string Origin { get; set; }
    }
}

