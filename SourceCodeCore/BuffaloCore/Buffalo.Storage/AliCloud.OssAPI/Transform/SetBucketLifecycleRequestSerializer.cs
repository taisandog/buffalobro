﻿/*
 * Copyright (C) Alibaba Cloud Computing
 * All rights reserved.
 * 
 */

using System.IO;
using Aliyun.OSS.Util;
using Aliyun.OSS.Model;

namespace Aliyun.OSS.Transform
{
    internal class SetBucketLifecycleRequestSerializer : RequestSerializer<SetBucketLifecycleRequest, LifecycleConfiguration>
    {
        public SetBucketLifecycleRequestSerializer(ISerializer<LifecycleConfiguration, Stream> contentSerializer)
            : base(contentSerializer)
        { }

        public override Stream Serialize(SetBucketLifecycleRequest request)
        {
            var rules = request.LifecycleRules;
            var lcc = new LifecycleConfiguration
            {
                LifecycleRules = new Model.LifecycleRule[rules.Count]
            };
            
            for (var i = 0; i < rules.Count; i++)
            {
                lcc.LifecycleRules[i] = new Model.LifecycleRule
                {
                    ID = rules[i].ID, 
                    Prefix = rules[i].Prefix
                };

                switch (rules[i].Status)
                {
                    case RuleStatus.Enabled:
                        lcc.LifecycleRules[i].Status = RuleStatus.Enabled.ToString();
                        break;
                    case RuleStatus.Disabled:
                        lcc.LifecycleRules[i].Status = RuleStatus.Disabled.ToString();
                        break;
                }

                lcc.LifecycleRules[i].Expiration = new Expiration();
                if (rules[i].CreatedBeforeDate.HasValue)
                    lcc.LifecycleRules[i].Expiration.CreatedBeforeDate = DateUtils.FormatIso8601Date(rules[i].CreatedBeforeDate.Value);
                else if (rules[i].ExpriationDays.HasValue)
                    lcc.LifecycleRules[i].Expiration.Days = rules[i].ExpriationDays.Value;
                else if (rules[i].ExpirationTime.HasValue)
                    lcc.LifecycleRules[i].Expiration.Date = DateUtils.FormatIso8601Date(rules[i].ExpirationTime.Value);

                if(rules[i].Transitions != null)
                {
                    lcc.LifecycleRules[i].Transition = new LifecycleRuleTransition[rules[i].Transitions.Length];
                    for (int j = 0; j < lcc.LifecycleRules[i].Transition.Length; j++)
                    {
                        lcc.LifecycleRules[i].Transition[j] = ConvertTransition(rules[i].Transitions[j]);
                    }
                }

                if (rules[i].AbortMultipartUpload != null)
                {
                    lcc.LifecycleRules[i].AbortMultipartUpload = ConvertExpiration(rules[i].AbortMultipartUpload);
                }
            }

            return ContentSerializer.Serialize(lcc);
        }

        internal static LifecycleRuleTransition ConvertTransition(LifecycleRule.LifeCycleTransition transition)
        {
            LifecycleRuleTransition lifecycleRuleTransition = new LifecycleRuleTransition();
            if (transition.LifeCycleExpiration != null)
            {
                lifecycleRuleTransition.Days = transition.LifeCycleExpiration.Days;
                lifecycleRuleTransition.CreatedBeforeDate = transition.LifeCycleExpiration.CreatedBeforeDate != null ?
                    DateUtils.FormatIso8601Date(transition.LifeCycleExpiration.CreatedBeforeDate.Value) : null;
            }

            lifecycleRuleTransition.StorageClass = transition.StorageClass;

            return lifecycleRuleTransition;
        }

        internal static Expiration ConvertExpiration(LifecycleRule.LifeCycleExpiration lifeCycleExpiration)
        {
            Expiration expiration = new Expiration()
            {
                Days = lifeCycleExpiration.Days
            };

            expiration.CreatedBeforeDate = lifeCycleExpiration.CreatedBeforeDate != null ? 
                DateUtils.FormatIso8601Date(lifeCycleExpiration.CreatedBeforeDate.Value) : null;

            return expiration;
        }
    }
}
