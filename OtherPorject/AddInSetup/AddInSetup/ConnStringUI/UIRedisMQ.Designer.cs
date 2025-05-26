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
            chkSSL = new System.Windows.Forms.CheckBox();
            txtPwd = new System.Windows.Forms.TextBox();
            txtExpir = new System.Windows.Forms.NumericUpDown();
            txtServer = new System.Windows.Forms.TextBox();
            label5 = new System.Windows.Forms.Label();
            label6 = new System.Windows.Forms.Label();
            label7 = new System.Windows.Forms.Label();
            label8 = new System.Windows.Forms.Label();
            txtName = new System.Windows.Forms.TextBox();
            chkUseQueue = new System.Windows.Forms.CheckBox();
            label9 = new System.Windows.Forms.Label();
            label10 = new System.Windows.Forms.Label();
            txtDatabase = new System.Windows.Forms.NumericUpDown();
            label11 = new System.Windows.Forms.Label();
            cmbMessageMode = new System.Windows.Forms.ComboBox();
            cmbCommandFlags = new System.Windows.Forms.ComboBox();
            label12 = new System.Windows.Forms.Label();
            nupMessageMode = new System.Windows.Forms.NumericUpDown();
            nupSyncTimeout = new System.Windows.Forms.NumericUpDown();
            label13 = new System.Windows.Forms.Label();
            chkSkipCert = new System.Windows.Forms.CheckBox();
            txtModeLab = new System.Windows.Forms.TextBox();
            txtModeLabel = new System.Windows.Forms.TextBox();
            groupBox1 = new System.Windows.Forms.GroupBox();
            label15 = new System.Windows.Forms.Label();
            txtConsumerGroupName = new System.Windows.Forms.TextBox();
            label14 = new System.Windows.Forms.Label();
            txtConsumerName = new System.Windows.Forms.TextBox();
            gpSetting.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)txtExpir).BeginInit();
            ((System.ComponentModel.ISupportInitialize)txtDatabase).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nupMessageMode).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nupSyncTimeout).BeginInit();
            groupBox1.SuspendLayout();
            SuspendLayout();
            // 
            // gpSetting
            // 
            gpSetting.Controls.Add(groupBox1);
            gpSetting.Controls.Add(txtModeLabel);
            gpSetting.Controls.Add(txtModeLab);
            gpSetting.Controls.Add(chkSkipCert);
            gpSetting.Controls.Add(nupSyncTimeout);
            gpSetting.Controls.Add(label13);
            gpSetting.Controls.Add(label12);
            gpSetting.Controls.Add(nupMessageMode);
            gpSetting.Controls.Add(label11);
            gpSetting.Controls.Add(cmbMessageMode);
            gpSetting.Controls.Add(label10);
            gpSetting.Controls.Add(txtDatabase);
            gpSetting.Controls.Add(label9);
            gpSetting.Controls.Add(cmbCommandFlags);
            gpSetting.Controls.Add(chkUseQueue);
            gpSetting.Controls.Add(label8);
            gpSetting.Controls.Add(txtName);
            gpSetting.Controls.Add(label7);
            gpSetting.Controls.Add(label6);
            gpSetting.Controls.Add(label5);
            gpSetting.Controls.Add(chkSSL);
            gpSetting.Controls.Add(txtPwd);
            gpSetting.Controls.Add(txtExpir);
            gpSetting.Controls.Add(txtServer);
            gpSetting.Font = new System.Drawing.Font("微软雅黑", 12F);
            gpSetting.Size = new System.Drawing.Size(659, 524);
            // 
            // gbProxy
            // 
            gbProxy.Location = new System.Drawing.Point(0, 524);
            // 
            // chkSSL
            // 
            chkSSL.AutoSize = true;
            chkSSL.Location = new System.Drawing.Point(111, 380);
            chkSSL.Name = "chkSSL";
            chkSSL.Size = new System.Drawing.Size(55, 25);
            chkSSL.TabIndex = 64;
            chkSSL.Text = "SSL";
            chkSSL.UseVisualStyleBackColor = true;
            // 
            // txtPwd
            // 
            txtPwd.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            txtPwd.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            txtPwd.Location = new System.Drawing.Point(194, 104);
            txtPwd.Name = "txtPwd";
            txtPwd.Size = new System.Drawing.Size(457, 29);
            txtPwd.TabIndex = 63;
            // 
            // txtExpir
            // 
            txtExpir.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            txtExpir.Location = new System.Drawing.Point(194, 139);
            txtExpir.Maximum = new decimal(new int[] { 999999999, 0, 0, 0 });
            txtExpir.Name = "txtExpir";
            txtExpir.Size = new System.Drawing.Size(457, 29);
            txtExpir.TabIndex = 61;
            // 
            // txtServer
            // 
            txtServer.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            txtServer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            txtServer.Location = new System.Drawing.Point(194, 69);
            txtServer.Name = "txtServer";
            txtServer.Size = new System.Drawing.Size(457, 29);
            txtServer.TabIndex = 60;
            txtServer.Text = "127.0.0.1:6379";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new System.Drawing.Point(2, 69);
            label5.Name = "label5";
            label5.Size = new System.Drawing.Size(184, 21);
            label5.TabIndex = 65;
            label5.Text = "服务器(多个用逗号隔开):";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new System.Drawing.Point(140, 105);
            label6.Name = "label6";
            label6.Size = new System.Drawing.Size(46, 21);
            label6.TabIndex = 66;
            label6.Text = "密码:";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Font = new System.Drawing.Font("微软雅黑", 10F);
            label7.Location = new System.Drawing.Point(13, 143);
            label7.Name = "label7";
            label7.Size = new System.Drawing.Size(173, 20);
            label7.TabIndex = 67;
            label7.Text = "保存时间(秒数,0表示一直):";
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new System.Drawing.Point(124, 33);
            label8.Name = "label8";
            label8.Size = new System.Drawing.Size(62, 21);
            label8.TabIndex = 69;
            label8.Text = "队列名:";
            // 
            // txtName
            // 
            txtName.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            txtName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            txtName.Location = new System.Drawing.Point(194, 34);
            txtName.Name = "txtName";
            txtName.Size = new System.Drawing.Size(457, 29);
            txtName.TabIndex = 68;
            txtName.Text = "myTest";
            // 
            // chkUseQueue
            // 
            chkUseQueue.AutoSize = true;
            chkUseQueue.Checked = true;
            chkUseQueue.CheckState = System.Windows.Forms.CheckState.Checked;
            chkUseQueue.Location = new System.Drawing.Point(320, 380);
            chkUseQueue.Name = "chkUseQueue";
            chkUseQueue.Size = new System.Drawing.Size(157, 25);
            chkUseQueue.TabIndex = 70;
            chkUseQueue.Text = "使用队列存储信息";
            chkUseQueue.UseVisualStyleBackColor = true;
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Font = new System.Drawing.Font("微软雅黑", 12F);
            label9.Location = new System.Drawing.Point(53, 346);
            label9.Name = "label9";
            label9.Size = new System.Drawing.Size(133, 21);
            label9.TabIndex = 72;
            label9.Text = "CommandFlags:";
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Location = new System.Drawing.Point(71, 219);
            label10.Name = "label10";
            label10.Size = new System.Drawing.Size(115, 21);
            label10.TabIndex = 74;
            label10.Text = "database索引:";
            // 
            // txtDatabase
            // 
            txtDatabase.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            txtDatabase.Location = new System.Drawing.Point(194, 211);
            txtDatabase.Maximum = new decimal(new int[] { int.MaxValue, 0, 0, 0 });
            txtDatabase.Name = "txtDatabase";
            txtDatabase.Size = new System.Drawing.Size(457, 29);
            txtDatabase.TabIndex = 73;
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.Font = new System.Drawing.Font("微软雅黑", 12F);
            label11.Location = new System.Drawing.Point(108, 250);
            label11.Name = "label11";
            label11.Size = new System.Drawing.Size(78, 21);
            label11.TabIndex = 76;
            label11.Text = "推送模式:";
            // 
            // cmbMessageMode
            // 
            cmbMessageMode.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            cmbMessageMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cmbMessageMode.FormattingEnabled = true;
            cmbMessageMode.Location = new System.Drawing.Point(458, 246);
            cmbMessageMode.Name = "cmbMessageMode";
            cmbMessageMode.Size = new System.Drawing.Size(193, 29);
            cmbMessageMode.TabIndex = 75;
            // 
            // cmbCommandFlags
            // 
            cmbCommandFlags.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            cmbCommandFlags.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cmbCommandFlags.FormattingEnabled = true;
            cmbCommandFlags.Location = new System.Drawing.Point(193, 341);
            cmbCommandFlags.Name = "cmbCommandFlags";
            cmbCommandFlags.Size = new System.Drawing.Size(457, 29);
            cmbCommandFlags.TabIndex = 71;
            // 
            // label12
            // 
            label12.AutoSize = true;
            label12.Location = new System.Drawing.Point(69, 285);
            label12.Name = "label12";
            label12.Size = new System.Drawing.Size(120, 21);
            label12.TabIndex = 78;
            label12.Text = "轮询间隔(毫秒):";
            // 
            // nupMessageMode
            // 
            nupMessageMode.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            nupMessageMode.Location = new System.Drawing.Point(458, 295);
            nupMessageMode.Maximum = new decimal(new int[] { int.MaxValue, 0, 0, 0 });
            nupMessageMode.Minimum = new decimal(new int[] { 10, 0, 0, 0 });
            nupMessageMode.Name = "nupMessageMode";
            nupMessageMode.Size = new System.Drawing.Size(193, 29);
            nupMessageMode.TabIndex = 77;
            nupMessageMode.Value = new decimal(new int[] { 500, 0, 0, 0 });
            // 
            // nupSyncTimeout
            // 
            nupSyncTimeout.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            nupSyncTimeout.Location = new System.Drawing.Point(194, 174);
            nupSyncTimeout.Maximum = new decimal(new int[] { 999999999, 0, 0, 0 });
            nupSyncTimeout.Name = "nupSyncTimeout";
            nupSyncTimeout.Size = new System.Drawing.Size(457, 29);
            nupSyncTimeout.TabIndex = 80;
            // 
            // label13
            // 
            label13.AutoSize = true;
            label13.Font = new System.Drawing.Font("微软雅黑", 10F);
            label13.Location = new System.Drawing.Point(1, 179);
            label13.Name = "label13";
            label13.Size = new System.Drawing.Size(187, 20);
            label13.TabIndex = 79;
            label13.Text = "访问超时(毫秒,0表示不指定):";
            // 
            // chkSkipCert
            // 
            chkSkipCert.AutoSize = true;
            chkSkipCert.Location = new System.Drawing.Point(172, 380);
            chkSkipCert.Name = "chkSkipCert";
            chkSkipCert.Size = new System.Drawing.Size(119, 25);
            chkSkipCert.TabIndex = 81;
            chkSkipCert.Text = "SSL跳过证书";
            chkSkipCert.UseVisualStyleBackColor = true;
            // 
            // txtModeLab
            // 
            txtModeLab.BackColor = System.Drawing.Color.White;
            txtModeLab.Font = new System.Drawing.Font("微软雅黑", 8F);
            txtModeLab.ForeColor = System.Drawing.Color.OliveDrab;
            txtModeLab.Location = new System.Drawing.Point(193, 244);
            txtModeLab.Multiline = true;
            txtModeLab.Name = "txtModeLab";
            txtModeLab.ReadOnly = true;
            txtModeLab.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            txtModeLab.Size = new System.Drawing.Size(261, 33);
            txtModeLab.TabIndex = 86;
            txtModeLab.Text = "Polling:轮询模式,按照间隔获取队列数据\r\n\r\nSubscriber为订阅发布模式,使用pub/sub实现发布,如果勾选使用队列存储信息,则pub/sub只推送通知，实际数据还是从队列获取，以防止消息丢失\r\n\r\nBlockQueue:阻塞队列模式，利用brpop指令实现阻塞读取队列";
            // 
            // txtModeLabel
            // 
            txtModeLabel.BackColor = System.Drawing.Color.White;
            txtModeLabel.Font = new System.Drawing.Font("微软雅黑", 8F);
            txtModeLabel.ForeColor = System.Drawing.Color.OliveDrab;
            txtModeLabel.Location = new System.Drawing.Point(193, 281);
            txtModeLabel.Multiline = true;
            txtModeLabel.Name = "txtModeLabel";
            txtModeLabel.ReadOnly = true;
            txtModeLabel.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            txtModeLabel.Size = new System.Drawing.Size(261, 54);
            txtModeLabel.TabIndex = 87;
            txtModeLabel.Text = "Polling模式时：此为每次轮询到空队列时候的睡眠时间(越小越实时，但负担更重，为0时候是50ms)\r\n\r\nBlockQueue模式时：此为brPop的超时时间(最小1秒，尽量大，值为0时候是30秒)\r\n\r\n\r\n";
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(label15);
            groupBox1.Controls.Add(txtConsumerGroupName);
            groupBox1.Controls.Add(label14);
            groupBox1.Controls.Add(txtConsumerName);
            groupBox1.Dock = System.Windows.Forms.DockStyle.Bottom;
            groupBox1.Location = new System.Drawing.Point(3, 428);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new System.Drawing.Size(653, 93);
            groupBox1.TabIndex = 88;
            groupBox1.TabStop = false;
            groupBox1.Text = "Stream模式";
            // 
            // label15
            // 
            label15.AutoSize = true;
            label15.Location = new System.Drawing.Point(17, 61);
            label15.Name = "label15";
            label15.Size = new System.Drawing.Size(78, 21);
            label15.TabIndex = 92;
            label15.Text = "消费者组:";
            // 
            // txtConsumerGroupName
            // 
            txtConsumerGroupName.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            txtConsumerGroupName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            txtConsumerGroupName.Location = new System.Drawing.Point(101, 58);
            txtConsumerGroupName.Name = "txtConsumerGroupName";
            txtConsumerGroupName.Size = new System.Drawing.Size(200, 29);
            txtConsumerGroupName.TabIndex = 91;
            // 
            // label14
            // 
            label14.AutoSize = true;
            label14.Location = new System.Drawing.Point(17, 25);
            label14.Name = "label14";
            label14.Size = new System.Drawing.Size(78, 21);
            label14.TabIndex = 90;
            label14.Text = "消费者名:";
            // 
            // txtConsumerName
            // 
            txtConsumerName.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            txtConsumerName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            txtConsumerName.Location = new System.Drawing.Point(101, 22);
            txtConsumerName.Name = "txtConsumerName";
            txtConsumerName.Size = new System.Drawing.Size(200, 29);
            txtConsumerName.TabIndex = 89;
            // 
            // UIRedisMQ
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(10F, 21F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Name = "UIRedisMQ";
            Size = new System.Drawing.Size(659, 776);
            gpSetting.ResumeLayout(false);
            gpSetting.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)txtExpir).EndInit();
            ((System.ComponentModel.ISupportInitialize)txtDatabase).EndInit();
            ((System.ComponentModel.ISupportInitialize)nupMessageMode).EndInit();
            ((System.ComponentModel.ISupportInitialize)nupSyncTimeout).EndInit();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ResumeLayout(false);

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
        private System.Windows.Forms.CheckBox chkSkipCert;
        private System.Windows.Forms.TextBox txtModeLab;
        private System.Windows.Forms.TextBox txtModeLabel;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox txtConsumerGroupName;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox txtConsumerName;
    }
}
