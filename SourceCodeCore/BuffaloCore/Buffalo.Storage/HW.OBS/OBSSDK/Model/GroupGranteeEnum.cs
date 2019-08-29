namespace OBS.Model
{
    using OBS.Internal;
    using System;

    public enum GroupGranteeEnum
    {
        [StringValue("http://acs.amazonaws.com/groups/global/AllUsers")]
        AllUsers = 0,
        [StringValue("http://acs.amazonaws.com/groups/global/AuthenticatedUsers"), Obsolete]
        AuthenticatedUsers = 1,
        [StringValue("http://acs.amazonaws.com/groups/s3/LogDelivery"), Obsolete]
        LogDelivery = 2
    }
}

