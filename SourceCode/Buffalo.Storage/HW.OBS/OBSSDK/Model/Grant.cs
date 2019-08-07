namespace OBS.Model
{
    using System;
    using System.Runtime.CompilerServices;

    public class Grant
    {
        public bool Delivered { get; set; }

        public OBS.Model.Grantee Grantee { get; set; }

        public PermissionEnum? Permission { get; set; }
    }
}

