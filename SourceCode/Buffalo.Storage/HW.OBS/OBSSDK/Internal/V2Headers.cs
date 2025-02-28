﻿namespace OBS.Internal
{
    using System;

    internal class V2Headers : IHeaders
    {
        private static V2Headers instance = new V2Headers();

        private V2Headers()
        {
        }

        public string AclHeader()
        {
            return (this.HeaderPrefix() + "acl");
        }

        public string BucketRegionHeader()
        {
            return (this.HeaderPrefix() + "bucket-region");
        }

        public string ContentSha256Header()
        {
            return (this.HeaderPrefix() + "content-sha256");
        }

        public string CopySourceHeader()
        {
            return (this.HeaderPrefix() + "copy-source");
        }

        public string CopySourceIfMatchHeader()
        {
            return (this.HeaderPrefix() + "copy-source-if-match");
        }

        public string CopySourceIfModifiedSinceHeader()
        {
            return (this.HeaderPrefix() + "copy-source-if-modified-since");
        }

        public string CopySourceIfNoneMatchHeader()
        {
            return (this.HeaderPrefix() + "copy-source-if-none-match");
        }

        public string CopySourceIfUnmodifiedSinceHeader()
        {
            return (this.HeaderPrefix() + "copy-source-if-unmodified-since");
        }

        public string CopySourceRangeHeader()
        {
            return (this.HeaderPrefix() + "copy-source-range");
        }

        public string CopySourceSseCHeader()
        {
            return (this.HeaderPrefix() + "copy-source-server-side-encryption-customer-algorithm");
        }

        public string CopySourceSseCKeyHeader()
        {
            return (this.HeaderPrefix() + "copy-source-server-side-encryption-customer-key");
        }

        public string CopySourceSseCKeyMd5Header()
        {
            return (this.HeaderPrefix() + "copy-source-server-side-encryption-customer-key-MD5");
        }

        public string CopySourceVersionIdHeader()
        {
            return (this.HeaderPrefix() + "copy-source-version-id");
        }

        public string DateHeader()
        {
            return (this.HeaderPrefix() + "date");
        }

        public string DefaultStorageClassHeader()
        {
            return "x-default-storage-class";
        }

        public string DeleteMarkerHeader()
        {
            return (this.HeaderPrefix() + "delete-marker");
        }

        public string ExpirationHeader()
        {
            return (this.HeaderPrefix() + "expiration");
        }

        public string ExpiresHeader()
        {
            return "x-obs-expires";
        }

        public static IHeaders GetInstance()
        {
            return instance;
        }

        public string GrantFullControlDeliveredHeader()
        {
            return null;
        }

        public string GrantFullControlHeader()
        {
            return (this.HeaderPrefix() + "grant-full-control");
        }

        public string GrantReadAcpHeader()
        {
            return (this.HeaderPrefix() + "grant-read-acp");
        }

        public string GrantReadDeliveredHeader()
        {
            return null;
        }

        public string GrantReadHeader()
        {
            return (this.HeaderPrefix() + "grant-read");
        }

        public string GrantWriteAcpHeader()
        {
            return (this.HeaderPrefix() + "grant-write-acp");
        }

        public string GrantWriteHeader()
        {
            return (this.HeaderPrefix() + "grant-write");
        }

        public string HeaderMetaPrefix()
        {
            return "x-amz-meta-";
        }

        public string HeaderPrefix()
        {
            return "x-amz-";
        }

        public string LocationHeader()
        {
            return (this.HeaderPrefix() + "location");
        }

        public string MetadataDirectiveHeader()
        {
            return (this.HeaderPrefix() + "metadata-directive");
        }

        public string NextPositionHeader()
        {
            return "x-obs-next-append-position";
        }

        public string ObjectTypeHeader()
        {
            return "x-obs-object-type";
        }

        public string RequestId2Header()
        {
            return (this.HeaderPrefix() + "id-2");
        }

        public string RequestIdHeader()
        {
            return (this.HeaderPrefix() + "request-id");
        }

        public string RestoreHeader()
        {
            return (this.HeaderPrefix() + "restore");
        }

        public string SecurityTokenHeader()
        {
            return (this.HeaderPrefix() + "security-token");
        }

        public string ServerVersionHeader()
        {
            return "x-obs-version";
        }

        public string SseCHeader()
        {
            return (this.HeaderPrefix() + "server-side-encryption-customer-algorithm");
        }

        public string SseCKeyHeader()
        {
            return (this.HeaderPrefix() + "server-side-encryption-customer-key");
        }

        public string SseCKeyMd5Header()
        {
            return (this.HeaderPrefix() + "server-side-encryption-customer-key-MD5");
        }

        public string SseKmsHeader()
        {
            return (this.HeaderPrefix() + "server-side-encryption");
        }

        public string SseKmsKeyHeader()
        {
            return (this.HeaderPrefix() + "server-side-encryption-aws-kms-key-id");
        }

        public string StorageClassHeader()
        {
            return (this.HeaderPrefix() + "storage-class");
        }

        public string SuccessRedirectLocationHeader()
        {
            return null;
        }

        public string VersionIdHeader()
        {
            return (this.HeaderPrefix() + "version-id");
        }

        public string WebsiteRedirectLocationHeader()
        {
            return (this.HeaderPrefix() + "website-redirect-location");
        }
    }
}

