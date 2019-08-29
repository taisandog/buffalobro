namespace OBS.Model
{
    using System;
    using System.Runtime.CompilerServices;

    public class RoutingRule
    {
        public OBS.Model.Condition Condition { get; set; }

        public OBS.Model.Redirect Redirect { get; set; }
    }
}

