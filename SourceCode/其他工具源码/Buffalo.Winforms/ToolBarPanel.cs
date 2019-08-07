using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;

using System.Text;

using System.Windows.Forms;

namespace Buffalo.Winforms
{
    public delegate bool OnCommand(Control sender, EventArgs e);
    public class ToolBarPanel:Panel
    {
        /// <summary>
        /// 命令按钮按下
        /// </summary>
        public event OnCommand OnCommandClick;

        public ToolBarPanel() :base()
        {
            this.BackColor = Color.FromArgb(224, 222, 222);
            this.Padding = new Padding(0, 0, 0, 1);
        }

        private Dictionary<string, ToolBarItem> _dicCommandButton = new Dictionary<string, ToolBarItem>(StringComparer.CurrentCultureIgnoreCase);

        /// <summary>
        /// 添加按钮
        /// </summary>
        /// <param name="text">文字</param>
        /// <param name="icon">图标</param>
        /// <param name="command">命令</param>
        public void AddButton(String text, Image icon, string command) 
        {
            ToolBarItem btn = new ToolBarItem();
            btn.Width = 80;
            btn.Dock = DockStyle.Left;
            btn.Image = icon;
            btn.Text = text;
            btn.Command = command;
            btn.Click += btn_Click;
            this.Controls.Add(btn);
            _dicCommandButton[command] = btn;
            btn.BringToFront();
        }

        /// <summary>
        /// 选中命令的按钮
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public bool SelectedCommand(string command) 
        {
            ToolBarItem btn = null;
            if (!_dicCommandButton.TryGetValue(command, out btn)) 
            {
                return false;
            }

            btn_Click(btn, new EventArgs());
            return true;
        }
        void btn_Click(object sender, EventArgs e)
        {
            ToolBarItem btn = sender as ToolBarItem;
            if (OnCommandClick != null) 
            {
                bool con = OnCommandClick(btn, e);
                if (!con ) 
                {
                    return;
                }
            }
            _curButtonRec = new RectangleF(btn.Location.X, btn.Location.Y, btn.Size.Width, btn.Size.Height);
            RefreashBackGround();
            ClearSelect();
            btn.Selected = true;

        }

        /// <summary>
        /// 当前按钮的矩形
        /// </summary>
        private RectangleF _curButtonRec;

        private void RefreashBackGround() 
        {
            if (this.Size.Width <= 0 || this.Size.Height <= 0) 
            {
                return;
            }
            Bitmap bmp = new Bitmap(this.Size.Width, this.Size.Height);
            using (Graphics grp = Graphics.FromImage(bmp))
            {
                Pen linePen = new Pen(Color.FromArgb(149, 149, 149), 1);//设置笔的粗细为
                linePen.DashStyle = DashStyle.Solid;//定义虚线的样式为点
                grp.Clear(this.BackColor);
                float y = this.Height - linePen.Width;
                if (_curButtonRec.X > 0)
                {
                    grp.DrawLine(linePen, 0f, y, (float)_curButtonRec.X, y);
                }

                grp.DrawLine(linePen, (float)_curButtonRec.X + _curButtonRec.Width, y, this.Width, y);

                linePen = new Pen(Color.FromArgb(240, 246, 249), 5);//设置笔的粗细为
                grp.DrawLine(linePen, (float)_curButtonRec.X, y, (float)_curButtonRec.X + _curButtonRec.Width, y);
            }
            if (BackgroundImage != null)
            {
                try
                {
                    BackgroundImage.Dispose();
                }
                catch { }
            }
            this.BackgroundImage = bmp;
        }
        protected override void OnSizeChanged(EventArgs e)
        {
            RefreashBackGround();
            base.OnSizeChanged(e);
        }
        private void ClearSelect() 
        {
            foreach (Control ctr in this.Controls) 
            {
                ToolBarItem btn = ctr as ToolBarItem;
                if (btn == null) 
                {
                    continue;
                }
                btn.Selected = false;
            }
        }
    }
}
