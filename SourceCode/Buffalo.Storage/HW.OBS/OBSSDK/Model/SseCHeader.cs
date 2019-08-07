namespace OBS.Model
{
    using System;
    using System.Runtime.CompilerServices;

    public class SseCHeader : SseHeader
    {
        public SseCAlgorithmEnum Algorithm { get; set; }

        public byte[] Key { get; set; }

        public string KeyBase64 { get; set; }
    }
}

