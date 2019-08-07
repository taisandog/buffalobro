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
using Buffalo.WebKernel.WebCommons.ContorlCommons;
using Buffalo.Kernel;



public partial class WebPageNumberBar_MyPageNumberBar : PagerBase
{
        private LinkButton[] btnArr;
    
		private Literal CreateLiteral(string text)
		{
			Literal lit=new Literal();
			lit.EnableViewState=false;
			lit.Text=text;
			return lit;
		}
		/// <summary>
		/// 填充数据
		/// </summary>
		protected override void FillPageNum()
		{
            //lbNext.Enabled = true;
            //lbPri.Enabled = true;
            lbFrist.Enabled = true;
            lbLast.Enabled = true;
            long curPage = dataSource.CurrentPage + 1;
            long totlePage = dataSource.TotlePage;
            long totleRecords = dataSource.TotleRecords;
            this.Visible = true;
            if (totleRecords == 0)
            {
                labCur.Text = "0";
                labTotle.Text = "0";
                this.Visible = false;
                return;
            }
			TotlePage=totlePage;
            CurrentPage = dataSource.CurrentPage;
			labCur.Text=curPage.ToString();
			labTotle.Text=totlePage.ToString();
			txtPage.Value=curPage.ToString();
			int index=0;
            
			for(long i=curPage-3;i<=curPage+4;i++)
			{
				if(i<1)
				{
					btnArr[index].Visible=false;
				}
				else if(i>totlePage)
				{
					btnArr[index].Visible=false;
				}
				else if(i==curPage)
				{
					lb4.Text=i.ToString();
					continue;
				}
				else
				{
					btnArr[index].Text=i.ToString();
					btnArr[index].CommandArgument=(i-1).ToString();
					btnArr[index].Visible=true;
				}
				index++;
			}
            if (dataSource.CurrentPage == 0)
            {
                //lbPri.Enabled = false;
                lbFrist.Enabled = false;
            }
            if (dataSource.CurrentPage >= totlePage - 1)
            {
                //lbNext.Enabled = false;
                lbLast.Enabled = false;
            }
			
		}
		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad (e);
            AutoBtnClick.SaveJs();
            string key = "pagerAutoInclude";
            if (!Page.ClientScript.IsClientScriptIncludeRegistered(key))
            {
                Page.ClientScript.RegisterClientScriptInclude(key, JsSaver.GetDefualtJsUrl(AutoBtnClick.JsName));
            }

            txtPage.Attributes.Add("onkeydown", "return onPage('" + btnGo.ClientID + "',event)");
            
		}

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeComponent();
        }
    	
        
		#region Web 窗体设计器生成的代码
		
		
		/// <summary>
		///		设计器支持所需的方法 - 不要使用代码编辑器
		///		修改此方法的内容。
		/// </summary>
		private void InitializeComponent()
		{
			btnArr=new LinkButton[7];
			btnArr[0]=lb1;
			btnArr[1]=lb2;
			btnArr[2]=lb3;
			btnArr[3]=lb5;
			btnArr[4]=lb6;
			btnArr[5]=lb7;
			btnArr[6]=lb8;
		}
		#endregion
		
		protected void lbFrist_Click(object sender, System.EventArgs e)
		{
            
            DoPageIndexChange(this, 0);
		}

        protected void lbLast_Click(object sender, System.EventArgs e)
		{
            long page = TotlePage;
			if(page<0)
			{
				page=1;
			}
            
            DoPageIndexChange(this, page - 1);
            
		}
        protected void btnPage_Click(object sender, System.EventArgs e)
		{
			int page=0;
			LinkButton btn=(LinkButton)sender;
			page=Convert.ToInt32(btn.CommandArgument);
            
            DoPageIndexChange(this, page);
            
		}

        protected void btnGo_Click(object sender, System.EventArgs e)
		{
            int page = Convert.ToInt32(CommonMethods.GetAllNumber(txtPage.Value));

            long totle = TotlePage;
            if (page > totle)
            {
                page = 0;
            }
            
            DoPageIndexChange(this, page-1);
            
		}
       
        
    protected void lbPri_Click(object sender, EventArgs e)
    {
        long totlePage = TotlePage;
        long curPage = CurrentPage;
        if (curPage > 0)
        {
            
            DoPageIndexChange(this, curPage - 1);
        }
        
    }
    protected void lbNext_Click(object sender, EventArgs e)
    {
        long totlePage = TotlePage;
        long curPage = CurrentPage;
        if (totlePage - 1 > curPage)
        {
            
            DoPageIndexChange(this, curPage + 1);
        }
    }
}

