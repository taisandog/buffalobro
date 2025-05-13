using System;
using System.Collections.Generic;
using System.Text;

namespace Buffalo.DBTools.UIHelper.ModelLoader
{
    public interface ICodeOutputer
    {
        /// <summary>
        /// ЛёШЁДњТы
        /// </summary>
        /// <returns></returns>
        string GetCode(Queue<ExpressionItem> queitem);
    }
}
