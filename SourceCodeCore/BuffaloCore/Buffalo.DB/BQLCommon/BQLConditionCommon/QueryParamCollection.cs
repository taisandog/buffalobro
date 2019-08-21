using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace Buffalo.DB.BQLCommon.BQLConditionCommon
{
    public class QueryParamCollection : IEnumerable<ParamInfo>
    {
        private string tableName;
        private Dictionary<string, ParamInfo> dicQueryParams = new Dictionary<string, ParamInfo>();//�˲�ѯ�����ص��ֶ�
        
        /// <summary>
        /// ��Ӧ�ı���
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
        /// ��ȡ��������Ӧ���ֶ���
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

        #region IEnumerable<string> ��Ա

        public IEnumerator<ParamInfo> GetEnumerator()
        {
            return new QueryParamEnumerator(dicQueryParams);
        }

        #endregion

        #region IEnumerable ��Ա

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new QueryParamEnumerator(dicQueryParams);
        }

        #endregion
    }
}
