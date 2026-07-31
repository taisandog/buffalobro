using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Amazon.Util;
using Buffalo.ArgCommon;
using Buffalo.Storage.HW.OBS;
using Buffalo.Storage.QCloud.CosApi;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Buffalo.Storage.AWS.S3
{
    public partial class AWSS3Adapter
    {
        public override async Task<APIResault> RemoveDirectoryAsync(
            string path,
            CancellationToken cancellationToken = default)
        {
            path = HWOBSAdapter.GetPath(path);
            ListObjectsRequest request = new ListObjectsRequest
            {
                BucketName = _bucketName,
                Prefix = path,
                MaxKeys = 20
            };

            ListObjectsResponse response;
            do
            {
                response = await _client.ListObjectsAsync(request, cancellationToken).ConfigureAwait(false);
                if (response.S3Objects.Count > 0)
                {
                    DeleteObjectsRequest deleteRequest = new DeleteObjectsRequest
                    {
                        BucketName = _bucketName,
                        Quiet = true
                    };

                    foreach (S3Object entry in response.S3Objects)
                    {
                        deleteRequest.AddKey(entry.Key);
                    }

                    await _client.DeleteObjectsAsync(deleteRequest, cancellationToken).ConfigureAwait(false);
                }

                request.Marker = response.NextMarker;
            }
            while (response.IsTruncated.GetValueOrDefault());

            return ApiCommon.GetSuccess();
        }

        public override async Task<FileInfoBase> GetFileInfoAsync(
            string path,
            CancellationToken cancellationToken = default)
        {
            path = FileInfoBase.FormatKey(path);
            try
            {
                GetObjectMetadataRequest request = new GetObjectMetadataRequest
                {
                    BucketName = _bucketName,
                    Key = path
                };

                GetObjectMetadataResponse response =
                    await _client.GetObjectMetadataAsync(request, cancellationToken).ConfigureAwait(false);
                string url = FileInfoBase.CombineUriToString(_internetUrl, request.Key);
                string accessUrl = FileInfoBase.CombineUriToString(_lanUrl, request.Key);

                return new NetStorageFileInfo(
                    response.LastModified.GetValueOrDefault(),
                    response.LastModified.GetValueOrDefault(),
                    path,
                    url,
                    accessUrl,
                    response.ETag,
                    response.ContentLength);
            }
            catch (OperationCanceledException)
            {
                throw;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public override Task<bool> ExistsFileAsync(
            string path,
            CancellationToken cancellationToken = default)
        {
            return ExistsMetadataAsync(FileInfoBase.FormatKey(path), cancellationToken);
        }

        public override Task<bool> ExistDirectoryAsync(
            string folder,
            CancellationToken cancellationToken = default)
        {
            return ExistsMetadataAsync(HWOBSAdapter.GetPath(folder), cancellationToken);
        }

        public override async Task<List<string>> GetDirectoriesAsync(
            string path,
            SearchOption searchOption,
            CancellationToken cancellationToken = default)
        {
            ListObjectsRequest request = new ListObjectsRequest
            {
                Prefix = HWOBSAdapter.GetPath(path),
                Delimiter = "/",
                BucketName = _bucketName
            };

            ListObjectsResponse response =
                await _client.ListObjectsAsync(request, cancellationToken).ConfigureAwait(false);
            return new List<string>(response.CommonPrefixes);
        }

        public override async Task<Stream> GetFileStreamAsync(
            string path,
            CancellationToken cancellationToken = default)
        {
            GetObjectRequest request = new GetObjectRequest
            {
                BucketName = _bucketName,
                Key = FileInfoBase.FormatKey(path)
            };

            GetObjectResponse response =
                await _client.GetObjectAsync(request, cancellationToken).ConfigureAwait(false);
            return new ResponseOwnedStream(response);
        }

        public override async Task<Stream> GetFileStreamAsync(
            string path,
            long postion,
            long length,
            CancellationToken cancellationToken = default)
        {
            path = FileInfoBase.FormatKey(path);
            FileInfoBase info = await GetFileInfoAsync(path, cancellationToken).ConfigureAwait(false);
            if (info == null)
            {
                return null;
            }

            GetObjectRequest request = new GetObjectRequest
            {
                BucketName = _bucketName,
                Key = path,
                ByteRange = new ByteRange(
                    postion,
                    FileInfoBase.GetRangeEnd(postion, length, info.Length))
            };

            GetObjectResponse response =
                await _client.GetObjectAsync(request, cancellationToken).ConfigureAwait(false);
            return new ResponseOwnedStream(response);
        }

        public override async Task<List<FileInfoBase>> GetFilesAsync(
            string path,
            SearchOption searchOption,
            CancellationToken cancellationToken = default)
        {
            path = HWOBSAdapter.GetPath(path);
            ListObjectsRequest request = new ListObjectsRequest
            {
                BucketName = _bucketName,
                Prefix = path,
                MaxKeys = 50
            };
            if (searchOption == SearchOption.TopDirectoryOnly)
            {
                request.Delimiter = "/";
            }

            List<FileInfoBase> files = new List<FileInfoBase>();
            ListObjectsResponse response;
            do
            {
                response = await _client.ListObjectsAsync(request, cancellationToken).ConfigureAwait(false);
                foreach (S3Object entry in response.S3Objects)
                {
                    if (entry.Key.EndsWith("/", StringComparison.Ordinal))
                    {
                        continue;
                    }

                    files.Add(new NetStorageFileInfo(
                        entry.LastModified.GetValueOrDefault(),
                        entry.LastModified.GetValueOrDefault(),
                        entry.Key,
                        FileInfoBase.CombineUriToString(_internetUrl, entry.Key),
                        FileInfoBase.CombineUriToString(_lanUrl, entry.Key),
                        entry.ETag,
                        entry.Size.GetValueOrDefault()));
                }

                request.Marker = response.NextMarker;
            }
            while (response.IsTruncated.GetValueOrDefault());

            return files;
        }

        public override async Task<APIResault> RemoveFileAsync(
            string path,
            CancellationToken cancellationToken = default)
        {
            DeleteObjectRequest request = new DeleteObjectRequest
            {
                BucketName = _bucketName,
                Key = FileInfoBase.FormatKey(path)
            };

            DeleteObjectResponse response =
                await _client.DeleteObjectAsync(request, cancellationToken).ConfigureAwait(false);
            return response.HttpStatusCode == HttpStatusCode.OK
                ? ApiCommon.GetSuccess()
                : ApiCommon.GetFault(null, response.HttpStatusCode);
        }

        public override async Task<APIResault> RenameFileAsync(
            string source,
            string target,
            CancellationToken cancellationToken = default)
        {
            source = FileInfoBase.FormatKey(source);
            target = FileInfoBase.FormatKey(target);

            Task<GetACLResponse> getAclTask = _client.GetACLAsync(
                new GetACLRequest
                {
                    BucketName = _bucketName,
                    Key = source
                },
                cancellationToken);
            Task<CopyObjectResponse> copyTask = _client.CopyObjectAsync(
                new CopyObjectRequest
                {
                    SourceBucket = _bucketName,
                    DestinationBucket = _bucketName,
                    SourceKey = source,
                    DestinationKey = target
                },
                cancellationToken);

            await Task.WhenAll(getAclTask, copyTask).ConfigureAwait(false);
            CopyObjectResponse copyResponse = await copyTask.ConfigureAwait(false);
            if (copyResponse.HttpStatusCode != HttpStatusCode.OK)
            {
                return ApiCommon.GetFault(null, copyResponse.HttpStatusCode);
            }

            GetACLResponse getAclResponse = await getAclTask.ConfigureAwait(false);
            if (getAclResponse.HttpStatusCode != HttpStatusCode.OK)
            {
                return ApiCommon.GetFault(null, getAclResponse.HttpStatusCode);
            }

            PutACLResponse setAclResponse = await _client.PutACLAsync(
                new PutACLRequest
                {
                    BucketName = _bucketName,
                    Key = target,
                    AccessControlList = getAclResponse.AccessControlList
                },
                cancellationToken).ConfigureAwait(false);
            if (setAclResponse.HttpStatusCode != HttpStatusCode.OK)
            {
                return ApiCommon.GetFault(null, setAclResponse.HttpStatusCode);
            }

            DeleteObjectResponse deleteResponse = await _client.DeleteObjectAsync(
                new DeleteObjectRequest
                {
                    BucketName = _bucketName,
                    Key = source
                },
                cancellationToken).ConfigureAwait(false);

            return deleteResponse.HttpStatusCode == HttpStatusCode.OK
                ? ApiCommon.GetSuccess()
                : ApiCommon.GetFault(null, deleteResponse.HttpStatusCode);
        }

        public override async Task<APIResault> SaveFileAsync(
            string sourcePath,
            string targetPath,
            CancellationToken cancellationToken = default)
        {
            await using FileStream file = new FileStream(
                sourcePath,
                FileMode.Open,
                FileAccess.Read,
                FileShare.Read,
                81920,
                useAsync: true);
            return await SaveFileAsync(
                targetPath,
                file,
                file.Length,
                cancellationToken).ConfigureAwait(false);
        }

        public override Task<APIResault> SaveFileAsync(
            string path,
            Stream content,
            long contentLength,
            CancellationToken cancellationToken = default)
        {
            path = FileInfoBase.FormatKey(path);
            return contentLength < FileInfoBase.SLICE_UPLOAD_FILE_SIZE
                ? SaveFileSingleAsync(path, content, cancellationToken)
                : SaveFileMultipartAsync(path, content, cancellationToken);
        }

        public override async Task<APIResault> CreateDirectoryAsync(
            string folder,
            CancellationToken cancellationToken = default)
        {
            PutObjectResponse response = await _client.PutObjectAsync(
                new PutObjectRequest
                {
                    BucketName = _bucketName,
                    Key = HWOBSAdapter.GetPath(folder)
                },
                cancellationToken).ConfigureAwait(false);

            return response.HttpStatusCode == HttpStatusCode.OK
                ? ApiCommon.GetSuccess()
                : ApiCommon.GetFault(null, response.HttpStatusCode);
        }

        public override async Task ReadFileToStreamAsync(
            string path,
            Stream stream,
            long postion,
            long length,
            CancellationToken cancellationToken = default)
        {
            path = FileInfoBase.FormatKey(path);
            FileInfoBase info = await GetFileInfoAsync(path, cancellationToken).ConfigureAwait(false);
            if (info == null)
            {
                throw new FileNotFoundException(path + " 不存在");
            }

            GetObjectRequest request = new GetObjectRequest
            {
                BucketName = _bucketName,
                Key = path,
                ByteRange = new ByteRange(
                    postion,
                    FileInfoBase.GetRangeEnd(postion, length, info.Length))
            };

            using GetObjectResponse response =
                await _client.GetObjectAsync(request, cancellationToken).ConfigureAwait(false);
            await response.ResponseStream.CopyToAsync(stream, cancellationToken).ConfigureAwait(false);
        }

        private async Task<bool> ExistsMetadataAsync(
            string key,
            CancellationToken cancellationToken)
        {
            key = EncodeKey(key);
            try
            {
                GetObjectMetadataRequest request = new GetObjectMetadataRequest
                {
                    BucketName = _bucketName,
                    Key = key
                };
                ((Amazon.Runtime.Internal.IAmazonWebServiceRequest)request)
                    .AddBeforeRequestHandler(FileIORequestEventHandler);

                await _client.GetObjectMetadataAsync(request, cancellationToken).ConfigureAwait(false);
                return true;
            }
            catch (AmazonS3Exception exception)
                when (string.Equals(exception.ErrorCode, "NoSuchBucket", StringComparison.Ordinal) ||
                      string.Equals(exception.ErrorCode, "NotFound", StringComparison.Ordinal))
            {
                return false;
            }
        }

        private async Task<APIResault> SaveFileSingleAsync(
            string path,
            Stream stream,
            CancellationToken cancellationToken)
        {
            PutObjectRequest request = new PutObjectRequest
            {
                BucketName = _bucketName,
                Key = path,
                InputStream = stream,
                CannedACL = _acl
            };
            if (_needHash)
            {
                request.MD5Digest = GetMD5HashFromStream(stream);
                stream.Position = 0;
            }

            PutObjectResponse response =
                await _client.PutObjectAsync(request, cancellationToken).ConfigureAwait(false);
            return response.HttpStatusCode == HttpStatusCode.OK
                ? ApiCommon.GetSuccess()
                : ApiCommon.GetFault(null, response.HttpStatusCode);
        }

        private async Task<APIResault> SaveFileMultipartAsync(
            string path,
            Stream stream,
            CancellationToken cancellationToken)
        {
            TransferUtilityUploadRequest request = new TransferUtilityUploadRequest
            {
                BucketName = _bucketName,
                Key = path,
                InputStream = stream,
                PartSize = PartSize,
                CannedACL = _acl
            };

            using TransferUtility transferUtility = new TransferUtility(_client);
            await transferUtility.UploadAsync(request, cancellationToken).ConfigureAwait(false);
            return ApiCommon.GetSuccess();
        }

        private sealed class ResponseOwnedStream : Stream
        {
            private GetObjectResponse _response;
            private Stream Inner => _response?.ResponseStream
                ?? throw new ObjectDisposedException(nameof(ResponseOwnedStream));

            public ResponseOwnedStream(GetObjectResponse response)
            {
                _response = response ?? throw new ArgumentNullException(nameof(response));
            }

            public override bool CanRead => Inner.CanRead;
            public override bool CanSeek => Inner.CanSeek;
            public override bool CanWrite => Inner.CanWrite;
            public override long Length => Inner.Length;
            public override long Position
            {
                get => Inner.Position;
                set => Inner.Position = value;
            }

            public override void Flush() => Inner.Flush();
            public override int Read(byte[] buffer, int offset, int count) =>
                Inner.Read(buffer, offset, count);
            public override long Seek(long offset, SeekOrigin origin) =>
                Inner.Seek(offset, origin);
            public override void SetLength(long value) => Inner.SetLength(value);
            public override void Write(byte[] buffer, int offset, int count) =>
                Inner.Write(buffer, offset, count);

            public override Task FlushAsync(CancellationToken cancellationToken) =>
                Inner.FlushAsync(cancellationToken);
            public override Task<int> ReadAsync(
                byte[] buffer,
                int offset,
                int count,
                CancellationToken cancellationToken) =>
                Inner.ReadAsync(buffer, offset, count, cancellationToken);
            public override Task WriteAsync(
                byte[] buffer,
                int offset,
                int count,
                CancellationToken cancellationToken) =>
                Inner.WriteAsync(buffer, offset, count, cancellationToken);

            protected override void Dispose(bool disposing)
            {
                if (disposing)
                {
                    GetObjectResponse response = _response;
                    _response = null;
                    response?.Dispose();
                }

                base.Dispose(disposing);
            }

            public override async ValueTask DisposeAsync()
            {
                GetObjectResponse response = _response;
                _response = null;
                if (response != null)
                {
                    try
                    {
                        await response.ResponseStream.DisposeAsync().ConfigureAwait(false);
                    }
                    finally
                    {
                        response.Dispose();
                    }
                }

                GC.SuppressFinalize(this);
            }
        }
    }
}
