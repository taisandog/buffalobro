namespace AddInSetup.ConnStringUI
{
    partial class UIRedis2
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
            this.chkThrow = new System.Windows.Forms.CheckBox();
            this.txtExpir = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.txtServer = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtPwd = new System.Windows.Forms.TextBox();
            this.chkSSL = new System.Windows.Forms.CheckBox();
            this.cmbCommandFlags = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtDatabase = new System.Windows.Forms.NumericUpDown();
            this.label7 = new System.Windows.Forms.Label();
            this.gpSetting.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtExpir)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDatabase)).BeginInit();
            this.SuspendLayout();
            // 
            // gpSetting
            // 
            this.gpSetting.Controls.Add(this.txtDatabase);
            this.gpSetting.Controls.Add(this.label7);
            this.gpSetting.Controls.Add(this.label6);
            this.gpSetting.Controls.Add(this.cmbCommandFlags);
            this.gpSetting.Controls.Add(this.chkSSL);
            this.gpSetting.Controls.Add(this.txtPwd);
            this.gpSetting.Controls.Add(this.label4);
            this.gpSetting.Controls.Add(this.chkThrow);
            this.gpSetting.Controls.Add(this.txtExpir);
            this.gpSetting.Controls.Add(this.label5);
            this.gpSetting.Controls.Add(this.txtServer);
            this.gpSetting.Controls.Add(this.label1);
            // 
            // chkThrow
            // 
            this.chkThrow.AutoSize = true;
            this.chkThrow.Location = new System.Drawing.Point(198, 263);
            this.chkThrow.Name = "chkThrow";
            this.chkThrow.Size = new System.Drawing.Size(297, 29);
            this.chkThrow.TabIndex = 55;
            this.chkThrow.Text = "是当存取错误时候是否抛出异常";
            this.chkThrow.UseVisualStyleBackColor = true;
            // 
            // txtExpir
            // 
            this.txtExpir.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtExpir.Location = new System.Drawing.Point(194, 110);
            this.txtExpir.Maximum = new decimal(new int[] {
            999999999,
            0,
            0,
            0});
            this.txtExpir.Name = "txtExpir";
            this.txtExpir.Size = new System.Drawing.Size(457, 33);
            this.txtExpir.TabIndex = 52;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.label5.Location = new System.Drawing.Point(7, 115);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(181, 21);
            this.label5.TabIndex = 51;
            this.label5.Text = "超时(分钟,0表示不超时):";
            // 
            // txtServer
            // 
            this.txtServer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtServer.Location = new System.Drawing.Point(194, 32);
            this.txtServer.Name = "txtServer";
            this.txtServer.Size = new System.Drawing.Size(457, 33);
            this.txtServer.TabIndex = 50;
            this.txtServer.Text = "127.0.0.1:6379";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.label1.Location = new System.Drawing.Point(4, 38);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(184, 21);
            this.label1.TabIndex = 49;
            this.label1.Text = "服务器(多个用逗号隔开):";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.label4.Location = new System.Drawing.Point(142, 77);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(46, 21);
            this.label4.TabIndex = 57;
            this.label4.Text = "密码:";
            // 
            // txtPwd
            // 
            this.txtPwd.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPwd.Location = new System.Drawing.Point(194, 71);
            this.txtPwd.Name = "txtPwd";
            this.txtPwd.Size = new System.Drawing.Size(457, 33);
            this.txtPwd.TabIndex = 58;
            // 
            // chkSSL
            // 
            this.chkSSL.AutoSize = true;
            this.chkSSL.Location = new System.Drawing.Point(195, 228);
            this.chkSSL.Name = "chkSSL";
            this.chkSSL.Size = new System.Drawing.Size(63, 29);
            this.chkSSL.TabIndex = 59;
            this.chkSSL.Text = "SSL";
            this.chkSSL.UseVisualStyleBackColor = true;
            // 
            // cmbCommandFlags
            // 
            this.cmbCommandFlags.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbCommandFlags.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCommandFlags.FormattingEnabled = true;
            this.cmbCommandFlags.Location = new System.Drawing.Point(194, 189);
            this.cmbCommandFlags.Name = "cmbCommandFlags";
            this.cmbCommandFlags.Size = new System.Drawing.Size(457, 33);
            this.cmbCommandFlags.TabIndex = 60;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.label6.Location = new System.Drawing.Point(55, 195);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(133, 21);
            this.label6.TabIndex = 61;
            this.label6.Text = "CommandFlags:";
            // 
            // txtDatabase
            // 
            this.txtDatabase.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDatabase.Location = new System.Drawing.Point(196, 150);
            this.txtDatabase.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.txtDatabase.Name = "txtDatabase";
            this.txtDatabase.Size = new System.Drawing.Size(457, 33);
            this.txtDatabase.TabIndex = 63;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.label7.Location = new System.Drawing.Point(94, 155);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(94, 21);
            this.label7.TabIndex = 62;
            this.label7.Text = "使用数据库:";
            // 
            // UIRedis2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "UIRedis2";
            this.gpSetting.ResumeLayout(false);
            this.gpSetting.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtExpir)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDatabase)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.CheckBox chkThrow;
        private System.Windows.Forms.NumericUpDown txtExpir;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtServer;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox chkSSL;
        private System.Windows.Forms.TextBox txtPwd;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox cmbCommandFlags;
        private System.Windows.Forms.NumericUpDown txtDatabase;
        private System.Windows.Forms.Label label7;
    }
}
