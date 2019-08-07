namespace OBS.Internal
{
    using OBS.Model;

    internal interface IConvertor
    {
        HttpRequest Trans(AbortMultipartUploadRequest request);
        HttpRequest Trans(AppendObjectRequest request);
        HttpRequest Trans(CompleteMultipartUploadRequest request);
        HttpRequest Trans(CopyObjectRequest request);
        HttpRequest Trans(CopyPartRequest request);
        HttpRequest Trans(CreateBucketRequest request);
        HttpRequest Trans(DeleteBucketCorsRequest request);
        HttpRequest Trans(DeleteBucketLifecycleRequest request);
        HttpRequest Trans(DeleteBucketPolicyRequest request);
        HttpRequest Trans(DeleteBucketReplicationRequest request);
        HttpRequest Trans(DeleteBucketRequest request);
        HttpRequest Trans(DeleteBucketTaggingRequest request);
        HttpRequest Trans(DeleteBucketWebsiteRequest request);
        HttpRequest Trans(DeleteObjectRequest request);
        HttpRequest Trans(DeleteObjectsRequest request);
        HttpRequest Trans(GetBucketAclRequest request);
        HttpRequest Trans(GetBucketCorsRequest request);
        HttpRequest Trans(GetBucketLifecycleRequest request);
        HttpRequest Trans(GetBucketLocationRequest request);
        HttpRequest Trans(GetBucketLoggingRequest request);
        HttpRequest Trans(GetBucketMetadataRequest request);
        HttpRequest Trans(GetBucketNotificationRequest request);
        HttpRequest Trans(GetBucketPolicyRequest request);
        HttpRequest Trans(GetBucketQuotaRequest request);
        HttpRequest Trans(GetBucketReplicationRequest request);
        HttpRequest Trans(GetBucketStorageInfoRequest request);
        HttpRequest Trans(GetBucketStoragePolicyRequest request);
        HttpRequest Trans(GetBucketTaggingRequest request);
        HttpRequest Trans(GetBucketVersioningRequest request);
        HttpRequest Trans(GetBucketWebsiteRequest request);
        HttpRequest Trans(GetObjectAclRequest request);
        HttpRequest Trans(GetObjectMetadataRequest request);
        HttpRequest Trans(GetObjectRequest request);
        HttpRequest Trans(HeadBucketRequest request);
        HttpRequest Trans(InitiateMultipartUploadRequest request);
        HttpRequest Trans(ListBucketsRequest request);
        HttpRequest Trans(ListMultipartUploadsRequest request);
        HttpRequest Trans(ListObjectsRequest request);
        HttpRequest Trans(ListPartsRequest request);
        HttpRequest Trans(ListVersionsRequest request);
        HttpRequest Trans(PutObjectRequest request);
        HttpRequest Trans(RestoreObjectRequest request);
        HttpRequest Trans(SetBucketAclRequest request);
        HttpRequest Trans(SetBucketCorsRequest request);
        HttpRequest Trans(SetBucketLifecycleRequest request);
        HttpRequest Trans(SetBucketLoggingRequest request);
        HttpRequest Trans(SetBucketNotificationRequest request);
        HttpRequest Trans(SetBucketPolicyRequest request);
        HttpRequest Trans(SetBucketQuotaRequest request);
        HttpRequest Trans(SetBucketReplicationRequest request);
        HttpRequest Trans(SetBucketStoragePolicyRequest request);
        HttpRequest Trans(SetBucketTaggingRequest request);
        HttpRequest Trans(SetBucketVersioningRequest request);
        HttpRequest Trans(SetBucketWebsiteRequest request);
        HttpRequest Trans(SetObjectAclRequest request);
        HttpRequest Trans(UploadPartRequest request);
    }
}

