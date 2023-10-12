namespace AddInSetup.ConnStringUI
{
    partial class UIMQTT
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
            this.label5 = new System.Windows.Forms.Label();
            this.txtServer = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtUid = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtPwd = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.txtClientId = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.txtWebSocketServer = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.nupSessionExpiry = new System.Windows.Forms.NumericUpDown();
            this.nulKeepAlivePeriod = new System.Windows.Forms.NumericUpDown();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.txtProxyUserName = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.txtProxyPassword = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.txtProxy = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.txtDomain = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.cmbQualityOfServiceLevel = new System.Windows.Forms.ComboBox();
            this.chkRetainAsPublished = new System.Windows.Forms.CheckBox();
            this.chkNoLocal = new System.Windows.Forms.CheckBox();
            this.cmbRetainHandling = new System.Windows.Forms.ComboBox();
            this.label17 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.chkKeepAlive = new System.Windows.Forms.CheckBox();
            this.cmbProtocolVersion = new System.Windows.Forms.ComboBox();
            this.label19 = new System.Windows.Forms.Label();
            this.chkCleanSession = new System.Windows.Forms.CheckBox();
            this.nupSessionExpiryInterval = new System.Windows.Forms.NumericUpDown();
            this.label20 = new System.Windows.Forms.Label();
            this.gpSetting.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nupSessionExpiry)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nulKeepAlivePeriod)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nupSessionExpiryInterval)).BeginInit();
            this.SuspendLayout();
            // 
            // gpSetting
            // 
            this.gpSetting.Controls.Add(this.nupSessionExpiryInterval);
            this.gpSetting.Controls.Add(this.label20);
            this.gpSetting.Controls.Add(this.chkCleanSession);
            this.gpSetting.Controls.Add(this.cmbProtocolVersion);
            this.gpSetting.Controls.Add(this.label19);
            this.gpSetting.Controls.Add(this.chkKeepAlive);
            this.gpSetting.Controls.Add(this.label18);
            this.gpSetting.Controls.Add(this.txtName);
            this.gpSetting.Controls.Add(this.cmbRetainHandling);
            this.gpSetting.Controls.Add(this.label17);
            this.gpSetting.Controls.Add(this.chkNoLocal);
            this.gpSetting.Controls.Add(this.chkRetainAsPublished);
            this.gpSetting.Controls.Add(this.cmbQualityOfServiceLevel);
            this.gpSetting.Controls.Add(this.label16);
            this.gpSetting.Controls.Add(this.label15);
            this.gpSetting.Controls.Add(this.txtDomain);
            this.gpSetting.Controls.Add(this.label12);
            this.gpSetting.Controls.Add(this.txtProxyUserName);
            this.gpSetting.Controls.Add(this.label13);
            this.gpSetting.Controls.Add(this.txtProxyPassword);
            this.gpSetting.Controls.Add(this.label14);
            this.gpSetting.Controls.Add(this.txtProxy);
            this.gpSetting.Controls.Add(this.nulKeepAlivePeriod);
            this.gpSetting.Controls.Add(this.label11);
            this.gpSetting.Controls.Add(this.nupSessionExpiry);
            this.gpSetting.Controls.Add(this.label10);
            this.gpSetting.Controls.Add(this.label9);
            this.gpSetting.Controls.Add(this.txtWebSocketServer);
            this.gpSetting.Controls.Add(this.label8);
            this.gpSetting.Controls.Add(this.txtClientId);
            this.gpSetting.Controls.Add(this.label7);
            this.gpSetting.Controls.Add(this.txtUid);
            this.gpSetting.Controls.Add(this.label6);
            this.gpSetting.Controls.Add(this.txtPwd);
            this.gpSetting.Controls.Add(this.label5);
            this.gpSetting.Controls.Add(this.txtServer);
            this.gpSetting.Size = new System.Drawing.Size(750, 388);
            // 
            // gbProxy
            // 
            this.gbProxy.Size = new System.Drawing.Size(750, 59);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.label5.Location = new System.Drawing.Point(51, 71);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(62, 21);
            this.label5.TabIndex = 76;
            this.label5.Text = "服务器:";
            // 
            // txtServer
            // 
            this.txtServer.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.txtServer.Location = new System.Drawing.Point(119, 67);
            this.txtServer.Name = "txtServer";
            this.txtServer.Size = new System.Drawing.Size(244, 29);
            this.txtServer.TabIndex = 75;
            this.txtServer.Text = "127.0.0.1:1883";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.label7.Location = new System.Drawing.Point(51, 108);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(62, 21);
            this.label7.TabIndex = 84;
            this.label7.Text = "用户名:";
            // 
            // txtUid
            // 
            this.txtUid.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.txtUid.Location = new System.Drawing.Point(119, 104);
            this.txtUid.Name = "txtUid";
            this.txtUid.Size = new System.Drawing.Size(244, 29);
            this.txtUid.TabIndex = 83;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.label6.Location = new System.Drawing.Point(67, 146);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(46, 21);
            this.label6.TabIndex = 82;
            this.label6.Text = "密码:";
            // 
            // txtPwd
            // 
            this.txtPwd.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.txtPwd.Location = new System.Drawing.Point(119, 141);
            this.txtPwd.Name = "txtPwd";
            this.txtPwd.Size = new System.Drawing.Size(244, 29);
            this.txtPwd.TabIndex = 81;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.label8.Location = new System.Drawing.Point(43, 183);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(70, 21);
            this.label8.TabIndex = 86;
            this.label8.Text = "clientId:";
            this.tsNote.SetToolTip(this.label8, "客户端id，\r\n如果多个客户端用同一个ID则会互相挤出");
            // 
            // txtClientId
            // 
            this.txtClientId.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.txtClientId.Location = new System.Drawing.Point(119, 178);
            this.txtClientId.Name = "txtClientId";
            this.txtClientId.Size = new System.Drawing.Size(244, 29);
            this.txtClientId.TabIndex = 85;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.label9.Location = new System.Drawing.Point(26, 219);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(87, 21);
            this.label9.TabIndex = 88;
            this.label9.Text = "WS服务器:";
            // 
            // txtWebSocketServer
            // 
            this.txtWebSocketServer.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.txtWebSocketServer.Location = new System.Drawing.Point(119, 215);
            this.txtWebSocketServer.Name = "txtWebSocketServer";
            this.txtWebSocketServer.Size = new System.Drawing.Size(244, 29);
            this.txtWebSocketServer.TabIndex = 87;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.label10.Location = new System.Drawing.Point(9, 259);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(104, 21);
            this.label10.TabIndex = 90;
            this.label10.Text = "会话超时(秒):";
            // 
            // nupSessionExpiry
            // 
            this.nupSessionExpiry.Location = new System.Drawing.Point(119, 252);
            this.nupSessionExpiry.Maximum = new decimal(new int[] {
            1000000000,
            0,
            0,
            0});
            this.nupSessionExpiry.Name = "nupSessionExpiry";
            this.nupSessionExpiry.Size = new System.Drawing.Size(244, 33);
            this.nupSessionExpiry.TabIndex = 91;
            // 
            // nulKeepAlivePeriod
            // 
            this.nulKeepAlivePeriod.Location = new System.Drawing.Point(119, 291);
            this.nulKeepAlivePeriod.Maximum = new decimal(new int[] {
            1000000000,
            0,
            0,
            0});
            this.nulKeepAlivePeriod.Name = "nulKeepAlivePeriod";
            this.nulKeepAlivePeriod.Size = new System.Drawing.Size(244, 33);
            this.nulKeepAlivePeriod.TabIndex = 93;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.label11.Location = new System.Drawing.Point(10, 297);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(104, 21);
            this.label11.TabIndex = 92;
            this.label11.Text = "连接超时(秒):";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.label12.Location = new System.Drawing.Point(415, 71);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(78, 21);
            this.label12.TabIndex = 99;
            this.label12.Text = "代理用户:";
            // 
            // txtProxyUserName
            // 
            this.txtProxyUserName.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.txtProxyUserName.Location = new System.Drawing.Point(498, 67);
            this.txtProxyUserName.Name = "txtProxyUserName";
            this.txtProxyUserName.Size = new System.Drawing.Size(244, 29);
            this.txtProxyUserName.TabIndex = 98;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.label13.Location = new System.Drawing.Point(415, 108);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(78, 21);
            this.label13.TabIndex = 97;
            this.label13.Text = "代理密码:";
            // 
            // txtProxyPassword
            // 
            this.txtProxyPassword.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.txtProxyPassword.Location = new System.Drawing.Point(498, 104);
            this.txtProxyPassword.Name = "txtProxyPassword";
            this.txtProxyPassword.Size = new System.Drawing.Size(244, 29);
            this.txtProxyPassword.TabIndex = 96;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.label14.Location = new System.Drawing.Point(447, 34);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(46, 21);
            this.label14.TabIndex = 95;
            this.label14.Text = "代理:";
            // 
            // txtProxy
            // 
            this.txtProxy.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.txtProxy.Location = new System.Drawing.Point(498, 30);
            this.txtProxy.Name = "txtProxy";
            this.txtProxy.Size = new System.Drawing.Size(244, 29);
            this.txtProxy.TabIndex = 94;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.label15.Location = new System.Drawing.Point(431, 144);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(62, 21);
            this.label15.TabIndex = 101;
            this.label15.Text = "代理域:";
            // 
            // txtDomain
            // 
            this.txtDomain.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.txtDomain.Location = new System.Drawing.Point(498, 141);
            this.txtDomain.Name = "txtDomain";
            this.txtDomain.Size = new System.Drawing.Size(244, 29);
            this.txtDomain.TabIndex = 100;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.label16.Location = new System.Drawing.Point(415, 183);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(78, 21);
            this.label16.TabIndex = 102;
            this.label16.Text = "消息等级:";
            // 
            // cmbQualityOfServiceLevel
            // 
            this.cmbQualityOfServiceLevel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbQualityOfServiceLevel.FormattingEnabled = true;
            this.cmbQualityOfServiceLevel.Location = new System.Drawing.Point(498, 178);
            this.cmbQualityOfServiceLevel.Name = "cmbQualityOfServiceLevel";
            this.cmbQualityOfServiceLevel.Size = new System.Drawing.Size(244, 33);
            this.cmbQualityOfServiceLevel.TabIndex = 103;
            // 
            // chkRetainAsPublished
            // 
            this.chkRetainAsPublished.AutoSize = true;
            this.chkRetainAsPublished.Location = new System.Drawing.Point(14, 344);
            this.chkRetainAsPublished.Name = "chkRetainAsPublished";
            this.chkRetainAsPublished.Size = new System.Drawing.Size(184, 29);
            this.chkRetainAsPublished.TabIndex = 104;
            this.chkRetainAsPublished.Text = "保留为已发布(5.0)";
            this.tsNote.SetToolTip(this.chkRetainAsPublished, "订阅选项的第 3 位表示保留为已发布(Retain As Published)。\r\n若该值为 1，服务器须将转发消息的 RETAIN flag 设为与接收到的 P" +
        "UBLISH 报文的 RETAIN flag 一致。\r\n若该值为 0，不管接收到的 PUBLISH 报文中的 RETAIN flag 是何值，\r\n服务器都需将转" +
        "发消息的 RETAIN flag 置为 0。");
            this.chkRetainAsPublished.UseVisualStyleBackColor = true;
            // 
            // chkNoLocal
            // 
            this.chkNoLocal.AutoSize = true;
            this.chkNoLocal.Location = new System.Drawing.Point(204, 344);
            this.chkNoLocal.Name = "chkNoLocal";
            this.chkNoLocal.Size = new System.Drawing.Size(127, 29);
            this.chkNoLocal.TabIndex = 105;
            this.chkNoLocal.Text = "非本地(5.0)";
            this.tsNote.SetToolTip(this.chkNoLocal, "订阅选项第 2 位表示非本地选项(No Local)。\r\n如果值为 1，应用消息就不会发布给订阅发布主题的发布者本身，\r\n如果在共享订阅中将该选项设置为 1 的话" +
        "，\r\n就会触发协议错误。");
            this.chkNoLocal.UseVisualStyleBackColor = true;
            // 
            // cmbRetainHandling
            // 
            this.cmbRetainHandling.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbRetainHandling.FormattingEnabled = true;
            this.cmbRetainHandling.Location = new System.Drawing.Point(498, 252);
            this.cmbRetainHandling.Name = "cmbRetainHandling";
            this.cmbRetainHandling.Size = new System.Drawing.Size(244, 33);
            this.cmbRetainHandling.TabIndex = 107;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.label17.Location = new System.Drawing.Point(389, 258);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(110, 21);
            this.label17.TabIndex = 106;
            this.label17.Text = "保留消息(5.0):";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.label18.Location = new System.Drawing.Point(51, 34);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(62, 21);
            this.label18.TabIndex = 109;
            this.label18.Text = "队列名:";
            // 
            // txtName
            // 
            this.txtName.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.txtName.Location = new System.Drawing.Point(119, 30);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(244, 29);
            this.txtName.TabIndex = 108;
            this.tsNote.SetToolTip(this.txtName, "代码里的队列名区分多个队列的链接");
            // 
            // chkKeepAlive
            // 
            this.chkKeepAlive.AutoSize = true;
            this.chkKeepAlive.Checked = true;
            this.chkKeepAlive.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkKeepAlive.Location = new System.Drawing.Point(346, 344);
            this.chkKeepAlive.Name = "chkKeepAlive";
            this.chkKeepAlive.Size = new System.Drawing.Size(121, 29);
            this.chkKeepAlive.TabIndex = 110;
            this.chkKeepAlive.Text = "KeepAlive";
            this.chkKeepAlive.UseVisualStyleBackColor = true;
            // 
            // cmbProtocolVersion
            // 
            this.cmbProtocolVersion.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbProtocolVersion.FormattingEnabled = true;
            this.cmbProtocolVersion.Location = new System.Drawing.Point(498, 215);
            this.cmbProtocolVersion.Name = "cmbProtocolVersion";
            this.cmbProtocolVersion.Size = new System.Drawing.Size(244, 33);
            this.cmbProtocolVersion.TabIndex = 112;
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.label19.Location = new System.Drawing.Point(446, 221);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(46, 21);
            this.label19.TabIndex = 111;
            this.label19.Text = "协议:";
            // 
            // chkCleanSession
            // 
            this.chkCleanSession.AutoSize = true;
            this.chkCleanSession.Checked = true;
            this.chkCleanSession.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkCleanSession.Location = new System.Drawing.Point(498, 344);
            this.chkCleanSession.Name = "chkCleanSession";
            this.chkCleanSession.Size = new System.Drawing.Size(152, 29);
            this.chkCleanSession.TabIndex = 113;
            this.chkCleanSession.Text = "CleanSession";
            this.tsNote.SetToolTip(this.chkCleanSession, "是用来控制会话状态生命周期的标志位，\r\n为 true 时表示创建一个新的会话，在客户端断开连接时，\r\n会话将自动销毁。\r\n为 false 时表示创建一个持久会话，" +
        "在客户端断开连接后会话仍然保持，直到会话超时注销。\r\n5.0时候则设置CleanStart");
            this.chkCleanSession.UseVisualStyleBackColor = true;
            // 
            // nupSessionExpiryInterval
            // 
            this.nupSessionExpiryInterval.Location = new System.Drawing.Point(536, 293);
            this.nupSessionExpiryInterval.Maximum = new decimal(new int[] {
            1000000000,
            0,
            0,
            0});
            this.nupSessionExpiryInterval.Name = "nupSessionExpiryInterval";
            this.nupSessionExpiryInterval.Size = new System.Drawing.Size(206, 33);
            this.nupSessionExpiryInterval.TabIndex = 115;
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.label20.Location = new System.Drawing.Point(364, 298);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(166, 21);
            this.label20.TabIndex = 114;
            this.label20.Text = "Session超时秒数(5.0):";
            this.tsNote.SetToolTip(this.label20, "Session Expiry Interval 以秒为单位，\r\n如果 Session Expiry Interval 设置为 0 或者未指定，\r\n会话将在网络连接" +
        "关闭时结束。");
            // 
            // UIMQTT
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "UIMQTT";
            this.Size = new System.Drawing.Size(750, 640);
            this.gpSetting.ResumeLayout(false);
            this.gpSetting.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nupSessionExpiry)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nulKeepAlivePeriod)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nupSessionExpiryInterval)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtServer;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtClientId;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtUid;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtPwd;
        private System.Windows.Forms.NumericUpDown nulKeepAlivePeriod;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.NumericUpDown nupSessionExpiry;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtWebSocketServer;
        private System.Windows.Forms.ComboBox cmbRetainHandling;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.CheckBox chkNoLocal;
        private System.Windows.Forms.CheckBox chkRetainAsPublished;
        private System.Windows.Forms.ComboBox cmbQualityOfServiceLevel;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox txtDomain;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox txtProxyUserName;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox txtProxyPassword;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox txtProxy;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.CheckBox chkKeepAlive;
        private System.Windows.Forms.ComboBox cmbProtocolVersion;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.NumericUpDown nupSessionExpiryInterval;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.CheckBox chkCleanSession;
    }
}
