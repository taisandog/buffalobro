namespace OBS.Model
{
    using System;
    using System.Runtime.CompilerServices;

    public class DeleteError
    {
        public string Code { get; internal set; }

        public string Message { get; internal set; }

        public string ObjectKey { get; internal set; }

        public string VersionId { get; internal set; }
    }
}

