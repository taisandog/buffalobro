namespace OBS.Internal
{
    using System;

    internal interface IHeaders
    {
        string AclHeader();
        string BucketRegionHeader();
        string ContentSha256Header();
        string CopySourceHeader();
        string CopySourceIfMatchHeader();
        string CopySourceIfModifiedSinceHeader();
        string CopySourceIfNoneMatchHeader();
        string CopySourceIfUnmodifiedSinceHeader();
        string CopySourceRangeHeader();
        string CopySourceSseCHeader();
        string CopySourceSseCKeyHeader();
        string CopySourceSseCKeyMd5Header();
        string CopySourceVersionIdHeader();
        string DateHeader();
        string DefaultStorageClassHeader();
        string DeleteMarkerHeader();
        string ExpirationHeader();
        string ExpiresHeader();
        string GrantFullControlDeliveredHeader();
        string GrantFullControlHeader();
        string GrantReadAcpHeader();
        string GrantReadDeliveredHeader();
        string GrantReadHeader();
        string GrantWriteAcpHeader();
        string GrantWriteHeader();
        string HeaderMetaPrefix();
        string HeaderPrefix();
        string LocationHeader();
        string MetadataDirectiveHeader();
        string NextPositionHeader();
        string ObjectTypeHeader();
        string RequestId2Header();
        string RequestIdHeader();
        string RestoreHeader();
        string SecurityTokenHeader();
        string ServerVersionHeader();
        string SseCHeader();
        string SseCKeyHeader();
        string SseCKeyMd5Header();
        string SseKmsHeader();
        string SseKmsKeyHeader();
        string StorageClassHeader();
        string SuccessRedirectLocationHeader();
        string VersionIdHeader();
        string WebsiteRedirectLocationHeader();
    }
}

