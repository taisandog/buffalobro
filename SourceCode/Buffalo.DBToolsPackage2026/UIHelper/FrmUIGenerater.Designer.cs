namespace Buffalo.DBTools.UIHelper
{
    partial class FrmUIGenerater
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmUIGenerater));
            this.panel1 = new System.Windows.Forms.Panel();
            this.cmbProjects = new Buffalo.WinFormsControl.Editors.ComboBoxEditor();
            this.cmbModels = new Buffalo.WinFormsControl.Editors.ComboBoxEditor();
            this.btnReFreash = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.btnGen = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.tabPanel = new System.Windows.Forms.TableLayoutPanel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.gvMember = new System.Windows.Forms.DataGridView();
            this.ColCheck = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.ColName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel5 = new System.Windows.Forms.Panel();
            this.btnUp = new System.Windows.Forms.Button();
            this.btnDown = new System.Windows.Forms.Button();
            this.panel6 = new System.Windows.Forms.Panel();
            this.pnlClassConfig = new System.Windows.Forms.FlowLayoutPanel();
            this.dataGridViewCheckBoxColumn1 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvMember)).BeginInit();
            this.panel5.SuspendLayout();
            this.panel6.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.cmbProjects);
            this.panel1.Controls.Add(this.cmbModels);
            this.panel1.Controls.Add(this.btnReFreash);
            this.panel1.Controls.Add(this.button2);
            this.panel1.Controls.Add(this.btnGen);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 641);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(884, 71);
            this.panel1.TabIndex = 0;
            // 
            // cmbProjects
            // 
            this.cmbProjects.BindPropertyName = null;
            this.cmbProjects.LableFont = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cmbProjects.LableForeColor = System.Drawing.SystemColors.WindowText;
            this.cmbProjects.LableText = "输出代码:";
            this.cmbProjects.LableWidth = 80;
            this.cmbProjects.Location = new System.Drawing.Point(12, 43);
            this.cmbProjects.MaximumSize = new System.Drawing.Size(500, 25);
            this.cmbProjects.MinimumSize = new System.Drawing.Size(200, 25);
            this.cmbProjects.Name = "cmbProjects";
            this.cmbProjects.Size = new System.Drawing.Size(221, 25);
            this.cmbProjects.TabIndex = 5;
            this.cmbProjects.Value = null;
            // 
            // cmbModels
            // 
            this.cmbModels.BindPropertyName = null;
            this.cmbModels.LableFont = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cmbModels.LableForeColor = System.Drawing.SystemColors.WindowText;
            this.cmbModels.LableText = "方案:";
            this.cmbModels.LableWidth = 80;
            this.cmbModels.Location = new System.Drawing.Point(12, 12);
            this.cmbModels.MaximumSize = new System.Drawing.Size(500, 25);
            this.cmbModels.MinimumSize = new System.Drawing.Size(200, 25);
            this.cmbModels.Name = "cmbModels";
            this.cmbModels.Size = new System.Drawing.Size(221, 25);
            this.cmbModels.TabIndex = 5;
            this.cmbModels.Value = null;
            // 
            // btnReFreash
            // 
            this.btnReFreash.Location = new System.Drawing.Point(239, 12);
            this.btnReFreash.Name = "btnReFreash";
            this.btnReFreash.Size = new System.Drawing.Size(56, 23);
            this.btnReFreash.TabIndex = 4;
            this.btnReFreash.Text = "重加载";
            this.btnReFreash.UseVisualStyleBackColor = true;
            this.btnReFreash.Click += new System.EventHandler(this.btnReFreash_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(601, 23);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 36);
            this.button2.TabIndex = 1;
            this.button2.Text = "关闭";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // btnGen
            // 
            this.btnGen.Location = new System.Drawing.Point(520, 23);
            this.btnGen.Name = "btnGen";
            this.btnGen.Size = new System.Drawing.Size(75, 36);
            this.btnGen.TabIndex = 0;
            this.btnGen.Text = "生成";
            this.btnGen.UseVisualStyleBackColor = true;
            this.btnGen.Click += new System.EventHandler(this.btnGen_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.panel4);
            this.panel2.Controls.Add(this.panel3);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 111);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(884, 530);
            this.panel2.TabIndex = 1;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.tabPanel);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(184, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(700, 530);
            this.panel4.TabIndex = 1;
            // 
            // tabPanel
            // 
            this.tabPanel.ColumnCount = 2;
            this.tabPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tabPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tabPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabPanel.Location = new System.Drawing.Point(0, 0);
            this.tabPanel.Name = "tabPanel";
            this.tabPanel.RowCount = 2;
            this.tabPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 6.338028F));
            this.tabPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 93.66197F));
            this.tabPanel.Size = new System.Drawing.Size(700, 530);
            this.tabPanel.TabIndex = 0;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.gvMember);
            this.panel3.Controls.Add(this.panel5);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(184, 530);
            this.panel3.TabIndex = 0;
            // 
            // gvMember
            // 
            this.gvMember.AllowUserToAddRows = false;
            this.gvMember.AllowUserToDeleteRows = false;
            this.gvMember.AllowUserToResizeColumns = false;
            this.gvMember.AllowUserToResizeRows = false;
            this.gvMember.BackgroundColor = System.Drawing.Color.White;
            this.gvMember.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gvMember.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColCheck,
            this.ColName});
            this.gvMember.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gvMember.Location = new System.Drawing.Point(0, 0);
            this.gvMember.MultiSelect = false;
            this.gvMember.Name = "gvMember";
            this.gvMember.RowHeadersVisible = false;
            this.gvMember.RowTemplate.Height = 23;
            this.gvMember.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gvMember.Size = new System.Drawing.Size(184, 498);
            this.gvMember.TabIndex = 0;
            this.gvMember.CurrentCellChanged += new System.EventHandler(this.gvMember_CurrentCellChanged);
            // 
            // ColCheck
            // 
            this.ColCheck.DataPropertyName = "IsGenerate";
            this.ColCheck.HeaderText = "";
            this.ColCheck.Name = "ColCheck";
            this.ColCheck.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.ColCheck.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.ColCheck.Width = 50;
            // 
            // ColName
            // 
            this.ColName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ColName.DataPropertyName = "PropertyName";
            this.ColName.HeaderText = "属性";
            this.ColName.Name = "ColName";
            this.ColName.ReadOnly = true;
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.btnUp);
            this.panel5.Controls.Add(this.btnDown);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel5.Location = new System.Drawing.Point(0, 498);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(184, 32);
            this.panel5.TabIndex = 1;
            // 
            // btnUp
            // 
            this.btnUp.Location = new System.Drawing.Point(64, 5);
            this.btnUp.Name = "btnUp";
            this.btnUp.Size = new System.Drawing.Size(24, 24);
            this.btnUp.TabIndex = 1;
            this.btnUp.Text = "↑";
            this.btnUp.UseVisualStyleBackColor = true;
            this.btnUp.Click += new System.EventHandler(this.btnUp_Click);
            // 
            // btnDown
            // 
            this.btnDown.Location = new System.Drawing.Point(103, 5);
            this.btnDown.Name = "btnDown";
            this.btnDown.Size = new System.Drawing.Size(24, 24);
            this.btnDown.TabIndex = 0;
            this.btnDown.Text = "↓";
            this.btnDown.UseVisualStyleBackColor = true;
            this.btnDown.Click += new System.EventHandler(this.btnDown_Click);
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.pnlClassConfig);
            this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel6.Location = new System.Drawing.Point(0, 0);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(884, 111);
            this.panel6.TabIndex = 2;
            // 
            // pnlClassConfig
            // 
            this.pnlClassConfig.AutoScroll = true;
            this.pnlClassConfig.BackColor = System.Drawing.Color.Transparent;
            this.pnlClassConfig.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlClassConfig.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlClassConfig.Location = new System.Drawing.Point(0, 0);
            this.pnlClassConfig.Name = "pnlClassConfig";
            this.pnlClassConfig.Size = new System.Drawing.Size(884, 111);
            this.pnlClassConfig.TabIndex = 0;
            // 
            // dataGridViewCheckBoxColumn1
            // 
            this.dataGridViewCheckBoxColumn1.DataPropertyName = "IsGenerate";
            this.dataGridViewCheckBoxColumn1.HeaderText = "";
            this.dataGridViewCheckBoxColumn1.Name = "dataGridViewCheckBoxColumn1";
            this.dataGridViewCheckBoxColumn1.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewCheckBoxColumn1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.dataGridViewCheckBoxColumn1.Width = 50;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn1.DataPropertyName = "PropertyName";
            this.dataGridViewTextBoxColumn1.HeaderText = "";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn2.HeaderText = "属性";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            // 
            // FrmUIGenerater
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(884, 712);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel6);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FrmUIGenerater";
            this.Text = "界面生成";
            this.Load += new System.EventHandler(this.FrmUIGenerater_Load);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gvMember)).EndInit();
            this.panel5.ResumeLayout(false);
            this.panel6.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.DataGridView gvMember;
        private System.Windows.Forms.TableLayoutPanel tabPanel;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewCheckBoxColumn ColCheck;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColName;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button btnGen;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.FlowLayoutPanel pnlClassConfig;
        private Buffalo.WinFormsControl.Editors.ComboBoxEditor cmbProjects;
        private System.Windows.Forms.Button btnReFreash;
        private Buffalo.WinFormsControl.Editors.ComboBoxEditor cmbModels;
        private System.Windows.Forms.DataGridViewCheckBoxColumn dataGridViewCheckBoxColumn1;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Button btnUp;
        private System.Windows.Forms.Button btnDown;


    }
}