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
            groupBox1 = new System.Windows.Forms.GroupBox();
            btnXTrimMaxLengthRemarl = new System.Windows.Forms.Button();
            btnNoAckRemark = new System.Windows.Forms.Button();
            label16 = new System.Windows.Forms.Label();
            nupXTrimMaxLength = new System.Windows.Forms.NumericUpDown();
            chkNoAck = new System.Windows.Forms.CheckBox();
            label15 = new System.Windows.Forms.Label();
            txtConsumerGroupName = new System.Windows.Forms.TextBox();
            label14 = new System.Windows.Forms.Label();
            txtConsumerName = new System.Windows.Forms.TextBox();
            btnRemarkPolling = new System.Windows.Forms.Button();
            btnMessageModeRemark = new System.Windows.Forms.Button();
            gpSetting.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)txtExpir).BeginInit();
            ((System.ComponentModel.ISupportInitialize)txtDatabase).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nupMessageMode).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nupSyncTimeout).BeginInit();
            groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nupXTrimMaxLength).BeginInit();
            SuspendLayout();
            // 
            // gpSetting
            // 
            gpSetting.Controls.Add(btnMessageModeRemark);
            gpSetting.Controls.Add(btnRemarkPolling);
            gpSetting.Controls.Add(groupBox1);
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
            txtPwd.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            txtPwd.Location = new System.Drawing.Point(194, 104);
            txtPwd.Name = "txtPwd";
            txtPwd.Size = new System.Drawing.Size(457, 29);
            txtPwd.TabIndex = 63;
            // 
            // txtExpir
            // 
            txtExpir.Location = new System.Drawing.Point(194, 139);
            txtExpir.Maximum = new decimal(new int[] { 999999999, 0, 0, 0 });
            txtExpir.Name = "txtExpir";
            txtExpir.Size = new System.Drawing.Size(457, 29);
            txtExpir.TabIndex = 61;
            // 
            // txtServer
            // 
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
            label10.Location = new System.Drawing.Point(71, 214);
            label10.Name = "label10";
            label10.Size = new System.Drawing.Size(115, 21);
            label10.TabIndex = 74;
            label10.Text = "database索引:";
            // 
            // txtDatabase
            // 
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
            label11.Location = new System.Drawing.Point(108, 251);
            label11.Name = "label11";
            label11.Size = new System.Drawing.Size(78, 21);
            label11.TabIndex = 76;
            label11.Text = "推送模式:";
            // 
            // cmbMessageMode
            // 
            cmbMessageMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cmbMessageMode.FormattingEnabled = true;
            cmbMessageMode.Location = new System.Drawing.Point(194, 248);
            cmbMessageMode.Name = "cmbMessageMode";
            cmbMessageMode.Size = new System.Drawing.Size(379, 29);
            cmbMessageMode.TabIndex = 75;
            // 
            // cmbCommandFlags
            // 
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
            label12.Location = new System.Drawing.Point(69, 289);
            label12.Name = "label12";
            label12.Size = new System.Drawing.Size(120, 21);
            label12.TabIndex = 78;
            label12.Text = "轮询间隔(毫秒):";
            // 
            // nupMessageMode
            // 
            nupMessageMode.Location = new System.Drawing.Point(193, 286);
            nupMessageMode.Maximum = new decimal(new int[] { int.MaxValue, 0, 0, 0 });
            nupMessageMode.Minimum = new decimal(new int[] { 10, 0, 0, 0 });
            nupMessageMode.Name = "nupMessageMode";
            nupMessageMode.Size = new System.Drawing.Size(380, 29);
            nupMessageMode.TabIndex = 77;
            nupMessageMode.Value = new decimal(new int[] { 500, 0, 0, 0 });
            // 
            // nupSyncTimeout
            // 
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
            // groupBox1
            // 
            groupBox1.Controls.Add(btnXTrimMaxLengthRemarl);
            groupBox1.Controls.Add(btnNoAckRemark);
            groupBox1.Controls.Add(label16);
            groupBox1.Controls.Add(nupXTrimMaxLength);
            groupBox1.Controls.Add(chkNoAck);
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
            // btnXTrimMaxLengthRemarl
            // 
            btnXTrimMaxLengthRemarl.Location = new System.Drawing.Point(562, 57);
            btnXTrimMaxLengthRemarl.Name = "btnXTrimMaxLengthRemarl";
            btnXTrimMaxLengthRemarl.Size = new System.Drawing.Size(71, 33);
            btnXTrimMaxLengthRemarl.TabIndex = 97;
            btnXTrimMaxLengthRemarl.Text = "说明";
            btnXTrimMaxLengthRemarl.UseVisualStyleBackColor = true;
            btnXTrimMaxLengthRemarl.Click += btnXTrimMaxLengthRemarl_Click;
            // 
            // btnNoAckRemark
            // 
            btnNoAckRemark.Location = new System.Drawing.Point(562, 18);
            btnNoAckRemark.Name = "btnNoAckRemark";
            btnNoAckRemark.Size = new System.Drawing.Size(71, 33);
            btnNoAckRemark.TabIndex = 96;
            btnNoAckRemark.Text = "说明";
            btnNoAckRemark.UseVisualStyleBackColor = true;
            btnNoAckRemark.Click += btnNoAckRemark_Click;
            // 
            // label16
            // 
            label16.AutoSize = true;
            label16.Location = new System.Drawing.Point(352, 60);
            label16.Name = "label16";
            label16.Size = new System.Drawing.Size(78, 21);
            label16.TabIndex = 95;
            label16.Text = "最大记录:";
            // 
            // nupXTrimMaxLength
            // 
            nupXTrimMaxLength.Location = new System.Drawing.Point(436, 57);
            nupXTrimMaxLength.Maximum = new decimal(new int[] { 9999, 0, 0, 0 });
            nupXTrimMaxLength.Name = "nupXTrimMaxLength";
            nupXTrimMaxLength.Size = new System.Drawing.Size(120, 29);
            nupXTrimMaxLength.TabIndex = 94;
            nupXTrimMaxLength.Value = new decimal(new int[] { 1024, 0, 0, 0 });
            // 
            // chkNoAck
            // 
            chkNoAck.AutoSize = true;
            chkNoAck.Location = new System.Drawing.Point(352, 24);
            chkNoAck.Name = "chkNoAck";
            chkNoAck.Size = new System.Drawing.Size(185, 25);
            chkNoAck.TabIndex = 93;
            chkNoAck.Text = "启动加载未Ack的消息";
            chkNoAck.UseVisualStyleBackColor = true;
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
            txtConsumerName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            txtConsumerName.Location = new System.Drawing.Point(101, 22);
            txtConsumerName.Name = "txtConsumerName";
            txtConsumerName.Size = new System.Drawing.Size(200, 29);
            txtConsumerName.TabIndex = 89;
            // 
            // btnRemarkPolling
            // 
            btnRemarkPolling.Location = new System.Drawing.Point(579, 286);
            btnRemarkPolling.Name = "btnRemarkPolling";
            btnRemarkPolling.Size = new System.Drawing.Size(71, 33);
            btnRemarkPolling.TabIndex = 89;
            btnRemarkPolling.Text = "说明";
            btnRemarkPolling.UseVisualStyleBackColor = true;
            btnRemarkPolling.Click += btnRemarkPolling_Click;
            // 
            // btnMessageModeRemark
            // 
            btnMessageModeRemark.Location = new System.Drawing.Point(579, 248);
            btnMessageModeRemark.Name = "btnMessageModeRemark";
            btnMessageModeRemark.Size = new System.Drawing.Size(71, 33);
            btnMessageModeRemark.TabIndex = 90;
            btnMessageModeRemark.Text = "说明";
            btnMessageModeRemark.UseVisualStyleBackColor = true;
            btnMessageModeRemark.Click += btnMessageModeRemark_Click;
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
            ((System.ComponentModel.ISupportInitialize)nupXTrimMaxLength).EndInit();
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
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox txtConsumerGroupName;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox txtConsumerName;
        private System.Windows.Forms.Button btnRemarkPolling;
        private System.Windows.Forms.Button btnMessageModeRemark;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.NumericUpDown nupXTrimMaxLength;
        private System.Windows.Forms.CheckBox chkNoAck;
        private System.Windows.Forms.Button btnNoAckRemark;
        private System.Windows.Forms.Button btnXTrimMaxLengthRemarl;
    }
}
