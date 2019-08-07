namespace OBS.Model
{
    using System;
    using System.Runtime.CompilerServices;

    public class ObsObject
    {
        public bool Appendable { get; internal set; }

        public string ETag { get; internal set; }

        public DateTime? LastModified { get; internal set; }

        public string ObjectKey { get; internal set; }

        public OBS.Model.Owner Owner { get; internal set; }

        public long Size { get; internal set; }

        public StorageClassEnum? StorageClass { get; internal set; }
    }
}

