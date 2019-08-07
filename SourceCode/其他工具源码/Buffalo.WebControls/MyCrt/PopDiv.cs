using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using Buffalo.WebKernel.WebCommons.ContorlCommons;

namespace Buffalo.WebControls.MyCrt
{
    
    [ToolboxData("<{0}:PopDiv runat=server></{0}:PopDiv>")]
    public class PopDiv : Panel
    {
        private const string jsName = "PopDiv.js";
        private static bool isChecked = false;
        
        /// <summary>
        /// 阴影颜色
        /// </summary>
        [Description("阴影颜色"),
       Category("外观"),
        NotifyParentProperty(true)]
        public Color BackgroundColor
        {
            get
            {
                object val = ViewState["BackgroundColor"];
                if (val != null)
                {
                    return (Color)val;
                }
                return Color.Empty;
            }
            set
            {
                ViewState["BackgroundColor"] = value;
            }
        }
        /// <summary>
        /// 阴影的CSS
        /// </summary>
        [Description("阴影的CSS"),
       Category("外观"),
        NotifyParentProperty(true)]
        public string BackgroundClass
        {
            get
            {
                object val = ViewState["BackgroundClass"];
                if (val != null)
                {
                    return val.ToString();
                }
                return "";
            }
            set
            {
                ViewState["BackgroundClass"] = value;
            }
        }
        


        /// <summary>
        ///背景透明度
        /// </summary>
        [Description("背景透明度"),
       Category("外观"),
        NotifyParentProperty(true)
        ]
        public int BackgroupOpacity
        {
            get
            {
                object val = ViewState["BackgroupOpacity"];
                if (val != null)
                {
                    return (int)val;
                }
                return 0;
            }
            set
            {
                ViewState["BackgroupOpacity"] = value;
            }
        }
        /// <summary>
        /// 容器的CSS
        /// </summary>
        [Description("容器的CSS"),
       Category("外观"),
        NotifyParentProperty(true)
        ]
        public string ContainerClass
        {
            get
            {
                object val = ViewState["ContainerClass"];
                if (val != null)
                {
                    return val.ToString();
                }
                return "";
            }
            set
            {
                ViewState["ContainerClass"] = value;
            }
        }

       

        /// <summary>
        /// 标题栏的CSS
        /// </summary>
        [Description("标题栏的CSS"),
       Category("外观"),
       NotifyParentProperty(true)]
        public string TitleClass
        {
            get
            {
                object val = ViewState["TitleClass"];
                if (val != null)
                {
                    return val.ToString();
                }
                return "";
            }
            set
            {
                ViewState["TitleClass"] = value;
            }
        }
        /// <summary>
        /// 关闭按钮的文本
        /// </summary>
        [Description("关闭按钮的文本"),
       Category("外观"),
       NotifyParentProperty(true)]
        public string CloseButtonText
        {
            get
            {
                object val = ViewState["CloseButtonText"];
                if (val != null)
                {
                    return val.ToString();
                }
                return "";
            }
            set
            {
                ViewState["CloseButtonText"] = value;
            }
        }
        /// <summary>
        /// 关闭按钮的CSS
        /// </summary>
        [Description("关闭按钮的CSS"),
       Category("外观"),
        NotifyParentProperty(true)]
        public string CloseButtonClass
        {
            get
            {
                object val = ViewState["CloseButtonClass"];
                if (val != null)
                {
                    return val.ToString();
                }
                return "";
            }
            set
            {
                ViewState["CloseButtonClass"] = value;
            }
        }
        /// <summary>
        /// 标题
        /// </summary>
        [Description("标题"),
       Category("外观"),
        NotifyParentProperty(true)]
        public string Title
        {
            get
            {
                object val = ViewState["Title"];
                if (val != null)
                {
                    return val.ToString();
                }
                return "";
            }
            set
            {
                ViewState["Title"] = value;
            }
        }
        string instanceName = null;

