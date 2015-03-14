namespace Buffalo.WinFormsControl.PagingBar
{
    partial class MediumPagingBar
    {
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

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.labInfo = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnLast = new System.Windows.Forms.Button();
            this.btnNext = new System.Windows.Forms.Button();
            this.btnUp = new System.Windows.Forms.Button();
            this.btnFirsh = new System.Windows.Forms.Button();
            this.txtPage = new System.Windows.Forms.TextBox();
            this.lblPage = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // labInfo
            // 
            this.labInfo.AutoSize = true;
            this.labInfo.Location = new System.Drawing.Point(3, 5);
            this.labInfo.Name = "labInfo";
            this.labInfo.Size = new System.Drawing.Size(41, 12);
            this.labInfo.TabIndex = 25;
            this.labInfo.Text = "共[]条";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnLast);
            this.panel1.Controls.Add(this.btnNext);
            this.panel1.Controls.Add(this.btnUp);
            this.panel1.Controls.Add(this.btnFirsh);
            this.panel1.Controls.Add(this.txtPage);
            this.panel1.Controls.Add(this.lblPage);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel1.Location = new System.Drawing.Point(53, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(173, 22);
            this.panel1.TabIndex = 26;
            // 
            // btnLast
            // 
            this.btnLast.Image = global::Buffalo.WinFormsControl.Properties.Resources.Last;
            this.btnLast.Location = new System.Drawing.Point(153, 1);
            this.btnLast.Name = "btnLast";
            this.btnLast.Size = new System.Drawing.Size(20, 20);
            this.btnLast.TabIndex = 30;
            this.btnLast.UseVisualStyleBackColor = true;
            this.btnLast.Click += new System.EventHandler(this.btnLast_Click);
            // 
            // btnNext
            // 
            this.btnNext.Image = global::Buffalo.WinFormsControl.Properties.Resources.Next;
            this.btnNext.Location = new System.Drawing.Point(133, 1);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(20, 20);
            this.btnNext.TabIndex = 29;
            this.btnNext.UseVisualStyleBackColor = true;
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // btnUp
            // 
            this.btnUp.Image = global::Buffalo.WinFormsControl.Properties.Resources.Previous;
            this.btnUp.Location = new System.Drawing.Point(21, 1);
            this.btnUp.Name = "btnUp";
            this.btnUp.Size = new System.Drawing.Size(20, 20);
            this.btnUp.TabIndex = 28;
            this.btnUp.UseVisualStyleBackColor = true;
            this.btnUp.Click += new System.EventHandler(this.btnUp_Click);
            // 
            // btnFirsh
            // 
            this.btnFirsh.Image = global::Buffalo.WinFormsControl.Properties.Resources.Home;
            this.btnFirsh.Location = new System.Drawing.Point(1, 1);
            this.btnFirsh.Name = "btnFirsh";
            this.btnFirsh.Size = new System.Drawing.Size(20, 20);
            this.btnFirsh.TabIndex = 27;
            this.btnFirsh.UseVisualStyleBackColor = true;
            this.btnFirsh.Click += new System.EventHandler(this.btnFirsh_Click);
            // 
            // txtPage
            // 
            this.txtPage.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtPage.Location = new System.Drawing.Point(45, 4);
            this.txtPage.Name = "txtPage";
            this.txtPage.Size = new System.Drawing.Size(36, 14);
            this.txtPage.TabIndex = 25;
            this.txtPage.Text = "0";
            this.txtPage.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtPage.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtPage_KeyPress);
            // 
            // lblPage
            // 
            this.lblPage.Location = new System.Drawing.Point(82, 4);
            this.lblPage.Name = "lblPage";
            this.lblPage.Size = new System.Drawing.Size(47, 15);
            this.lblPage.TabIndex = 26;
            this.lblPage.Text = "/0页";
            // 
            // MediumPagingBar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.labInfo);
            this.Name = "MediumPagingBar";
            this.Size = new System.Drawing.Size(226, 22);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labInfo;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox txtPage;
        private System.Windows.Forms.Label lblPage;
        private System.Windows.Forms.Button btnLast;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.Button btnUp;
        private System.Windows.Forms.Button btnFirsh;

    }
}
