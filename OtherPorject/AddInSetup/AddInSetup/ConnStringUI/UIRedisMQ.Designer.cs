namespace AddInSetup.ConnStringUI
{
    partial class UIRedisMQ
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
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.chkSSL = new System.Windows.Forms.CheckBox();
            this.txtPwd = new System.Windows.Forms.TextBox();
            this.txtExpir = new System.Windows.Forms.NumericUpDown();
            this.txtServer = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.chkUseQueue = new System.Windows.Forms.CheckBox();
            this.label9 = new System.Windows.Forms.Label();
            this.cmbCommandFlags = new System.Windows.Forms.ComboBox();
            this.label10 = new System.Windows.Forms.Label();
            this.txtDatabase = new System.Windows.Forms.NumericUpDown();
            this.gpSetting.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtExpir)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDatabase)).BeginInit();
            this.SuspendLayout();
            // 
            // gpSetting
            // 
            this.gpSetting.Controls.Add(this.label10);
            this.gpSetting.Controls.Add(this.txtDatabase);
            this.gpSetting.Controls.Add(this.label9);
            this.gpSetting.Controls.Add(this.cmbCommandFlags);
            this.gpSetting.Controls.Add(this.chkUseQueue);
            this.gpSetting.Controls.Add(this.label8);
            this.gpSetting.Controls.Add(this.txtName);
            this.gpSetting.Controls.Add(this.label7);
            this.gpSetting.Controls.Add(this.label6);
            this.gpSetting.Controls.Add(this.label5);
            this.gpSetting.Controls.Add(this.chkSSL);
            this.gpSetting.Controls.Add(this.txtPwd);
            this.gpSetting.Controls.Add(this.txtExpir);
            this.gpSetting.Controls.Add(this.txtServer);
            this.gpSetting.Font = new System.Drawing.Font("微软雅黑", 12F);
            // 
            // chkSSL
            // 
            this.chkSSL.AutoSize = true;
            this.chkSSL.Location = new System.Drawing.Point(193, 243);
            this.chkSSL.Name = "chkSSL";
            this.chkSSL.Size = new System.Drawing.Size(55, 25);
            this.chkSSL.TabIndex = 64;
            this.chkSSL.Text = "SSL";
            this.chkSSL.UseVisualStyleBackColor = true;
            // 
            // txtPwd
            // 
            this.txtPwd.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPwd.Location = new System.Drawing.Point(194, 101);
            this.txtPwd.Name = "txtPwd";
            this.txtPwd.Size = new System.Drawing.Size(457, 29);
            this.txtPwd.TabIndex = 63;
            // 
            // txtExpir
            // 
            this.txtExpir.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtExpir.Location = new System.Drawing.Point(194, 138);
            this.txtExpir.Maximum = new decimal(new int[] {
            999999999,
            0,
            0,
            0});
            this.txtExpir.Name = "txtExpir";
            this.txtExpir.Size = new System.Drawing.Size(457, 29);
            this.txtExpir.TabIndex = 61;
            // 
            // txtServer
            // 
            this.txtServer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtServer.Location = new System.Drawing.Point(194, 64);
            this.txtServer.Name = "txtServer";
            this.txtServer.Size = new System.Drawing.Size(457, 29);
            this.txtServer.TabIndex = 60;
            this.txtServer.Text = "127.0.0.1:6379";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(2, 63);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(184, 21);
            this.label5.TabIndex = 65;
            this.label5.Text = "服务器(多个用逗号隔开):";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(140, 104);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(46, 21);
            this.label6.TabIndex = 66;
            this.label6.Text = "密码:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(5, 141);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(181, 21);
            this.label7.TabIndex = 67;
            this.label7.Text = "超时(分钟,0表示不超时):";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(124, 29);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(62, 21);
            this.label8.TabIndex = 69;
            this.label8.Text = "队列名:";
            // 
            // txtName
            // 
            this.txtName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtName.Location = new System.Drawing.Point(193, 29);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(457, 29);
            this.txtName.TabIndex = 68;
            this.txtName.Text = "myTest";
            // 
            // chkUseQueue
            // 
            this.chkUseQueue.AutoSize = true;
            this.chkUseQueue.Checked = true;
            this.chkUseQueue.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkUseQueue.Location = new System.Drawing.Point(193, 274);
            this.chkUseQueue.Name = "chkUseQueue";
            this.chkUseQueue.Size = new System.Drawing.Size(157, 25);
            this.chkUseQueue.TabIndex = 70;
            this.chkUseQueue.Text = "使用队列存储信息";
            this.chkUseQueue.UseVisualStyleBackColor = true;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.label9.Location = new System.Drawing.Point(53, 212);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(133, 21);
            this.label9.TabIndex = 72;
            this.label9.Text = "CommandFlags:";
            // 
            // cmbCommandFlags
            // 
            this.cmbCommandFlags.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbCommandFlags.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCommandFlags.FormattingEnabled = true;
            this.cmbCommandFlags.Location = new System.Drawing.Point(194, 208);
            this.cmbCommandFlags.Name = "cmbCommandFlags";
            this.cmbCommandFlags.Size = new System.Drawing.Size(457, 29);
            this.cmbCommandFlags.TabIndex = 71;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(48, 177);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(138, 21);
            this.label10.TabIndex = 74;
            this.label10.Text = "使用数据库(0-15):";
            // 
            // txtDatabase
            // 
            this.txtDatabase.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDatabase.Location = new System.Drawing.Point(193, 173);
            this.txtDatabase.Maximum = new decimal(new int[] {
            15,
            0,
            0,
            0});
            this.txtDatabase.Name = "txtDatabase";
            this.txtDatabase.Size = new System.Drawing.Size(457, 29);
            this.txtDatabase.TabIndex = 73;
            // 
            // UIRedisMQ
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "UIRedisMQ";
            this.gpSetting.ResumeLayout(false);
            this.gpSetting.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtExpir)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDatabase)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.CheckBox chkSSL;
        private System.Windows.Forms.TextBox txtPwd;
        private System.Windows.Forms.NumericUpDown txtExpir;
        private System.Windows.Forms.TextBox txtServer;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.CheckBox chkUseQueue;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox cmbCommandFlags;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.NumericUpDown txtDatabase;
    }
}
