namespace AddInSetup.ConnStringUI
{
    partial class UIRabbitMQ
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
            this.label8 = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txtPwd = new System.Windows.Forms.TextBox();
            this.txtServer = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtUid = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.txtExchangeName = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.cmbExchangeMode = new System.Windows.Forms.ComboBox();
            this.txtQueue = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.chkAutoDelete = new System.Windows.Forms.CheckBox();
            this.chkDeliveryMode = new System.Windows.Forms.CheckBox();
            this.label12 = new System.Windows.Forms.Label();
            this.txtVirtualHost = new System.Windows.Forms.TextBox();
            this.gpSetting.SuspendLayout();
            this.SuspendLayout();
            // 
            // gpSetting
            // 
            this.gpSetting.Controls.Add(this.label12);
            this.gpSetting.Controls.Add(this.txtVirtualHost);
            this.gpSetting.Controls.Add(this.chkDeliveryMode);
            this.gpSetting.Controls.Add(this.chkAutoDelete);
            this.gpSetting.Controls.Add(this.label11);
            this.gpSetting.Controls.Add(this.txtQueue);
            this.gpSetting.Controls.Add(this.cmbExchangeMode);
            this.gpSetting.Controls.Add(this.label10);
            this.gpSetting.Controls.Add(this.label9);
            this.gpSetting.Controls.Add(this.txtExchangeName);
            this.gpSetting.Controls.Add(this.label7);
            this.gpSetting.Controls.Add(this.txtUid);
            this.gpSetting.Controls.Add(this.label8);
            this.gpSetting.Controls.Add(this.txtName);
            this.gpSetting.Controls.Add(this.label6);
            this.gpSetting.Controls.Add(this.label5);
            this.gpSetting.Controls.Add(this.txtPwd);
            this.gpSetting.Controls.Add(this.txtServer);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.label8.Location = new System.Drawing.Point(126, 37);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(62, 21);
            this.label8.TabIndex = 78;
            this.label8.Text = "队列名:";
            // 
            // txtName
            // 
            this.txtName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtName.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.txtName.Location = new System.Drawing.Point(194, 32);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(457, 29);
            this.txtName.TabIndex = 77;
            this.txtName.Text = "myTest";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.label6.Location = new System.Drawing.Point(142, 177);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(46, 21);
            this.label6.TabIndex = 75;
            this.label6.Text = "密码:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.label5.Location = new System.Drawing.Point(126, 72);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(62, 21);
            this.label5.TabIndex = 74;
            this.label5.Text = "服务器:";
            // 
            // txtPwd
            // 
            this.txtPwd.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPwd.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.txtPwd.Location = new System.Drawing.Point(194, 173);
            this.txtPwd.Name = "txtPwd";
            this.txtPwd.Size = new System.Drawing.Size(457, 29);
            this.txtPwd.TabIndex = 72;
            // 
            // txtServer
            // 
            this.txtServer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtServer.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.txtServer.Location = new System.Drawing.Point(194, 67);
            this.txtServer.Name = "txtServer";
            this.txtServer.Size = new System.Drawing.Size(457, 29);
            this.txtServer.TabIndex = 70;
            this.txtServer.Text = "127.0.0.1:5672";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.label7.Location = new System.Drawing.Point(126, 142);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(62, 21);
            this.label7.TabIndex = 80;
            this.label7.Text = "用户名:";
            // 
            // txtUid
            // 
            this.txtUid.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtUid.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.txtUid.Location = new System.Drawing.Point(194, 138);
            this.txtUid.Name = "txtUid";
            this.txtUid.Size = new System.Drawing.Size(457, 29);
            this.txtUid.TabIndex = 79;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.label9.Location = new System.Drawing.Point(94, 212);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(94, 21);
            this.label9.TabIndex = 82;
            this.label9.Text = "交换器名称:";
            // 
            // txtExchangeName
            // 
            this.txtExchangeName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtExchangeName.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.txtExchangeName.Location = new System.Drawing.Point(194, 208);
            this.txtExchangeName.Name = "txtExchangeName";
            this.txtExchangeName.Size = new System.Drawing.Size(457, 29);
            this.txtExchangeName.TabIndex = 81;
            this.txtExchangeName.Text = "MyExchange";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.label10.Location = new System.Drawing.Point(94, 247);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(94, 21);
            this.label10.TabIndex = 84;
            this.label10.Text = "交换器模式:";
            // 
            // cmbExchangeMode
            // 
            this.cmbExchangeMode.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbExchangeMode.FormattingEnabled = true;
            this.cmbExchangeMode.Items.AddRange(new object[] {
            "direct",
            "fanout",
            "headers",
            "topic"});
            this.cmbExchangeMode.Location = new System.Drawing.Point(194, 243);
            this.cmbExchangeMode.Name = "cmbExchangeMode";
            this.cmbExchangeMode.Size = new System.Drawing.Size(457, 33);
            this.cmbExchangeMode.TabIndex = 85;
            this.cmbExchangeMode.Text = "fanout";
            // 
            // txtQueue
            // 
            this.txtQueue.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtQueue.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.txtQueue.Location = new System.Drawing.Point(194, 282);
            this.txtQueue.Name = "txtQueue";
            this.txtQueue.Size = new System.Drawing.Size(457, 29);
            this.txtQueue.TabIndex = 86;
            this.txtQueue.Text = "myqueue";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.label11.Location = new System.Drawing.Point(16, 282);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(172, 21);
            this.label11.TabIndex = 87;
            this.label11.Text = "绑定的队列名(用|隔开):";
            // 
            // chkAutoDelete
            // 
            this.chkAutoDelete.AutoSize = true;
            this.chkAutoDelete.Location = new System.Drawing.Point(288, 332);
            this.chkAutoDelete.Name = "chkAutoDelete";
            this.chkAutoDelete.Size = new System.Drawing.Size(107, 29);
            this.chkAutoDelete.TabIndex = 88;
            this.chkAutoDelete.Text = "自动删除";
            this.chkAutoDelete.UseVisualStyleBackColor = true;
            // 
            // chkDeliveryMode
            // 
            this.chkDeliveryMode.AutoSize = true;
            this.chkDeliveryMode.Location = new System.Drawing.Point(194, 332);
            this.chkDeliveryMode.Name = "chkDeliveryMode";
            this.chkDeliveryMode.Size = new System.Drawing.Size(88, 29);
            this.chkDeliveryMode.TabIndex = 89;
            this.chkDeliveryMode.Text = "持久化";
            this.chkDeliveryMode.UseVisualStyleBackColor = true;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.label12.Location = new System.Drawing.Point(89, 107);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(99, 21);
            this.label12.TabIndex = 91;
            this.label12.Text = "VirtualHost:";
            // 
            // txtVirtualHost
            // 
            this.txtVirtualHost.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtVirtualHost.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.txtVirtualHost.Location = new System.Drawing.Point(194, 103);
            this.txtVirtualHost.Name = "txtVirtualHost";
            this.txtVirtualHost.Size = new System.Drawing.Size(457, 29);
            this.txtVirtualHost.TabIndex = 90;
            this.txtVirtualHost.Text = "/";
            // 
            // UIRabbitMQ
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "UIRabbitMQ";
            this.gpSetting.ResumeLayout(false);
            this.gpSetting.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtPwd;
        private System.Windows.Forms.TextBox txtServer;
        private System.Windows.Forms.ComboBox cmbExchangeMode;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtExchangeName;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtUid;
        private System.Windows.Forms.CheckBox chkDeliveryMode;
        private System.Windows.Forms.CheckBox chkAutoDelete;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox txtQueue;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox txtVirtualHost;
    }
}
