using System;
using System.Collections.Generic;
using System.Text;

namespace Buffalo.DB.DataBaseAdapter.AccessAdapter
{
    /// <summary>
    /// �ؽ��ı�����Ϣ
    /// </summary>
    public class RebuildParamInfo
    {
        private string _paramName;
        /// <summary>
        /// ������
        /// </summary>
        public string ParamName
        {
            get { return _paramName; }
            set { _paramName = value; }
        }

        private int _index;
        /// <summary>
        /// ����
        /// </summary>
        public int Index
        {
            get { return _index; }
            set { _index = value; }
        }
    }
}
