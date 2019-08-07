using System;
using System.Collections.Generic;

using System.Text;
using System.Windows.Forms;

namespace Buffalo.Winforms.UILoaderUnit
{
    public interface IShowToolTip
    {
        /// <summary>
        /// 显示tooltip
        /// </summary>
        /// <param name="ctr"></param>
        /// <param name="message"></param>
        void ShowTooltip(Control ctr, string title, string message, bool isErr);
    }
}
