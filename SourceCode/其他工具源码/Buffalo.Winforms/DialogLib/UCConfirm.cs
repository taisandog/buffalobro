using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;

using System.Text;

using System.Windows.Forms;

namespace Buffalo.Winforms.DialogLib
{
    public partial class UCConfirm : FormModelBase
    {
        public UCConfirm()
        {
            InitializeComponent();
            
        }
        public override void OnUIInit()
        {
            ParentForm.AcceptButton = btnYes;
            ParentForm.CancelButton = btnNo;
            ParentForm.KeyPreview = true;
            base.OnUIInit();
        }
        private void btnYes_Click(object sender, EventArgs e)
        {
            this.ParentForm.DialogResult = DialogResult.Yes;
        }

        private void btnNo_Click(object sender, EventArgs e)
        {
            this.ParentForm.DialogResult = DialogResult.No;
        }

        /// <summary>
        /// 显示是否框
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public void SetConfirm(string text)
        {
            labText.Text = text;
        }
    }
}
