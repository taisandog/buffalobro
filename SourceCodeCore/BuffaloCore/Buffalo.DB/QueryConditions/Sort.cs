using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using Buffalo.DB.BQLCommon.BQLConditionCommon;
using Buffalo.Kernel;

namespace Buffalo.DB.QueryConditions
{
    public class Sort
    {
        private string _propertyName;
        private SortType _sortType;
        private BQLOrderByHandle _orderHandle;
        /// <summary>
        /// ��ǰ�������������
        /// </summary>
        [Description("��ǰ�������������")]
        [NotifyParentProperty(true)]
        public string PropertyName 
        {
            get 
            {
                return _propertyName;
            }
            set 
            {
                _propertyName = value;
            }
        }
        /// <summary>
        /// ��ǰ������ʽ
        /// </summary>
        [Description("��ǰ������ʽ")]
        [NotifyParentProperty(true)]
        public SortType SortType
        {
            get
            {
                return _sortType;
            }
            set
            {
                _sortType = value;
            }
        }
        /// <summary>
        /// BQL��������
        /// </summary>
        public BQLOrderByHandle OrderHandle
        {
            get
            {
                return _orderHandle;
            }
            set
            {
                _orderHandle = value;
            }
        }
        public override string ToString()
        {
            if (_propertyName!=null) 
            {
                return _propertyName +" "+ _sortType.ToString();
            }
            if (!CommonMethods.IsNull(OrderHandle)) 
            {
                return OrderHandle.ToString();
            }
            return base.ToString();
        }
    }
}
