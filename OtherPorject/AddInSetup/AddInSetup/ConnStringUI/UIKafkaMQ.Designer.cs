namespace AddInSetup.ConnStringUI
{
    partial class UIKafkaMQ
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
            this.label12 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txtServer = new System.Windows.Forms.TextBox();
            this.nupInterval = new System.Windows.Forms.NumericUpDown();
            this.nupSessionTimeout = new System.Windows.Forms.NumericUpDown();
            this.cmbAutoOffsetReset = new System.Windows.Forms.ComboBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label14 = new System.Windows.Forms.Label();
            this.txtSslCaLocation = new System.Windows.Forms.TextBox();
            this.cmbSaslMechanism = new System.Windows.Forms.ComboBox();
            this.label13 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.txtSaslPassword = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.cmbSecurityProtocol = new System.Windows.Forms.ComboBox();
            this.txtSaslUsername = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.txtGroupId = new System.Windows.Forms.TextBox();
            this.chkAutoCommit = new System.Windows.Forms.CheckBox();
            this.labkafka = new System.Windows.Forms.LinkLabel();
            this.gpSetting.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nupInterval)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nupSessionTimeout)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // gpSetting
            // 
            this.gpSetting.Controls.Add(this.labkafka);
            this.gpSetting.Controls.Add(this.chkAutoCommit);
            this.gpSetting.Controls.Add(this.label15);
            this.gpSetting.Controls.Add(this.txtGroupId);
            this.gpSetting.Controls.Add(this.groupBox1);
            this.gpSetting.Controls.Add(this.cmbAutoOffsetReset);
            this.gpSetting.Controls.Add(this.nupSessionTimeout);
            this.gpSetting.Controls.Add(this.nupInterval);
            this.gpSetting.Controls.Add(this.label12);
            this.gpSetting.Controls.Add(this.label7);
            this.gpSetting.Controls.Add(this.label8);
            this.gpSetting.Controls.Add(this.txtName);
            this.gpSetting.Controls.Add(this.label6);
            this.gpSetting.Controls.Add(this.label5);
            this.gpSetting.Controls.Add(this.txtServer);
            this.gpSetting.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.gpSetting.Size = new System.Drawing.Size(665, 359);
            // 
            // gbProxy
            // 
            this.gbProxy.Size = new System.Drawing.Size(479, 118);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.label12.Location = new System.Drawing.Point(81, 91);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(78, 20);
            this.label12.TabIndex = 109;
            this.label12.Text = "超时(毫秒):";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.label7.Location = new System.Drawing.Point(350, 91);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(90, 20);
            this.label7.TabIndex = 99;
            this.label7.Text = "Session超时:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.label8.Location = new System.Drawing.Point(105, 27);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(54, 20);
            this.label8.TabIndex = 97;
            this.label8.Text = "队列名:";
            // 
            // txtName
            // 
            this.txtName.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.txtName.Location = new System.Drawing.Point(161, 24);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(463, 25);
            this.txtName.TabIndex = 96;
            this.txtName.Text = "myTest";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.label6.Location = new System.Drawing.Point(351, 122);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(121, 20);
            this.label6.TabIndex = 95;
            this.label6.Text = "AutoOffsetReset:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.label5.Location = new System.Drawing.Point(105, 59);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(54, 20);
            this.label5.TabIndex = 94;
            this.label5.Text = "服务器:";
            // 
            // txtServer
            // 
            this.txtServer.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.txtServer.Location = new System.Drawing.Point(161, 56);
            this.txtServer.Name = "txtServer";
            this.txtServer.Size = new System.Drawing.Size(463, 25);
            this.txtServer.TabIndex = 92;
            this.txtServer.Text = "127.0.0.1:9092";
            // 
            // nupInterval
            // 
            this.nupInterval.Location = new System.Drawing.Point(161, 88);
            this.nupInterval.Name = "nupInterval";
            this.nupInterval.Size = new System.Drawing.Size(183, 25);
            this.nupInterval.TabIndex = 110;
            // 
            // nupSessionTimeout
            // 
            this.nupSessionTimeout.Location = new System.Drawing.Point(442, 88);
            this.nupSessionTimeout.Name = "nupSessionTimeout";
            this.nupSessionTimeout.Size = new System.Drawing.Size(182, 25);
            this.nupSessionTimeout.TabIndex = 111;
            // 
            // cmbAutoOffsetReset
            // 
            this.cmbAutoOffsetReset.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbAutoOffsetReset.FormattingEnabled = true;
            this.cmbAutoOffsetReset.Location = new System.Drawing.Point(474, 119);
            this.cmbAutoOffsetReset.Name = "cmbAutoOffsetReset";
            this.cmbAutoOffsetReset.Size = new System.Drawing.Size(152, 27);
            this.cmbAutoOffsetReset.TabIndex = 112;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label14);
            this.groupBox1.Controls.Add(this.txtSslCaLocation);
            this.groupBox1.Controls.Add(this.cmbSaslMechanism);
            this.groupBox1.Controls.Add(this.label13);
            this.groupBox1.Controls.Add(this.label11);
            this.groupBox1.Controls.Add(this.txtSaslPassword);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.cmbSecurityProtocol);
            this.groupBox1.Controls.Add(this.txtSaslUsername);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupBox1.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.groupBox1.Location = new System.Drawing.Point(3, 203);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(659, 153);
            this.groupBox1.TabIndex = 113;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "登录";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.label14.Location = new System.Drawing.Point(47, 119);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(105, 20);
            this.label14.TabIndex = 121;
            this.label14.Text = "SslCaLocation:";
            // 
            // txtSslCaLocation
            // 
            this.txtSslCaLocation.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.txtSslCaLocation.Location = new System.Drawing.Point(158, 116);
            this.txtSslCaLocation.Name = "txtSslCaLocation";
            this.txtSslCaLocation.Size = new System.Drawing.Size(463, 25);
            this.txtSslCaLocation.TabIndex = 120;
            // 
            // cmbSaslMechanism
            // 
            this.cmbSaslMechanism.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSaslMechanism.FormattingEnabled = true;
            this.cmbSaslMechanism.Location = new System.Drawing.Point(158, 83);
            this.cmbSaslMechanism.Name = "cmbSaslMechanism";
            this.cmbSaslMechanism.Size = new System.Drawing.Size(463, 27);
            this.cmbSaslMechanism.TabIndex = 119;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.label13.Location = new System.Drawing.Point(36, 86);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(116, 20);
            this.label13.TabIndex = 118;
            this.label13.Text = "SaslMechanism:";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.label11.Location = new System.Drawing.Point(340, 53);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(101, 20);
            this.label11.TabIndex = 117;
            this.label11.Text = "SaslPassword:";
            // 
            // txtSaslPassword
            // 
            this.txtSaslPassword.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.txtSaslPassword.Location = new System.Drawing.Point(447, 51);
            this.txtSaslPassword.Name = "txtSaslPassword";
            this.txtSaslPassword.Size = new System.Drawing.Size(176, 25);
            this.txtSaslPassword.TabIndex = 116;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.label10.Location = new System.Drawing.Point(47, 53);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(105, 20);
            this.label10.TabIndex = 115;
            this.label10.Text = "SaslUsername:";
            // 
            // cmbSecurityProtocol
            // 
            this.cmbSecurityProtocol.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSecurityProtocol.FormattingEnabled = true;
            this.cmbSecurityProtocol.Location = new System.Drawing.Point(158, 17);
            this.cmbSecurityProtocol.Name = "cmbSecurityProtocol";
            this.cmbSecurityProtocol.Size = new System.Drawing.Size(463, 27);
            this.cmbSecurityProtocol.TabIndex = 115;
            // 
            // txtSaslUsername
            // 
            this.txtSaslUsername.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.txtSaslUsername.Location = new System.Drawing.Point(158, 51);
            this.txtSaslUsername.Name = "txtSaslUsername";
            this.txtSaslUsername.Size = new System.Drawing.Size(176, 25);
            this.txtSaslUsername.TabIndex = 114;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.label9.Location = new System.Drawing.Point(30, 20);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(122, 20);
            this.label9.TabIndex = 114;
            this.label9.Text = "SecurityProtocol:";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.BackColor = System.Drawing.SystemColors.Control;
            this.label15.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.label15.Location = new System.Drawing.Point(93, 122);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(67, 20);
            this.label15.TabIndex = 115;
            this.label15.Text = "GroupId:";
            // 
            // txtGroupId
            // 
            this.txtGroupId.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.txtGroupId.Location = new System.Drawing.Point(162, 119);
            this.txtGroupId.Name = "txtGroupId";
            this.txtGroupId.Size = new System.Drawing.Size(183, 25);
            this.txtGroupId.TabIndex = 114;
            // 
            // chkAutoCommit
            // 
            this.chkAutoCommit.AutoSize = true;
            this.chkAutoCommit.Location = new System.Drawing.Point(161, 150);
            this.chkAutoCommit.Name = "chkAutoCommit";
            this.chkAutoCommit.Size = new System.Drawing.Size(84, 24);
            this.chkAutoCommit.TabIndex = 116;
            this.chkAutoCommit.Text = "自动提交";
            this.chkAutoCommit.UseVisualStyleBackColor = true;
            // 
            // labkafka
            // 
            this.labkafka.AutoSize = true;
            this.labkafka.Location = new System.Drawing.Point(351, 154);
            this.labkafka.Name = "labkafka";
            this.labkafka.Size = new System.Drawing.Size(233, 20);
            this.labkafka.TabIndex = 117;
            this.labkafka.TabStop = true;
            this.labkafka.Text = "请把依赖包放进您的运行程序文件夹";
            this.labkafka.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.Labkafka_LinkClicked);
            // 
            // UIKafkaMQ
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "UIKafkaMQ";
            this.Size = new System.Drawing.Size(665, 620);
            this.gpSetting.ResumeLayout(false);
            this.gpSetting.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nupInterval)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nupSessionTimeout)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtServer;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox cmbSecurityProtocol;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox cmbAutoOffsetReset;
        private System.Windows.Forms.NumericUpDown nupSessionTimeout;
        private System.Windows.Forms.NumericUpDown nupInterval;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox txtSslCaLocation;
        private System.Windows.Forms.ComboBox cmbSaslMechanism;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox txtSaslPassword;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txtSaslUsername;
        private System.Windows.Forms.CheckBox chkAutoCommit;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox txtGroupId;
        private System.Windows.Forms.LinkLabel labkafka;
    }
}
