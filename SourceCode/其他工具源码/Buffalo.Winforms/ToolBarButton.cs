using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;

using System.Text;

using System.Windows.Forms;

namespace Buffalo.Winforms
{
    public class ToolBarItem : Button
    {
        public ToolBarItem()
            : base()
        {
            this.Font = new Font("微软雅黑", 10f);
            //this.Size = new Size(100, 80);
            this.ForeColor = Color.Black;
            this.TextAlign = ContentAlignment.BottomCenter;
            this.Cursor = Cursors.Hand;
            this.FlatStyle = FlatStyle.Flat;
            this.FlatAppearance.BorderSize = 0;
            this.ImageAlign = ContentAlignment.TopCenter;
            this.FlatAppearance.MouseOverBackColor = Color.Transparent;
            this.FlatAppearance.MouseDownBackColor = Color.Transparent;
        }


       

        private bool _selected;
        /// <summary>
        /// 选中
        /// </summary>
        public bool Selected
        {
            get 
            { 
                return _selected;
            }
            set 
            { 
                _selected = value;
                RefreashBack();
            }
        }
        /// <summary>
        /// 刷新背景
        /// </summary>
        private void RefreashBack() 
        {
            if (_selected)
            {
                this.BackgroundImage = SelectedBack;
            }
            else
            {
                this.BackgroundImage = UpBack;
            }
        }

        private string _command;

        /// <summary>
        /// 命令 
        /// </summary>
        public string Command
        {
            get { return _command; }
            set { _command = value; }
        }

        private Image _selectedBack;

        /// <summary>
        /// 选中时候的背景
        /// </summary>
        public Image SelectedBack
        {
            get
            {
                if (_selectedBack == null)
                {
                    _selectedBack = SelectedBackImage();
                }
                return _selectedBack;
            }
        }
        private Image SelectedBackImage()
        {
            Bitmap bmp = new Bitmap(this.Size.Width, this.Size.Height);
            using (Graphics grp = Graphics.FromImage(bmp))
            {
                grp.Clear(Color.FromArgb(240, 246, 249));

                Pen linePen = new Pen(Color.FromArgb(34, 137, 204), 5);//设置笔的粗细为
                linePen.DashStyle = DashStyle.Solid;//定义虚线的样式为点
                grp.DrawLine(linePen, 0f, 0, this.Width, 0);
            }
            return bmp;
        }


        private Image _upBack;

        /// <summary>
        /// 未选中时候的背景
        /// </summary>
        public Image UpBack
        {
            get
            {
                if (_upBack == null)
                {
                    _upBack = UpBackImage();
                }
                return _upBack;
            }
        }
        private Image UpBackImage()
        {
            Bitmap bmp = new Bitmap(this.Size.Width, this.Size.Height);
            using (Graphics grp = Graphics.FromImage(bmp))
            {
                grp.Clear(Color.FromArgb(224, 222, 222));
            }
            return bmp;
        }

        private Image _downBack;

        /// <summary>
        /// 按下时候的背景
        /// </summary>
        public Image DownBack
        {
            get
            {
                if (_downBack == null)
                {
                    _downBack = DownBackImage();
                }
                return _downBack;
            }
        }
        private Image DownBackImage()
        {
            Bitmap bmp = new Bitmap(this.Size.Width, this.Size.Height);
            using (Graphics grp = Graphics.FromImage(bmp))
            {
                grp.Clear(Color.FromArgb(210, 206, 206));
                DrawBoder(grp);
            }
            return bmp;
        }

        private Image _overBack;

        /// <summary>
        /// 划过时候的背景
        /// </summary>
        public Image OverBack
        {
            get
            {
                if (_overBack == null)
                {
                    _overBack = DownOverImage();
                }
                return _overBack;
            }
        }
        private Image DownOverImage()
        {
            Bitmap bmp = new Bitmap(this.Size.Width, this.Size.Height);
            using (Graphics grp = Graphics.FromImage(bmp))
            {
                grp.Clear(Color.FromArgb(236, 233, 233));
                DrawBoder(grp);
            }
            return bmp;
        }
        protected override void OnMouseUp(MouseEventArgs mevent)
        {

            base.OnMouseUp(mevent);
        }
        protected override void OnMouseLeave(EventArgs e)
        {
            if (!_selected) 
            {
                RefreashBack();
            }
            base.OnMouseLeave(e);
        }
        protected override void OnMouseDown(MouseEventArgs mevent)
        {
            this.BackgroundImage = DownBack;
            base.OnMouseDown(mevent);

        }

        protected override void OnMouseEnter(EventArgs e)
        {
            if (!_selected) 
            {
                this.BackgroundImage = OverBack;
            }
            base.OnMouseEnter(e);
        }

        private void DrawBoder(Graphics grp) 
        {
            Pen linePen = new Pen(Color.FromArgb(160, 160, 160), 1);//设置笔的粗细为
            grp.DrawLine(linePen, 0, 0, 0, this.Height - 1);
            grp.DrawLine(linePen, this.Width - 1, 0, this.Width - 1, this.Height - 1);
        }
    }
}
