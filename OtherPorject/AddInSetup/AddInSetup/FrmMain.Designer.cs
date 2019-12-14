namespace AddInSetup
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
            this.gvAddIns = new System.Windows.Forms.DataGridView();
            this.ColName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColState = new System.Windows.Forms.DataGridViewButtonColumn();
            this.gpAddIn = new System.Windows.Forms.GroupBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.gvDllVer = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColPut = new System.Windows.Forms.DataGridViewButtonColumn();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.tsFile = new System.Windows.Forms.ToolStripMenuItem();
            this.tsODAC = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsExit = new System.Windows.Forms.ToolStripMenuItem();
            this.tsHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.tsIndex = new System.Windows.Forms.ToolStripMenuItem();
            this.tsConn = new System.Windows.Forms.ToolStripMenuItem();
            this.tsVedio = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.tsNew = new System.Windows.Forms.ToolStripMenuItem();
            this.timCheck = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.gvAddIns)).BeginInit();
            this.gpAddIn.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvDllVer)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // gvAddIns
            // 
            this.gvAddIns.AllowUserToAddRows = false;
            this.gvAddIns.AllowUserToDeleteRows = false;
            this.gvAddIns.AllowUserToResizeColumns = false;
            this.gvAddIns.AllowUserToResizeRows = false;
            this.gvAddIns.BackgroundColor = System.Drawing.Color.White;
            this.gvAddIns.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gvAddIns.ColumnHeadersVisible = false;
            this.gvAddIns.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColName,
            this.ColState});
            this.gvAddIns.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gvAddIns.Location = new System.Drawing.Point(4, 23);
            this.gvAddIns.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.gvAddIns.MultiSelect = false;
            this.gvAddIns.Name = "gvAddIns";
            this.gvAddIns.RowHeadersVisible = false;
            this.gvAddIns.RowTemplate.Height = 23;
            this.gvAddIns.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gvAddIns.Size = new System.Drawing.Size(583, 235);
            this.gvAddIns.TabIndex = 0;
            this.gvAddIns.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.gvAddIns_CellContentClick);
            this.gvAddIns.CellMouseEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.gvAddIns_CellMouseEnter);
            this.gvAddIns.MouseLeave += new System.EventHandler(this.gvAddIns_MouseLeave);
            // 
            // ColName
            // 
            this.ColName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ColName.DataPropertyName = "Name";
            this.ColName.HeaderText = "开发工具";
            this.ColName.Name = "ColName";
            this.ColName.ReadOnly = true;
            // 
            // ColState
            // 
            this.ColState.DataPropertyName = "ButtonText";
            this.ColState.HeaderText = "";
            this.ColState.Name = "ColState";
            this.ColState.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.ColState.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.ColState.Width = 80;
            // 
            // gpAddIn
            // 
            this.gpAddIn.Controls.Add(this.gvAddIns);
            this.gpAddIn.Dock = System.Windows.Forms.DockStyle.Top;
            this.gpAddIn.Location = new System.Drawing.Point(0, 25);
            this.gpAddIn.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.gpAddIn.Name = "gpAddIn";
            this.gpAddIn.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.gpAddIn.Size = new System.Drawing.Size(591, 263);
            this.gpAddIn.TabIndex = 1;
            this.gpAddIn.TabStop = false;
            this.gpAddIn.Text = "插件安装";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.gvDllVer);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 288);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox1.Size = new System.Drawing.Size(591, 232);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "输出类库";
            // 
            // gvDllVer
            // 
            this.gvDllVer.AllowUserToAddRows = false;
            this.gvDllVer.AllowUserToDeleteRows = false;
            this.gvDllVer.AllowUserToResizeColumns = false;
            this.gvDllVer.AllowUserToResizeRows = false;
            this.gvDllVer.BackgroundColor = System.Drawing.Color.White;
            this.gvDllVer.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gvDllVer.ColumnHeadersVisible = false;
            this.gvDllVer.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.ColPut});
            this.gvDllVer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gvDllVer.Location = new System.Drawing.Point(4, 23);
            this.gvDllVer.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.gvDllVer.MultiSelect = false;
            this.gvDllVer.Name = "gvDllVer";
            this.gvDllVer.RowHeadersVisible = false;
            this.gvDllVer.RowTemplate.Height = 23;
            this.gvDllVer.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gvDllVer.Size = new System.Drawing.Size(583, 204);
            this.gvDllVer.TabIndex = 0;
            this.gvDllVer.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.gvDllVer_CellContentClick);
            this.gvDllVer.CellMouseEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.gvDllVer_CellMouseEnter);
            this.gvDllVer.MouseLeave += new System.EventHandler(this.gvDllVer_MouseLeave);
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn1.DataPropertyName = "VerName";
            this.dataGridViewTextBoxColumn1.HeaderText = ".Net版本";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            // 
            // ColPut
            // 
            this.ColPut.HeaderText = "";
            this.ColPut.Name = "ColPut";
            this.ColPut.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.ColPut.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.ColPut.Text = "输出";
            this.ColPut.UseColumnTextForButtonValue = true;
            this.ColPut.Width = 80;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsFile,
            this.tsHelp,
            this.tsNew});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(591, 25);
            this.menuStrip1.TabIndex = 4;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // tsFile
            // 
            this.tsFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsODAC,
            this.toolStripMenuItem1,
            this.tsExit});
            this.tsFile.Name = "tsFile";
            this.tsFile.Size = new System.Drawing.Size(58, 21);
            this.tsFile.Text = "文件(&F)";
            // 
            // tsODAC
            // 
            this.tsODAC.Name = "tsODAC";
            this.tsODAC.Size = new System.Drawing.Size(153, 22);
            this.tsODAC.Text = "ODAC下载(&O)";
            this.tsODAC.Click += new System.EventHandler(this.tsODAC_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(150, 6);
            // 
            // tsExit
            // 
            this.tsExit.Name = "tsExit";
            this.tsExit.Size = new System.Drawing.Size(153, 22);
            this.tsExit.Text = "退出(&X)";
            this.tsExit.Click += new System.EventHandler(this.tsExit_Click);
            // 
            // tsHelp
            // 
            this.tsHelp.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsIndex,
            this.tsConn,
            this.tsVedio,
            this.toolStripMenuItem2});
            this.tsHelp.Name = "tsHelp";
            this.tsHelp.Size = new System.Drawing.Size(61, 21);
            this.tsHelp.Text = "帮助(&H)";
            // 
            // tsIndex
            // 
            this.tsIndex.Name = "tsIndex";
            this.tsIndex.Size = new System.Drawing.Size(180, 22);
            this.tsIndex.Text = "Buffalo主页";
            this.tsIndex.Click += new System.EventHandler(this.tsIndex_Click);
            // 
            // tsConn
            // 
            this.tsConn.Name = "tsConn";
            this.tsConn.Size = new System.Drawing.Size(180, 22);
            this.tsConn.Text = "连接字符串";
            this.tsConn.Click += new System.EventHandler(this.tsConn_Click);
            // 
            // tsVedio
            // 
            this.tsVedio.Name = "tsVedio";
            this.tsVedio.Size = new System.Drawing.Size(180, 22);
            this.tsVedio.Text = "使用视频";
            this.tsVedio.Click += new System.EventHandler(this.tsVedio_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(177, 6);
            // 
            // tsNew
            // 
            this.tsNew.ForeColor = System.Drawing.Color.Red;
            this.tsNew.Name = "tsNew";
            this.tsNew.Size = new System.Drawing.Size(68, 21);
            this.tsNew.Text = "有新版本";
            this.tsNew.Click += new System.EventHandler(this.tsNew_Click);
            // 
            // timCheck
            // 
            this.timCheck.Interval = 1000;
            this.timCheck.Tick += new System.EventHandler(this.timCheck_Tick);
            // 
            // FrmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(591, 520);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.gpAddIn);
            this.Controls.Add(this.menuStrip1);
            this.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "FrmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Buffalo插件安装助手";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmMain_FormClosing);
            this.Load += new System.EventHandler(this.FrmMain_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gvAddIns)).EndInit();
            this.gpAddIn.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gvDllVer)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView gvAddIns;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColName;
        private System.Windows.Forms.DataGridViewButtonColumn ColState;
        private System.Windows.Forms.GroupBox gpAddIn;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DataGridView gvDllVer;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewButtonColumn ColPut;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem tsFile;
        private System.Windows.Forms.ToolStripMenuItem tsODAC;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem tsExit;
        private System.Windows.Forms.ToolStripMenuItem tsHelp;
        private System.Windows.Forms.ToolStripMenuItem tsIndex;
        private System.Windows.Forms.ToolStripMenuItem tsVedio;
        private System.Windows.Forms.ToolStripMenuItem tsNew;
        private System.Windows.Forms.Timer timCheck;
        private System.Windows.Forms.ToolStripMenuItem tsConn;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
    }
}

