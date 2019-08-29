namespace OBS.Model
{
    using OBS.Internal;
    using System;

    public enum CannedAclEnum
    {
        [StringValue("authenticated-read"), Obsolete]
        AuthenticatedRead = 5,
        [StringValue("bucket-owner-full-control"), Obsolete]
        BucketOwnerFullControl = 7,
        [StringValue("bucket-owner-read"), Obsolete]
        BucketOwnerRead = 6,
        [StringValue("log-delivery-write"), Obsolete]
        LogDeliveryWrite = 8,
        [StringValue("private")]
        Private = 0,
        [StringValue("public-read")]
        PublicRead = 1,
        [StringValue("public-read-delivered")]
        PublicReadDelivered = 3,
        [StringValue("public-read-write")]
        PublicReadWrite = 2,
        [StringValue("public-read-write-delivered")]
        PublicReadWriteDelivered = 4
    }
}

