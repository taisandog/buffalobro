namespace OBS.Model
{
    using OBS.Internal;
    using System;

    public enum StorageClassEnum
    {
        [StringValue("GLACIER")]
        Cold = 2,
        [StringValue("STANDARD")]
        Standard = 0,
        [StringValue("STANDARD_IA")]
        Warm = 1
    }
}

