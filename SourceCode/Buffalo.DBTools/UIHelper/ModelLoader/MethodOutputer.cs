using System;
using System.Collections.Generic;
using System.Text;

namespace Buffalo.DBTools.UIHelper.ModelLoader
{
    public class MethodOutputer : ICodeOutputer
    {

        #region ICodeOutputer ≥…‘±

        public string GetCode(Queue<ExpressionItem> queitem)
        {
            StringBuilder sbOut = new StringBuilder();
            while (queitem.Count > 0)
            {
                ExpressionItem item = queitem.Dequeue();
                switch (item.Type)
                {
                    case ExpressionType.String:
                        sbOut.Append(item.Content.ToString());
                        break;
                    default:
                        break;
                }
            }
            return sbOut.ToString();
        }

        #endregion
    }
}
