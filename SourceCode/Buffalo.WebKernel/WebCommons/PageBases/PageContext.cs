using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web;
using Buffalo.DB.QueryConditions;
using Buffalo.WebKernel.ARTDialog;

namespace Buffalo.WebKernel.WebCommons.PageBases
{
    public class PageContext : System.Web.UI.Page
    {

        /// <summary>
        /// 直接把文字输出到页面
        /// </summary>
        /// <param name="content"></param>
        protected static void ShowResponseText(string content)
        {
            HttpResponse response = HttpContext.Current.Response;
            response.Clear();
            response.Write(content);
            response.End();
        }

        /// <summary>
        /// 给文本框控件添加keydown事件
        /// </summary>
        /// <param name="ctl">控件</param>
        /// <returns></returns>
        private static void AddTextKeyDown(Control ctl)
        {
            Type type = ctl.GetType();
            string key = "onkeydown";
            string value = "keydown();";
            if (type == typeof(TextBox))
            {
                TextBox txt = (TextBox)ctl;
                txt.Attributes.Add(key, value);
            }
            else if (type == typeof(HtmlInputText))
            {
                HtmlInputText txt = (HtmlInputText)ctl;
                txt.Attributes.Add(key, value);
            }
        }

        /// <summary>
        /// 向页面注册JS脚本
        /// </summary>
        /// <param name="jsName">JS块名</param>
        /// <param name="root">JS路径</param>
        public static void LoadJsBlock(string jsName, string root)
        {
            Page page = (Page)System.Web.HttpContext.Current.Handler;
            if (!page.ClientScript.IsClientScriptBlockRegistered(jsName))
            {
                page.ClientScript.RegisterClientScriptBlock(page.GetType(), jsName, " <script src=\"" + root + "\" language=\"JavaScript\" type=\"text/javascript\"></script>");
            }
        }

        /// <summary>
        /// 设置本页面的默认提交控件
        /// </summary>
        /// <param name="conotrl"></param>
        public void SetDefaultButton(System.Web.UI.Control conotrl)
        {
            this.Form.Attributes.Add("defaultbutton", conotrl.ClientID);
            //string js = "function keydown(){";
            //js+="var keycode=window.event.keyCode;";   
            //js+="if(keycode==13)";
            //js+=conotrl.ClientID+".click();";
            //js+="}";
            //Page.ClientScript.RegisterClientScriptBlock(System.Web.HttpContext.Current.Handler.GetType(), "DefaultButton", js, true);
            //foreach (Control curCtl in this.Controls) 
            //{
            //    AddTextKeyDown(curCtl);
            //}
        }

        /// <summary>
        /// 加载指定路径的CSS
        /// </summary>
        /// <param name="cssUrl">路径</param>
        public static void LoadCSS(string cssUrl)
        {
            Page page = (Page)System.Web.HttpContext.Current.Handler;
            if (!page.ClientScript.IsClientScriptBlockRegistered(System.Web.HttpContext.Current.Handler.GetType(), cssUrl))
            {
                string css = "<link rel=\"stylesheet\" type=\"text/css\" href=\"" + cssUrl + "\" />";
                page.ClientScript.RegisterClientScriptBlock(System.Web.HttpContext.Current.Handler.GetType(), cssUrl, css, false);
            }
        }
        /// <summary>
        /// 输出对话框
        /// </summary>
        /// <param name="message">需要输出的信息</param>
        /// <param name="isShowInLoadFinish">是否在页面加载完毕时候再弹出</param>
        public static void Alert(string message,bool isShowInLoadFinish)
        {
            Page page = (Page)System.Web.HttpContext.Current.Handler;
            message = message.Replace("\n", "\\n");
            if (isShowInLoadFinish)
            {
                page.ClientScript.RegisterStartupScript(System.Web.HttpContext.Current.Handler.GetType(), message, "alert('" + message + "');", true);
            }
            else
            {
                page.ClientScript.RegisterClientScriptBlock(System.Web.HttpContext.Current.Handler.GetType(), message, "alert('" + message + "');", true);
            }
        }
        /// <summary>
        /// 输出对话框
        /// </summary>
        /// <param name="message">需要输出的信息</param>
        public static void Alert(string message)
        {
            Alert(message, false);
        }

        ///// <summary>
        ///// 输出对话框(for AJAX)
        ///// </summary>
        ///// <param name="message">需要输出的信息</param>
        //public void Alert2(string message) 
        //{
        //    message = message.Replace("\n", "\\n");
        //    ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), message, "alert('"+message+"')", true);
        //}

        ///// <summary>
        ///// 重定向页面(for AJAX)
        ///// </summary>
        ///// <param name="url">定向的页面</param>
        //public void JSRedirect2(string url)
        //{
        //    ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), url, "window.location='" + url + "';", true);
        //}

        /// <summary>
        /// 重定向页面
        /// </summary>
        /// <param name="url">定向的页面</param>
        public static void JSRedirect(string url)
        {
            Page page = (Page)System.Web.HttpContext.Current.Handler;
            page.ClientScript.RegisterClientScriptBlock(System.Web.HttpContext.Current.Handler.GetType(), url, "window.location='" + url + "';", true);
        }

        /// <summary>
        /// 清空页面上所有的文本
        /// </summary>
        public void ClearAllText()
        {
            Page page = (Page)System.Web.HttpContext.Current.Handler;
            for (int i = 0; i < page.Controls[0].Controls.Count; i++)
            {
                if (page.Controls[0].Controls[i].GetType() == typeof(System.Web.UI.WebControls.TextBox))
                {
                    TextBox tmpTxt = (TextBox)page.Controls[0].Controls[i];
                    tmpTxt.Text = "";
                }
            }
        }

