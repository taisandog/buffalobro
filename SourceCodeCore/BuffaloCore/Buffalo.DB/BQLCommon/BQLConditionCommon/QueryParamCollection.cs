using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace Buffalo.DB.BQLCommon.BQLConditionCommon
{
    public class QueryParamCollection : IEnumerable<ParamInfo>
    {
        private string tableName;
        private Dictionary<string, ParamInfo> dicQueryParams = new Dictionary<string, ParamInfo>();//此查询所返回的字段
        
        /// <summary>
        /// 对应的表名
        /// </summary>
        internal string TableName 
        {
            get 
            {
                return tableName;
            }
            set 
            {
                tableName = value;
            }
        }
        /// <summary>
        /// 获取属性名对应的字段名
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        internal ParamInfo this[string propertyName]
        {
            get 
            {
                ParamInfo ret = null;
                dicQueryParams.TryGetValue(propertyName, out ret);
                return ret;
            }
            set 
            {
                dicQueryParams[propertyName] = value;
            }
        }

        #region IEnumerable<string> 成员

        public IEnumerator<ParamInfo> GetEnumerator()
        {
            return new QueryParamEnumerator(dicQueryParams);
        }

        #endregion

        #region IEnumerable 成员

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new QueryParamEnumerator(dicQueryParams);
        }

        #endregion
    }
}
