namespace AddInSetup.ConnStringUI
{
    partial class UIAWS
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
            this.txtInternetUrl = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtTimeout = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.txtBucketName = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtSecretKey = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtAccessKey = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.cmbACL = new System.Windows.Forms.ComboBox();
            this.chkHash = new System.Windows.Forms.CheckBox();
            this.label8 = new System.Windows.Forms.Label();
            this.txtServer = new System.Windows.Forms.ComboBox();
            this.gpSetting.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtTimeout)).BeginInit();
            this.SuspendLayout();
            // 
            // gpSetting
            // 
            this.gpSetting.Controls.Add(this.txtServer);
            this.gpSetting.Controls.Add(this.chkHash);
            this.gpSetting.Controls.Add(this.label8);
            this.gpSetting.Controls.Add(this.cmbACL);
            this.gpSetting.Controls.Add(this.label6);
            this.gpSetting.Controls.Add(this.txtInternetUrl);
            this.gpSetting.Controls.Add(this.label7);
            this.gpSetting.Controls.Add(this.txtTimeout);
            this.gpSetting.Controls.Add(this.label5);
            this.gpSetting.Controls.Add(this.txtBucketName);
            this.gpSetting.Controls.Add(this.label4);
            this.gpSetting.Controls.Add(this.txtSecretKey);
            this.gpSetting.Controls.Add(this.label3);
            this.gpSetting.Controls.Add(this.txtAccessKey);
            this.gpSetting.Controls.Add(this.label2);
            this.gpSetting.Controls.Add(this.label1);
            this.gpSetting.Size = new System.Drawing.Size(1010, 371);
            // 
            // txtInternetUrl
            // 
            this.txtInternetUrl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtInternetUrl.Location = new System.Drawing.Point(106, 221);
            this.txtInternetUrl.Name = "txtInternetUrl";
            this.txtInternetUrl.Size = new System.Drawing.Size(586, 33);
            this.txtInternetUrl.TabIndex = 43;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.label7.Location = new System.Drawing.Point(23, 227);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(78, 21);
            this.label7.TabIndex = 42;
            this.label7.Text = "外网地址:";
            // 
            // txtTimeout
            // 
            this.txtTimeout.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtTimeout.Location = new System.Drawing.Point(107, 182);
            this.txtTimeout.Maximum = new decimal(new int[] {
            999999999,
            0,
            0,
            0});
            this.txtTimeout.Name = "txtTimeout";
            this.txtTimeout.Size = new System.Drawing.Size(585, 33);
            this.txtTimeout.TabIndex = 39;
            this.txtTimeout.Value = new decimal(new int[] {
            60000,
            0,
            0,
            0});
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.label5.Location = new System.Drawing.Point(13, 187);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(88, 21);
            this.label5.TabIndex = 38;
            this.label5.Text = "超时(毫秒):";
            // 
            // txtBucketName
            // 
            this.txtBucketName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtBucketName.Location = new System.Drawing.Point(107, 143);
            this.txtBucketName.Name = "txtBucketName";
            this.txtBucketName.Size = new System.Drawing.Size(586, 33);
            this.txtBucketName.TabIndex = 37;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.label4.Location = new System.Drawing.Point(35, 149);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(66, 21);
            this.label4.TabIndex = 36;
            this.label4.Text = "Bucket:";
            // 
            // txtSecretKey
            // 
            this.txtSecretKey.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSecretKey.Location = new System.Drawing.Point(107, 104);
            this.txtSecretKey.Name = "txtSecretKey";
            this.txtSecretKey.Size = new System.Drawing.Size(586, 33);
            this.txtSecretKey.TabIndex = 35;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.label3.Location = new System.Drawing.Point(13, 110);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(88, 21);
            this.label3.TabIndex = 34;
            this.label3.Text = "SecretKey:";
            // 
            // txtAccessKey
            // 
            this.txtAccessKey.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtAccessKey.Location = new System.Drawing.Point(106, 65);
            this.txtAccessKey.Name = "txtAccessKey";
            this.txtAccessKey.Size = new System.Drawing.Size(586, 33);
            this.txtAccessKey.TabIndex = 33;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.label2.Location = new System.Drawing.Point(10, 71);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(91, 21);
            this.label2.TabIndex = 32;
            this.label2.Text = "AccessKey:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.label1.Location = new System.Drawing.Point(39, 32);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(62, 21);
            this.label1.TabIndex = 30;
            this.label1.Text = "服务器:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.label6.Location = new System.Drawing.Point(55, 266);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(46, 21);
            this.label6.TabIndex = 44;
            this.label6.Text = "权限:";
            // 
            // cmbACL
            // 
            this.cmbACL.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbACL.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbACL.FormattingEnabled = true;
            this.cmbACL.Location = new System.Drawing.Point(106, 260);
            this.cmbACL.Name = "cmbACL";
            this.cmbACL.Size = new System.Drawing.Size(586, 33);
            this.cmbACL.TabIndex = 45;
            // 
            // chkHash
            // 
            this.chkHash.AutoSize = true;
            this.chkHash.Font = new System.Drawing.Font("微软雅黑", 14F);
            this.chkHash.Location = new System.Drawing.Point(107, 301);
            this.chkHash.Name = "chkHash";
            this.chkHash.Size = new System.Drawing.Size(15, 14);
            this.chkHash.TabIndex = 47;
            this.chkHash.UseVisualStyleBackColor = true;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.label8.Location = new System.Drawing.Point(17, 296);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(84, 21);
            this.label8.TabIndex = 46;
            this.label8.Text = "Hash效验:";
            // 
            // txtServer
            // 
            this.txtServer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtServer.FormattingEnabled = true;
            this.txtServer.Location = new System.Drawing.Point(106, 26);
            this.txtServer.Name = "txtServer";
            this.txtServer.Size = new System.Drawing.Size(586, 33);
            this.txtServer.TabIndex = 48;
            // 
            // UIAWS
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Margin = new System.Windows.Forms.Padding(8, 9, 8, 9);
            this.Name = "UIAWS";
            this.Size = new System.Drawing.Size(1010, 632);
            this.gpSetting.ResumeLayout(false);
            this.gpSetting.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtTimeout)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox cmbACL;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtInternetUrl;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.NumericUpDown txtTimeout;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtBucketName;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtSecretKey;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtAccessKey;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox chkHash;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox txtServer;
    }
}
