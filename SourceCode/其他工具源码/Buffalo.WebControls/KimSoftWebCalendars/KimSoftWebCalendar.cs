using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Buffalo.WebKernel.WebCommons.ContorlCommons;
using System.Drawing;
using Buffalo.WebKernel.WebCommons;

namespace Buffalo.WebControls.KimSoftWebCalendars
{
    [DefaultProperty("CurrentDate")]
    [ValidationProperty("Text")] 
    [ToolboxData("<{0}:KimSoftWebCalendar runat=server></{0}:KimSoftWebCalendar>")]
    public class KimSoftWebCalendar : WebControl, INamingContainer,ITextControl
    {
        private static bool isChecked = false;//�Ƿ��Ѿ������ļ�
        private const string jsName = "KimSoftWebCalendar.js";

        

        /// <summary>
        /// �ı�
        /// </summary>
        [Bindable(true)]
        [Category("Appearance")]
        [Localizable(true)]
        public string Text
        {
            get
            {
                return txtValue.Text;
            }
            set
            {
                if (string.IsNullOrEmpty(value)) 
                {
                    txtValue.Text = "";
                    return;
                }
                DateTime dt = DateTime.MinValue;
                if (DateTime.TryParse(value, out dt))
                {
                    txtValue.Text = dt.ToString("yyyy-MM-dd");
                }
            }
        }

        

        /// <summary>
        /// ��ǰ����
        /// </summary>
        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("")]
        [Localizable(true)]
        public DateTime CurrentDate
        {
            get
            {
                if (Text == "")
                {
                    return DateTime.MinValue;
                }
                DateTime dt = Convert.ToDateTime(Text);
                return dt;
            }
            set
            {
                Text = value.ToShortDateString();
            }
        }

        [Description("��������������ɫ"),
       Category("���")]
        public Color CurWord 
        {
            get 
            {
                if (ViewState["CurWord"]==null) 
                {
                    return Color.Transparent;
                }
                return (Color)ViewState["CurWord"];
            }
            set 
            {
                ViewState["CurWord"] = value;
            }
        }
        /// <summary>
        /// ��ǰ���ڵ����һ��(��2007-1-1�ͷ���2007-1-1 23:59:59 999)
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
        [Description("�������ڵ�Ԫ��Ӱɫ"),
       Category("���")]
        public Color CurBg
        {
            get
            {
                if (ViewState["CurBg"] == null)
                {
                    return Color.Transparent;
                }
                return (Color)ViewState["CurBg"];
            }
            set
            {
                ViewState["CurBg"] = value;
            }
        }

        [Description("������������ɫ"),
       Category("���")]
        public Color SunWord
        {
            get
            {
                if (ViewState["SunWord"] == null)
                {
                    return Color.Transparent;
                }
                return (Color)ViewState["SunWord"];
            }
            set
            {
                ViewState["SunWord"] = value;
            }
        }

        [Description("������������ɫ"),
       Category("���")]
        public Color SatWord
        {
            get
            {
                if (ViewState["SatWord"] == null)
                {
                    return Color.Transparent;
                }
                return (Color)ViewState["SatWord"];
            }
            set
            {
                ViewState["SatWord"] = value;
            }
        }

        [Description("��Ԫ��������ɫ"),
       Category("���")]
        public Color TdWordLight
        {
            get
            {
                if (ViewState["TdWordLight"] == null)
                {
                    return Color.Transparent;
                }
                return (Color)ViewState["TdWordLight"];
            }
            set
            {
                ViewState["TdWordLight"] = value;
            }
        }
        [Description("��Ԫ�����ְ�ɫ"),
       Category("���")]
        public Color TdWordDark
        {
            get
            {
                if (ViewState["TdWordDark"] == null)
                {
                    return Color.Transparent;
                }
                return (Color)ViewState["TdWordDark"];
            }
            set
            {
                ViewState["TdWordDark"] = value;
            }
        }
        [Description("��Ԫ��Ӱɫ"),
       Category("���")]
        public Color TdBgOut
        {
            get
            {
                if (ViewState["TdBgOut"] == null)
                {
                    return Color.Transparent;
                }
                return (Color)ViewState["TdBgOut"];
            }
            set
            {
                ViewState["TdBgOut"] = value;
            }
        }
        [Description("��Ԫ��Ӱɫ"),
       Category("���")]
        public Color TdBgOver
        {
            get
            {
                if (ViewState["TdBgOver"] == null)
                {
                    return Color.Transparent;
                }
                return (Color)ViewState["TdBgOver"];
            }
            set
            {
                ViewState["TdBgOver"] = value;
            }
        }
        [Description("����ͷ������ɫ"),
       Category("���")]
        public Color TrWord
        {
            get
            {
                if (ViewState["TrWord"] == null)
                {
                    return Color.Transparent;
                }
                return (Color)ViewState["TrWord"];
            }
            set
            {
                ViewState["TrWord"] = value;
            }
        }
        [Description("����ͷ��Ӱɫ"),
       Category("���")]
        public Color TrBg
        {
            get
            {
                if (ViewState["TrBg"] == null)
                {
                    return Color.Transparent;
                }
                return (Color)ViewState["TrBg"];
            }
            set
            {
                ViewState["TrBg"] = value;
            }
        }
        [Description("input�ؼ��ı߿���ɫ"),
       Category("���")]
        public Color InputBorder
        {
            get
            {
                if (ViewState["InputBorder"] == null)
                {
                    return Color.Transparent;
                }
                return (Color)ViewState["InputBorder"];
            }
            set
            {
                ViewState["InputBorder"] = value;
            }
        }
        [Description("input�ؼ��ı�Ӱɫ"),
       Category("���")]
        public Color InputBg
        {
            get
            {
                if (ViewState["InputBg"] == null)
                {
                    return Color.Transparent;
                }
                return (Color)ViewState["InputBg"];
            }
            set
            {
                ViewState["InputBg"] = value;
            }
        }

