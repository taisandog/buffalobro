namespace Buffalo.WinFormsControl.Editors
{
    partial class ComboBoxEditor
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
            this.pnlLable = new System.Windows.Forms.Panel();
            this.labSummary = new System.Windows.Forms.Label();
            this.pnlValue = new System.Windows.Forms.Panel();
            this.cmbValue = new System.Windows.Forms.ComboBox();
            this.pnlLable.SuspendLayout();
            this.pnlValue.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlLable
            // 
            this.pnlLable.Controls.Add(this.labSummary);
            this.pnlLable.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlLable.Location = new System.Drawing.Point(0, 0);
            this.pnlLable.Name = "pnlLable";
            this.pnlLable.Size = new System.Drawing.Size(50, 25);
            this.pnlLable.TabIndex = 0;
            // 
            // labSummary
            // 
            this.labSummary.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labSummary.Location = new System.Drawing.Point(0, 0);
            this.labSummary.Name = "labSummary";
            this.labSummary.Size = new System.Drawing.Size(50, 25);
            this.labSummary.TabIndex = 0;
            this.labSummary.Text = "标签";
            this.labSummary.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // pnlValue
            // 
            this.pnlValue.Controls.Add(this.cmbValue);
            this.pnlValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlValue.Location = new System.Drawing.Point(50, 0);
            this.pnlValue.Name = "pnlValue";
            this.pnlValue.Size = new System.Drawing.Size(150, 25);
            this.pnlValue.TabIndex = 1;
            this.pnlValue.Resize += new System.EventHandler(this.pnlValue_Resize);
            // 
            // cmbValue
            // 
            this.cmbValue.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbValue.FormattingEnabled = true;
            this.cmbValue.Location = new System.Drawing.Point(3, 2);
            this.cmbValue.Name = "cmbValue";
            this.cmbValue.Size = new System.Drawing.Size(144, 20);
            this.cmbValue.TabIndex = 0;
            this.cmbValue.SelectedIndexChanged += new System.EventHandler(this.cmbValue_SelectedIndexChanged);
            // 
            // ComboBoxEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnlValue);
            this.Controls.Add(this.pnlLable);
            this.MaximumSize = new System.Drawing.Size(500, 25);
            this.MinimumSize = new System.Drawing.Size(200, 25);
            this.Name = "ComboBoxEditor";
            this.Size = new System.Drawing.Size(200, 25);
            this.pnlLable.ResumeLayout(false);
            this.pnlValue.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlLable;
        private System.Windows.Forms.Label labSummary;
        private System.Windows.Forms.Panel pnlValue;
        private System.Windows.Forms.ComboBox cmbValue;
    }
}
