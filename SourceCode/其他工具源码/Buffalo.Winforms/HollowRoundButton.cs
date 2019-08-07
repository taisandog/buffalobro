using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;

using System.Text;

namespace Buffalo.Winforms
{
    /// <summary>
    /// 空心按钮
    /// </summary>
    public class HollowRoundButton:RoundButton
    {
        public HollowRoundButton()
        {
            this.Font = new Font("微软雅黑", 14f);

            this.ForeColor = Color.Black;
            this._upColor = Color.Green;
            this._backImgColor = Color.White;
            this._downColor = Color.Red;
            this._disableColor = Color.LightGray;
            this._enterColor = Color.GreenYellow;
            this._matrixRound = 20;

            RefreashImage();
        }

        public override void EndInit()
        {
            
        }

        
        private int _marginPx=2;

        /// <summary>
        /// 边框粗细
        /// </summary>
       [Browsable(true), Category("Appearance"), Localizable(true)]
        public int MarginPx
        {
            get { return _marginPx; }
            set { _marginPx = value; }
        }
       private const int Mag = 1;
        protected override System.Drawing.Image DrawImage(System.Drawing.Color color)
        {
            int hmag = _marginPx * 2;

            int width = base.Width - base.Margin.Left - base.Margin.Right;
            int height = base.Height - base.Margin.Top - base.Margin.Bottom ;
            Rectangle rec = new Rectangle(base.Margin.Left+Mag, base.Margin.Top+Mag, width - (Mag * 2), height- (Mag * 2));
            GraphicsPath round = ClickButton.CreateRound(rec, _matrixRound);

            rec = new Rectangle(base.Margin.Left + _marginPx+Mag, base.Margin.Top + _marginPx+Mag, width - hmag- (Mag * 2), height - hmag- (Mag * 2));
            GraphicsPath roundHollow = ClickButton.CreateRound(rec, _matrixRound);

            Image img = new Bitmap(base.Width, base.Height);
            using (Graphics grp = Graphics.FromImage(img))
            {
                grp.SmoothingMode = SmoothingMode.AntiAlias;
                grp.Clear(Color.Transparent);
                grp.FillPath((Brush)(new SolidBrush(color)), round);

                grp.FillPath((Brush)(new SolidBrush(_backImgColor)), roundHollow);
            }
            return img;
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
    }
}
