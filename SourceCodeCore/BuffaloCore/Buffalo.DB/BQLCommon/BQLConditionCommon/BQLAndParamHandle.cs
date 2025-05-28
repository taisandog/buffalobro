using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DB.DataBaseAdapter.IDbAdapters;
using Buffalo.DB.DataBaseAdapter;
using Buffalo.Kernel;

namespace Buffalo.DB.BQLCommon.BQLConditionCommon
{
    /// <summary>
    /// 任意字段（占位符为{0},{1},例如:min({0},{1})）
    /// </summary>
    public class BQLAnyParamHandle:BQLParamHandle
    {
        
        private string _paramContent;
        private IEnumerable<BQLValueItem> _valueHandle;
        /// <summary>
        /// 字段值
        /// </summary>
        public string ParamContent
        {
            get { return _paramContent; }
        }
        /// <summary>
        /// 字段关联值
        /// </summary>
        public IEnumerable<BQLValueItem> ValueHandle
        {
            get { return _valueHandle; }
        }
        /// <summary>
        /// 任意字段（占位符为{0},{1},例如:min({0},{1})）
        /// </summary>
        /// <param name="paramContent">字段</param>
        /// <param name="valueHandle">关联值</param>
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
