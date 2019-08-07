namespace OBS
{
    using System;

    public enum AuthTypeEnum
    {
        OBS = 2,
        V2 = 0,
        [Obsolete]
        V4 = 1
    }
}

