using System;
using System.Collections.Generic;
using System.Text;

namespace Buffalo.DB.BQLCommon
{
    public class BQLInfos
    {
        private bool _isShowTableName = true;
        
        /// <summary>
        /// 如果是输出字段时候是否显示表名
        /// </summary>
        public bool IsShowTableName
        {
            get { return _isShowTableName; }
            set { _isShowTableName = value; }
        }

        private bool _isPutPropertyName = false;
        /// <summary>
        /// 是否输出属性名
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
        /// 分页类的数量
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
