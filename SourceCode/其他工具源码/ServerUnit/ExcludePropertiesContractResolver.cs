using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;

using System.Text;

namespace ServerUnit
{
    public class ExcludePropertiesContractResolver : DefaultContractResolver
    {
        private IEnumerable<string> _lstExclude;
        private bool _isShow=false;
        /// <summary>
        /// 属性过滤
        /// </summary>
        /// <param name="excludedProperties">过滤的属性集合</param>
        /// <param name="isShow">是否显示</param>
        public ExcludePropertiesContractResolver(IEnumerable<string> excludedProperties, bool isShow)
        {
            _isShow = isShow;
            _lstExclude = excludedProperties;
        }
        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {
            IList<JsonProperty> jps = new List<JsonProperty>();
            if (_isShow)
            {
                IList<JsonProperty> lst=base.CreateProperties(type, memberSerialization);
                bool hasCollection = false;
                foreach(JsonProperty pro in lst)
                {
                    hasCollection = false;
                    foreach(string name in _lstExclude)
                    {
                        if(pro.PropertyName.Contains(name))
                        {
                            hasCollection = true;
                            break;
                        }
                    }
                    if (hasCollection)
                    {
                        jps.Add(pro);
                    }
                }
            }
            else 
            {
                IList<JsonProperty> lst = base.CreateProperties(type, memberSerialization);
                bool hasCollection = false;
                foreach (JsonProperty pro in lst)
                {
                    hasCollection = false;
                    foreach (string name in _lstExclude)
                    {
                        if (pro.PropertyName.Contains(name))
                        {
                            hasCollection = true;
                            break;
                        }
                    }
                    if (!hasCollection)
                    {
                        jps.Add(pro);
                    }
                }
            }
            

            return jps;
        }
    }
}
