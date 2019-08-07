using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.Kernel;

namespace Buffalo.DB.BQLCommon.BQLConditionCommon
{
    public class BQLComparItem : BQLCondition, IOperatorPriorityLevel
    {
        private DelFunctionHandle function;
        private IList<BQLValueItem> parameters;

        public BQLComparItem(DelFunctionHandle function, IList<BQLValueItem> parameters) 
        {
            this.parameters = parameters;
            this.function = function;
            //this.valueType = BQLValueType.Function;
        }
        /// <summary>
        /// 参数
        /// </summary>
        internal IList<BQLValueItem> GetParameters()
        {
            
            return parameters;
            
        }
        
        internal override void FillInfo(KeyWordInfomation info)
        {
            foreach (BQLValueItem value in parameters)
            {

                BQLValueItem.DoFillInfo(value, info);

            }
        }

        internal override string DisplayValue(KeyWordInfomation info)
        {
            DelFunctionHandle degFHandle = function;
            if (degFHandle != null)
            {
                if (_priorityLevel > 0)
                {
                    return degFHandle(this, info);
                }
                return  "(" + degFHandle(this,info) + ")";
            }
            
            return null;
            
        }

        #region IOperatorPriorityLevel 成员

        private int _priorityLevel=10;
        /// <summary>
        /// 运算符优先级
        /// </summary>
        public int PriorityLevel
        {
            get { return _priorityLevel; }
            internal set { _priorityLevel = value; }
        }


        #endregion
    }
}
