using System.Windows.Forms;

namespace Buffalo.Winforms.DialogLib
{
    partial class InputModel
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InputModel));
            this.txtText = new System.Windows.Forms.TextBox();
            this.btnOK = new Buffalo.Winforms.RoundButton();
            this.btnCancel = new Buffalo.Winforms.RoundButton();
            this.labText = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.btnOK)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnCancel)).BeginInit();
            this.SuspendLayout();
            // 
            // txtText
            // 
            this.txtText.Location = new System.Drawing.Point(120, 32);
            this.txtText.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtText.Name = "txtText";
            this.txtText.Size = new System.Drawing.Size(357, 29);
            this.txtText.TabIndex = 0;
            // 
            // btnOK
            // 
            this.btnOK.BackImgColor = System.Drawing.Color.Transparent;
            this.btnOK.BoderColor = System.Drawing.SystemColors.ControlDarkDark;
            this.btnOK.ButtonEnable = true;
            this.btnOK.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnOK.DisableColor = System.Drawing.Color.LightGray;
            this.btnOK.DisableImage = ((System.Drawing.Image)(resources.GetObject("btnOK.DisableImage")));
            this.btnOK.DownColor = System.Drawing.SystemColors.ControlDarkDark;
            this.btnOK.DownImage = ((System.Drawing.Image)(resources.GetObject("btnOK.DownImage")));
            this.btnOK.EnterColor = System.Drawing.Color.WhiteSmoke;
            this.btnOK.EnterImage = ((System.Drawing.Image)(resources.GetObject("btnOK.EnterImage")));
            this.btnOK.FlatAppearance.BorderSize = 0;
            this.btnOK.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnOK.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnOK.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOK.Font = new System.Drawing.Font("微软雅黑", 14F);
            this.btnOK.ForeColor = System.Drawing.Color.Black;
            this.btnOK.Image = ((System.Drawing.Image)(resources.GetObject("btnOK.Image")));
            this.btnOK.Location = new System.Drawing.Point(204, 87);
            this.btnOK.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnOK.MatrixRound = 20;
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(128, 46);
            this.btnOK.TabIndex = 12;
            this.btnOK.Text = "确定";
            this.btnOK.UpColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(225)))), ((int)(((byte)(225)))));
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.BackImgColor = System.Drawing.Color.Transparent;
            this.btnCancel.BoderColor = System.Drawing.SystemColors.ControlDarkDark;
            this.btnCancel.ButtonEnable = true;
            this.btnCancel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCancel.DisableColor = System.Drawing.Color.LightGray;
            this.btnCancel.DisableImage = ((System.Drawing.Image)(resources.GetObject("btnCancel.DisableImage")));
            this.btnCancel.DownColor = System.Drawing.SystemColors.ControlDarkDark;
            this.btnCancel.DownImage = ((System.Drawing.Image)(resources.GetObject("btnCancel.DownImage")));
            this.btnCancel.EnterColor = System.Drawing.Color.WhiteSmoke;
            this.btnCancel.EnterImage = ((System.Drawing.Image)(resources.GetObject("btnCancel.EnterImage")));
            this.btnCancel.FlatAppearance.BorderSize = 0;
            this.btnCancel.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnCancel.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Font = new System.Drawing.Font("微软雅黑", 14F);
            this.btnCancel.ForeColor = System.Drawing.Color.Black;
            this.btnCancel.Image = ((System.Drawing.Image)(resources.GetObject("btnCancel.Image")));
            this.btnCancel.Location = new System.Drawing.Point(349, 87);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnCancel.MatrixRound = 20;
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(128, 46);
            this.btnCancel.TabIndex = 13;
            this.btnCancel.Text = "取消";
            this.btnCancel.UpColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(225)))), ((int)(((byte)(225)))));
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // labText
            // 
            this.labText.Location = new System.Drawing.Point(3, 35);
            this.labText.Name = "labText";
            this.labText.Size = new System.Drawing.Size(110, 21);
            this.labText.TabIndex = 14;
            this.labText.Text = "请输入:";
            this.labText.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // CDMSInputModel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.labText);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.txtText);
            this.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "CDMSInputModel";
            this.Size = new System.Drawing.Size(510, 155);
            ((System.ComponentModel.ISupportInitialize)(this.btnOK)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnCancel)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private TextBox txtText;
        private RoundButton btnOK;
        private RoundButton btnCancel;
        private System.Windows.Forms.Label labText;
    }
}
