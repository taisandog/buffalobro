namespace Buffalo.WinFormsControl.PagingBar
{
    public partial class PagingBar
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        ///// <summary>
        ///// 清理所有正在使用的资源。
        ///// </summary>
        ///// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing && (components != null))
        //    {
        //        components.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.btnUp = new System.Windows.Forms.LinkLabel();
            this.labInfo = new System.Windows.Forms.Label();
            this.btnFirsh = new System.Windows.Forms.LinkLabel();
            this.btnNext = new System.Windows.Forms.LinkLabel();
            this.btnLast = new System.Windows.Forms.LinkLabel();
            this.lblPage = new System.Windows.Forms.Label();
            this.txtPage = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btnUp
            // 
            this.btnUp.Location = new System.Drawing.Point(73, 8);
            this.btnUp.Name = "btnUp";
            this.btnUp.Size = new System.Drawing.Size(51, 20);
            this.btnUp.TabIndex = 0;
            this.btnUp.TabStop = true;
            this.btnUp.Text = "上一页";
            this.btnUp.Click += new System.EventHandler(this.btnUp_Click);
            // 
            // labInfo
            // 
            this.labInfo.Location = new System.Drawing.Point(-1, 9);
            this.labInfo.Name = "labInfo";
            this.labInfo.Size = new System.Drawing.Size(71, 16);
            this.labInfo.TabIndex = 8;
            this.labInfo.Text = "共[]条";
            // 
            // btnFirsh
            // 
            this.btnFirsh.Location = new System.Drawing.Point(44, 8);
            this.btnFirsh.Name = "btnFirsh";
            this.btnFirsh.Size = new System.Drawing.Size(36, 17);
            this.btnFirsh.TabIndex = 2;
            this.btnFirsh.TabStop = true;
            this.btnFirsh.Text = "首页";
            this.btnFirsh.Click += new System.EventHandler(this.btnFirsh_Click);
            // 
            // btnNext
            // 
            this.btnNext.Location = new System.Drawing.Point(121, 8);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(54, 20);
            this.btnNext.TabIndex = 3;
            this.btnNext.TabStop = true;
            this.btnNext.Text = "下一页";
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // btnLast
            // 
            this.btnLast.Location = new System.Drawing.Point(168, 8);
            this.btnLast.Name = "btnLast";
            this.btnLast.Size = new System.Drawing.Size(44, 20);
            this.btnLast.TabIndex = 4;
            this.btnLast.TabStop = true;
            this.btnLast.Text = "末页";
            this.btnLast.Click += new System.EventHandler(this.btnLast_Click);
            // 
            // lblPage
            // 
            this.lblPage.Location = new System.Drawing.Point(241, 7);
            this.lblPage.Name = "lblPage";
            this.lblPage.Size = new System.Drawing.Size(60, 15);
            this.lblPage.TabIndex = 7;
            this.lblPage.Text = "/  页";
            // 
            // txtPage
            // 
            this.txtPage.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtPage.Location = new System.Drawing.Point(204, 7);
            this.txtPage.Name = "txtPage";
            this.txtPage.Size = new System.Drawing.Size(36, 14);
            this.txtPage.TabIndex = 6;
            this.txtPage.Text = "0";
            this.txtPage.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtPage_KeyPress);
            // 
            // PagingBar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this.txtPage);
            this.Controls.Add(this.lblPage);
            this.Controls.Add(this.btnLast);
            this.Controls.Add(this.btnUp);
            this.Controls.Add(this.btnFirsh);
            this.Controls.Add(this.labInfo);
            this.Controls.Add(this.btnNext);
            this.Name = "PagingBar";
            this.Size = new System.Drawing.Size(283, 26);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.LinkLabel btnUp;
        private System.Windows.Forms.Label labInfo;
        private System.Windows.Forms.LinkLabel btnFirsh;
        private System.Windows.Forms.LinkLabel btnNext;
        private System.Windows.Forms.LinkLabel btnLast;
        private System.Windows.Forms.Label lblPage;
        private System.Windows.Forms.TextBox txtPage;
    }
}
