using System;
using System.Collections.Generic;
using System.Text;

namespace Buffalo.DBTools.UIHelper.ModelLoader
{
    /// <summary>
    /// 表达式项
    /// </summary>
    public class ExpressionItem
    {
        private StringBuilder _content = new StringBuilder();
        /// <summary>
        /// 内容
        /// </summary>
        public StringBuilder Content
        {
            get { return _content; }
        }

        private ExpressionType _type = ExpressionType.String;
        /// <summary>
        /// 类型
        /// </summary>
        public ExpressionType Type
        {
            get { return _type; }
            set { _type = value; }
        }
    }
}
