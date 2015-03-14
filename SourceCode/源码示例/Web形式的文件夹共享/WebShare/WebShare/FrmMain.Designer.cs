namespace WebShare
{
    partial class FrmMain
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

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmMain));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnStop = new System.Windows.Forms.Button();
            this.btnStart = new System.Windows.Forms.Button();
            this.gpIP = new System.Windows.Forms.GroupBox();
            this.btnGetIP = new System.Windows.Forms.Button();
            this.txtInternetIP = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.btnCopyInternet = new System.Windows.Forms.Button();
            this.txturl = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnCopy = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.cmbIP = new System.Windows.Forms.ComboBox();
            this.nupPort = new System.Windows.Forms.NumericUpDown();
            this.pnlSetting = new System.Windows.Forms.Panel();
            this.gvPath = new System.Windows.Forms.DataGridView();
            this.colName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColPath = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColDelete = new System.Windows.Forms.DataGridViewButtonColumn();
            this.panel2 = new System.Windows.Forms.Panel();
            this.chkAutoStart = new System.Windows.Forms.CheckBox();
            this.btnAddShare = new System.Windows.Forms.Button();
            this.nfIco = new System.Windows.Forms.NotifyIcon(this.components);
            this.cmsIcon = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.显示窗体ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.退出ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.groupBox1.SuspendLayout();
            this.gpIP.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nupPort)).BeginInit();
            this.pnlSetting.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvPath)).BeginInit();
            this.panel2.SuspendLayout();
            this.cmsIcon.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnStop);
            this.groupBox1.Controls.Add(this.btnStart);
            this.groupBox1.Controls.Add(this.gpIP);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.cmbIP);
            this.groupBox1.Controls.Add(this.nupPort);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupBox1.Location = new System.Drawing.Point(0, 256);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(418, 140);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "基础";
            // 
            // btnStop
            // 
            this.btnStop.Location = new System.Drawing.Point(359, 18);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(55, 23);
            this.btnStop.TabIndex = 22;
            this.btnStop.Text = "停止";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(298, 18);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(55, 23);
            this.btnStart.TabIndex = 21;
            this.btnStart.Text = "开始";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // gpIP
            // 
            this.gpIP.Controls.Add(this.btnGetIP);
            this.gpIP.Controls.Add(this.txtInternetIP);
            this.gpIP.Controls.Add(this.label5);
            this.gpIP.Controls.Add(this.btnCopyInternet);
            this.gpIP.Controls.Add(this.txturl);
            this.gpIP.Controls.Add(this.label1);
            this.gpIP.Controls.Add(this.btnCopy);
            this.gpIP.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.gpIP.Location = new System.Drawing.Point(3, 46);
            this.gpIP.Name = "gpIP";
            this.gpIP.Size = new System.Drawing.Size(412, 91);
            this.gpIP.TabIndex = 20;
            this.gpIP.TabStop = false;
            this.gpIP.Text = "地址";
            // 
            // btnGetIP
            // 
            this.btnGetIP.Location = new System.Drawing.Point(300, 19);
            this.btnGetIP.Name = "btnGetIP";
            this.btnGetIP.Size = new System.Drawing.Size(50, 23);
            this.btnGetIP.TabIndex = 20;
            this.btnGetIP.Text = "获取";
            this.btnGetIP.UseVisualStyleBackColor = true;
            this.btnGetIP.Click += new System.EventHandler(this.btnGetIP_Click);
            // 
            // txtInternetIP
            // 
            this.txtInternetIP.Location = new System.Drawing.Point(93, 20);
            this.txtInternetIP.Name = "txtInternetIP";
            this.txtInternetIP.ReadOnly = true;
            this.txtInternetIP.Size = new System.Drawing.Size(204, 21);
            this.txtInternetIP.TabIndex = 18;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(18, 23);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(77, 12);
            this.label5.TabIndex = 17;
            this.label5.Text = "外网地址为：";
            // 
            // btnCopyInternet
            // 
            this.btnCopyInternet.Location = new System.Drawing.Point(352, 19);
            this.btnCopyInternet.Name = "btnCopyInternet";
            this.btnCopyInternet.Size = new System.Drawing.Size(50, 23);
            this.btnCopyInternet.TabIndex = 19;
            this.btnCopyInternet.Text = "复制";
            this.btnCopyInternet.UseVisualStyleBackColor = true;
            this.btnCopyInternet.Click += new System.EventHandler(this.btnCopyInternet_Click);
            // 
            // txturl
            // 
            this.txturl.Location = new System.Drawing.Point(93, 49);
            this.txturl.Name = "txturl";
            this.txturl.ReadOnly = true;
            this.txturl.Size = new System.Drawing.Size(204, 21);
            this.txturl.TabIndex = 6;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(18, 52);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 12);
            this.label1.TabIndex = 5;
            this.label1.Text = "内网地址为：";
            // 
            // btnCopy
            // 
            this.btnCopy.Location = new System.Drawing.Point(303, 49);
            this.btnCopy.Name = "btnCopy";
            this.btnCopy.Size = new System.Drawing.Size(75, 23);
            this.btnCopy.TabIndex = 16;
            this.btnCopy.Text = "复制";
            this.btnCopy.UseVisualStyleBackColor = true;
            this.btnCopy.Click += new System.EventHandler(this.btnCopy_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 23);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(59, 12);
            this.label4.TabIndex = 19;
            this.label4.Text = "绑定地址:";
            // 
            // cmbIP
            // 
            this.cmbIP.FormattingEnabled = true;
            this.cmbIP.Location = new System.Drawing.Point(72, 20);
            this.cmbIP.Name = "cmbIP";
            this.cmbIP.Size = new System.Drawing.Size(167, 20);
            this.cmbIP.TabIndex = 17;
            // 
            // nupPort
            // 
            this.nupPort.Location = new System.Drawing.Point(245, 19);
            this.nupPort.Maximum = new decimal(new int[] {
            -1530494977,
            232830,
            0,
            0});
            this.nupPort.Name = "nupPort";
            this.nupPort.Size = new System.Drawing.Size(47, 21);
            this.nupPort.TabIndex = 18;
            this.nupPort.Value = new decimal(new int[] {
            80,
            0,
            0,
            0});
            // 
            // pnlSetting
            // 
            this.pnlSetting.Controls.Add(this.gvPath);
            this.pnlSetting.Controls.Add(this.panel2);
            this.pnlSetting.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlSetting.Location = new System.Drawing.Point(0, 0);
            this.pnlSetting.Name = "pnlSetting";
            this.pnlSetting.Size = new System.Drawing.Size(418, 256);
            this.pnlSetting.TabIndex = 1;
            // 
            // gvPath
            // 
            this.gvPath.AllowUserToAddRows = false;
            this.gvPath.AllowUserToResizeColumns = false;
            this.gvPath.AllowUserToResizeRows = false;
            this.gvPath.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.gvPath.BackgroundColor = System.Drawing.Color.White;
            this.gvPath.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gvPath.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colName,
            this.ColPath,
            this.ColDelete});
            this.gvPath.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gvPath.GridColor = System.Drawing.Color.White;
            this.gvPath.Location = new System.Drawing.Point(0, 0);
            this.gvPath.Name = "gvPath";
            this.gvPath.RowHeadersVisible = false;
            this.gvPath.RowTemplate.Height = 23;
            this.gvPath.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gvPath.Size = new System.Drawing.Size(418, 225);
            this.gvPath.TabIndex = 1;
            this.gvPath.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.gvPath_CellContentClick);
            // 
            // colName
            // 
            this.colName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.colName.DataPropertyName = "Name";
            this.colName.FillWeight = 97.97999F;
            this.colName.HeaderText = "共享名";
            this.colName.Name = "colName";
            this.colName.ReadOnly = true;
            // 
            // ColPath
            // 
            this.ColPath.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ColPath.DataPropertyName = "Path";
            this.ColPath.FillWeight = 141.1063F;
            this.ColPath.HeaderText = "路径";
            this.ColPath.Name = "ColPath";
            this.ColPath.ReadOnly = true;
            // 
            // ColDelete
            // 
            this.ColDelete.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.ColDelete.FillWeight = 60.91371F;
            this.ColDelete.HeaderText = "";
            this.ColDelete.Name = "ColDelete";
            this.ColDelete.ReadOnly = true;
            this.ColDelete.Text = "删除";
            this.ColDelete.UseColumnTextForButtonValue = true;
            this.ColDelete.Width = 50;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.chkAutoStart);
            this.panel2.Controls.Add(this.btnAddShare);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 225);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(418, 31);
            this.panel2.TabIndex = 0;
            // 
            // chkAutoStart
            // 
            this.chkAutoStart.AutoSize = true;
            this.chkAutoStart.Location = new System.Drawing.Point(3, 9);
            this.chkAutoStart.Name = "chkAutoStart";
            this.chkAutoStart.Size = new System.Drawing.Size(72, 16);
            this.chkAutoStart.TabIndex = 1;
            this.chkAutoStart.Text = "开机启动";
            this.chkAutoStart.UseVisualStyleBackColor = true;
            this.chkAutoStart.CheckedChanged += new System.EventHandler(this.chkAutoStart_CheckedChanged);
            // 
            // btnAddShare
            // 
            this.btnAddShare.Location = new System.Drawing.Point(339, 5);
            this.btnAddShare.Name = "btnAddShare";
            this.btnAddShare.Size = new System.Drawing.Size(75, 23);
            this.btnAddShare.TabIndex = 0;
            this.btnAddShare.Text = "添加共享";
            this.btnAddShare.UseVisualStyleBackColor = true;
            this.btnAddShare.Click += new System.EventHandler(this.btnAddShare_Click);
            // 
            // nfIco
            // 
            this.nfIco.ContextMenuStrip = this.cmsIcon;
            this.nfIco.Icon = ((System.Drawing.Icon)(resources.GetObject("nfIco.Icon")));
            this.nfIco.Text = "ShareServer";
            this.nfIco.Visible = true;
            this.nfIco.DoubleClick += new System.EventHandler(this.nfIco_DoubleClick);
            // 
            // cmsIcon
            // 
            this.cmsIcon.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.显示窗体ToolStripMenuItem,
            this.toolStripMenuItem2,
            this.退出ToolStripMenuItem});
            this.cmsIcon.Name = "cmsIcon";
            this.cmsIcon.Size = new System.Drawing.Size(125, 54);
            // 
            // 显示窗体ToolStripMenuItem
            // 
            this.显示窗体ToolStripMenuItem.Name = "显示窗体ToolStripMenuItem";
            this.显示窗体ToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.显示窗体ToolStripMenuItem.Text = "显示窗体";
            this.显示窗体ToolStripMenuItem.Click += new System.EventHandler(this.显示窗体ToolStripMenuItem_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(121, 6);
            // 
            // 退出ToolStripMenuItem
            // 
            this.退出ToolStripMenuItem.Name = "退出ToolStripMenuItem";
            this.退出ToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.退出ToolStripMenuItem.Text = "退出";
            this.退出ToolStripMenuItem.Click += new System.EventHandler(this.退出ToolStripMenuItem_Click);
            // 
            // FrmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(418, 396);
            this.Controls.Add(this.pnlSetting);
            this.Controls.Add(this.groupBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "FrmMain";
            this.Text = "网络共享";
            this.Load += new System.EventHandler(this.FrmMain_Load);
            this.SizeChanged += new System.EventHandler(this.FrmMain_SizeChanged);
            this.Shown += new System.EventHandler(this.FrmMain_Shown);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmMain_FormClosing);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.gpIP.ResumeLayout(false);
            this.gpIP.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nupPort)).EndInit();
            this.pnlSetting.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gvPath)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.cmsIcon.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cmbIP;
        private System.Windows.Forms.NumericUpDown nupPort;
        private System.Windows.Forms.GroupBox gpIP;
        private System.Windows.Forms.Button btnGetIP;
        private System.Windows.Forms.TextBox txtInternetIP;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnCopyInternet;
        private System.Windows.Forms.TextBox txturl;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnCopy;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Panel pnlSetting;
        private System.Windows.Forms.DataGridView gvPath;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnAddShare;
        private System.Windows.Forms.DataGridViewTextBoxColumn colName;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColPath;
        private System.Windows.Forms.DataGridViewButtonColumn ColDelete;
        private System.Windows.Forms.NotifyIcon nfIco;
        private System.Windows.Forms.CheckBox chkAutoStart;
        private System.Windows.Forms.ContextMenuStrip cmsIcon;
        private System.Windows.Forms.ToolStripMenuItem 显示窗体ToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem 退出ToolStripMenuItem;
    }
}

