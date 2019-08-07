using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;

using System.Text;

using System.Windows.Forms;


namespace Buffalo.Winforms.DialogLib
{
    public partial class InputModel : FormModelBase
    {
        public InputModel()
        {
            InitializeComponent();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            DialogClose(DialogResult.OK);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogClose(DialogResult.Cancel);
        }

        /// <summary>
        /// 显示输入框
        /// </summary>
        /// <returns></returns>
        public static string ShowInput(string title,string label,char passwordChar='\0') 
        {
            using (FrmUIDialog frm = FrmUIDialog.UCGetForm<InputModel>())
            {
                frm.Text = title;
                InputModel content = frm.ContentControl as InputModel;
                content.labText.Text = label;
                if (passwordChar!='\0') 
                {
                    content.txtText.PasswordChar = passwordChar;
                }
                frm.StartPosition = FormStartPosition.CenterParent;
                if (frm.ShowDialogForm(null) == DialogResult.OK)
                {
                    InputModel iModel = frm.ContentControl as InputModel;
                    return iModel.txtText.Text;
                }
            }
            return null;
        }
    }
}
