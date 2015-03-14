using Buffalo.Kernel.Win32;
namespace WordFilter
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
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolKey = new System.Windows.Forms.ToolStripMenuItem();
            this.转换类型ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.itemFont = new System.Windows.Forms.ToolStripMenuItem();
            this.itemQRCode = new System.Windows.Forms.ToolStripMenuItem();
            this.itemQRCodeEncry = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.退出ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tipInfo = new System.Windows.Forms.ToolTip(this.components);
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.ContextMenuStrip = this.contextMenuStrip1;
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "notifyIcon1";
            this.notifyIcon1.Visible = true;
            this.notifyIcon1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_MouseDoubleClick);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolKey,
            this.转换类型ToolStripMenuItem,
            this.toolStripMenuItem1,
            this.退出ToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(125, 76);
            // 
            // toolKey
            // 
            this.toolKey.Name = "toolKey";
            this.toolKey.Size = new System.Drawing.Size(124, 22);
            this.toolKey.Text = "设置";
            this.toolKey.Click += new System.EventHandler(this.toolKey_Click);
            // 
            // 转换类型ToolStripMenuItem
            // 
            this.转换类型ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.itemFont,
            this.itemQRCode,
            this.itemQRCodeEncry});
            this.转换类型ToolStripMenuItem.Name = "转换类型ToolStripMenuItem";
            this.转换类型ToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.转换类型ToolStripMenuItem.Text = "转换类型";
            // 
            // itemFont
            // 
            this.itemFont.Name = "itemFont";
            this.itemFont.Size = new System.Drawing.Size(144, 22);
            this.itemFont.Text = "文字";
            this.itemFont.Click += new System.EventHandler(this.itemType_Click);
            // 
            // itemQRCode
            // 
            this.itemQRCode.Name = "itemQRCode";
            this.itemQRCode.Size = new System.Drawing.Size(144, 22);
            this.itemQRCode.Text = "二维码";
            this.itemQRCode.Click += new System.EventHandler(this.itemType_Click);
            // 
            // itemQRCodeEncry
            // 
            this.itemQRCodeEncry.Name = "itemQRCodeEncry";
            this.itemQRCodeEncry.Size = new System.Drawing.Size(144, 22);
            this.itemQRCodeEncry.Text = "二维码(加密)";
            this.itemQRCodeEncry.Click += new System.EventHandler(this.itemType_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(121, 6);
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
            this.ClientSize = new System.Drawing.Size(251, 225);
            this.Name = "FrmMain";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem 退出ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 转换类型ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem itemFont;
        private System.Windows.Forms.ToolStripMenuItem itemQRCode;
        private System.Windows.Forms.ToolStripMenuItem itemQRCodeEncry;
        private System.Windows.Forms.ToolTip tipInfo;
        private System.Windows.Forms.ToolStripMenuItem toolKey;

    }
}

