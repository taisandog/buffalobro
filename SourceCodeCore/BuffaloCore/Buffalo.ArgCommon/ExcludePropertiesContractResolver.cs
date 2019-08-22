using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Text;

namespace Buffalo.ArgCommon
{
    public class ExcludePropertiesContractResolver : DefaultContractResolver
    {
        private Dictionary<string,bool> _lstExclude = new Dictionary<string, bool>();
        private bool _isShow=false;
        /// <summary>
        /// 属性过滤
        /// </summary>
        /// <param name="excludedProperties">过滤的属性集合</param>
        /// <param name="isShow">是否显示</param>
        public ExcludePropertiesContractResolver(IEnumerable<string> excludedProperties, bool isShow)
        {
            _isShow = isShow;
            Dictionary<string, bool> dic = new Dictionary<string, bool>();
            foreach (string item in excludedProperties) 
            {
                _lstExclude[item] = true;
            }
        }
        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {
            IList<JsonProperty> jps = new List<JsonProperty>();

            IList<JsonProperty> lst = base.CreateProperties(type, memberSerialization);
            foreach (JsonProperty pro in lst)
            {
                bool isContains = _lstExclude.ContainsKey(pro.PropertyName);
                if (_isShow == isContains)
                {
                    jps.Add(pro);
                }
            }
            return jps;
        }
    }
}
