using System;
using System.Collections.Generic;
using System.Text;

namespace Buffalo.DB.BQLCommon
{
    public class BQLInfos
    {
        private bool _isShowTableName = true;
        
        /// <summary>
        /// ���������ֶ�ʱ���Ƿ���ʾ����
        /// </summary>
        public bool IsShowTableName
        {
            get { return _isShowTableName; }
            set { _isShowTableName = value; }
        }

        private bool _isPutPropertyName = false;
        /// <summary>
        /// �Ƿ����������
        /// </summary>
        public bool IsPutPropertyName
        {
            get
            {
                return _isPutPropertyName;
            }
            set
            {
                _isPutPropertyName = value;
            }
        }

        private int _pagerCount = 0;

        /// <summary>
        /// ��ҳ�������
        /// </summary>
        public int PagerCount 
        {
            get
            {
                return _pagerCount;
            }
            set
            {
                _pagerCount = value;
            }
        }
    }
}
