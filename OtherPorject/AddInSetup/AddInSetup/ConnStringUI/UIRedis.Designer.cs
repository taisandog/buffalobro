namespace AddInSetup.ConnStringUI
{
    partial class UIRedis
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
            this.chkThrow = new System.Windows.Forms.CheckBox();
            this.txtPoolsize = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.txtExpir = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.txtServer = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtRoServer = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.gpSetting.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtPoolsize)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtExpir)).BeginInit();
            this.SuspendLayout();
            // 
            // gpSetting
            // 
            this.gpSetting.Controls.Add(this.label3);
            this.gpSetting.Controls.Add(this.txtRoServer);
            this.gpSetting.Controls.Add(this.label6);
            this.gpSetting.Controls.Add(this.chkThrow);
            this.gpSetting.Controls.Add(this.txtPoolsize);
            this.gpSetting.Controls.Add(this.label2);
            this.gpSetting.Controls.Add(this.txtExpir);
            this.gpSetting.Controls.Add(this.label5);
            this.gpSetting.Controls.Add(this.txtServer);
            this.gpSetting.Controls.Add(this.label1);
            // 
            // chkThrow
            // 
            this.chkThrow.AutoSize = true;
            this.chkThrow.Location = new System.Drawing.Point(194, 246);
            this.chkThrow.Name = "chkThrow";
            this.chkThrow.Size = new System.Drawing.Size(297, 29);
            this.chkThrow.TabIndex = 45;
            this.chkThrow.Text = "是当存取错误时候是否抛出异常";
            this.chkThrow.UseVisualStyleBackColor = true;
            // 
            // txtPoolsize
            // 
            this.txtPoolsize.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPoolsize.Location = new System.Drawing.Point(194, 207);
            this.txtPoolsize.Maximum = new decimal(new int[] {
            999999999,
            0,
            0,
            0});
            this.txtPoolsize.Name = "txtPoolsize";
            this.txtPoolsize.Size = new System.Drawing.Size(457, 33);
            this.txtPoolsize.TabIndex = 40;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.label2.Location = new System.Drawing.Point(27, 212);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(161, 21);
            this.label2.TabIndex = 39;
            this.label2.Text = "最大连接数(0不启用):";
            // 
            // txtExpir
            // 
            this.txtExpir.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtExpir.Location = new System.Drawing.Point(194, 168);
            this.txtExpir.Maximum = new decimal(new int[] {
            999999999,
            0,
            0,
            0});
            this.txtExpir.Name = "txtExpir";
            this.txtExpir.Size = new System.Drawing.Size(457, 33);
            this.txtExpir.TabIndex = 38;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.label5.Location = new System.Drawing.Point(7, 173);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(181, 21);
            this.label5.TabIndex = 37;
            this.label5.Text = "超时(分钟,0表示不超时):";
            // 
            // txtServer
            // 
            this.txtServer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtServer.Location = new System.Drawing.Point(194, 32);
            this.txtServer.Name = "txtServer";
            this.txtServer.Size = new System.Drawing.Size(457, 33);
            this.txtServer.TabIndex = 36;
            this.txtServer.Text = "127.0.0.1:6379";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.label1.Location = new System.Drawing.Point(4, 38);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(184, 21);
            this.label1.TabIndex = 35;
            this.label1.Text = "服务器(多个用逗号隔开):";
            // 
            // txtRoServer
            // 
            this.txtRoServer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtRoServer.Location = new System.Drawing.Point(194, 71);
            this.txtRoServer.Name = "txtRoServer";
            this.txtRoServer.Size = new System.Drawing.Size(457, 33);
            this.txtRoServer.TabIndex = 47;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.label6.Location = new System.Drawing.Point(4, 77);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(184, 21);
            this.label6.TabIndex = 46;
            this.label6.Text = "只读服务器(用逗号隔开):";
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("楷体", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(191, 107);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(277, 46);
            this.label3.TabIndex = 48;
            this.label3.Text = "服务器格式使用 密码@IP:端口 形式\r\n例如：123456@127.0.0.1:6379";
            // 
            // UIRedis
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "UIRedis";
            this.gpSetting.ResumeLayout(false);
            this.gpSetting.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtPoolsize)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtExpir)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.CheckBox chkThrow;
        private System.Windows.Forms.NumericUpDown txtPoolsize;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown txtExpir;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtServer;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtRoServer;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label3;
    }
}
