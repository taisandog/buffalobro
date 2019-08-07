namespace Buffalo.Winforms.DialogLib
{
    partial class UCConfirm
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
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UCConfirm));
            this.panel3 = new System.Windows.Forms.Panel();
            this.pbLogo = new System.Windows.Forms.PictureBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnYes = new RoundButton();
            this.btnNo = new RoundButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.labText = new System.Windows.Forms.Label();
            this.panel5 = new System.Windows.Forms.Panel();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbLogo)).BeginInit();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btnYes)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnNo)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel5.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.pbLogo);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(169, 241);
            this.panel3.TabIndex = 8;
            // 
            // pbLogo
            // 
            this.pbLogo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pbLogo.Image = Resource.questionLogo;
            this.pbLogo.Location = new System.Drawing.Point(0, 0);
            this.pbLogo.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.pbLogo.Name = "pbLogo";
            this.pbLogo.Size = new System.Drawing.Size(169, 241);
            this.pbLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pbLogo.TabIndex = 1;
            this.pbLogo.TabStop = false;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btnYes);
            this.panel2.Controls.Add(this.btnNo);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel2.Location = new System.Drawing.Point(278, 0);
            this.panel2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(413, 83);
            this.panel2.TabIndex = 1;
            // 
            // btnYes
            // 
            this.btnYes.BackImgColor = System.Drawing.Color.Transparent;
            this.btnYes.BoderColor = System.Drawing.SystemColors.ControlDarkDark;
            this.btnYes.ButtonEnable = true;
            this.btnYes.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnYes.DisableColor = System.Drawing.Color.LightGray;
            this.btnYes.DisableImage = ((System.Drawing.Image)(resources.GetObject("btnYes.DisableImage")));
            this.btnYes.DownColor = System.Drawing.SystemColors.ControlDark;
            this.btnYes.DownImage = ((System.Drawing.Image)(resources.GetObject("btnYes.DownImage")));
            this.btnYes.EnterColor = System.Drawing.Color.WhiteSmoke;
            this.btnYes.EnterImage = ((System.Drawing.Image)(resources.GetObject("btnYes.EnterImage")));
            this.btnYes.FlatAppearance.BorderSize = 0;
            this.btnYes.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnYes.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnYes.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnYes.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.btnYes.ForeColor = System.Drawing.Color.Black;
            this.btnYes.Image = ((System.Drawing.Image)(resources.GetObject("btnYes.Image")));
            this.btnYes.Location = new System.Drawing.Point(21, 10);
            this.btnYes.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnYes.MatrixRound = 20;
            this.btnYes.Name = "btnYes";
            this.btnYes.Size = new System.Drawing.Size(188, 61);
            this.btnYes.TabIndex = 2;
            this.btnYes.Text = "确定";
            this.btnYes.UpColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(225)))), ((int)(((byte)(225)))));
            this.btnYes.UseVisualStyleBackColor = true;
            this.btnYes.Click += new System.EventHandler(this.btnYes_Click);
            // 
            // btnNo
            // 
            this.btnNo.BackImgColor = System.Drawing.Color.Transparent;
            this.btnNo.BoderColor = System.Drawing.SystemColors.ControlDarkDark;
            this.btnNo.ButtonEnable = true;
            this.btnNo.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnNo.DisableColor = System.Drawing.Color.LightGray;
            this.btnNo.DisableImage = ((System.Drawing.Image)(resources.GetObject("btnNo.DisableImage")));
            this.btnNo.DownColor = System.Drawing.SystemColors.ControlDark;
            this.btnNo.DownImage = ((System.Drawing.Image)(resources.GetObject("btnNo.DownImage")));
            this.btnNo.EnterColor = System.Drawing.Color.WhiteSmoke;
            this.btnNo.EnterImage = ((System.Drawing.Image)(resources.GetObject("btnNo.EnterImage")));
            this.btnNo.FlatAppearance.BorderSize = 0;
            this.btnNo.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnNo.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnNo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNo.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.btnNo.ForeColor = System.Drawing.Color.Black;
            this.btnNo.Image = ((System.Drawing.Image)(resources.GetObject("btnNo.Image")));
            this.btnNo.Location = new System.Drawing.Point(217, 10);
            this.btnNo.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnNo.MatrixRound = 20;
            this.btnNo.Name = "btnNo";
            this.btnNo.Size = new System.Drawing.Size(188, 61);
            this.btnNo.TabIndex = 1;
            this.btnNo.Text = "取消";
            this.btnNo.UpColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(225)))), ((int)(((byte)(225)))));
            this.btnNo.UseVisualStyleBackColor = true;
            this.btnNo.Click += new System.EventHandler(this.btnNo_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 239);
            this.panel1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(691, 83);
            this.panel1.TabIndex = 7;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.labText);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(169, 0);
            this.panel4.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(522, 241);
            this.panel4.TabIndex = 9;
            // 
            // labText
            // 
            this.labText.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labText.Font = new System.Drawing.Font("微软雅黑", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labText.ForeColor = System.Drawing.Color.Black;
            this.labText.Location = new System.Drawing.Point(0, 0);
            this.labText.Name = "labText";
            this.labText.Size = new System.Drawing.Size(522, 241);
            this.labText.TabIndex = 6;
            this.labText.Text = "消息";
            this.labText.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.panel4);
            this.panel5.Controls.Add(this.panel3);
            this.panel5.Location = new System.Drawing.Point(0, 0);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(691, 241);
            this.panel5.TabIndex = 10;
            // 
            // UCConfirm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel5);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "UCConfirm";
            this.Size = new System.Drawing.Size(691, 322);
            this.panel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbLogo)).EndInit();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.btnYes)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnNo)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.PictureBox pbLogo;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Label labText;
        private RoundButton btnYes;
        private RoundButton btnNo;
        private System.Windows.Forms.Panel panel5;

    }
}
