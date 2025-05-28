using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DB.DataBaseAdapter.IDbAdapters;
using Buffalo.DB.DataBaseAdapter;
using Buffalo.Kernel;

namespace Buffalo.DB.BQLCommon.BQLConditionCommon
{
    /// <summary>
    /// �����ֶΣ�ռλ��Ϊ{0},{1},����:min({0},{1})��
    /// </summary>
    public class BQLAnyParamHandle:BQLParamHandle
    {
        
        private string _paramContent;
        private IEnumerable<BQLValueItem> _valueHandle;
        /// <summary>
        /// �ֶ�ֵ
        /// </summary>
        public string ParamContent
        {
            get { return _paramContent; }
        }
        /// <summary>
        /// �ֶι���ֵ
        /// </summary>
        public IEnumerable<BQLValueItem> ValueHandle
        {
            get { return _valueHandle; }
        }
        /// <summary>
        /// �����ֶΣ�ռλ��Ϊ{0},{1},����:min({0},{1})��
        /// </summary>
        /// <param name="paramContent">�ֶ�</param>
        /// <param name="valueHandle">����ֵ</param>
        public BQLAnyParamHandle(string paramContent, IEnumerable<BQLValueItem> valueHandle)
        {
            _valueHandle = valueHandle;
            this._paramContent = paramContent;
        }
        internal override string DisplayValue(KeyWordInfomation info)
        {
            List<object> varArr = new List<object>();
            if (_valueHandle != null)
            {
                foreach (var item in _valueHandle)
                {
                    varArr.Add(item.DisplayValue(info));
                }
            }

            string ret = string.Format(_paramContent, varArr.ToArray());
            return ret;
        }

        internal override void FillInfo(KeyWordInfomation info)
        {
            
            if (_valueHandle == null)
            {
                return;
            }
            foreach (var item in _valueHandle)
            {
                BQLValueItem.DoFillInfo(item, info);
            }
        }
    }
}
