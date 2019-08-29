namespace OBS.Model
{
    using System;
    using System.Runtime.CompilerServices;

    public class SseKmsHeader : SseHeader
    {
        public SseKmsAlgorithmEnum Algorithm { get; set; }

        public string Key { get; set; }
    }
}

