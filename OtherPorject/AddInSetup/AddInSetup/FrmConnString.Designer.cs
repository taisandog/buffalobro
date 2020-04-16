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
            this.tbCOS = new System.Windows.Forms.TabPage();
            this.tbOBS = new System.Windows.Forms.TabPage();
            this.tbAWS = new System.Windows.Forms.TabPage();
            this.tbLocal = new System.Windows.Forms.TabPage();
            this.tabCache = new System.Windows.Forms.TabPage();
            this.tbCache = new System.Windows.Forms.TabControl();
            this.tbMemcached = new System.Windows.Forms.TabPage();
            this.tbRedis = new System.Windows.Forms.TabPage();
            this.tbRedis2 = new System.Windows.Forms.TabPage();
            this.tbWeb = new System.Windows.Forms.TabPage();
            this.tbMemory = new System.Windows.Forms.TabPage();
            this.tabMQ = new System.Windows.Forms.TabPage();
            this.tbMQ = new System.Windows.Forms.TabControl();
            this.tpRedisMQ = new System.Windows.Forms.TabPage();
            this.tpRabbitMQ = new System.Windows.Forms.TabPage();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.uioss1 = new AddInSetup.ConnStringUI.UIOSS();
            this.uicos1 = new AddInSetup.ConnStringUI.UICOS();
            this.uiobs1 = new AddInSetup.ConnStringUI.UIOBS();
            this.uiaws1 = new AddInSetup.ConnStringUI.UIAWS();
            this.uiLocationStorage1 = new AddInSetup.ConnStringUI.UILocationStorage();
            this.uiMemcached1 = new AddInSetup.ConnStringUI.UIMemcached();
            this.uiRedis1 = new AddInSetup.ConnStringUI.UIRedis();
            this.uiRedis21 = new AddInSetup.ConnStringUI.UIRedis2();
            this.uiWebCache1 = new AddInSetup.ConnStringUI.UIWebCache();
            this.uiSysMemory1 = new AddInSetup.ConnStringUI.UISysMemory();
            this.uiRedisMQ1 = new AddInSetup.ConnStringUI.UIRedisMQ();
            this.uiRabbitMQ1 = new AddInSetup.ConnStringUI.UIRabbitMQ();
            this.tbItems.SuspendLayout();
            this.tpOSS.SuspendLayout();
            this.tabStorage.SuspendLayout();
            this.tbOSS.SuspendLayout();
            this.tbCOS.SuspendLayout();
            this.tbOBS.SuspendLayout();
            this.tbAWS.SuspendLayout();
            this.tbLocal.SuspendLayout();
            this.tabCache.SuspendLayout();
            this.tbCache.SuspendLayout();
            this.tbMemcached.SuspendLayout();
            this.tbRedis.SuspendLayout();
            this.tbRedis2.SuspendLayout();
            this.tbWeb.SuspendLayout();
            this.tbMemory.SuspendLayout();
            this.tabMQ.SuspendLayout();
            this.tbMQ.SuspendLayout();
            this.tpRedisMQ.SuspendLayout();
            this.tpRabbitMQ.SuspendLayout();
            this.SuspendLayout();
            // 
            // tbItems
            // 
            this.tbItems.Controls.Add(this.tpOSS);
            this.tbItems.Controls.Add(this.tabCache);
            this.tbItems.Controls.Add(this.tabMQ);
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
            this.tabStorage.Controls.Add(this.tbAWS);
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
            // tbAWS
            // 
            this.tbAWS.Controls.Add(this.uiaws1);
            this.tbAWS.Location = new System.Drawing.Point(4, 30);
            this.tbAWS.Name = "tbAWS";
            this.tbAWS.Size = new System.Drawing.Size(929, 619);
            this.tbAWS.TabIndex = 4;
            this.tbAWS.Text = "AWS S3";
            this.tbAWS.UseVisualStyleBackColor = true;
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
            this.tbCache.Size = new System.Drawing.Size(937, 661);
            this.tbCache.TabIndex = 0;
            // 
            // tbMemcached
            // 
            this.tbMemcached.Controls.Add(this.uiMemcached1);
            this.tbMemcached.Location = new System.Drawing.Point(4, 30);
            this.tbMemcached.Name = "tbMemcached";
            this.tbMemcached.Size = new System.Drawing.Size(929, 627);
            this.tbMemcached.TabIndex = 2;
            this.tbMemcached.Text = "Memcached";
            this.tbMemcached.UseVisualStyleBackColor = true;
            // 
            // tbRedis
            // 
            this.tbRedis.Controls.Add(this.uiRedis1);
            this.tbRedis.Location = new System.Drawing.Point(4, 30);
            this.tbRedis.Name = "tbRedis";
            this.tbRedis.Size = new System.Drawing.Size(929, 627);
            this.tbRedis.TabIndex = 3;
            this.tbRedis.Text = "Redis(.NET3.5、.NET4.0)";
            this.tbRedis.UseVisualStyleBackColor = true;
            // 
            // tbRedis2
            // 
            this.tbRedis2.Controls.Add(this.uiRedis21);
            this.tbRedis2.Location = new System.Drawing.Point(4, 30);
            this.tbRedis2.Name = "tbRedis2";
            this.tbRedis2.Size = new System.Drawing.Size(929, 627);
            this.tbRedis2.TabIndex = 4;
            this.tbRedis2.Text = "Redis(.NET4.5或以上)";
            this.tbRedis2.UseVisualStyleBackColor = true;
            // 
            // tbWeb
            // 
            this.tbWeb.Controls.Add(this.uiWebCache1);
            this.tbWeb.Location = new System.Drawing.Point(4, 30);
            this.tbWeb.Name = "tbWeb";
            this.tbWeb.Padding = new System.Windows.Forms.Padding(3);
            this.tbWeb.Size = new System.Drawing.Size(929, 627);
            this.tbWeb.TabIndex = 0;
            this.tbWeb.Text = "Web类缓存";
            this.tbWeb.UseVisualStyleBackColor = true;
            // 
            // tbMemory
            // 
            this.tbMemory.Controls.Add(this.uiSysMemory1);
            this.tbMemory.Location = new System.Drawing.Point(4, 30);
            this.tbMemory.Name = "tbMemory";
            this.tbMemory.Padding = new System.Windows.Forms.Padding(3);
            this.tbMemory.Size = new System.Drawing.Size(929, 627);
            this.tbMemory.TabIndex = 1;
            this.tbMemory.Text = "系统内存";
            this.tbMemory.UseVisualStyleBackColor = true;
            // 
            // tabMQ
            // 
            this.tabMQ.Controls.Add(this.tbMQ);
            this.tabMQ.Location = new System.Drawing.Point(4, 30);
            this.tabMQ.Name = "tabMQ";
            this.tabMQ.Size = new System.Drawing.Size(943, 659);
            this.tabMQ.TabIndex = 2;
            this.tabMQ.Text = "队列";
            this.tabMQ.UseVisualStyleBackColor = true;
            // 
            // tbMQ
            // 
            this.tbMQ.Controls.Add(this.tpRedisMQ);
            this.tbMQ.Controls.Add(this.tpRabbitMQ);
            this.tbMQ.Controls.Add(this.tabPage4);
            this.tbMQ.Controls.Add(this.tabPage5);
            this.tbMQ.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbMQ.Location = new System.Drawing.Point(0, 0);
            this.tbMQ.Name = "tbMQ";
            this.tbMQ.SelectedIndex = 0;
            this.tbMQ.Size = new System.Drawing.Size(943, 659);
            this.tbMQ.TabIndex = 1;
            // 
            // tpRedisMQ
            // 
            this.tpRedisMQ.Controls.Add(this.uiRedisMQ1);
            this.tpRedisMQ.Location = new System.Drawing.Point(4, 30);
            this.tpRedisMQ.Name = "tpRedisMQ";
            this.tpRedisMQ.Size = new System.Drawing.Size(935, 625);
            this.tpRedisMQ.TabIndex = 2;
            this.tpRedisMQ.Text = "RedisMQ";
            this.tpRedisMQ.UseVisualStyleBackColor = true;
            // 
            // tpRabbitMQ
            // 
            this.tpRabbitMQ.Controls.Add(this.uiRabbitMQ1);
            this.tpRabbitMQ.Location = new System.Drawing.Point(4, 30);
            this.tpRabbitMQ.Name = "tpRabbitMQ";
            this.tpRabbitMQ.Size = new System.Drawing.Size(935, 625);
            this.tpRabbitMQ.TabIndex = 4;
            this.tpRabbitMQ.Text = "RabbitMQ";
            this.tpRabbitMQ.UseVisualStyleBackColor = true;
            // 
            // tabPage4
            // 
            this.tabPage4.Location = new System.Drawing.Point(4, 30);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(935, 633);
            this.tabPage4.TabIndex = 0;
            this.tabPage4.Text = "Web类缓存";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // tabPage5
            // 
            this.tabPage5.Location = new System.Drawing.Point(4, 30);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage5.Size = new System.Drawing.Size(935, 633);
            this.tabPage5.TabIndex = 1;
            this.tabPage5.Text = "系统内存";
            this.tabPage5.UseVisualStyleBackColor = true;
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
            // uicos1
            // 
            this.uicos1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uicos1.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.uicos1.Location = new System.Drawing.Point(3, 3);
            this.uicos1.Margin = new System.Windows.Forms.Padding(8, 9, 8, 9);
            this.uicos1.Name = "uicos1";
            this.uicos1.Size = new System.Drawing.Size(923, 621);
            this.uicos1.TabIndex = 0;
            // 
            // uiobs1
            // 
            this.uiobs1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiobs1.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.uiobs1.Location = new System.Drawing.Point(3, 3);
            this.uiobs1.Margin = new System.Windows.Forms.Padding(5);
            this.uiobs1.Name = "uiobs1";
            this.uiobs1.Size = new System.Drawing.Size(923, 621);
            this.uiobs1.TabIndex = 0;
            // 
            // uiaws1
            // 
            this.uiaws1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiaws1.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.uiaws1.Location = new System.Drawing.Point(0, 0);
            this.uiaws1.Margin = new System.Windows.Forms.Padding(8, 9, 8, 9);
            this.uiaws1.Name = "uiaws1";
            this.uiaws1.Size = new System.Drawing.Size(929, 627);
            this.uiaws1.TabIndex = 0;
            // 
            // uiLocationStorage1
            // 
            this.uiLocationStorage1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiLocationStorage1.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.uiLocationStorage1.Location = new System.Drawing.Point(0, 0);
            this.uiLocationStorage1.Margin = new System.Windows.Forms.Padding(5);
            this.uiLocationStorage1.Name = "uiLocationStorage1";
            this.uiLocationStorage1.Size = new System.Drawing.Size(929, 627);
            this.uiLocationStorage1.TabIndex = 0;
            // 
            // uiMemcached1
            // 
            this.uiMemcached1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiMemcached1.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.uiMemcached1.Location = new System.Drawing.Point(0, 0);
            this.uiMemcached1.Margin = new System.Windows.Forms.Padding(5);
            this.uiMemcached1.Name = "uiMemcached1";
            this.uiMemcached1.Size = new System.Drawing.Size(929, 627);
            this.uiMemcached1.TabIndex = 0;
            // 
            // uiRedis1
            // 
            this.uiRedis1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiRedis1.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.uiRedis1.Location = new System.Drawing.Point(0, 0);
            this.uiRedis1.Margin = new System.Windows.Forms.Padding(5);
            this.uiRedis1.Name = "uiRedis1";
            this.uiRedis1.Size = new System.Drawing.Size(929, 627);
            this.uiRedis1.TabIndex = 0;
            // 
            // uiRedis21
            // 
            this.uiRedis21.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiRedis21.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.uiRedis21.Location = new System.Drawing.Point(0, 0);
            this.uiRedis21.Margin = new System.Windows.Forms.Padding(5);
            this.uiRedis21.Name = "uiRedis21";
            this.uiRedis21.Size = new System.Drawing.Size(929, 627);
            this.uiRedis21.TabIndex = 0;
            // 
            // uiWebCache1
            // 
            this.uiWebCache1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiWebCache1.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.uiWebCache1.Location = new System.Drawing.Point(3, 3);
            this.uiWebCache1.Margin = new System.Windows.Forms.Padding(5);
            this.uiWebCache1.Name = "uiWebCache1";
            this.uiWebCache1.Size = new System.Drawing.Size(923, 621);
            this.uiWebCache1.TabIndex = 0;
            // 
            // uiSysMemory1
            // 
            this.uiSysMemory1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiSysMemory1.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.uiSysMemory1.Location = new System.Drawing.Point(3, 3);
            this.uiSysMemory1.Margin = new System.Windows.Forms.Padding(5);
            this.uiSysMemory1.Name = "uiSysMemory1";
            this.uiSysMemory1.Size = new System.Drawing.Size(923, 621);
            this.uiSysMemory1.TabIndex = 0;
            // 
            // uiRedisMQ1
            // 
            this.uiRedisMQ1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiRedisMQ1.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.uiRedisMQ1.Location = new System.Drawing.Point(0, 0);
            this.uiRedisMQ1.Margin = new System.Windows.Forms.Padding(5);
            this.uiRedisMQ1.Name = "uiRedisMQ1";
            this.uiRedisMQ1.Size = new System.Drawing.Size(935, 625);
            this.uiRedisMQ1.TabIndex = 0;
            // 
            // uiRabbitMQ1
            // 
            this.uiRabbitMQ1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiRabbitMQ1.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.uiRabbitMQ1.Location = new System.Drawing.Point(0, 0);
            this.uiRabbitMQ1.Margin = new System.Windows.Forms.Padding(5);
            this.uiRabbitMQ1.Name = "uiRabbitMQ1";
            this.uiRabbitMQ1.Size = new System.Drawing.Size(935, 625);
            this.uiRabbitMQ1.TabIndex = 0;
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
            this.tbAWS.ResumeLayout(false);
            this.tbLocal.ResumeLayout(false);
            this.tabCache.ResumeLayout(false);
            this.tbCache.ResumeLayout(false);
            this.tbMemcached.ResumeLayout(false);
            this.tbRedis.ResumeLayout(false);
            this.tbRedis2.ResumeLayout(false);
            this.tbWeb.ResumeLayout(false);
            this.tbMemory.ResumeLayout(false);
            this.tabMQ.ResumeLayout(false);
            this.tbMQ.ResumeLayout(false);
            this.tpRedisMQ.ResumeLayout(false);
            this.tpRabbitMQ.ResumeLayout(false);
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
        private System.Windows.Forms.TabPage tbAWS;
        private ConnStringUI.UIAWS uiaws1;
        private System.Windows.Forms.TabPage tabMQ;
        private System.Windows.Forms.TabControl tbMQ;
        private System.Windows.Forms.TabPage tpRedisMQ;
        private System.Windows.Forms.TabPage tpRabbitMQ;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.TabPage tabPage5;
        private ConnStringUI.UIRedisMQ uiRedisMQ1;
        private ConnStringUI.UIRabbitMQ uiRabbitMQ1;
    }
}