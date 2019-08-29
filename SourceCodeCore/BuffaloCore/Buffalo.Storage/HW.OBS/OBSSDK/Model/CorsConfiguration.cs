namespace OBS.Model
{
    using System;
    using System.Collections.Generic;

    public class CorsConfiguration
    {
        private IList<CorsRule> rules;

        public IList<CorsRule> Rules
        {
            get
            {
                return (this.rules ?? (this.rules = new List<CorsRule>()));
            }
            set
            {
                this.rules = value;
            }
        }
    }
}

