using System;
using System.Collections.Generic;
using System.Text;

namespace Buffalo.DBTools.UIHelper.ModelLoader
{
    /// <summary>
    /// ���ʽ��
    /// </summary>
    public class ExpressionItem
    {
        private StringBuilder _content = new StringBuilder();
        /// <summary>
        /// ����
        /// </summary>
        public StringBuilder Content
        {
            get { return _content; }
        }

        private ExpressionType _type = ExpressionType.String;
        /// <summary>
        /// ����
        /// </summary>
        public ExpressionType Type
        {
            get { return _type; }
            set { _type = value; }
        }
    }
}
