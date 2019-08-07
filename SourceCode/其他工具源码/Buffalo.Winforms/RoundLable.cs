using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;

using System.Text;
using System.Windows.Forms;

namespace Buffalo.Winforms
{
    public class RoundLable : Label
    {

        public RoundLable() :base()
        {
            this.AutoSize = false;
            this.Font = new Font("微软雅黑", 14f);
            this.Size = new Size(50, 50);
            this.ForeColor = Color.Black;
            this._roundColor = Color.White;
            this._backImgColor = Color.Transparent;
            
        }
        
        /// <summary>
        /// 密码显示
        /// </summary>
        
        public override string Text
        {
            get
            {
                return base.Text;
            }
            set
            {
                base.Text = value;
                RefreashImage();
            }
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

       

        private Color _backImgColor;
        /// <summary>
        /// 背景色
        /// </summary>
        [Browsable(true), Category("Appearance"), Localizable(true)]
        public Color BackImgColor
        {
            get { return _backImgColor; }
            set
            {
                if (value == null)
                {
                    _backImgColor = Control.DefaultBackColor;
                }
                else
                {
                    _backImgColor = value;

                    RefreashImage();

                }
            }
        }

        private Color _roundColor;
        /// <summary>
        /// 前景色
        /// </summary>
        [Browsable(true), Category("Appearance"), Localizable(true)]
        public Color RoundColor
        {
            get { return _roundColor; }
            set
            {
                if (value == null)
                {
                    _roundColor = Control.DefaultBackColor;
                }
                else
                {
                    _roundColor = value;

                    RefreashImage();

                }
            }
        }

        protected virtual Image DrawImage()
        {
            int margin = 2;
            int width = base.Width - margin*2;
            int height = base.Height - margin*2;
            Rectangle rec = new Rectangle(margin, margin, width, height);
            GraphicsPath round = ClickButton.CreateRound(rec, _matrixRound);

            Image img = new Bitmap(base.Width, base.Height);
            using (Graphics grp = Graphics.FromImage(img))
            {
                grp.SmoothingMode = SmoothingMode.AntiAlias;
                grp.Clear(_backImgColor);
                grp.FillPath((Brush)(new SolidBrush(_roundColor)), round);
            }
            return img;
        }
        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            RefreashImage();
        }
        protected virtual void RefreashImage() 
        {
            this.Image = DrawImage();
        }
    }
}
