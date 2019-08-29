namespace OBS.Model
{
    using System;
    using System.Runtime.CompilerServices;

    public class ObsObjectVersion : ObsObject
    {
        public bool IsDeleteMarker { get; internal set; }

        public bool IsLatest { get; internal set; }

        public string VersionId { get; internal set; }
    }
}

