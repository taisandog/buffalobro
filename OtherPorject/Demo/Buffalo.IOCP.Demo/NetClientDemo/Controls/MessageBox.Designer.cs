namespace Library
{
    partial class MessageBox
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
            this.components = new System.ComponentModel.Container();
            this.Cbx_Warning = new System.Windows.Forms.CheckBox();
            this.Cbx_Error = new System.Windows.Forms.CheckBox();
            this.Cbx_Normal = new System.Windows.Forms.CheckBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnClear = new System.Windows.Forms.Button();
            this.nupMax = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.dgDisplay = new System.Windows.Forms.DataGridView();
            this.cmCopy = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.ColType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColItems = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nupMax)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgDisplay)).BeginInit();
            this.cmCopy.SuspendLayout();
            this.SuspendLayout();
            // 
            // Cbx_Warning
            // 
            this.Cbx_Warning.AutoSize = true;
            this.Cbx_Warning.Checked = true;
            this.Cbx_Warning.CheckState = System.Windows.Forms.CheckState.Checked;
            this.Cbx_Warning.Location = new System.Drawing.Point(106, 8);
            this.Cbx_Warning.Name = "Cbx_Warning";
            this.Cbx_Warning.Size = new System.Drawing.Size(96, 20);
            this.Cbx_Warning.TabIndex = 15;
            this.Cbx_Warning.Text = "警告日志";
            this.Cbx_Warning.UseVisualStyleBackColor = true;
            // 
            // Cbx_Error
            // 
            this.Cbx_Error.AutoSize = true;
            this.Cbx_Error.Checked = true;
            this.Cbx_Error.CheckState = System.Windows.Forms.CheckState.Checked;
            this.Cbx_Error.Location = new System.Drawing.Point(209, 8);
            this.Cbx_Error.Name = "Cbx_Error";
            this.Cbx_Error.Size = new System.Drawing.Size(96, 20);
            this.Cbx_Error.TabIndex = 14;
            this.Cbx_Error.Text = "错误日志";
            this.Cbx_Error.UseVisualStyleBackColor = true;
            // 
            // Cbx_Normal
            // 
            this.Cbx_Normal.AutoSize = true;
            this.Cbx_Normal.Checked = true;
            this.Cbx_Normal.CheckState = System.Windows.Forms.CheckState.Checked;
            this.Cbx_Normal.Location = new System.Drawing.Point(3, 8);
            this.Cbx_Normal.Name = "Cbx_Normal";
            this.Cbx_Normal.Size = new System.Drawing.Size(96, 20);
            this.Cbx_Normal.TabIndex = 13;
            this.Cbx_Normal.Text = "普通日志";
            this.Cbx_Normal.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Controls.Add(this.Cbx_Normal);
            this.panel1.Controls.Add(this.Cbx_Warning);
            this.panel1.Controls.Add(this.Cbx_Error);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Font = new System.Drawing.Font("宋体", 10F);
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(611, 37);
            this.panel1.TabIndex = 16;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btnClear);
            this.panel2.Controls.Add(this.nupMax);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel2.Location = new System.Drawing.Point(322, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(289, 37);
            this.panel2.TabIndex = 18;
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(3, 3);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(58, 27);
            this.btnClear.TabIndex = 26;
            this.btnClear.Text = "清空";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // nupMax
            // 
            this.nupMax.Location = new System.Drawing.Point(146, 3);
            this.nupMax.Maximum = new decimal(new int[] {
            9999999,
            0,
            0,
            0});
            this.nupMax.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nupMax.Name = "nupMax";
            this.nupMax.Size = new System.Drawing.Size(140, 23);
            this.nupMax.TabIndex = 24;
            this.nupMax.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(69, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(63, 14);
            this.label2.TabIndex = 25;
            this.label2.Text = "最多条数";
            // 
            // dgDisplay
            // 
            this.dgDisplay.AllowUserToAddRows = false;
            this.dgDisplay.AllowUserToDeleteRows = false;
            this.dgDisplay.AllowUserToResizeColumns = false;
            this.dgDisplay.AllowUserToResizeRows = false;
            this.dgDisplay.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgDisplay.ColumnHeadersVisible = false;
            this.dgDisplay.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColType,
            this.ColItems,
            this.ColDate});
            this.dgDisplay.ContextMenuStrip = this.cmCopy;
            this.dgDisplay.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgDisplay.Location = new System.Drawing.Point(0, 37);
            this.dgDisplay.MultiSelect = false;
            this.dgDisplay.Name = "dgDisplay";
            this.dgDisplay.ReadOnly = true;
            this.dgDisplay.RowHeadersVisible = false;
            this.dgDisplay.RowTemplate.Height = 23;
            this.dgDisplay.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgDisplay.Size = new System.Drawing.Size(611, 296);
            this.dgDisplay.TabIndex = 17;
            this.dgDisplay.CellMouseEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgDisplay_CellMouseEnter);
            // 
            // cmCopy
            // 
            this.cmCopy.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsCopy});
            this.cmCopy.Name = "cmCopy";
            this.cmCopy.Size = new System.Drawing.Size(101, 26);
            // 
            // tsCopy
            // 
            this.tsCopy.Name = "tsCopy";
            this.tsCopy.Size = new System.Drawing.Size(100, 22);
            this.tsCopy.Text = "复制";
            this.tsCopy.Click += new System.EventHandler(this.tsCopy_Click);
            // 
            // ColType
            // 
            this.ColType.HeaderText = "类型";
            this.ColType.Name = "ColType";
            this.ColType.ReadOnly = true;
            this.ColType.Width = 50;
            // 
            // ColItems
            // 
            this.ColItems.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ColItems.HeaderText = "消息";
            this.ColItems.Name = "ColItems";
            this.ColItems.ReadOnly = true;
            // 
            // ColDate
            // 
            this.ColDate.HeaderText = "时间";
            this.ColDate.Name = "ColDate";
            this.ColDate.ReadOnly = true;
            this.ColDate.Width = 200;
            // 
            // MessageBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dgDisplay);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("宋体", 10F);
            this.Name = "MessageBox";
            this.Size = new System.Drawing.Size(611, 333);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nupMax)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgDisplay)).EndInit();
            this.cmCopy.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.CheckBox Cbx_Warning;
        private System.Windows.Forms.CheckBox Cbx_Error;
        private System.Windows.Forms.CheckBox Cbx_Normal;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.DataGridView dgDisplay;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.NumericUpDown nupMax;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ContextMenuStrip cmCopy;
        private System.Windows.Forms.ToolStripMenuItem tsCopy;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColType;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColItems;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColDate;
    }
}
