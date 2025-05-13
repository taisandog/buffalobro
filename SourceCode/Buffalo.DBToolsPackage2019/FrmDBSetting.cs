using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Buffalo.DBTools.HelperKernel;
using Buffalo.DB.DataBaseAdapter;
using Buffalo.DB.DbCommon;
using EnvDTE;
using Microsoft.VisualStudio.Modeling.Diagrams;
using Microsoft.VisualStudio.EnterpriseTools.ClassDesigner;
using Buffalo.Kernel;
using Buffalo.DBTools.DocSummary;
using Buffalo.DB.CommBase;
using Buffalo.Win32Kernel;
using Microsoft.VisualStudio.EnterpriseTools.ArtifactModel.Clr;
using Buffalo.DBTools.UIHelper;
using System.IO;
using Buffalo.Kernel.ZipUnit;

namespace Buffalo.DBTools
{
    public partial class FrmDBSetting : Form
    {
        public FrmDBSetting()
        {
            InitializeComponent();
            FillSummary();
        }

        private DBConfigInfo _info=new DBConfigInfo();

        public static DBConfigInfo GetDBConfigInfo(ClassDesignerInfo info,string dalNamespace) 
        {
            DBConfigInfo dbinfo = DBConfigInfo.LoadInfo(info);
            if (dbinfo == null)
            {
                using (FrmDBSetting frmSetting = new FrmDBSetting())
                {
                    frmSetting.Info.DbName = DBConfigInfo.GetDbName(info);
                    if (frmSetting.ShowDialog() != DialogResult.OK)
                    {
                        return null;
                    }
                    dbinfo = frmSetting.Info;

                    dbinfo.AppNamespace = dalNamespace+"."+dbinfo.DbType;
                    dbinfo.FileName = DBConfigInfo.GetFileName(info);
                    dbinfo.SaveConfig(dbinfo.FileName);
                    //StaticConnection.ClearCacheOperate(dbinfo);
                }
            }
            return dbinfo;
        }

        

        /// <summary>
        /// 数据库信息
        /// </summary>
        public DBConfigInfo Info
        {
            get { return _info; }
            set { _info = value; }
        }

        /// <summary>
        /// 初始化数据库类型
        /// </summary>
        private void InitDBType() 
        {
            cmbType.DisplayMember = "Text";
            cmbType.ValueMember = "Value";
            cmbType.DataSource = Generate3Tier.DataAccessTypes;
        }

        private void InitTiers() 
        {

            cmbTier.DisplayMember = "Text";
            cmbTier.ValueMember = "Value";
            cmbTier.DataSource = Generate3Tier.Tiers;
        }
        

        Size _maxSize = Size.Empty;
        private void FrmDBSetting_Load(object sender, EventArgs e)
        {
            
            _maxSize = this.Size;
            InitTiers();
            InitDBType();
            InitCacheType();
            InitLazyType();
            FillEdit();
            this.Text += ToolVersionInfo.ToolVerInfo;
            ShowOrHideCache(!string.IsNullOrEmpty(_info.CacheType));
        }
        /// <summary>
        /// 初始化缓存类型
        /// </summary>
        private void InitCacheType()
        {
            cmbCacheType.DisplayMember = "Text";
            cmbCacheType.ValueMember = "Value";
            cmbCacheType.DataSource = Generate3Tier.CacheTypes;
            
        }
        /// <summary>
        /// 初始化缓存类型
        /// </summary>
        private void InitLazyType()
        {
            cmbLazy.DisplayMember = "Description";
            cmbLazy.ValueMember = "Value";
            List<EnumInfo> lst = EnumUnit.GetEnumInfos(typeof(LazyType));
            cmbLazy.DataSource = lst;

        }
        /// <summary>
        /// 填充编辑项
        /// </summary>
        private void FillEdit() 
        {
            if (_info != null) 
            {
                if (!string.IsNullOrEmpty(_info.DbType))
                {
                    cmbType.SelectedValue = _info.DbType;
                }
                if (_info.Tier == 1 || _info.Tier == 3)
                {
                    cmbTier.SelectedValue = _info.Tier;
                }
                rtbConnstr.Text = _info.ConnectionString;
                chkAllDal.Checked = _info.IsAllDal;
                chkEntityToDirectory.Checked = _info.EntityToDirectory;
                rtbReadConnstr.Text = _info.ROConnectionString;
                //缓存
                if (!string.IsNullOrEmpty(_info.CacheType)) 
                {
                    cmbCacheType.SelectedValue = _info.CacheType;
                }
                ckbAll.Checked = _info.IsAllTable;
                cmbLazy.SelectedValue = _info.AllowLazy;
                txtCacheServer.Text = Info.CacheConnString;
            }
        }
        /// <summary>
        /// 填充注释设置
        /// </summary>
        private void FillSummary() 
        {
            List<EnumInfo> infos = EnumUnit.GetEnumInfos(typeof(SummaryShowItem));
            
            foreach (EnumInfo info in infos) 
            {
                if (info.FieldName != "All") 
                {
                    ComboBoxItem item = new ComboBoxItem(info.Description, (int)info.Value);
                    clbSummary.Items.Add(item);
                }
                
            }

        }

