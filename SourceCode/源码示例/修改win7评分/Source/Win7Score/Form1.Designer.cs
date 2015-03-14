namespace Win7Score
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
            this.label1 = new System.Windows.Forms.Label();
            this.txtSystemScore = new System.Windows.Forms.TextBox();
            this.txtMemoryScore = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtCpuScore = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtGraphicsScore = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtGamingScore = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtDiskScore = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.btnSubmit = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "总分:";
            // 
            // txtSystemScore
            // 
            this.txtSystemScore.Location = new System.Drawing.Point(54, 10);
            this.txtSystemScore.Name = "txtSystemScore";
            this.txtSystemScore.Size = new System.Drawing.Size(100, 21);
            this.txtSystemScore.TabIndex = 1;
            // 
            // txtMemoryScore
            // 
            this.txtMemoryScore.Location = new System.Drawing.Point(54, 37);
            this.txtMemoryScore.Name = "txtMemoryScore";
            this.txtMemoryScore.Size = new System.Drawing.Size(100, 21);
            this.txtMemoryScore.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 40);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "内存:";
            // 
            // txtCpuScore
            // 
            this.txtCpuScore.Location = new System.Drawing.Point(54, 64);
            this.txtCpuScore.Name = "txtCpuScore";
            this.txtCpuScore.Size = new System.Drawing.Size(100, 21);
            this.txtCpuScore.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 67);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(47, 12);
            this.label3.TabIndex = 4;
            this.label3.Text = "处理器:";
            // 
            // txtGraphicsScore
            // 
            this.txtGraphicsScore.Location = new System.Drawing.Point(54, 91);
            this.txtGraphicsScore.Name = "txtGraphicsScore";
            this.txtGraphicsScore.Size = new System.Drawing.Size(100, 21);
            this.txtGraphicsScore.TabIndex = 7;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(13, 94);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(35, 12);
            this.label4.TabIndex = 6;
            this.label4.Text = "图形:";
            // 
            // txtGamingScore
            // 
            this.txtGamingScore.Location = new System.Drawing.Point(54, 118);
            this.txtGamingScore.Name = "txtGamingScore";
            this.txtGamingScore.Size = new System.Drawing.Size(100, 21);
            this.txtGamingScore.TabIndex = 9;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(13, 121);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(35, 12);
            this.label5.TabIndex = 8;
            this.label5.Text = "游戏:";
            // 
            // txtDiskScore
            // 
            this.txtDiskScore.Location = new System.Drawing.Point(54, 145);
            this.txtDiskScore.Name = "txtDiskScore";
            this.txtDiskScore.Size = new System.Drawing.Size(100, 21);
            this.txtDiskScore.TabIndex = 11;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(13, 148);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(35, 12);
            this.label6.TabIndex = 10;
            this.label6.Text = "硬盘:";
            // 
            // btnSubmit
            // 
            this.btnSubmit.Location = new System.Drawing.Point(79, 199);
            this.btnSubmit.Name = "btnSubmit";
            this.btnSubmit.Size = new System.Drawing.Size(75, 23);
            this.btnSubmit.TabIndex = 12;
            this.btnSubmit.Text = "修改";
            this.btnSubmit.UseVisualStyleBackColor = true;
            this.btnSubmit.Click += new System.EventHandler(this.btnSubmit_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.btnSubmit);
            this.Controls.Add(this.txtDiskScore);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.txtGamingScore);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtGraphicsScore);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtCpuScore);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtMemoryScore);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtSystemScore);
            this.Controls.Add(this.label1);
            this.Name = "Form1";
            this.Text = "Win7评分修改";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtSystemScore;
        private System.Windows.Forms.TextBox txtMemoryScore;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtCpuScore;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtGraphicsScore;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtGamingScore;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtDiskScore;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btnSubmit;
    }
}

