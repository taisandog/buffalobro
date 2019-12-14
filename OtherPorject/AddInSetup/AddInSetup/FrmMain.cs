using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;
using Buffalo.Kernel;

namespace AddInSetup
{
    public partial class FrmMain : Form
    {
        public FrmMain()
        {
            InitializeComponent();
        }
        private ConfigLoader _loader;

        public ConfigLoader CurrentLoader
        {
            get { return _loader; }
        }
        /// <summary>
        /// 刷新列表
        /// </summary>
        private void RefreashDisplay() 
        {
            gvAddIns.DataSource = null;
            if (_loader.AddInInfos != null && _loader.AddInInfos.Count > 0) 
            {
                gvAddIns.DataSource = _loader.AddInInfos;
            }
            if (_loader.DllVerInfos != null && _loader.DllVerInfos.Count > 0)
            {
                gvDllVer.DataSource = _loader.DllVerInfos;
            }
        }
        Thread _thd;
        private void FrmMain_Load(object sender, EventArgs e)
        {

            this.Text = ToolVersionInfo.ToolVerInfo;
            tsNew.Visible = false;
            _loader = new ConfigLoader();
            _loader.LoadConfig();
            gvAddIns.AutoGenerateColumns = false;
            gvDllVer.AutoGenerateColumns = false;
            
            RefreashDisplay();
            ClearSelect(gvAddIns);
            ClearSelect(gvDllVer);
            LoadDocItem();
            _thd = new Thread(new ThreadStart(CheckUpdate));
            _thd.Start();
        }

        private void CheckUpdate()
        {
            UpdateInfo info = new UpdateInfo();
            DateTime dt = _loader.Version;
            DateTime lastestDate = UpdateInfo.GetLastestVersion();
            if (dt < lastestDate)
            {
                if (this.IsDisposed) 
                {
                    return;
                }
                this.Invoke((EventHandler)delegate
                {
                    tsNew.Visible = true;
                    timCheck.Start();
                });
                
            }
            
        }

        private void gvAddIns_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string colName = gvAddIns.Columns[e.ColumnIndex].Name;
            if (colName == "ColState")
            {
                AddInInfo info = gvAddIns.Rows[e.RowIndex].DataBoundItem as AddInInfo;
                if (info == null)
                {
                    return;
                }
                if (info.IsSetup)
                {
                    string err = info.UnInstall();
                    if (err == null)
                    {
                        MessageBox.Show("卸载完毕", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("卸载错误:" + err, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    string err = info.Install();
                    if (!info.IsVSIX)
                    {
                        if (err == null)
                        {
                            MessageBox.Show("安装完毕", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("安装错误:" + err, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
                RefreashDisplay();

            }
        }

        private void gvAddIns_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                gvAddIns.Rows[e.RowIndex].Cells[e.ColumnIndex].Selected = true;
            }
        }

        private void gvDllVer_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string colName = gvDllVer.Columns[e.ColumnIndex].Name;
            if (e.ColumnIndex < 0 || e.RowIndex < 0) 
            {
                return;
            }
            if (colName == "ColPut")
            {
                DllVerInfo info = gvDllVer.Rows[e.RowIndex].DataBoundItem as DllVerInfo;
                if (info == null)
                {
                    return;
                }
                FrmPutFile.ShowForm(info);
            }
        }

        private void gvDllVer_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                gvDllVer.Rows[e.RowIndex].Cells[e.ColumnIndex].Selected = true;
            }
        }

        private void tsExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void tsODAC_Click(object sender, EventArgs e)
        {
            Process.Start("https://www.oracle.com/technetwork/topics/dotnet/downloads/index.html");
        }

        private void gvAddIns_MouseLeave(object sender, EventArgs e)
        {
            ClearSelect(gvAddIns);
        }

        private void ClearSelect(DataGridView gv) 
        {
            foreach (DataGridViewCell cell in gv.SelectedCells)
            {
                cell.Selected = false;
            }
        }

        private void gvDllVer_MouseLeave(object sender, EventArgs e)
        {
            ClearSelect(gvDllVer);
        }

       

        private void tsIndex_Click(object sender, EventArgs e)
        {
            string page = System.Configuration.ConfigurationManager.AppSettings["App.Index"];
            Process.Start(page);
        }

        private void tsVedio_Click(object sender, EventArgs e)
        {
            string page = System.Configuration.ConfigurationManager.AppSettings["App.TechVedio"];
            Process.Start(page);
            
        }

        private void tsNew_Click(object sender, EventArgs e)
        {
            using (FrmUpdate frm = new FrmUpdate())
            {
                timCheck.Stop();
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    string path = CommonMethods.GetBaseRoot();
                    Process.Start(path);
                    this.Close();
                    return;
                }
                timCheck.Start();
            }
        }

        private void timCheck_Tick(object sender, EventArgs e)
        {
            timCheck.Stop();
            if (tsNew.Text[0]==' ')
            {
                tsNew.Text = "有新版本!!";
            }
            else
            {
                tsNew.Text = "        ";
            }
            timCheck.Start();
        }

        private void FrmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            timCheck.Stop();
        }

        private void tsConn_Click(object sender, EventArgs e)
        {
            using (FrmConnString frm = new FrmConnString())
            {
                frm.ShowDialog();
            }
        }

        private void tsDoc_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem item = sender as ToolStripMenuItem;
            if (item == null)
            {
                return;
            }
            string path = ConfigLoader.BasePath + (item.Tag as string);
            Process.Start(path);
        }

        private void LoadDocItem()
        {
            int index = 0;
            List<ToolStripMenuItem> items = new List<ToolStripMenuItem>();
            foreach (HelpDocItem item in _loader.LstDocItems)
            {
                ToolStripMenuItem ts = new ToolStripMenuItem();
                ts.Name = "tsDoc_"+index;
                ts.Size = new System.Drawing.Size(180, 22);
                ts.Text = item.Title;
                ts.Tag = item.Path;
                ts.Click += new System.EventHandler(this.tsDoc_Click);
                items.Add(ts);
            }
            this.tsHelp.DropDownItems.AddRange(items.ToArray());
        }
    }
}