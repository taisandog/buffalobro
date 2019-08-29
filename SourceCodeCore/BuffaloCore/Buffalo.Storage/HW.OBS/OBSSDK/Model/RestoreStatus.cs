namespace OBS.Model
{
    using System;
    using System.Runtime.CompilerServices;

    public class RestoreStatus
    {
        public DateTime? ExpiryDate { get; set; }

        public bool Restored { get; set; }
    }
}

