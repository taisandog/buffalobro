namespace OBS.Model
{
    using System;
    using System.Runtime.CompilerServices;

    public class Owner
    {
        [Obsolete]
        public string DisplayName { get; set; }

        public string Id { get; set; }
    }
}

