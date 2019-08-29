namespace OBS.Model
{
    using OBS.Internal;
    using System;

    public enum SubResourceEnum
    {
        [StringValue("acl")]
        Acl = 3,
        [StringValue("append")]
        Append = 11,
        [StringValue("cors")]
        Cors = 12,
        [StringValue("delete")]
        Delete = 15,
        [StringValue("lifecycle")]
        Lifecyle = 6,
        [StringValue("location")]
        Location = 0,
        [StringValue("logging")]
        Logging = 4,
        [StringValue("notification")]
        Notification = 0x12,
        [StringValue("policy")]
        Policy = 5,
        [StringValue("quota")]
        Quota = 2,
        [StringValue("replication")]
        Replication = 0x13,
        [StringValue("restore")]
        Restore = 0x10,
        [StringValue("storageClass")]
        StorageClass = 9,
        [StringValue("storageinfo")]
        StorageInfo = 1,
        [StringValue("storagePolicy"), Obsolete]
        StoragePolicy = 10,
        [StringValue("tagging")]
        Tagging = 0x11,
        [StringValue("uploads")]
        Uploads = 13,
        [StringValue("versioning")]
        Versioning = 8,
        [StringValue("versions")]
        Versions = 14,
        [StringValue("website")]
        Website = 7
    }
}

