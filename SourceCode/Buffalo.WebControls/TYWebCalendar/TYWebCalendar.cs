using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;
using System.Text;
using System.Drawing;
using System.Web;
using System.IO;
using Buffalo.WebKernel.WebCommons.ContorlCommons;
namespace TYInfoTechCalendar
{
    /// <summary>
    /// TYWebCalendar1.1
    /// </summary>
    [DefaultProperty("CurrentDate"),
        ToolboxData("<{0}:TYWebCalendar runat=server></{0}:TYWebCalendar>")]
    public class TYWebCalendar : System.Web.UI.WebControls.TextBox
    {
        
        //private int minYear;
        //private int maxYear;
        //private Color mainColor;
        //private Color shadow;
        //private int alpha;
        //private string bgTitle;
        private static bool isChecked=false;//是否已经检查过文件
        private const string jsName = "TYWebCalendar.js";
        //private DateTime curDate;
        /// <summary>
        /// 最小年份
        /// </summary>
        public int MinYear
        {
            get
            {
                object val = ViewState["minYear"];
                if (val != null)
                {
                    return (int)val;
                }
                return 0;
            }
            set
            {
                ViewState["minYear"] = value;
            }
        }

        /// <summary>
        /// 最大年份
        /// </summary>
        public int MaxYear
        {
            get
            {
                object val = ViewState["maxYear"];
                if (val != null)
                {
                    return (int)val;
                }
                return 2020;
            }
            set
            {
                ViewState["maxYear"] = value;
            }
        }

        /// <summary>
        /// 清空日期框
        /// </summary>
        public void ResetDate()
        {
            base.Text = "";
        }
        /// <summary>
        /// 主颜色
        /// </summary>
        public Color MainColor
        {
            get
            {
                object val = ViewState["mainColor"];
                if (val != null)
                {
                    return (Color)val;
                }
                return Color.Empty;
            }
            set
            {
                ViewState["mainColor"] = value;
            }
        }

        /// <summary>
        /// 阴影颜色
        /// </summary>
        public Color Shadow
        {
            get
            {
                object val = ViewState["shadow"];
                if (val != null)
                {
                    return (Color)val;
                }
                return Color.Empty;
            }
            set
            {
                ViewState["shadow"] = value;
            }
        }

        /// <summary>
        /// 透明值
        /// </summary>
        public int Alpha
        {
            get
            {
                object val = ViewState["alpha"];
                if (val != null)
                {
                    return (int)val;
                }
                return 0;
            }
            set
            {

                ViewState["alpha"] = value;
            }
        }
        /// <summary>
        /// 当前日期
        /// </summary>
        public DateTime CurrentDate
        {
            get
            {
                if (this.Text == "")
                {
                    return DateTime.MinValue;
                }
                DateTime dt = Convert.ToDateTime(this.Text);
                return dt;
            }
            set
            {
                base.Text = value.ToShortDateString();
            }
        }
        /// <summary>
        /// 当前日期的最后一刻(如2007-1-1就返回2007-1-1 23:59:59 999)
        /// </summary>
        public DateTime CurrentDayLast
        {
            get
            {
                if (this.Text == "")
                {
                    return DateTime.MinValue;
                }
                DateTime dt = Convert.ToDateTime(this.Text);
                dt = dt.AddDays(1).Subtract(TimeSpan.FromMilliseconds(1));
                return dt;
            }
            
        }
        /// <summary>
        /// 背景标题
        /// </summary>
        public string BgTitle
        {
            get
            {
                object val = ViewState["bgTitle"];
                if (val != null)
                {
                    return (string)val;
                }
                return "";
            }
            set
            {
                ViewState["bgTitle"] = value;
            }
        }
        /// <summary>
        /// 文本
        /// </summary>
        public override string Text
        {
            get
            {
                return base.Text;
            }
            set
            {
                DateTime dt = DateTime.MinValue;
                if (DateTime.TryParse(value, out dt))
                {
                    base.Text = dt.ToString("yyyy-MM-dd");
                }
            }
        }

