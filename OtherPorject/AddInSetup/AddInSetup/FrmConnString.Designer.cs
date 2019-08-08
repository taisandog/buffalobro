namespace AddInSetup
{
    partial class FrmConnString
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmConnString));
            this.tbItems = new System.Windows.Forms.TabControl();
            this.tpOSS = new System.Windows.Forms.TabPage();
            this.tabStorage = new System.Windows.Forms.TabControl();
            this.tbOSS = new System.Windows.Forms.TabPage();
            this.uioss1 = new AddInSetup.ConnStringUI.UIOSS();
            this.tbCOS = new System.Windows.Forms.TabPage();
            this.uicos1 = new AddInSetup.ConnStringUI.UICOS();
            this.tbOBS = new System.Windows.Forms.TabPage();
            this.tbLocal = new System.Windows.Forms.TabPage();
            this.uiLocationStorage1 = new AddInSetup.ConnStringUI.UILocationStorage();
            this.tabCache = new System.Windows.Forms.TabPage();
            this.tbCache = new System.Windows.Forms.TabControl();
            this.tbMemcached = new System.Windows.Forms.TabPage();
            this.uiMemcached1 = new AddInSetup.ConnStringUI.UIMemcached();
            this.tbRedis = new System.Windows.Forms.TabPage();
            this.uiRedis1 = new AddInSetup.ConnStringUI.UIRedis();
            this.tbRedis2 = new System.Windows.Forms.TabPage();
            this.uiRedis21 = new AddInSetup.ConnStringUI.UIRedis2();
            this.tbWeb = new System.Windows.Forms.TabPage();
            this.uiWebCache1 = new AddInSetup.ConnStringUI.UIWebCache();
            this.tbMemory = new System.Windows.Forms.TabPage();
            this.uiSysMemory1 = new AddInSetup.ConnStringUI.UISysMemory();
            this.uiobs1 = new AddInSetup.ConnStringUI.UIOBS();
            this.tbItems.SuspendLayout();
            this.tpOSS.SuspendLayout();
            this.tabStorage.SuspendLayout();
            this.tbOSS.SuspendLayout();
            this.tbCOS.SuspendLayout();
            this.tbOBS.SuspendLayout();
            this.tbLocal.SuspendLayout();
            this.tabCache.SuspendLayout();
            this.tbCache.SuspendLayout();
            this.tbMemcached.SuspendLayout();
            this.tbRedis.SuspendLayout();
            this.tbRedis2.SuspendLayout();
            this.tbWeb.SuspendLayout();
            this.tbMemory.SuspendLayout();
            this.SuspendLayout();
            // 
            // tbItems
            // 
            this.tbItems.Controls.Add(this.tpOSS);
            this.tbItems.Controls.Add(this.tabCache);
            this.tbItems.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbItems.Location = new System.Drawing.Point(0, 0);
            this.tbItems.Name = "tbItems";
            this.tbItems.SelectedIndex = 0;
            this.tbItems.Size = new System.Drawing.Size(951, 693);
            this.tbItems.TabIndex = 0;
            // 
            // tpOSS
            // 
            this.tpOSS.Controls.Add(this.tabStorage);
            this.tpOSS.Location = new System.Drawing.Point(4, 30);
            this.tpOSS.Name = "tpOSS";
            this.tpOSS.Padding = new System.Windows.Forms.Padding(3);
            this.tpOSS.Size = new System.Drawing.Size(943, 659);
            this.tpOSS.TabIndex = 0;
            this.tpOSS.Text = "Buffalo存储";
            this.tpOSS.UseVisualStyleBackColor = true;
            // 
            // tabStorage
            // 
            this.tabStorage.Controls.Add(this.tbOSS);
            this.tabStorage.Controls.Add(this.tbCOS);
            this.tabStorage.Controls.Add(this.tbOBS);
            this.tabStorage.Controls.Add(this.tbLocal);
            this.tabStorage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabStorage.Location = new System.Drawing.Point(3, 3);
            this.tabStorage.Name = "tabStorage";
            this.tabStorage.SelectedIndex = 0;
            this.tabStorage.Size = new System.Drawing.Size(937, 653);
            this.tabStorage.TabIndex = 0;
            // 
            // tbOSS
            // 
            this.tbOSS.Controls.Add(this.uioss1);
            this.tbOSS.Location = new System.Drawing.Point(4, 30);
            this.tbOSS.Name = "tbOSS";
            this.tbOSS.Padding = new System.Windows.Forms.Padding(3);
            this.tbOSS.Size = new System.Drawing.Size(929, 619);
            this.tbOSS.TabIndex = 0;
            this.tbOSS.Text = "阿里云OSS";
            this.tbOSS.UseVisualStyleBackColor = true;
            // 
            // uioss1
            // 
            this.uioss1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uioss1.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.uioss1.Location = new System.Drawing.Point(3, 3);
            this.uioss1.Margin = new System.Windows.Forms.Padding(5);
            this.uioss1.Name = "uioss1";
            this.uioss1.Size = new System.Drawing.Size(923, 613);
            this.uioss1.TabIndex = 0;
            // 
            // tbCOS
            // 
            this.tbCOS.Controls.Add(this.uicos1);
            this.tbCOS.Location = new System.Drawing.Point(4, 30);
            this.tbCOS.Name = "tbCOS";
            this.tbCOS.Padding = new System.Windows.Forms.Padding(3);
            this.tbCOS.Size = new System.Drawing.Size(929, 619);
            this.tbCOS.TabIndex = 1;
            this.tbCOS.Text = "腾讯云COS";
            this.tbCOS.UseVisualStyleBackColor = true;
            // 
            // uicos1
            // 
            this.uicos1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uicos1.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.uicos1.Location = new System.Drawing.Point(3, 3);
            this.uicos1.Margin = new System.Windows.Forms.Padding(8, 9, 8, 9);
            this.uicos1.Name = "uicos1";
            this.uicos1.Size = new System.Drawing.Size(923, 613);
            this.uicos1.TabIndex = 0;
            // 
            // tbOBS
            // 
            this.tbOBS.Controls.Add(this.uiobs1);
            this.tbOBS.Location = new System.Drawing.Point(4, 30);
            this.tbOBS.Name = "tbOBS";
            this.tbOBS.Padding = new System.Windows.Forms.Padding(3);
            this.tbOBS.Size = new System.Drawing.Size(929, 619);
            this.tbOBS.TabIndex = 3;
            this.tbOBS.Text = "华为云OBS";
            this.tbOBS.UseVisualStyleBackColor = true;
            // 
            // tbLocal
            // 
            this.tbLocal.Controls.Add(this.uiLocationStorage1);
            this.tbLocal.Location = new System.Drawing.Point(4, 30);
            this.tbLocal.Name = "tbLocal";
            this.tbLocal.Size = new System.Drawing.Size(929, 619);
            this.tbLocal.TabIndex = 2;
            this.tbLocal.Text = "本地/局域网共享";
            this.tbLocal.UseVisualStyleBackColor = true;
            // 
            // uiLocationStorage1
            // 
            this.uiLocationStorage1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiLocationStorage1.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.uiLocationStorage1.Location = new System.Drawing.Point(0, 0);
            this.uiLocationStorage1.Margin = new System.Windows.Forms.Padding(5);
            this.uiLocationStorage1.Name = "uiLocationStorage1";
            this.uiLocationStorage1.Size = new System.Drawing.Size(929, 619);
            this.uiLocationStorage1.TabIndex = 0;
            // 
            // tabCache
            // 
            this.tabCache.Controls.Add(this.tbCache);
            this.tabCache.Location = new System.Drawing.Point(4, 30);
            this.tabCache.Name = "tabCache";
            this.tabCache.Padding = new System.Windows.Forms.Padding(3);
            this.tabCache.Size = new System.Drawing.Size(943, 659);
            this.tabCache.TabIndex = 1;
            this.tabCache.Text = "Buffalo缓存";
            this.tabCache.UseVisualStyleBackColor = true;
            // 
            // tbCache
            // 
            this.tbCache.Controls.Add(this.tbMemcached);
            this.tbCache.Controls.Add(this.tbRedis);
            this.tbCache.Controls.Add(this.tbRedis2);
            this.tbCache.Controls.Add(this.tbWeb);
            this.tbCache.Controls.Add(this.tbMemory);
            this.tbCache.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbCache.Location = new System.Drawing.Point(3, 3);
            this.tbCache.Name = "tbCache";
            this.tbCache.SelectedIndex = 0;
            this.tbCache.Size = new System.Drawing.Size(937, 653);
            this.tbCache.TabIndex = 0;
            // 
            // tbMemcached
            // 
            this.tbMemcached.Controls.Add(this.uiMemcached1);
            this.tbMemcached.Location = new System.Drawing.Point(4, 30);
            this.tbMemcached.Name = "tbMemcached";
            this.tbMemcached.Size = new System.Drawing.Size(929, 619);
            this.tbMemcached.TabIndex = 2;
            this.tbMemcached.Text = "Memcached";
            this.tbMemcached.UseVisualStyleBackColor = true;
            // 
            // uiMemcached1
            // 
            this.uiMemcached1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiMemcached1.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.uiMemcached1.Location = new System.Drawing.Point(0, 0);
            this.uiMemcached1.Margin = new System.Windows.Forms.Padding(5);
            this.uiMemcached1.Name = "uiMemcached1";
            this.uiMemcached1.Size = new System.Drawing.Size(929, 619);
            this.uiMemcached1.TabIndex = 0;
            // 
            // tbRedis
            // 
            this.tbRedis.Controls.Add(this.uiRedis1);
            this.tbRedis.Location = new System.Drawing.Point(4, 30);
            this.tbRedis.Name = "tbRedis";
            this.tbRedis.Size = new System.Drawing.Size(929, 619);
            this.tbRedis.TabIndex = 3;
            this.tbRedis.Text = "Redis(.NET3.5、.NET4.0)";
            this.tbRedis.UseVisualStyleBackColor = true;
            // 
            // uiRedis1
            // 
            this.uiRedis1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiRedis1.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.uiRedis1.Location = new System.Drawing.Point(0, 0);
            this.uiRedis1.Margin = new System.Windows.Forms.Padding(5);
            this.uiRedis1.Name = "uiRedis1";
            this.uiRedis1.Size = new System.Drawing.Size(929, 619);
            this.uiRedis1.TabIndex = 0;
            // 
            // tbRedis2
            // 
            this.tbRedis2.Controls.Add(this.uiRedis21);
            this.tbRedis2.Location = new System.Drawing.Point(4, 30);
            this.tbRedis2.Name = "tbRedis2";
            this.tbRedis2.Size = new System.Drawing.Size(929, 619);
            this.tbRedis2.TabIndex = 4;
            this.tbRedis2.Text = "Redis(.NET4.5或以上)";
            this.tbRedis2.UseVisualStyleBackColor = true;
            // 
            // uiRedis21
            // 
            this.uiRedis21.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiRedis21.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.uiRedis21.Location = new System.Drawing.Point(0, 0);
            this.uiRedis21.Margin = new System.Windows.Forms.Padding(5);
            this.uiRedis21.Name = "uiRedis21";
            this.uiRedis21.Size = new System.Drawing.Size(929, 619);
            this.uiRedis21.TabIndex = 0;
            // 
            // tbWeb
            // 
            this.tbWeb.Controls.Add(this.uiWebCache1);
            this.tbWeb.Location = new System.Drawing.Point(4, 30);
            this.tbWeb.Name = "tbWeb";
            this.tbWeb.Padding = new System.Windows.Forms.Padding(3);
            this.tbWeb.Size = new System.Drawing.Size(929, 619);
            this.tbWeb.TabIndex = 0;
            this.tbWeb.Text = "Web类缓存";
            this.tbWeb.UseVisualStyleBackColor = true;
            // 
            // uiWebCache1
            // 
            this.uiWebCache1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiWebCache1.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.uiWebCache1.Location = new System.Drawing.Point(3, 3);
            this.uiWebCache1.Margin = new System.Windows.Forms.Padding(5);
            this.uiWebCache1.Name = "uiWebCache1";
            this.uiWebCache1.Size = new System.Drawing.Size(923, 613);
            this.uiWebCache1.TabIndex = 0;
            // 
            // tbMemory
            // 
            this.tbMemory.Controls.Add(this.uiSysMemory1);
            this.tbMemory.Location = new System.Drawing.Point(4, 30);
            this.tbMemory.Name = "tbMemory";
            this.tbMemory.Padding = new System.Windows.Forms.Padding(3);
            this.tbMemory.Size = new System.Drawing.Size(929, 619);
            this.tbMemory.TabIndex = 1;
            this.tbMemory.Text = "系统内存";
            this.tbMemory.UseVisualStyleBackColor = true;
            // 
            // uiSysMemory1
            // 
            this.uiSysMemory1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiSysMemory1.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.uiSysMemory1.Location = new System.Drawing.Point(3, 3);
            this.uiSysMemory1.Margin = new System.Windows.Forms.Padding(5);
            this.uiSysMemory1.Name = "uiSysMemory1";
            this.uiSysMemory1.Size = new System.Drawing.Size(923, 613);
            this.uiSysMemory1.TabIndex = 0;
            // 
            // uiobs1
            // 
            this.uiobs1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiobs1.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.uiobs1.Location = new System.Drawing.Point(3, 3);
            this.uiobs1.Margin = new System.Windows.Forms.Padding(5);
            this.uiobs1.Name = "uiobs1";
            this.uiobs1.Size = new System.Drawing.Size(923, 613);
            this.uiobs1.TabIndex = 0;
            // 
            // FrmConnString
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(951, 693);
            this.Controls.Add(this.tbItems);
            this.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(5);
            this.Name = "FrmConnString";
            this.Text = "BuffaloStorage字符串生成器";
            this.tbItems.ResumeLayout(false);
            this.tpOSS.ResumeLayout(false);
            this.tabStorage.ResumeLayout(false);
            this.tbOSS.ResumeLayout(false);
            this.tbCOS.ResumeLayout(false);
            this.tbOBS.ResumeLayout(false);
            this.tbLocal.ResumeLayout(false);
            this.tabCache.ResumeLayout(false);
            this.tbCache.ResumeLayout(false);
            this.tbMemcached.ResumeLayout(false);
            this.tbRedis.ResumeLayout(false);
            this.tbRedis2.ResumeLayout(false);
            this.tbWeb.ResumeLayout(false);
            this.tbMemory.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tbItems;
        private System.Windows.Forms.TabPage tpOSS;
        private System.Windows.Forms.TabPage tabCache;
        private System.Windows.Forms.TabControl tabStorage;
        private System.Windows.Forms.TabPage tbOSS;
        private ConnStringUI.UIOSS uioss1;
        private System.Windows.Forms.TabPage tbCOS;
        private ConnStringUI.UICOS uicos1;
        private System.Windows.Forms.TabControl tbCache;
        private System.Windows.Forms.TabPage tbMemory;
        private System.Windows.Forms.TabPage tbWeb;
        private System.Windows.Forms.TabPage tbMemcached;
        private System.Windows.Forms.TabPage tbRedis;
        private ConnStringUI.UIMemcached uiMemcached1;
        private ConnStringUI.UIRedis uiRedis1;
        private ConnStringUI.UIWebCache uiWebCache1;
        private ConnStringUI.UISysMemory uiSysMemory1;
        private System.Windows.Forms.TabPage tbLocal;
        private ConnStringUI.UILocationStorage uiLocationStorage1;
        private System.Windows.Forms.TabPage tbRedis2;
        private ConnStringUI.UIRedis2 uiRedis21;
        private System.Windows.Forms.TabPage tbOBS;
        private ConnStringUI.UIOBS uiobs1;
    }
}