        [Description("�Ƿ�������ʾ,(trueΪ���ģ�falseΪӢ��)"),
       Category("���")]
        public bool IsChinese
        {
            get
            {
                if (ViewState["IsChinese"] == null)
                {
                    return true;
                }
                return (bool)ViewState["IsChinese"];
            }
            set
            {
                ViewState["IsChinese"] = value;
            }
        }
        [Description("��ʼ���"),
       Category("���")]
        public int BeginYear
        {
            get
            {
                if (ViewState["BeginYear"] == null)
                {
                    return 0;
                }
                return (int)ViewState["BeginYear"];
            }
            set
            {
                ViewState["BeginYear"] = value;
            }
        }

        [Description("�������"),
       Category("���")]
        public int EndYear
        {
            get
            {
                if (ViewState["EndYear"] == null)
                {
                    return 0;
                }
                return (int)ViewState["EndYear"];
            }
            set
            {
                ViewState["EndYear"] = value;
            }
        }


        private TextBox txtValue;

       
        /// <summary>
        /// �����Ŀͻ���ID
        /// </summary>
        public string TextClientID 
        {
            get 
            {
                return txtValue.ClientID;
            }
        }
        /// <summary>
        /// ��ʼ����תֵ�Ŀؼ�
        /// </summary>
        private void InitTextControl()
        {
            if (txtValue == null)
            {
                txtValue = new TextBox();

                txtValue.ID = "txtCalendar";
                
            }
        }
        protected override void CreateChildControls()
        {
            this.Controls.Clear();

            //txtProvice.RenderControl(writer);
            //txtCity.RenderControl(writer);
            //txtArea.RenderControl(writer);

            this.Controls.Add(txtValue);
            
            base.CreateChildControls();
            txtValue.Width = this.Width;
            txtValue.Height = this.Height;
        }
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            //string js=CreateJs();
            
            SaveJs();

            if (!Page.ClientScript.IsClientScriptIncludeRegistered(jsName+"Include"))
            {
                Page.ClientScript.RegisterClientScriptInclude(jsName+"Include", JsSaver.GetDefualtJsUrl(jsName));
            }
            if (!Page.ClientScript.IsStartupScriptRegistered("Init"))
            {
                string commonPanl = "document.write('<div id=\"calendarPanel\" style=\"position: absolute;visibility: hidden;z-index: 9999;background-color: #FFFFFF;border: 1px solid #CCCCCC;width:175px;font-size:12px;\"></div>');\n";
                Page.ClientScript.RegisterStartupScript(this.GetType(), "Init", commonPanl, true);
            }
            string lan = "1";//����
            if (IsChinese) 
            {
                lan = "0";
            }
            string showScript = "new Calendar(" + BeginYear.ToString() + ", " + EndYear.ToString() + ", " + lan + ",'yyyy-MM-dd','" +
                ContorlCommon.ToColorString(CurWord) + "','" + ContorlCommon.ToColorString(CurBg) + "','" + ContorlCommon.ToColorString(SunWord) + "','" +
                ContorlCommon.ToColorString(SatWord) + "','" + ContorlCommon.ToColorString(TdWordLight) + "','" + ContorlCommon.ToColorString(TdWordDark) + "','" +
                ContorlCommon.ToColorString(TdBgOut) + "','" + ContorlCommon.ToColorString(TdBgOver) + "','" + ContorlCommon.ToColorString(TrWord) + "'" +
                ",'" + ContorlCommon.ToColorString(TrBg) + "','" + ContorlCommon.ToColorString(InputBorder) + "','" + ContorlCommon.ToColorString(InputBg) + "').show(this,null);";
            txtValue.Attributes.Add("onclick", showScript);
            txtValue.Attributes.Add("readonly", "readonly");
            
        }



        /// <summary>
        /// �����JS�ļ�
        /// </summary>
        private void SaveJs()
        {
            if (!isChecked)
            {
                JsSaver.SaveJS(jsName, CreateJs());
                isChecked = true;
            }
        }
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
        }
        public KimSoftWebCalendar()
        {
            
            IsChinese = true;
            BeginYear = 1900;
            EndYear = DateTime.Now.Year+100;

            CurWord = ContorlCommon.ColorStringToColor("#FFFFFF");
            CurBg = ContorlCommon.ColorStringToColor("#00FF00");
            SunWord = ContorlCommon.ColorStringToColor("#FF0000");
            SatWord = ContorlCommon.ColorStringToColor("#0000FF");
            TdWordLight = ContorlCommon.ColorStringToColor("#333333");
            TdWordDark = ContorlCommon.ColorStringToColor("#CCCCCC");
            TdBgOut = ContorlCommon.ColorStringToColor("#EFEFEF");
            TdBgOver = ContorlCommon.ColorStringToColor("#FFCC00");
            TrWord = ContorlCommon.ColorStringToColor("#FFFFFF");
            TrBg = ContorlCommon.ColorStringToColor("#666666");
            InputBorder = ContorlCommon.ColorStringToColor("#CCCCCC");
            InputBg = ContorlCommon.ColorStringToColor("#EFEFEF");
            this.Width = 100;
            this.Height = 20;
            InitTextControl();

           // ((WebControl)this.Page.FindControl(this.ID)).Height = 20;
        }


        
        /// <summary>
        /// �������ڵ�JS�ű�
        /// </summary>
        /// <returns></returns>
        private string CreateJs()
        {
            return Resource.KimSoftWebCalendar;
        }

        

        
    }
}
