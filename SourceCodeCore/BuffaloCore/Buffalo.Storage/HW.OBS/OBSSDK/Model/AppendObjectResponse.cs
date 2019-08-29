namespace OBS.Model
{
    using OBS;
    using System;
    using System.Runtime.CompilerServices;

    public class AppendObjectResponse : ObsWebServiceResponse
    {
        private long _nextPosition = -1L;

        public string ETag { get; internal set; }

        public long NextPosition
        {
            get
            {
                return this._nextPosition;
            }
            internal set
            {
                this._nextPosition = value;
            }
        }

        public StorageClassEnum? StorageClass { get; internal set; }
    }
}

