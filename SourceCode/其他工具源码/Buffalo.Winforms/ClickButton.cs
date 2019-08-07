using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;

using System.Text;
using System.Windows.Forms;

namespace Buffalo.Winforms
{
    public class ClickButton : Button, ISupportInitialize
    {
        public ClickButton() 
        {
            //this.FlatStyle = FlatStyle.Flat;
            //this.SizeMode = PictureBoxSizeMode.CenterImage;
            this.TextAlign = ContentAlignment.MiddleCenter;
            this.Cursor = Cursors.Hand;
            this.FlatStyle = FlatStyle.Flat;
            this.FlatAppearance.BorderSize = 0;
            this.FlatAppearance.MouseOverBackColor = Color.Transparent;
            this.FlatAppearance.MouseDownBackColor = Color.Transparent;
        }

        public override bool AutoSize
        {
            get
            {
                return false;
            }
            set
            {
                
            }
        }

        
        

        /// <summary>
        /// 创建图片
        /// </summary>
        /// <param name="backColor"></param>
        /// <param name="font"></param>
        /// <param name="color"></param>
        /// <returns></returns>
        private Image CreateBtnPic(Image backColor, Font font, Color color,string text) 
        {
            Image img=new Bitmap(backColor.Size.Width,backColor.Size.Height);
            using (Graphics grp = Graphics.FromImage(img)) 
            {
                
                grp.Clear(this.BackColor);
                grp.DrawImage(backColor,new Point(0,0));
                if (!string.IsNullOrEmpty(text))
                {
                    SizeF fontSize = grp.MeasureString(text, font);
                    float tx = (img.Size.Width - fontSize.Width) / 2;
                    float ty = (img.Size.Height - fontSize.Height) / 2;
                    Brush brs = new SolidBrush(color);
                    grp.DrawString(text, font, brs, new PointF(tx, ty));
                }

            }
            return img;
        }
       
        //protected override void OnCreateControl()
        //{
        //    base.OnCreateControl();


            
        //}

        private Image _upImage;

        public  Image UpImage
        {
            get { return _upImage; }
            set 
            { 
                _upImage = value;
                if (_buttonEnable) 
                {
                    Image = _upImage;
                }
            }
        }

        private bool _buttonEnable=true;

        /// <summary>
        /// 按钮是否可用
        /// </summary>
        [Browsable(true), Category("Appearance"), Localizable(true)]
        public bool ButtonEnable 
        {
            get 
            {
                return _buttonEnable;
            }
            set 
            {
                _buttonEnable = value;
                if (_buttonEnable && _upImage != null)
                {
                    Image = _upImage;
                }
                else if (_buttonEnable == false && DisableImage != null)
                {
                    Image = DisableImage;
                }
            }
        }

        ///// <summary>
        ///// 弹起时候的图片
        ///// </summary>
        //[Browsable(true), Category("Appearance")]
        //public virtual Image UpImage
        //{
        //    get;
        //    set;
        //}
        /// <summary>
        /// 按下时候的图片
        /// </summary>
        [Browsable(true), Category("Appearance"), Localizable(true)]
        public virtual Image DownImage { get; set; }

        /// <summary>
        /// 禁用时候的图片
        /// </summary>
        [Browsable(true), Category("Appearance"), Localizable(true)]
        public virtual Image DisableImage { 
            get; 
            set; 
        }
        
        private Image _enterImage;
        /// <summary>
        /// 进入时候的图片
        /// </summary>
        [Browsable(true), Category("Appearance"), Localizable(true)]
        public virtual Image EnterImage
        {
            get { return _enterImage; }
            set
            {
                _enterImage = value;
            }
        }
        

        protected override void OnMouseDown(MouseEventArgs mevent)
        {
            if (!_buttonEnable)
            {
                return;
            }
            if (_upImage == null)
            {
                this._upImage = Image;
            }
            if(DownImage!=null)
            {
                this.Image = DownImage;
            }
            base.OnMouseDown(mevent);
            
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            if (!_buttonEnable)
            {
                return;
            }
            if (_enterImage != null)
            {
                this.Image = _enterImage;
            }
            base.OnMouseEnter(e);
        }
        
        protected override void OnMouseLeave(EventArgs e)
        {
            if (!_buttonEnable)
            {
                return;
            }
            if (_enterImage != null)
            {
                this.Image = _upImage;
            }
            base.OnMouseLeave(e);
        }
       
       
        //protected override void OnEnabledChanged(EventArgs e)
        //{
            
        //    base.OnEnabledChanged(e);
           
        //}
        protected override void OnClick(EventArgs e)
        {
            
            if (!_buttonEnable)
            { 
                return; 
            }
            base.OnClick(e);
        }
        
        protected override void OnMouseUp(MouseEventArgs mevent)
        {
            if (!_buttonEnable)
            {
                return;
            }
            if (_upImage != null)
            {
                this.Image = _upImage;
            }
            base.OnMouseUp(mevent);
        }

        public virtual void BeginInit()
        {
            this.AutoSize = false;
            this.TextAlign = ContentAlignment.MiddleCenter;
            this.ImageAlign = ContentAlignment.MiddleCenter;
            this.Cursor = Cursors.Hand;
        }


        /// <summary>
        /// 创建圆角正方形
        /// </summary>
        /// <param name="rect"></param>
        /// <param name="radius"></param>
        /// <returns></returns>
        public static GraphicsPath CreateRound(Rectangle rect, int radius)
        {
            GraphicsPath roundRect = new GraphicsPath();
            //顶端 
            roundRect.AddLine(rect.Left + radius - 1, rect.Top - 1, rect.Right - radius, rect.Top - 1);
            //右上角 
            roundRect.AddArc(rect.Right - radius, rect.Top - 1, radius, radius, 270, 90);
            //右边 
            roundRect.AddLine(rect.Right, rect.Top + radius, rect.Right, rect.Bottom - radius);
            //右下角
            roundRect.AddArc(rect.Right - radius, rect.Bottom - radius, radius, radius, 0, 90);
            //底边 
            roundRect.AddLine(rect.Right - radius, rect.Bottom, rect.Left + radius, rect.Bottom);
            //左下角 
            roundRect.AddArc(rect.Left - 1, rect.Bottom - radius, radius, radius, 90, 90);
            //左边 
            roundRect.AddLine(rect.Left - 1, rect.Top + radius, rect.Left - 1, rect.Bottom - radius);
            //左上角 
            roundRect.AddArc(rect.Left - 1, rect.Top - 1, radius, radius, 180, 90);
            return roundRect;
        }

        public virtual void EndInit()
        {

        }
    }
}
