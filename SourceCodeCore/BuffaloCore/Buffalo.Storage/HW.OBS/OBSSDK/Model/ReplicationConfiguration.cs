namespace OBS.Model
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class ReplicationConfiguration
    {
        private IList<ReplicationRule> rules;

        public string Agency { get; set; }

        public IList<ReplicationRule> Rules
        {
            get
            {
                return (this.rules ?? (this.rules = new List<ReplicationRule>()));
            }
            set
            {
                this.rules = value;
            }
        }
    }
}

