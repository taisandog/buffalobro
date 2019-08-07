namespace OBS.Internal
{
    using OBS.Model;

    internal interface IParser
    {
        AppendObjectResponse ParseAppendObjectResponse(HttpResponse httpResponse);
        CompleteMultipartUploadResponse ParseCompleteMultipartUploadResponse(HttpResponse httpResponse);
        CopyObjectResponse ParseCopyObjectResponse(HttpResponse httpResponse);
        CopyPartResponse ParseCopyPartResponse(HttpResponse httpResponse);
        DeleteObjectResponse ParseDeleteObjectResponse(HttpResponse httpResponse);
        DeleteObjectsResponse ParseDeleteObjectsResponse(HttpResponse httpResponse);
        GetBucketAclResponse ParseGetBucketAclResponse(HttpResponse httpResponse);
        GetBucketCorsResponse ParseGetBucketCorsResponse(HttpResponse httpResponse);
        GetBucketLifecycleResponse ParseGetBucketLifecycleResponse(HttpResponse httpResponse);
        GetBucketLocationResponse ParseGetBucketLocationResponse(HttpResponse httpResponse);
        GetBucketLoggingResponse ParseGetBucketLoggingResponse(HttpResponse httpResponse);
        GetBucketMetadataResponse ParseGetBucketMetadataResponse(HttpResponse httpResponse);
        GetBucketNotificationReponse ParseGetBucketNotificationReponse(HttpResponse httpResponse);
        GetBucketPolicyResponse ParseGetBucketPolicyResponse(HttpResponse httpResponse);
        GetBucketQuotaResponse ParseGetBucketQuotaResponse(HttpResponse httpResponse);
        GetBucketReplicationResponse ParseGetBucketReplicationResponse(HttpResponse httpResponse);
        GetBucketStorageInfoResponse ParseGetBucketStorageInfoResponse(HttpResponse httpResponse);
        GetBucketStoragePolicyResponse ParseGetBucketStoragePolicyResponse(HttpResponse httpResonse);
        GetBucketTaggingResponse ParseGetBucketTaggingResponse(HttpResponse httpResponse);
        GetBucketVersioningResponse ParseGetBucketVersioningResponse(HttpResponse httpResponse);
        GetBucketWebsiteResponse ParseGetBucketWebsiteResponse(HttpResponse httpResponse);
        GetObjectAclResponse ParseGetObjectAclResponse(HttpResponse httpResponse);
        GetObjectMetadataResponse ParseGetObjectMetadataResponse(HttpResponse httpResponse);
        GetObjectResponse ParseGetObjectResponse(HttpResponse httpResponse);
        InitiateMultipartUploadResponse ParseInitiateMultipartUploadResponse(HttpResponse httpResponse);
        ListBucketsResponse ParseListBucketsResponse(HttpResponse httpResponse);
        ListMultipartUploadsResponse ParseListMultipartUploadsResponse(HttpResponse httpResponse);
        ListObjectsResponse ParseListObjectsResponse(HttpResponse httpResponse);
        ListPartsResponse ParseListPartsResponse(HttpResponse httpResponse);
        ListVersionsResponse ParseListVersionsResponse(HttpResponse httpResponse);
        PutObjectResponse ParsePutObjectResponse(HttpResponse httpResponse);
        UploadPartResponse ParseUploadPartResponse(HttpResponse httpResponse);
    }
}

