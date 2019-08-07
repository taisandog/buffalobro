using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;

using System.Text;

using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace Buffalo.Winforms
{
    public class UIHeader : Label
    {
        #region 组件设计器生成的代码
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            

            this.SuspendLayout();
            this.Dock = DockStyle.Top;
            this.AutoSize = false;
            this.Font = new System.Drawing.Font(Font.FontFamily, 20, FontStyle.Bold);
            this.Size = new Size(this.Size.Width, 40);
            this.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.BackColor = Color.GhostWhite;
            this.BorderStyle = BorderStyle.None;
            // 
            // BaseHeader
            // 
            this.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Font = new System.Drawing.Font("宋体", 20F);
            this.Size = new System.Drawing.Size(100, 40);
            this.ResumeLayout(false);

        }

        #endregion
        public UIHeader()
        {
            InitializeComponent();
        }

        public UIHeader(IContainer container)
        {
            container.Add(this);
            
            InitializeComponent();
        }

        

        protected override void OnPaint(PaintEventArgs e)
        {
            if (this.Height == 0) return;
            Brush backbrush = new LinearGradientBrush(new Point(0, 0), new Point(0, this.Height), Color.White, SystemColors.Control);
            e.Graphics.FillRectangle(backbrush, 0, 0, this.Width, this.Height);
            //绘制框
            if (this.Height > 0)
            {
                Color tarColor = Color.FromArgb(SystemColors.Control.R * 5 / 10, SystemColors.Control.G * 5 / 10, SystemColors.Control.B * 5 / 10);
                using (Brush borderBrush = new System.Drawing.Drawing2D.LinearGradientBrush(new Point(0, 0), new Point(this.Width / 2, 0), SystemColors.Control, tarColor))
                {
                    e.Graphics.DrawLine(new Pen(borderBrush), new Point(0, this.Height - 1), new Point(this.Width / 2, this.Height - 1));
                }
                using (Brush borderBrush = new System.Drawing.Drawing2D.LinearGradientBrush(new Point(this.Width / 2 - 2, 0), new Point(this.Width - 1, 0), tarColor, SystemColors.Control))
                {
                    e.Graphics.DrawLine(new Pen(borderBrush), new Point(this.Width - 1, this.Height - 1), new Point(this.Width / 2 - 1, this.Height - 1));
                }
            }
            base.OnPaint(e);
        }
    }
}
