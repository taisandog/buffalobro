namespace OBS.Model
{
    using OBS;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class CreateTemporarySignatureRequest : ObsBucketWebServiceRequest
    {
        private IDictionary<string, string> headers;
        private MetadataCollection metadataCollection;
        private IDictionary<string, string> parameters;

        internal override string GetAction()
        {
            return "CreateTemporarySignature";
        }

        public override string BucketName { get; set; }

        public long? Expires { get; set; }

        public IDictionary<string, string> Headers
        {
            get
            {
                return (this.headers ?? (this.headers = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)));
            }
            internal set
            {
                this.headers = value;
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

        public HttpVerb Method { get; set; }

        public string ObjectKey { get; set; }

        public IDictionary<string, string> Parameters
        {
            get
            {
                return (this.parameters ?? (this.parameters = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)));
            }
            set
            {
                this.parameters = value;
            }
        }

        public SubResourceEnum? SubResource { get; set; }
    }
}

