using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using System.Text;
using Buffalo.WebKernel.WebCommons.PagerCommon;

namespace WebPageNumberBar
{
    public partial class WebPageNumberBar_NFPageBar:PagerBase
    {
        /// <summary>
        /// 填充数据
        /// </summary>
        protected override void FillPageNum()
        {
            lbNext.Enabled = true;
            lbPri.Enabled = true;
            lbFrist.Enabled = true;
            lbLast.Enabled = true;
            long curPage = dataSource.CurrentPage;
            long totlePage = dataSource.TotlePage;
            TotlePage = totlePage;
            CurrentPage = curPage;
            labCur.Text = (curPage + 1).ToString();
            labTotle.Text = totlePage.ToString();
            if (curPage == 0) 
            {
                lbPri.Enabled = false;
                lbFrist.Enabled = false;
            }
            if (curPage >= totlePage-1) 
            {
                lbNext.Enabled = false;
                lbLast.Enabled = false;
            }
        }
        
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        
        protected void lbPri_Click(object sender, EventArgs e)
        {

            //int totlePage = TotlePage;
            //int curPage = CurrentPage;
            if (CurrentPage > 0)
            {
                DoPageIndexChange(this, CurrentPage-1);
            }
            
        }
        protected void lbNext_Click(object sender, EventArgs e)
        {
            
            //int totlePage = TotlePage;
            //int curPage = CurrentPage;
            if (TotlePage - 1 > CurrentPage)
            {
                DoPageIndexChange(this, CurrentPage + 1);
            }
            
        }
        protected void lbFrist_Click(object sender, EventArgs e)
        {
            DoPageIndexChange(this, 0);
        }
        protected void lbLast_Click(object sender, EventArgs e)
        {
            long totlePage = TotlePage;
            long curPage = totlePage - 1;
            if (curPage < 0)
            {
                curPage = 0;
            }
            DoPageIndexChange(this, curPage);
        }
}
}