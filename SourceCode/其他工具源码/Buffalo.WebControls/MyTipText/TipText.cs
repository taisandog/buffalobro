using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using Buffalo.WebKernel.WebCommons.ContorlCommons;
using Buffalo.WebKernel.WebCommons;

namespace MyTipText
{
    [DefaultProperty("Text")]
    [ToolboxData("<{0}:TipText runat=server></{0}:TipText>")]
    public class TipText : System.Web.UI.WebControls.TextBox
    {
        //private TipItemInfo tipItemInfo=new TipItemInfo();
        //private ContainerInfo containerInfo=new ContainerInfo();
        //private ListItemCollection tipItems=new ListItemCollection();
        private static bool isChecked = false;//记录是否已经检查过JS文件是否存在
        private const string jsName = "TipText.js";
        /// <summary>
        /// 获取提示子项的外观信息
        /// </summary>
        [Description("获取提示子项的外观信息"),
       Category("外观"),
        NotifyParentProperty(true),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
        PersistenceMode(PersistenceMode.InnerProperty)]
        public TipItemInfo ChildItemInfo 
        {
            get 
            {
                object info=ViewState["tipItemInfo"];
                if (info == null)
                {
                    info = new TipItemInfo();
                    ViewState["tipItemInfo"] = info;
                }
                
                return (TipItemInfo)info;
            }
        }

        /// <summary>
        /// 获取提示容器的外观信息
        /// </summary>
        [Description("获取提示容器的外观信息"),
       Category("外观"),
        NotifyParentProperty(true),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
        PersistenceMode(PersistenceMode.InnerProperty)]
        public ContainerInfo ContainerStyleInfo
        {
            get
            {
                object info = ViewState["containerInfo"];
                if (info == null)
                {
                    info = new ContainerInfo();
                    ViewState["containerInfo"] = info;
                }
                
                return (ContainerInfo)info;
            }
        }

        /// <summary>
        /// 提示的项集合
        /// </summary>
        [Description("提示的项集合"),
       Category("外观"),
        NotifyParentProperty(true),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
      PersistenceMode(PersistenceMode.InnerProperty), TypeConverter(typeof(System.ComponentModel.ArrayConverter))]
        public TipItemCollection TipItems
        {
            get
            {
                object info = ViewState["tipItems"];
                if (info == null)
                {
                    info = new TipItemCollection();
                    ViewState["tipItems"] = info;
                }

                return (TipItemCollection)info;
            }
        }

        string instanceName = null;//JS类的实例名
        string divId  = null;//容器的ID
        string txtId = null;//控件的ID

        public TipText() 
        {
            InitTipStyle();

        }

        protected override void OnDataBinding(EventArgs e)
        {
            base.OnDataBinding(e);
            
        }

        

