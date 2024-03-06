namespace NetClientDemo
{
    partial class UCWebSocket
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
            txtContent = new RichTextBox();
            btnStop = new Button();
            label3 = new Label();
            btnSend = new Button();
            btnStart = new Button();
            txtFastIP = new TextBox();
            label1 = new Label();
            label2 = new Label();
            nupFastPort = new NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)nupFastPort).BeginInit();
            SuspendLayout();
            // 
            // txtContent
            // 
            txtContent.Location = new Point(57, 40);
            txtContent.Name = "txtContent";
            txtContent.Size = new Size(218, 56);
            txtContent.TabIndex = 26;
            txtContent.Text = "";
            // 
            // btnStop
            // 
            btnStop.Location = new Point(276, 102);
            btnStop.Name = "btnStop";
            btnStop.Size = new Size(86, 31);
            btnStop.TabIndex = 25;
            btnStop.Text = "停止";
            btnStop.UseVisualStyleBackColor = true;
            btnStop.Click += btnStop_Click;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(16, 79);
            label3.Name = "label3";
            label3.Size = new Size(35, 17);
            label3.TabIndex = 24;
            label3.Text = "消息:";
            // 
            // btnSend
            // 
            btnSend.Location = new Point(276, 53);
            btnSend.Name = "btnSend";
            btnSend.Size = new Size(86, 43);
            btnSend.TabIndex = 23;
            btnSend.Text = "发消息";
            btnSend.UseVisualStyleBackColor = true;
            btnSend.Click += btnSend_Click;
            // 
            // btnStart
            // 
            btnStart.Location = new Point(172, 102);
            btnStart.Name = "btnStart";
            btnStart.Size = new Size(86, 31);
            btnStart.TabIndex = 22;
            btnStart.Text = "连接";
            btnStart.UseVisualStyleBackColor = true;
            btnStart.Click += btnStart_Click;
            // 
            // txtFastIP
            // 
            txtFastIP.Location = new Point(57, 11);
            txtFastIP.Name = "txtFastIP";
            txtFastIP.Size = new Size(201, 23);
            txtFastIP.TabIndex = 19;
            txtFastIP.Text = "wss://localhost:8589";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(16, 14);
            label1.Name = "label1";
            label1.Size = new Size(35, 17);
            label1.TabIndex = 18;
            label1.Text = "地址:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(259, 14);
            label2.Name = "label2";
            label2.Size = new Size(11, 17);
            label2.TabIndex = 20;
            label2.Text = ":";
            label2.Visible = false;
            // 
            // nupFastPort
            // 
            nupFastPort.Location = new Point(276, 11);
            nupFastPort.Maximum = new decimal(new int[] { 65535, 0, 0, 0 });
            nupFastPort.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            nupFastPort.Name = "nupFastPort";
            nupFastPort.Size = new Size(86, 23);
            nupFastPort.TabIndex = 21;
            nupFastPort.Value = new decimal(new int[] { 8588, 0, 0, 0 });
            nupFastPort.Visible = false;
            // 
            // UCWebSocket
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
            Name = "UCWebSocket";
            Size = new Size(379, 150);
            ((System.ComponentModel.ISupportInitialize)nupFastPort).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private RichTextBox txtContent;
        private Button btnStop;
        private Label label3;
        private Button btnSend;
        private Button btnStart;
        private NumericUpDown nupFastPort;
        private Label label2;
        private TextBox txtFastIP;
        private Label label1;
    }
}
