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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmMain));
            this.gvAddIns = new System.Windows.Forms.DataGridView();
            this.ColName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColState = new System.Windows.Forms.DataGridViewButtonColumn();
            ((System.ComponentModel.ISupportInitialize)(this.gvAddIns)).BeginInit();
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
            this.gvAddIns.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColName,
            this.ColState});
            this.gvAddIns.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gvAddIns.Location = new System.Drawing.Point(0, 0);
            this.gvAddIns.MultiSelect = false;
            this.gvAddIns.Name = "gvAddIns";
            this.gvAddIns.RowTemplate.Height = 23;
            this.gvAddIns.Size = new System.Drawing.Size(309, 363);
            this.gvAddIns.TabIndex = 0;
            this.gvAddIns.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.gvAddIns_CellContentClick);
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
            // FrmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(309, 363);
            this.Controls.Add(this.gvAddIns);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FrmMain";
            this.Text = "Buffalo插件安装助手";
            this.Load += new System.EventHandler(this.FrmMain_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gvAddIns)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView gvAddIns;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColName;
        private System.Windows.Forms.DataGridViewButtonColumn ColState;
    }
}

