namespace BroadcastDesktop
{
    partial class Form1
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
            this.btnListen = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.cmbIP = new System.Windows.Forms.ComboBox();
            this.nupPort = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.txturl = new System.Windows.Forms.TextBox();
            this.nupFPS = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.chkIsMouse = new System.Windows.Forms.CheckBox();
            this.cmbQty = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.gpSetting = new System.Windows.Forms.GroupBox();
            this.btnCopy = new System.Windows.Forms.Button();
            this.gpIP = new System.Windows.Forms.GroupBox();
            this.btnGetIP = new System.Windows.Forms.Button();
            this.txtInternetIP = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.btnCopyInternet = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.nupPort)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nupFPS)).BeginInit();
            this.gpSetting.SuspendLayout();
            this.gpIP.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnListen
            // 
            this.btnListen.Location = new System.Drawing.Point(48, 236);
            this.btnListen.Name = "btnListen";
            this.btnListen.Size = new System.Drawing.Size(75, 23);
            this.btnListen.TabIndex = 0;
            this.btnListen.Text = "开始";
            this.btnListen.UseVisualStyleBackColor = true;
            this.btnListen.Click += new System.EventHandler(this.btnListen_Click);
            // 
            // btnStop
            // 
            this.btnStop.Location = new System.Drawing.Point(162, 236);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(75, 23);
            this.btnStop.TabIndex = 1;
            this.btnStop.Text = "停止";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // cmbIP
            // 
            this.cmbIP.FormattingEnabled = true;
            this.cmbIP.Location = new System.Drawing.Point(66, 20);
            this.cmbIP.Name = "cmbIP";
            this.cmbIP.Size = new System.Drawing.Size(167, 20);
            this.cmbIP.TabIndex = 2;
            // 
            // nupPort
            // 
            this.nupPort.Location = new System.Drawing.Point(239, 20);
            this.nupPort.Maximum = new decimal(new int[] {
            -1530494977,
            232830,
            0,
            0});
            this.nupPort.Name = "nupPort";
            this.nupPort.Size = new System.Drawing.Size(47, 21);
            this.nupPort.TabIndex = 3;
            this.nupPort.Value = new decimal(new int[] {
            8080,
            0,
            0,
            0});
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(18, 52);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 12);
            this.label1.TabIndex = 5;
            this.label1.Text = "内网地址为：";
            // 
            // txturl
            // 
            this.txturl.Location = new System.Drawing.Point(93, 49);
            this.txturl.Name = "txturl";
            this.txturl.ReadOnly = true;
            this.txturl.Size = new System.Drawing.Size(204, 21);
            this.txturl.TabIndex = 6;
            // 
            // nupFPS
            // 
            this.nupFPS.Location = new System.Drawing.Point(239, 50);
            this.nupFPS.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.nupFPS.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nupFPS.Name = "nupFPS";
            this.nupFPS.Size = new System.Drawing.Size(53, 21);
            this.nupFPS.TabIndex = 7;
            this.nupFPS.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(298, 54);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 12);
            this.label2.TabIndex = 8;
            this.label2.Text = "帧/秒";
            // 
            // chkIsMouse
            // 
            this.chkIsMouse.AutoSize = true;
            this.chkIsMouse.Checked = true;
            this.chkIsMouse.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkIsMouse.Location = new System.Drawing.Point(296, 21);
            this.chkIsMouse.Name = "chkIsMouse";
            this.chkIsMouse.Size = new System.Drawing.Size(72, 16);
            this.chkIsMouse.TabIndex = 9;
            this.chkIsMouse.Text = "绘制鼠标";
            this.chkIsMouse.UseVisualStyleBackColor = true;
            // 
            // cmbQty
            // 
            this.cmbQty.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbQty.FormattingEnabled = true;
            this.cmbQty.Location = new System.Drawing.Point(66, 51);
            this.cmbQty.Name = "cmbQty";
            this.cmbQty.Size = new System.Drawing.Size(163, 20);
            this.cmbQty.TabIndex = 11;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.Color.Red;
            this.label3.Location = new System.Drawing.Point(18, 106);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(221, 36);
            this.label3.TabIndex = 12;
            this.label3.Text = "把地址通过IM软件发给好友\r\nWin7下选择文本的鼠标指针会不见了\r\n只需要去控制面板选择黑色鼠标方案即可";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 23);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(59, 12);
            this.label4.TabIndex = 13;
            this.label4.Text = "绑定地址:";
            // 
            // gpSetting
            // 
            this.gpSetting.Controls.Add(this.button1);
            this.gpSetting.Controls.Add(this.cmbQty);
            this.gpSetting.Controls.Add(this.label4);
            this.gpSetting.Controls.Add(this.cmbIP);
            this.gpSetting.Controls.Add(this.nupPort);
            this.gpSetting.Controls.Add(this.nupFPS);
            this.gpSetting.Controls.Add(this.chkIsMouse);
            this.gpSetting.Controls.Add(this.label2);
            this.gpSetting.Dock = System.Windows.Forms.DockStyle.Top;
            this.gpSetting.Location = new System.Drawing.Point(0, 0);
            this.gpSetting.Name = "gpSetting";
            this.gpSetting.Size = new System.Drawing.Size(408, 83);
            this.gpSetting.TabIndex = 14;
            this.gpSetting.TabStop = false;
            this.gpSetting.Text = "设置";
            // 
            // btnCopy
            // 
            this.btnCopy.Location = new System.Drawing.Point(303, 49);
            this.btnCopy.Name = "btnCopy";
            this.btnCopy.Size = new System.Drawing.Size(75, 23);
            this.btnCopy.TabIndex = 16;
            this.btnCopy.Text = "复制";
            this.btnCopy.UseVisualStyleBackColor = true;
            this.btnCopy.Click += new System.EventHandler(this.btnCopy_Click);
            // 
            // gpIP
            // 
            this.gpIP.Controls.Add(this.btnGetIP);
            this.gpIP.Controls.Add(this.txtInternetIP);
            this.gpIP.Controls.Add(this.label5);
            this.gpIP.Controls.Add(this.btnCopyInternet);
            this.gpIP.Controls.Add(this.txturl);
            this.gpIP.Controls.Add(this.label1);
            this.gpIP.Controls.Add(this.btnCopy);
            this.gpIP.Controls.Add(this.label3);
            this.gpIP.Dock = System.Windows.Forms.DockStyle.Top;
            this.gpIP.Location = new System.Drawing.Point(0, 83);
            this.gpIP.Name = "gpIP";
            this.gpIP.Size = new System.Drawing.Size(408, 147);
            this.gpIP.TabIndex = 17;
            this.gpIP.TabStop = false;
            this.gpIP.Text = "地址";
            // 
            // btnGetIP
            // 
            this.btnGetIP.Location = new System.Drawing.Point(300, 19);
            this.btnGetIP.Name = "btnGetIP";
            this.btnGetIP.Size = new System.Drawing.Size(50, 23);
            this.btnGetIP.TabIndex = 20;
            this.btnGetIP.Text = "获取";
            this.btnGetIP.UseVisualStyleBackColor = true;
            this.btnGetIP.Click += new System.EventHandler(this.btnGetIP_Click);
            // 
            // txtInternetIP
            // 
            this.txtInternetIP.Location = new System.Drawing.Point(93, 20);
            this.txtInternetIP.Name = "txtInternetIP";
            this.txtInternetIP.ReadOnly = true;
            this.txtInternetIP.Size = new System.Drawing.Size(204, 21);
            this.txtInternetIP.TabIndex = 18;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(18, 23);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(77, 12);
            this.label5.TabIndex = 17;
            this.label5.Text = "外网地址为：";
            // 
            // btnCopyInternet
            // 
            this.btnCopyInternet.Location = new System.Drawing.Point(352, 19);
            this.btnCopyInternet.Name = "btnCopyInternet";
            this.btnCopyInternet.Size = new System.Drawing.Size(50, 23);
            this.btnCopyInternet.TabIndex = 19;
            this.btnCopyInternet.Text = "复制";
            this.btnCopyInternet.UseVisualStyleBackColor = true;
            this.btnCopyInternet.Click += new System.EventHandler(this.btnCopyInternet_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(339, 47);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(63, 23);
            this.button1.TabIndex = 14;
            this.button1.Text = "NAT端口";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(408, 262);
            this.Controls.Add(this.gpIP);
            this.Controls.Add(this.gpSetting);
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.btnListen);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form1";
            this.Text = "桌面共享";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.nupPort)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nupFPS)).EndInit();
            this.gpSetting.ResumeLayout(false);
            this.gpSetting.PerformLayout();
            this.gpIP.ResumeLayout(false);
            this.gpIP.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnListen;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.ComboBox cmbIP;
        private System.Windows.Forms.NumericUpDown nupPort;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txturl;
        private System.Windows.Forms.NumericUpDown nupFPS;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox chkIsMouse;
        private System.Windows.Forms.ComboBox cmbQty;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox gpSetting;
        private System.Windows.Forms.Button btnCopy;
        private System.Windows.Forms.GroupBox gpIP;
        private System.Windows.Forms.TextBox txtInternetIP;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnCopyInternet;
        private System.Windows.Forms.Button btnGetIP;
        private System.Windows.Forms.Button button1;
    }
}

