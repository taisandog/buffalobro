using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Microsoft.VisualStudio.EnterpriseTools.ClassDesigner.PresentationModel;
using Microsoft.VisualStudio.Modeling.Diagrams;
using EnvDTE;
using Microsoft.VisualStudio.EnterpriseTools.ArtifactModel.Clr;
using System.IO;
using Buffalo.DBTools.HelperKernel;
using Microsoft.VisualStudio.EnterpriseTools.ClassDesigner;
using Buffalo.DB.DataBaseAdapter.IDbAdapters;
using Buffalo.Kernel;

namespace Buffalo.DBTools
{
    public partial class FrmClassDesigner : Form
    {
        public FrmClassDesigner()
        {
            InitializeComponent();
        }
        private ClrTypeShape _selectedClass = null;

        /// <summary>
        /// 选中的类
        /// </summary>
        public ClrTypeShape SelectedClass
        {
            get { return _selectedClass; }
            set { _selectedClass = value; }
        }

        private ClassDesignerInfo _designerInfo;

        /// <summary>
        /// 类设计图信息
        /// </summary>
        public ClassDesignerInfo DesignerInfo
        {
            get { return _designerInfo; }
            set { _designerInfo = value; }
        }

        IDBAdapter _ida;
        GridViewComboBoxCell _cmbCell = null;
        GridViewComboBoxCell _relationCell = null;
        EntityConfig _config = null;
        private void FrmClassDesigner_Load(object sender, EventArgs e)
        {
            this.Text += ToolVersionInfo.ToolVerInfo;
            _cmbCell = new GridViewComboBoxCell(gvField);
            _relationCell = new GridViewComboBoxCell(gvMapping);
            gvField.AutoGenerateColumns = false;
            gvMapping.AutoGenerateColumns = false;
            _config = new EntityConfig(SelectedClass.AssociatedType, DesignerInfo);
            //_config.SelectDocView = SelectDocView;
            _config.InitDBConfig();
            _ida = _config.DbInfo.CreateDBInfo().CurrentDbAdapter;
            BindFieldInfos();
            //_cmbCell.SetDataSource(EntityFieldBase.GetAllSupportTypes());
            gvMapping.CurrentCellChanged += new EventHandler(gvMapping_CurrentCellChanged);
            gvField.CurrentCellChanged+=new EventHandler(gvField_CurrentCellChanged);
        }

        void gvMapping_CurrentCellChanged(object sender, EventArgs e)
        {
            if (gvMapping.CurrentCell == null) 
            {
                return;
            }
            string colName = gvMapping.Columns[gvMapping.CurrentCell.ColumnIndex].Name;
            if (colName == "ColSource") 
            {
                _relationCell.SetDataSource(_config.AllPropertyNames);
                _relationCell.ShowComboBox(gvMapping.CurrentCell);
            }
            else if (colName == "ColTarget")
            {
                EntityRelationItem erf = gvMapping.Rows[gvMapping.CurrentCell.RowIndex].DataBoundItem as EntityRelationItem;
                if (erf != null)
                {
                    _relationCell.SetDataSource(erf.TargetPropertyList);
                    _relationCell.ShowComboBox(gvMapping.CurrentCell);
                }
            }
        }

        private void BindFieldInfos()
        {
            txtClassName.Text = _config.ClassName;
            txtTableName.Text = _config.TableName;
            ckbCache.Checked = _config.UseCache;
            txtBaseClass.Text = _config.BaseTypeName;

            gvField.DataSource = _config.EParamFields;
            gvMapping.DataSource = _config.ERelation;

            for (int i = 0; i < gvField.Rows.Count; i++)
            {
                FillDBRealType(i);
            }
        }

        private void gvField_CurrentCellChanged(object sender, EventArgs e)
        {
            if (gvField.CurrentCell == null)
            {
                return;
            }
            string colName=gvField.Columns[gvField.CurrentCell.ColumnIndex].Name;
            if (colName == "ColParamType") 
            {
                EntityParamField epf=gvField.Rows[gvField.CurrentCell.RowIndex].DataBoundItem as EntityParamField;
                if (epf != null)
                {
                    DataTypeInfos info = EntityFieldBase.GetTypeInfo(epf.FInfo);
                    if (info != null)
                    {
                        _cmbCell.SetDataSource(info.DbTypes);
                        _cmbCell.ShowComboBox(gvField.CurrentCell);

                    }
                    
                }
            }

            if (colName == "ColPropertyType")
            {
                EntityParamField epf = gvField.Rows[gvField.CurrentCell.RowIndex].DataBoundItem as EntityParamField;
                if (epf != null)
                {
                    DataTypeInfos info = EntityFieldBase.GetTypeInfo(epf.FInfo);
                    if (info != null)
                    {
                        _cmbCell.SetDataSource(EntityFieldBase.PropertyTypeItems);
                        _cmbCell.ShowComboBox(gvField.CurrentCell);
                    }

                }
            }
        }

        private void btnGenCode_Click(object sender, EventArgs e)
        {
            //_config.BaseType = txtBaseClass.Text;
            _config.TableName = txtTableName.Text;
            _config.UseCache = ckbCache.Checked;
            _config.GenerateCode();
            this.DialogResult = DialogResult.OK;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void gvField_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            
        }

        private void gvField_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
            {
                return;
            }

            string colName = gvField.Columns[e.ColumnIndex].Name;
            if (colName.Equals("ColParamType", StringComparison.CurrentCultureIgnoreCase) ||
                colName.Equals("ColLength", StringComparison.CurrentCultureIgnoreCase))
            {
                FillDBRealType(e.RowIndex);
            }
        }

        /// <summary>
        /// 填充数据层实际类型
        /// </summary>
        /// <param name="rowIndex"></param>
        private void FillDBRealType(int rowIndex) 
        {
            int len = Convert.ToInt32(gvField.Rows[rowIndex].Cells["ColLength"].Value);
            DbType type = (DbType)EnumUnit.GetEnumInfoByName(typeof(DbType),
                gvField.Rows[rowIndex].Cells["ColParamType"].Value.ToString()).Value;

            gvField.Rows[rowIndex].Cells["DBRealType"].Value = _ida.DBTypeToSQL(type, len);
        }
        

    }
}