        /// <summary>
        /// JS类的实例名
        /// </summary>
        public string InstanceName 
        {
            get
            {
                if (instanceName == null)
                {
                    instanceName = "obj"+this.ClientID;
                }
                return instanceName;
            }
        }

        /// <summary>
        /// 显示此对话框的JS
        /// </summary>
        public string JSShowDialog
        {
            get
            {
                return InstanceName+".showDiv()";
            }
        }
        /// <summary>
        /// 显示此对话框的JS
        /// </summary>
        public string JSCloseDialog
        {
            get
            {
                return InstanceName + ".hideDiv()";
            }
        }
        

        ///// <summary>
        ///// 创建初始的JS
        ///// </summary>
        ///// <returns></returns>
        private string CreateInitJS()
        {
            StringBuilder js = new StringBuilder(5000);
            instanceName = "obj" + this.ClientID;//JS类的实例名
            js.Append("	var " + InstanceName + "=new PopDiv(\""+this.ClientID+"\");\n");
            return js.ToString();
        }


        protected override void Render(HtmlTextWriter writer)
        {
            string initJs = CreateInitJS();
            Page.ClientScript.RegisterStartupScript(this.GetType(), this.ClientID + "Init", initJs, true);
            float opacity = ((float)BackgroupOpacity / 100f);
            writer.Write("<div id=\"" + this.ClientID + "Back\" style=\" display:none; position:absolute; background-color:" + ContorlCommon.ToColorString(BackgroundColor) + ";filter:alpha(opacity=" + BackgroupOpacity + ");opacity:" + opacity.ToString() + ";\" class=\"" + BackgroundClass + "\"></div>");
            writer.Write("<div id=\"" + this.ClientID + "Link\" style=\"display:none;position:absolute; z-index:10;width:" + this.Width + ";height:" + this.Height + "\" class=\"" + ContainerClass + "\">");
            writer.Write("<div class=\"" + TitleClass + "\"><table style=\"width:100%;boder-width:0px\"><tr><td style=\"text-align:left\">" + Title + "</td><td  style=\"text-align:right\"><input type=\"button\"  value=\"" + CloseButtonText + "\" onclick=\"" + InstanceName + ".hideDiv()\" class=\"" + CloseButtonClass + "\"/></td></tr></table></div>");
            base.Render(writer);
            writer.Write("</div>");
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            SaveJs();
            if (!Page.ClientScript.IsClientScriptIncludeRegistered(jsName + "Include"))
            {
                Page.ClientScript.RegisterClientScriptInclude(jsName + "Include", JsSaver.GetDefualtJsUrl(jsName));
            }
        }

        /// <summary>
        /// 保存成JS文件
        /// </su
        private void SaveJs()
        {
            if (!isChecked)
            {
                JsSaver.SaveJS(jsName, CreateJS());
                isChecked = true;
            }
        }

