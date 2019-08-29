namespace OBS.Model
{
    using OBS;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class CreatePostSignatureRequest : ObsBucketWebServiceRequest
    {
        private IDictionary<string, string> parameters;

        internal override string GetAction()
        {
            return "CreatePostSignature";
        }

        public override string BucketName { get; set; }

        public DateTime? Expires { get; set; }

        public IDictionary<string, string> FormParameters
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

        public string ObjectKey { get; set; }
    }
}

