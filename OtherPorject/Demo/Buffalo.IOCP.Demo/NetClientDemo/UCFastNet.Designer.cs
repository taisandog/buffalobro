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
            label3 = new Label();
            btnSend = new Button();
            btnStart = new Button();
            nupFastPort = new NumericUpDown();
            label2 = new Label();
            txtFastIP = new TextBox();
            label1 = new Label();
            btnStop = new Button();
            txtContent = new RichTextBox();
            ((System.ComponentModel.ISupportInitialize)nupFastPort).BeginInit();
            SuspendLayout();
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(13, 82);
            label3.Name = "label3";
            label3.Size = new Size(35, 17);
            label3.TabIndex = 15;
            label3.Text = "消息:";
            // 
            // btnSend
            // 
            btnSend.Location = new Point(273, 56);
            btnSend.Name = "btnSend";
            btnSend.Size = new Size(86, 43);
            btnSend.TabIndex = 13;
            btnSend.Text = "发消息";
            btnSend.UseVisualStyleBackColor = true;
            btnSend.Click += btnSend_Click;
            // 
            // btnStart
            // 
            btnStart.Location = new Point(169, 105);
            btnStart.Name = "btnStart";
            btnStart.Size = new Size(86, 31);
            btnStart.TabIndex = 12;
            btnStart.Text = "连接";
            btnStart.UseVisualStyleBackColor = true;
            btnStart.Click += btnStart_Click;
            // 
            // nupFastPort
            // 
            nupFastPort.Location = new Point(186, 14);
            nupFastPort.Maximum = new decimal(new int[] { 65535, 0, 0, 0 });
            nupFastPort.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            nupFastPort.Name = "nupFastPort";
            nupFastPort.Size = new Size(86, 23);
            nupFastPort.TabIndex = 11;
            nupFastPort.Value = new decimal(new int[] { 8587, 0, 0, 0 });
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(169, 17);
            label2.Name = "label2";
            label2.Size = new Size(11, 17);
            label2.TabIndex = 10;
            label2.Text = ":";
            // 
            // txtFastIP
            // 
            txtFastIP.Location = new Point(54, 14);
            txtFastIP.Name = "txtFastIP";
            txtFastIP.Size = new Size(109, 23);
            txtFastIP.TabIndex = 9;
            txtFastIP.Text = "localhost";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(13, 17);
            label1.Name = "label1";
            label1.Size = new Size(35, 17);
            label1.TabIndex = 8;
            label1.Text = "地址:";
            // 
            // btnStop
            // 
            btnStop.Location = new Point(273, 105);
            btnStop.Name = "btnStop";
            btnStop.Size = new Size(86, 31);
            btnStop.TabIndex = 16;
            btnStop.Text = "停止";
            btnStop.UseVisualStyleBackColor = true;
            btnStop.Click += btnStop_Click;
            // 
            // txtContent
            // 
            txtContent.Location = new Point(54, 43);
            txtContent.Name = "txtContent";
            txtContent.Size = new Size(218, 56);
            txtContent.TabIndex = 17;
            txtContent.Text = "";
            // 
            // UCFastNet
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(txtContent);
            Controls.Add(btnStop);
            Controls.Add(label3);
            Controls.Add(btnSend);
            Controls.Add(btnStart);
            Controls.Add(nupFastPort);
            Controls.Add(label2);
            Controls.Add(txtFastIP);
            Controls.Add(label1);
            Name = "UCFastNet";
            Size = new Size(387, 190);
            Load += UCFastNet_Load;
            ((System.ComponentModel.ISupportInitialize)nupFastPort).EndInit();
            ResumeLayout(false);
            PerformLayout();
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
