using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;

using System.Text;
using System.Windows.Forms;

namespace Buffalo.Winforms
{
    public class RoundClickButton : ClickButton
    {
        public RoundClickButton()
            : base()
        {
            this.Font = new Font("微软雅黑", 12f);
            this.BackColor = Color.Transparent;
            //this.Size = new Size(100, 80);
            this.ForeColor = Color.Black;
            this._upColor = Color.FromArgb(225,225,225);
            this._backImgColor = Color.Transparent;
            this._downColor = SystemColors.ControlDark;
            this._boderColor = Color.Transparent;
            
            this._disableColor = Color.LightGray;
            this._enterColor = Color.WhiteSmoke;
            this._darkColor = Color.FromArgb(0xd1, 0xd1, 0xd1);
            this._matrixRound = 20;
            RefreashImage();
        }
        protected int _matrixRound = 8;
        /// <summary>
        /// 圆角
        /// </summary>
        [Browsable(true), Category("Appearance"), Localizable(true)]
        public int MatrixRound
        {
            get { return _matrixRound; }
            set { _matrixRound = value; }
        }


        protected Color _backImgColor;
        /// <summary>
        /// 背景色
        /// </summary>
        [Browsable(true), Category("Appearance"), Localizable(true)]
        public virtual Color BackImgColor
        {
            get { return _backImgColor; }
            set
            {
                if (value == null)
                {

                    _backImgColor = Color.White;
                }
                else
                {
                    _backImgColor = value;
                    RefreashImage();
                }
            }
        }

        protected Color _darkColor;
        /// <summary>
        /// 阴影色
        /// </summary>
        [Browsable(true), Category("Appearance"), Localizable(true)]
        public virtual Color DarkColor
        {
            get { return _darkColor; }
            set
            {
                if (value == null)
                {

                    _darkColor = Color.White;
                }
                else
                {
                    _darkColor = value;
                    RefreashImage();
                }
            }
        }
        /// <summary>
        /// 按钮是否可用
        /// </summary>
        [Browsable(false), Category("Appearance"), Localizable(true)]
        public new Image Image
        {
            get 
            {
                return base.Image;
            }
            set 
            {
                base.Image = Image;
            }
        }

        /// <summary>
        /// 按钮是否可用
        /// </summary>
        [Browsable(false), Category("Appearance"), Localizable(true)]
        public override Image DownImage
        {
            get
            {
                return base.DownImage;
            }
            set
            {
                base.DownImage = value;
            }
        }
        /// <summary>
        /// 按钮是否可用
        /// </summary>
        [Browsable(false), Category("Appearance"), Localizable(true)]
        public override Image DisableImage
        {
            get
            {
                return base.DisableImage;
            }
            set
            {
                base.DisableImage = value;
            }
        }
        /// <summary>
        /// 进入时候的图片
        /// </summary>
        [Browsable(false), Category("Appearance"), Localizable(true)]
        public override Image EnterImage
        {
            get
            {
                return base.EnterImage;
            }
            set
            {
                base.EnterImage = value;
            }
        }

        protected Color _downColor;
        /// <summary>
        /// 按下的前景色
        /// </summary>
        [Browsable(true), Category("Appearance"), Localizable(true)]
        public Color DownColor
        {
            get { return _downColor; }
            set
            {
                if (value == null)
                {
                    _downColor = Control.DefaultBackColor;
                }
                else
                {
                    _downColor = value;
                    DownImage = DrawImage(_downColor,true);
                }
            }
        }
        protected Color _boderColor;
        /// <summary>
        /// 边框颜色
        /// </summary>
        [Browsable(true), Category("Appearance"), Localizable(true)]
        public Color BoderColor
        {
            get { return _boderColor; }
            set
            {
                if (value == null)
                {
                    _boderColor = Control.DefaultBackColor;
                }
                else
                {
                    _boderColor = value;
                    DisableImage = DrawImage(_downColor,false);
                }
            }
        }


        protected Color _disableColor;
        /// <summary>
        /// 禁用的颜色
        /// </summary>
        [Browsable(true), Category("Appearance"), Localizable(true)]
        public Color DisableColor
        {
            get { return _disableColor; }
            set
            {
                if (value == null)
                {
                    _disableColor = Control.DefaultBackColor;
                }
                else
                {
                    _disableColor = value;
                    DisableImage = DrawImage(_disableColor,true);
                }
            }
        }
        protected Color _upColor;

        /// <summary>
        /// 前景色
        /// </summary>
        [Browsable(true), Category("Appearance"), Localizable(true)]
        public Color UpColor
        {
            get { return _upColor; }
            set
            {
                if (value == null)
                {
                    _upColor = Control.DefaultBackColor;
                }
                else
                {
                    _upColor = value;
                    
                    UpImage = DrawImage(_upColor,true);
                   
                }
            }
        }

        

        protected Color _enterColor;

        /// <summary>
        /// 前景色
        /// </summary>
        [Browsable(true), Category("Appearance"), Localizable(true)]
        public Color EnterColor
        {
            get { return _enterColor; }
            set
            {
                if (value == null)
                {
                    _enterColor = Control.DefaultBackColor;
                }
                else
                {
                    _enterColor = value;

                    EnterImage = DrawImage(_upColor,true);

                }
            }
        }
        

        protected virtual Image DrawImage(Color color,bool hasdark)
        {
            int magin = 4;
            int boder = 1;
            int darkBroder = 2;
            int width = base.Width - magin*2-darkBroder;

            int height = base.Height - magin * 2 - darkBroder;
            
            Rectangle rec = new Rectangle(magin, magin, width, height);

            if (!hasdark) 
            {
                rec = new Rectangle(magin + darkBroder/2, magin + darkBroder/2, width, height);
            }

            Rectangle recDark = new Rectangle(magin + darkBroder, magin + darkBroder, width, height);

            GraphicsPath round = ClickButton.CreateRound(rec, _matrixRound);
            GraphicsPath roundBoder = ClickButton.CreateRound(new Rectangle(rec.X - boder, rec.Y - boder, rec.Width + 2 * boder, rec.Height + 2 * boder), _matrixRound);

            GraphicsPath roundDark = ClickButton.CreateRound(recDark, _matrixRound);

            Image img = new Bitmap(base.Width, base.Height);
            using (Graphics grp = Graphics.FromImage(img))
            {
                grp.SmoothingMode = SmoothingMode.AntiAlias;
                grp.Clear(_backImgColor);

                if (hasdark && _darkColor!=Color.Transparent)
                {
                    grp.FillPath((Brush)(new SolidBrush(_darkColor)), roundDark);
                }
                if (_boderColor != Color.Transparent)
                {
                    grp.FillPath((Brush)(new SolidBrush(_boderColor)), roundBoder);
                }
                grp.FillPath((Brush)(new SolidBrush(color)), round);
            }
            return img;
        }

        public void RefreashImage() 
        {
            Image = DrawImage(_upColor,true);
            UpImage = DrawImage(_upColor, true);
            DownImage = DrawImage(_downColor, false);
            DisableImage = DrawImage(_disableColor, true);
            EnterImage = DrawImage(_enterColor, true);
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            RefreashImage();
        }
        public override void BeginInit()
        {
            
            
        }

        public override void EndInit()
        {
            
        }
    }
}
