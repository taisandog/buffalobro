using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Models;

namespace Models
{
    public partial class EditModel : System.Web.UI.MasterPage, IModelInfo
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
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
            get { return false; }
        }
    }
}