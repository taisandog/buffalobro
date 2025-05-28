using Buffalo.DB.BQLCommon.BQLBaseFunction;
using Buffalo.DB.DataBaseAdapter;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Buffalo.DB.BQLCommon.BQLConditionCommon
{
    /// <summary>
    /// 任意条件（占位符为{0},{1},例如:{0} like '{1}'）
    /// </summary>
    public class BQLAnyCondition : BQLCondition
    {
        private IEnumerable<BQLValueItem> _valueHandle;
        private string _condition;
        /// <summary>
        ///  任意条件（占位符为{0},{1},例如:{0} like '{1}'）
        /// </summary>
        /// <param name="condition">条件</param>
        /// <param name="valueHandle">参数值</param>
        public BQLAnyCondition(string condition, IEnumerable<BQLValueItem>  valueHandle)
        {
            _condition = condition;
            _valueHandle = valueHandle;
            this._valueDbType = DbType.Boolean;
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
            
            string ret=string.Format(_condition, varArr.ToArray());
            return ret;
        }
    }
}
