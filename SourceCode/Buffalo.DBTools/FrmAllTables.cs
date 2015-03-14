using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Buffalo.DB.DBCheckers;
using Buffalo.DBTools.HelperKernel;
using Buffalo.DB.DataBaseAdapter;
using EnvDTE;
using Microsoft.VisualStudio.Modeling.Diagrams;
using Buffalo.DBTools.ROMHelper;
using Microsoft.VisualStudio.EnterpriseTools.ClassDesigner;
using Buffalo.DB.CommBase.BusinessBases;
using System.IO;
using System.Xml;
using Buffalo.Win32Kernel.DataGridViewUnit;
using Microsoft.VisualStudio.EnterpriseTools.ArtifactModel.Clr;
using Buffalo.DB.CommBase;

namespace Buffalo.DBTools
{
    public partial class FrmAllTables : Form
    {
        public FrmAllTables()
        {
            InitializeComponent();
        }
        //ClassDesignerDocView _selectDocView;
        ///// <summary>
        ///// 选择的文档
        ///// </summary>
        //public ClassDesignerDocView SelectDocView
        //{
        //    get { return _selectDocView; }
        //    set { _selectDocView = value; }
        //}
        //private Diagram _selectedDiagram = null;

        ///// <summary>
        ///// 选中的关系图
        ///// </summary>
        //public Diagram SelectedDiagram
        //{
        //    get { return _selectedDiagram; }
        //    set { _selectedDiagram = value; }
        //}
        //private Project _currentProject;
        ///// <summary>
        ///// 当前项目
        ///// </summary>
        //public Project CurrentProject
        //{
        //    get { return _currentProject; }
        //    set { _currentProject = value; }
        //}

        private ClassDesignerInfo _designerInfo;

