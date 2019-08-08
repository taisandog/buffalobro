using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Buffalo.WebKernel.WebCommons.PageBases;
using Buffalo.WebKernel.ARTDialog;
using System.Web.UI.WebControls;
using Buffalo.DB.QueryConditions;
using System.Text;
using Buffalo.Kernel;
using Buffalo.DB.CommBase.BusinessBases;
using Buffalo.DB.CommBase;

namespace Models
{
    public class UserPageBase : PageBase<object>
    {
        protected override void OnLoad(EventArgs e)
        {
            object o = ADialog;
            base.OnLoad(e);
        }
        /// <summary>
        /// 设置Gridview的绑定
        /// </summary>
        /// <param name="gv"></param>
        protected void SetGridViewStyle(GridView gv)
        {
            gv.RowDataBound += new GridViewRowEventHandler(gv_RowDataBound);
        }
        void gv_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes.Add("onmouseover", "c=this.style.backgroundColor;this.style.backgroundColor='#c1ebff'");

                e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=c");
            }
        }
        /// <summary>
        /// 获取GridView的数据的排序方式
        /// </summary>
        /// <param name="gv">GridView</param>
        /// <param name="lstSort">排序</param>
        /// <returns></returns>
        protected virtual string GetGridViewOrderString(GridView gv)
        {
            SortList lstSort = new SortList();
            FillOrderBy(gv.ClientID, lstSort);
            StringBuilder sbRet = new StringBuilder();
            foreach (Sort objSort in lstSort)
            {

                string name = objSort.PropertyName;
                foreach (DataControlField col in gv.Columns)
                {
                    if (name == col.SortExpression)
                    {
                        sbRet.Append(col.HeaderText);
                        sbRet.Append("-");
                        sbRet.Append(EnumUnit.GetEnumDescription(objSort.SortType));
                        sbRet.Append(",");
                    }
                }
            }
            if (sbRet.Length > 0)
            {
                sbRet.Remove(sbRet.Length - 1, 1);
            }
            return sbRet.ToString();
        }

        /// <summary>
        /// 填充下拉框
        /// </summary>
        /// <param name="ddl">下拉框</param>
        /// <param name="objEntity">填充的实体</param>
        /// <param name="textPrm">显示的字段</param>
        /// <param name="valuePrm">值字段</param>
        public static void FillDorpDown<P>(DropDownList ddl,
            string textPrm,string valuePrm,bool hasAll) where P:EntityBase,new()
        {
            BusinessModelBaseForSelect<P> bo=new BusinessModelBaseForSelect<P>();
            ScopeList lstScope=new ScopeList();
            List<P> lstEnt = bo.SelectList(lstScope);
            ddl.Items.Clear();
            ListItem item = null;
            if (hasAll) 
            {
                item = new ListItem("[全部]", "");
                ddl.Items.Add(item);
            }
            foreach (P obj in lstEnt) 
            {
                object tmp=obj[textPrm];
                string txt=tmp==null?"":tmp.ToString();
                tmp=obj[valuePrm];
                string val=tmp==null?"":tmp.ToString();
                item = new ListItem(txt, val);
                ddl.Items.Add(item);
            }
        }

        /// <summary>
        /// 位置字符串
        /// </summary>
        public string LocationString
        {
            get
            {
                IModelInfo master = Master as IModelInfo;
                if (master != null)
                {
                    return master.LocationString;
                }
                return "";
            }
            set
            {
                IModelInfo master = Master as IModelInfo;
                if (master != null)
                {
                    master.LocationString = value;
                    Title = value;
                }

            }
        }

        public const string DialogTitle = "蜂背后台管理";

        /// <summary>
        /// 提示框
        /// </summary>
        /// <param name="content"></param>
        public void ArtAlert(string content)
        {
            ADialog.Alert(DialogTitle, content);
        }
        /// <summary>
        /// 警告提示
        /// </summary>
        /// <param name="content"></param>
        public void ArtWarning(string content)
        {
            ADialog.WarningDialog(DialogTitle, content);
        }

        /// <summary>
        /// 成功提示
        /// </summary>
        /// <param name="content"></param>
        public void ArtSuccess(string content)
        {
            ADialog.SuccessDialog(DialogTitle, content);
        }
        /// <summary>
        /// 错误提示
        /// </summary>
        /// <param name="content"></param>
        public void ArtError(string content)
        {
            ADialog.ErrorDialog(DialogTitle, content);
        }
        private ArtDialog _artDialog;
        /// <summary>
        /// ArtDialog对话框
        /// </summary>
        public ArtDialog ADialog
        {
            get
            {
                if (_artDialog == null)
                {
                    _artDialog = new ArtDialog();
                }
                return _artDialog;
            }
        }
    }
}