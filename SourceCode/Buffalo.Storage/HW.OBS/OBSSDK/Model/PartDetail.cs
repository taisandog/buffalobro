namespace OBS.Model
{
    using System;
    using System.Runtime.CompilerServices;

    public class PartDetail : PartETag
    {
        public DateTime? LastModified { get; internal set; }

        public long Size { get; internal set; }
    }
}

