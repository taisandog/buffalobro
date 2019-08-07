namespace OBS.Model
{
    using System;
    using System.Runtime.CompilerServices;

    public class Transition
    {
        public DateTime? Date { get; set; }

        public int? Days { get; set; }

        public StorageClassEnum? StorageClass { get; set; }
    }
}

