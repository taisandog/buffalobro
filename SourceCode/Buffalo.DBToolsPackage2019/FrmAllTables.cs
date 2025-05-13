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
using Buffalo.DBTools.UIHelper;
using Buffalo2015.DBToolsPackage;

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
        ///// ѡ����ĵ�
        ///// </summary>
        //public ClassDesignerDocView SelectDocView
        //{
        //    get { return _selectDocView; }
        //    set { _selectDocView = value; }
        //}
        //private Diagram _selectedDiagram = null;

        ///// <summary>
        ///// ѡ�еĹ�ϵͼ
        ///// </summary>
        //public Diagram SelectedDiagram
        //{
        //    get { return _selectedDiagram; }
        //    set { _selectedDiagram = value; }
        //}
        //private Project _currentProject;
        ///// <summary>
        ///// ��ǰ��Ŀ
        ///// </summary>
        //public Project CurrentProject
        //{
        //    get { return _currentProject; }
        //    set { _currentProject = value; }
        //}

        private ClassDesignerInfo _designerInfo;

        /// <summary>
        /// �����ͼ��Ϣ
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
        /// ���ݿ���Ϣ
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
        /// ��ǰ����
        /// </summary>
        List<DBTableInfo> _curLst;
        /// <summary>
        /// ���ҵı���
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
        /// ˢ�±�״̬
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
        /// Ĭ�ϻ���
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
        /// ������
        /// </summary>
        private void FillBaseType() 
        {
            List<ClrClass> lstClass= BuffaloToolCDCommand.GetAllClass(DesignerInfo.SelectedDiagram);
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
        /// ɸѡ����Ϣ
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
        /// ������б��ѡ��״̬
        /// </summary>
        private void ClearTableCheck() 
        {
            foreach (DBTableInfo info in _curLst)
            {
                info.IsGenerate = false;
            }
        }

        /// <summary>
        /// ����Ƿ���ڵ���
        /// </summary>
        private void RefreashExistsInfo() 
        {
            foreach (DataGridViewRow dr in gvTables.Rows) 
            {
                string exists = "δ����";
                DBTableInfo info = dr.DataBoundItem as DBTableInfo;
                if (info != null) 
                {
                    string fileName = DBEntityInfo.GetEntityRealFileName(info, _dbInfo, DesignerInfo);
                    try
                    {
                        if (File.Exists(fileName))
                        {
                            exists = "������";
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
            string exists = "δ����";
            DBTableInfo info = dr.DataBoundItem as DBTableInfo;
            if (info != null)
            {
                string fileName = DBEntityInfo.GetEntityRealFileName(info,_dbInfo, DesignerInfo);
                
                try
                {
                    if (File.Exists(fileName))
                    {
                        exists = "������";
                        dr.DefaultCellStyle.ForeColor = Color.Red;
                    }
                }
                catch { }
            }
            dr.Cells["ColExists"].Value = exists;
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            try
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
                DBInfo db = DbInfo.CreateDBInfo();


                using (BatchAction ba = db.DefaultOperate.StarBatchAction())
                {
                    using (FrmProcess frmPro = FrmProcess.ShowProcess())
                    {
                        string file = DesignerInfo.SelectDocView.DocData.FileName;
                        XmlDocument doc = DBEntityInfo.GetClassDiagram(file);

                        frmPro.UpdateProgress(0, 10, "���ڶ�ȡ����Ϣ");
                        List<DBTableInfo> lstGen = TableChecker.GetTableInfo(db, selection);
                        string entityNamespace = DesignerInfo.GetNameSpace();
                        for (int i = 0; i < lstGen.Count; i++)
                        {
                            frmPro.UpdateProgress(i, lstGen.Count, "��������");
                            string baseType = cmbBaseType.Text;
                            if (string.IsNullOrEmpty(baseType))
                            {
                                baseType = GetDefaultBaseType();
                            }
                            DBEntityInfo info = new DBEntityInfo(entityNamespace, lstGen[i], DesignerInfo, DbInfo, baseType);
                            info.GreanCode(doc);
                        }
                        //��������
                        File.Copy(file, file + ".bak", true);
                        EntityMappingConfig.SaveXML(file, doc);
                    }
                }
                this.Close();
            }
            catch (Exception ex) 
            {
                FrmCompileResault.ShowError(ex.ToString(), ToolVersionInfo.ToolVerInfo);
            }
        }

       


        /// <summary>
        /// ˢ��ѡ��״̬
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