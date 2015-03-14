using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Buffalo.Win32Kernel;
using Buffalo.Kernel;
namespace Buffalo.WinFormsControl.Editors
{
    /// <summary>
    /// 文本框的编辑器
    /// </summary>
    [ToolboxItem(true)]
    public partial class ComboBoxEditor : EditorBase
    {
        public ComboBoxEditor()
        {
            InitializeComponent();
        }

        private void ComboBoxEditor_Load(object sender, EventArgs e)
        {
            
        }

        private void pnlValue_Resize(object sender, EventArgs e)
        {
            int width = pnlValue.Width - 4;
            if (width > 0) 
            {
                cmbValue.Width = width;
            }
        }
        public override string  Text
        {
	        get 
	        {
                return cmbValue.Text;
	        }
	        set 
	        {
                cmbValue.Text = value;
	        }
        }

        public override int LableWidth
        {
            get
            {
                return pnlLable.Width;
            }
            set
            {
                pnlLable.Width = value;
            }
        }

        /// <summary>
        /// 值
        /// </summary>
        public override object Value
        {
            get
            {
                return cmbValue.SelectedValue;
            }
            set
            {
                if (value == null) 
                {
                    return;
                }
                cmbValue.SelectedValue=value;
            }
        }
        public override void Reset()
        {
            if (cmbValue.Items.Count > 0) 
            {
                cmbValue.SelectedIndex = 0;
            }
        }

        /// <summary>
        /// 标签字体
        /// </summary>
        public Font LableFont
        {
            get
            {
                return cmbValue.Font;
            }
            set
            {
                cmbValue.Font = value;
            }
        }

        public override Color ForeColor
        {
            get
            {
                return cmbValue.ForeColor;
            }
            set
            {
                cmbValue.ForeColor = value;
            }
        }


        public override Label Lable
        {
            get
            {
                return labSummary;
            }
        }


        

        private void cmbValue_SelectedIndexChanged(object sender, EventArgs e)
        {
            DoValueChange(cmbValue.SelectedValue);
        }

        /// <summary>
        /// 绑定值
        /// </summary>
        /// <param name="lstItem"></param>
        public void BindValue(List<ComboBoxItem> lstItem) 
        {
            cmbValue.DisplayMember = "Text";
            cmbValue.ValueMember = "Value";
            cmbValue.DataSource = lstItem;
        }


    }
}
