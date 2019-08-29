namespace OBS.Model
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class CreateTemporarySignatureResponse
    {
        private IDictionary<string, string> actualSignedRequestHeaders;

        public IDictionary<string, string> ActualSignedRequestHeaders
        {
            get
            {
                return (this.actualSignedRequestHeaders ?? (this.actualSignedRequestHeaders = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)));
            }
            internal set
            {
                this.actualSignedRequestHeaders = value;
            }
        }

        public string SignUrl { get; internal set; }
    }
}

