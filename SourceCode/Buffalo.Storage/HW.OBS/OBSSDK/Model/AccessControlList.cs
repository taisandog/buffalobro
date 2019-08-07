namespace OBS.Model
{
    using System;
    using System.Runtime.CompilerServices;

    public class AccessControlList : AbstractAccessControlList
    {
        public bool Delivered { get; set; }

        public OBS.Model.Owner Owner { get; set; }
    }
}

