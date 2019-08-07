using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DB.QueryConditions;

namespace Buffalo.WebKernel.WebCommons.PagerCommon
{
    /// <summary>
    /// 分页保存的地方
    /// </summary>
    public enum PagerType:int
    {
        /// <summary>
        /// 放在ViewState
        /// </summary>
        ViewStateRecord=0,
        /// <summary>
        /// 放在url
        /// </summary>
        RequestUrl=1
    }
    public delegate void PageIndexChange(object sender, long currentIndex);
    public abstract class PagerBase : System.Web.UI.UserControl
    {
        public event PageIndexChange OnPageIndexChange;
        protected PageContent dataSource;
        protected PagerType pagerType = PagerType.ViewStateRecord;

        /// <summary>
        /// 当前Url名称
        /// </summary>
        protected string RequestPageNumName 
        {
            get
            {
                return this.ClientID + "_pagenum";
            }
        }


        /// <summary>
        /// 重定向到指定页
        /// </summary>
        /// <param name="url">url参数</param>
        public void Redirect(PagerUrlCreater url)
        {
            url[RequestPageNumName] = (CurrentPage + 1).ToString();
            Response.Redirect(url.GetUrl());
        }

        protected override void OnInit(EventArgs e)
        {
            
            if (!Page.IsPostBack && pagerType == PagerType.RequestUrl)
            {
                
                if (Request[RequestPageNumName] != null)
                {
                    long num = 1;
                    long.TryParse(Request[RequestPageNumName], out num);
                    ViewState["current"] = num-1;
                }
            }
            base.OnInit(e);
        }

        /// <summary>
        /// 总页数
        /// </summary>
        public long TotalPage 
        {
            get 
            {
                if (ViewState["total"] != null) 
                {
                    return (long)ViewState["total"];
                }
                return -1;
            }
            set 
            {
                ViewState["total"] = value;
            }
        }
        /// <summary>
        /// 分页类型
        /// </summary>
        public virtual PagerType PagerType
        {
            get
            {

                return pagerType;
            }
            set
            {
                pagerType = value;
            }
        }
        /// <summary>
        /// 总页数
        /// </summary>
        public long CurrentPage
        {
            get
            {
                
                if (ViewState["current"] != null)
                {
                    return (long)ViewState["current"];
                }
                
                
                return 0;
            }
            set
            {
                ViewState["current"] = value;
            }
        }

        

        /// <summary>
        /// 数据源
        /// </summary>
        public PageContent DataSource
        {
            get
            {
                return dataSource;
            }
            set
            {
                dataSource = value;

                if (dataSource != null)
                {
                    FillPageNum();
                }
            }
        }

        /// <summary>
        /// 填充数据
        /// </summary>
        protected abstract void FillPageNum();

        /// <summary>
        /// 对控件进行翻页处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="currentIndex"></param>
        protected void DoPageIndexChange(object sender, long currentIndex) 
        {
            if (pagerType == PagerType.RequestUrl) 
            {
                PagerUrlCreater url = new PagerUrlCreater();
                url[RequestPageNumName] = (currentIndex + 1).ToString();
                Response.Redirect(url.GetUrl());
                return;
            }
            ((PagerBase)sender).CurrentPage = currentIndex;
            if (OnPageIndexChange != null)
            {
                OnPageIndexChange(sender, currentIndex);
            }
        }
    }
}
