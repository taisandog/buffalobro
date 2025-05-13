using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace Buffalo.MongoDB.QueryCondition
{
    public class MGSort
    {
        private string _propertyName;
        private MGSortType _sortType;
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
        public MGSortType SortType
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
        
        public override string ToString()
        {
            if (_propertyName!=null) 
            {
                return _propertyName +" "+ _sortType.ToString();
            }
            
            return base.ToString();
        }
    }
}
