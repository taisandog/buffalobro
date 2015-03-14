using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Buffalo.Win32Kernel;
using Buffalo.Kernel;
using Buffalo.Kernel.Win32;

namespace WordFilter
{
    public partial class FrmKeys : Form
    {
        public FrmKeys()
        {
            InitializeComponent();
        }

        private static FrmKeys _frm;
        /// <summary>
        /// 显示热键
        /// </summary>
        public static void ShowBox() 
        {
            if (_frm == null || _frm.IsDisposed) 
            {
                _frm = new FrmKeys();
            }
            _frm.Show();
            _frm.TopMost = true;
            _frm.TopMost = false;
        }

        private void FrmKeys_Load(object sender, EventArgs e)
        {
            BindKeys();
            InitKeys();
        }

        private void BindKeys() 
        {
            List<EnumInfo> lstModifiers = EnumUnit.GetEnumInfos(typeof(KeyModifiers));
            cmbModifiers.Items.Clear();
            List<ComboBoxItem> lstValue = new List<ComboBoxItem>();
            foreach (EnumInfo info in lstModifiers) 
            {
                string name = info.FieldName;
                if (name == "Control") 
                {
                    name = "Ctrl";
                }
                else if (name == "Windows" || name == "All") 
                {
                    continue;
                }
                ComboBoxItem item = new ComboBoxItem(name, (int)info.Value);
                lstValue.Add(item);
            }
            cmbModifiers.DisplayMember = "FieldName";
            cmbModifiers.ValueMember = "Value";
            cmbModifiers.DataSource = lstValue;
            

            List<EnumInfo> lstKeys = EnumUnit.GetEnumInfos(typeof(Keys));
            cmbKeys.Items.Clear();
            lstValue = new List<ComboBoxItem>();
            foreach (EnumInfo info in lstKeys)
            {
                int val = (int)info.Value;
                if (val >= 48 && val <= 120)
                {
                    string name = info.FieldName;
                    if (name.Length == 2 && name[0] == 'D')
                    {
                        name = name.Substring(1);
                    }
                    ComboBoxItem item = new ComboBoxItem(name, (int)info.Value);
                    lstValue.Add(item);
                }
            }
            cmbKeys.DisplayMember = "FieldName";
            cmbKeys.ValueMember = "Value";
            cmbKeys.DataSource = lstValue;
            
        }

        /// <summary>
        /// 初始化热键
        /// </summary>
        private void InitKeys() 
        {
            ConfigSave config = Program.MainForm.Config;
            int modifiers = (int)config.Modifiers; ;
            if (modifiers == 0)
            {
                cmbModifiers.SelectedIndex = 0;
            }
            else
            {
                cmbModifiers.SelectedValue = (int)config.Modifiers;
            }
            cmbKeys.SelectedValue = (int)config.HotKey;
            txtSide.Value = config.Side;
            txtShow.Value = config.ShowTime;
            chkListen.Checked = config.ListenClipboard;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            ConfigSave config = Program.MainForm.Config;

            config.Modifiers = (KeyModifiers)cmbModifiers.SelectedValue;
            config.HotKey = (Keys)cmbKeys.SelectedValue;
            config.Side = (int)txtSide.Value;
            config.ShowTime = (int)txtShow.Value;
            config.ListenClipboard = chkListen.Checked;
            Program.MainForm.ReSetConfig();
            this.Close();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cmbKeys_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}