namespace AddInSetup.ConnStringUI
{
    partial class UIConnBase
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
            this.components = new System.ComponentModel.Container();
            this.gpOutput = new System.Windows.Forms.GroupBox();
            this.btnTech = new System.Windows.Forms.Button();
            this.scOut = new System.Windows.Forms.SplitContainer();
            this.txtOutConn = new System.Windows.Forms.RichTextBox();
            this.cmsCopy = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.txtOutCode = new System.Windows.Forms.RichTextBox();
            this.gpSetting = new System.Windows.Forms.GroupBox();
            this.pnlButton = new System.Windows.Forms.Panel();
            this.gbProxy = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtProxyPass = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtProxyUser = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtProxyPort = new System.Windows.Forms.NumericUpDown();
            this.txtProxyHost = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnTest = new System.Windows.Forms.Button();
            this.btnOut = new System.Windows.Forms.Button();
            this.gpOutput.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.scOut)).BeginInit();
            this.scOut.Panel1.SuspendLayout();
            this.scOut.Panel2.SuspendLayout();
            this.scOut.SuspendLayout();
            this.cmsCopy.SuspendLayout();
            this.pnlButton.SuspendLayout();
            this.gbProxy.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtProxyPort)).BeginInit();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // gpOutput
            // 
            this.gpOutput.Controls.Add(this.btnTech);
            this.gpOutput.Controls.Add(this.scOut);
            this.gpOutput.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.gpOutput.Location = new System.Drawing.Point(0, 497);
            this.gpOutput.Margin = new System.Windows.Forms.Padding(5);
            this.gpOutput.Name = "gpOutput";
            this.gpOutput.Padding = new System.Windows.Forms.Padding(5);
            this.gpOutput.Size = new System.Drawing.Size(659, 143);
            this.gpOutput.TabIndex = 0;
            this.gpOutput.TabStop = false;
            this.gpOutput.Text = "输出";
            this.gpOutput.SizeChanged += new System.EventHandler(this.gpOutput_SizeChanged);
            // 
            // btnTech
            // 
            this.btnTech.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnTech.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.btnTech.Location = new System.Drawing.Point(513, 1);
            this.btnTech.Name = "btnTech";
            this.btnTech.Size = new System.Drawing.Size(138, 27);
            this.btnTech.TabIndex = 2;
            this.btnTech.Text = "帮助";
            this.btnTech.UseVisualStyleBackColor = true;
            this.btnTech.Click += new System.EventHandler(this.btnTech_Click);
            // 
            // scOut
            // 
            this.scOut.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scOut.Location = new System.Drawing.Point(5, 27);
            this.scOut.Name = "scOut";
            // 
            // scOut.Panel1
            // 
            this.scOut.Panel1.Controls.Add(this.txtOutConn);
            // 
            // scOut.Panel2
            // 
            this.scOut.Panel2.Controls.Add(this.txtOutCode);
            this.scOut.Size = new System.Drawing.Size(649, 111);
            this.scOut.SplitterDistance = 287;
            this.scOut.TabIndex = 2;
            // 
            // txtOutConn
            // 
            this.txtOutConn.ContextMenuStrip = this.cmsCopy;
            this.txtOutConn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtOutConn.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.txtOutConn.Location = new System.Drawing.Point(0, 0);
            this.txtOutConn.Name = "txtOutConn";
            this.txtOutConn.ReadOnly = true;
            this.txtOutConn.Size = new System.Drawing.Size(287, 111);
            this.txtOutConn.TabIndex = 1;
            this.txtOutConn.Text = "";
            // 
            // cmsCopy
            // 
            this.cmsCopy.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsCopy});
            this.cmsCopy.Name = "cmsCopy";
            this.cmsCopy.Size = new System.Drawing.Size(101, 26);
            // 
            // tsCopy
            // 
            this.tsCopy.Name = "tsCopy";
            this.tsCopy.Size = new System.Drawing.Size(100, 22);
            this.tsCopy.Text = "复制";
            this.tsCopy.Click += new System.EventHandler(this.tsCopy_Click);
            // 
            // txtOutCode
            // 
            this.txtOutCode.ContextMenuStrip = this.cmsCopy;
            this.txtOutCode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtOutCode.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.txtOutCode.Location = new System.Drawing.Point(0, 0);
            this.txtOutCode.Name = "txtOutCode";
            this.txtOutCode.ReadOnly = true;
            this.txtOutCode.Size = new System.Drawing.Size(358, 111);
            this.txtOutCode.TabIndex = 2;
            this.txtOutCode.Text = "";
            // 
            // gpSetting
            // 
            this.gpSetting.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gpSetting.Font = new System.Drawing.Font("微软雅黑", 14.25F);
            this.gpSetting.Location = new System.Drawing.Point(0, 0);
            this.gpSetting.Name = "gpSetting";
            this.gpSetting.Size = new System.Drawing.Size(659, 379);
            this.gpSetting.TabIndex = 1;
            this.gpSetting.TabStop = false;
            this.gpSetting.Text = "配置";
            // 
            // pnlButton
            // 
            this.pnlButton.Controls.Add(this.gbProxy);
            this.pnlButton.Controls.Add(this.panel2);
            this.pnlButton.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlButton.Location = new System.Drawing.Point(0, 379);
            this.pnlButton.Name = "pnlButton";
            this.pnlButton.Size = new System.Drawing.Size(659, 118);
            this.pnlButton.TabIndex = 2;
            // 
            // gbProxy
            // 
            this.gbProxy.Controls.Add(this.label4);
            this.gbProxy.Controls.Add(this.txtProxyPass);
            this.gbProxy.Controls.Add(this.label3);
            this.gbProxy.Controls.Add(this.txtProxyUser);
            this.gbProxy.Controls.Add(this.label2);
            this.gbProxy.Controls.Add(this.txtProxyPort);
            this.gbProxy.Controls.Add(this.txtProxyHost);
            this.gbProxy.Controls.Add(this.label1);
            this.gbProxy.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbProxy.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.gbProxy.Location = new System.Drawing.Point(186, 0);
            this.gbProxy.Name = "gbProxy";
            this.gbProxy.Size = new System.Drawing.Size(473, 118);
            this.gbProxy.TabIndex = 4;
            this.gbProxy.TabStop = false;
            this.gbProxy.Text = "代理";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(232, 24);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(12, 20);
            this.label4.TabIndex = 11;
            this.label4.Text = ":";
            // 
            // txtProxyPass
            // 
            this.txtProxyPass.Location = new System.Drawing.Point(63, 86);
            this.txtProxyPass.Name = "txtProxyPass";
            this.txtProxyPass.Size = new System.Drawing.Size(241, 25);
            this.txtProxyPass.TabIndex = 10;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(20, 89);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(40, 20);
            this.label3.TabIndex = 9;
            this.label3.Text = "密码:";
            // 
            // txtProxyUser
            // 
            this.txtProxyUser.Location = new System.Drawing.Point(63, 55);
            this.txtProxyUser.Name = "txtProxyUser";
            this.txtProxyUser.Size = new System.Drawing.Size(241, 25);
            this.txtProxyUser.TabIndex = 8;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 58);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(54, 20);
            this.label2.TabIndex = 6;
            this.label2.Text = "用户名:";
            // 
            // txtProxyPort
            // 
            this.txtProxyPort.Location = new System.Drawing.Point(247, 24);
            this.txtProxyPort.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.txtProxyPort.Name = "txtProxyPort";
            this.txtProxyPort.Size = new System.Drawing.Size(57, 25);
            this.txtProxyPort.TabIndex = 7;
            this.txtProxyPort.Value = new decimal(new int[] {
            1080,
            0,
            0,
            0});
            // 
            // txtProxyHost
            // 
            this.txtProxyHost.Location = new System.Drawing.Point(63, 24);
            this.txtProxyHost.Name = "txtProxyHost";
            this.txtProxyHost.Size = new System.Drawing.Size(167, 25);
            this.txtProxyHost.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(20, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(40, 20);
            this.label1.TabIndex = 4;
            this.label1.Text = "地址:";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btnTest);
            this.panel2.Controls.Add(this.btnOut);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(186, 118);
            this.panel2.TabIndex = 3;
            // 
            // btnTest
            // 
            this.btnTest.Location = new System.Drawing.Point(3, 6);
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(138, 39);
            this.btnTest.TabIndex = 1;
            this.btnTest.Text = "测试";
            this.btnTest.UseVisualStyleBackColor = true;
            this.btnTest.Click += new System.EventHandler(this.btnTest_Click);
            // 
            // btnOut
            // 
            this.btnOut.Location = new System.Drawing.Point(5, 70);
            this.btnOut.Name = "btnOut";
            this.btnOut.Size = new System.Drawing.Size(138, 39);
            this.btnOut.TabIndex = 0;
            this.btnOut.Text = "输出";
            this.btnOut.UseVisualStyleBackColor = true;
            this.btnOut.Click += new System.EventHandler(this.btnOut_Click);
            // 
            // UIConnBase
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gpSetting);
            this.Controls.Add(this.pnlButton);
            this.Controls.Add(this.gpOutput);
            this.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.Margin = new System.Windows.Forms.Padding(5);
            this.Name = "UIConnBase";
            this.Size = new System.Drawing.Size(659, 640);
            this.gpOutput.ResumeLayout(false);
            this.scOut.Panel1.ResumeLayout(false);
            this.scOut.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.scOut)).EndInit();
            this.scOut.ResumeLayout(false);
            this.cmsCopy.ResumeLayout(false);
            this.pnlButton.ResumeLayout(false);
            this.gbProxy.ResumeLayout(false);
            this.gbProxy.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtProxyPort)).EndInit();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gpOutput;
        private System.Windows.Forms.SplitContainer scOut;
        public System.Windows.Forms.GroupBox gpSetting;
        private System.Windows.Forms.Panel pnlButton;
        private System.Windows.Forms.Button btnTest;
        private System.Windows.Forms.Button btnOut;
        private System.Windows.Forms.RichTextBox txtOutConn;
        private System.Windows.Forms.RichTextBox txtOutCode;
        private System.Windows.Forms.ContextMenuStrip cmsCopy;
        private System.Windows.Forms.ToolStripMenuItem tsCopy;
        private System.Windows.Forms.Button btnTech;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtProxyPass;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtProxyUser;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown txtProxyPort;
        private System.Windows.Forms.TextBox txtProxyHost;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel2;
        protected System.Windows.Forms.GroupBox gbProxy;
    }
}