        /// <summary>
        /// 页面滚动到最下方
        /// </summary>
        public static void ScrollToEnd()
        {
            Page page = (Page)System.Web.HttpContext.Current.Handler;
            page.ClientScript.RegisterStartupScript(System.Web.HttpContext.Current.Handler.GetType(), "scroll", "window.scrollTo(0, document.body.scrollHeight);", true);
        }

        


        /// <summary>
        /// 当前用户的登陆地址
        /// </summary>
        public static string LoginUrl
        {
            get
            {
                if (HttpContext.Current.Session["CurrentRoot"] != null)
                {
                    return HttpContext.Current.Session["CurrentRoot"].ToString();
                }
                return null;
            }
            set
            {
                HttpContext.Current.Session["CurrentRoot"] = value;
            }
        }
        ///// <summary>
        ///// 返回页面信息到实体
        ///// </summary>
        ///// <param name="objPage">页面</param>
        ///// <param name="modle">实体</param>
        ///// <param name="pType">命名规则</param>
        //public static void UpdateModel(Control objControl, object modle, PrefixType pType)
        //{
        //    WebModel.UpdateModel(objControl, modle, pType);
        //}
        ///// <summary>
        ///// 返回页面信息到实体(默认前缀是小写)
        ///// </summary>
        ///// <param name="objPage">页面</param>
        ///// <param name="modle">实体</param>
        //public static void UpdateModel(Control objControl, object modle)
        //{
        //    WebModel.UpdateModel(objControl, modle, PrefixType.Camel);
        //}

        ///// <summary>
        ///// 返回页面信息到实体
        ///// </summary>
        ///// <param name="objPage">页面</param>
        ///// <param name="modle">实体</param>
        ///// <param name="pType">命名规则</param>
        //public void UpdateModel(object modle, PrefixType pType)
        //{
        //    WebModel.UpdateModel(Page, modle, pType);
        //}
        ///// <summary>
        ///// 返回页面信息到实体(默认前缀是小写)
        ///// </summary>
        ///// <param name="objPage">页面</param>
        ///// <param name="modle">实体</param>
        //public void UpdateModel(object modle)
        //{
        //    WebModel.UpdateModel(Page, modle, PrefixType.Camel);
        //}



        #region 排序管理

        protected const string SortViewStateID="$SortViewID";

        /// <summary>
        /// 填充排序
        /// </summary>
        /// <param name="lstSort"></param>
        protected virtual void FillOrderBy(string name,SortList lstSort)
        {
            string str = ViewState[name+SortViewStateID] as string;
            if (string.IsNullOrEmpty(str)) 
            {
                return;
            }
            string[] sortItems = str.Split(',');
            foreach (string item in sortItems) 
            {
                Sort objSort = new Sort();
                string[] iPart = item.Trim().Split(' ');
                if (iPart.Length <1) 
                {
                    continue;
                }
                objSort.PropertyName = iPart[0];
                if (iPart.Length < 2)
                {
                    objSort.SortType = SortType.ASC;
                }
                else 
                {
                    objSort.SortType = (iPart[1].ToLower() == "asc") ? SortType.ASC : SortType.DESC;
                }
                lstSort.Add(objSort);
            }
        }
        /// <summary>
        /// 触发排序
        /// </summary>
        /// <param name="name">排序的表格名</param>
        /// <param name="sortName">排序的属性名</param>
        /// <returns></returns>
        protected virtual SortType ChickOrderBy(string name, string sortName) 
        {
            SortList lstSort = new SortList();
            FillOrderBy(name, lstSort);

            
            Sort objSort=lstSort[sortName];
            if (objSort == null)
            {
                objSort = new Sort();
                objSort.PropertyName = sortName;
                objSort.SortType = SortType.ASC;
                lstSort.Add(objSort);
            }
            else
            {
                if (objSort.SortType == SortType.ASC) 
                {
                    objSort.SortType = SortType.DESC;
                }
                else if (objSort.SortType == SortType.DESC) 
                {
                    for (int i = lstSort.Count - 1; i >= 0; i--) 
                    {
                        if (lstSort[i].PropertyName == objSort.PropertyName) 
                        {
                            lstSort.RemoveAt(i);
                        }
                    }
                }
                
            }
            SetOrderByString(name, lstSort);
            return objSort.SortType;
        }



        /// <summary>
        /// 保存设置排序的字符串
        /// </summary>
        /// <param name="name"></param>
        /// <param name="lstSort"></param>
        protected virtual void SetOrderByString(string name, SortList lstSort) 
        {
            string value = GetOrderByString(lstSort);
            ViewState[name + SortViewStateID] = value;
        }

        /// <summary>
        /// 填充排序
        /// </summary>
        /// <param name="lstSort"></param>
        private string GetOrderByString(SortList lstSort)
        {
            StringBuilder sbRet = new StringBuilder();
            foreach (Sort obj in lstSort) 
            {
                sbRet.Append(obj.PropertyName);
                sbRet.Append(' ');
                sbRet.Append((obj.SortType == SortType.ASC)?"asc":"desc");
                sbRet.Append(',');
            }
            if (sbRet.Length > 0) 
            {
                sbRet.Remove(sbRet.Length-1, 1);
            }
            return sbRet.ToString();
        }

        protected virtual void GridViewSorting(object sender, GridViewSortEventArgs e)
        {
            GridView gv = sender as GridView;
            if (gv == null)
            {
                return;
            }
            SortType objSortType = ChickOrderBy(gv.ClientID, e.SortExpression);
            GridBind();
        }

        /// <summary>
        /// 输出绑定
        /// </summary>
        protected virtual void GridBind() 
        {

        }
        #endregion


    }
}
