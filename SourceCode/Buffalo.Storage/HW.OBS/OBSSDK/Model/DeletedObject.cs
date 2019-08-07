namespace OBS.Model
{
    using System;
    using System.Runtime.CompilerServices;

    public class DeletedObject
    {
        public bool DeleteMarker { get; internal set; }

        public string DeleteMarkerVersionId { get; internal set; }

        public string ObjectKey { get; internal set; }

        public string VersionId { get; internal set; }
    }
}

