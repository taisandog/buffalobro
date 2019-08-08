using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Buffalo.WebKernel.WebCommons.PagerCommon;
using Buffalo.Kernel;
using Buffalo.WebKernel.WebCommons.ContorlCommons;

namespace WebPageNumberBar
{
    public partial class WebPagingBar :PagerBase
    {
        /// <summary>
        /// 填充数据
        /// </summary>
        protected override void FillPageNum()
        {

            long curPage = dataSource.CurrentPage + 1;
            long totlePage = dataSource.TotlePage;
            this.Visible = true;
            if (dataSource.TotleRecords == 0)
            {
                this.Visible = false;
                return;
            }
            TotlePage = totlePage;
            CurrentPage = dataSource.CurrentPage;
            lblPage.Text = TotlePage.ToString();
            lblCP.InnerText = dataSource.TotleRecords.ToString();
            txtPage.Value = curPage.ToString();
            btnLast.Enabled = true;
            btnNext.Enabled = true;
            btnFirsh.Enabled = true;
            btnUp.Enabled = true;
            if (dataSource.CurrentPage == 0)
            {
                //lbPri.Enabled = false;

                btnFirsh.Enabled = false;
                btnUp.Enabled = false;
            }
            if (dataSource.CurrentPage >= TotlePage - 1)
            {
                //lbNext.Enabled = false;
                btnLast.Enabled = false;
                btnNext.Enabled = false;
            }
        }


        protected void btnGo_Click(object sender, System.EventArgs e)
        {
            int page = Convert.ToInt32(CommonMethods.GetAllNumber(txtPage.Value));

            long totle = TotlePage;
            if (page > totle)
            {
                page = 0;
            }
            DoPageIndexChange(this, page - 1);

        }
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            AutoBtnClick.SaveJs();
            string key = "pagerAutoInclude";
            if (!Page.ClientScript.IsClientScriptIncludeRegistered(key))
            {
                Page.ClientScript.RegisterClientScriptInclude(key, JsSaver.GetDefualtJsUrl(AutoBtnClick.JsName));
            }

            txtPage.Attributes.Add("onkeydown", "return onPage('" + btnGo.ClientID + "',event)");
        }

        /// <summary>
        /// go按钮的ID
        /// </summary>
        public string GoBtnID 
        {
            get
            {
                return  btnGo.ClientID ;
            }
        }

        protected void btnUp_Click(object sender, EventArgs e)
        {
            long page = CurrentPage - 1;
            if (page < 0)
            {
                page = 0;
            }
            DoPageIndexChange(this, page);

        }
        protected void btnNext_Click(object sender, EventArgs e)
        {
            long page = CurrentPage + 1;
            if (page > TotlePage - 1)
            {
                page = TotlePage - 1;
            }

            DoPageIndexChange(this, page);

        }
        protected void btnFirsh_Click(object sender, EventArgs e)
        {
            DoPageIndexChange(this, 0);
        }
        protected void btnLast_Click(object sender, EventArgs e)
        {

            DoPageIndexChange(this, TotlePage - 1);

        }
    }
}