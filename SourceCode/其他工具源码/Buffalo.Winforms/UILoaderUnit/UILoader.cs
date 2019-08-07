using Buffalo.Winforms;
using System;
using System.Collections.Generic;
using System.Drawing;

using System.Reflection;
using System.Text;

using System.Windows.Forms;
using System.Xml;

namespace Buffalo.Winforms.UILoaderUnit
{
    public class UILoader
    {
        private static UIMapID _uiMap = new UIMapID();

        /// <summary>
        /// UI表
        /// </summary>
        public static UIMapID UiMap
        {
            get { return UILoader._uiMap; }
        }

        private static Dictionary<string, Image> _dicImages = null;

        /// <summary>
        /// 设置或获取图片集合
        /// </summary>
        public static Dictionary<string, Image> Images
        {
            get { return _dicImages; }
            set { _dicImages = value; }
        }

        /// <summary>
        /// 加载程序集
        /// </summary>
        /// <param name="ass"></param>
        public static void LoadAssembly(Assembly ass)
        {
            foreach (Type t in ass.GetTypes()) 
            {
                _uiMap.AddModel(t);
            }
        }

        /// <summary>
        /// 加载UI
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="pnlToobar"></param>
        /// <param name="pnlContent"></param>
        public static void LoadUI(XmlDocument doc,ToolBarPanel pnlToobar,Panel pnlContent) 
        {
            XmlNodeList lstNodes = doc.GetElementsByTagName("maintype");
            foreach (XmlNode mainNode in lstNodes) 
            {
                XmlAttribute att = mainNode.Attributes["id"];
                string command = null;
                if (att == null) 
                {
                    continue;
                }
                command = att.InnerText;

                att = mainNode.Attributes["text"];
                string text = null;
                if (att == null)
                {
                    continue;
                }
                text = att.InnerText;

                att = mainNode.Attributes["icon"];
                Image imgIcon = null;
                if (att != null)
                {
                    string resId=att.InnerText;
                    if (_dicImages != null) 
                    {
                        _dicImages.TryGetValue(resId, out imgIcon);
                    }
                    //imgIcon = Resources.ResourceManager.GetObject(resId, Resources.Culture) as Image;

                }

                pnlToobar.AddButton(text, imgIcon,command);


                Control tabCtr = LoadChildItem(mainNode);
                pnlContent.Controls.Add(tabCtr);
                tabCtr.Dock = DockStyle.Fill;
                tabCtr.Hide();
                tabCtr.Tag = command;

            }
        }



        /// <summary>
        /// 显示UI
        /// </summary>
        /// <param name="command">命令</param>
        /// <param name="pnlContent">容器</param>
        public static void ShowUI(string command, Panel pnlContent) 
        {
            foreach (Control ctr in pnlContent.Controls) 
            {
                Control tabCtr = ctr;
                if (tabCtr == null) 
                {
                    continue;
                }
                string curCommand = tabCtr.Tag as string;
                if (string.IsNullOrEmpty(curCommand)) 
                {
                    continue;
                }
                if (string.Equals(curCommand, command, StringComparison.CurrentCultureIgnoreCase))
                {
                    tabCtr.Show();
                    OnSelectTab(tabCtr, true);
                }
                else 
                {
                    if (tabCtr.Visible) 
                    {
                        OnSelectTab(tabCtr, false);
                    }
                    tabCtr.Hide();
                }
            }
        }

