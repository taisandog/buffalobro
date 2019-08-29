namespace OBS.Model
{
    using OBS;
    using System;
    using System.IO;

    public class GetObjectResponse : GetObjectMetadataResponse, IDisposable
    {
        private bool _disposed;
        private Stream _outputStream;

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Dispose(bool disposing)
        {
            if (!this._disposed && disposing)
            {
                if (this.OutputStream != null)
                {
                    this.OutputStream.Close();
                    this.OutputStream = null;
                }
                this._disposed = true;
            }
        }

        public void WriteResponseStreamToFile(string filePath)
        {
            this.WriteResponseStreamToFile(filePath, false);
        }

        public void WriteResponseStreamToFile(string filePath, bool append)
        {
            filePath = Path.GetFullPath(filePath);
            Directory.CreateDirectory(new FileInfo(filePath).DirectoryName);
            FileMode create = FileMode.Create;
            if (append && File.Exists(filePath))
            {
                create = FileMode.Append;
            }
            using (Stream stream = new FileStream(filePath, create, FileAccess.Write, FileShare.Read, 0x2000))
            {
                long num = 0L;
                byte[] buffer = new byte[0x2000];
                int count = 0;
                while ((count = this.OutputStream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    stream.Write(buffer, 0, count);
                    num += count;
                }
                if (num != this.ContentLength)
                {
                    throw new ObsException(string.Format("The total bytes read {0} from response stream is not equal to the Content-Length {1}", num, this.ContentLength));
                }
            }
        }

        public Stream OutputStream
        {
            get
            {
                if (this._disposed)
                {
                    throw new ObjectDisposedException(base.GetType().Name);
                }
                return this._outputStream;
            }
            internal set
            {
                this._outputStream = value;
            }
        }
    }
}

