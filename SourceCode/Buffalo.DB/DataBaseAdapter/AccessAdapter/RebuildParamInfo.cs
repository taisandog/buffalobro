using System;
using System.Collections.Generic;
using System.Text;

namespace Buffalo.DB.DataBaseAdapter.AccessAdapter
{
    /// <summary>
    /// 重建的变量信息
    /// </summary>
    public class RebuildParamInfo
    {
        private string _paramName;
        /// <summary>
        /// 变量名
        /// </summary>
        public string ParamName
        {
            get { return _paramName; }
            set { _paramName = value; }
        }

        private int _index;
        /// <summary>
        /// 索引
        /// </summary>
        public int Index
        {
            get { return _index; }
            set { _index = value; }
        }
    }
}
