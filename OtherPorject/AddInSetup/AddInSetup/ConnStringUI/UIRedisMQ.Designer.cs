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
            this.label10 = new System.Windows.Forms.Label();
            this.txtDatabase = new System.Windows.Forms.NumericUpDown();
            this.label11 = new System.Windows.Forms.Label();
            this.cmbMessageMode = new System.Windows.Forms.ComboBox();
            this.cmbCommandFlags = new System.Windows.Forms.ComboBox();
            this.label12 = new System.Windows.Forms.Label();
            this.nupMessageMode = new System.Windows.Forms.NumericUpDown();
            this.nupSyncTimeout = new System.Windows.Forms.NumericUpDown();
            this.label13 = new System.Windows.Forms.Label();
            this.gpSetting.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtExpir)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDatabase)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nupMessageMode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nupSyncTimeout)).BeginInit();
            this.SuspendLayout();
            // 
            // gpSetting
            // 
            this.gpSetting.Controls.Add(this.nupSyncTimeout);
            this.gpSetting.Controls.Add(this.label13);
            this.gpSetting.Controls.Add(this.label12);
            this.gpSetting.Controls.Add(this.nupMessageMode);
            this.gpSetting.Controls.Add(this.label11);
            this.gpSetting.Controls.Add(this.cmbMessageMode);
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
            this.chkSSL.Location = new System.Drawing.Point(112, 355);
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
            this.txtPwd.Location = new System.Drawing.Point(194, 104);
            this.txtPwd.Name = "txtPwd";
            this.txtPwd.Size = new System.Drawing.Size(457, 29);
            this.txtPwd.TabIndex = 63;
            // 
            // txtExpir
            // 
            this.txtExpir.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtExpir.Location = new System.Drawing.Point(194, 139);
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
            this.txtServer.Location = new System.Drawing.Point(194, 69);
            this.txtServer.Name = "txtServer";
            this.txtServer.Size = new System.Drawing.Size(457, 29);
            this.txtServer.TabIndex = 60;
            this.txtServer.Text = "127.0.0.1:6379";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(2, 69);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(184, 21);
            this.label5.TabIndex = 65;
            this.label5.Text = "服务器(多个用逗号隔开):";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(140, 105);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(46, 21);
            this.label6.TabIndex = 66;
            this.label6.Text = "密码:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.label7.Location = new System.Drawing.Point(13, 143);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(173, 20);
            this.label7.TabIndex = 67;
            this.label7.Text = "保存时间(秒数,0表示一直):";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(124, 33);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(62, 21);
            this.label8.TabIndex = 69;
            this.label8.Text = "队列名:";
            // 
            // txtName
            // 
            this.txtName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtName.Location = new System.Drawing.Point(194, 34);
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
            this.chkUseQueue.Location = new System.Drawing.Point(194, 355);
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
            this.label9.Location = new System.Drawing.Point(54, 321);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(133, 21);
            this.label9.TabIndex = 72;
            this.label9.Text = "CommandFlags:";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(92, 215);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(94, 21);
            this.label10.TabIndex = 74;
            this.label10.Text = "使用数据库:";
            // 
            // txtDatabase
            // 
            this.txtDatabase.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDatabase.Location = new System.Drawing.Point(194, 211);
            this.txtDatabase.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.txtDatabase.Name = "txtDatabase";
            this.txtDatabase.Size = new System.Drawing.Size(457, 29);
            this.txtDatabase.TabIndex = 73;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.label11.Location = new System.Drawing.Point(108, 250);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(78, 21);
            this.label11.TabIndex = 76;
            this.label11.Text = "推送模式:";
            // 
            // cmbMessageMode
            // 
            this.cmbMessageMode.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbMessageMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbMessageMode.FormattingEnabled = true;
            this.cmbMessageMode.Location = new System.Drawing.Point(194, 246);
            this.cmbMessageMode.Name = "cmbMessageMode";
            this.cmbMessageMode.Size = new System.Drawing.Size(457, 29);
            this.cmbMessageMode.TabIndex = 75;
            // 
            // cmbCommandFlags
            // 
            this.cmbCommandFlags.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbCommandFlags.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCommandFlags.FormattingEnabled = true;
            this.cmbCommandFlags.Location = new System.Drawing.Point(194, 316);
            this.cmbCommandFlags.Name = "cmbCommandFlags";
            this.cmbCommandFlags.Size = new System.Drawing.Size(457, 29);
            this.cmbCommandFlags.TabIndex = 71;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(66, 285);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(120, 21);
            this.label12.TabIndex = 78;
            this.label12.Text = "轮询间隔(毫秒):";
            // 
            // nupMessageMode
            // 
            this.nupMessageMode.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.nupMessageMode.Location = new System.Drawing.Point(194, 281);
            this.nupMessageMode.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.nupMessageMode.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nupMessageMode.Name = "nupMessageMode";
            this.nupMessageMode.Size = new System.Drawing.Size(457, 29);
            this.nupMessageMode.TabIndex = 77;
            this.nupMessageMode.Value = new decimal(new int[] {
            500,
            0,
            0,
            0});
            // 
            // nupSyncTimeout
            // 
            this.nupSyncTimeout.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.nupSyncTimeout.Location = new System.Drawing.Point(194, 174);
            this.nupSyncTimeout.Maximum = new decimal(new int[] {
            999999999,
            0,
            0,
            0});
            this.nupSyncTimeout.Name = "nupSyncTimeout";
            this.nupSyncTimeout.Size = new System.Drawing.Size(457, 29);
            this.nupSyncTimeout.TabIndex = 80;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.label13.Location = new System.Drawing.Point(1, 179);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(187, 20);
            this.label13.TabIndex = 79;
            this.label13.Text = "访问超时(毫秒,0表示不指定):";
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
            ((System.ComponentModel.ISupportInitialize)(this.nupMessageMode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nupSyncTimeout)).EndInit();
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
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.NumericUpDown txtDatabase;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.ComboBox cmbMessageMode;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.NumericUpDown nupMessageMode;
        private System.Windows.Forms.ComboBox cmbCommandFlags;
        private System.Windows.Forms.NumericUpDown nupSyncTimeout;
        private System.Windows.Forms.Label label13;
    }
}
