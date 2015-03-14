using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.Mobile;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.MobileControls;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Text;
using Buffalo.WebKernel.WebCommons.PagerCommon;
using Buffalo.WebKernel.WebCommons;
using Buffalo.Kernel;
using Buffalo.WebKernel.WebCommons.ContorlCommons;
public partial class UrlPagingBar : PagerBase
{
    protected override void OnInit(EventArgs e)
    {
        base.pagerType = PagerType.RequestUrl;
        base.OnInit(e);
    }

    public override PagerType PagerType
    {
        get
        {
            return base.PagerType;
        }
        set
        {
            
        }
    }

    

    /// <summary>
    /// Ìî³äÊý¾Ý
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
        //btnLast.Enabled = true;
        //btnNext.Enabled = true;
        //btnFirsh.Enabled = true;
        //btnUp.Enabled = true;
        //if (dataSource.CurrentPage == 0)
        //{
        //    //lbPri.Enabled = false;

        //    btnFirsh.Enabled = false;
        //    btnUp.Enabled = false;
        //}
        //if (dataSource.CurrentPage >= TotlePage - 1)
        //{
        //    //lbNext.Enabled = false;
        //    btnLast.Enabled = false;
        //    btnNext.Enabled = false;
        //}
        PagerUrlCreater url=new PagerUrlCreater();
        if (dataSource.CurrentPage != 0)
        {
            url[RequestPageNumName] = "1";
            btnFirsh.NavigateUrl = url.GetUrl();
            url[RequestPageNumName] = CurrentPage.ToString();
            btnUp.NavigateUrl = url.GetUrl();
        }
        if (dataSource.CurrentPage < TotlePage - 1)
        {
            url[RequestPageNumName] = TotlePage.ToString();
            btnLast.NavigateUrl = url.GetUrl();
            url[RequestPageNumName] = (CurrentPage + 2).ToString();
            btnNext.NavigateUrl = url.GetUrl();
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

    

    protected void btnUp_Click(object sender, EventArgs e)
    {
        long page = CurrentPage - 1;
        if (page<0)
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
