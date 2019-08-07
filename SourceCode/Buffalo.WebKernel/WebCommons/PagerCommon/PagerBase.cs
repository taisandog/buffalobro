using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DB.QueryConditions;

namespace Buffalo.WebKernel.WebCommons.PagerCommon
{
    /// <summary>
    /// ��ҳ����ĵط�
    /// </summary>
    public enum PagerType:int
    {
        /// <summary>
        /// ����ViewState
        /// </summary>
        ViewStateRecord=0,
        /// <summary>
        /// ����url
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
        /// ��ǰUrl����
        /// </summary>
        protected string RequestPageNumName 
        {
            get
            {
                return this.ClientID + "_pagenum";
            }
        }


        /// <summary>
        /// �ض���ָ��ҳ
        /// </summary>
        /// <param name="url">url����</param>
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
        /// ��ҳ��
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
        /// ��ҳ����
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
        /// ��ҳ��
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
        /// ����Դ
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
        /// �������
        /// </summary>
        protected abstract void FillPageNum();

        /// <summary>
        /// �Կؼ����з�ҳ����
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
