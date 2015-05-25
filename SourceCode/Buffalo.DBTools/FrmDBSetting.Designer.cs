namespace Buffalo.DBTools
{
    partial class FrmDBSetting
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmDBSetting));
            this.cmbType = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.rtbConnstr = new System.Windows.Forms.RichTextBox();
            this.btnTest = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.cmbTier = new System.Windows.Forms.ComboBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.clbSummary = new System.Windows.Forms.CheckedListBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnModel = new System.Windows.Forms.Button();
            this.chkAllDal = new System.Windows.Forms.CheckBox();
            this.chkEntityToDirectory = new System.Windows.Forms.CheckBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.gpCache = new System.Windows.Forms.GroupBox();
            this.ckbAll = new System.Windows.Forms.CheckBox();
            this.gpCacheServer = new System.Windows.Forms.GroupBox();
            this.txtCacheServer = new System.Windows.Forms.RichTextBox();
            this.btnCacheModel = new System.Windows.Forms.Button();
            this.cmbCacheType = new System.Windows.Forms.ComboBox();
            this.btnCache = new System.Windows.Forms.Button();
            this.btnImp = new System.Windows.Forms.Button();
            this.cmbLazy = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.gpCache.SuspendLayout();
            this.gpCacheServer.SuspendLayout();
            this.SuspendLayout();
            // 
            // cmbType
            // 
            this.cmbType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbType.FormattingEnabled = true;
            this.cmbType.Location = new System.Drawing.Point(77, 11);
            this.cmbType.Name = "cmbType";
            this.cmbType.Size = new System.Drawing.Size(157, 20);
            this.cmbType.TabIndex = 0;
            this.cmbType.SelectedValueChanged += new System.EventHandler(this.cmbType_SelectedValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(2, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "数据库类型:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(2, 85);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(71, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "连接字符串:";
            // 
            // rtbConnstr
            // 
            this.rtbConnstr.Location = new System.Drawing.Point(78, 83);
            this.rtbConnstr.Name = "rtbConnstr";
            this.rtbConnstr.Size = new System.Drawing.Size(265, 72);
            this.rtbConnstr.TabIndex = 3;
            this.rtbConnstr.Text = "";
            // 
            // btnTest
            // 
            this.btnTest.Location = new System.Drawing.Point(78, 187);
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(75, 23);
            this.btnTest.TabIndex = 4;
            this.btnTest.Text = "测试";
            this.btnTest.UseVisualStyleBackColor = true;
            this.btnTest.Click += new System.EventHandler(this.btnTest_Click);
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(159, 187);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 5;
            this.btnOK.Text = "确定";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(268, 187);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "取消";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // cmbTier
            // 
            this.cmbTier.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTier.FormattingEnabled = true;
            this.cmbTier.Location = new System.Drawing.Point(240, 11);
            this.cmbTier.Name = "cmbTier";
            this.cmbTier.Size = new System.Drawing.Size(95, 20);
            this.cmbTier.TabIndex = 7;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(346, 15);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(132, 166);
            this.pictureBox1.TabIndex = 8;
            this.pictureBox1.TabStop = false;
            // 
            // clbSummary
            // 
            this.clbSummary.ColumnWidth = 65;
            this.clbSummary.FormattingEnabled = true;
            this.clbSummary.Location = new System.Drawing.Point(79, 160);
            this.clbSummary.MultiColumn = true;
            this.clbSummary.Name = "clbSummary";
            this.clbSummary.Size = new System.Drawing.Size(265, 20);
            this.clbSummary.TabIndex = 9;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(2, 165);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(59, 12);
            this.label3.TabIndex = 10;
            this.label3.Text = "注释显示:";
            // 
            // btnModel
            // 
            this.btnModel.Location = new System.Drawing.Point(3, 103);
            this.btnModel.Name = "btnModel";
            this.btnModel.Size = new System.Drawing.Size(68, 23);
            this.btnModel.TabIndex = 11;
            this.btnModel.Text = "参考";
            this.btnModel.UseVisualStyleBackColor = true;
            this.btnModel.Click += new System.EventHandler(this.btnModel_Click);
            // 
            // chkAllDal
            // 
            this.chkAllDal.AutoSize = true;
            this.chkAllDal.Location = new System.Drawing.Point(203, 61);
            this.chkAllDal.Name = "chkAllDal";
            this.chkAllDal.Size = new System.Drawing.Size(84, 16);
            this.chkAllDal.TabIndex = 12;
            this.chkAllDal.Text = "所有数据层";
            this.chkAllDal.UseVisualStyleBackColor = true;
            // 
            // chkEntityToDirectory
            // 
            this.chkEntityToDirectory.AutoSize = true;
            this.chkEntityToDirectory.Location = new System.Drawing.Point(77, 61);
            this.chkEntityToDirectory.Name = "chkEntityToDirectory";
            this.chkEntityToDirectory.Size = new System.Drawing.Size(120, 16);
            this.chkEntityToDirectory.TabIndex = 13;
            this.chkEntityToDirectory.Text = "实体保存到文件夹";
            this.toolTip1.SetToolTip(this.chkEntityToDirectory, "库到类模式下，实体将会放到Entity中");
            this.chkEntityToDirectory.UseVisualStyleBackColor = true;
            // 
            // gpCache
            // 
            this.gpCache.Controls.Add(this.ckbAll);
            this.gpCache.Controls.Add(this.gpCacheServer);
            this.gpCache.Controls.Add(this.cmbCacheType);
            this.gpCache.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.gpCache.Location = new System.Drawing.Point(0, 211);
            this.gpCache.Name = "gpCache";
            this.gpCache.Size = new System.Drawing.Size(479, 115);
            this.gpCache.TabIndex = 14;
            this.gpCache.TabStop = false;
            this.gpCache.Text = "缓存设置";
            // 
            // ckbAll
            // 
            this.ckbAll.AutoSize = true;
            this.ckbAll.Location = new System.Drawing.Point(361, 17);
            this.ckbAll.Name = "ckbAll";
            this.ckbAll.Size = new System.Drawing.Size(108, 16);
            this.ckbAll.TabIndex = 2;
            this.ckbAll.Text = "所有表启用缓存";
            this.ckbAll.UseVisualStyleBackColor = true;
            // 
            // gpCacheServer
            // 
            this.gpCacheServer.Controls.Add(this.txtCacheServer);
            this.gpCacheServer.Controls.Add(this.btnCacheModel);
            this.gpCacheServer.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.gpCacheServer.Location = new System.Drawing.Point(3, 37);
            this.gpCacheServer.Name = "gpCacheServer";
            this.gpCacheServer.Size = new System.Drawing.Size(473, 75);
            this.gpCacheServer.TabIndex = 1;
            this.gpCacheServer.TabStop = false;
            this.gpCacheServer.Text = "缓存服务器连接字符串";
            // 
            // txtCacheServer
            // 
            this.txtCacheServer.Dock = System.Windows.Forms.DockStyle.Right;
            this.txtCacheServer.Location = new System.Drawing.Point(103, 17);
            this.txtCacheServer.Name = "txtCacheServer";
            this.txtCacheServer.Size = new System.Drawing.Size(367, 55);
            this.txtCacheServer.TabIndex = 6;
            this.txtCacheServer.Text = "";
            // 
            // btnCacheModel
            // 
            this.btnCacheModel.Location = new System.Drawing.Point(5, 30);
            this.btnCacheModel.Name = "btnCacheModel";
            this.btnCacheModel.Size = new System.Drawing.Size(62, 23);
            this.btnCacheModel.TabIndex = 5;
            this.btnCacheModel.Text = "参考";
            this.btnCacheModel.UseVisualStyleBackColor = true;
            this.btnCacheModel.Click += new System.EventHandler(this.btnCacheModel_Click);
            // 
            // cmbCacheType
            // 
            this.cmbCacheType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCacheType.FormattingEnabled = true;
            this.cmbCacheType.Location = new System.Drawing.Point(8, 14);
            this.cmbCacheType.Name = "cmbCacheType";
            this.cmbCacheType.Size = new System.Drawing.Size(145, 20);
            this.cmbCacheType.TabIndex = 0;
            this.cmbCacheType.SelectedIndexChanged += new System.EventHandler(this.cmbCacheType_SelectedIndexChanged);
            // 
            // btnCache
            // 
            this.btnCache.Location = new System.Drawing.Point(355, 186);
            this.btnCache.Name = "btnCache";
            this.btnCache.Size = new System.Drawing.Size(108, 23);
            this.btnCache.TabIndex = 15;
            this.btnCache.Text = "缓存设置↓";
            this.btnCache.UseVisualStyleBackColor = true;
            this.btnCache.Click += new System.EventHandler(this.btnCache_Click);
            // 
            // btnImp
            // 
            this.btnImp.ForeColor = System.Drawing.Color.Red;
            this.btnImp.Location = new System.Drawing.Point(3, 132);
            this.btnImp.Name = "btnImp";
            this.btnImp.Size = new System.Drawing.Size(67, 23);
            this.btnImp.TabIndex = 16;
            this.btnImp.Text = "注意事项";
            this.btnImp.UseVisualStyleBackColor = true;
            this.btnImp.Click += new System.EventHandler(this.btnImp_Click);
            // 
            // cmbLazy
            // 
            this.cmbLazy.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbLazy.FormattingEnabled = true;
            this.cmbLazy.Location = new System.Drawing.Point(77, 36);
            this.cmbLazy.Name = "cmbLazy";
            this.cmbLazy.Size = new System.Drawing.Size(157, 20);
            this.cmbLazy.TabIndex = 17;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 39);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(59, 12);
            this.label4.TabIndex = 18;
            this.label4.Text = "延迟加载:";
            // 
            // FrmDBSetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(479, 326);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.cmbLazy);
            this.Controls.Add(this.btnImp);
            this.Controls.Add(this.btnCache);
            this.Controls.Add(this.gpCache);
            this.Controls.Add(this.chkEntityToDirectory);
            this.Controls.Add(this.clbSummary);
            this.Controls.Add(this.rtbConnstr);
            this.Controls.Add(this.chkAllDal);
            this.Controls.Add(this.btnModel);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cmbTier);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnTest);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmbType);
            this.Controls.Add(this.pictureBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmDBSetting";
            this.Text = "Buffalo助手--数据库设置";
            this.Load += new System.EventHandler(this.FrmDBSetting_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.gpCache.ResumeLayout(false);
            this.gpCache.PerformLayout();
            this.gpCacheServer.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cmbType;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.RichTextBox rtbConnstr;
        private System.Windows.Forms.Button btnTest;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ComboBox cmbTier;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.CheckedListBox clbSummary;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnModel;
        private System.Windows.Forms.CheckBox chkAllDal;
        private System.Windows.Forms.CheckBox chkEntityToDirectory;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.GroupBox gpCache;
        private System.Windows.Forms.ComboBox cmbCacheType;
        private System.Windows.Forms.GroupBox gpCacheServer;
        private System.Windows.Forms.RichTextBox txtCacheServer;
        private System.Windows.Forms.Button btnCacheModel;
        private System.Windows.Forms.CheckBox ckbAll;
        private System.Windows.Forms.Button btnCache;
        private System.Windows.Forms.Button btnImp;
        private System.Windows.Forms.ComboBox cmbLazy;
        private System.Windows.Forms.Label label4;
    }
}