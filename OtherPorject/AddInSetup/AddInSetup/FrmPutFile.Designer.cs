namespace AddInSetup
{
    partial class FrmPutFile
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmPutFile));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnFile = new System.Windows.Forms.Button();
            this.txtFile = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.gvFile = new System.Windows.Forms.DataGridView();
            this.ColState = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.ColName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColVersion = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColDescription = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.fbd = new System.Windows.Forms.FolderBrowserDialog();
            this.labHelp = new System.Windows.Forms.LinkLabel();
            this.groupBox1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvFile)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.panel1);
            this.groupBox1.Controls.Add(this.btnFile);
            this.groupBox1.Controls.Add(this.txtFile);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupBox1.Location = new System.Drawing.Point(0, 427);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox1.Size = new System.Drawing.Size(659, 98);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "输出";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.labHelp);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(3, 48);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(653, 46);
            this.panel1.TabIndex = 3;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btnClose);
            this.panel2.Controls.Add(this.btnSave);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel2.Location = new System.Drawing.Point(389, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(264, 46);
            this.panel2.TabIndex = 1;
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(153, 3);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(102, 39);
            this.btnClose.TabIndex = 1;
            this.btnClose.Text = "关闭";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(3, 3);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(102, 39);
            this.btnSave.TabIndex = 0;
            this.btnSave.Text = "保存";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnFile
            // 
            this.btnFile.Location = new System.Drawing.Point(458, 13);
            this.btnFile.Name = "btnFile";
            this.btnFile.Size = new System.Drawing.Size(75, 31);
            this.btnFile.TabIndex = 2;
            this.btnFile.Text = "浏览";
            this.btnFile.UseVisualStyleBackColor = true;
            this.btnFile.Click += new System.EventHandler(this.btnFile_Click);
            // 
            // txtFile
            // 
            this.txtFile.Location = new System.Drawing.Point(76, 17);
            this.txtFile.Name = "txtFile";
            this.txtFile.Size = new System.Drawing.Size(376, 23);
            this.txtFile.TabIndex = 1;
            this.txtFile.Text = "D:\\Dll\\";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(68, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "输出目录：";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.gvFile);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(0, 0);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox2.Size = new System.Drawing.Size(659, 427);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "选择输出库";
            // 
            // gvFile
            // 
            this.gvFile.AllowUserToAddRows = false;
            this.gvFile.AllowUserToDeleteRows = false;
            this.gvFile.AllowUserToResizeColumns = false;
            this.gvFile.AllowUserToResizeRows = false;
            this.gvFile.BackgroundColor = System.Drawing.Color.White;
            this.gvFile.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gvFile.ColumnHeadersVisible = false;
            this.gvFile.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColState,
            this.ColName,
            this.ColVersion,
            this.ColDescription});
            this.gvFile.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gvFile.Location = new System.Drawing.Point(3, 20);
            this.gvFile.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.gvFile.MultiSelect = false;
            this.gvFile.Name = "gvFile";
            this.gvFile.RowHeadersVisible = false;
            this.gvFile.RowTemplate.Height = 23;
            this.gvFile.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gvFile.Size = new System.Drawing.Size(653, 403);
            this.gvFile.TabIndex = 1;
            this.gvFile.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.gvFile_CellClick);
            this.gvFile.CellMouseEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.gvFile_CellMouseEnter);
            this.gvFile.CellMouseLeave += new System.Windows.Forms.DataGridViewCellEventHandler(this.gvFile_CellMouseLeave);
            this.gvFile.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.gvFile_DataBindingComplete);
            // 
            // ColState
            // 
            this.ColState.HeaderText = "";
            this.ColState.Name = "ColState";
            this.ColState.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.ColState.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.ColState.Width = 80;
            // 
            // ColName
            // 
            this.ColName.DataPropertyName = "Name";
            this.ColName.HeaderText = "名称";
            this.ColName.Name = "ColName";
            this.ColName.ReadOnly = true;
            this.ColName.Width = 180;
            // 
            // ColVersion
            // 
            this.ColVersion.HeaderText = "版本号";
            this.ColVersion.Name = "ColVersion";
            // 
            // ColDescription
            // 
            this.ColDescription.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ColDescription.DataPropertyName = "Description";
            this.ColDescription.HeaderText = "备注名";
            this.ColDescription.Name = "ColDescription";
            // 
            // labHelp
            // 
            this.labHelp.AutoSize = true;
            this.labHelp.Location = new System.Drawing.Point(11, 14);
            this.labHelp.Name = "labHelp";
            this.labHelp.Size = new System.Drawing.Size(66, 17);
            this.labHelp.TabIndex = 2;
            this.labHelp.TabStop = true;
            this.labHelp.Text = "linkLabel1";
            this.labHelp.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.labHelp_LinkClicked);
            // 
            // FrmPutFile
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(659, 525);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "FrmPutFile";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "输出";
            this.Load += new System.EventHandler(this.FrmPutFile_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gvFile)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.DataGridView gvFile;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnFile;
        private System.Windows.Forms.TextBox txtFile;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.FolderBrowserDialog fbd;
        private System.Windows.Forms.DataGridViewCheckBoxColumn ColState;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColName;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColVersion;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColDescription;
        private System.Windows.Forms.LinkLabel labHelp;
    }
}