        /// <summary>
        /// 选中的关系图信息
        /// </summary>
        private ClassDesignerInfo _selectedClassDesigner;
        
        /// <summary>
        /// 显示配置框
        /// </summary>
        /// <param name="curProject"></param>
        /// <param name="docView"></param>
        /// <param name="dalNamespace"></param>
        public static void ShowConfig(ClassDesignerInfo desinfo, string dalNamespace) 
        {
            DBConfigInfo dbinfo = DBConfigInfo.LoadInfo(desinfo);
            using (FrmDBSetting frmSetting = new FrmDBSetting())
            {
                frmSetting._selectedClassDesigner = desinfo;
                if (dbinfo == null)
                {
                    dbinfo = new DBConfigInfo();
                    dbinfo.DbName = DBConfigInfo.GetDbName(desinfo);
                    dbinfo.SummaryShow = SummaryShowItem.All;
                    dbinfo.FileName = DBConfigInfo.GetFileName(desinfo);

                }
                frmSetting.Info= dbinfo;
                frmSetting.SummaryItem = dbinfo.SummaryShow;
                if (frmSetting.ShowDialog() != DialogResult.OK)
                {
                    return;
                }
                if (string.IsNullOrEmpty(dbinfo.AppNamespace)) 
                {
                    dbinfo.AppNamespace = dalNamespace + "." + dbinfo.DbType;
                }

                dbinfo.SaveConfig(dbinfo.FileName);
                //StaticConnection.ClearCacheOperate(dbinfo);
            }
            
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                if (FillInfo())
                {
                    this.DialogResult = DialogResult.OK;
                }
            }
            catch (Exception ex)
            {
                FrmCompileResault.ShowError(ex.ToString(), ToolVersionInfo.ToolVerInfo);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private SummaryShowItem _summaryItem;

        /// <summary>
        /// 设置注释选中状态
        /// </summary>
        private SummaryShowItem SummaryItem 
        {
            get 
            {
                int val = 0;
                for (int i = 0; i < clbSummary.Items.Count; i++) 
                {
                    ComboBoxItem item = clbSummary.Items[i] as ComboBoxItem;
                    
                    if (clbSummary.GetItemChecked(i) && item != null)
                    {
                        int curVal = Convert.ToInt32(item.Value);
                        val = val | curVal;
                    }
                }
                return (SummaryShowItem)val;
            }
            set 
            {
                _summaryItem = value;
                if (clbSummary != null)
                {
                    int val = (int)_summaryItem;
                    for (int i = 0; i < clbSummary.Items.Count; i++)
                    {
                        ComboBoxItem item = clbSummary.Items[i] as ComboBoxItem;
                        if (EnumUnit.ContainerValue(val, Convert.ToInt32(item.Value)))
                        {
                            clbSummary.SetItemChecked(i, true);
                        }

                    }
                }
                
            }
        }

        /// <summary>
        /// 填充信息
        /// </summary>
        /// <returns></returns>
        private bool FillInfo() 
        {
            if (string.IsNullOrEmpty(rtbConnstr.Text))
            {
                MessageBox.Show("连接字符串不能为空", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            string dbType = cmbType.SelectedValue as string;
            if (string.IsNullOrEmpty(dbType))
            {
                MessageBox.Show("请选择数据库类型", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            object tier = cmbTier.SelectedValue;
            if (tier==null)
            {
                MessageBox.Show("请选择架构", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            _info.ConnectionString = rtbConnstr.Text;
            _info.ROConnectionString = rtbReadConnstr.Text;
            _info.DbType = dbType;
            _info.SummaryShow = SummaryItem;
            _info.IsAllDal = chkAllDal.Checked;
            _info.EntityToDirectory = chkEntityToDirectory.Checked;
            _info.Tier = Convert.ToInt32(tier);
            _info.CacheType = cmbCacheType.SelectedValue as string;
            _info.CacheConnString = txtCacheServer.Text;
            _info.IsAllTable = ckbAll.Checked;
            _info.AllowLazy = (LazyType)cmbLazy.SelectedValue;
            return true;
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            try
            {
                if (!FillInfo())
                {
                    return;
                }
                DBInfo db = _info.CreateDBInfo();
                using (DataBaseOperate oper = db.CreateOperate())
                {

                    oper.ConnectDataBase();
                    MessageBox.Show("测试成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("连接错误:" + ex.ToString(), "连接数据库错误", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }

        private void btnModel_Click(object sender, EventArgs e)
        {
            //string dbType = cmbType.SelectedValue as string;
            ComboBoxItem item = cmbType.SelectedItem as ComboBoxItem;
            if (item==null)
            {
                MessageBox.Show("请选择数据库类型", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            ComboBoxItem conn = item.Tag as ComboBoxItem;
            if (!string.IsNullOrEmpty(conn.Text)) 
            {
                rtbConnstr.Text = conn.Text;
            }
        }

        private void cmbCacheType_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBoxItem item = cmbCacheType.SelectedItem as ComboBoxItem;
            if (item != null)
            {
                string connStr = item.Tag as string;
                gpCacheServer.Enabled = !string.IsNullOrEmpty(connStr);
            }
        }

        private void btnCacheModel_Click(object sender, EventArgs e)
        {
            ComboBoxItem item = cmbCacheType.SelectedItem as ComboBoxItem;
            if (item == null)
            {
                return;
            }
            string conn = item.Tag as string;
            if (!string.IsNullOrEmpty(conn))
            {
                txtCacheServer.Text = conn;
            }
        }

        /// <summary>
        /// 显示或隐藏缓存
        /// </summary>
        private void ShowOrHideCache(bool isShow) 
        {
            gpCache.Visible = isShow;
            string mark = null;
            if (isShow)
            {
                mark = "↑";
            }
            else 
            {
                mark = "↓";
            }
            btnCache.Text = "缓存设置" + mark;
            if (isShow)
            {
                this.Height = _maxSize.Height;
            }
            else 
            {
                this.Height = _maxSize.Height - gpCache.Height;
            }

        }

        private void btnCache_Click(object sender, EventArgs e)
        {
            ShowOrHideCache(!gpCache.Visible);
        }

        private void cmbType_SelectedValueChanged(object sender, EventArgs e)
        {
            ComboBoxItem item = cmbType.SelectedItem as ComboBoxItem;
            if (item == null)
            {
                btnImp.Visible = false;
                return;
            }

            ComboBoxItem conn = item.Tag as ComboBoxItem;
            string summary = conn.Value as string;
            if (!string.IsNullOrEmpty(summary))
            {
                btnImp.Visible = true;
            }
            else
            {
                btnImp.Visible = false;
            }
        }

        private void btnImp_Click(object sender, EventArgs e)
        {
            ComboBoxItem item = cmbType.SelectedItem as ComboBoxItem;
            if (item == null)
            {
                
                return;
            }

            ComboBoxItem conn = item.Tag as ComboBoxItem;
            string summary = conn.Value as string;
            if (!string.IsNullOrEmpty(summary))
            {
                FrmCompileResault.ShowCompileResault(summary, null, "注意事项");
            }
            
        }

        private void btnReadModel_Click(object sender, EventArgs e)
        {
            ComboBoxItem item = cmbType.SelectedItem as ComboBoxItem;
            if (item == null)
            {
                MessageBox.Show("请选择数据库类型", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            ComboBoxItem conn = item.Tag as ComboBoxItem;
            if (!string.IsNullOrEmpty(conn.Text))
            {
                rtbReadConnstr.Text = conn.Text;
            }
        }
    }

}