        protected override void Render(HtmlTextWriter writer)
        {
            base.Render(writer);
            //instanceName = "obj" + this.ClientID;//JS类的实例名
            divId = "div" + this.ClientID;
            txtId = this.ClientID;
            string initJs = CreateInitJS();
            
            Page.ClientScript.RegisterStartupScript(this.GetType(), this.ClientID + "Init", initJs, true);
            
        }
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            SaveJs();
            if (!Page.ClientScript.IsClientScriptIncludeRegistered(jsName + "Include"))
            {
                Page.ClientScript.RegisterClientScriptInclude(jsName + "Include", JsSaver.GetDefualtJsUrl(jsName));
            }
            instanceName = "obj" + this.ClientID;//JS类的实例名
            this.Attributes.Add("onclick", instanceName + ".show()");
            this.Attributes.Add("onkeydown", instanceName + ".keyPressListener(event)");
            this.Attributes.Add("onkeyup", instanceName + ".onKeyUp(event)");
            this.Attributes.Add("onBlur", instanceName + ".focusout(event)");
            this.Attributes.Add("autocomplete", "off");
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
        /// <summary>
        /// 初始化提示标签的外观
        /// </summary>
        private void InitTipStyle() 
        {
            ContainerStyleInfo.ContainerHeight = 100;
        }
        ///// <summary>
        ///// 创建初始的JS
        ///// </summary>
        ///// <returns></returns>
        private string CreateInitJS()
        {
            StringBuilder js = new StringBuilder(5000);
            js.Append("var backColor=\""+ContorlCommon.ToColorString(ChildItemInfo.BackColor)+"\";\n");
            js.Append("	var fontColor=\"" + ContorlCommon.ToColorString(ChildItemInfo.FontColor) + "\";\n");
            js.Append("	var shadowColor=\"" + ContorlCommon.ToColorString(ChildItemInfo.ShadowColor) + "\";\n");
            js.Append("	var shadowFontColor=\"" + ContorlCommon.ToColorString(ChildItemInfo.ShadowFontColor) + "\";\n");
            js.Append("\n");
            js.Append("	var divId=\""+divId+"\";\n");
            js.Append("	var txtId=\""+txtId+"\";\n");
            js.Append("	var divHeight="+ContainerStyleInfo.ContainerHeight.ToString()+";\n");
            js.Append("	var instanceName=\"" + instanceName + "\";\n");
            js.Append("	var cssItem=\"" + ChildItemInfo.ItemClassName + "\";\n");
            js.Append("	var cssSelectedItem=\"" + ChildItemInfo.SelectedClassName + "\"\n");
            js.Append("	var arr=new Array(" + CreateItemArray()+ ");\n");
            
            js.Append("	\n");
            string css = "";
            if (ContainerStyleInfo.ClassName != "" && ContainerStyleInfo != null) 
            {
                css = "class=" + ContainerStyleInfo.ClassName;
            }
            js.Append("	document.write(\"<div style=\\\"position:absolute;\\\" " + css + " id=\\\"" + divId + "\\\"></div>\");\n");
            js.Append("	var " + instanceName + "=new TextTip(backColor,fontColor,shadowColor,shadowFontColor,divId,txtId,divHeight,cssItem,cssSelectedItem,arr,instanceName);\n");
            return js.ToString();
        }

        /// <summary>
        /// 创建选择项的数组
        /// </summary>
        /// <returns></returns>
        private string CreateItemArray() 
        {
            StringBuilder js = new StringBuilder(5000);
            TipItemCollection itemColl=TipItems;
            for (int i = 0; i < itemColl.Count; i++) 
            {
                TipItem item = itemColl[i];
                string strComma = ",";
                if (i >= itemColl.Count - 1) //如果是最后一项就不用加逗号
                {
                    strComma = "";
                }
                js.Append("{name:\"" + WebCommon.HTMLEncode(item.Text) + "\",value:\"" + WebCommon.HTMLEncode(item.Value) + "\"}" + strComma + "\n");
            }
            return js.ToString();
        }

