namespace NetClientDemo
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            messageBox1 = new Library.MessageBox();
            tbMain = new TabControl();
            tbFast = new TabPage();
            ucFastNet1 = new UCFastNet();
            tbWebSocket = new TabPage();
            ucWebSocket1 = new UCWebSocket();
            tbMain.SuspendLayout();
            tbFast.SuspendLayout();
            tbWebSocket.SuspendLayout();
            SuspendLayout();
            // 
            // messageBox1
            // 
            messageBox1.Dock = DockStyle.Fill;
            messageBox1.Font = new Font("宋体", 10F, FontStyle.Regular, GraphicsUnit.Point);
            messageBox1.Location = new Point(0, 224);
            messageBox1.Name = "messageBox1";
            messageBox1.ShowError = true;
            messageBox1.ShowLog = true;
            messageBox1.ShowWarning = true;
            messageBox1.Size = new Size(752, 334);
            messageBox1.TabIndex = 0;
            // 
            // tbMain
            // 
            tbMain.Controls.Add(tbFast);
            tbMain.Controls.Add(tbWebSocket);
            tbMain.Dock = DockStyle.Top;
            tbMain.Location = new Point(0, 0);
            tbMain.Name = "tbMain";
            tbMain.SelectedIndex = 0;
            tbMain.Size = new Size(752, 224);
            tbMain.TabIndex = 1;
            // 
            // tbFast
            // 
            tbFast.Controls.Add(ucFastNet1);
            tbFast.Location = new Point(4, 26);
            tbFast.Name = "tbFast";
            tbFast.Padding = new Padding(3);
            tbFast.Size = new Size(744, 194);
            tbFast.TabIndex = 0;
            tbFast.Text = "Fast";
            tbFast.UseVisualStyleBackColor = true;
            // 
            // ucFastNet1
            // 
            ucFastNet1.Dock = DockStyle.Fill;
            ucFastNet1.Location = new Point(3, 3);
            ucFastNet1.Messbox = null;
            ucFastNet1.Name = "ucFastNet1";
            ucFastNet1.Size = new Size(738, 188);
            ucFastNet1.TabIndex = 0;
            ucFastNet1.Load += ucFastNet1_Load;
            // 
            // tbWebSocket
            // 
            tbWebSocket.Controls.Add(ucWebSocket1);
            tbWebSocket.Location = new Point(4, 26);
            tbWebSocket.Name = "tbWebSocket";
            tbWebSocket.Padding = new Padding(3);
            tbWebSocket.Size = new Size(744, 194);
            tbWebSocket.TabIndex = 1;
            tbWebSocket.Text = "WebSocket";
            tbWebSocket.UseVisualStyleBackColor = true;
            // 
            // ucWebSocket1
            // 
            ucWebSocket1.Dock = DockStyle.Fill;
            ucWebSocket1.Location = new Point(3, 3);
            ucWebSocket1.Messbox = null;
            ucWebSocket1.Name = "ucWebSocket1";
            ucWebSocket1.Size = new Size(738, 188);
            ucWebSocket1.TabIndex = 0;
            ucWebSocket1.Load += ucWebSocket1_Load;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(752, 558);
            Controls.Add(messageBox1);
            Controls.Add(tbMain);
            Name = "Form1";
            Text = "Form1";
            FormClosing += Form1_FormClosing;
            Load += Form1_Load;
            tbMain.ResumeLayout(false);
            tbFast.ResumeLayout(false);
            tbWebSocket.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Library.MessageBox messageBox1;
        private TabControl tbMain;
        private TabPage tbFast;
        private TabPage tbWebSocket;
        private UCFastNet ucFastNet1;
        private UCWebSocket ucWebSocket1;
    }
}