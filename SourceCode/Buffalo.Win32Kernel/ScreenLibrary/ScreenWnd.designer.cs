namespace Buffalo.Win32Kernel.ScreenLibrary
{
    partial class ScreenWnd
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.smi_Cancel = new System.Windows.Forms.ToolStripMenuItem();
            this.smi_OK = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.smi_Cancel,
            this.smi_OK});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(99, 48);
            this.contextMenuStrip1.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip1_Opening);
            // 
            // smi_Cancel
            // 
            this.smi_Cancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(238)))), ((int)(((byte)(255)))));
            this.smi_Cancel.Image = Resource.Cancel;
            this.smi_Cancel.Name = "smi_Cancel";
            this.smi_Cancel.Size = new System.Drawing.Size(98, 22);
            this.smi_Cancel.Text = "取消";
            this.smi_Cancel.Click += new System.EventHandler(this.smi_Cancel_Click);
            // 
            // smi_OK
            // 
            this.smi_OK.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(238)))), ((int)(((byte)(255)))));
            this.smi_OK.Image = Resource.OK;
            this.smi_OK.Name = "smi_OK";
            this.smi_OK.Size = new System.Drawing.Size(98, 22);
            this.smi_OK.Text = "完成";
            this.smi_OK.Click += new System.EventHandler(this.smi_OK_Click);
            // 
            // ScreenWnd
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.ContextMenuStrip = this.contextMenuStrip1;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.KeyPreview = true;
            this.Name = "ScreenWnd";
            this.Text = "ScreenWnd";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.ScreenWnd_Load);
            this.DoubleClick += new System.EventHandler(this.ScreenWnd_DoubleClick);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ScreenWnd_KeyDown);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem smi_Cancel;
        private System.Windows.Forms.ToolStripMenuItem smi_OK;
    }
}