        private static Control LoadChildItem(XmlNode mainNode) 
        {

            TabControl tabCtr = new TabControl();
            
            tabCtr.Font = new Font("微软黑体", 12);
            tabCtr.Padding = new Point(10, 5);
            foreach (XmlNode subNode in mainNode.ChildNodes)
            {
                if (!string.Equals(subNode.Name, "subtype", StringComparison.CurrentCultureIgnoreCase))
                {
                    continue;
                }
                XmlAttribute att = subNode.Attributes["id"];
                string command = null;
                if (att == null)
                {
                    continue;
                }
                command = att.InnerText;

                att = subNode.Attributes["text"];
                string text = null;
                if (att == null)
                {
                    continue;
                }
                text = att.InnerText;

                TabPage page = new TabPage();
                page.Name = "tab" + command;
                page.Text = text;
                page.AutoScroll = true;

                tabCtr.Controls.Add(page);
                Panel pnlfPage = null;
                
                //加载控件
                foreach (XmlNode modelNode in subNode.ChildNodes)
                {
                    if (!string.Equals(modelNode.Name, "model", StringComparison.CurrentCultureIgnoreCase))
                    {
                        continue;
                    }
                    att = modelNode.Attributes["id"];
                    command = null;
                    if (att == null)
                    {
                        continue;
                    }
                    command = att.InnerText;

                    att = modelNode.Attributes["fill"];
                    bool fill = false;
                    if (att != null)
                    {
                        fill = att.InnerText != "0";
                    }
                    Type ctrType = _uiMap.GetModelType(command);
                    if (ctrType == null)
                    {
                        throw new Exception("找不到ID为:" + command + "的组件");
                    }
                    FormModelBase ctr = Activator.CreateInstance(ctrType) as FormModelBase;
                    ctr.OnShowToolTip += ctr_OnShowToolTip;
                    if (fill)
                    {
                        if (pnlfPage==null)
                        {
                            pnlfPage=new Panel();
                        }
                        pnlfPage.Controls.Add(ctr);
                        ctr.Dock = DockStyle.Fill;
                        
                        break;
                    }
                    else
                    {
                        if (pnlfPage == null)
                        {
                            pnlfPage = new FlowLayoutPanel();
                        }
                        
                        pnlfPage.Controls.Add(ctr);
                    }
                }
                pnlfPage.AutoScroll = true;
                pnlfPage.Font = SystemFonts.DefaultFont;

                if (mainNode.ChildNodes.Count <= 1) 
                {
                    return pnlfPage;
                }

                page.Controls.Add(pnlfPage);
                pnlfPage.Dock = DockStyle.Fill;
               

            }
            tabCtr.SelectedIndexChanged += tabCtr_SelectedIndexChanged;
            tabCtr.Deselecting += tabCtr_Deselecting;
            return tabCtr;
        }

        static void ctr_OnShowToolTip(UserControl ctr,string title, string message,bool isErr) 
        {
            IShowToolTip frm = ctr.ParentForm as IShowToolTip;
            if (frm != null)
            {
                frm.ShowTooltip(ctr,title , message,isErr);
            }
        }

        public static void ShowToolTip(ToolTip tp, Form frm, Point mousePosition, Control sender, string title, string message, bool isErr) 
        {
            if (isErr)
            {
                tp.ToolTipIcon = ToolTipIcon.Error;
            }
            else
            {
                tp.ToolTipIcon = ToolTipIcon.Info;
            }
            Point mousep = frm.PointToClient(mousePosition);
            tp.ToolTipTitle = title;
            //LastMsg = Infomsg;
            tp.IsBalloon = true;
            tp.Show(message, frm, mousep.X - 20, mousep.Y - 60, 2000);
            tp.Hide(frm);
            tp.Show(message, frm, mousep.X - 20, mousep.Y - 60, 2000);
        }

        static void tabCtr_Deselecting(object sender, TabControlCancelEventArgs e)
        {
            Control rc = sender as Control;
            OnSelectTab(rc, false);
        }

        private static void SelectPanel(Panel pnl, bool select) 
        {
            FormModelBase model = null;
            foreach (Control innerctr in pnl.Controls)
            {

                model = innerctr as FormModelBase;
                if (model != null)
                {
                    if (select)
                    {
                        model.ShowUI();
                    }
                    else
                    {
                        model.HideUI();
                    }
                }
            }
        }

        private static void OnSelectTab(Control curctr,bool select) 
        {
            TabControl rc = curctr as TabControl;
            if (rc != null)
            {
                TabPage tb = rc.TabPages[rc.SelectedIndex];
                FormModelBase model = null;
                foreach (Control ctr in tb.Controls)
                {
                    Panel pnl = ctr as Panel;
                    if (pnl != null)
                    {
                        SelectPanel(pnl, select);
                        continue;
                    }
                    model = ctr as FormModelBase;
                    if (model != null)
                    {
                        if (select)
                        {
                            model.ShowUI();
                        }
                        else
                        {
                            model.HideUI();
                        }
                    }
                }
            }
            else 
            {
                Panel p = curctr as Panel;
                if (p != null)
                {
                    SelectPanel(p, select);
                }
            }
        }

        static void tabCtr_SelectedIndexChanged(object sender, EventArgs e)
        {
            Control rc = sender as Control;
            OnSelectTab(rc, true);
        }
    }
}
