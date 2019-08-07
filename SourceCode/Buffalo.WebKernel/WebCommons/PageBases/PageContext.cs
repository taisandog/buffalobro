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
        /// ֱ�Ӱ����������ҳ��
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
        /// ���ı���ؼ����keydown�¼�
        /// </summary>
        /// <param name="ctl">�ؼ�</param>
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
        /// ��ҳ��ע��JS�ű�
        /// </summary>
        /// <param name="jsName">JS����</param>
        /// <param name="root">JS·��</param>
        public static void LoadJsBlock(string jsName, string root)
        {
            Page page = (Page)System.Web.HttpContext.Current.Handler;
            if (!page.ClientScript.IsClientScriptBlockRegistered(jsName))
            {
                page.ClientScript.RegisterClientScriptBlock(page.GetType(), jsName, " <script src=\"" + root + "\" language=\"JavaScript\" type=\"text/javascript\"></script>");
            }
        }

        /// <summary>
        /// ���ñ�ҳ���Ĭ���ύ�ؼ�
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
        /// ����ָ��·����CSS
        /// </summary>
        /// <param name="cssUrl">·��</param>
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
        /// ����Ի���
        /// </summary>
        /// <param name="message">��Ҫ�������Ϣ</param>
        /// <param name="isShowInLoadFinish">�Ƿ���ҳ��������ʱ���ٵ���</param>
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
        /// ����Ի���
        /// </summary>
        /// <param name="message">��Ҫ�������Ϣ</param>
        public static void Alert(string message)
        {
            Alert(message, false);
        }

        ///// <summary>
        ///// ����Ի���(for AJAX)
        ///// </summary>
        ///// <param name="message">��Ҫ�������Ϣ</param>
        //public void Alert2(string message) 
        //{
        //    message = message.Replace("\n", "\\n");
        //    ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), message, "alert('"+message+"')", true);
        //}

        ///// <summary>
        ///// �ض���ҳ��(for AJAX)
        ///// </summary>
        ///// <param name="url">�����ҳ��</param>
        //public void JSRedirect2(string url)
        //{
        //    ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), url, "window.location='" + url + "';", true);
        //}

        /// <summary>
        /// �ض���ҳ��
        /// </summary>
        /// <param name="url">�����ҳ��</param>
        public static void JSRedirect(string url)
        {
            Page page = (Page)System.Web.HttpContext.Current.Handler;
            page.ClientScript.RegisterClientScriptBlock(System.Web.HttpContext.Current.Handler.GetType(), url, "window.location='" + url + "';", true);
        }

        /// <summary>
        /// ���ҳ�������е��ı�
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
        /// ҳ����������·�
        /// </summary>
        public static void ScrollToEnd()
        {
            Page page = (Page)System.Web.HttpContext.Current.Handler;
            page.ClientScript.RegisterStartupScript(System.Web.HttpContext.Current.Handler.GetType(), "scroll", "window.scrollTo(0, document.body.scrollHeight);", true);
        }

        


        /// <summary>
        /// ��ǰ�û��ĵ�½��ַ
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
        ///// ����ҳ����Ϣ��ʵ��
        ///// </summary>
        ///// <param name="objPage">ҳ��</param>
        ///// <param name="modle">ʵ��</param>
        ///// <param name="pType">��������</param>
        //public static void UpdateModel(Control objControl, object modle, PrefixType pType)
        //{
        //    WebModel.UpdateModel(objControl, modle, pType);
        //}
        ///// <summary>
        ///// ����ҳ����Ϣ��ʵ��(Ĭ��ǰ׺��Сд)
        ///// </summary>
        ///// <param name="objPage">ҳ��</param>
        ///// <param name="modle">ʵ��</param>
        //public static void UpdateModel(Control objControl, object modle)
        //{
        //    WebModel.UpdateModel(objControl, modle, PrefixType.Camel);
        //}

        ///// <summary>
        ///// ����ҳ����Ϣ��ʵ��
        ///// </summary>
        ///// <param name="objPage">ҳ��</param>
        ///// <param name="modle">ʵ��</param>
        ///// <param name="pType">��������</param>
        //public void UpdateModel(object modle, PrefixType pType)
        //{
        //    WebModel.UpdateModel(Page, modle, pType);
        //}
        ///// <summary>
        ///// ����ҳ����Ϣ��ʵ��(Ĭ��ǰ׺��Сд)
        ///// </summary>
        ///// <param name="objPage">ҳ��</param>
        ///// <param name="modle">ʵ��</param>
        //public void UpdateModel(object modle)
        //{
        //    WebModel.UpdateModel(Page, modle, PrefixType.Camel);
        //}



        #region �������

        protected const string SortViewStateID="$SortViewID";

        /// <summary>
        /// �������
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
        /// ��������
        /// </summary>
        /// <param name="name">����ı����</param>
        /// <param name="sortName">�����������</param>
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
        /// ��������������ַ���
        /// </summary>
        /// <param name="name"></param>
        /// <param name="lstSort"></param>
        protected virtual void SetOrderByString(string name, SortList lstSort) 
        {
            string value = GetOrderByString(lstSort);
            ViewState[name + SortViewStateID] = value;
        }

        /// <summary>
        /// �������
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
        /// �����
        /// </summary>
        protected virtual void GridBind() 
        {

        }
        #endregion


    }
}
