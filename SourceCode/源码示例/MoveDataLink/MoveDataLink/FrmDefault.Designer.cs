namespace MoveDataLink
{
    partial class FrmDefault
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
            this.lbPaths = new System.Windows.Forms.ListView();
            this.colSummary = new System.Windows.Forms.ColumnHeader();
            this.colPath = new System.Windows.Forms.ColumnHeader();
            this.SuspendLayout();
            // 
            // lbPaths
            // 
            this.lbPaths.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colSummary,
            this.colPath});
            this.lbPaths.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbPaths.FullRowSelect = true;
            this.lbPaths.Location = new System.Drawing.Point(0, 0);
            this.lbPaths.Name = "lbPaths";
            this.lbPaths.Size = new System.Drawing.Size(452, 520);
            this.lbPaths.TabIndex = 0;
            this.lbPaths.UseCompatibleStateImageBehavior = false;
            this.lbPaths.View = System.Windows.Forms.View.Details;
            this.lbPaths.Resize += new System.EventHandler(this.lbPaths_Resize);
            this.lbPaths.DoubleClick += new System.EventHandler(this.lbPaths_DoubleClick);
            // 
            // colSummary
            // 
            this.colSummary.Text = "说明";
            this.colSummary.Width = 150;
            // 
            // colPath
            // 
            this.colPath.Text = "路径";
            this.colPath.Width = 180;
            // 
            // FrmDefault
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(452, 520);
            this.Controls.Add(this.lbPaths);
            this.Name = "FrmDefault";
            this.Text = "选择文件夹";
            this.Load += new System.EventHandler(this.FrmDefault_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView lbPaths;
        private System.Windows.Forms.ColumnHeader colSummary;
        private System.Windows.Forms.ColumnHeader colPath;

    }
}