using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.Kernel;

namespace Buffalo.DB.BQLCommon.BQLConditionCommon
{

    public class BQLOperatorHandle : BQLParamHandle, IOperatorPriorityLevel
    {
        private DelOperatorHandle function;
        private BQLValueItem[] parameters;

        public BQLOperatorHandle(DelOperatorHandle function, BQLValueItem[] parameters) 
        {
            this.parameters = parameters;
            this.function = function;

            
            //this.valueType = BQLValueType.Function;
        }
        /// <summary>
        /// ����
        /// </summary>
        internal BQLValueItem[] GetParameters()
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
            DelOperatorHandle degFHandle = function;
            if (degFHandle != null)
            {
                if (_priorityLevel > 0) 
                {
                    return degFHandle(this, info);
                }
                return "(" + degFHandle(this, info) + ")";
            }

            return null;
        }

        #region IOperatorPriorityLevel ��Ա

        private int _priorityLevel;
        /// <summary>
        /// ��������ȼ�
        /// </summary>
        public int PriorityLevel
        {
            get { return _priorityLevel; }
            internal set { _priorityLevel = value; }
        }

        
        #endregion
    }
}
