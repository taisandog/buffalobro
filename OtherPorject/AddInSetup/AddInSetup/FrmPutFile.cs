using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;

namespace AddInSetup
{
    public partial class FrmPutFile : Form
    {
        public FrmPutFile()
        {
            InitializeComponent();
        }
        DllVerInfo _selectVer = null;
        string _docLink = null;
        /// <summary>
        /// 显示输出窗体
        /// </summary>
        /// <param name="selectVer">选中的版本</param>
        public static void ShowForm(DllVerInfo selectVer) 
        {
            using (FrmPutFile frm = new FrmPutFile()) 
            {
                frm._selectVer = selectVer;
                frm.Text = "Buffalo for "+selectVer.VerName;
                if (!string.IsNullOrWhiteSpace(selectVer.HelpText))
                {
                    frm.labHelp.Visible = true;
                    frm.labHelp.Text = selectVer.HelpText;
                    frm._docLink = Path.Combine(ConfigLoader.BasePath, selectVer.HelpDoc);
                }
                else
                {
                    frm.labHelp.Visible = false;
                }
                frm.ShowDialog();
            }
        }

        private void btnFile_Click(object sender, EventArgs e)
        {
            if (fbd.ShowDialog() == DialogResult.OK) 
            {
                txtFile.Text = fbd.SelectedPath;
            }
        }

        private void FrmPutFile_Load(object sender, EventArgs e)
        {
            gvFile.AutoGenerateColumns = false;
            BindFiles();
            ClearSelect(gvFile);
        }

        private void BindFiles() 
        {
            gvFile.DataSource = null;
            List<DllItem> lstSource = new List<DllItem>();
            foreach(DllItem item in Program.CurrentMainForm.CurrentLoader.DllItems)
            {
                if(item.Ignore.ContainsKey(_selectVer.NetVersion))
                {
                    continue;
                }
                if (!item.ExistsFile(_selectVer))
                {
                    continue;
                }
                lstSource.Add(item);
            }
            gvFile.DataSource = lstSource;
            foreach (DataGridViewRow row in gvFile.Rows) 
            {
                DllItem item = row.DataBoundItem as DllItem;
                if (item == null) 
                {
                    continue;
                }
                row.Cells["ColState"].Value = item.Selected;
            }
        }

        private void gvFile_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                gvFile.Rows[e.RowIndex].Cells[e.ColumnIndex].Selected = true;
            }
        }

        private void gvFile_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex<0 )
            {
                return;
            }
            object oval = gvFile.Rows[e.RowIndex].Cells["ColState"].Value;

            bool val = false;
            if (oval != null) 
            {
                val = (bool)oval;
            }
            gvFile.Rows[e.RowIndex].Cells["ColState"].Value = !val;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.No;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string targetPath=txtFile.Text;
            if (string.IsNullOrEmpty(targetPath)) 
            {
                MessageBox.Show("请选择要输出的文件夹", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                if (!Directory.Exists(targetPath))
                {
                    Directory.CreateDirectory(targetPath);
                }
            }
            catch (Exception ex) 
            {
                MessageBox.Show(ex.ToString(), "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            foreach (DataGridViewRow row in gvFile.Rows)
            {
                bool selected = (bool)row.Cells["ColState"].Value;
                if (!selected)
                {
                    continue;
                }
                DllItem item = row.DataBoundItem as DllItem;
                if (item == null)
                {
                    continue;
                }
                try
                {
                    item.PutFiles(_selectVer, targetPath);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString(), "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            MessageBox.Show("复制完毕", "复制完毕", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void gvFile_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            foreach (DataGridViewRow row in gvFile.Rows)
            {
                DllItem item = row.DataBoundItem as DllItem;
                if (item == null)
                {
                    continue;
                }
                row.Cells["ColVersion"].Value = item.GetMainVersion(_selectVer);
            }
        }
        private void ClearSelect(DataGridView gv)
        {
            foreach (DataGridViewCell cell in gv.SelectedCells)
            {
                cell.Selected = false;
            }
        }

        private void gvFile_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
        {
            ClearSelect(gvFile);
        }

        private void labHelp_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string path = _docLink;
            Process.Start(path);
        }
    }
}