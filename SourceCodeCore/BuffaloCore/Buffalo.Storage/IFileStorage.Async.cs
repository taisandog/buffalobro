using Buffalo.ArgCommon;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Buffalo.Storage
{
    /// <summary>
    /// 文件存储的异步 API。默认实现用于兼容现有适配器；
    /// 支持真正异步 I/O 的适配器应重写对应方法。
    /// </summary>
    public abstract partial class IFileStorage : IAsyncDisposable
    {
        public virtual Task<APIResault> AppendFileAsync(
            string path,
            Stream content,
            long postion,
            CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return Task.FromResult(AppendFile(path, content, postion));
        }

        public virtual Task<APIResault> AppendFileAsync(
            string path,
            Stream content,
            CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return Task.FromResult(AppendFile(path, content));
        }

        public virtual Task<APIResault> CloseAsync(CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return Task.FromResult(Close());
        }

        public virtual Task<List<string>> GetDirectoriesAsync(
            string path,
            SearchOption searchOption,
            CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return Task.FromResult(GetDirectories(path, searchOption));
        }

        public virtual Task<Stream> GetFileStreamAsync(
            string path,
            CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return Task.FromResult(GetFileStream(path));
        }

        public virtual Task<Stream> GetFileStreamAsync(
            string path,
            long postion,
            long length,
            CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return Task.FromResult(GetFileStream(path, postion, length));
        }

        public virtual Task ReadFileToStreamAsync(
            string path,
            Stream stream,
            long postion,
            long length,
            CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ReadFileToStream(path, stream, postion, length);
            return Task.CompletedTask;
        }

        public virtual Task<FileInfoBase> GetFileInfoAsync(
            string path,
            CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return Task.FromResult(GetFileInfo(path));
        }

        public virtual Task<List<FileInfoBase>> GetFilesAsync(
            string path,
            SearchOption searchOption,
            CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return Task.FromResult(GetFiles(path, searchOption));
        }

        public virtual Task<APIResault> OpenAsync(CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return Task.FromResult(Open());
        }

        public virtual Task<APIResault> RemoveFileAsync(
            string path,
            CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return Task.FromResult(RemoveFile(path));
        }

        public virtual Task<APIResault> RenameFileAsync(
            string source,
            string target,
            CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return Task.FromResult(RenameFile(source, target));
        }

        public virtual Task<APIResault> SaveFileAsync(
            string path,
            Stream content,
            long contentLength,
            CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return Task.FromResult(SaveFile(path, content, contentLength));
        }

        public virtual Task<APIResault> SaveFileAsync(
            string sourcePath,
            string targetPath,
            CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return Task.FromResult(SaveFile(sourcePath, targetPath));
        }

        public virtual Task<APIResault> RemoveDirectoryAsync(
            string path,
            CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return Task.FromResult(RemoveDirectory(path));
        }

        public virtual Task<bool> ExistDirectoryAsync(
            string folder,
            CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return Task.FromResult(ExistDirectory(folder));
        }

        public virtual Task<bool> ExistsFileAsync(
            string path,
            CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return Task.FromResult(ExistsFile(path));
        }

        public virtual Task<APIResault> CreateDirectoryAsync(
            string folder,
            CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return Task.FromResult(CreateDirectory(folder));
        }

        public virtual ValueTask DisposeAsync()
        {
            Dispose();
            return ValueTask.CompletedTask;
        }
    }
}
