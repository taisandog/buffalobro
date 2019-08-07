namespace Buffalo.Winforms
{
    partial class MedPagingBar
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
            this.txtPage = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnLast = new System.Windows.Forms.Button();
            this.labInfo = new System.Windows.Forms.Label();
            this.btnNext = new System.Windows.Forms.Button();
            this.btnUp = new System.Windows.Forms.Button();
            this.btnFirsh = new System.Windows.Forms.Button();
            this.lblPage = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtPage
            // 
            this.txtPage.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtPage.Location = new System.Drawing.Point(204, 10);
            this.txtPage.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtPage.Name = "txtPage";
            this.txtPage.Size = new System.Drawing.Size(60, 22);
            this.txtPage.TabIndex = 25;
            this.txtPage.Text = "0";
            this.txtPage.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtPage.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtPage_KeyPress);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnLast);
            this.panel1.Controls.Add(this.labInfo);
            this.panel1.Controls.Add(this.btnNext);
            this.panel1.Controls.Add(this.btnUp);
            this.panel1.Controls.Add(this.btnFirsh);
            this.panel1.Controls.Add(this.txtPage);
            this.panel1.Controls.Add(this.lblPage);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel1.Location = new System.Drawing.Point(52, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(429, 41);
            this.panel1.TabIndex = 28;
            // 
            // btnLast
            // 
            this.btnLast.BackgroundImage = Resource.Last;
            this.btnLast.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnLast.Location = new System.Drawing.Point(388, 7);
            this.btnLast.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnLast.Name = "btnLast";
            this.btnLast.Size = new System.Drawing.Size(31, 27);
            this.btnLast.TabIndex = 30;
            this.btnLast.UseVisualStyleBackColor = true;
            this.btnLast.Click += new System.EventHandler(this.btnLast_Click);
            // 
            // labInfo
            // 
            this.labInfo.AutoSize = true;
            this.labInfo.Location = new System.Drawing.Point(4, 9);
            this.labInfo.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labInfo.Name = "labInfo";
            this.labInfo.Size = new System.Drawing.Size(52, 21);
            this.labInfo.TabIndex = 27;
            this.labInfo.Text = "共[]条";
            // 
            // btnNext
            // 
            this.btnNext.BackgroundImage = Resource.Next;
            this.btnNext.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnNext.Location = new System.Drawing.Point(347, 7);
            this.btnNext.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(31, 27);
            this.btnNext.TabIndex = 29;
            this.btnNext.UseVisualStyleBackColor = true;
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // btnUp
            // 
            this.btnUp.BackgroundImage = Resource.Previous;
            this.btnUp.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnUp.Location = new System.Drawing.Point(164, 7);
            this.btnUp.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnUp.Name = "btnUp";
            this.btnUp.Size = new System.Drawing.Size(31, 27);
            this.btnUp.TabIndex = 28;
            this.btnUp.UseVisualStyleBackColor = true;
            this.btnUp.Click += new System.EventHandler(this.btnUp_Click);
            // 
            // btnFirsh
            // 
            this.btnFirsh.BackgroundImage = Resource.Home;
            this.btnFirsh.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnFirsh.Location = new System.Drawing.Point(131, 7);
            this.btnFirsh.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnFirsh.Name = "btnFirsh";
            this.btnFirsh.Size = new System.Drawing.Size(31, 27);
            this.btnFirsh.TabIndex = 27;
            this.btnFirsh.UseVisualStyleBackColor = true;
            this.btnFirsh.Click += new System.EventHandler(this.btnFirsh_Click);
            // 
            // lblPage
            // 
            this.lblPage.Location = new System.Drawing.Point(265, 10);
            this.lblPage.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblPage.Name = "lblPage";
            this.lblPage.Size = new System.Drawing.Size(79, 26);
            this.lblPage.TabIndex = 26;
            this.lblPage.Text = "/0页";
            // 
            // UCMedPagingBar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "UCMedPagingBar";
            this.Size = new System.Drawing.Size(481, 41);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox txtPage;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnLast;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.Button btnUp;
        private System.Windows.Forms.Button btnFirsh;
        private System.Windows.Forms.Label lblPage;
        private System.Windows.Forms.Label labInfo;
    }
}
