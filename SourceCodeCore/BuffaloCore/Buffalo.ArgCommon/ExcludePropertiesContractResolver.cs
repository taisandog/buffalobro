using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Text;

namespace Buffalo.ArgCommon
{
    /// <summary>
    /// 显示和屏蔽信息开关
    /// </summary>
    public class ExcludePropertiesContractResolver : DefaultContractResolver
    {
        private Dictionary<string,bool> _dicExclude = new Dictionary<string, bool>();
        private JsonParseType _showType = JsonParseType.Show;

        /// <summary>
        /// 显示或屏蔽,true时候是只显示ExcludeProperties，false时候则为屏蔽ExcludeProperties
        /// </summary>
        public JsonParseType ShowType 
        {
            get 
            {
                return _showType;
            }
        }
        /// <summary>
        /// 要处理的属性
        /// </summary>
        public Dictionary<string, bool> ExcludeProperties 
        {
            get 
            {
                return _dicExclude;
            }
        }

        /// <summary>
        /// 属性过滤
        /// </summary>
        /// <param name="excludedProperties">属性集合</param>
        /// <param name="showType">显示或屏蔽,Show时候是只显示ExcludeProperties，Ignore时候则为屏蔽ExcludeProperties</param>
        public ExcludePropertiesContractResolver(IEnumerable<string> excludedProperties, JsonParseType showType)
        {
            _showType = showType;
            Dictionary<string, bool> dic = new Dictionary<string, bool>();
            foreach (string item in excludedProperties) 
            {
                _dicExclude[item] = true;
            }
        }

        /// <summary>
        /// 内部处理接口，返回要显示的属性
        /// </summary>
        /// <param name="type"></param>
        /// <param name="memberSerialization"></param>
        /// <returns></returns>
        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {
           

            IList<JsonProperty> lst = base.CreateProperties(type, memberSerialization);
            IList<JsonProperty> jps = new List<JsonProperty>(lst.Count);
            bool isShow = (_showType == JsonParseType.Show);
            foreach (JsonProperty pro in lst)
            {
                bool isContains = _dicExclude.ContainsKey(pro.PropertyName);
                if (isShow == isContains)
                {
                    jps.Add(pro);
                }
            }
            return jps;
        }
    }
}