        /// <summary>
        /// 类设计图信息
        /// </summary>
        public ClassDesignerInfo DesignerInfo
        {
            get { return _designerInfo; }
            set { _designerInfo = value; }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        DBConfigInfo _dbInfo;
        /// <summary>
        /// 数据库信息
        /// </summary>
        public DBConfigInfo DbInfo 
        {
            get 
            {
                if (_dbInfo == null) 
                {
                    _dbInfo = FrmDBSetting.GetDBConfigInfo(DesignerInfo, 
                        DesignerInfo.GetNameSpace() + ".DataAccess");
                    

                }
                return _dbInfo;
            }
        }

        /// <summary>
        /// 当前表集合
        /// </summary>
        List<DBTableInfo> _curLst;
        /// <summary>
        /// 查找的表名
        /// </summary>
        string _searchName;
        private void FrmAllTables_Load(object sender, EventArgs e)
        {
            this.Text += ToolVersionInfo.ToolVerInfo;
            gvTables.AutoGenerateColumns = false;
            DBInfo info = DbInfo.CreateDBInfo();
            _curLst =TableChecker.GetAllTables(info);
            gvTables.AllowUserToOrderColumns = true;
            RefreashTablesInfo();
            FillBaseType();
        }

        /// <summary>
        /// 刷新表状态
        /// </summary>
        private void RefreashTablesInfo()
        {
            gvTables.DataSource = null;
            if (_curLst != null && _curLst.Count > 0)
            {

                gvTables.DataSource = FilterTableInfo();
            }
            //RefreashExistsInfo();
            

        }

        /// <summary>
        /// 默认基类
        /// </summary>
        /// <returns></returns>
        private string GetDefaultBaseType()
        {
            string ret = null;

            ret = typeof(EntityBase).FullName;

            if (DbInfo.Tier == 1)
            {
                ret = typeof(ThinModelBase).FullName;
            }
            return ret;
        }

        /// <summary>
        /// 填充基类
        /// </summary>
        private void FillBaseType() 
        {
            List<ClrClass> lstClass=Connect.GetAllClass(DesignerInfo.SelectedDiagram);
            cmbBaseType.Items.Clear();
            cmbBaseType.Items.Add(GetDefaultBaseType());
            foreach (ClrClass ctype in lstClass) 
            {
                string fullName = EntityConfig.GetFullName(ctype);
                cmbBaseType.Items.Add(fullName);
            }
            cmbBaseType.SelectedIndex = 0;
        }


        /// <summary>
        /// 筛选表信息
        /// </summary>
        private BindingCollection<DBTableInfo> FilterTableInfo() 
        {
            ClearTableCheck();
            if (string.IsNullOrEmpty(_searchName) || string.IsNullOrEmpty(_searchName.Trim()))
            {
                return new BindingCollection<DBTableInfo>(_curLst);
            }
            BindingCollection<DBTableInfo> lst = new BindingCollection<DBTableInfo>();
            foreach( DBTableInfo info in _curLst)
            {
                if (info.Name.IndexOf(_searchName,StringComparison.CurrentCultureIgnoreCase) >= 0) 
                {
                    lst.Add(info);
                }
            }
            
            return lst;
        }

        /// <summary>
        /// 清空所有表的选中状态
        /// </summary>
        private void ClearTableCheck() 
        {
            foreach (DBTableInfo info in _curLst)
            {
                info.IsGenerate = false;
            }
        }

        /// <summary>
        /// 填充是否存在的列
        /// </summary>
        private void RefreashExistsInfo() 
        {
            foreach (DataGridViewRow dr in gvTables.Rows) 
            {
                string exists = "未生成";
                DBTableInfo info = dr.DataBoundItem as DBTableInfo;
                if (info != null) 
                {
                    string fileName = DBEntityInfo.GetEntityRealFileName(info, _dbInfo, DesignerInfo);
                    try
                    {
                        if (File.Exists(fileName))
                        {
                            exists = "已生成";
                            dr.DefaultCellStyle.ForeColor = Color.Red;
                        }
                    }
                    catch { }
                }
                dr.Cells["ColExists"].Value = exists;
            }
        }

        void gvTables_RowPrePaint(object sender, System.Windows.Forms.DataGridViewRowPrePaintEventArgs e)
        {
            int row = e.RowIndex;
            if (row < 0)
            {
                return;
            }
            DataGridViewRow dr = gvTables.Rows[row];
            string exists = "未生成";
            DBTableInfo info = dr.DataBoundItem as DBTableInfo;
            if (info != null)
            {
                string fileName = DBEntityInfo.GetEntityRealFileName(info,_dbInfo, DesignerInfo);
                
                try
                {
                    if (File.Exists(fileName))
                    {
                        exists = "已生成";
                        dr.DefaultCellStyle.ForeColor = Color.Red;
                    }
                }
                catch { }
            }
            dr.Cells["ColExists"].Value = exists;
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {

            IEnumerable<DBTableInfo> lst = gvTables.DataSource as IEnumerable<DBTableInfo>;
            if (lst == null) 
            {
                return;
            }
            List<string> selection = new List<string>();
            foreach (DBTableInfo info in lst) 
            {
                if (info.IsGenerate) 
                {
                    selection.Add(info.Name);
                }
            }
            DBInfo db=DbInfo.CreateDBInfo();


            using (BatchAction ba = db.DefaultOperate.StarBatchAction())
            {
                using (FrmProcess frmPro = FrmProcess.ShowProcess())
                {
                    string file = DesignerInfo.SelectDocView.DocData.FileName;
                    XmlDocument doc = DBEntityInfo.GetClassDiagram(file);
                    
                    frmPro.UpdateProgress(0, 10, "正在读取类信息");
                    List<DBTableInfo> lstGen = TableChecker.GetTableInfo(db, selection);
                    string entityNamespace = DesignerInfo.GetNameSpace();
                    for (int i = 0; i < lstGen.Count; i++)
                    {
                        frmPro.UpdateProgress(i, lstGen.Count, "正在生成");
                        string baseType = cmbBaseType.Text;
                        if (string.IsNullOrEmpty(baseType)) 
                        {
                            baseType = GetDefaultBaseType();
                        }
                        DBEntityInfo info = new DBEntityInfo(entityNamespace, lstGen[i], DesignerInfo, DbInfo, baseType);
                        info.GreanCode(doc);
                    }
                    //拷贝备份
                    File.Copy(file, file + ".bak", true);
                    EntityMappingConfig.SaveXML(file, doc);
                }
            }
            this.Close();
        }

       


        /// <summary>
        /// 刷新选中状态
        /// </summary>
        private void RefreashCheckType() 
        {
           
            if (gvTables.Rows.Count>0)
            {
                bool isCheckAll = true;

                foreach (DataGridViewRow row in gvTables.Rows) 
                {
                    DataGridViewCheckBoxCell cell = row.Cells["ColChecked"] as DataGridViewCheckBoxCell;
                    if (cell != null && !(bool)cell.FormattedValue) 
                    {
                        isCheckAll = false;
                        break;
                    }
                }
                chkAll.Checked = isCheckAll;
            }
            
        }


        private void gvTables_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            string colName = gvTables.Columns[e.ColumnIndex].Name;
            if (colName == "ColChecked")
            {
                gvTables.EndEdit();
                RefreashCheckType();
                //if ((bool)gvTables.Rows[e.RowIndex].Cells[e.ColumnIndex].FormattedValue)
                //{
                //    dgTest[colNumber, e.RowIndex].Value = e.RowIndex.ToString() + 9999;
                //}
                
            }
        }


        private void chkAll_MouseUp(object sender, MouseEventArgs e)
        {
            foreach (DataGridViewRow row in gvTables.Rows)
            {
                DataGridViewCheckBoxCell cell = row.Cells["ColChecked"] as DataGridViewCheckBoxCell;
                cell.Value = chkAll.Checked;
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            _searchName = txtSearch.Text;
            RefreashTablesInfo();
        }
    }
}