        /// <summary>
        /// 生成必要的JS文件
        /// </summary>
        /// <returns></returns>
        private string CreateJS() 
        {
            StringBuilder js = new StringBuilder(7000);
            js.Append("\n");
            js.Append("function getElementPos(element) { \n");
            js.Append("    var ua = navigator.userAgent.toLowerCase(); \n");
            js.Append("    var isOpera = (ua.indexOf('opera') != -1); \n");
            js.Append("    var isIE = (ua.indexOf('msie') != -1 && !isOpera); // not opera spoof \n");
            js.Append(" \n");
            js.Append("    var el = element; \n");
            js.Append(" \n");
            js.Append("    if(el.parentNode === null || el.style.display == 'none')  \n");
            js.Append("    { \n");
            js.Append("        return false; \n");
            js.Append("    } \n");
            js.Append(" \n");
            js.Append("    var parent = null; \n");
            js.Append("    var pos = []; \n");
            js.Append("    var box; \n");
            js.Append(" \n");
            js.Append("    if(el.getBoundingClientRect)    //IE \n");
            js.Append("    { \n");
            js.Append("        box = el.getBoundingClientRect(); \n");
            js.Append("        var scrollTop = Math.max(document.documentElement.scrollTop, document.body.scrollTop); \n");
            js.Append("        var scrollLeft = Math.max(document.documentElement.scrollLeft, document.body.scrollLeft); \n");
            js.Append(" \n");
            js.Append("        return {x:box.left + scrollLeft, y:box.top + scrollTop}; \n");
            js.Append("    } \n");
            js.Append("    else if(document.getBoxObjectFor)    // gecko \n");
            js.Append("    { \n");
            js.Append("        box = document.getBoxObjectFor(el); \n");
            js.Append("            \n");
            js.Append("        var borderLeft = (el.style.borderLeftWidth)?parseInt(el.style.borderLeftWidth):0; \n");
            js.Append("        var borderTop = (el.style.borderTopWidth)?parseInt(el.style.borderTopWidth):0; \n");
            js.Append(" \n");
            js.Append("        pos = [box.x - borderLeft, box.y - borderTop]; \n");
            js.Append("    } \n");
            js.Append("    else    // safari & opera \n");
            js.Append("    { \n");
            js.Append("        pos = [el.offsetLeft, el.offsetTop]; \n");
            js.Append("        parent = el.offsetParent; \n");
            js.Append("        if (parent != el) { \n");
            js.Append("            while (parent) { \n");
            js.Append("                pos[0] += parent.offsetLeft; \n");
            js.Append("                pos[1] += parent.offsetTop; \n");
            js.Append("                parent = parent.offsetParent; \n");
            js.Append("            } \n");
            js.Append("        } \n");
            js.Append("        if (ua.indexOf('opera') != -1  \n");
            js.Append("            || ( ua.indexOf('safari') != -1 && el.style.position == 'absolute' ))  \n");
            js.Append("        { \n");
            js.Append("                pos[0] -= document.body.offsetLeft; \n");
            js.Append("                pos[1] -= document.body.offsetTop; \n");
            js.Append("        }  \n");
            js.Append("    } \n");
            js.Append("         \n");
            js.Append("    if (el.parentNode) { parent = el.parentNode; } \n");
            js.Append("    else { parent = null; } \n");
            js.Append("   \n");
            js.Append("    while (parent && parent.tagName != 'BODY' && parent.tagName != 'HTML')  \n");
            js.Append("    { // account for any scrolled ancestors \n");
            js.Append("        pos[0] -= parent.scrollLeft; \n");
            js.Append("        pos[1] -= parent.scrollTop; \n");
            js.Append("   \n");
            js.Append("        if (parent.parentNode) { parent = parent.parentNode; }  \n");
            js.Append("        else { parent = null; } \n");
            js.Append("    } \n");
            js.Append("    return {x:pos[0], y:pos[1]}; \n");
            js.Append("} \n");
            js.Append(" \n");
            js.Append(" 	function TextTip(backColor,fontColor,shadowColor,shadowFontColor,divId,txtId,divHeight,cssItem,cssSelectedItem,arr,instanceName)\n");
            js.Append(" 	{\n");
            js.Append(" 		this.backColor=backColor;\n");
            js.Append("		this.fontColor=fontColor;\n");
            js.Append("		this.shadowColor=shadowColor;\n");
            js.Append("		this.shadowFontColor=shadowFontColor;\n");
            js.Append("		this.divId=divId;\n");
            js.Append("		this.txtId=txtId;\n");
            js.Append("		this.divHeight=divHeight;\n");
            js.Append("		this.arr=arr;\n");
            js.Append("		this.curDiv=null;\n");
            js.Append("		this.cssItem=cssItem;\n");
            js.Append("		this.instanceName=instanceName;\n");
            js.Append("		this.cssSelectedItem=cssSelectedItem;\n");
            js.Append("		this.isOnContainer=false;\n");
            js.Append("		this.initContainer();\n");
            js.Append("		this.timer=null;\n");
            js.Append("	}\n");
            js.Append("	\n");
            js.Append("	\n");
            js.Append("	TextTip.prototype.initContainer=function()\n");
            js.Append("	{\n");
            js.Append("		var div=document.getElementById(this.divId);\n");
            js.Append("		var txt=document.getElementById(this.txtId);\n");
            js.Append("\n");
            js.Append("		var instance=this;\n");
            js.Append("		div.onmouseover=function()\n");
            js.Append("		{\n");
            js.Append("			instance.isOnContainer=true;\n");
            js.Append("		}\n");
            js.Append("		div.onmouseout=function()\n");
            js.Append("		{\n");
            js.Append("			instance.isOnContainer=false;\n");
            js.Append("		}\n");
            js.Append("		div.onscroll=function()\n");
            js.Append("		{\n");
            js.Append("			document.getElementById(instance.txtId).focus();\n");
            js.Append("		}\n");
            js.Append("		var cssWidth=divInfo(div).width;\n");
            js.Append("		if(cssWidth==0 || cssWidth==\"auto\")\n");
            js.Append("		{\n");
            js.Append("			div.style.width=txt.clientWidth+\"px\";\n");
            js.Append("		}\n");
            js.Append("		\n");
            js.Append("		div.style.overflow=\"auto\";\n");
            js.Append("		div.style.overflowX=\"hidden\";\n");
            js.Append("		div.style.display=\"none\";\n");
            js.Append("	}\n");
            js.Append("\n");
            js.Append("	\n");
            js.Append("	TextTip.prototype.show=function()\n");
            js.Append("	{\n");
            js.Append("		\n");
            js.Append("     if (this.arr.length <= 0) \n");
            js.Append("     {\n");
            js.Append("	        return;\n");
            js.Append("     }\n");
            js.Append("		var div=document.getElementById(this.divId);\n");
            js.Append("		var txt=document.getElementById(this.txtId);\n");
            js.Append("		var loc=getElementPos(txt);\n");
            js.Append("		div.style.left=loc.x+\"px\";\n");
            js.Append("		div.style.top=loc.y+txt.clientHeight+2+\"px\";\n");
            js.Append("		div.style.display=\"\";\n");
            js.Append("		\n");
            js.Append("		\n");
            js.Append("		this.loadDivs();\n");
            js.Append("	}\n");
            js.Append("	\n");
            js.Append("	TextTip.prototype.onKeyUp=function(e)\n");
            js.Append("	{\n");
            js.Append("		var eve=window.event||e;\n");
            js.Append("		var keycode=eve.keyCode;\n");
            js.Append("		if((keycode>=13&&keycode<=18)||(keycode>=37&&keycode<=40))\n");
            js.Append("		{\n");
            js.Append("			return;\n");
            js.Append("		}\n");
            js.Append("		this.loadDivs();\n");
            js.Append("	}\n");
            js.Append("	\n");
            js.Append("	function divInfo(div)//get div in css info\n");
            js.Append("	{\n");
            js.Append("		var divCssHeight=null;\n");
            js.Append("		var divCssWidth=null;\n");
            js.Append("		if(div.currentStyle!=null)\n");
            js.Append("		{\n");
            js.Append("			divCssHeight=div.currentStyle.height;\n");
            js.Append("			divCssWidth=div.currentStyle.width;\n");
            js.Append("		}else\n");
            js.Append("		{\n");
            js.Append("			divCssHeight=div.clientHeight;\n");
            js.Append("			divCssWidth=div.clientWidth;\n");
            js.Append("		}\n");
            js.Append("		return {width:divCssWidth,height:divCssHeight}\n");
            js.Append("	}\n");
            js.Append("	\n");
            js.Append("	TextTip.prototype.loadDivs=function()\n");
            js.Append("	{\n");
            js.Append("		var div=document.getElementById(this.divId);\n");
            js.Append("		var txt=document.getElementById(this.txtId);\n");
            js.Append("		div.innerHTML=\"\";\n");
            js.Append("		div.style.height=null;\n");
            js.Append("		var html=\"\";\n");
            js.Append("		var cssStr=\"\";\n");
            js.Append("		if(this.cssItem!=null || this.cssItem!=\"\")\n");
            js.Append("		{\n");
            js.Append("			var cssStr=\" class=\\\"\"+this.cssItem+\"\\\"\";\n");
            js.Append("		}\n");
            js.Append("		for(var i=0;i<this.arr.length;i++)\n");
            js.Append("		{\n");
            js.Append("			if(this.arr[i].value.indexOf(txt.value)==0)\n");
            js.Append("			{\n");
            js.Append("				html+=\"<div \"+cssStr+\" style=\\\"cursor:pointer;color:\"+this.fontColor+\";\"+\n");
            js.Append("				this.backColor+\"\\\" onmousedown=\\\"\"+\n");
            js.Append("				this.instanceName+\".selectItemValue(this)\\\" onmouseover=\\\"\"+\n");
            js.Append("				this.instanceName+\".selectItem(this)\\\">\"+\n");
            js.Append("				\"<input type=\\\"hidden\\\" value=\\\"\"+this.arr[i].value+\"\\\"/>\"+\n");
            js.Append("				this.arr[i].name+\"</div>\";\n");
            js.Append("			}\n");
            js.Append("		}\n");
            js.Append("		var divCssHeight=divInfo(div).height;\n");
            js.Append("		div.innerHTML=html;\n");
            js.Append("		//set the container height\n");
            js.Append("		\n");
            js.Append("		if(divCssHeight==\"auto\"||divCssHeight==0)\n");
            js.Append("		{\n");
            js.Append("			var cHeight=this.containerHeight();\n");
            js.Append("			if(cHeight>this.divHeight)\n");
            js.Append("			{\n");
            js.Append("				div.style.height=this.divHeight+\"px\";\n");
            js.Append("			}		\n");
            js.Append("		}\n");
            js.Append("		this.curDiv=null;\n");
            js.Append("	}\n");
            js.Append("	\n");
            js.Append("	TextTip.prototype.containerHeight=function()\n");
            js.Append("	{\n");
            js.Append("		var ret=0;\n");
            js.Append("		var nods=document.getElementById(this.divId).childNodes;\n");
            js.Append("		for(var i=0;i<nods.length;i++)\n");
            js.Append("		{\n");
            js.Append("			ret+=nods[i].offsetHeight;\n");
            js.Append("		}\n");
            js.Append("		return ret;\n");
            js.Append("	}\n");
            js.Append("	\n");
            js.Append("	TextTip.prototype.autoScroll=function()\n");
            js.Append("	{\n");
            js.Append("		if(this.curDiv!=null)\n");
            js.Append("		{\n");
            js.Append("			var index=this.findItemIndex(this.curDiv);\n");
            js.Append("			var divHeight=document.getElementById(this.divId).clientHeight\n");
            js.Append("			var height=this.curDiv.offsetHeight*(index+1);//total height\n");
            js.Append("			var scrollHeight=height-this.divHeight;\n");
            js.Append("			if(scrollHeight>document.getElementById(this.divId).scrollTop)\n");
            js.Append("			{\n");
            js.Append("				document.getElementById(this.divId).scrollTop=scrollHeight;\n");
            js.Append("			}\n");
            js.Append("			else if(document.getElementById(this.divId).scrollTop>scrollHeight+this.divHeight-this.curDiv.offsetHeight)\n");
            js.Append("			{\n");
            js.Append("				document.getElementById(this.divId).scrollTop=scrollHeight+this.divHeight-this.curDiv.offsetHeight;\n");
            js.Append("			}\n");
            js.Append("		}\n");
            js.Append("	}\n");
            js.Append("	\n");
            js.Append("	TextTip.prototype.resetChildsStyle=function()//reset child divs style\n");
            js.Append("	{\n");
            js.Append("		var nods=document.getElementById(this.divId).childNodes;\n");
            js.Append("		for(var i=0;i<nods.length;i++)\n");
            js.Append("		{\n");
            js.Append("			nods[i].style.backgroundColor=this.backColor;\n");
            js.Append("			nods[i].style.color=this.fontColor;\n");
            js.Append("			if(this.cssItem!=null || this.cssItem!=\"\")\n");
            js.Append("			{\n");
            js.Append("				nods[i].className=this.cssItem;\n");
            js.Append("			}\n");
            js.Append("		}\n");
            js.Append("	}\n");
            js.Append("		\n");
            js.Append("	TextTip.prototype.selectItem=function(div)\n");
            js.Append("	{\n");
            js.Append("		this.resetChildsStyle();\n");
            js.Append("		div.style.backgroundColor=this.shadowColor;\n");
            js.Append("		div.style.color=this.shadowFontColor;\n");
            js.Append("		if(this.cssSelectedItem!=null || this.cssSelectedItem!=\"\")\n");
            js.Append("		{\n");
            js.Append("			div.className=this.cssSelectedItem;\n");
            js.Append("		}\n");
            js.Append("		this.curDiv=div;\n");
            js.Append("	}\n");
            js.Append("	\n");
            js.Append("function getDivValue(div)\n");
            js.Append("	{\n");
            js.Append("		var nodes=div.childNodes;\n");
            js.Append("		for(var i=0;i<nodes.length;i++)\n");
            js.Append("		{\n");
            js.Append("			var curNode=nodes[i];\n");
            js.Append("			if(curNode.tagName!=null&&curNode.tagName.toLowerCase()==\"input\")\n");
            js.Append("			{\n");
            js.Append("				if(curNode.type!=null&&curNode.type.toLowerCase()==\"hidden\")\n");
            js.Append("				{\n");
            js.Append("					return curNode.value;\n");
            js.Append("				}\n");
            js.Append("			}\n");
            js.Append("		}\n");
            js.Append("		return \"\";\n");
            js.Append("	}\n");
            js.Append("	TextTip.prototype.selectItemValue=function(div)//selectValue\n");
            js.Append("	{\n");
            js.Append("		\n");
            js.Append("		document.getElementById(this.txtId).value=getDivValue(div);\n");
            js.Append("		document.getElementById(this.divId).style.display=\"none\";\n");
            js.Append("		this.curDiv=null;\n");
            js.Append("	}\n");
            js.Append("	\n");
            js.Append("	//find the index of current item in the items\n");
            js.Append("	TextTip.prototype.findItemIndex=function(curDiv)\n");
            js.Append("	{\n");
            js.Append("		if(curDiv==null)\n");
            js.Append("		{\n");
            js.Append("			return -1;\n");
            js.Append("		}\n");
            js.Append("		var nods=document.getElementById(this.divId).childNodes;\n");
            js.Append("		for(var i=0;i<nods.length;i++)\n");
            js.Append("		{\n");
            js.Append("			if(curDiv==nods[i])\n");
            js.Append("			{\n");
            js.Append("				return i;\n");
            js.Append("			}\n");
            js.Append("		}\n");
            js.Append("		return -1;\n");
            js.Append("	}\n");
            js.Append("	TextTip.prototype.keyPressListener=function(e)\n");
            js.Append("	{\n");
            js.Append("		var eve= window.event||e;\n");
            js.Append("		var keycode=eve.keyCode;\n");
            js.Append("		var div=document.getElementById(this.divId);\n");
            js.Append("		var nods=div.childNodes;\n");
            js.Append("		if(nods.length<=0)\n");
            js.Append("		{\n");
            js.Append("			return;\n");
            js.Append("		}\n");
            js.Append("		var index=this.findItemIndex(this.curDiv);\n");
            js.Append("		if(keycode==38)\n");
            js.Append("		{\n");
            js.Append("			index--;\n");
            js.Append("			if(index<0)\n");
            js.Append("			{\n");
            js.Append("				index=0;\n");
            js.Append("			}\n");
            js.Append("			this.selectItem(nods[index]);\n");
            js.Append("			this.autoScroll();\n");
            js.Append("		}\n");
            js.Append("		else if(keycode==40)\n");
            js.Append("		{\n");
            js.Append("			index++;\n");
            js.Append("			if(index<0 || index>nods.length-1)\n");
            js.Append("			{\n");
            js.Append("				return;\n");
            js.Append("			}\n");
            js.Append("			this.selectItem(nods[index]);\n");
            js.Append("			this.autoScroll();\n");
            js.Append("		}\n");
            js.Append("		else if(keycode==13)\n");
            js.Append("		{\n");
            js.Append("			if(this.curDiv!=null)\n");
            js.Append("			{\n");
            js.Append("				this.selectItemValue(this.curDiv);\n");
            js.Append("			}\n");
            js.Append("			if(window.event)\n");
            js.Append("			{\n");
            js.Append("				eve.returnValue=false; //cancel Enter action\n");
            js.Append("			}else\n");
            js.Append("			{\n");
            js.Append("				eve.preventDefault();\n");
            js.Append("			}\n");
            js.Append("		}\n");
            js.Append("	}\n");
            js.Append("	\n");
            js.Append("	TextTip.prototype.focusout=function()\n");
            js.Append("	{		if(!this.isOnContainer)\n");
            js.Append("		{\n");
            js.Append("			document.getElementById(this.divId).style.display=\"none\";\n");
            js.Append("			this.curDiv=null\n");
            js.Append("		}\n");
            js.Append("	}\n");
            js.Append("		\n");
            js.Append("	\n");
            return js.ToString();
        }
    }
}
