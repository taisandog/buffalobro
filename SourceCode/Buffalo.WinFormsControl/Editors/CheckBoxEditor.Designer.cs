namespace Buffalo.WinFormsControl.Editors
{
    partial class CheckBoxEditor
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
            this.chkValue = new Buffalo.WinFormsControl.Editors.OnOffCheckBox();
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
            this.pnlLable.Size = new System.Drawing.Size(102, 25);
            this.pnlLable.TabIndex = 0;
            // 
            // labSummary
            // 
            this.labSummary.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labSummary.Location = new System.Drawing.Point(0, 0);
            this.labSummary.Name = "labSummary";
            this.labSummary.Size = new System.Drawing.Size(102, 25);
            this.labSummary.TabIndex = 0;
            this.labSummary.Text = "标签";
            this.labSummary.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // pnlValue
            // 
            this.pnlValue.Controls.Add(this.chkValue);
            this.pnlValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlValue.Location = new System.Drawing.Point(102, 0);
            this.pnlValue.Name = "pnlValue";
            this.pnlValue.Size = new System.Drawing.Size(58, 25);
            this.pnlValue.TabIndex = 1;
            // 
            // chkValue
            // 
            this.chkValue.CheckState = System.Windows.Forms.CheckState.Unchecked;
            this.chkValue.Location = new System.Drawing.Point(4, 1);
            this.chkValue.MaximumSize = new System.Drawing.Size(51, 23);
            this.chkValue.MinimumSize = new System.Drawing.Size(51, 23);
            this.chkValue.Name = "chkValue";
            this.chkValue.OnOffType = Buffalo.WinFormsControl.Editors.OnOffButtonType.Quadrate;
            this.chkValue.Size = new System.Drawing.Size(51, 23);
            this.chkValue.TabIndex = 0;
            this.chkValue.CheckStateChanged += new System.EventHandler(this.chkValue_CheckStateChanged);
            // 
            // CheckBoxEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnlValue);
            this.Controls.Add(this.pnlLable);
            this.MaximumSize = new System.Drawing.Size(500, 25);
            this.MinimumSize = new System.Drawing.Size(100, 25);
            this.Name = "CheckBoxEditor";
            this.Size = new System.Drawing.Size(160, 25);
            this.Load += new System.EventHandler(this.CheckBoxEditor_Load);
            this.pnlLable.ResumeLayout(false);
            this.pnlValue.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlLable;
        private System.Windows.Forms.Label labSummary;
        private System.Windows.Forms.Panel pnlValue;
        private OnOffCheckBox chkValue;
    }
}
