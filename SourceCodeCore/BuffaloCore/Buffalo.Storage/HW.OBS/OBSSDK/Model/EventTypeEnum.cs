namespace OBS.Model
{
    using OBS.Internal;
    using System;

    public enum EventTypeEnum
    {
        [StringValue("s3:ObjectCreated:*")]
        ObjectCreatedAll = 0,
        [StringValue("s3:ObjectCreated:CompleteMultipartUpload")]
        ObjectCreatedCompleteMultipartUpload = 4,
        [StringValue("s3:ObjectCreated:Copy")]
        ObjectCreatedCopy = 3,
        [StringValue("s3:ObjectCreated:Post")]
        ObjectCreatedPost = 2,
        [StringValue("s3:ObjectCreated:Put")]
        ObjectCreatedPut = 1,
        [StringValue("s3:ObjectRemoved:*")]
        ObjectRemovedAll = 5,
        [StringValue("s3:ObjectRemoved:Delete")]
        ObjectRemovedDelete = 6,
        [StringValue("s3:ObjectRemoved:DeleteMarkerCreated")]
        ObjectRemovedDeleteMarkerCreated = 7
    }
}

