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
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnTest = new System.Windows.Forms.Button();
            this.btnOut = new System.Windows.Forms.Button();
            this.gpOutput.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.scOut)).BeginInit();
            this.scOut.Panel1.SuspendLayout();
            this.scOut.Panel2.SuspendLayout();
            this.scOut.SuspendLayout();
            this.cmsCopy.SuspendLayout();
            this.pnlButton.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // gpOutput
            // 
            this.gpOutput.Controls.Add(this.btnTech);
            this.gpOutput.Controls.Add(this.scOut);
            this.gpOutput.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.gpOutput.Location = new System.Drawing.Point(0, 486);
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
            this.gpSetting.Size = new System.Drawing.Size(659, 429);
            this.gpSetting.TabIndex = 1;
            this.gpSetting.TabStop = false;
            this.gpSetting.Text = "配置";
            // 
            // pnlButton
            // 
            this.pnlButton.Controls.Add(this.panel1);
            this.pnlButton.Controls.Add(this.btnOut);
            this.pnlButton.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlButton.Location = new System.Drawing.Point(0, 429);
            this.pnlButton.Name = "pnlButton";
            this.pnlButton.Size = new System.Drawing.Size(659, 57);
            this.pnlButton.TabIndex = 2;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnTest);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel1.Location = new System.Drawing.Point(510, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(149, 57);
            this.panel1.TabIndex = 2;
            // 
            // btnTest
            // 
            this.btnTest.Location = new System.Drawing.Point(3, 13);
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(138, 39);
            this.btnTest.TabIndex = 1;
            this.btnTest.Text = "测试";
            this.btnTest.UseVisualStyleBackColor = true;
            this.btnTest.Click += new System.EventHandler(this.btnTest_Click);
            // 
            // btnOut
            // 
            this.btnOut.Location = new System.Drawing.Point(5, 13);
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
            this.Size = new System.Drawing.Size(659, 629);
            this.gpOutput.ResumeLayout(false);
            this.scOut.Panel1.ResumeLayout(false);
            this.scOut.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.scOut)).EndInit();
            this.scOut.ResumeLayout(false);
            this.cmsCopy.ResumeLayout(false);
            this.pnlButton.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gpOutput;
        private System.Windows.Forms.SplitContainer scOut;
        public System.Windows.Forms.GroupBox gpSetting;
        private System.Windows.Forms.Panel pnlButton;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnTest;
        private System.Windows.Forms.Button btnOut;
        private System.Windows.Forms.RichTextBox txtOutConn;
        private System.Windows.Forms.RichTextBox txtOutCode;
        private System.Windows.Forms.ContextMenuStrip cmsCopy;
        private System.Windows.Forms.ToolStripMenuItem tsCopy;
        private System.Windows.Forms.Button btnTech;
    }
}
