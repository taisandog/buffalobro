using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DB.BQLCommon.BQLConditionCommon;
using System.Data;

namespace Buffalo.DB.BQLCommon.BQLExtendFunction
{
    /// <summary>
    /// 自定义函数委托
    /// </summary>
    /// <param name="handle"></param>
    /// <param name="info"></param>
    /// <returns></returns>
    public delegate string DelCustomizeFunction(BQLCustomizeFunction handle, KeyWordInfomation info);
    /// <summary>
    /// 自定义函数
    /// </summary>
    public class BQLCustomizeFunction : BQLParamHandle
    {
        private string _funName;
        private BQLValueItem[] _values;
        private DelCustomizeFunction _handle;
        /// <summary>
        /// 函数名
        /// </summary>
        public string FunctionName
        {
            get
            {
                return _funName;
            }
        }

        /// <summary>
        /// 参数,获取SQL语句则使用 arg.DisplayValue(info)
        /// </summary>
        public BQLValueItem[] Args
        {
            get
            {
                return _values;
            }
        }



        /// <summary>
        /// 自定义函数
        /// </summary>
        /// <param name="funName">函数名</param>
        /// <param name="returnType">返回值类型</param>
        /// <param name="values">函数值</param>
        public BQLCustomizeFunction(string funName, DbType returnType, BQLValueItem[] values)
        {
            this._funName = funName;
            this._values = values;
            this._valueDbType = returnType;
        }

        /// <summary>
        /// 自定义函数
        /// </summary>
        /// <param name="handle">函数</param>
        /// <param name="returnType">返回值类型</param>
        /// <param name="values">函数值</param>
        public BQLCustomizeFunction(DelCustomizeFunction handle, DbType returnType, BQLValueItem[] values)
        {
            this._handle = handle;
            this._values = values;
            this._valueDbType = returnType;
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
            if (_handle != null)
            {
                return _handle(this, info);
            }

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
            StringBuilder sbRet = new StringBuilder(sb.Length + _funName.Length + 5);
            sbRet.Append(_funName);
            sbRet.Append("(");
            sbRet.Append(sb.ToString());
            sbRet.Append(")");
            return sbRet.ToString();
        }
    }
}
