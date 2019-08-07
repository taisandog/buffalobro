using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DB.BQLCommon.BQLConditionCommon;
using System.Data;

namespace Buffalo.DB.BQLCommon.BQLExtendFunction
{
    public class BQLCustomizeFunction : BQLParamHandle
    {
        private string _funName;
        private BQLValueItem[] _values;
        /// <summary>
        /// 自定义函数
        /// </summary>
        /// <param name="funName">函数名</param>
        /// <param name="values">函数值</param>
        public BQLCustomizeFunction(string funName, BQLValueItem[] values)
        {
            this._funName = funName;
            this._values = values;
            this._valueDbType = DbType.Object;
        }
        /// <summary>
        /// 自定义函数
        /// </summary>
        /// <param name="funName">函数名</param>
        /// <param name="values">函数值</param>
        public BQLCustomizeFunction(string funName)
            :this(funName,null)
        {
        }

        internal override void FillInfo(KeyWordInfomation info)
        {
            if (_values != null && _values.Length > 0) 
            {
                foreach (BQLValueItem param in _values) 
                {
                    BQLValueItem.DoFillInfo(param, info);
                }
            }
        }

        internal override string DisplayValue(KeyWordInfomation info)
        {
            //DelCommonFunction handle = funHandle;
            StringBuilder sb = new StringBuilder();
            if (_values != null)
            {
                foreach (BQLValueItem value in _values) 
                {
                    sb.Append(value.DisplayValue(info) + ",");
                }
                if (sb.Length > 0) 
                {
                    sb.Remove(sb.Length - 1, 1);
                }
            }
            StringBuilder sbRet = new StringBuilder(sb.Length+_funName.Length+5);
            sbRet.Append(_funName);
            sbRet.Append("(");
            sbRet.Append(sb.ToString());
            sbRet.Append(")");
            return sbRet.ToString();
        }
    }
}
