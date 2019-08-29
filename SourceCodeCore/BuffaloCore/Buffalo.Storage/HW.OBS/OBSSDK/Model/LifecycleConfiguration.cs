namespace OBS.Model
{
    using System;
    using System.Collections.Generic;

    public class LifecycleConfiguration
    {
        private IList<LifecycleRule> rules;

        public IList<LifecycleRule> Rules
        {
            get
            {
                return (this.rules ?? (this.rules = new List<LifecycleRule>()));
            }
            set
            {
                this.rules = value;
            }
        }
    }
}

