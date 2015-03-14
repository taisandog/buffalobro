using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace MoveDataLink
{
    public partial class FrmDefault : Form
    {
        public FrmDefault()
        {
            InitializeComponent();
        }

        private void FrmDefault_Load(object sender, EventArgs e)
        {
            Bind();
            lbPaths_Resize(lbPaths, new EventArgs());
        }

        /// <summary>
        /// ÏÔÊ¾Ñ¡Ôñ¿ò
        /// </summary>
        /// <returns></returns>
        public static string ShowPath() 
        {
            using (FrmDefault frm = new FrmDefault()) 
            {
                if (frm.ShowDialog() == DialogResult.OK) 
                {
                    return frm._selectedPath;
                }
            }
            return null;
        }


        private void Bind() 
        {
            List<SpecialPath> lst=SpecialPath.DefaultPath;
            lbPaths.Items.Clear();
            foreach (SpecialPath path in lst) 
            {
                ListViewItem item = new ListViewItem(new string[] { path.Summary, path.Path });
                lbPaths.Items.Add(item);
            }
        }

        private string _selectedPath;

        private void lbPaths_DoubleClick(object sender, EventArgs e)
        {
            if (lbPaths.SelectedIndices.Count > 0) 
            {
                int index = lbPaths.SelectedIndices[0];
                if (index >= 0) 
                {
                    string path=lbPaths.Items[index].SubItems[1].Text;
                    _selectedPath = path;
                    this.DialogResult = DialogResult.OK;
                }
            }
        }

        private void lbPaths_Resize(object sender, EventArgs e)
        {
            lbPaths.Columns[0].Width = 150;
            int pathWidth = 0;
            if (lbPaths.Width > lbPaths.Columns[0].Width) 
            {
                pathWidth = lbPaths.Width - lbPaths.Columns[0].Width;
            }
            lbPaths.Columns[1].Width = pathWidth;
        }
    }
}