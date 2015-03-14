using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using Buffalo.WebKernel.WebCommons;
using System.Web.UI;
using System.Drawing.Design;
using System.Web;
using System.IO;

namespace MyCrt
{
    public class MyLabel:System.Web.UI.WebControls.Label
    {
        
        /// <summary>
        /// HTML字符
        /// </summary>
        [
        Category("外观"),
        Description("设置或获取HTML字符")]
        public string InnerText 
        {
            get 
            {
                return Text;
            }
            set 
            {
                Text = WebCommon.HTMLEncode(value);
            }
        }
        /// <summary>
        /// HTML字符
        /// </summary>
        [Editor("System.Web.UI.Design.UrlEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor)), DefaultValue(""), UrlProperty, Bindable(true)]
        public virtual string FileUrl
        {
            get
            {
                string str = ViewState["FileUrl"] as string;
                return str;
                
            }
            set
            {
                this.ViewState["FileUrl"] = value;
                
            }
        }
        public override void RenderControl(HtmlTextWriter writer)
        {
            base.RenderControl(writer);
            if (FileUrl != null)
            {

                Stream stm = OpenFile(FileUrl);
                if (stm == null)
                {
                    this.Text = "空";
                }
                else
                {
                    this.Text = "非空";
                }
            }
        }
        protected override void AddAttributesToRender(HtmlTextWriter writer)
        {
            base.AddAttributesToRender(writer);
            
            
        }

        

 

    }
}
