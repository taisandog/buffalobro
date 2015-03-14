using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.Web.UI.HtmlControls;
namespace Buffalo.WebControls.LocationSelecter
{
    public class SelectControlFilter : ControlIDConverter
    {
        protected override bool FilterControl(Control control)
        {
            if (control is HtmlSelect)
                return true;
            else
            {
                return false;
            }

        }
    }

    
}