        public void ClearDate()
        {
            base.Text = "";
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            //string js=CreateJs();
            string initJs = CreateInitJS();
            SaveJs();

            if (!Page.ClientScript.IsClientScriptIncludeRegistered(jsName+"Include"))
            {
                Page.ClientScript.RegisterClientScriptInclude(jsName+"Include", JsSaver.GetDefualtJsUrl(jsName));
            }

            if (!Page.ClientScript.IsStartupScriptRegistered("Init"))
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "Init", initJs, true);
            }
            this.Attributes.Add("onFocus", "myCalendar.Show(this,'" + MinYear.ToString() + "','" + MaxYear.ToString() + "','" + ContorlCommon.ToColorString(MainColor) + "','" + ContorlCommon.ToColorString(Shadow) + "','" + Alpha.ToString() + "','" + BgTitle + "')");
            this.Attributes.Add("onBlur", "myCalendar.Hidden()");
            this.Attributes.Add("onkeydown", "myCalendar.txtClear(event,this);");
            base.ReadOnly = true;
            base.EnableViewState = false;
        }

        /// <summary>
        /// 保存成JS文件
        /// </summary>
        private void SaveJs()
        {
            if (!isChecked)
            {
                JsSaver.SaveJS(jsName,CreateJs());
                isChecked = true;
            }
        }
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
        }

        

        ///// <summary>
        ///// 创建初始的JS
        ///// </summary>
        ///// <returns></returns>
        private string CreateInitJS()
        {
            StringBuilder js = new StringBuilder(5000);
            //js.Append("<script language=\"javascript\">\n");
            js.Append("var myCalendar= new WebCalendar();\n");
            //js.Append("cal" + this.ClientID + ".MinYear = \"" + minYear.ToString() + "\";\n");
            //js.Append("cal" + this.ClientID + ".MaxYear = \"" + maxYear.ToString() + "\";\n");
            //js.Append("cal" + this.ClientID + ".MainColor = \"" + ToColorString(mainColor) + "\";\n");
            //js.Append("cal" + this.ClientID + ".Shadow = \"" + ToColorString(shadow) + "\";\n");
            //js.Append("cal" + this.ClientID + ".Alpha = \"" + alpha.ToString() + "\";\n");
            //js.Append("}catch(e){status = \"Error to init WebCalendar\";}\n");
            //js.Append("</script>\n");
            //if (this.CurrentDate == DateTime.MinValue)
            //{
            //    this.CurrentDate = DateTime.Now;
            //}
            return js.ToString();
        }
        /// <summary>
        /// 创建日期的JS脚本
        /// </summary>
        /// <returns></returns>
        private string CreateJs()
        {
            StringBuilder sb = new StringBuilder(28468);
            //js.Append("<script language=\"javascript\">\n");
            sb.Append("//\n");
            sb.Append("var webCalendar;\n");
            sb.Append("function WebCalendar()\n");
            sb.Append("{\n");
            sb.Append("    webCalendar = this;\n");
            sb.Append("	this.calendar_state = new Object();\n");
            sb.Append("	this.MinYear = 1900;\n");
            sb.Append("	this.MaxYear = 2020;\n");
            sb.Append("	this.Width = 170; \n");
            sb.Append("	this.Height = 200; \n");
            sb.Append("	this.BgTitle=\"\"\n");
            sb.Append("	this.DateFormat = \"yyyy-MM-dd\";\n");
            sb.Append("	this.Source = null;\n");
            sb.Append("	\n");
            sb.Append("	this.UnselectBgColor = \"\"; \n");
            sb.Append("	this.MainColor = \"#55B9F4\";\n");
            sb.Append("	this.Shadow = \"#666666\";\n");
            sb.Append("	this.Alpha = \"100\";\n");
            sb.Append("	this.SelectedColor = \"#FFFFFF\"; \n");
            sb.Append("	this.DayBdWidth = \"1\"; \n");
            sb.Append("	this.DayBdColor = this.UnselectBgColor; \n");
            sb.Append("	this.TodayBdColor = \"#000000\"; \n");
            sb.Append("	this.InvalidColor = \"#808080\";\n");
            sb.Append("	this.ValidColor = \"#000099\";\n");
            sb.Append("	this.WeekendBgColor = this.UnselectBgColor;\n");
            sb.Append("	this.WeekendColor = \"#006600\";\n");
            sb.Append("\n");
            sb.Append("	this.YearListStyle = \"font-size:12px; font-family:宋体;\"; \n");
            sb.Append("	this.MonthListStyle = \"font-size:12px; font-family:宋体;\"; \n");
            sb.Append("	this.TitleStyle = \"cursor:default; height:24px; color:#000000; background-color:\" + this.UnselectBgColor + \"; font-size:12px;  text-align:center; vertical-align:bottom;\";  \n");
            sb.Append("	this.FooterOverStyle = \"height:20px; cursor:hand; color:#000000; background-color:#999999; font-size:12px; font-family:Verdana; text-align:center; vertical-align:middle;\"; \n");
            sb.Append("	this.FooterStyle = \"height:20px; cursor:hand; color:#FFFFFF; background-color:#333333; font-size:12px; font-family:Verdana; text-align:center; vertical-align:middle;\";\n");
            sb.Append("	\n");
            sb.Append("	this.TodayTitle = \"今天：\";\n");
            sb.Append("	\n");
            sb.Append("	this.LineBgStyle = \"height:6px; background-color:\" + this.UnselectBgColor + \"; text-align:center; vertical-align:middle;\";\n");
            sb.Append("	this.LineStyle = \"width:94%; height:1px; background-color:#000000;\"; \n");
            sb.Append("	this.DayStyle = \"cursor:hand; font-size:12px; font-family:Verdana; text-align:center; vertical-align:middle;\"; \n");
            sb.Append("	this.OverDayStyle = \"this.style.backgroundColor = '#aaaaaa';\"; \n");
            sb.Append("	this.OutDayStyle = \"this.style.backgroundColor = 'Transparent';\";\n");
            sb.Append("	\n");
            sb.Append("	this.MonthName = new Array(\"1月\", \"2月\", \"3月\", \"4月\", \"5月\", \"6月\", \"7月\", \"8月\", \"9月\", \"10月\", \"11月\", \"12月\");\n");
            sb.Append("	this.WeekName = new Array(\"日\", \"一\", \"二\", \"三\", \"四\", \"五\", \"六\"); \n");
            sb.Append("	\n");
            sb.Append("	this.boolCalendarPadExist = false;\n");
            sb.Append("	\n");
            sb.Append("	this.CreateCalendarPad = function()\n");
            sb.Append("	{\n");
            sb.Append("		this.boolCalendarPadExist = true;\n");
            sb.Append("		var theValue = this.Source.value; \n");
            sb.Append("		var theCurrentDate = new Date(this.GetTextDate(theValue)); \n");
            sb.Append("		if (isNaN(theCurrentDate))\n");
            sb.Append("		{ \n");
            sb.Append("			theCurrentDate = new Date(); \n");
            sb.Append("		}\n");
            sb.Append("		var CalendarPadHTML = \"\";\n");
            sb.Append("		CalendarPadHTML += \" <div id=\\\"_CalendarPad\\\" style=\\\"Z-INDEX: 201; POSITION: absolute;top:-100;left:-100;\"; \n");
            sb.Append("		if(this.Shadow != \"\")\n");
            sb.Append("		{\n");
            sb.Append("			CalendarPadHTML += \"FILTER:progid:DXImageTransform.Microsoft.Shadow(direction=135,color=\" + this.Shadow + \",strength=3);\";\n");
            sb.Append("		}\n");
            sb.Append("		if(this.Alpha != \"100\" && this.Alpha != \"\")\n");
            sb.Append("		{\n");
            sb.Append("			CalendarPadHTML += \"FILTER: progid:DXImageTransform.Microsoft.Alpha( style=0,opacity=\" + this.Alpha + \");\";\n");
            sb.Append("		}\n");
            sb.Append("		CalendarPadHTML += \" \\\">\";\n");
            sb.Append("		CalendarPadHTML += \" <iframe frameborder=0 width=\\\"\" + (this.Width+2).toString() + \"\\\" height=\\\"\" + (this.Height+2).toString() + \"\\\"></iframe>\";\n");
            sb.Append("		CalendarPadHTML += \" <div style=\\\"Z-INDEX:202;position:absolute;top:0;left:0;\\\"><table width=\\\"\" + this.Width.toString() + \"\\\" height=\\\"\" + this.Height.toString() + \"\\\" border=\\\"0\\\" cellspacing=\\\"0\\\" cellpadding=\\\"0\\\"><tr><td align=\\\"center\\\" style=\\\" font-family:黑体;color:#E6E6E6 ; font-size:24px; font-weight:bold\\\">\"+this.BgTitle+\"</td></tr></table></div>\";\n");
            sb.Append("		CalendarPadHTML += \" <div style=\\\"Z-INDEX:203;position:absolute;top:0;left:0;border:1px solid #000000;\\\" onclick=\\\"webCalendar.Source.select()\\\">\";\n");
            sb.Append("		CalendarPadHTML += \" <table width=\\\"\" + this.Width.toString() + \"\\\" height=\\\"\" + this.Height.toString() + \"\\\" cellpadding=\\\"0\\\" cellspacing=\\\"0\\\">\"; \n");
            sb.Append("		CalendarPadHTML += \" <tr>\";\n");
            sb.Append("		CalendarPadHTML += \" <td align=\\\"center\\\" valign=\\\"top\\\">\"; \n");
            sb.Append("		CalendarPadHTML += \" <table width=\\\"100%\\\" height=\\\"100%\\\" border=\\\"0\\\" cellspacing=\\\"0\\\" cellpadding=\\\"0\\\">\"; \n");
            sb.Append("		CalendarPadHTML += \" <tr align=\\\"center\\\" style=\\\"height:30px; background-color:\" + this.MainColor + \";\\\">\"; \n");
            sb.Append("		CalendarPadHTML += \" <td align=\\\"center\\\">\"; \n");
            sb.Append("		CalendarPadHTML += \" <table border=\\\"0\\\" cellspacing=\\\"0\\\" cellpadding=\\\"3\\\">\"; \n");
            sb.Append("		CalendarPadHTML += \" <tr>\"; \n");
            sb.Append("		CalendarPadHTML += \" <td align=\\\"center\\\">\"; \n");
            sb.Append("		CalendarPadHTML += \" <input type=\\\"button\\\" tabIndex=\\\"-1\\\" style=\\\"font-family:Marlett; CURSOR:hand; font-size:12px;width:14px;height:18px;border:1px solid #EEEEEE;color:#EEEEEE;background-color:\" + this.MainColor + \";\\\" id=\\\"_goPreviousMonth\\\" value=\\\"3\\\" onClick=\\\"webCalendar.UpdateCalendarGrid(this)\\\" onmouseover=\\\"this.style.cssText='border:1px solid #FFFFFF;color:#FFFFFF;font-family:Marlett; CURSOR:hand; font-size:12px;width:14px;height:18px;background-color:\" + this.MainColor + \";'\\\" onmouseout=\\\"this.style.cssText='font-family:Marlett; CURSOR:hand; font-size:12px;width:14px;height:18px;border:1px solid #EEEEEE;color:#EEEEEE;background-color:\" + this.MainColor + \";'\\\">\"; \n");
            sb.Append("		CalendarPadHTML += \" </td>\"; \n");
            sb.Append("		CalendarPadHTML += \" <td align=\\\"center\\\">\"; \n");
            sb.Append("		CalendarPadHTML += \" <select id=\\\"_YearList\\\">\";\n");
            sb.Append("		CalendarPadHTML += \" </select>\"; \n");
            sb.Append("		CalendarPadHTML += \" </td>\"; \n");
            sb.Append("		CalendarPadHTML += \" <td align=\\\"center\\\">\"; \n");
            sb.Append("		CalendarPadHTML += \" <select id=\\\"_MonthList\\\">\";\n");
            sb.Append("		CalendarPadHTML += \" </select>\"; \n");
            sb.Append("		CalendarPadHTML += \" <input type=\\\"hidden\\\" id=\\\"_DayList\\\" value=\\\"1\\\">\";\n");
            sb.Append("		CalendarPadHTML += \" </td>\"; \n");
            sb.Append("		CalendarPadHTML += \" <td align=\\\"center\\\">\"; \n");
            sb.Append("		CalendarPadHTML += \" <input type=\\\"button\\\" tabIndex=\\\"-1\\\" style=\\\"font-family:Marlett; CURSOR:hand; font-size:12px;width:14px;height:18px;border:1px solid #EEEEEE;color:#EEEEEE;background-color:\" + this.MainColor + \";\\\" id=\\\"_goNextMonth\\\" value=\\\"4\\\" onClick=\\\"webCalendar.UpdateCalendarGrid(this)\\\" onmouseover=\\\"this.style.cssText='border:1px solid #FFFFFF;color:#FFFFFF;font-family:Marlett; CURSOR:hand; font-size:12px;width:14px;height:18px;background-color:\" + this.MainColor + \";'\\\" onmouseout=\\\"this.style.cssText='font-family:Marlett; CURSOR:hand; font-size:12px;width:14px;height:18px;border:1px solid #EEEEEE;color:#EEEEEE;background-color:\" + this.MainColor + \";'\\\">\";\n");
            sb.Append("		CalendarPadHTML += \" </td>\"; \n");
            sb.Append("		CalendarPadHTML += \" </tr>\"; \n");
            sb.Append("		CalendarPadHTML += \" </table>\";\n");
            sb.Append("		CalendarPadHTML += \" </td>\"; \n");
            sb.Append("		CalendarPadHTML += \" </tr>\"; \n");
            sb.Append("		CalendarPadHTML += \" <tr>\"; \n");
            sb.Append("		CalendarPadHTML += \" <td align=\\\"center\\\">\"; \n");
            sb.Append("		CalendarPadHTML += \" <div id=\\\"_CalendarGrid\\\"></div>\";\n");
            sb.Append("		CalendarPadHTML += \" </td>\"; \n");
            sb.Append("		CalendarPadHTML += \" </tr>\"; \n");
            sb.Append("		CalendarPadHTML += \" </table>\"; \n");
            sb.Append("		CalendarPadHTML += \" </td>\"; \n");
            sb.Append("		CalendarPadHTML += \" </tr>\"; \n");
            sb.Append("		CalendarPadHTML += \" </table>\"; \n");
            sb.Append("		CalendarPadHTML += \"</div>\";\n");
            sb.Append("		CalendarPadHTML += \"</div>\"; \n");
            sb.Append("		document.body.insertAdjacentHTML(\"beforeEnd\", CalendarPadHTML);\n");
            sb.Append("		this.CreateYearList(this.MinYear, this.MaxYear); \n");
            sb.Append("		this.CreateMonthList();\n");
            sb.Append("		theCalendarPadObject = document.all.item(\"_CalendarPad\"); \n");
            sb.Append("		theCalendarPadObject.style.position = \"absolute\"; \n");
            sb.Append("		theCalendarPadObject.style.posLeft = this.GetCalendarPadLeft(this.Source); \n");
            sb.Append("		theCalendarPadObject.style.posTop = this.GetCalendarPadTop(this.Source); \n");
            sb.Append("		this.CreateCalendarGrid(theCurrentDate.getFullYear(), theCurrentDate.getMonth(), theCurrentDate.getDate());\n");
            sb.Append("	} \n");
            sb.Append("	\n");
            sb.Append("	//创建年下拉菜单\n");
            sb.Append("	this.CreateYearList = function(MinYear, MaxYear)\n");
            sb.Append("	{\n");
            sb.Append("		var theYearObject = document.all.item(\"_YearList\"); \n");
            sb.Append("		if (theYearObject == null)\n");
            sb.Append("		{ \n");
            sb.Append("			return; \n");
            sb.Append("		} \n");
            sb.Append("		var theYear = 0; \n");
            sb.Append("		var theYearHTML = \"<select id=\\\"_YearList\\\"  style=\\\"\" + this.YearListStyle + \"\\\"  tabIndex=\\\"-1\\\" onChange=\\\"webCalendar.UpdateCalendarGrid(this)\\\">\"; \n");
            sb.Append("		for (theYear = MinYear; theYear <= MaxYear; theYear++)\n");
            sb.Append("		{ \n");
            sb.Append("			theYearHTML += \"<option value=\\\"\" + theYear.toString() + \"\\\">\" + theYear.toString() + \"年</option>\"; \n");
            sb.Append("		} \n");
            sb.Append("		theYearHTML += \"</select>\"; \n");
            sb.Append("		theYearObject.outerHTML = theYearHTML; \n");
            sb.Append("	} \n");
            sb.Append("\n");
            sb.Append("	//创建月下拉菜单\n");
            sb.Append("	this.CreateMonthList = function( )\n");
            sb.Append("	{\n");
            sb.Append("		var theMonthObject = document.all.item(\"_MonthList\"); \n");
            sb.Append("		if (theMonthObject == null)\n");
            sb.Append("		{ \n");
            sb.Append("			return; \n");
            sb.Append("		} \n");
            sb.Append("		var theMonth = 0; \n");
            sb.Append("		var theMonthHTML = \"<select id=\\\"_MonthList\\\" tabIndex=\\\"-1\\\" style=\\\"\" + this.MonthListStyle + \"\\\"  onChange=\\\"webCalendar.UpdateCalendarGrid(this)\\\">\"; \n");
            sb.Append("		for (theMonth = 0; theMonth < 12; theMonth++)\n");
            sb.Append("		{ \n");
            sb.Append("			theMonthHTML += \"<option value=\\\"\" + theMonth.toString() + \"\\\">\" + this.MonthName[theMonth] + \"</option>\"; \n");
            sb.Append("		} \n");
            sb.Append("		theMonthHTML +=\"</select>\"; \n");
            sb.Append("		theMonthObject.outerHTML = theMonthHTML;\n");
            sb.Append("	} \n");
            sb.Append("	\n");
            sb.Append("	this.CreateCalendarGrid = function(theYear, theMonth, theDay, forceToClose)\n");
            sb.Append("	{\n");
            sb.Append("		var theTextObject = this.Source;\n");
            sb.Append("		if (theTextObject == null)\n");
            sb.Append("		{\n");
            sb.Append("			return;\n");
            sb.Append("		}\n");
            sb.Append("		var theYearObject = document.all.item(\"_YearList\");\n");
            sb.Append("		var theMonthObject = document.all.item(\"_MonthList\");\n");
            sb.Append("		var tmpYear = theYear;\n");
            sb.Append("		var tmpMonth = theMonth;\n");
            sb.Append("		var tmpDay = 1;\n");
            sb.Append("		if (tmpMonth < 0)\n");
            sb.Append("		{\n");
            sb.Append("			tmpYear--;\n");
            sb.Append("			tmpMonth = 11;\n");
            sb.Append("		}\n");
            sb.Append("		if (tmpMonth > 11)\n");
            sb.Append("		{ \n");
            sb.Append("			tmpYear++; \n");
            sb.Append("			tmpMonth = 0; \n");
            sb.Append("		} \n");
            sb.Append("		if (tmpYear < this.MinYear)\n");
            sb.Append("		{ \n");
            sb.Append("			tmpYear = this.MinYear; \n");
            sb.Append("		} \n");
            sb.Append("		if (tmpYear > this.MaxYear)\n");
            sb.Append("		{ \n");
            sb.Append("			tmpYear = this.MaxYear; \n");
            sb.Append("		} \n");
            sb.Append("		if (theDay < 1)\n");
            sb.Append("		{ \n");
            sb.Append("			tmpDay = 1; \n");
            sb.Append("		}\n");
            sb.Append("		else\n");
            sb.Append("		{ \n");
            sb.Append("			tmpDay = this.GetMonthDays(tmpYear, tmpMonth); \n");
            sb.Append("			if (theDay < tmpDay)\n");
            sb.Append("			{ \n");
            sb.Append("				tmpDay = theDay;\n");
            sb.Append("			} \n");
            sb.Append("		} \n");
            sb.Append("		theYearObject.value = tmpYear; \n");
            sb.Append("		theMonthObject.value = tmpMonth;\n");
            sb.Append("		this.SetDayList(tmpYear, tmpMonth, tmpDay);\n");
            sb.Append("		theTextObject.value = this.SetDateFormat(tmpYear, tmpMonth, tmpDay);\n");
            sb.Append("		theTextObject.select();\n");
            sb.Append("		if(forceToClose)\n");
            sb.Append("		{\n");
            sb.Append("			this.Hidden(forceToClose);\n");
            sb.Append("		}\n");
            sb.Append("	} \n");
            sb.Append("\n");
            sb.Append("	this.SetDayList = function(theYear, theMonth, theDay)\n");
            sb.Append("	{\n");
            sb.Append("		var theDayObject = document.all.item(\"_DayList\"); \n");
            sb.Append("		if (theDayObject == null)\n");
            sb.Append("		{ \n");
            sb.Append("			return; \n");
            sb.Append("		} \n");
            sb.Append("		theDayObject.value = theDay.toString(); \n");
            sb.Append("		var theFirstDay = new Date(theYear, theMonth, 1); \n");
            sb.Append("		var theCurrentDate = new Date(); \n");
            sb.Append("		var theWeek = theFirstDay.getDay(); \n");
            sb.Append("		if (theWeek == 0)\n");
            sb.Append("		{ \n");
            sb.Append("			theWeek = 7; \n");
            sb.Append("		} \n");
            sb.Append("		var theLeftDay = 0; \n");
            sb.Append("		if (theMonth == 0)\n");
            sb.Append("		{ \n");
            sb.Append("			theLeftDay = 31; \n");
            sb.Append("		}\n");
            sb.Append("		else\n");
            sb.Append("		{ \n");
            sb.Append("			theLeftDay = this.GetMonthDays(theYear, theMonth - 1); \n");
            sb.Append("		} \n");
            sb.Append("		var theRightDay = this.GetMonthDays(theYear, theMonth); \n");
            sb.Append("		var theCurrentDay = theLeftDay - theWeek + 1; \n");
            sb.Append("		var offsetMonth = -1; \n");
            sb.Append("		var theColor = this.InvalidColor; \n");
            sb.Append("		var theBgColor = this.UnselectBgColor; \n");
            sb.Append("		var theBdColor = theBgColor; \n");
            sb.Append("		var WeekId = 0 \n");
            sb.Append("		var DayId = 0; \n");
            sb.Append("		var theStyle = \"\"; \n");
            sb.Append("		var theDayHTML = \"<table width=\\\"100%\\\" height=\\\"100%\\\" border=\\\"0\\\" cellspacing=\\\"1\\\" cellpadding=\\\"0\\\">\"; \n");
            sb.Append("		theDayHTML += \" <tr style=\\\"cursor:default; height:24px; color:#000000; font-size:12px;  text-align:center; vertical-align:bottom;\\\">\"; \n");
            sb.Append("		for (DayId = 0; DayId < 7; DayId++)\n");
            sb.Append("		{ \n");
            sb.Append("			theDayHTML += \" <td>\" + this.WeekName[DayId] + \"</td>\"; \n");
            sb.Append("		} \n");
            sb.Append("		theDayHTML += \" </tr>\"; \n");
            sb.Append("		theDayHTML += \" <tr>\"; \n");
            sb.Append("		theDayHTML += \" <td colspan=\\\"7\\\" style=\\\"\" + this.LineBgStyle + \"\\\">\"; \n");
            sb.Append("		theDayHTML += \" <table style=\\\"\" + this.LineStyle + \"\\\" border=\\\"0\\\" cellspacing=\\\"0\\\" cellpadding=\\\"0\\\"><tr><td style=\\\"height:1px\\\"></td></tr></table>\"; \n");
            sb.Append("		theDayHTML += \" </td>\";\n");
            sb.Append("		theDayHTML += \" </td>\"; \n");
            sb.Append("		theDayHTML += \" </tr>\"; \n");
            sb.Append("		for (WeekId = 0; WeekId < 6; WeekId++)\n");
            sb.Append("		{ \n");
            sb.Append("			theDayHTML += \" <tr  style=\\\"\" + this.DayStyle + \"\\\">\"; \n");
            sb.Append("			for (DayId = 0; DayId < 7; DayId++)\n");
            sb.Append("			{ \n");
            sb.Append("				if ((theCurrentDay > theLeftDay) && (WeekId < 3))\n");
            sb.Append("				{ \n");
            sb.Append("					offsetMonth++; \n");
            sb.Append("					theCurrentDay = 1; \n");
            sb.Append("				} \n");
            sb.Append("				if ((theCurrentDay > theRightDay) && (WeekId > 3))\n");
            sb.Append("				{ \n");
            sb.Append("					offsetMonth++; \n");
            sb.Append("					theCurrentDay = 1; \n");
            sb.Append("				} \n");
            sb.Append("				switch (offsetMonth)\n");
            sb.Append("				{ \n");
            sb.Append("					case -1: \n");
            sb.Append("					theColor = this.InvalidColor; \n");
            sb.Append("					break; \n");
            sb.Append("					case 1: \n");
            sb.Append("					theColor = this.InvalidColor; \n");
            sb.Append("					break; \n");
            sb.Append("					case 0: \n");
            sb.Append("					if ((DayId==0)||(DayId==6))\n");
            sb.Append("					{ \n");
            sb.Append("						theColor = this.WeekendColor; \n");
            sb.Append("					}\n");
            sb.Append("					else\n");
            sb.Append("					{ \n");
            sb.Append("						theColor = this.ValidColor; \n");
            sb.Append("					} \n");
            sb.Append("					break; \n");
            sb.Append("				} \n");
            sb.Append("				if ((DayId==0)||(DayId==6))\n");
            sb.Append("				{ \n");
            sb.Append("					theBgColor = this.WeekendBgColor; \n");
            sb.Append("				}\n");
            sb.Append("				else\n");
            sb.Append("				{ \n");
            sb.Append("					theBgColor = this.UnselectBgColor; \n");
            sb.Append("				} \n");
            sb.Append("				theBdColor = this.DayBdColor; \n");
            sb.Append("				var isToday = false;\n");
            sb.Append("				if ((theCurrentDay == theDay) && (offsetMonth == 0))\n");
            sb.Append("				{\n");
            sb.Append("					theColor = this.SelectedColor; \n");
            sb.Append("					theBgColor = this.MainColor; \n");
            sb.Append("					theBdColor = theBgColor; \n");
            sb.Append("					isToday = true;\n");
            sb.Append("				}\n");
            sb.Append("				if ((theYear == theCurrentDate.getFullYear()) && (theMonth == theCurrentDate.getMonth()) && (theCurrentDay == theCurrentDate.getDate()) && (offsetMonth == 0))\n");
            sb.Append("				{ \n");
            sb.Append("					theBdColor = this.TodayBdColor;\n");
            sb.Append("					isToday = true;\n");
            sb.Append("				} \n");
            sb.Append("				theStyle = \"border:\" + this.DayBdWidth + \"px solid \" + theBdColor + \"; color:\" + theColor + \"; background-color:\" + theBgColor + \";\"; \n");
            sb.Append("				if(isToday)\n");
            sb.Append("					theDayHTML += \" <td style=\\\"\" + theStyle + \"\\\" onMouseDown=\\\"WebCalendar_Click(\" + theYear.toString() + \", \" + (theMonth + offsetMonth).toString() + \", \" + theCurrentDay.toString() + \")\\\">\"; \n");
            sb.Append("				else\n");
            sb.Append("					theDayHTML += \" <td style=\\\"\" + theStyle + \"\\\" onMouseOver=\\\"\" + this.OverDayStyle + \"\\\" onMouseOut=\\\"\" + this.OutDayStyle + \"\\\" onMouseDown=\\\"WebCalendar_Click(\" + theYear.toString() + \", \" + (theMonth + offsetMonth).toString() + \", \" + theCurrentDay.toString() + \")\\\">\"; \n");
            sb.Append("				theDayHTML += theCurrentDay.toString();\n");
            sb.Append("				theDayHTML += \" </td>\"; \n");
            sb.Append("				theCurrentDay++; \n");
            sb.Append("			} \n");
            sb.Append("			theDayHTML += \" </tr>\";\n");
            sb.Append("		}\n");
            sb.Append("		\n");
            sb.Append("		theDayHTML += \" <tr>\"; \n");
            sb.Append("		theDayHTML += \" <td colspan=\\\"7\\\" style=\\\"\" + this.FooterStyle + \"\\\" onmouseover=\\\"this.style.cssText='\" + this.FooterOverStyle + \"'\\\" onmouseout=\\\"this.style.cssText='\" + this.FooterStyle + \"'\\\" onMouseDown=\\\"WebCalendar_Click(\" + theCurrentDate.getFullYear().toString() + \", \" + theCurrentDate.getMonth().toString() + \", \" + theCurrentDate.getDate().toString() + \");\\\" >\" + this.TodayTitle + \" \" + this.SetDateFormat(theCurrentDate.getFullYear(), theCurrentDate.getMonth(), theCurrentDate.getDate()) + \"</td>\"; \n");
            sb.Append("		theDayHTML += \" </tr>\"; \n");
            sb.Append("		theDayHTML += \" </table>\"; \n");
            sb.Append("		var theCalendarGrid = document.all.item(\"_CalendarGrid\"); \n");
            sb.Append("		theCalendarGrid.innerHTML = theDayHTML; \n");
            sb.Append("	}\n");
            sb.Append("	\n");
            sb.Append("	this.GetCalendarPadLeft = function(theObject)\n");
            sb.Append("	{\n");
            sb.Append("		var absLeft = 0; \n");
            sb.Append("		var thePosition=\"\"; \n");
            sb.Append("		var tmpObject = theObject; \n");
            sb.Append("		while (tmpObject != null)\n");
            sb.Append("		{ \n");
            sb.Append("			thePosition = tmpObject.position; \n");
            sb.Append("			tmpObject.position = \"static\"; \n");
            sb.Append("			absLeft += tmpObject.offsetLeft; \n");
            sb.Append("			tmpObject.position = thePosition; \n");
            sb.Append("			tmpObject = tmpObject.offsetParent; \n");
            sb.Append("		} \n");
            sb.Append("		return absLeft;\n");
            sb.Append("	} \n");
            sb.Append("	\n");
            sb.Append("	this.GetCalendarPadTop = function(theObject)\n");
            sb.Append("	{\n");
            sb.Append("		var absTop = 0; \n");
            sb.Append("		var thePosition = \"\"; \n");
            sb.Append("		var tmpObject = theObject; \n");
            sb.Append("		while (tmpObject != null)\n");
            sb.Append("		{ \n");
            sb.Append("			thePosition = tmpObject.position; \n");
            sb.Append("			tmpObject.position = \"static\"; \n");
            sb.Append("			absTop += tmpObject.offsetTop; \n");
            sb.Append("			tmpObject.position = thePosition; \n");
            sb.Append("			tmpObject = tmpObject.offsetParent;\n");
            sb.Append("		} \n");
            sb.Append("		return absTop + this.Source.offsetHeight; \n");
            sb.Append("	} \n");
            sb.Append("	\n");
            sb.Append("	//转换字符串为日期格式\n");
            sb.Append("	//**GetTextDate Start**\n");
            sb.Append("	this.GetTextDate = function(txtInTextBox)\n");
            sb.Append("	{\n");
            sb.Append("		var i = 0, tmpChar = \"\", find_tag = \"\";\n");
            sb.Append("		var start_at = 0, end_at = 0, year_at = 0, month_at = 0, day_at = 0;\n");
            sb.Append("		var tmp_at = 0, one_at, two_at = 0, one_days = 0, two_days = 0;\n");
            sb.Append("		var aryDate = new Array();\n");
            sb.Append("		var tmpYear = -1, tmpMonth = -1, tmpDay = -1;\n");
            sb.Append("		var tmpDate = txtInTextBox.toLowerCase();\n");
            sb.Append("		var defDate = \"\";\n");
            sb.Append("		end_at = tmpDate.length;\n");
            sb.Append("		\n");
            sb.Append("		for (i=1;i<end_at;i++)\n");
            sb.Append("		{\n");
            sb.Append("			if (tmpDate.charAt(i)==\"0\")\n");
            sb.Append("			{\n");
            sb.Append("				tmpChar = tmpDate.charAt(i-1);\n");
            sb.Append("				if (tmpChar<\"0\" || tmpChar>\"9\")\n");
            sb.Append("				{\n");
            sb.Append("					tmpDate = tmpDate.substr(0,i-1) + \"-\" + tmpDate.substr(i+1);\n");
            sb.Append("				}\n");
            sb.Append("			}\n");
            sb.Append("		}\n");
            sb.Append("		for (i=0;i<9;i++)\n");
            sb.Append("		{ \n");
            sb.Append("			tmpDate = tmpDate.replace(this.MonthName[i].toLowerCase().substr(0,3), \"-00\" + (i+1).toString() + \"-\"); \n");
            sb.Append("		} \n");
            sb.Append("		for (i=9;i<12;i++)\n");
            sb.Append("		{ \n");
            sb.Append("			tmpDate = tmpDate.replace(this.MonthName[i].toLowerCase().substr(0,3), \"-0\" + (i+1).toString() + \"-\"); \n");
            sb.Append("		} \n");
            sb.Append("		tmpDate = tmpDate.replace(/jan/g, \"-001-\"); \n");
            sb.Append("		tmpDate = tmpDate.replace(/feb/g, \"-002-\"); \n");
            sb.Append("		tmpDate = tmpDate.replace(/mar/g, \"-003-\"); \n");
            sb.Append("		tmpDate = tmpDate.replace(/apr/g, \"-004-\"); \n");
            sb.Append("		tmpDate = tmpDate.replace(/may/g, \"-005-\"); \n");
            sb.Append("		tmpDate = tmpDate.replace(/jun/g, \"-006-\"); \n");
            sb.Append("		tmpDate = tmpDate.replace(/jul/g, \"-007-\"); \n");
            sb.Append("		tmpDate = tmpDate.replace(/aug/g, \"-008-\"); \n");
            sb.Append("		tmpDate = tmpDate.replace(/sep/g, \"-009-\"); \n");
            sb.Append("		tmpDate = tmpDate.replace(/oct/g, \"-010-\"); \n");
            sb.Append("		tmpDate = tmpDate.replace(/nov/g, \"-011-\"); \n");
            sb.Append("		tmpDate = tmpDate.replace(/dec/g, \"-012-\"); \n");
            sb.Append("		for (i=0;i<tmpDate.length;i++)\n");
            sb.Append("		{ \n");
            sb.Append("			tmpChar = tmpDate.charAt(i); \n");
            sb.Append("			if ((tmpChar<\"0\" || tmpChar>\"9\") && (tmpChar != \"-\"))\n");
            sb.Append("			{ \n");
            sb.Append("				tmpDate = tmpDate.replace(tmpChar,\"-\") \n");
            sb.Append("			} \n");
            sb.Append("		} \n");
            sb.Append("		while(tmpDate.indexOf(\"--\") != -1)\n");
            sb.Append("		{ \n");
            sb.Append("			tmpDate = tmpDate.replace(/--/g,\"-\"); \n");
            sb.Append("		} \n");
            sb.Append("		start_at = 0; \n");
            sb.Append("		end_at = tmpDate.length-1; \n");
            sb.Append("		while (tmpDate.charAt(start_at)==\"-\")\n");
            sb.Append("		{ \n");
            sb.Append("			start_at++; \n");
            sb.Append("		} \n");
            sb.Append("		while (tmpDate.charAt(end_at)==\"-\")\n");
            sb.Append("		{ \n");
            sb.Append("			end_at--; \n");
            sb.Append("		} \n");
            sb.Append("		if (start_at < end_at+1)\n");
            sb.Append("		{ \n");
            sb.Append("			tmpDate = tmpDate.substring(start_at,end_at+1); \n");
            sb.Append("		}\n");
            sb.Append("		else\n");
            sb.Append("		{ \n");
            sb.Append("			tmpDate = \"\"; \n");
            sb.Append("		} \n");
            sb.Append("		aryDate = tmpDate.split(\"-\"); \n");
            sb.Append("		if (aryDate.length != 3)\n");
            sb.Append("		{ \n");
            sb.Append("			return(defDate); \n");
            sb.Append("		} \n");
            sb.Append("		for (i=0;i<3;i++)\n");
            sb.Append("		{ \n");
            sb.Append("			if (parseInt(aryDate[i],10)<1)\n");
            sb.Append("			{ \n");
            sb.Append("				aryDate[i] = \"1\"; \n");
            sb.Append("			} \n");
            sb.Append("		} \n");
            sb.Append("		find_tag=\"000\"; \n");
            sb.Append("		for (i=2;i>=0;i--)\n");
            sb.Append("		{ \n");
            sb.Append("			if (aryDate[i].length==3)\n");
            sb.Append("			{ \n");
            sb.Append("				if (aryDate[i]>=\"001\" && aryDate[i]<=\"012\")\n");
            sb.Append("				{ \n");
            sb.Append("					tmpMonth = parseInt(aryDate[i],10)-1; \n");
            sb.Append("					switch (i)\n");
            sb.Append("					{ \n");
            sb.Append("						case 0: \n");
            sb.Append("						find_tag = \"100\"; \n");
            sb.Append("						one_at = parseInt(aryDate[1],10); \n");
            sb.Append("						two_at = parseInt(aryDate[2],10); \n");
            sb.Append("						break; \n");
            sb.Append("						case 1: \n");
            sb.Append("						find_tag = \"010\"; \n");
            sb.Append("						one_at = parseInt(aryDate[0],10); \n");
            sb.Append("						two_at = parseInt(aryDate[2],10); \n");
            sb.Append("						break; \n");
            sb.Append("						case 2: \n");
            sb.Append("						find_tag = \"001\"; \n");
            sb.Append("						one_at = parseInt(aryDate[0],10); \n");
            sb.Append("						two_at = parseInt(aryDate[1],10); \n");
            sb.Append("						break; \n");
            sb.Append("					} \n");
            sb.Append("				} \n");
            sb.Append("			} \n");
            sb.Append("		} \n");
            sb.Append("		if (find_tag!=\"000\")\n");
            sb.Append("		{ \n");
            sb.Append("			one_days = this.GetMonthDays(two_at,tmpMonth); \n");
            sb.Append("			two_days = this.GetMonthDays(one_at,tmpMonth); \n");
            sb.Append("			if ((one_at>one_days)&&(two_at>two_days))\n");
            sb.Append("			{ \n");
            sb.Append("				return(defDate); \n");
            sb.Append("			} \n");
            sb.Append("			if ((one_at<=one_days)&&(two_at>two_days))\n");
            sb.Append("			{ \n");
            sb.Append("				tmpYear = this.GetFormatYear(two_at); \n");
            sb.Append("				tmpDay = one_at; \n");
            sb.Append("			} \n");
            sb.Append("			if ((one_at>one_days)&&(two_at<=two_days))\n");
            sb.Append("			{ \n");
            sb.Append("				tmpYear = this.GetFormatYear(one_at); \n");
            sb.Append("				tmpDay = two_at; \n");
            sb.Append("			} \n");
            sb.Append("			if ((one_at<=one_days)&&(two_at<=two_days))\n");
            sb.Append("			{ \n");
            sb.Append("				tmpYear = this.GetFormatYear(one_at); \n");
            sb.Append("				tmpDay = two_at; \n");
            sb.Append("				tmpDate = this.DateFormat;\n");
            sb.Append("				year_at = tmpDate.indexOf(\"yyyy\"); \n");
            sb.Append("				if (year_at == -1)\n");
            sb.Append("				{ \n");
            sb.Append("					year_at = tmpDate.indexOf(\"yy\"); \n");
            sb.Append("				} \n");
            sb.Append("				day_at = tmpDate.indexOf(\"dd\"); \n");
            sb.Append("				if (day_at == -1)\n");
            sb.Append("				{ \n");
            sb.Append("					day_at = tmpDate.indexOf(\"d\"); \n");
            sb.Append("				} \n");
            sb.Append("				if (year_at >= day_at)\n");
            sb.Append("				{ \n");
            sb.Append("					tmpYear = this.GetFormatYear(two_at); \n");
            sb.Append("					tmpDay = one_at; \n");
            sb.Append("				} \n");
            sb.Append("			} \n");
            sb.Append("			return(new Date(tmpYear, tmpMonth, tmpDay)); \n");
            sb.Append("		}\n");
            sb.Append("		find_tag = \"000\"; \n");
            sb.Append("		for (i=2;i>=0;i--)\n");
            sb.Append("		{ \n");
            sb.Append("			if (parseInt(aryDate[i],10)>31)\n");
            sb.Append("			{ \n");
            sb.Append("				tmpYear = this.GetFormatYear(parseInt(aryDate[i],10)); \n");
            sb.Append("				switch (i)\n");
            sb.Append("				{ \n");
            sb.Append("					case 0: \n");
            sb.Append("					find_tag = \"100\"; \n");
            sb.Append("					one_at = parseInt(aryDate[1],10); \n");
            sb.Append("					two_at = parseInt(aryDate[2],10); \n");
            sb.Append("					break; \n");
            sb.Append("					case 1: \n");
            sb.Append("					find_tag = \"010\"; \n");
            sb.Append("					one_at = parseInt(aryDate[0],10); \n");
            sb.Append("					two_at = parseInt(aryDate[2],10); \n");
            sb.Append("					break; \n");
            sb.Append("					case 2: \n");
            sb.Append("					find_tag = \"001\"; \n");
            sb.Append("					one_at = parseInt(aryDate[0],10); \n");
            sb.Append("					two_at = parseInt(aryDate[1],10); \n");
            sb.Append("					break; \n");
            sb.Append("				} \n");
            sb.Append("			} \n");
            sb.Append("		} \n");
            sb.Append("		if (find_tag==\"000\")\n");
            sb.Append("		{ \n");
            sb.Append("			tmpDate = this.DateFormat; \n");
            sb.Append("			year_at = tmpDate.indexOf(\"yyyy\"); \n");
            sb.Append("			if (year_at == -1)\n");
            sb.Append("			{ \n");
            sb.Append("				year_at = tmpDate.indexOf(\"yy\"); \n");
            sb.Append("			} \n");
            sb.Append("			month_at = tmpDate.indexOf(\"<MMMMMM>\"); \n");
            sb.Append("			if (month_at == -1)\n");
            sb.Append("			{ \n");
            sb.Append("				month_at = tmpDate.indexOf(\"<MMM>\"); \n");
            sb.Append("			} \n");
            sb.Append("			if (month_at == -1)\n");
            sb.Append("			{ \n");
            sb.Append("				month_at = tmpDate.indexOf(\"MM\"); \n");
            sb.Append("			} \n");
            sb.Append("			if (month_at == -1)\n");
            sb.Append("			{ \n");
            sb.Append("				month_at = tmpDate.indexOf(\"M\"); \n");
            sb.Append("			} \n");
            sb.Append("			day_at = tmpDate.indexOf(\"dd\"); \n");
            sb.Append("			if (day_at == -1)\n");
            sb.Append("			{ \n");
            sb.Append("				day_at = tmpDate.indexOf(\"d\"); \n");
            sb.Append("			} \n");
            sb.Append("			if ((year_at>month_at)&&(year_at>day_at))\n");
            sb.Append("			{ \n");
            sb.Append("				find_tag=\"001\" \n");
            sb.Append("			} \n");
            sb.Append("			if ((year_at>month_at)&&(year_at<=day_at))\n");
            sb.Append("			{ \n");
            sb.Append("				find_tag=\"010\"; \n");
            sb.Append("			} \n");
            sb.Append("			if ((year_at<=month_at)&&(year_at>day_at))\n");
            sb.Append("			{ \n");
            sb.Append("				find_tag=\"010\"; \n");
            sb.Append("			} \n");
            sb.Append("			if ((year_at<=month_at)&&(year_at<=day_at))\n");
            sb.Append("			{ \n");
            sb.Append("				find_tag=\"100\"; \n");
            sb.Append("			} \n");
            sb.Append("			switch (find_tag)\n");
            sb.Append("			{ \n");
            sb.Append("				case \"100\": \n");
            sb.Append("				tmpYear = parseInt(aryDate[0],10); \n");
            sb.Append("				one_at = parseInt(aryDate[1],10); \n");
            sb.Append("				two_at = parseInt(aryDate[2],10); \n");
            sb.Append("				break; \n");
            sb.Append("				case \"010\": \n");
            sb.Append("				one_at = parseInt(aryDate[0],10); \n");
            sb.Append("				tmpYear = parseInt(aryDate[1],10); \n");
            sb.Append("				two_at = parseInt(aryDate[2],10); \n");
            sb.Append("				break; \n");
            sb.Append("				case \"001\": \n");
            sb.Append("				one_at = parseInt(aryDate[0],10); \n");
            sb.Append("				two_at = parseInt(aryDate[1],10); \n");
            sb.Append("				tmpYear = parseInt(aryDate[2],10); \n");
            sb.Append("				break; \n");
            sb.Append("			} \n");
            sb.Append("			tmpYear = this.GetFormatYear(tmpYear); \n");
            sb.Append("		} \n");
            sb.Append("\n");
            sb.Append("		if (find_tag!=\"000\")\n");
            sb.Append("		{ \n");
            sb.Append("			if ((one_at>12)&&(two_at>12))\n");
            sb.Append("			{ \n");
            sb.Append("				return(defDate); \n");
            sb.Append("			} \n");
            sb.Append("			if (one_at<=12)\n");
            sb.Append("			{ \n");
            sb.Append("				if (two_at > this.GetMonthDays(tmpYear,one_at-1))\n");
            sb.Append("				{ \n");
            sb.Append("					return(new Date(tmpYear, one_at-1, this.GetMonthDays(tmpYear,one_at-1))); \n");
            sb.Append("				} \n");
            sb.Append("				if (two_at>12)\n");
            sb.Append("				{ \n");
            sb.Append("					return(new Date(tmpYear, one_at-1, two_at)); \n");
            sb.Append("				} \n");
            sb.Append("			} \n");
            sb.Append("			if (two_at<=12)\n");
            sb.Append("			{ \n");
            sb.Append("				if (one_at > this.GetMonthDays(tmpYear,two_at-1))\n");
            sb.Append("				{ \n");
            sb.Append("					return(new Date(tmpYear, two_at-1, this.GetMonthDays(tmpYear,two_at-1))); \n");
            sb.Append("				} \n");
            sb.Append("				if (one_at>12)\n");
            sb.Append("				{ \n");
            sb.Append("					return(new Date(tmpYear, two_at-1, one_at)); \n");
            sb.Append("				} \n");
            sb.Append("			} \n");
            sb.Append("			if ((one_at<=12)&&(two_at<=12))\n");
            sb.Append("			{ \n");
            sb.Append("				tmpMonth = one_at-1; \n");
            sb.Append("				tmpDay = two_at; \n");
            sb.Append("				tmpDate = this.DateFormat; \n");
            sb.Append("				month_at = tmpDate.indexOf(\"MMMMMM\"); \n");
            sb.Append("				if (month_at == -1)\n");
            sb.Append("				{ \n");
            sb.Append("					month_at = tmpDate.indexOf(\"MMM\"); \n");
            sb.Append("				} \n");
            sb.Append("				if (month_at == -1)\n");
            sb.Append("				{ \n");
            sb.Append("					month_at = tmpDate.indexOf(\"MM\"); \n");
            sb.Append("				} \n");
            sb.Append("				if (month_at == -1)\n");
            sb.Append("				{ \n");
            sb.Append("					month_at = tmpDate.indexOf(\"M\"); \n");
            sb.Append("				} \n");
            sb.Append("				day_at = tmpDate.indexOf(\"dd\"); \n");
            sb.Append("				if (day_at == -1)\n");
            sb.Append("				{ \n");
            sb.Append("					day_at = tmpDate.indexOf(\"d\"); \n");
            sb.Append("				} \n");
            sb.Append("				if (month_at >= day_at)\n");
            sb.Append("				{ \n");
            sb.Append("					tmpMonth = two_at-1; \n");
            sb.Append("					tmpDay = one_at; \n");
            sb.Append("				} \n");
            sb.Append("				return(new Date(tmpYear, tmpMonth, tmpDay)); \n");
            sb.Append("			} \n");
            sb.Append("		} \n");
            sb.Append("	}\n");
            sb.Append("	//**GetTextDate End**\n");
            sb.Append("	\n");
            sb.Append("	//格式花年为4位纪元\n");
            sb.Append("	this.GetFormatYear = function(theYear)\n");
            sb.Append("	{		\n");
            sb.Append("		var tmpYear = theYear; \n");
            sb.Append("		if (tmpYear < 100)\n");
            sb.Append("		{ \n");
            sb.Append("			tmpYear += 1900; \n");
            sb.Append("			if (tmpYear < 1970)\n");
            sb.Append("			{ \n");
            sb.Append("				tmpYear += 100; \n");
            sb.Append("			} \n");
            sb.Append("		} \n");
            sb.Append("\n");
            sb.Append("		if (tmpYear < this.MinYear)\n");
            sb.Append("		{ \n");
            sb.Append("			tmpYear = this.MinYear; \n");
            sb.Append("		} \n");
            sb.Append("\n");
            sb.Append("		if (tmpYear > this.MaxYear)\n");
            sb.Append("		{ \n");
            sb.Append("			tmpYear = this.MaxYear; \n");
            sb.Append("		}\n");
            sb.Append("		return(tmpYear); \n");
            sb.Append("	}\n");
            sb.Append("	\n");
            sb.Append("	//获取日期\n");
            sb.Append("	//**GetMonthDays Start**\n");
            sb.Append("	this.GetMonthDays = function(theYear, theMonth)\n");
            sb.Append("	{\n");
            sb.Append("		var theDays = new Array(31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31);\n");
            sb.Append("		var theMonthDay = 0, tmpYear = this.GetFormatYear(theYear); \n");
            sb.Append("		theMonthDay = theDays[theMonth]; \n");
            sb.Append("		if (theMonth == 1)\n");
            sb.Append("		{ //theMonth is February \n");
            sb.Append("			if(((tmpYear % 4 == 0) && (tmpYear % 100 != 0)) || (tmpYear % 400 == 0))\n");
            sb.Append("			{ \n");
            sb.Append("				theMonthDay++; \n");
            sb.Append("			} \n");
            sb.Append("		} \n");
            sb.Append("		return(theMonthDay); \n");
            sb.Append("	}\n");
            sb.Append("	//**GetMonthDays End**\n");
            sb.Append("	\n");
            sb.Append("	\n");
            sb.Append("	//格式化日期\n");
            sb.Append("	this.SetDateFormat = function(theYear, theMonth, theDay)\n");
            sb.Append("	{\n");
            sb.Append("		var theDate = this.DateFormat; \n");
            sb.Append("		var tmpYear = this.GetFormatYear(theYear); \n");
            sb.Append("		var tmpMonth = theMonth; \n");
            sb.Append("		if (tmpMonth < 0)\n");
            sb.Append("		{ \n");
            sb.Append("			tmpMonth = 0; \n");
            sb.Append("		} \n");
            sb.Append("		if (tmpMonth > 11)\n");
            sb.Append("		{ \n");
            sb.Append("			tmpMonth = 11; \n");
            sb.Append("		} \n");
            sb.Append("		var tmpDay = theDay; \n");
            sb.Append("		if (tmpDay < 1)\n");
            sb.Append("		{ \n");
            sb.Append("			tmpDay = 1; \n");
            sb.Append("		}\n");
            sb.Append("		else\n");
            sb.Append("		{ \n");
            sb.Append("			tmpDay = this.GetMonthDays(tmpYear, tmpMonth); \n");
            sb.Append("			if (theDay < tmpDay)\n");
            sb.Append("			{ \n");
            sb.Append("				tmpDay = theDay; \n");
            sb.Append("			} \n");
            sb.Append("		} \n");
            sb.Append("		theDate = theDate.replace(/yyyy/g, tmpYear.toString()); \n");
            sb.Append("		theDate = theDate.replace(/yy/g, tmpYear.toString().substr(2,2)); \n");
            sb.Append("		theDate = theDate.replace(/MMMMMM/g, this.MonthName[tmpMonth]); \n");
            sb.Append("		theDate = theDate.replace(/MMM/g, this.MonthName[tmpMonth].substr(0,3)); \n");
            sb.Append("		if (theMonth < 9)\n");
            sb.Append("		{ \n");
            sb.Append("			theDate = theDate.replace(/MM/g, \"0\" + (tmpMonth + 1).toString()); \n");
            sb.Append("		}\n");
            sb.Append("		else\n");
            sb.Append("		{ \n");
            sb.Append("			theDate = theDate.replace(/MM/g, (tmpMonth + 1).toString()); \n");
            sb.Append("		} \n");
            sb.Append("		theDate = theDate.replace(/M/g, (tmpMonth + 1).toString()); \n");
            sb.Append("		if (theDay < 10)\n");
            sb.Append("		{ \n");
            sb.Append("			theDate = theDate.replace(/dd/g, \"0\" + tmpDay.toString()); \n");
            sb.Append("		}\n");
            sb.Append("		else\n");
            sb.Append("		{ \n");
            sb.Append("			theDate = theDate.replace(/dd/g, tmpDay.toString()); \n");
            sb.Append("		}\n");
            sb.Append("		theDate = theDate.replace(/d/g, tmpDay.toString()); \n");
            sb.Append("		return(theDate); \n");
            sb.Append("	}\n");
            sb.Append("	\n");
            sb.Append("	this.UpdateCalendarGrid = function(theObject)\n");
            sb.Append("	{\n");
            sb.Append("		var theTextObject = this.Source; \n");
            sb.Append("		var theYearObject = document.all.item(\"_YearList\"); \n");
            sb.Append("		var theMonthObject = document.all.item(\"_MonthList\"); \n");
            sb.Append("		var theDayObject = document.all.item(\"_DayList\"); \n");
            sb.Append("		var tmpName = theObject.id;\n");
            sb.Append("		switch (tmpName)\n");
            sb.Append("		{ \n");
            sb.Append("			case \"_goPreviousMonth\": //go previous month button \n");
            sb.Append("			theObject.disabled = true; \n");
            sb.Append("			this.CreateCalendarGrid(parseInt(theYearObject.value, 10), parseInt(theMonthObject.value, 10) - 1, parseInt(theDayObject.value, 10)); \n");
            sb.Append("			theObject.disabled = false; \n");
            sb.Append("			break; \n");
            sb.Append("			case \"_goNextMonth\": //go next month button \n");
            sb.Append("			theObject.disabled = true;\n");
            sb.Append("			this.CreateCalendarGrid(parseInt(theYearObject.value, 10), parseInt(theMonthObject.value, 10) + 1, parseInt(theDayObject.value, 10)); \n");
            sb.Append("			theObject.disabled = false; \n");
            sb.Append("			break; \n");
            sb.Append("			case \"_YearList\": //year list \n");
            sb.Append("			this.CreateCalendarGrid(parseInt(theYearObject.value, 10), parseInt(theMonthObject.value, 10), parseInt(theDayObject.value, 10)); \n");
            sb.Append("			break; \n");
            sb.Append("			case \"_MonthList\": //month list \n");
            sb.Append("			this.CreateCalendarGrid(parseInt(theYearObject.value, 10), parseInt(theMonthObject.value, 10), parseInt(theDayObject.value, 10)); \n");
            sb.Append("			break; \n");
            sb.Append("			default: \n");
            sb.Append("			return; \n");
            sb.Append("		} \n");
            sb.Append("	}\n");
            sb.Append("	\n");
            sb.Append("	this.DeleteCalendarPad = function(forceToClose)\n");
            sb.Append("	{ \n");
            sb.Append("		var theCalendarPadObject = document.all.item(\"_CalendarPad\"); \n");
            sb.Append("		if (theCalendarPadObject == null)\n");
            sb.Append("		{ \n");
            sb.Append("			return; \n");
            sb.Append("		}\n");
            sb.Append("		var tmpObject;\n");
            sb.Append("		if(!forceToClose)\n");
            sb.Append("		{\n");
            sb.Append("			tmpObject = document.activeElement;\n");
            sb.Append("			while (tmpObject != null)\n");
            sb.Append("			{ \n");
            sb.Append("				if (tmpObject.id == \"_CalendarPad\")\n");
            sb.Append("				{ \n");
            sb.Append("					return; \n");
            sb.Append("				} \n");
            sb.Append("				if (tmpObject.id == \"_CalendarGrid\")\n");
            sb.Append("				{\n");
            sb.Append("					return; \n");
            sb.Append("				} \n");
            sb.Append("				if (tmpObject == this.Source)\n");
            sb.Append("				{\n");
            sb.Append("					return; \n");
            sb.Append("				} \n");
            sb.Append("				if (tmpObject.id == \"_goPreviousMonth\")\n");
            sb.Append("				{ \n");
            sb.Append("					return; \n");
            sb.Append("				} \n");
            sb.Append("				if (tmpObject.id == \"_goNextMonth\")\n");
            sb.Append("				{ \n");
            sb.Append("					return; \n");
            sb.Append("				} \n");
            sb.Append("				if (tmpObject.id == \"_YearList\")\n");
            sb.Append("				{\n");
            sb.Append("					return; \n");
            sb.Append("				}\n");
            sb.Append("				if (tmpObject.id == \"_MonthList\")\n");
            sb.Append("				{ \n");
            sb.Append("					return; \n");
            sb.Append("				}\n");
            sb.Append("				if (tmpObject.id == \"_DayList\")\n");
            sb.Append("				{\n");
            sb.Append("					return; \n");
            sb.Append("				} \n");
            sb.Append("				tmpObject = tmpObject.parentElement; \n");
            sb.Append("			}\n");
            sb.Append("		}\n");
            sb.Append("		//Delete CalendarPad\n");
            sb.Append("		if (tmpObject == null)\n");
            sb.Append("		{ \n");
            sb.Append("			theCalendarPadObject.outerHTML = \"\"; \n");
            sb.Append("			this.boolCalendarPadExist = false;\n");
            sb.Append("			var theDate = new Date(this.GetTextDate(this.Source.value)); \n");
            sb.Append("			if (isNaN(theDate))\n");
            sb.Append("			{ \n");
            sb.Append("				this.Source.value = \"\"; \n");
            sb.Append("			}\n");
            sb.Append("			else\n");
            sb.Append("			{ \n");
            sb.Append("				this.Source.value = this.SetDateFormat(theDate.getFullYear(), theDate.getMonth(), theDate.getDate()); \n");
            sb.Append("			}\n");
            sb.Append("			if(forceToClose)\n");
            sb.Append("				this.Source.blur();\n");
            sb.Append("			this.Source = null; \n");
            sb.Append("		} \n");
            sb.Append("	} \n");
            sb.Append("	\n");
            sb.Append("	this.Show = function(targetObject,minYear,maxYear,mainColor,shadow,alpha,bgTitle)\n");
            sb.Append("	{\n");
            sb.Append("	    if(!window.event)\n");
            sb.Append("	    {\n");
            sb.Append("	        targetObject.readOnly=false;\n");
            sb.Append("	        addDateCheck(targetObject);\n");
            sb.Append("	        return;\n");
            sb.Append("	    }else\n");
            sb.Append("	    {\n");
            sb.Append("	        targetObject.readOnly=true;\n");
            sb.Append("	    }\n");
            sb.Append("	    this.MinYear = minYear;\n");
            sb.Append("	    this.MaxYear = maxYear;\n");
            sb.Append("	    this.MainColor = mainColor;\n");
            sb.Append("	    this.Shadow = shadow;\n");
            sb.Append("	    this.Alpha = alpha;\n");
            sb.Append("	    this.BgTitle = bgTitle;\n");
            sb.Append("		if(targetObject == undefined)\n");
            sb.Append("		{\n");
            sb.Append("			alert(\"未设置接受日期返回值的对像!\");\n");
            sb.Append("			return false;\n");
            sb.Append("		}\n");
            sb.Append("		else\n");
            sb.Append("		{\n");
            sb.Append("			this.Source = targetObject;\n");
            sb.Append("			if(targetObject.value != \"\")\n");
            sb.Append("			{\n");
            sb.Append("				webCalendar.calendar_state[targetObject.id] = true;\n");
            sb.Append("			}\n");
            sb.Append("		}\n");
            sb.Append("		var theCalendarPadObject = document.all.item(\"_CalendarPad\"); \n");
            sb.Append("		if (theCalendarPadObject != null)\n");
            sb.Append("		{ \n");
            sb.Append("			return; \n");
            sb.Append("		}\n");
            sb.Append("		else if(!this.boolCalendarPadExist)\n");
            sb.Append("		{\n");
            sb.Append("			this.CreateCalendarPad();\n");
            sb.Append("		}\n");
            sb.Append("		else return;\n");
            sb.Append("	}\n");
            sb.Append("	\n");
            sb.Append("	function addDateCheck(txt)\n");
            sb.Append("	{\n");
            sb.Append("	    txt.form.onsubmit=function()\n");
            sb.Append("	    {\n");
            sb.Append("	        if(!isDate(txt.value))\n");
            sb.Append("            {\n");
            sb.Append("                alert(\"请输入正确的日期格式(如:2008-08-08)\");\n");
            sb.Append("                txt.focus();\n");
            sb.Append("                return false;\n");
            sb.Append("            }\n");
            sb.Append("            return true;\n");
            sb.Append("        }\n");
            sb.Append("	}\n");
            sb.Append("	\n");
            sb.Append("	function isDate(dateStr) \n");
            sb.Append("	{ \n");
            sb.Append("	    if(dateStr==\"\" || dateStr==null)\n");
            sb.Append("	    {\n");
            sb.Append("	        return true;\n");
            sb.Append("	    }\n");
            sb.Append("        var datePat = /^(\\d{4})(\\/|-)(\\d{1,2})(\\/|-)(\\d{1,2})$/; \n");
            sb.Append("        var matchArray = dateStr.match(datePat); // is the format ok? \n");
            sb.Append("\n");
            sb.Append("        if (matchArray == null) { \n");
            sb.Append("            //alert(\"Please enter date as either mm/dd/yyyy or mm-dd-yyyy.\"); \n");
            sb.Append("            return false; \n");
            sb.Append("        } \n");
            sb.Append("\n");
            sb.Append("//        month = matchArray[1]; // parse date into variables \n");
            sb.Append("//        day = matchArray[3]; \n");
            sb.Append("//        year = matchArray[5]; \n");
            sb.Append("        year=matchArray[1];\n");
            sb.Append("        month = matchArray[3];\n");
            sb.Append("        day = matchArray[5];\n");
            sb.Append("//        alert(year+\",\"+month+\",\"+day);\n");
            sb.Append("        if (month < 1 || month > 12) { // check month range \n");
            sb.Append("            return false; \n");
            sb.Append("        } \n");
            sb.Append("\n");
            sb.Append("        if (day < 1 || day > 31) { \n");
            sb.Append("            //alert(\"Day must be between 1 and 31.\"); \n");
            sb.Append("            return false; \n");
            sb.Append("        } \n");
            sb.Append("\n");
            sb.Append("        if ((month==4 || month==6 || month==9 || month==11) && day==31) { \n");
            sb.Append("            //alert(\"Month \"+month+\" doesn't have 31 days!\") \n");
            sb.Append("            return false; \n");
            sb.Append("        } \n");
            sb.Append("\n");
            sb.Append("        if (month == 2) { // check for february 29th \n");
            sb.Append("            var isleap = (year % 4 == 0 && (year % 100 != 0 || year % 400 == 0)); \n");
            sb.Append("            if (day > 29 || (day==29 && !isleap)) { \n");
            sb.Append("                //alert(\"February \" + year + \" doesn't have \" + day + \" days!\"); \n");
            sb.Append("                return false; \n");
            sb.Append("            } \n");
            sb.Append("        } \n");
            sb.Append("        return true; // date is valid \n");
            sb.Append("    }\n");
            sb.Append("\n");
            sb.Append("	this.Hidden = function(forceToClose)\n");
            sb.Append("	{\n");
            sb.Append("		if(this.Source == null)\n");
            sb.Append("			return;\n");
            sb.Append("			\n");
            sb.Append("		var theCalendarPadObject = document.all.item(\"_CalendarPad\");\n");
            sb.Append("		if(!this.calendar_state[this.Source.id])\n");
            sb.Append("		{\n");
            sb.Append("			this.Source.value = \"\";\n");
            sb.Append("		}\n");
            sb.Append("		else if(this.Source.value == \"\")\n");
            sb.Append("		{\n");
            sb.Append("			this.calendar_state[this.Source.id] = false;		\n");
            sb.Append("		}\n");
            sb.Append("		this.DeleteCalendarPad(forceToClose);\n");
            sb.Append("		if(this.Source == null)\n");
            sb.Append("		{\n");
            sb.Append("			theCalendarPadObject = null\n");
            sb.Append("		}\n");
            sb.Append("	}\n");
            sb.Append("	this.txtClear = function(e,txt)\n");
            sb.Append("    {\n");
            sb.Append("        if(window.event)\n");
            sb.Append("        {\n");
            sb.Append("            var eve= window.event;\n");
            sb.Append("            if(eve.keyCode==8)\n");
            sb.Append("            {\n");
            sb.Append("                txt.value=\"\";\n");
            sb.Append("            }\n");
            sb.Append("        }\n");
            sb.Append("    }\n");
            sb.Append("}\n");
            sb.Append("\n");
            sb.Append("function WebCalendar_Click(theYear, theMonth, theDay)\n");
            sb.Append("{\n");
            sb.Append("	webCalendar.calendar_state[webCalendar.Source.id] = true;\n");
            sb.Append("	webCalendar.CreateCalendarGrid(theYear, theMonth, theDay, true);\n");
            sb.Append("}\n");
            sb.Append("\n");



            //js.Append("</script>\n");

            return sb.ToString();
        }
        public TYWebCalendar()
        {
            //CurrentDate=DateTime.Now;
            Shadow = Color.FromArgb(102, 102, 102);
            MainColor = Color.FromArgb(152, 186, 169);
            MinYear = 1900;
            MaxYear = 2020;
            Alpha = 100;
        }
    }
}
