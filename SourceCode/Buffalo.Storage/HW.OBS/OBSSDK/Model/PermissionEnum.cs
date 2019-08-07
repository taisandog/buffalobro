namespace OBS.Model
{
    using OBS.Internal;
    using System;

    public enum PermissionEnum
    {
        [StringValue("FULL_CONTROL")]
        FullControl = 4,
        [StringValue("READ")]
        Read = 0,
        [StringValue("READ_ACP")]
        ReadAcp = 2,
        [StringValue("WRITE")]
        Write = 1,
        [StringValue("WRITE_ACP")]
        WriteAcp = 3
    }
}