        private string CreateJS() 
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("function PopDiv(cilentId)\n");
            sb.Append("    {\n");
            sb.Append("        this.cilentId=cilentId;\n");
            sb.Append("        this.hidArr=[];\n");
            sb.Append("    }\n");
            sb.Append("    PopDiv.prototype.showDiv=function()\n");
            sb.Append("	{\n");
            sb.Append("	    var cur=this;\n");
            sb.Append("		this.onShow();\n");
            sb.Append("	}\n");
            sb.Append("	\n");
            sb.Append("	\n");
            sb.Append("	PopDiv.prototype.hidenSelect=function()\n");
            sb.Append("	{\n");
            sb.Append("	    var link=document.getElementById(this.cilentId+\"Link\");\n");
            sb.Append("	   \n");
            sb.Append("	    var sels = document.getElementsByTagName('select'); \n");
            sb.Append("        for (var i = 0; i < sels.length; i++) \n");
            sb.Append("        {\n");
            sb.Append("            if(!isSelectExistsLink(link,sels[i]))\n");
            sb.Append("            {\n");
            sb.Append("                sels[i].style.visibility = 'hidden';\n");
            sb.Append("                this.hidArr[this.hidArr.length]=sels[i];\n");
            sb.Append("            }\n");
            sb.Append("        }\n");
            sb.Append("        \n");
            sb.Append("	}\n");
            sb.Append("	//判断该select是否在Link容器里边\n");
            sb.Append("	function isSelectExistsLink(canLink,sel)\n");
            sb.Append("	{\n");
            sb.Append("	     var noHiddenSels=canLink.getElementsByTagName('select'); \n");
            sb.Append("	     for(var k=0;k<noHiddenSels.length;k++)//如果不在link内的select才要隐藏\n");
            sb.Append("	     {\n");
            sb.Append("            if(sel==noHiddenSels[k])\n");
            sb.Append("            {\n");
            sb.Append("                return true;\n");
            sb.Append("            }\n");
            sb.Append("         }\n");
            sb.Append("         return false;\n");
            sb.Append("	}\n");
            sb.Append("	\n");
            sb.Append("	function isIE()\n");
            sb.Append("	{\n");
            sb.Append("	   var navigatorName = \"Microsoft Internet Explorer\";   \n");
            sb.Append("       var ie = false;   \n");
            sb.Append("       if( navigator.appName == navigatorName )\n");
            sb.Append("       {   \n");
            sb.Append("        ie = true;       \n");
            sb.Append("       }  \n");
            sb.Append("       return ie;\n");
            sb.Append("	}\n");
            sb.Append("	\n");
            sb.Append("	PopDiv.prototype.onShow=function()\n");
            sb.Append("	{\n");
            sb.Append("	    \n");
            sb.Append("		//document.documentElement.style.overflow='hidden';\n");
            sb.Append("		var back=document.getElementById(this.cilentId+\"Back\");\n");
            sb.Append("	    var link=document.getElementById(this.cilentId+\"Link\");\n");
            sb.Append("		link.style.display=\"\";\n");
            sb.Append("		reset(link,back);\n");
            sb.Append("		window.onresize=function()\n");
            sb.Append("		{\n");
            sb.Append("		    if (back.style.display == \"none\")\n");
            sb.Append("		    {\n");
            sb.Append("		        return;\n");
            sb.Append("		    }\n");
            sb.Append("         reset(link,back);\n");
            sb.Append("		}\n");
            sb.Append("        window.onscroll=function()\n");
            sb.Append("		{\n");
            sb.Append("		    if (back.style.display == \"none\")\n");
            sb.Append("		    {\n");
            sb.Append("		        return;\n");
            sb.Append("		    }\n");
            sb.Append("         reset(link,back);\n");
            sb.Append("		}\n");
            sb.Append("//		link.style.top=document.documentElement.scrollTop+((window.screen.availHeight-info.height)/2)-100+\"px\";\n");
            sb.Append("//		link.style.left=document.documentElement.scrollLeft+(window.screen.availWidth-info.width)/2+\"px\";\n");
            sb.Append("		if(isIE())\n");
            sb.Append("		{\n");
            sb.Append("		    this.hidenSelect();\n");
            sb.Append("		}\n");
            sb.Append("		//alert(\"SelectAddImage.aspx?addtion=\"+addtion);\n");
            sb.Append("	}\n");
            sb.Append("	\n");
            sb.Append("	function reset(div,back)   \n");
            sb.Append("    {   \n");
            sb.Append("        \n");
            sb.Append("	    back.style.display=\"none\";\n");
            sb.Append("          var   doc   =   document.documentElement;   \n");
            sb.Append("          //     首先取得页面现在的左上角相对位置   \n");
            sb.Append("          var   x1   =   doc.scrollLeft;   \n");
            sb.Append("          var   y1   =   doc.scrollTop;\n");
            sb.Append("          var   w1   =   doc.clientWidth;   \n");
            sb.Append("          var   h1   =   doc.clientHeight;   \n");
            sb.Append("            \n");
            sb.Append("          //     取得浮动层的信息   \n");
            sb.Append("          var   divWidth   =   div.offsetWidth||div.clientWidth;\n");
            sb.Append("          var   divHeight   =   div.offsetHeight||div.clientHeight;  \n");
            sb.Append("          var   divX   =   Math.ceil((w1   -   divWidth)/2)   +   x1;   \n");
            sb.Append("          var   divY   =   Math.ceil((h1   -   divHeight)/2)   +   y1;   \n");
            sb.Append("          \n");
            sb.Append("          if(divX<0)\n");
            sb.Append("          {\n");
            sb.Append("            divX=0;\n");
            sb.Append("          }\n");
            sb.Append("          \n");
            sb.Append("          //     设置位置   \n");
            sb.Append("\n");
            sb.Append("        setAbsoluteLocation(div,divY,divX);\n");
            sb.Append("        back.style.display=\"\";\n");
            sb.Append("		back.style.top=\"0px\";\n");
            sb.Append("		back.style.left=\"0px\";\n");
            sb.Append("		back.style.height=document.documentElement.clientHeight+\"px\";\n");
            sb.Append("		back.style.width=document.documentElement.clientWidth+\"px\";\n");
            sb.Append("    }  \n");
            sb.Append("    \n");
            sb.Append("	function setAbsoluteLocation(div,top,left) \n");
            sb.Append("    {\n");
            sb.Append("        var element=div;\n");
            sb.Append("        \n");
            sb.Append("        while(element = element.offsetParent)\n");
            sb.Append("        {\n");
            sb.Append("             if(element.tagName!=null)\n");
            sb.Append("             {\n");
            sb.Append("                 if(element.tagName.toLowerCase()==\"div\" || element.tagName.toLowerCase()==\"span\")\n");
            sb.Append("                 {\n");
            sb.Append("                     top -= element.offsetTop;\n");
            sb.Append("                     left -= element.offsetLeft;\n");
            sb.Append("                 }\n");
            sb.Append("             }\n");
            sb.Append("        }\n");
            sb.Append("        \n");
            sb.Append("        div.style.left=left+\"px\";\n");
            sb.Append("        div.style.top=top+\"px\";\n");
            sb.Append("    }\n");
            sb.Append("    \n");
            sb.Append("	PopDiv.prototype.showSelect=function()\n");
            sb.Append("	{\n");
            sb.Append("	    for (var i = 0; i < this.hidArr.length; i++) \n");
            sb.Append("        {\n");
            sb.Append("            this.hidArr[i].style.visibility = 'visible';\n");
            sb.Append("        }\n");
            sb.Append("        this.hidArr.length=0;\n");
            sb.Append("	}	\n");
            sb.Append("	PopDiv.prototype.hideDiv=function()\n");
            sb.Append("	{\n");
            sb.Append("	    var back=document.getElementById(this.cilentId+\"Back\");\n");
            sb.Append("	    var link=document.getElementById(this.cilentId+\"Link\");\n");
            sb.Append("		document.documentElement.style.overflow='';\n");
            sb.Append("		back.style.display=\"none\";\n");
            sb.Append("		link.style.display=\"none\";\n");
            sb.Append("		window.onresize=null;\n");
            sb.Append("		if(isIE())\n");
            sb.Append("		{\n");
            sb.Append("		    this.showSelect();\n");
            sb.Append("		}\n");
            sb.Append("	}\n");
            sb.Append("\n");
            sb.Append("\n");
            sb.Append("\n");
            sb.Append("\n");
            return sb.ToString();
        }
        public PopDiv()
        {
            BackgroundColor = Color.Gray;
            BackgroupOpacity = 50;
            CloseButtonText = "X";
        }
    }
}
