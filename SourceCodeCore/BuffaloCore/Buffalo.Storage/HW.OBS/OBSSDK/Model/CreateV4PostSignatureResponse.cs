namespace OBS.Model
{
    using System;
    using System.Runtime.CompilerServices;

    public class CreateV4PostSignatureResponse : CreatePostSignatureResponse
    {
        public string Algorithm { get; internal set; }

        public string Credential { get; internal set; }

        public string Date { get; internal set; }
    }
}

