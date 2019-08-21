using System;
using System.Collections.Generic;
using System.Text;

namespace Buffalo.DB.BQLCommon.BQLConditionCommon
{
    public class AliasCollection : IEnumerable<QueryParamCollection>
    {
        private Dictionary<string, QueryParamCollection> _dicAliass = new Dictionary<string, QueryParamCollection>();//此查询所返回的字段

        /// <summary>
        /// 获取属性名对应的字段名
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        internal QueryParamCollection this[string aliasName]
        {
            get
            {
                QueryParamCollection ret = null;
                _dicAliass.TryGetValue(aliasName, out ret);
                return ret;
            }
            set
            {
                _dicAliass[aliasName] = value;
            }
        }

        #region IEnumerable<string> 成员

        public IEnumerator<QueryParamCollection> GetEnumerator()
        {
            return new AliasEnumerator(_dicAliass);
        }

        #endregion

        #region IEnumerable 成员

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return new AliasEnumerator(_dicAliass);
        }

        #endregion
  
    }
}
