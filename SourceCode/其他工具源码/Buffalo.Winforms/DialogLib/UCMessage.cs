using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;

using System.Text;

using System.Windows.Forms;

namespace Buffalo.Winforms.DialogLib
{
    public partial class UCMessage : FormModelBase
    {
        public UCMessage():base()
        {
            InitializeComponent();
            
        }
        public override void OnUIInit()
        {
            ParentForm.AcceptButton = btnOK;
            ParentForm.KeyPreview = true;
            base.OnUIInit();
        }
        /// <summary>
        /// 显示信息框
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public void SetMessage(string text)
        {
            labText.Text = text;
            pbLogo.Image = Resource.infoLogo;
        }
        /// <summary>
        /// 显示错误框
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public void SetError(string error)
        {
            labText.Text = error;
            pbLogo.Image = Resource.errorLogo;
        }
        /// <summary>
        /// 显示成功框
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public void SetSuccess(string error)
        {
            labText.Text = error;
            pbLogo.Image = Resource.successlogo;
        }
        private void btnOK_Click(object sender, EventArgs e)
        {
            DialogClose(DialogResult.OK);
        }
    }
}
