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
using Buffalo.DBTools.UIHelper;

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
            chkLazy.Checked = _config.AllowLazy;
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
            try
            {
                //_config.BaseType = txtBaseClass.Text;
                _config.TableName = txtTableName.Text;
                _config.UseCache = ckbCache.Checked;
                _config.AllowLazy = chkLazy.Checked;
                _config.GenerateCode();
                this.DialogResult = DialogResult.OK;
            }
            catch (Exception ex) 
            {
                FrmCompileResault.ShowCompileResault(null, ex.ToString(), ToolVersionInfo.ToolVerInfo);
            }
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
            DataGridViewRow row = gvField.Rows[rowIndex];

            EntityParamField item= row.DataBoundItem as EntityParamField;
            int len = Convert.ToInt32(row.Cells["ColLength"].Value);
            DbType type = (DbType)EnumUnit.GetEnumInfoByName(typeof(DbType),
                row.Cells["ColParamType"].Value.ConvertTo<string>()).Value;
             bool allowNull=false;
            if(item!=null)
            {
                allowNull = item.AllowNull;
            }
            row.Cells["DBRealType"].Value = _ida.DBTypeToSQL(type, len, allowNull);
        }

        private void labHelp_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            StringBuilder sbInfo = new StringBuilder();
            sbInfo.AppendLine("====属性名===");
            sbInfo.AppendLine("自动生成代码的属性的名字，默认跟变量名对应\r\n");

            sbInfo.AppendLine("====字段名====");
            sbInfo.AppendLine("生成数据库的字段\r\n");

            sbInfo.AppendLine("====长度====");
            sbInfo.AppendLine("生成字段的数据库长度(如果是string 且大于8000，则自动变成Text)\r\n");
            sbInfo.AppendLine("decimal类型数值的长度为：10000部分是整数长度，10000以下部分为小数长度。整数=长度/10000，小数部分=长度%10000\r\n");

            sbInfo.AppendLine("====类型====");
            sbInfo.AppendLine("===此属性的类型:");
            sbInfo.AppendLine("普通:普通属性");
            sbInfo.AppendLine("主键:主键属性，并在数据库设置为主键");
            sbInfo.AppendLine("自增长:自增长属性，并在数据库设置为自增长");
            sbInfo.AppendLine("自增长主键:自增长主键属性，并在数据库设置为主键和自增长");
            sbInfo.AppendLine("版本号:标记此属性为版本号，Update时候使用此属性进行乐观锁更新");
            sbInfo.AppendLine("例如(当此属性名字是ver类型为int时候值为1时候，update时候为 set ver=ver+1 where ver=1)\r\n");

            sbInfo.AppendLine("====默认值====");
            sbInfo.AppendLine("此属性的默认值，不填则没有默认值");
            sbInfo.AppendLine("如果需要默认为空字符串，请填入''");
            sbInfo.AppendLine("===日期默认值函数:");
            sbInfo.AppendLine("当前数据库时间:db_now()");

            FrmCompileResault.ShowCompileResault(sbInfo.ToString(), null,"表格上的填空表示什么?");
        }
    }
}