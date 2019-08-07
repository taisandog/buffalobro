using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;

using System.Text;
using System.Windows.Forms;

namespace Buffalo.Winforms
{
    /// <summary>
    /// 圆角Panel
    /// </summary>
    public class RoundPanel:Panel
    {
        public RoundPanel()
            : base()
        {
            _frontColor = Color.White;
            _borderColor = Color.Black;
            DrawBack();
        }

        private int _matrixRound = 8;
        private Color _frontColor;

        /// <summary>
        /// 前景色
        /// </summary>
        [Browsable(true), Category("Appearance"),Localizable(true)]
        public Color FrontColor
        {
            get { return _frontColor; }
            set
            {

                _frontColor = value;

                DrawBack();
            }
        }

        private Color _borderColor;
        /// <summary>
        /// 边框颜色
        /// </summary>
        [Browsable(true), Category("Appearance"), Localizable(true)]
        public Color BorderColor
        {
            get { return _borderColor; }
            set
            {

                _borderColor = value;

                DrawBack();
            }
        }
        /// <summary>
        /// 圆角
        /// </summary>
        [Browsable(true), Category("Appearance"), Localizable(true)]
        public int MatrixRound
        {
            get { return _matrixRound; }
            set
            {
                _matrixRound = value;
                DrawBack();
            }
        }

        
        private void DrawBack()
        {
            int margin = 2;
            int width = base.Width - margin*2;
            int height = base.Height - margin*2;
            int broder = 1;
            Rectangle rec = new Rectangle(margin, margin, width, height);
            GraphicsPath round = ClickButton.CreateRound(rec, _matrixRound);

            rec = new Rectangle(margin - broder, margin - broder, width + broder * 2, height + broder * 2);
            GraphicsPath borderround = ClickButton.CreateRound(rec, _matrixRound);

            Image img = new Bitmap(base.Width, base.Height);
            using (Graphics grp = Graphics.FromImage(img))
            {
                
                grp.SmoothingMode = SmoothingMode.AntiAlias;
                grp.Clear(BackColor);
                grp.FillPath((Brush)(new SolidBrush(_borderColor)), borderround);
                grp.FillPath((Brush)(new SolidBrush(_frontColor)), round);
            }
            this.BackgroundImage = img;
        }

        protected override void OnResize(EventArgs eventargs)
        {
            DrawBack();
        }
    }
}
