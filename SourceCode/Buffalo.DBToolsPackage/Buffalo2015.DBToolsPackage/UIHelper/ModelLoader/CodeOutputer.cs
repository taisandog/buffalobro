using System;
using System.Collections.Generic;
using System.Text;

namespace Buffalo.DBTools.UIHelper.ModelLoader
{
    public class CodeOutputer : ICodeOutputer
    {
        #region ICodeOutputer 成员

        public string GetCode(Queue<ExpressionItem> queitem)
        {
            StringBuilder sbOut = new StringBuilder();
            while (queitem.Count > 0)
            {
                ExpressionItem item = queitem.Dequeue();
                switch (item.Type)
                {
                    case ExpressionType.String:
                        FilterCode(item.Content);
                        sbOut.Append("            SystemOut.Append(\"" + item.Content.ToString() + "\");\n");
                        break;
                    case ExpressionType.Code:
                        sbOut.Append("            "+item.Content.ToString() + "\n");
                        break;
                    case ExpressionType.Express:
                        sbOut.Append("            SystemOut.Append(" + item.Content.ToString() + ");\n");
                        break;
                    default:
                        break;
                }
            }
            return sbOut.ToString();
        }

        /// <summary>
        /// 处理字符
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static void FilterCode(StringBuilder sb) 
        {
            sb.Replace("\\", "\\\\");
            sb.Replace("\"", "\\\"");
            sb.Replace("\t", "\\t");
            sb.Replace("\r", "\\r");
            sb.Replace("\n", "\\n");
        }
        #endregion
    }
}
