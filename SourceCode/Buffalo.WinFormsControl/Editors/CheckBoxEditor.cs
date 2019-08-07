using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Buffalo.WinFormsControl.Editors
{
    /// <summary>
    /// �ı���ı༭��
    /// </summary>
    [ToolboxItem(true)]
    public partial class CheckBoxEditor : EditorBase
    {
        public CheckBoxEditor()
        {
            InitializeComponent();
        }

        private void CheckBoxEditor_Load(object sender, EventArgs e)
        {
            
        }
        /// <summary>
        /// ��ť��ʽ����
        /// </summary>
        public OnOffButtonType OnOffType
        {
            get { return chkValue.OnOffType; }
            set 
            { 
                chkValue.OnOffType = value;

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
        /// �Ƿ�ѡ��
        /// </summary>
        public bool Checked 
        {
            get 
            {
                return chkValue.Checked;
            }
            set 
            {
                chkValue.Checked = value;
            }
        }
        /// <summary>
        /// ֵ
        /// </summary>
        public override object Value
        {
            get
            {
                return Checked;
            }
            set
            {
                string str = value as string;
                if (str!=null) 
                {
                    if (str == "1") 
                    {
                        Checked = true;
                        return;
                    }
                    if (str == "0")
                    {
                        Checked = false;
                        return;
                    }
                }
                Checked = Convert.ToBoolean(value);

            }
        }

        public override void Reset()
        {
            Checked = false;
        }

        public override Label Lable
        {
            get
            {
                return labSummary;
            }
        }

        
        private void chkValue_CheckedChanged(object sender, EventArgs e)
        {
            
        }

        private void chkValue_CheckStateChanged(object sender, EventArgs e)
        {
            DoValueChange(chkValue.Checked);
        }


    }
}
