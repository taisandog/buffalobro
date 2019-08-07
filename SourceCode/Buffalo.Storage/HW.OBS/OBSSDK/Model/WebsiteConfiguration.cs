namespace OBS.Model
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class WebsiteConfiguration
    {
        private IList<RoutingRule> routingRules;

        public string ErrorDocument { get; set; }

        public string IndexDocument { get; set; }

        public RedirectBasic RedirectAllRequestsTo { get; set; }

        public IList<RoutingRule> RoutingRules
        {
            get
            {
                return (this.routingRules ?? (this.routingRules = new List<RoutingRule>()));
            }
            set
            {
                this.routingRules = value;
            }
        }
    }
}

