namespace OBS.Model
{
    using System;
    using System.Runtime.CompilerServices;

    public class NoncurrentVersionTransition
    {
        public int NoncurrentDays { get; set; }

        public StorageClassEnum? StorageClass { get; set; }
    }
}

