using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using ManagementLib;
using Buffalo.DB.QueryConditions;

public partial class _Default : System.Web.UI.Page 
{
    protected void Page_Load(object sender, EventArgs e)
    {
        pb.OnPageIndexChange += new Buffalo.WebKernel.WebCommons.PagerCommon.PageIndexChange(pb_OnPageIndexChange);
    }

    void pb_OnPageIndexChange(object sender, long currentIndex)
    {
        Bind();
    }

    private string[] _keywords = null;
    /// <summary>
    /// 关键字集合
    /// </summary>
    private string[] KeyWords 
    {
        get 
        {
            if (_keywords != null) 
            {
                return _keywords;
            }
            string content = ViewState["content"] as string;
            if (string.IsNullOrEmpty(content))
            {
                return null ;
            }
            _keywords = content.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            return _keywords;
        }
    }

    /// <summary>
    /// 加亮关键字
    /// </summary>
    /// <param name="content"></param>
    /// <returns></returns>
    public string HighlightKeyWord(object content)
    {
        string[] keywords = KeyWords;
        string str = content as string;
        if (string.IsNullOrEmpty(str)) 
        {
            return "";
        }
        foreach (string keyword in keywords) 
        {
            str = str.Replace(keyword, "<font color=\"red\">" + keyword + "</font>");
        }
        return str;
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        ViewState["content"] = txtSearch.Text;
        pb.CurrentPage = 0;
        Bind();
    }

    private void Bind() 
    {
        string[] content = KeyWords;
        if (content==null || content.Length==0) 
        {
            return;
        }
        PageContent objPage = new PageContent();
        objPage.PageSize = 15;
        objPage.CurrentPage = pb.CurrentPage;
        GridView1.DataSource = SampleInfo.Search(content, objPage);
        GridView1.DataBind();
        pb.DataSource = objPage;
    }
}
