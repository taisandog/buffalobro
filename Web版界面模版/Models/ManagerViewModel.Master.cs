using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Models
{
    public partial class ManagerViewModel : System.Web.UI.MasterPage,IModelInfo
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        #region IModelInfo 成员

        /// <summary>
        /// 位置字符串
        /// </summary>
        public string LocationString
        {
            get
            {
                return labLocation.Text;
            }
            set
            {
                labLocation.Text = value;
            }
        }

        public bool IsSearch
        {
            get { return true; }
        }

        #endregion
    }
}