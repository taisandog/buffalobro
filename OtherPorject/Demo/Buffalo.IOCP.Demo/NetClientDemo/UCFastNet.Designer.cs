namespace NetClientDemo
{
    partial class UCFastNet
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
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.label3 = new System.Windows.Forms.Label();
            this.btnSend = new System.Windows.Forms.Button();
            this.btnStart = new System.Windows.Forms.Button();
            this.nupFastPort = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.txtFastIP = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnStop = new System.Windows.Forms.Button();
            this.txtContent = new System.Windows.Forms.RichTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.nupFastPort)).BeginInit();
            this.SuspendLayout();
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 82);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 17);
            this.label3.TabIndex = 15;
            this.label3.Text = "消息:";
            // 
            // btnSend
            // 
            this.btnSend.Location = new System.Drawing.Point(273, 56);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(86, 43);
            this.btnSend.TabIndex = 13;
            this.btnSend.Text = "发消息";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(169, 105);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(86, 31);
            this.btnStart.TabIndex = 12;
            this.btnStart.Text = "连接";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // nupFastPort
            // 
            this.nupFastPort.Location = new System.Drawing.Point(186, 14);
            this.nupFastPort.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.nupFastPort.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nupFastPort.Name = "nupFastPort";
            this.nupFastPort.Size = new System.Drawing.Size(86, 23);
            this.nupFastPort.TabIndex = 11;
            this.nupFastPort.Value = new decimal(new int[] {
            8586,
            0,
            0,
            0});
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(169, 17);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(11, 17);
            this.label2.TabIndex = 10;
            this.label2.Text = ":";
            // 
            // txtFastIP
            // 
            this.txtFastIP.Location = new System.Drawing.Point(54, 14);
            this.txtFastIP.Name = "txtFastIP";
            this.txtFastIP.Size = new System.Drawing.Size(109, 23);
            this.txtFastIP.TabIndex = 9;
            this.txtFastIP.Text = "localhost";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 17);
            this.label1.TabIndex = 8;
            this.label1.Text = "地址:";
            // 
            // btnStop
            // 
            this.btnStop.Location = new System.Drawing.Point(273, 105);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(86, 31);
            this.btnStop.TabIndex = 16;
            this.btnStop.Text = "停止";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // txtContent
            // 
            this.txtContent.Location = new System.Drawing.Point(54, 43);
            this.txtContent.Name = "txtContent";
            this.txtContent.Size = new System.Drawing.Size(218, 56);
            this.txtContent.TabIndex = 17;
            this.txtContent.Text = "";
            // 
            // UCFastNet
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.txtContent);
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnSend);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.nupFastPort);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtFastIP);
            this.Controls.Add(this.label1);
            this.Name = "UCFastNet";
            this.Size = new System.Drawing.Size(387, 190);
            this.Load += new System.EventHandler(this.UCFastNet_Load);
            ((System.ComponentModel.ISupportInitialize)(this.nupFastPort)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Label label3;
        private Button btnSend;
        private Button btnStart;
        private NumericUpDown nupFastPort;
        private Label label2;
        private TextBox txtFastIP;
        private Label label1;
        private Button btnStop;
        private RichTextBox txtContent;
    }
}
