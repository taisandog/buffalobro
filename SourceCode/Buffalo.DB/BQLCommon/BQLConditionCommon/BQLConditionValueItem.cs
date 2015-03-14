using System;
using System.Collections.Generic;
using System.Text;

namespace Buffalo.DB.BQLCommon.BQLConditionCommon
{
    /// <summary>
    /// 条件值项
    /// </summary>
    public class BQLConditionValueItem : BQLCondition
    {
        private bool _value;
        public BQLConditionValueItem(bool value) 
        {
            _value = value;
        }

        internal override void FillInfo(KeyWordInfomation info)
        {
            
        }

        internal override string DisplayValue(KeyWordInfomation info)
        {
            if (_value) 
            {
                return "1=1";
            }
            return "1=2";

        }
    }
}
