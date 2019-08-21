using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DB.BQLCommon.BQLKeyWordCommon;
using Buffalo.DB.BQLCommon.BQLConditionCommon;
using Buffalo.DB.BQLCommon.BQLBaseFunction;
using Buffalo.Kernel;

namespace Buffalo.DB.QueryConditions
{
    /// <summary>
    /// ֵ���ü���
    /// </summary>
    public class ValueSetList : Dictionary<string, BQLValueItem>
    {
        /// <summary>
        /// ���һ��Update����
        /// </summary>
        /// <param name="parameter">�ֶ�</param>
        /// <param name="valueItem">ֵ</param>
        public void Add(BQLParamHandle parameter, BQLValueItem valueItem) 
        {
            string key=GetKey(parameter);
            
            this[key]=valueItem;
        }

        /// <summary>
        /// ��ȡ������
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        private string GetKey(BQLParamHandle parameter)
        {
            BQLEntityParamHandle ehandle=parameter as BQLEntityParamHandle;
            if(!CommonMethods.IsNull( ehandle))
            {
                return ehandle.PInfo.PropertyName;
            }
            BQLOtherParamHandle ohandle=parameter as BQLOtherParamHandle;
            if(!CommonMethods.IsNull( ohandle))
            {
                return ohandle.ParamName;
            }
            return parameter.DisplayValue(BQLValueItem.GetKeyInfo());
        }

        /// <summary>
        /// ���һ��Update����
        /// </summary>
        /// <param name="propertyName">����</param>
        /// <param name="valueItem">ֵ</param>
        public void Add(string propertyName, BQLValueItem valueItem)
        {
            Add(BQL.ToParam(propertyName),valueItem);
        }

        

    }
}
