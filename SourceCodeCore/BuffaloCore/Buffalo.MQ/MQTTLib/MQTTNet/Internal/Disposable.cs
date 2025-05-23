﻿using System;

namespace Buffalo.MQ.MQTTLib.MQTTnet.Internal
{
    public abstract class Disposable : IDisposable
    {
        protected bool IsDisposed { get; private set; } = false;

        protected void ThrowIfDisposed()
        {
            if (IsDisposed)
            {
                throw new ObjectDisposedException(GetType().Name);
            }
        }

        protected virtual void Dispose(bool disposing)
        {
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~Disposable()
        // {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.

            if (IsDisposed)
            {
                return;
            }

            IsDisposed = true;

            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
